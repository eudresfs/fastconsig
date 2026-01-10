import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { z } from 'zod'

export const relatoriosRouter = router({
  resumoGeral: protectedProcedure
    .use(withPermission('RELATORIOS_VISUALIZAR'))
    .query(async ({ ctx }) => {
      const [
        totalFuncionarios,
        funcionariosAtivos,
        totalAverbacoes,
        averbacoesPendentes,
        averbacoesMes,
      ] = await Promise.all([
        ctx.prisma.funcionario.count({ where: { tenantId: ctx.tenantId } }),
        ctx.prisma.funcionario.count({ where: { tenantId: ctx.tenantId, situacao: 'ATIVO' } }),
        ctx.prisma.averbacao.count({ where: { tenantId: ctx.tenantId } }),
        ctx.prisma.averbacao.count({ where: { tenantId: ctx.tenantId, situacao: 'AGUARDANDO_APROVACAO' } }),
        ctx.prisma.averbacao.count({
          where: {
            tenantId: ctx.tenantId,
            createdAt: { gte: new Date(new Date().getFullYear(), new Date().getMonth(), 1) },
          },
        }),
      ])

      const valorTotalAverbacoes = await ctx.prisma.averbacao.aggregate({
        where: { tenantId: ctx.tenantId, situacao: { in: ['APROVADA', 'ENVIADA', 'DESCONTADA'] } },
        _sum: { valorTotal: true },
      })

      const valorParcelasMes = await ctx.prisma.averbacao.aggregate({
        where: { tenantId: ctx.tenantId, situacao: 'DESCONTADA' },
        _sum: { valorParcela: true },
      })

      return {
        funcionarios: {
          total: totalFuncionarios,
          ativos: funcionariosAtivos,
        },
        averbacoes: {
          total: totalAverbacoes,
          pendentes: averbacoesPendentes,
          noMes: averbacoesMes,
          valorTotal: valorTotalAverbacoes._sum.valorTotal?.toNumber() ?? 0,
        },
        folha: {
          valorParcelasMes: valorParcelasMes._sum.valorParcela?.toNumber() ?? 0,
        },
      }
    }),

  averbacoesPorConsignataria: protectedProcedure
    .use(withPermission('RELATORIOS_VISUALIZAR'))
    .input(z.object({
      dataInicio: z.coerce.date().optional(),
      dataFim: z.coerce.date().optional(),
    }))
    .query(async ({ input, ctx }) => {
      const averbacoes = await ctx.prisma.averbacao.groupBy({
        by: ['tenantConsignatariaId'],
        where: {
          tenantId: ctx.tenantId,
          ...(input.dataInicio && { dataContrato: { gte: input.dataInicio } }),
          ...(input.dataFim && { dataContrato: { lte: input.dataFim } }),
        },
        _count: { id: true },
        _sum: { valorTotal: true, valorParcela: true },
      })

      const consignatariasIds = averbacoes.map((a) => a.tenantConsignatariaId)
      const consignatarias = await ctx.prisma.tenantConsignataria.findMany({
        where: { id: { in: consignatariasIds } },
        include: { consignataria: { select: { razaoSocial: true } } },
      })

      const consignatariasMap = new Map(consignatarias.map((c) => [c.id, c.consignataria.razaoSocial]))

      return averbacoes.map((a) => ({
        consignatariaId: a.tenantConsignatariaId,
        consignataria: consignatariasMap.get(a.tenantConsignatariaId) ?? 'Desconhecida',
        quantidade: a._count.id,
        valorTotal: a._sum.valorTotal?.toNumber() ?? 0,
        valorParcelas: a._sum.valorParcela?.toNumber() ?? 0,
      }))
    }),

  evolucaoMensal: protectedProcedure
    .use(withPermission('RELATORIOS_VISUALIZAR'))
    .input(z.object({ meses: z.number().default(12) }))
    .query(async ({ input, ctx }) => {
      const dataInicio = new Date()
      dataInicio.setMonth(dataInicio.getMonth() - input.meses)

      const averbacoes = await ctx.prisma.averbacao.findMany({
        where: {
          tenantId: ctx.tenantId,
          createdAt: { gte: dataInicio },
        },
        select: { createdAt: true, valorTotal: true, valorParcela: true },
      })

      const porMes = new Map<string, { quantidade: number; valorTotal: number; valorParcela: number }>()

      for (const av of averbacoes) {
        const mes = `${av.createdAt.getMonth() + 1}/${av.createdAt.getFullYear()}`
        const atual = porMes.get(mes) ?? { quantidade: 0, valorTotal: 0, valorParcela: 0 }
        porMes.set(mes, {
          quantidade: atual.quantidade + 1,
          valorTotal: atual.valorTotal + (av.valorTotal?.toNumber() ?? 0),
          valorParcela: atual.valorParcela + (av.valorParcela?.toNumber() ?? 0),
        })
      }

      return Array.from(porMes.entries())
        .map(([mes, dados]) => ({ mes, ...dados }))
        .sort((a, b) => {
          const [mesA, anoA] = a.mes.split('/').map(Number)
          const [mesB, anoB] = b.mes.split('/').map(Number)
          return anoA - anoB || mesA - mesB
        })
    }),
})
