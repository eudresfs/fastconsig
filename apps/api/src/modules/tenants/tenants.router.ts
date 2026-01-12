import { router, superAdminProcedure } from '../../core/trpc/trpc.init';
import { CreateTenantSchema } from '@fast-consig/shared';
import { TenantsService } from './tenants.service';
import { Injectable } from '@nestjs/common';

@Injectable()
export class TenantsRouter {
  constructor(private readonly tenantsService: TenantsService) {}

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
    });
  }
}
