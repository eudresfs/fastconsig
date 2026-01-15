import { Test, TestingModule } from '@nestjs/testing';
import { AuditTrailService } from '../audit-trail.service';
import { db } from '@fast-consig/database';
import { auditTrails } from '@fast-consig/database/schema';

// Mock Drizzle DB
jest.mock('@fast-consig/database', () => ({
  db: {
    insert: jest.fn().mockReturnThis(),
    values: jest.fn().mockResolvedValue(undefined),
    select: jest.fn().mockReturnThis(),
    from: jest.fn().mockReturnThis(),
    where: jest.fn().mockReturnThis(),
    orderBy: jest.fn().mockReturnThis(),
    limit: jest.fn().mockReturnThis(),
    offset: jest.fn().mockResolvedValue([]),
  },
}));

describe('AuditTrailService', () => {
  let service: AuditTrailService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [AuditTrailService],
    }).compile();

    service = module.get<AuditTrailService>(AuditTrailService);

    // Clear all mocks
    jest.clearAllMocks();
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('log', () => {
    it('should insert audit trail entry with all required fields', async () => {
      const params = {
        tenantId: 'tenant_123',
        userId: 'user_456',
        action: 'CREATE' as const,
        resourceType: 'employees',
        resourceId: 'emp_789',
        ipAddress: '192.168.1.100',
        userAgent: 'Mozilla/5.0',
        newValue: { name: 'João Silva', cpf: '12345678900' },
      };

      await service.log(params);

      expect(db.insert).toHaveBeenCalledWith(auditTrails);
      expect(db.insert(auditTrails).values).toHaveBeenCalledWith(
        expect.objectContaining({
          tenantId: params.tenantId,
          userId: params.userId,
          action: params.action,
          resourceType: params.resourceType,
          resourceId: params.resourceId,
          ipAddress: params.ipAddress,
          userAgent: params.userAgent,
          newValue: params.newValue,
        })
      );
    });

    it('should sanitize sensitive data (password, token)', async () => {
      const params = {
        tenantId: 'tenant_123',
        userId: 'user_456',
        action: 'CREATE' as const,
        resourceType: 'users',
        ipAddress: '192.168.1.100',
        newValue: {
          email: 'test@example.com',
          password: 'secret123',
          token: 'jwt_token_here',
        },
      };

      await service.log(params);

      const callArgs = (db.insert(auditTrails).values as jest.Mock).mock
        .calls[0][0];
      expect(callArgs.newValue).toEqual({
        email: 'test@example.com',
        password: '[REDACTED]',
        token: '[REDACTED]',
      });
    });

    it('should not throw error if DB insert fails (graceful degradation)', async () => {
      // Mock DB error
      (db.insert(auditTrails).values as jest.Mock).mockRejectedValueOnce(
        new Error('DB connection failed')
      );

      const params = {
        tenantId: 'tenant_123',
        userId: 'user_456',
        action: 'CREATE' as const,
        resourceType: 'employees',
        ipAddress: '192.168.1.100',
      };

      // Should NOT throw
      await expect(service.log(params)).resolves.not.toThrow();
    });

    it('should handle missing optional fields', async () => {
      const params = {
        tenantId: 'tenant_123',
        userId: 'user_456',
        action: 'DELETE' as const,
        resourceType: 'employees',
        ipAddress: '192.168.1.100',
        // resourceId, userAgent, oldValue, newValue are optional
      };

      await service.log(params);

      const callArgs = (db.insert(auditTrails).values as jest.Mock).mock
        .calls[0][0];
      expect(callArgs.resourceId).toBeUndefined();
      expect(callArgs.userAgent).toBeUndefined();
      expect(callArgs.oldValue).toBeUndefined();
      expect(callArgs.newValue).toBeUndefined();
    });
  });

  describe('getAuditTrail', () => {
    it('should query audit trail with tenant filter', async () => {
      const filters = {
        tenantId: 'tenant_123',
        limit: 50,
        offset: 0,
      };

      await service.getAuditTrail(filters);

      expect(db.select).toHaveBeenCalled();
      expect(db.select().from).toHaveBeenCalledWith(auditTrails);
    });

    it('should apply optional filters (userId, resourceType)', async () => {
      const filters = {
        tenantId: 'tenant_123',
        userId: 'user_456',
        resourceType: 'employees',
      };

      await service.getAuditTrail(filters);

      expect(db.select().from(auditTrails).where).toHaveBeenCalled();
    });

    it('should anonymize data when anonymize=true (LGPD)', async () => {
      // Mock DB result
      const mockResults = [
        {
          id: 'audit_1',
          tenantId: 'tenant_123',
          userId: 'user_real_id_456',
          action: 'CREATE',
          resourceType: 'employees',
          ipAddress: '192.168.1.100',
          userAgent: 'Mozilla/5.0',
          createdAt: new Date(),
        },
      ];

      (
        db
          .select()
          .from(auditTrails)
          .where({} as any)
          .orderBy({} as any)
          .limit(100)
          .offset as jest.Mock
      ).mockResolvedValueOnce(mockResults);

      const filters = {
        tenantId: 'tenant_123',
        anonymize: true,
      };

      const results = await service.getAuditTrail(filters);

      expect(results[0].userId).not.toBe('user_real_id_456');
      expect(results[0].userId).toContain('user_'); // Hashed
      expect(results[0].ipAddress).toBe('[ANONYMIZED]');
      expect(results[0].userAgent).toBeUndefined();
    });

    it('should apply pagination (limit, offset)', async () => {
      const filters = {
        tenantId: 'tenant_123',
        limit: 20,
        offset: 40,
      };

      await service.getAuditTrail(filters);

      expect(
        db.select().from(auditTrails).where({} as any).orderBy({} as any).limit
      ).toHaveBeenCalledWith(20);
      expect(
        db
          .select()
          .from(auditTrails)
          .where({} as any)
          .orderBy({} as any)
          .limit(20).offset
      ).toHaveBeenCalledWith(40);
    });

    it('should default to limit=100, offset=0 when not specified', async () => {
      const filters = {
        tenantId: 'tenant_123',
      };

      await service.getAuditTrail(filters);

      expect(
        db.select().from(auditTrails).where({} as any).orderBy({} as any).limit
      ).toHaveBeenCalledWith(100);
      expect(
        db
          .select()
          .from(auditTrails)
          .where({} as any)
          .orderBy({} as any)
          .limit(100).offset
      ).toHaveBeenCalledWith(0);
    });
  });

  describe('sanitizeForAudit', () => {
    it('should redact nested sensitive fields', async () => {
      const params = {
        tenantId: 'tenant_123',
        userId: 'user_456',
        action: 'UPDATE' as const,
        resourceType: 'users',
        ipAddress: '192.168.1.100',
        newValue: {
          profile: {
            name: 'João',
            credentials: {
              password: 'secret',
              apiKey: 'key123',
            },
          },
        },
      };

      await service.log(params);

      const callArgs = (db.insert(auditTrails).values as jest.Mock).mock
        .calls[0][0];
      expect(callArgs.newValue.profile.credentials.password).toBe(
        '[REDACTED]'
      );
      expect(callArgs.newValue.profile.credentials.apiKey).toBe('[REDACTED]');
      expect(callArgs.newValue.profile.name).toBe('João'); // Not redacted
    });
  });
});
