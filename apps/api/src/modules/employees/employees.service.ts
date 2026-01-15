import { Injectable, NotFoundException, Logger, BadRequestException } from '@nestjs/common';
import { createId } from '@paralleldrive/cuid2';
import { db, employees, sql as drizzleSql } from '@fast-consig/database';
import { eq, and, sql, isNull } from 'drizzle-orm';
import { ContextService } from '../../core/context/context.service';
import { AuditTrailService } from '../../shared/services/audit-trail.service';
import { MarginCalculationService } from '../../shared/services/margin-calculation.service';
import { CreateEmployeeDto, UpdateEmployeeDto, EmployeeResponseDto } from './dto';
import { OptimisticLockException } from './exceptions/optimistic-lock.exception';

@Injectable()
export class EmployeesService {
  private readonly logger = new Logger(EmployeesService.name);

  constructor(
    private readonly contextService: ContextService,
    private readonly auditTrailService: AuditTrailService,
    private readonly marginCalculationService: MarginCalculationService,
  ) {}

  /**
   * Set RLS context for the current tenant
   * This must be called before any query to enforce Row-Level Security
   */
  private async setTenantContext(tenantId: string): Promise<void> {
    await db.execute(sql`SELECT set_tenant_context(${tenantId})`);
  }

  async create(dto: CreateEmployeeDto): Promise<EmployeeResponseDto> {
    const tenantId = this.contextService.getTenantId();
    if (!tenantId) {
      throw new BadRequestException('Tenant ID is required');
    }

    // Set RLS context
    await this.setTenantContext(tenantId);

    // Check for duplicate CPF within tenant
    const existingByCpf = await db.query.employees.findFirst({
      where: (employees, { eq, and, isNull }) =>
        and(
          eq(employees.tenantId, tenantId),
          eq(employees.cpf, dto.cpf),
          isNull(employees.deletedAt),
        ),
    });

    if (existingByCpf) {
      throw new BadRequestException('Employee with this CPF already exists');
    }

    // Check for duplicate enrollment ID within tenant
    const existingByEnrollment = await db.query.employees.findFirst({
      where: (employees, { eq, and, isNull }) =>
        and(
          eq(employees.tenantId, tenantId),
          eq(employees.enrollmentId, dto.enrollmentId),
          isNull(employees.deletedAt),
        ),
    });

    if (existingByEnrollment) {
      throw new BadRequestException('Employee with this enrollment ID already exists');
    }

    const employeeId = createId();

    // Calculate margin BEFORE inserting
    const marginResult = await this.marginCalculationService.calculateAvailableMargin(
      dto.grossSalary,
      dto.mandatoryDiscounts || 0,
      tenantId,
    );

    try {
      const [newEmployee] = await db
        .insert(employees)
        .values({
          id: employeeId,
          tenantId,
          cpf: dto.cpf,
          enrollmentId: dto.enrollmentId,
          name: dto.name,
          email: dto.email,
          phone: dto.phone,
          grossSalary: dto.grossSalary,
          mandatoryDiscounts: dto.mandatoryDiscounts || 0,
          availableMargin: marginResult.availableMargin,
          usedMargin: 0,
        })
        .returning();

      // Audit trail
      await this.auditTrailService.log({
        tenantId,
        userId: this.contextService.getUserId()!,
        action: 'CREATE',
        resourceType: 'employee',
        resourceId: employeeId,
        ipAddress: this.contextService.getIp() || 'unknown',
        metadata: {
          name: dto.name,
          cpf: dto.cpf,
          marginCalculated: marginResult.availableMargin,
        },
      });

      return newEmployee;
    } catch (error: any) {
      // Handle unique constraint violations
      if (error?.code === '23505') {
        if (error.constraint?.includes('cpf')) {
          throw new BadRequestException('Employee with this CPF already exists');
        }
        if (error.constraint?.includes('enrollment')) {
          throw new BadRequestException('Employee with this enrollment ID already exists');
        }
        throw new BadRequestException('Employee with these details already exists');
      }
      throw error;
    }
  }

  async findAll(limit = 50, offset = 0): Promise<EmployeeResponseDto[]> {
    const tenantId = this.contextService.getTenantId();
    if (!tenantId) {
      throw new BadRequestException('Tenant ID is required');
    }

    // Set RLS context
    await this.setTenantContext(tenantId);

    // RLS will automatically filter by tenant
    return db.query.employees.findMany({
      where: (employees, { isNull }) => isNull(employees.deletedAt),
      limit,
      offset,
    });
  }

