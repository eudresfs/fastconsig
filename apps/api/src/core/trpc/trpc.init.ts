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
  // Check if user exists and has the required role
  if (!ctx.user || !ctx.user.roles?.includes('org:admin') && !ctx.user.roles?.includes('super_admin')) {
     // Note: 'org:admin' is the Clerk role we assigned. For MVP Super Admin, we might rely on a specific role or metadata.
     // Since the previous code used 'org:admin' in the invitation, let's assume 'org:admin' is what we have for now,
     // OR if we strictly want 'super_admin' we need to ensure that role exists.
     // The test mocked 'admin' as org_role.
     // Let's enforce that a user must exist and have appropriate permissions.
     // If the intention is strict 'super_admin' global role, we should enforce that.
     // Based on the code review finding: "Authorization Bypass".

     // STRICT CHECK:
     if (!ctx.user?.roles?.includes('super_admin')) {
        throw new TRPCError({ code: 'UNAUTHORIZED', message: 'Insufficient permissions' });
     }
  }

  return next({
    ctx: {
      user: ctx.user,
    },
  });
});

export const superAdminProcedure = t.procedure.use(isSuperAdmin);
