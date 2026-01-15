import { Test, TestingModule } from '@nestjs/testing';
import { SecurityService } from '../security.service';
import { ConfigService } from '@nestjs/config';
import { AuditTrailService } from '../audit-trail.service';
import { ContextService } from '../../../core/context/context.service';
import Redis from 'ioredis';
import { RATE_LIMITS } from '@fast-consig/shared';

// Mock IORedis
jest.mock('ioredis');

describe('SecurityService', () => {
  let service: SecurityService;
  let auditTrailService: AuditTrailService;
  let redisMock: any;

  beforeEach(async () => {
    // Reset mocks
    redisMock = {
      incr: jest.fn(),
      expire: jest.fn(),
      sadd: jest.fn(),
      scard: jest.fn(),
      disconnect: jest.fn(),
    };
    (Redis as unknown as jest.Mock).mockReturnValue(redisMock);

    const module: TestingModule = await Test.createTestingModule({
      providers: [
        SecurityService,
        {
          provide: ConfigService,
          useValue: { get: jest.fn().mockReturnValue('redis://localhost:6379') },
        },
        {
          provide: AuditTrailService,
          useValue: { log: jest.fn() },
        },
        {
          provide: ContextService,
          useValue: {
            getTenantId: jest.fn().mockReturnValue('tenant_1'),
            getUserId: jest.fn().mockReturnValue('user_1'),
          },
        },
      ],
    }).compile();

    service = module.get<SecurityService>(SecurityService);
    auditTrailService = module.get<AuditTrailService>(AuditTrailService);
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('checkMarginQueryAnomaly', () => {
    it('should allow normal query frequency', async () => {
      redisMock.incr.mockResolvedValue(1); // 1st request
      redisMock.scard.mockResolvedValue(1); // 1 IP

      await expect(service.checkMarginQueryAnomaly('12345678900', '127.0.0.1')).resolves.not.toThrow();
    });

    it('should block excessive queries (High Frequency)', async () => {
      // Mock counter exceeding limit
      redisMock.incr.mockResolvedValue(RATE_LIMITS.ANOMALY.LIMIT + 1);

      await expect(service.checkMarginQueryAnomaly('12345678900', '127.0.0.1'))
        .rejects.toThrow('Security Anomaly Detected');

      // Verify audit log
      expect(auditTrailService.log).toHaveBeenCalledWith(expect.objectContaining({
        action: 'SECURITY_ALERT',
        metadata: expect.objectContaining({
          type: 'HIGH_FREQUENCY_QUERY',
          severity: 'HIGH',
        }),
      }));
    });

    it('should block distributed attack (Same CPF, Multiple IPs)', async () => {
      redisMock.incr.mockResolvedValue(1); // Frequency OK
      redisMock.scard.mockResolvedValue(4); // 4 IPs (Threshold is 3)

      await expect(service.checkMarginQueryAnomaly('12345678900', '127.0.0.2'))
        .rejects.toThrow('Security Anomaly Detected');

      // Verify audit log
      expect(auditTrailService.log).toHaveBeenCalledWith(expect.objectContaining({
        action: 'SECURITY_ALERT',
        metadata: expect.objectContaining({
          type: 'DISTRIBUTED_ATTACK',
          severity: 'HIGH',
        }),
      }));
    });
  });
});
