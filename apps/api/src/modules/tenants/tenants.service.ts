import { Injectable, InternalServerErrorException, Logger, NotFoundException, BadRequestException } from '@nestjs/common';
import { ConfigService } from '@nestjs/config';
import { clerkClient } from '@clerk/clerk-sdk-node';
import { createId } from '@paralleldrive/cuid2';
import { db, tenants } from '@fast-consig/database';
import { CreateTenantInput, TenantResponse, ROLES } from '@fast-consig/shared';
import { ContextService } from '../../core/context/context.service';
import { AuditTrailService } from '../../shared/services/audit-trail.service';
import { eq } from 'drizzle-orm';

@Injectable()
export class TenantsService {
  private readonly logger = new Logger(TenantsService.name);

  constructor(
    private readonly configService: ConfigService,
    private readonly contextService: ContextService,
    private readonly auditTrailService: AuditTrailService,
  ) {}

  // ... existing code ...

  async listMembers(tenantId: string) {
    // 1. Get Clerk Org ID from local Tenant ID
    const tenant = await this.getTenant(tenantId);

    try {
      // 2. Fetch members from Clerk
      const memberships = await clerkClient.organizations.getOrganizationMembershipList({
        organizationId: tenant.clerkOrgId,
        limit: 100,
      });

      // 3. Map to application DTO
      return memberships.map(m => ({
        userId: m.publicUserData.userId,
        email: m.publicUserData.identifier,
        firstName: m.publicUserData.firstName,
        lastName: m.publicUserData.lastName,
        role: m.role, // org:admin or org:member
        joinedAt: m.createdAt,
      }));
    } catch (error) {
      this.logger.error(`Failed to list members for tenant ${tenantId}`, error);
      throw new InternalServerErrorException('Failed to fetch team members');
    }
  }

  async inviteMember(tenantId: string, email: string, role: string) {
    const tenant = await this.getTenant(tenantId);
    const validRoles = ['org:admin', 'org:member'];

    if (!validRoles.includes(role)) {
      throw new BadRequestException('Invalid role. Must be Manager or Operator.');
    }

    try {
      const invitation = await clerkClient.organizations.createOrganizationInvitation({
        organizationId: tenant.clerkOrgId,
        emailAddress: email,
        role: role as any,
        inviterUserId: this.contextService.getUserId(),
      });

      await this.auditTrailService.log({
        tenantId,
        userId: this.contextService.getUserId()!,
        action: 'CREATE',
        resourceType: 'team_invite',
        resourceId: invitation.id,
        ipAddress: this.contextService.getIp() || 'unknown',
        metadata: { invitedEmail: email, role },
      });

      return { id: invitation.id, status: 'pending', email };
    } catch (error) {
      this.logger.error(`Failed to invite member ${email} to tenant ${tenantId}`, error);
      throw new InternalServerErrorException('Failed to send invitation');
    }
  }

  async removeMember(tenantId: string, userId: string) {
    const tenant = await this.getTenant(tenantId);

    try {
      // Remove from Clerk Organization
      await clerkClient.organizations.deleteOrganizationMembership({
        organizationId: tenant.clerkOrgId,
        userId,
      });

      await this.auditTrailService.log({
        tenantId,
        userId: this.contextService.getUserId()!,
        action: 'DELETE',
        resourceType: 'team_member',
        resourceId: userId,
        ipAddress: this.contextService.getIp() || 'unknown',
      });

      return { success: true };
    } catch (error) {
      this.logger.error(`Failed to remove member ${userId} from tenant ${tenantId}`, error);
      throw new InternalServerErrorException('Failed to remove member');
    }
  }

  private async getTenant(tenantId: string) {
    const tenant = await db.query.tenants.findFirst({
      where: (tenants, { eq }) => eq(tenants.id, tenantId),
    });

    if (!tenant) {
      throw new NotFoundException('Tenant not found');
    }
    return tenant;
  }

  // ... existing code ...

  async create(input: CreateTenantInput): Promise<TenantResponse> {
    const clerkSecretKey = this.configService.get<string>('CLERK_SECRET_KEY');
    if (!clerkSecretKey) {
      throw new InternalServerErrorException('CLERK_SECRET_KEY is not configured');
    }

    // 0. Pre-check for duplicates to avoid unnecessary external calls
    const existingByCnpj = await db.query.tenants.findFirst({
      where: (tenants, { eq }) => eq(tenants.cnpj, input.cnpj),
    });

    if (existingByCnpj) {
      throw new InternalServerErrorException('Tenant with this CNPJ already exists');
    }

    const existingBySlug = await db.query.tenants.findFirst({
      where: (tenants, { eq }) => eq(tenants.slug, input.slug),
    });

    if (existingBySlug) {
      throw new InternalServerErrorException('Tenant with this Slug already exists');
    }

    // 1. Create Organization in Clerk
    let clerkOrg: any;
    try {
      clerkOrg = await clerkClient.organizations.createOrganization({
        name: input.name,
        slug: input.slug,
        createdBy: this.contextService.getUserId() || 'system', // Fallback for bootstrapping
      });
    } catch (error: any) {
      this.logger.error('Failed to create Clerk Organization', error);

      // Parse Clerk-specific errors for better user feedback
      if (error?.errors?.[0]?.code === 'duplicate_record') {
        throw new InternalServerErrorException('Organization with this slug already exists in identity provider');
      }

      throw new InternalServerErrorException('Failed to create identity provider organization. Please check your configuration and try again.');
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
      await this.auditTrailService.log({
        tenantId,
        userId: this.contextService.getUserId() || 'system',
        action: 'CREATE',
        resourceType: 'tenant',
        resourceId: tenantId,
        ipAddress: 'internal',
        details: 'Tenant Created',
      } as any);

      return newTenant;

    } catch (error) {
      this.logger.error('Failed to complete Tenant creation. Rolling back Clerk Org.', error);

      // Rollback: Delete Clerk Org and DB record if they exist
      try {
        // Always try to delete Clerk org first
        await clerkClient.organizations.deleteOrganization(clerkOrg.id);
      } catch (clerkRollbackError) {
        this.logger.error('CRITICAL: Failed to rollback Clerk Organization.', clerkRollbackError);
      }

      // Try to delete DB record if it was created
      try {
        const insertedTenant = await db.query.tenants.findFirst({
            where: (tenants, { eq }) => eq(tenants.id, tenantId)
        });

        if (insertedTenant) {
          await db.delete(tenants).where(eq(tenants.id, tenantId));
        }
      } catch (dbRollbackError) {
        this.logger.error('CRITICAL: Failed to rollback DB tenant record.', dbRollbackError);
      }

      throw new InternalServerErrorException('Failed to create tenant. Please try again.');
    }
  }

  async list(): Promise<TenantResponse[]> {
    return db.select().from(tenants);
  }
}
