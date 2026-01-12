import { Injectable, InternalServerErrorException, Logger } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { clerkClient } from '@clerk/clerk-sdk-node';
import { createId } from '@paralleldrive/cuid2';
import { db, tenants, auditTrails } from '@fast-consig/database';
import { CreateTenantInput, TenantResponse } from '@fast-consig/shared';
import { ContextService } from '../../core/context/context.service';
import { eq } from 'drizzle-orm';

@Injectable()
export class TenantsService {
  private readonly logger = new Logger(TenantsService.name);

  constructor(
    private readonly configService: ConfigService,
    private readonly contextService: ContextService,
  ) {
    console.log('TenantsService initialized with:', {
      configService: !!this.configService,
      contextService: !!this.contextService
    });
  }

  async create(input: CreateTenantInput): Promise<TenantResponse> {
    const clerkSecretKey = this.configService.get<string>('CLERK_SECRET_KEY');
    if (!clerkSecretKey) {
      throw new InternalServerErrorException('CLERK_SECRET_KEY is not configured');
    }

    // 1. Create Organization in Clerk
    let clerkOrg: any;
    try {
      clerkOrg = await clerkClient.organizations.createOrganization({
        name: input.name,
        slug: input.slug,
        createdBy: this.contextService.getUserId() || 'system', // Fallback for bootstrapping
      });
    } catch (error) {
      this.logger.error('Failed to create Clerk Organization', error);
      throw new InternalServerErrorException('Failed to create identity provider organization');
    }

    // 2. Create Tenant in Database (Dual-write)
    const tenantId = createId();
    try {
      const [newTenant] = await db.insert(tenants).values({
        id: tenantId,
        clerkOrgId: clerkOrg.id,
        name: input.name,
        cnpj: input.cnpj,
        slug: input.slug,
        active: true,
      }).returning();

      // 3. Invite Admin (if email provided) - Handled separately or here?
      // Story says: "Then the system triggers an invitation to the email provided"
      if (input.adminEmail) {
        await clerkClient.organizations.createOrganizationInvitation({
          organizationId: clerkOrg.id,
          emailAddress: input.adminEmail,
          role: 'org:admin',
          inviterUserId: this.contextService.getUserId(),
        });
      }

      // 4. Audit Trail
      await this.logAudit(tenantId, 'CREATE', 'Tenant Created');

      return newTenant;

    } catch (dbError) {
      this.logger.error('Failed to create Tenant in DB. Rolling back Clerk Org.', dbError);

      // Rollback: Delete Clerk Org
      try {
        await clerkClient.organizations.deleteOrganization(clerkOrg.id);
      } catch (rollbackError) {
        this.logger.error('CRITICAL: Failed to rollback Clerk Org creation after DB failure.', rollbackError);
        // Alerting should be triggered here
      }

      throw new InternalServerErrorException('Failed to create tenant. Please try again.');
    }
  }

  async list(): Promise<TenantResponse[]> {
    return db.select().from(tenants);
  }

  private async logAudit(resourceId: string, action: string, details: string) {
    try {
      const actorId = this.contextService.getUserId() || 'system';
      await db.insert(auditTrails).values({
        id: createId(),
        tenantId: resourceId, // For Tenant creation, the resource IS the tenant
        actorId,
        action,
        resource: 'tenant',
        details,
      });
    } catch (error) {
      // Audit failure shouldn't block main flow, but should be logged
      this.logger.error('Failed to write audit log', error);
    }
  }
}
