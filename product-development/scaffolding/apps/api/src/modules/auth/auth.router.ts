import { router, publicProcedure, protectedProcedure } from '@/trpc/trpc'
import { loginSchema, refreshTokenSchema, alterarSenhaSchema } from './auth.schema'
import * as authService from './auth.service'

export const authRouter = router({
  login: publicProcedure
    .input(loginSchema)
    .mutation(async ({ input, ctx }) => {
      return authService.login(input, (payload, options) =>
        ctx.req.server.jwt.sign(payload, options)
      )
    }),

  refresh: publicProcedure
    .input(refreshTokenSchema)
    .mutation(async ({ input, ctx }) => {
      const sessao = await ctx.prisma.sessao.findUnique({
        where: { refreshToken: input.refreshToken },
        include: { usuario: true },
      })

      if (!sessao || sessao.expiresAt < new Date()) {
        throw new Error('Sessao expirada ou invalida')
      }

      const tokenPayload = {
        sub: sessao.usuario.id,
        tenantId: sessao.usuario.tenantId,
        consignatariaId: sessao.usuario.consignatariaId,
        perfilId: sessao.usuario.perfilId,
      }

      const accessToken = ctx.req.server.jwt.sign(tokenPayload, {
        expiresIn: '15m',
      })

      return { accessToken }
    }),

  alterarSenha: protectedProcedure
    .input(alterarSenhaSchema)
    .mutation(async ({ input, ctx }) => {
      await authService.alterarSenha(ctx.userId, input)
      return { success: true }
    }),

  logout: protectedProcedure
    .input(refreshTokenSchema)
    .mutation(async ({ input }) => {
      await authService.logout(input.refreshToken)
      return { success: true }
    }),

  me: protectedProcedure.query(async ({ ctx }) => {
    const usuario = await ctx.prisma.usuario.findUnique({
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
        tenant: true,
        consignataria: true,
      },
    })

    if (!usuario) {
      throw new Error('Usuario nao encontrado')
    }

    return {
      id: usuario.id,
      nome: usuario.nome,
      email: usuario.email,
      login: usuario.login,
      primeiroAcesso: usuario.primeiroAcesso,
      perfil: {
        id: usuario.perfil.id,
        nome: usuario.perfil.nome,
        tipo: usuario.perfil.tipo,
      },
      permissoes: usuario.perfil.permissoes.map((pp) => pp.permissao.codigo),
      tenant: usuario.tenant
        ? {
            id: usuario.tenant.id,
            nome: usuario.tenant.nome,
          }
        : null,
      consignataria: usuario.consignataria
        ? {
            id: usuario.consignataria.id,
            razaoSocial: usuario.consignataria.razaoSocial,
          }
        : null,
    }
  }),
})
