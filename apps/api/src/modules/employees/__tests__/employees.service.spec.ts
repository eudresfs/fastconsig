import { Test, TestingModule } from '@nestjs/testing';
import { EmployeesService } from '../employees.service';
import { ContextService } from '../../../core/context/context.service';
import { AuditTrailService } from '../../../shared/services/audit-trail.service';
import { OptimisticLockException } from '../exceptions/optimistic-lock.exception';
import { NotFoundException, BadRequestException } from '@nestjs/common';
import { vi, describe, it, expect, beforeEach } from 'vitest';

// Mock database
vi.mock('@fast-consig/database', () => ({
  db: {
    insert: vi.fn(),
    update: vi.fn(),
    query: {
      employees: {
        findFirst: vi.fn(),
        findMany: vi.fn(),
      },
    },
  },
  employees: {
    id: 'id',
    tenantId: 'tenant_id',
    cpf: 'cpf',
    version: 'version',
    deletedAt: 'deleted_at',
  },
}));

describe('EmployeesService', () => {
  let service: EmployeesService;
  let contextService: ContextService;
  let auditTrailService: AuditTrailService;

  const mockContextService = {
    getTenantId: vi.fn(),
    getUserId: vi.fn(),
    getIp: vi.fn(),
  };

  const mockAuditTrailService = {
    log: vi.fn(),
  };

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        EmployeesService,
        { provide: ContextService, useValue: mockContextService },
        { provide: AuditTrailService, useValue: mockAuditTrailService },
      ],
    }).compile();

    service = module.get<EmployeesService>(EmployeesService);
    contextService = module.get<ContextService>(ContextService);
    auditTrailService = module.get<AuditTrailService>(AuditTrailService);

    // Reset mocks
    vi.clearAllMocks();
    mockContextService.getTenantId.mockReturnValue('tenant-123');
    mockContextService.getUserId.mockReturnValue('user-123');
    mockContextService.getIp.mockReturnValue('127.0.0.1');
  });

  describe('create', () => {
    it('should create a new employee', async () => {
      const dto = {
        cpf: '12345678901',
        enrollmentId: 'EMP001',
        name: 'John Doe',
        email: 'john@example.com',
        grossSalary: 500000,
        mandatoryDiscounts: 10000,
      };

      const { db } = await import('@fast-consig/database');
      db.query.employees.findFirst.mockResolvedValue(null);
      db.insert.mockReturnValue({
        values: vi.fn().mockReturnValue({
          returning: vi.fn().mockResolvedValue([{ id: 'emp-123', ...dto, tenantId: 'tenant-123', version: 1 }]),
        }),
      });

      const result = await service.create(dto);

      expect(result).toBeDefined();
      expect(result.cpf).toBe(dto.cpf);
      expect(mockAuditTrailService.log).toHaveBeenCalledWith(
        expect.objectContaining({
          action: 'CREATE',
          resourceType: 'employee',
        }),
      );
    });

    it('should throw BadRequestException for duplicate CPF', async () => {
      const dto = {
        cpf: '12345678901',
        enrollmentId: 'EMP001',
        name: 'John Doe',
        grossSalary: 500000,
      };

      const { db } = await import('@fast-consig/database');
      db.query.employees.findFirst.mockResolvedValue({ id: 'existing-emp' });

      await expect(service.create(dto)).rejects.toThrow(BadRequestException);
    });
  });

  describe('update - Optimistic Locking', () => {
    it('should update employee with correct version', async () => {
      const employeeId = 'emp-123';
      const dto = {
        name: 'Jane Doe',
        version: 1,
      };

      const currentEmployee = {
        id: employeeId,
        cpf: '12345678901',
        name: 'John Doe',
        version: 1,
      };

      const updatedEmployee = {
        ...currentEmployee,
        name: 'Jane Doe',
        version: 2,
      };

      const { db } = await import('@fast-consig/database');
      db.query.employees.findFirst.mockResolvedValue(currentEmployee);
      db.update.mockReturnValue({
        set: vi.fn().mockReturnValue({
          where: vi.fn().mockReturnValue({
            returning: vi.fn().mockResolvedValue([updatedEmployee]),
          }),
        }),
      });

      const result = await service.update(employeeId, dto);

      expect(result.version).toBe(2);
      expect(result.name).toBe('Jane Doe');
      expect(mockAuditTrailService.log).toHaveBeenCalledWith(
        expect.objectContaining({
          action: 'UPDATE',
          resourceType: 'employee',
          metadata: expect.objectContaining({
            before: currentEmployee,
            after: updatedEmployee,
          }),
        }),
      );
    });

    it('should throw OptimisticLockException when version mismatch', async () => {
      const employeeId = 'emp-123';
      const dto = {
        name: 'Jane Doe',
        version: 1, // Client has version 1
      };

      const currentEmployee = {
        id: employeeId,
        version: 2, // Database has version 2 (someone else updated it)
      };

      const { db } = await import('@fast-consig/database');
      db.query.employees.findFirst.mockResolvedValue(currentEmployee);
      db.update.mockReturnValue({
        set: vi.fn().mockReturnValue({
          where: vi.fn().mockReturnValue({
            returning: vi.fn().mockResolvedValue([]), // No rows updated due to version mismatch
          }),
        }),
      });

      await expect(service.update(employeeId, dto)).rejects.toThrow(OptimisticLockException);
    });
  });

  describe('findAll - RLS Isolation', () => {
    it('should return only tenant employees', async () => {
      const { db } = await import('@fast-consig/database');
      db.query.employees.findMany.mockResolvedValue([
        { id: 'emp-1', tenantId: 'tenant-123', name: 'Employee 1' },
        { id: 'emp-2', tenantId: 'tenant-123', name: 'Employee 2' },
      ]);

      const result = await service.findAll();

      expect(result).toHaveLength(2);
      expect(result.every((e) => e.tenantId === 'tenant-123')).toBe(true);
    });
  });

  describe('softDelete', () => {
    it('should soft delete employee', async () => {
      const employeeId = 'emp-123';
      const employee = {
        id: employeeId,
        name: 'John Doe',
        cpf: '12345678901',
      };

      const { db } = await import('@fast-consig/database');
      db.query.employees.findFirst.mockResolvedValue(employee);
      db.update.mockReturnValue({
        set: vi.fn().mockReturnValue({
          where: vi.fn().mockResolvedValue(undefined),
        }),
      });

      await service.softDelete(employeeId);

      expect(mockAuditTrailService.log).toHaveBeenCalledWith(
        expect.objectContaining({
          action: 'DELETE',
          resourceType: 'employee',
        }),
      );
    });
  });
});