  async findById(id: string): Promise<EmployeeResponseDto> {
    const tenantId = this.contextService.getTenantId();
    if (!tenantId) {
      throw new BadRequestException('Tenant ID is required');
    }

    // Set RLS context
    await this.setTenantContext(tenantId);

    const employee = await db.query.employees.findFirst({
      where: (employees, { eq, and, isNull }) =>
        and(eq(employees.id, id), isNull(employees.deletedAt)),
    });

    if (!employee) {
      throw new NotFoundException(`Employee ${id} not found`);
    }

    return employee;
  }

  async findByCpf(cpf: string): Promise<EmployeeResponseDto> {
    const tenantId = this.contextService.getTenantId();
    if (!tenantId) {
      throw new BadRequestException('Tenant ID is required');
    }

    // Set RLS context
    await this.setTenantContext(tenantId);

    const employee = await db.query.employees.findFirst({
      where: (employees, { eq, and, isNull }) =>
        and(eq(employees.cpf, cpf), isNull(employees.deletedAt)),
    });

    if (!employee) {
      throw new NotFoundException(`Employee with CPF ${cpf} not found`);
    }

    return employee;
  }

  async update(id: string, dto: UpdateEmployeeDto): Promise<EmployeeResponseDto> {
    const tenantId = this.contextService.getTenantId();
    if (!tenantId) {
      throw new BadRequestException('Tenant ID is required');
    }

    // Set RLS context
    await this.setTenantContext(tenantId);

    // Get current employee for audit trail (before update)
    const currentEmployee = await this.findById(id);

    // Extract version from dto to avoid spreading it
    const { version: expectedVersion, ...updateData } = dto;

    // Check if recalculation is needed
    const needsRecalculation =
      dto.grossSalary !== undefined ||
      dto.mandatoryDiscounts !== undefined;

    let finalUpdateData: any = { ...updateData };

    if (needsRecalculation) {
      // Recalculate margin with new values
      const newGrossSalary = dto.grossSalary ?? currentEmployee.grossSalary;
      const newMandatoryDiscounts = dto.mandatoryDiscounts ?? currentEmployee.mandatoryDiscounts;

      const marginResult = await this.marginCalculationService.calculateAvailableMargin(
        newGrossSalary,
        newMandatoryDiscounts,
        tenantId,
      );

      finalUpdateData.availableMargin = marginResult.availableMargin;
    }

    try {
      // Optimistic lock: update only if version matches
      const result = await db
        .update(employees)
        .set({
          ...finalUpdateData,
          version: sql`version + 1`,
          updatedAt: new Date(),
        })
        .where(
          and(
            eq(employees.id, id),
            eq(employees.version, expectedVersion), // Optimistic lock check
            isNull(employees.deletedAt),
          ),
        )
        .returning();

      if (result.length === 0) {
        // Either not found OR version mismatch
        const existing = await db.query.employees.findFirst({
          where: (employees, { eq }) => eq(employees.id, id),
        });

        if (!existing || existing.deletedAt) {
          throw new NotFoundException(`Employee ${id} not found`);
        }

        throw new OptimisticLockException(
          `Employee ${id} was modified by another user. Expected version ${expectedVersion}, current version ${existing.version}`,
        );
      }

      // Audit trail with before/after values
      await this.auditTrailService.log({
        tenantId,
        userId: this.contextService.getUserId()!,
        action: 'UPDATE',
        resourceType: 'employee',
        resourceId: id,
        ipAddress: this.contextService.getIp() || 'unknown',
        metadata: {
          before: currentEmployee,
          after: result[0],
        },
      });

      return result[0];
    } catch (error: any) {
      // Handle unique constraint violations
      if (error?.code === '23505') {
        if (error.constraint?.includes('cpf')) {
          throw new BadRequestException('Employee with this CPF already exists');
        }
        if (error.constraint?.includes('enrollment')) {
          throw new BadRequestException('Employee with this enrollment ID already exists');
        }
        throw new BadRequestException('Employee with these details already exists');
      }
      throw error;
    }
  }

  async softDelete(id: string): Promise<void> {
    const tenantId = this.contextService.getTenantId();
    if (!tenantId) {
      throw new BadRequestException('Tenant ID is required');
    }

    // Set RLS context
    await this.setTenantContext(tenantId);

    const employee = await this.findById(id);

    await db
      .update(employees)
      .set({
        deletedAt: new Date(),
        updatedAt: new Date(),
      })
      .where(eq(employees.id, id));

    // Audit trail
    await this.auditTrailService.log({
      tenantId,
      userId: this.contextService.getUserId()!,
      action: 'DELETE',
      resourceType: 'employee',
      resourceId: id,
      ipAddress: this.contextService.getIp() || 'unknown',
      metadata: { name: employee.name },
    });
  }
}
