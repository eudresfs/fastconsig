import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { z } from 'zod'

export const auditoriaRouter = router({
  list: protectedProcedure
    .use(withPermission('AUDITORIA_VISUALIZAR'))
    .input(z.object({
      page: z.number().default(1),
      pageSize: z.number().default(50),
      entidade: z.string().optional(),
      acao: z.enum(['CRIAR', 'ATUALIZAR', 'EXCLUIR', 'LOGIN', 'LOGOUT', 'APROVAR', 'REJEITAR', 'SUSPENDER', 'CANCELAR', 'IMPORTAR', 'EXPORTAR']).optional(),
      usuarioId: z.number().optional(),
      dataInicio: z.coerce.date().optional(),
      dataFim: z.coerce.date().optional(),
    }))
    .query(async ({ input, ctx }) => {
      const skip = (input.page - 1) * input.pageSize

      const where = {
        tenantId: ctx.tenantId,
        ...(input.entidade && { entidade: input.entidade }),
        ...(input.acao && { acao: input.acao }),
        ...(input.usuarioId && { usuarioId: input.usuarioId }),
        ...(input.dataInicio && { createdAt: { gte: input.dataInicio } }),
        ...(input.dataFim && { createdAt: { lte: input.dataFim } }),
      }

      const [registros, total] = await Promise.all([
        ctx.prisma.auditoria.findMany({
          where,
          include: { usuario: { select: { nome: true, login: true } } },
          skip,
          take: input.pageSize,
          orderBy: { createdAt: 'desc' },
        }),
        ctx.prisma.auditoria.count({ where }),
      ])

      return {
        data: registros,
        pagination: { page: input.page, pageSize: input.pageSize, total, totalPages: Math.ceil(total / input.pageSize) },
      }
    }),

  getById: protectedProcedure
    .use(withPermission('AUDITORIA_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      const registro = await ctx.prisma.auditoria.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
        include: { usuario: { select: { nome: true, login: true, email: true } } },
      })

      if (!registro) {
        throw new Error('Registro de auditoria nao encontrado')
      }

      return registro
    }),

  entidades: protectedProcedure
    .use(withPermission('AUDITORIA_VISUALIZAR'))
    .query(async ({ ctx }) => {
      const entidades = await ctx.prisma.auditoria.findMany({
        where: { tenantId: ctx.tenantId },
        distinct: ['entidade'],
        select: { entidade: true },
      })

      return entidades.map((e) => e.entidade)
    }),
})
