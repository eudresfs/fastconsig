import { Injectable } from '@nestjs/common';
import { UserContext } from '@fast-consig/shared';
import { contextStore } from './context.store';

@Injectable()
export class ContextService {
  run(context: UserContext, callback: () => void): void {
    contextStore.run(context, callback);
  }

  getContext(): UserContext | undefined {
    return contextStore.getStore();
  }

  getTenantId(): string | undefined {
    return this.getContext()?.tenantId;
  }

  getUserId(): string | undefined {
    return this.getContext()?.userId;
  }
}
