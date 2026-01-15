import { router, mergeRouters } from './core/trpc/trpc.init';
import { TenantsRouter } from './modules/tenants/tenants.router';
import { LoanOperationsRouter } from './modules/loans/loan-operations.router';
import { Injectable } from '@nestjs/common';

@Injectable()
export class AppRouter {
  constructor(
    private readonly tenantsRouter: TenantsRouter,
    private readonly loanOperationsRouter: LoanOperationsRouter,
  ) {}

  get appRouter() {
    return router({
      tenants: this.tenantsRouter.router,
      loans: this.loanOperationsRouter.router,
    });
  }
}

export type AppRouterType = AppRouter['appRouter'];
