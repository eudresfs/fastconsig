import {
  Injectable,
  NestInterceptor,
  ExecutionContext,
  CallHandler,
} from '@nestjs/common';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { AuditTrailService, type AuditAction } from '../services/audit-trail.service';

/**
 * Audit Log Interceptor - Automatic audit trail capture
 *
 * Captures all HTTP requests (tRPC or REST) and logs them to audit trail.
 * Implements FR15 from PRD - automatic logging of all mutations.
 *
 * @see ADR-006 (Audit & Observability) - Application-level audit events
 */
@Injectable()
export class AuditLogInterceptor implements NestInterceptor {
  constructor(private readonly auditTrailService: AuditTrailService) {}

  intercept(context: ExecutionContext, next: CallHandler): Observable<any> {
    const request = context.switchToHttp().getRequest();
    const { method, url, user, body, headers } = request;

    // Extract context from request (set by Context Middleware)
    const tenantId = request.tenantId || request.user?.organizationId;
    const userId = user?.id || 'anonymous';

    // Get IP address (handle proxy scenarios - NFR requirement)
    const ipAddress = this.extractIpAddress(request);
    const userAgent = headers['user-agent'];

    // Capture old value for UPDATE/DELETE operations (if available)
    // Note: This is simplified - full implementation would fetch from DB
    const oldValue = this.captureOldValue(request);

    // Continue with request and log after completion
    return next.handle().pipe(
      tap({
        next: (response) => {
          // Only log if we have tenant context (skip unauthenticated requests)
          if (tenantId && userId) {
            // Extract action, resource type, and resource ID
            const action = this.mapMethodToAction(method);
            const resourceType = this.extractResourceType(url);
            const resourceId = this.extractResourceId(url, body, response);

            // Log to audit trail (async, non-blocking)
            this.auditTrailService.log({
              tenantId,
              userId,
              action,
              resourceType,
              resourceId,
              ipAddress,
              userAgent,
              oldValue,
              newValue: this.extractNewValue(method, body, response),
              metadata: {
                url,
                method,
                statusCode: response?.statusCode || 200,
              },
            });
          }
        },
        error: (error) => {
          // Log failed operations too (for security audit)
          if (tenantId && userId) {
            this.auditTrailService.log({
              tenantId,
              userId,
              action: this.mapMethodToAction(method),
              resourceType: this.extractResourceType(url),
              ipAddress,
              userAgent,
              metadata: {
                url,
                method,
                error: error.message,
                statusCode: error.status || 500,
              },
            });
          }
        },
      })
    );
  }

  /**
   * Map HTTP method to audit action type
   */
  private mapMethodToAction(method: string): AuditAction {
    const mapping: Record<string, AuditAction> = {
      POST: 'CREATE',
      PUT: 'UPDATE',
      PATCH: 'UPDATE',
      DELETE: 'DELETE',
      GET: 'READ', // For sensitive reads
    };
    return mapping[method] || 'READ';
  }

  /**
   * Extract resource type from URL
   * Handles both tRPC and REST endpoints
   */
  private extractResourceType(url: string): string {
    try {
      // tRPC format: /trpc/employees.create
      if (url.includes('/trpc/')) {
        const match = url.match(/\/trpc\/([^.]+)\./);
        return match ? match[1] : 'unknown';
      }

      // REST format: /api/employees/123
      const match = url.match(/\/api\/([^/]+)/);
      return match ? match[1] : 'unknown';
    } catch {
      return 'unknown';
    }
  }

  /**
   * Extract resource ID from URL, body, or response
   */
  private extractResourceId(
    url: string,
    body: any,
    response: any
  ): string | undefined {
    try {
      // Try response first (for CREATE operations)
      if (response?.id) {
        return response.id;
      }

      // Try body (for UPDATE/DELETE)
      if (body?.id) {
        return body.id;
      }

      // Try URL path parameter
      const match = url.match(/\/([a-zA-Z0-9_-]{10,30})(?:\/|$|\?)/);
      return match ? match[1] : undefined;
    } catch {
      return undefined;
    }
  }

  /**
   * Extract IP address from request, handling proxy scenarios
   * Uses x-forwarded-for header if behind proxy (ADR-006 requirement)
   */
  private extractIpAddress(request: any): string {
    // Check for proxy headers first
    const forwardedFor = request.headers['x-forwarded-for'];
    if (forwardedFor) {
      // Take first IP if multiple (client IP)
      return forwardedFor.split(',')[0].trim();
    }

    // Fallback to direct connection IP
    return request.ip || request.connection?.remoteAddress || 'unknown';
  }

  /**
   * Capture old value for UPDATE/DELETE operations
   * Stores the request body as "old value" for UPDATE/DELETE
   *
   * Note: Full implementation would fetch current state from DB before mutation
   * For now, we store what the client sent as context
   */
  private captureOldValue(request: any): Record<string, unknown> | undefined {
    const method = request.method;

    // For UPDATE/DELETE, capture the resource ID and any query params as context
    if (method === 'PUT' || method === 'PATCH' || method === 'DELETE') {
      return {
        resourceId: this.extractResourceId(request.url, request.body, null),
        queryParams: request.query || {},
        timestamp: new Date().toISOString(),
      };
    }

    return undefined;
  }

  /**
   * Extract new value from request/response
   */
  private extractNewValue(
    method: string,
    body: any,
    response: any
  ): Record<string, unknown> | undefined {
    if (method === 'POST' || method === 'PUT' || method === 'PATCH') {
      // For mutations, log the response data
      return response?.data || response || body;
    }
    return undefined;
  }
}
