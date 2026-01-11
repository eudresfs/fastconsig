import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { z } from 'zod'

export const consignatariasRouter = router({
  list: protectedProcedure
    .use(withPermission('CONSIGNATARIAS_VISUALIZAR'))
    .query(async ({ ctx }) => {
      const consignatarias = await ctx.prisma.tenantConsignataria.findMany({
        where: { tenantId: ctx.tenantId },
        include: {
          consignataria: true,
        },
        orderBy: { consignataria: { razaoSocial: 'asc' } },
      })

      return consignatarias
    }),

  getById: protectedProcedure
    .use(withPermission('CONSIGNATARIAS_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      const consignataria = await ctx.prisma.tenantConsignataria.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
        include: {
          consignataria: {
            include: {
              produtos: { where: { ativo: true } },
            },
          },
        },
      })

      if (!consignataria) {
        throw new Error('Consignataria nao encontrada')
      }

      return consignataria
    }),

  produtos: protectedProcedure
    .input(z.object({ consignatariaId: z.number() }))
    .query(async ({ input, ctx }) => {
      const tenantConsignataria = await ctx.prisma.tenantConsignataria.findFirst({
        where: { id: input.consignatariaId, tenantId: ctx.tenantId },
      })

      if (!tenantConsignataria) {
        throw new Error('Consignataria nao encontrada para este tenant')
      }

      const produtos = await ctx.prisma.produto.findMany({
        where: {
          consignatariaId: tenantConsignataria.consignatariaId,
          ativo: true,
        },
        orderBy: { nome: 'asc' },
      })

      return produtos
    }),

  tabelasCoeficiente: protectedProcedure
    .input(z.object({ produtoId: z.number() }))
    .query(async ({ input, ctx }) => {
      const tabelas = await ctx.prisma.tabelaCoeficiente.findMany({
        where: {
          produtoId: input.produtoId,
          ativo: true,
        },
        include: {
          itens: { orderBy: { prazo: 'asc' } },
        },
        orderBy: { dataVigenciaInicio: 'desc' },
      })

      return tabelas
    }),
})
