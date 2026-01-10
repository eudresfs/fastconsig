import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { z } from 'zod'

export const conciliacaoRouter = router({
  list: protectedProcedure
    .use(withPermission('CONCILIACAO_VISUALIZAR'))
    .input(z.object({
      page: z.number().default(1),
      pageSize: z.number().default(20),
      status: z.enum(['ABERTA', 'EM_ANDAMENTO', 'FECHADA']).optional(),
    }))
    .query(async ({ input, ctx }) => {
      const skip = (input.page - 1) * input.pageSize

      const where = {
        tenantId: ctx.tenantId,
        ...(input.status && { status: input.status }),
      }

      const [conciliacoes, total] = await Promise.all([
        ctx.prisma.conciliacao.findMany({
          where,
          skip,
          take: input.pageSize,
          orderBy: { competencia: 'desc' },
        }),
        ctx.prisma.conciliacao.count({ where }),
      ])

      return {
        data: conciliacoes,
        pagination: { page: input.page, pageSize: input.pageSize, total, totalPages: Math.ceil(total / input.pageSize) },
      }
    }),

  getById: protectedProcedure
    .use(withPermission('CONCILIACAO_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      const conciliacao = await ctx.prisma.conciliacao.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
        include: {
          itens: {
            include: {
              averbacao: {
                include: {
                  funcionario: { select: { nome: true, cpf: true } },
                  tenantConsignataria: { include: { consignataria: { select: { razaoSocial: true } } } },
                },
              },
            },
          },
        },
      })

      if (!conciliacao) {
        throw new Error('Conciliacao nao encontrada')
      }

      return conciliacao
    }),

  criar: protectedProcedure
    .use(withPermission('CONCILIACAO_CRIAR'))
    .input(z.object({ competencia: z.string().regex(/^\d{2}\/\d{4}$/) }))
    .mutation(async ({ input, ctx }) => {
      const existente = await ctx.prisma.conciliacao.findFirst({
        where: { tenantId: ctx.tenantId, competencia: input.competencia },
      })

      if (existente) {
        throw new Error('Ja existe uma conciliacao para esta competencia')
      }

      // Buscar averbacoes que devem ser descontadas nesta competencia
      const [mes, ano] = input.competencia.split('/')
      const dataReferencia = new Date(parseInt(ano), parseInt(mes) - 1, 1)

      const averbacoes = await ctx.prisma.averbacao.findMany({
        where: {
          tenantId: ctx.tenantId,
          situacao: { in: ['APROVADA', 'ENVIADA', 'DESCONTADA'] },
          dataInicioDesconto: { lte: dataReferencia },
          dataFimDesconto: { gte: dataReferencia },
        },
      })

      const totalEnviado = averbacoes.reduce((sum, av) => sum + av.valorParcela.toNumber(), 0)

      const conciliacao = await ctx.prisma.conciliacao.create({
        data: {
          tenantId: ctx.tenantId,
          competencia: input.competencia,
          status: 'ABERTA',
          totalEnviado,
          totalDescontado: 0,
          totalDivergente: 0,
          qtdEnviados: averbacoes.length,
          qtdDescontados: 0,
          qtdDivergentes: 0,
        },
      })

      // Criar itens da conciliacao
      await ctx.prisma.conciliacaoItem.createMany({
        data: averbacoes.map((av) => ({
          conciliacaoId: conciliacao.id,
          averbacaoId: av.id,
          valorEnviado: av.valorParcela,
          valorDescontado: 0,
          status: 'NAO_DESCONTADO',
        })),
      })

      return conciliacao
    }),

  fechar: protectedProcedure
    .use(withPermission('CONCILIACAO_FECHAR'))
    .input(z.object({ id: z.number() }))
    .mutation(async ({ input, ctx }) => {
      const conciliacao = await ctx.prisma.conciliacao.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
      })

      if (!conciliacao) {
        throw new Error('Conciliacao nao encontrada')
      }

      if (conciliacao.status === 'FECHADA') {
        throw new Error('Conciliacao ja esta fechada')
      }

      const atualizada = await ctx.prisma.conciliacao.update({
        where: { id: input.id },
        data: {
          status: 'FECHADA',
          dataFechamento: new Date(),
          usuarioFechamentoId: ctx.userId,
        },
      })

      return atualizada
    }),
})
