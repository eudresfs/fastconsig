import { Injectable, Logger } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import Redis from 'ioredis';
import { AuditTrailService } from './audit-trail.service';
import { RATE_LIMITS } from '@fast-consig/shared';
import { ContextService } from '../../core/context/context.service';

@Injectable()
export class SecurityService {
  private readonly logger = new Logger(SecurityService.name);
  private readonly redis: Redis;

  constructor(
    private readonly configService: ConfigService,
    private readonly auditTrailService: AuditTrailService,
    private readonly contextService: ContextService,
  ) {
    const redisUrl = this.configService.get<string>('REDIS_URL', 'redis://localhost:6379');
    this.redis = new Redis(redisUrl);
  }

  /**
   * Checks for anomaly patterns in margin queries.
   * Detects:
   * 1. Rapid queries for the same CPF (Anomaly Rate Limit)
   * 2. Distributed attacks (Same CPF queried by multiple IPs)
   *
   * @param targetCpf The CPF being queried
   * @param ip The source IP address
   */
  async checkMarginQueryAnomaly(targetCpf: string, ip: string): Promise<void> {
    const tenantId = this.contextService.getTenantId() || 'system';
    const userId = this.contextService.getUserId() || 'anonymous';

    // 1. Check Anomaly Rate Limit (Queries per CPF per minute)
    // Key: anomaly:cpf:{cpf}:count
    const cpfRateKey = `anomaly:cpf:${targetCpf}:count`;
    const currentCount = await this.redis.incr(cpfRateKey);

    if (currentCount === 1) {
      await this.redis.expire(cpfRateKey, RATE_LIMITS.ANOMALY.TTL / 1000);
    }

    if (currentCount > RATE_LIMITS.ANOMALY.LIMIT) {
      await this.handleAnomaly(
        'HIGH_FREQUENCY_QUERY',
        `Excessive queries for CPF ${targetCpf} (${currentCount} in window)`,
        { targetCpf, count: currentCount, ip }
      );
      // We block here by throwing an exception usually, or return a signal.
      // For now, let's assume we throw to block the request.
      throw new Error('Security Anomaly Detected: Request blocked due to suspicious activity.');
    }

    // 2. Check Distributed Attack (Same CPF, Multiple IPs)
    // Key: anomaly:cpf:{cpf}:ips (Set of IPs)
    const cpfIpsKey = `anomaly:cpf:${targetCpf}:ips`;
    await this.redis.sadd(cpfIpsKey, ip);
    await this.redis.expire(cpfIpsKey, RATE_LIMITS.ANOMALY.TTL / 1000); // Window same as rate limit for simplicity

    const uniqueIps = await this.redis.scard(cpfIpsKey);
    const DISTRIBUTED_THRESHOLD = 3; // More than 3 distinct IPs querying same CPF

    if (uniqueIps >= DISTRIBUTED_THRESHOLD) {
       await this.handleAnomaly(
        'DISTRIBUTED_ATTACK',
        `Distributed query attack on CPF ${targetCpf} from ${uniqueIps} IPs`,
        { targetCpf, uniqueIps, lastIp: ip }
      );
       // We might not block THIS specific request if it's the first time seeing THIS IP,
       // but strictly we should lock down the target.
       throw new Error('Security Anomaly Detected: Distributed attack pattern.');
    }
  }

  private async handleAnomaly(type: string, description: string, metadata: any) {
    this.logger.warn(`SECURITY ALERT [${type}]: ${description}`);

    // Log to Audit Trail with HIGH severity (using metadata to indicate severity if schema allows, or just type)
    await this.auditTrailService.log({
      tenantId: this.contextService.getTenantId() || 'system',
      userId: this.contextService.getUserId() || 'system',
      action: 'SECURITY_ALERT',
      resourceType: 'security_engine',
      resourceId: metadata.targetCpf,
      ipAddress: metadata.ip || 'system',
      details: description, // Assuming 'details' field exists or mapping to metadata
      metadata: { ...metadata, severity: 'HIGH', type },
    } as any);

    // TODO: Trigger external alert (Email/Slack) via MailService
  }
}
