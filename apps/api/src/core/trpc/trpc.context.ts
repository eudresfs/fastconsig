import { inferAsyncReturnType } from '@trpc/server';
import { CreateFastifyContextOptions } from '@trpc/server/adapters/fastify';
import { ContextService } from '../context/context.service';

export const createContext = (
  contextService: ContextService,
) => {
  return async (opts: CreateFastifyContextOptions) => {
    return {
      req: opts.req,
      res: opts.res,
      contextService,
      user: contextService.getContext(),
    };
  };
};

export type TrpcContext = inferAsyncReturnType<ReturnType<typeof createContext>>;
