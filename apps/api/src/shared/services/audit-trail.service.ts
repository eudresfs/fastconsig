import { Injectable } from '@nestjs/common';
import { db, auditTrails } from '@fast-consig/database';
import { and, eq, gte, lte, desc } from 'drizzle-orm';
import { createHash } from 'crypto';

/**
 * Action types for audit logging
 */
export type AuditAction =
  | 'CREATE'
  | 'UPDATE'
  | 'DELETE'
  | 'QUERY_MARGIN'
  | 'QUERY_EMPLOYEE'
  | 'LOGIN'
  | 'LOGOUT'
  | 'TOKEN_GENERATE'
  | 'TOKEN_VALIDATE'
  | 'READ'
  | 'SECURITY_ALERT'; // Added for Story 1.6

/**
 * Parameters for logging an audit trail entry
 */
export interface AuditLogParams {
  tenantId: string;
  userId: string;
  action: AuditAction;
  resourceType: string;
  resourceId?: string;
  ipAddress: string;
  userAgent?: string;
  oldValue?: Record<string, unknown>;
  newValue?: Record<string, unknown>;
  metadata?: Record<string, unknown>;
}

/**
 * Filters for querying audit trail
 */
export interface AuditTrailFilters {
  tenantId: string;
  userId?: string;
  resourceType?: string;
  resourceId?: string;
  startDate?: Date;
  endDate?: Date;
  limit?: number;
  offset?: number;
  anonymize?: boolean; // LGPD compliance (FR16)
}

/**
 * Audit Trail Service - Compliance and Security Logging
 *
 * Implements FR15, FR16, NFR05 from PRD.
 * Records all data mutations and sensitive read operations.
 *
 * @see ADR-006 (Audit & Observability) - Append-only audit trail
 * @see ADR-002 (Multi-Tenancy) - Tenant-scoped queries with RLS
 */
@Injectable()
export class AuditTrailService {
  /**
   * Log an audit trail entry
   *
   * @param params - Audit log parameters
   * @throws Never - Failures are logged but don't propagate to prevent blocking operations
   */
  async log(params: AuditLogParams): Promise<void> {
    try {
      // Sanitize sensitive data before logging
      const sanitizedOldValue = params.oldValue
        ? this.sanitizeForAudit(params.oldValue)
        : undefined;
      const sanitizedNewValue = params.newValue
        ? this.sanitizeForAudit(params.newValue)
        : undefined;

      // Insert audit trail entry
      await db.insert(auditTrails).values({
        tenantId: params.tenantId,
        userId: params.userId,
        action: params.action,
        resourceType: params.resourceType,
        resourceId: params.resourceId,
        ipAddress: params.ipAddress,
        userAgent: params.userAgent,
        oldValue: sanitizedOldValue,
        newValue: sanitizedNewValue,
        metadata: params.metadata,
      });
    } catch (error) {
      // CRITICAL: Audit failures must NOT block business operations
      // Log error but don't throw - graceful degradation
      console.error('[AuditTrailService] Failed to log audit entry:', {
        error: error instanceof Error ? error.message : String(error),
        params: {
          ...params,
          oldValue: '[REDACTED]',
          newValue: '[REDACTED]',
        },
      });

      // TODO: Implement observability
      // - Emit metric to monitoring system (Prometheus/DataDog)
      // - Send alert if failure rate > 1%
      // - Consider fallback: queue to Redis if DB unavailable
    }
  }

  /**
   * Query audit trail with filters
   *
   * @param filters - Query filters (tenant-scoped)
   * @returns Array of audit trail entries
   */
  async getAuditTrail(filters: AuditTrailFilters) {
    const conditions = [eq(auditTrails.tenantId, filters.tenantId)];

    // Apply optional filters
    if (filters.userId) {
      conditions.push(eq(auditTrails.userId, filters.userId));
    }
    if (filters.resourceType) {
      conditions.push(eq(auditTrails.resourceType, filters.resourceType));
    }
    if (filters.resourceId) {
      conditions.push(eq(auditTrails.resourceId, filters.resourceId));
    }
    if (filters.startDate) {
      conditions.push(gte(auditTrails.createdAt, filters.startDate));
    }
    if (filters.endDate) {
      conditions.push(lte(auditTrails.createdAt, filters.endDate));
    }

    // Query with pagination
    const results = await db
      .select()
      .from(auditTrails)
      .where(and(...conditions))
      .orderBy(desc(auditTrails.createdAt))
      .limit(filters.limit ?? 100)
      .offset(filters.offset ?? 0);

    // LGPD Compliance: Anonymize if requested (FR16)
    if (filters.anonymize) {
      return results.map((entry) => ({
        ...entry,
        userId: this.hashUserId(entry.userId), // Hash instead of showing real ID
        ipAddress: '[ANONYMIZED]', // Remove IP for privacy
        userAgent: undefined, // Remove user agent
      }));
    }

    return results;
  }

  /**
   * Sanitize sensitive data before storing in audit log
   *
   * Removes passwords, tokens, API keys, and other secrets
   *
   * @param obj - Object to sanitize
   * @returns Sanitized object
   */
  private sanitizeForAudit(
    obj: Record<string, unknown>
  ): Record<string, unknown> {
    const sanitized = { ...obj };
    const sensitiveFields = [
      'password',
      'token',
      'secret',
      'apiKey',
      'api_key',
      'accessToken',
      'access_token',
      'refreshToken',
      'refresh_token',
      'privateKey',
      'private_key',
    ];

    // Recursively sanitize nested objects
    for (const [key, value] of Object.entries(sanitized)) {
      // Redact sensitive field values
      if (sensitiveFields.some((field) => key.toLowerCase().includes(field))) {
        sanitized[key] = '[REDACTED]';
      }
      // Recursively sanitize nested objects
      else if (value && typeof value === 'object' && !Array.isArray(value)) {
        sanitized[key] = this.sanitizeForAudit(
          value as Record<string, unknown>
        );
      }
    }

    return sanitized;
  }

  /**
   * Hash user ID for anonymized queries (LGPD compliance)
   *
   * Uses SHA-256 for proper anonymization (irreversible)
   *
   * @param userId - User ID to hash
   * @returns Hashed user ID
   */
  private hashUserId(userId: string): string {
    const hash = createHash('sha256').update(userId).digest('hex');
    return `user_${hash.substring(0, 16)}`;
  }
}
