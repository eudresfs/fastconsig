import { Module, Global } from '@nestjs/common';
import { AuditTrailService } from './services/audit-trail.service';
import { AuditLogInterceptor } from './interceptors/audit-log.interceptor';

/**
 * Audit Module - Global module for audit trail functionality
 *
 * Provides audit trail service and interceptor to all modules.
 * Marked as @Global() to avoid repeated imports.
 *
 * @see ADR-006 (Audit & Observability) - Audit infrastructure
 */
@Global()
@Module({
  providers: [AuditTrailService, AuditLogInterceptor],
  exports: [AuditTrailService, AuditLogInterceptor],
})
export class AuditModule {}
