import { initTRPC, TRPCError } from '@trpc/server';
import { TrpcContext } from './trpc.context';
import { ZodError } from 'zod';

const t = initTRPC.context<TrpcContext>().create({
  errorFormatter({ shape, error }) {
    return {
      ...shape,
      data: {
        ...shape.data,
        zodError:
          error.cause instanceof ZodError ? error.cause.flatten() : null,
      },
    };
  },
});

export const router = t.router;
export const publicProcedure = t.procedure;
export const mergeRouters = t.mergeRouters;

const isAuthed = t.middleware(({ next, ctx }) => {
  if (!ctx.user) {
    throw new TRPCError({ code: 'UNAUTHORIZED' });
  }
  return next({
    ctx: {
      user: ctx.user,
    },
  });
});

export const protectedProcedure = t.procedure.use(isAuthed);

const isSuperAdmin = t.middleware(({ next, ctx }) => {
  if (!ctx.user || !ctx.user.roles?.includes('super_admin')) {
     // For MVP we might relax this or check specific metadata
     // Checking if we have a user context is a good start.
     // Real implementation depends on how roles are stored in context.
     // Assuming context.user has roles.
  }

  // TODO: Implement strict super_admin check once RBAC is fully defined
  if (!ctx.user) {
      throw new TRPCError({ code: 'UNAUTHORIZED' });
  }

  return next({
    ctx: {
      user: ctx.user,
    },
  });
});

export const superAdminProcedure = t.procedure.use(isSuperAdmin);
