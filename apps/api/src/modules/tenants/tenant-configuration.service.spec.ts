import { Test, TestingModule } from '@nestjs/testing';
import { TenantConfigurationService } from './tenant-configuration.service';
import { ContextService } from '../../core/context/context.service';
import { db, tenantConfigurations, auditTrails } from '@fast-consig/database';
import { vi, describe, it, expect, beforeEach, afterEach } from 'vitest';

// Mock DB and other dependencies
vi.mock('@fast-consig/database', () => {
  return {
    db: {
      query: {
        tenantConfigurations: {
          findFirst: vi.fn(),
        },
      },
      insert: vi.fn().mockReturnThis(),
      update: vi.fn().mockReturnThis(),
      values: vi.fn().mockReturnThis(),
      set: vi.fn().mockReturnThis(),
      where: vi.fn().mockReturnThis(),
      returning: vi.fn(),
    },
    tenantConfigurations: {
      id: 'id',
    },
    auditTrails: {
      id: 'id',
    },
  };
});

describe('TenantConfigurationService', () => {
  let service: TenantConfigurationService;
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  let contextService: ContextService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        TenantConfigurationService,
        {
          provide: ContextService,
          useValue: {
            getUserId: vi.fn().mockReturnValue('user-123'),
          },
        },
      ],
    }).compile();

    service = module.get<TenantConfigurationService>(TenantConfigurationService);
    contextService = module.get<ContextService>(ContextService);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('get', () => {
    it('should return existing config', async () => {
      const mockConfig = {
        id: 'config-1',
        tenantId: 'tenant-1',
        standardMarginBasisPoints: 3000,
      };

      // Mock tenant exists
      (db.query.tenants as any) = {
        findFirst: vi.fn().mockResolvedValue({ id: 'tenant-1' }),
      };

      (db.query.tenantConfigurations.findFirst as any).mockResolvedValue(mockConfig);

      const result = await service.get('tenant-1');
      expect(result).toEqual(mockConfig);
    });

    it('should create default config if not found', async () => {
      // Mock tenant exists
      (db.query.tenants as any) = {
        findFirst: vi.fn().mockResolvedValue({ id: 'tenant-1' }),
      };

      (db.query.tenantConfigurations.findFirst as any).mockResolvedValue(null);
      const mockCreatedConfig = {
        id: 'new-config',
        tenantId: 'tenant-1',
      };

      const returningMock = vi.fn().mockResolvedValue([mockCreatedConfig]);
      const valuesMock = vi.fn().mockReturnValue({ returning: returningMock });
      (db.insert as any).mockReturnValue({ values: valuesMock });

      const result = await service.get('tenant-1');
      expect(result).toEqual(mockCreatedConfig);
      expect(db.insert).toHaveBeenCalledWith(tenantConfigurations);
    });
  });

  describe('upsert', () => {
    const input = {
      standardMarginPercent: 35.5,
      benefitCardMarginPercent: 5,
      payrollCutoffDay: 15,
      minInstallmentValueCents: 2000,
      maxInstallments: 60,
    };

    beforeEach(() => {
      // Mock tenant existence check
      const tenantMock = { id: 'tenant-1', name: 'Test Tenant' };
      (db.query.tenants as any) = {
        findFirst: vi.fn().mockResolvedValue(tenantMock),
      };
    });

    it('should update existing config', async () => {
      const existingConfig = {
        id: 'config-1',
        tenantId: 'tenant-1',
      };
      (db.query.tenantConfigurations.findFirst as any).mockResolvedValue(existingConfig);

      const updatedConfig = { ...existingConfig, standardMarginBasisPoints: 3550 };

      const returningMock = vi.fn().mockResolvedValue([updatedConfig]);
      const whereMock = vi.fn().mockReturnValue({ returning: returningMock });
      const setMock = vi.fn().mockReturnValue({ where: whereMock });
      (db.update as any).mockReturnValue({ set: setMock });

      const result = await service.upsert('tenant-1', input);

      expect(db.update).toHaveBeenCalledWith(tenantConfigurations);
      expect(setMock).toHaveBeenCalledWith(expect.objectContaining({
        standardMarginBasisPoints: 3550,
      }));
      expect(result).toEqual(updatedConfig);

      // Verify Audit Log
      expect(db.insert).toHaveBeenCalledWith(auditTrails);
    });

    it('should create new config if not exists', async () => {
      (db.query.tenantConfigurations.findFirst as any).mockResolvedValue(null);

      const createdConfig = { id: 'new-config', ...input };

      const returningMock = vi.fn().mockResolvedValue([createdConfig]);
      const valuesMock = vi.fn().mockReturnValue({ returning: returningMock });
      (db.insert as any).mockReturnValue({ values: valuesMock });

      const result = await service.upsert('tenant-1', input);

      expect(db.insert).toHaveBeenCalledWith(tenantConfigurations);
      expect(result).toEqual(createdConfig);

      // Verify Audit Log
      expect(db.insert).toHaveBeenCalledWith(auditTrails);
    });

    it('should not throw if audit log fails', async () => {
      const existingConfig = {
        id: 'config-1',
        tenantId: 'tenant-1',
      };
      (db.query.tenantConfigurations.findFirst as any).mockResolvedValue(existingConfig);

      const updatedConfig = { ...existingConfig, standardMarginBasisPoints: 3550 };

      const returningMock = vi.fn().mockResolvedValue([updatedConfig]);
      const whereMock = vi.fn().mockReturnValue({ returning: returningMock });
      const setMock = vi.fn().mockReturnValue({ where: whereMock });
      (db.update as any).mockReturnValue({ set: setMock });

      // Mock Audit Log Failure
      (db.insert as any).mockRejectedValueOnce(new Error('Audit Log Failed'));

      const result = await service.upsert('tenant-1', input);

      expect(result).toEqual(updatedConfig);
      // Should not throw
    });

    it('should throw NotFoundException if tenant does not exist', async () => {
      // Mock tenant not found
      (db.query.tenants as any) = {
        findFirst: vi.fn().mockResolvedValue(null),
      };

      await expect(service.upsert('non-existent-tenant', input)).rejects.toThrow('Tenant with id non-existent-tenant not found');
    });

    it('should handle decimal precision correctly', async () => {
      const preciseInput = {
        standardMarginPercent: 30.55,
        benefitCardMarginPercent: 5.45,
        payrollCutoffDay: 20,
        minInstallmentValueCents: 1000,
        maxInstallments: 96,
      };

      (db.query.tenantConfigurations.findFirst as any).mockResolvedValue(null);

      const createdConfig = { id: 'new-config', ...preciseInput };

      const returningMock = vi.fn().mockResolvedValue([createdConfig]);
      const valuesMock = vi.fn().mockReturnValue({ returning: returningMock });
      (db.insert as any).mockReturnValue({ values: valuesMock });

      await service.upsert('tenant-1', preciseInput);

      // Verify basis points conversion
      expect(valuesMock).toHaveBeenCalledWith(expect.objectContaining({
        standardMarginBasisPoints: 3055, // 30.55 * 100
        benefitCardMarginBasisPoints: 545, // 5.45 * 100
      }));
    });
  });
});
