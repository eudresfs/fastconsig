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
  // Strict super_admin role check - only users with 'super_admin' role can access
  if (!ctx.user || !ctx.user.roles?.includes('super_admin')) {
    throw new TRPCError({ code: 'FORBIDDEN', message: 'Super admin access required' });
  }

  return next({
    ctx: {
      user: ctx.user,
    },
  });
});

export const superAdminProcedure = t.procedure.use(isSuperAdmin);

const isAdmin = t.middleware(({ next, ctx }) => {
  if (!ctx.user || !ctx.user.roles?.includes('org:admin')) {
    throw new TRPCError({ code: 'FORBIDDEN', message: 'Admin access required' });
  }
  return next({
    ctx: {
      user: ctx.user,
    },
  });
});

export const adminProcedure = t.procedure.use(isAdmin);

const isOperator = t.middleware(({ next, ctx }) => {
  const hasAccess = ctx.user?.roles?.some((role) =>
    ['org:admin', 'org:member'].includes(role)
  );

  if (!ctx.user || !hasAccess) {
    throw new TRPCError({ code: 'FORBIDDEN', message: 'Operator access required' });
  }
  return next({
    ctx: {
      user: ctx.user,
    },
  });
});

export const operatorProcedure = t.procedure.use(isOperator);

