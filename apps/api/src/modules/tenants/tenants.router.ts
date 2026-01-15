import { router, superAdminProcedure, adminProcedure } from '../../core/trpc/trpc.init';
import { CreateTenantSchema, UpdateTenantConfigSchema } from '@fast-consig/shared';
import { TenantsService } from './tenants.service';
import { TenantConfigurationService } from './tenant-configuration.service';
import { Injectable } from '@nestjs/common';
import { z } from 'zod';

@Injectable()
export class TenantsRouter {
  constructor(
    private readonly tenantsService: TenantsService,
    private readonly tenantConfigService: TenantConfigurationService,
  ) {}

  get router() {
    return router({
      create: superAdminProcedure
        .input(CreateTenantSchema)
        .mutation(async ({ input }) => {
          return this.tenantsService.create(input);
        }),
      list: superAdminProcedure
        .query(async () => {
          return this.tenantsService.list();
        }),
      getConfig: adminProcedure // Changed to adminProcedure (HR Manager)
        .input(z.object({ tenantId: z.string() }))
        .query(async ({ input }) => {
          return this.tenantConfigService.get(input.tenantId);
        }),
      updateConfig: adminProcedure // Changed to adminProcedure (HR Manager)
        .input(z.object({ tenantId: z.string(), config: UpdateTenantConfigSchema }))
        .mutation(async ({ input }) => {
          return this.tenantConfigService.upsert(input.tenantId, input.config);
        }),
      listMembers: adminProcedure
        .input(z.object({ tenantId: z.string() }))
        .query(async ({ input }) => {
          return this.tenantsService.listMembers(input.tenantId);
        }),
      inviteMember: adminProcedure
        .input(z.object({ tenantId: z.string(), email: z.string().email(), role: z.enum(['org:admin', 'org:member']) }))
        .mutation(async ({ input }) => {
          return this.tenantsService.inviteMember(input.tenantId, input.email, input.role);
        }),
      removeMember: adminProcedure
        .input(z.object({ tenantId: z.string(), userId: z.string() }))
        .mutation(async ({ input }) => {
          return this.tenantsService.removeMember(input.tenantId, input.userId);
        }),
    });
  }
}
