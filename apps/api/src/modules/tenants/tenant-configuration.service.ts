import { Injectable, Logger, NotFoundException } from '@nestjs/common';
import { db, tenantConfigurations, auditTrails, tenants } from '@fast-consig/database';
import { UpdateTenantConfigInput, TenantConfigResponse } from '@fast-consig/shared';
import { ContextService } from '../../core/context/context.service';
import { createId } from '@paralleldrive/cuid2';
import { eq } from 'drizzle-orm';

@Injectable()
export class TenantConfigurationService {
  private readonly logger = new Logger(TenantConfigurationService.name);

  constructor(private readonly contextService: ContextService) {}

  async get(tenantId: string): Promise<TenantConfigResponse> {
    // Validate tenant exists first
    await this.validateTenantExists(tenantId);

    const config = await db.query.tenantConfigurations.findFirst({
      where: (configs, { eq }) => eq(configs.tenantId, tenantId),
    });

    if (!config) {
      // Create default config if not found
      return this.createDefault(tenantId);
    }

    return config;
  }

  async upsert(tenantId: string, input: UpdateTenantConfigInput): Promise<TenantConfigResponse> {
    // Validate tenant exists first
    await this.validateTenantExists(tenantId);

    const existingConfig = await db.query.tenantConfigurations.findFirst({
      where: (configs, { eq }) => eq(configs.tenantId, tenantId),
    });

    // Transform percent to basis points with precision validation
    // Input is already validated by Zod to have max 2 decimal places
    const standardMarginBasisPoints = Math.round(input.standardMarginPercent * 100);
    const benefitCardMarginBasisPoints = Math.round(input.benefitCardMarginPercent * 100);

    const dataToSave = {
      standardMarginBasisPoints,
      benefitCardMarginBasisPoints,
      payrollCutoffDay: input.payrollCutoffDay,
      minInstallmentValueCents: input.minInstallmentValueCents,
      maxInstallments: input.maxInstallments,
    };

    let result: TenantConfigResponse;

    if (existingConfig) {
      // Update
      const [updated] = await db.update(tenantConfigurations)
        .set({
          ...dataToSave,
          updatedAt: new Date(),
        })
        .where(eq(tenantConfigurations.id, existingConfig.id))
        .returning();

      result = updated;

      // Audit Log
      await this.logAudit(tenantId, 'UPDATE_CONFIG', JSON.stringify({
        old: existingConfig,
        new: updated,
      }));

    } else {
      // Create (should rarely happen if we create default on get, but good for safety)
      const [created] = await db.insert(tenantConfigurations).values({
        id: createId(),
        tenantId,
        ...dataToSave,
      }).returning();

      result = created;

      await this.logAudit(tenantId, 'CREATE_CONFIG', 'Initial configuration created via upsert');
    }

    return result;
  }

  private async createDefault(tenantId: string): Promise<TenantConfigResponse> {
    const [created] = await db.insert(tenantConfigurations).values({
      id: createId(),
      tenantId,
      // Defaults are handled by DB default values, but we can be explicit here if needed
    }).returning();

    await this.logAudit(tenantId, 'CREATE_CONFIG', 'Default configuration created');

    return created;
  }

  private async logAudit(resourceId: string, action: string, details: string) {
    try {
      const actorId = this.contextService.getUserId() || 'system';
      await db.insert(auditTrails).values({
        id: createId(),
        tenantId: resourceId, // For Config, resource is linked to tenant
        actorId,
        action,
        resource: 'tenant_configuration',
        details,
      });
    } catch (error) {
      this.logger.error('Failed to write audit log', error);
    }
  }

  /**
   * Validates that a tenant exists before performing operations
   * @throws NotFoundException if tenant does not exist
   */
  private async validateTenantExists(tenantId: string): Promise<void> {
    const tenant = await db.query.tenants.findFirst({
      where: (t, { eq }) => eq(t.id, tenantId),
    });

    if (!tenant) {
      throw new NotFoundException(`Tenant with id ${tenantId} not found`);
    }
  }
}
