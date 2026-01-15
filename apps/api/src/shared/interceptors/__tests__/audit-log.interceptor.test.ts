import { Test, TestingModule } from '@nestjs/testing';
import { AuditLogInterceptor } from '../audit-log.interceptor';
import { AuditTrailService } from '../../services/audit-trail.service';
import { ExecutionContext, CallHandler } from '@nestjs/common';
import { of, throwError } from 'rxjs';

describe('AuditLogInterceptor', () => {
  let interceptor: AuditLogInterceptor;
  let auditTrailService: jest.Mocked<AuditTrailService>;

  beforeEach(async () => {
    // Mock AuditTrailService
    const mockAuditTrailService = {
      log: jest.fn().mockResolvedValue(undefined),
    };

    const module: TestingModule = await Test.createTestingModule({
      providers: [
        AuditLogInterceptor,
        {
          provide: AuditTrailService,
          useValue: mockAuditTrailService,
        },
      ],
    }).compile();

    interceptor = module.get<AuditLogInterceptor>(AuditLogInterceptor);
    auditTrailService = module.get(AuditTrailService) as jest.Mocked<AuditTrailService>;
  });

  it('should be defined', () => {
    expect(interceptor).toBeDefined();
  });

  describe('intercept', () => {
    it('should log audit trail for successful POST request', (done) => {
      const mockContext = createMockContext('POST', '/api/employees', {
        tenantId: 'tenant_123',
        user: { id: 'user_456' },
        ip: '192.168.1.100',
        headers: { 'user-agent': 'Mozilla/5.0' },
        body: { name: 'João Silva' },
      });

      const mockCallHandler: CallHandler = {
        handle: () => of({ id: 'emp_789', name: 'João Silva' }),
      };

      interceptor.intercept(mockContext, mockCallHandler).subscribe({
        complete: () => {
          expect(auditTrailService.log).toHaveBeenCalledWith(
            expect.objectContaining({
              tenantId: 'tenant_123',
              userId: 'user_456',
              action: 'CREATE',
              resourceType: 'employees',
              ipAddress: '192.168.1.100',
              userAgent: 'Mozilla/5.0',
            })
          );
          done();
        },
      });
    });

    it('should log audit trail for UPDATE request', (done) => {
      const mockContext = createMockContext('PUT', '/api/employees/emp_789', {
        tenantId: 'tenant_123',
        user: { id: 'user_456' },
        ip: '192.168.1.100',
        headers: {},
        body: { name: 'João Silva Updated' },
      });

      const mockCallHandler: CallHandler = {
        handle: () => of({ id: 'emp_789', name: 'João Silva Updated' }),
      };

      interceptor.intercept(mockContext, mockCallHandler).subscribe({
        complete: () => {
          expect(auditTrailService.log).toHaveBeenCalledWith(
            expect.objectContaining({
              action: 'UPDATE',
              resourceType: 'employees',
              resourceId: 'emp_789',
            })
          );
          done();
        },
      });
    });

    it('should handle x-forwarded-for header for proxy scenarios', (done) => {
      const mockContext = createMockContext('POST', '/api/employees', {
        tenantId: 'tenant_123',
        user: { id: 'user_456' },
        ip: '10.0.0.1', // Proxy IP
        headers: {
          'x-forwarded-for': '203.0.113.1, 10.0.0.1', // Real client IP first
        },
        body: {},
      });

      const mockCallHandler: CallHandler = {
        handle: () => of({}),
      };

      interceptor.intercept(mockContext, mockCallHandler).subscribe({
        complete: () => {
          expect(auditTrailService.log).toHaveBeenCalledWith(
            expect.objectContaining({
              ipAddress: '203.0.113.1', // Should use real client IP
            })
          );
          done();
        },
      });
    });

    it('should NOT log if no tenant context (unauthenticated)', (done) => {
      const mockContext = createMockContext('GET', '/api/health', {
        // No tenantId, no user
        ip: '192.168.1.100',
        headers: {},
      });

      const mockCallHandler: CallHandler = {
        handle: () => of({ status: 'ok' }),
      };

      interceptor.intercept(mockContext, mockCallHandler).subscribe({
        complete: () => {
          expect(auditTrailService.log).not.toHaveBeenCalled();
          done();
        },
      });
    });

    it('should log failed requests with error details', (done) => {
      const mockContext = createMockContext('POST', '/api/employees', {
        tenantId: 'tenant_123',
        user: { id: 'user_456' },
        ip: '192.168.1.100',
        headers: {},
        body: {},
      });

      const mockError = {
        message: 'Validation failed',
        status: 400,
      };

      const mockCallHandler: CallHandler = {
        handle: () => throwError(() => mockError),
      };

      interceptor.intercept(mockContext, mockCallHandler).subscribe({
        error: () => {
          expect(auditTrailService.log).toHaveBeenCalledWith(
            expect.objectContaining({
              metadata: expect.objectContaining({
                error: 'Validation failed',
                statusCode: 400,
              }),
            })
          );
          done();
        },
      });
    });

    it('should extract resource type from tRPC URLs', (done) => {
      const mockContext = createMockContext('POST', '/trpc/employees.create', {
        tenantId: 'tenant_123',
        user: { id: 'user_456' },
        ip: '192.168.1.100',
        headers: {},
        body: {},
      });

      const mockCallHandler: CallHandler = {
        handle: () => of({ id: 'emp_789' }),
      };

      interceptor.intercept(mockContext, mockCallHandler).subscribe({
        complete: () => {
          expect(auditTrailService.log).toHaveBeenCalledWith(
            expect.objectContaining({
              resourceType: 'employees', // Extracted from tRPC path
            })
          );
          done();
        },
      });
    });
  });
});

// Helper to create mock ExecutionContext
function createMockContext(
  method: string,
  url: string,
  requestData: any
): ExecutionContext {
  const request = {
    method,
    url,
    ...requestData,
  };

  return {
    switchToHttp: () => ({
      getRequest: () => request,
      getResponse: () => ({}),
    }),
  } as ExecutionContext;
}
