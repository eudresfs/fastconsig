import { initTRPC, TRPCError } from '@trpc/server'
import superjson from 'superjson'
import { type Context } from './context'

const t = initTRPC.context<Context>().create({
  transformer: superjson,
  errorFormatter({ shape }) {
    return shape
  },
})

export const router = t.router
export const publicProcedure = t.procedure
export const middleware = t.middleware

// Middleware de autenticacao
const isAuthenticated = middleware(async ({ ctx, next }) => {
  if (!ctx.userId || !ctx.tenantId) {
    throw new TRPCError({
      code: 'UNAUTHORIZED',
      message: 'Voce precisa estar autenticado para acessar este recurso',
    })
  }

  return next({
    ctx: {
      ...ctx,
      userId: ctx.userId,
      tenantId: ctx.tenantId,
    },
  })
})

export const protectedProcedure = t.procedure.use(isAuthenticated)

// Middleware de permissao
export const withPermission = (permissionCode: string) =>
  middleware(async ({ ctx, next }) => {
    if (!ctx.userId) {
      throw new TRPCError({
        code: 'UNAUTHORIZED',
        message: 'Voce precisa estar autenticado',
      })
    }

    const userWithPermissions = await ctx.prisma.usuario.findUnique({
      where: { id: ctx.userId },
      include: {
        perfil: {
          include: {
            permissoes: {
              include: {
                permissao: true,
              },
            },
          },
        },
      },
    })

    const hasPermission = userWithPermissions?.perfil.permissoes.some(
      (pp) => pp.permissao.codigo === permissionCode
    )

    if (!hasPermission) {
      throw new TRPCError({
        code: 'FORBIDDEN',
        message: 'Voce nao tem permissao para acessar este recurso',
      })
    }

    return next({ ctx })
  })
