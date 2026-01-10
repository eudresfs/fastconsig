import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { z } from 'zod'

export const importacaoRouter = router({
  list: protectedProcedure
    .use(withPermission('IMPORTACAO_VISUALIZAR'))
    .input(z.object({
      page: z.number().default(1),
      pageSize: z.number().default(20),
      tipo: z.enum(['FUNCIONARIOS', 'CONTRATOS', 'RETORNO_FOLHA']).optional(),
      status: z.enum(['PENDENTE', 'PROCESSANDO', 'CONCLUIDO', 'ERRO', 'CANCELADO']).optional(),
    }))
    .query(async ({ input, ctx }) => {
      const skip = (input.page - 1) * input.pageSize

      const where = {
        tenantId: ctx.tenantId,
        ...(input.tipo && { tipo: input.tipo }),
        ...(input.status && { status: input.status }),
      }

      const [importacoes, total] = await Promise.all([
        ctx.prisma.importacao.findMany({
          where,
          include: { usuario: { select: { nome: true } } },
          skip,
          take: input.pageSize,
          orderBy: { createdAt: 'desc' },
        }),
        ctx.prisma.importacao.count({ where }),
      ])

      return {
        data: importacoes,
        pagination: { page: input.page, pageSize: input.pageSize, total, totalPages: Math.ceil(total / input.pageSize) },
      }
    }),

  getById: protectedProcedure
    .use(withPermission('IMPORTACAO_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      const importacao = await ctx.prisma.importacao.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
        include: { usuario: { select: { nome: true } } },
      })

      if (!importacao) {
        throw new Error('Importacao nao encontrada')
      }

      return importacao
    }),

  criar: protectedProcedure
    .use(withPermission('IMPORTACAO_CRIAR'))
    .input(z.object({
      tipo: z.enum(['FUNCIONARIOS', 'CONTRATOS', 'RETORNO_FOLHA']),
      nomeArquivo: z.string(),
      tamanhoBytes: z.number(),
    }))
    .mutation(async ({ input, ctx }) => {
      const importacao = await ctx.prisma.importacao.create({
        data: {
          tenantId: ctx.tenantId,
          tipo: input.tipo,
          nomeArquivo: input.nomeArquivo,
          tamanhoBytes: input.tamanhoBytes,
          status: 'PENDENTE',
          usuarioId: ctx.userId,
        },
      })

      // TODO: Adicionar job na fila do BullMQ para processar o arquivo

      return importacao
    }),

  cancelar: protectedProcedure
    .use(withPermission('IMPORTACAO_CRIAR'))
    .input(z.object({ id: z.number() }))
    .mutation(async ({ input, ctx }) => {
      const importacao = await ctx.prisma.importacao.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
      })

      if (!importacao) {
        throw new Error('Importacao nao encontrada')
      }

      if (importacao.status !== 'PENDENTE') {
        throw new Error('Apenas importacoes pendentes podem ser canceladas')
      }

      const atualizada = await ctx.prisma.importacao.update({
        where: { id: input.id },
        data: { status: 'CANCELADO' },
      })

      return atualizada
    }),
})
