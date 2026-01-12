import { router, mergeRouters } from './core/trpc/trpc.init';
import { TenantsRouter } from './modules/tenants/tenants.router';
import { Injectable } from '@nestjs/common';

@Injectable()
export class AppRouter {
  constructor(private readonly tenantsRouter: TenantsRouter) {}

  get appRouter() {
    return router({
      tenants: this.tenantsRouter.router,
    });
  }
}

export type AppRouterType = AppRouter['appRouter'];
