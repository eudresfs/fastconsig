import { Injectable, InternalServerErrorException, Logger } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { clerkClient } from '@clerk/clerk-sdk-node';
import { createId } from '@paralleldrive/cuid2';
import { db, tenants, auditTrails } from '@fast-consig/database';
import { CreateTenantInput, TenantResponse, ROLES } from '@fast-consig/shared';
import { ContextService } from '../../core/context/context.service';
import { eq } from 'drizzle-orm';

@Injectable()
export class TenantsService {
  private readonly logger = new Logger(TenantsService.name);

  constructor(
    private readonly configService: ConfigService,
    private readonly contextService: ContextService,
  ) {}

  async create(input: CreateTenantInput): Promise<TenantResponse> {
    const clerkSecretKey = this.configService.get<string>('CLERK_SECRET_KEY');
    if (!clerkSecretKey) {
      throw new InternalServerErrorException('CLERK_SECRET_KEY is not configured');
    }

    // 0. Pre-check for duplicates to avoid unnecessary external calls
    const existingTenant = await db.query.tenants.findFirst({
      where: (tenants, { or, eq }) => or(eq(tenants.cnpj, input.cnpj), eq(tenants.slug, input.slug)),
    });

    if (existingTenant) {
      throw new InternalServerErrorException('Tenant with this CNPJ or Slug already exists');
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

      // 3. Invite Admin (if email provided)
      if (input.adminEmail) {
        try {
          await clerkClient.organizations.createOrganizationInvitation({
            organizationId: clerkOrg.id,
            emailAddress: input.adminEmail,
            role: ROLES.ORG_ADMIN,
            inviterUserId: this.contextService.getUserId(),
          });
        } catch (inviteError) {
          this.logger.error('Failed to invite admin. Rolling back.', inviteError);
          // If invitation fails, we should rollback everything to avoid orphaned tenant
          throw new Error('Failed to send admin invitation');
        }
      }

      // 4. Audit Trail
      await this.logAudit(tenantId, 'CREATE', 'Tenant Created');

      return newTenant;

    } catch (error) {
      this.logger.error('Failed to complete Tenant creation. Rolling back Clerk Org.', error);

      // Rollback: Delete Clerk Org
      try {
        await clerkClient.organizations.deleteOrganization(clerkOrg.id);
        // If DB insert succeeded but invitation failed, we must also delete the DB record!
        // However, standard rollback here deletes Clerk org.
        // If DB insert passed, we need to delete from DB too.
        // Let's check if we have a DB record to delete (if error came from step 3)
        const insertedTenant = await db.query.tenants.findFirst({
            where: (tenants, { eq }) => eq(tenants.id, tenantId)
        });

        if (insertedTenant) {
             await db.delete(tenants).where(eq(tenants.id, tenantId));
        }

      } catch (rollbackError) {
        this.logger.error('CRITICAL: Failed to rollback Clerk Org or DB after failure.', rollbackError);
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
