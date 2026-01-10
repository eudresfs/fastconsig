import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { z } from 'zod'

export const margemRouter = router({
  calcular: protectedProcedure
    .use(withPermission('MARGEM_VISUALIZAR'))
    .input(z.object({ funcionarioId: z.number() }))
    .query(async ({ input, ctx }) => {
      const funcionario = await ctx.prisma.funcionario.findFirst({
        where: { id: input.funcionarioId, tenantId: ctx.tenantId },
      })

      if (!funcionario) {
        throw new Error('Funcionario nao encontrado')
      }

      const config = await ctx.prisma.tenantConfiguracao.findUnique({
        where: { tenantId: ctx.tenantId },
      })

      const percentualMargem = config?.percentualMargem ?? 35
      const percentualEmprestimo = config?.percentualEmprestimo ?? 30
      const percentualCartao = config?.percentualCartao ?? 5

      const margemTotal = funcionario.salarioBruto.toNumber() * (Number(percentualMargem) / 100)
      const margemEmprestimo = funcionario.salarioBruto.toNumber() * (Number(percentualEmprestimo) / 100)
      const margemCartao = funcionario.salarioBruto.toNumber() * (Number(percentualCartao) / 100)

      // Buscar averbacoes ativas para calcular margem utilizada
      const averbacoes = await ctx.prisma.averbacao.findMany({
        where: {
          funcionarioId: input.funcionarioId,
          situacao: { in: ['APROVADA', 'ENVIADA', 'DESCONTADA'] },
        },
        include: { produto: true },
      })

      let utilizadaEmprestimo = 0
      let utilizadaCartao = 0

      for (const av of averbacoes) {
        if (av.produto.tipo === 'CARTAO') {
          utilizadaCartao += av.valorParcela.toNumber()
        } else {
          utilizadaEmprestimo += av.valorParcela.toNumber()
        }
      }

      return {
        funcionarioId: funcionario.id,
        salarioBruto: funcionario.salarioBruto.toNumber(),
        margemTotal,
        emprestimo: {
          percentual: Number(percentualEmprestimo),
          total: margemEmprestimo,
          utilizada: utilizadaEmprestimo,
          disponivel: Math.max(0, margemEmprestimo - utilizadaEmprestimo),
        },
        cartao: {
          percentual: Number(percentualCartao),
          total: margemCartao,
          utilizada: utilizadaCartao,
          disponivel: Math.max(0, margemCartao - utilizadaCartao),
        },
        totalUtilizada: utilizadaEmprestimo + utilizadaCartao,
        totalDisponivel: Math.max(0, margemTotal - utilizadaEmprestimo - utilizadaCartao),
      }
    }),

  historico: protectedProcedure
    .use(withPermission('MARGEM_VISUALIZAR'))
    .input(z.object({
      funcionarioId: z.number(),
      limit: z.number().default(12),
    }))
    .query(async ({ input, ctx }) => {
      const historico = await ctx.prisma.margemHistorico.findMany({
        where: { funcionarioId: input.funcionarioId },
        orderBy: { competencia: 'desc' },
        take: input.limit,
      })

      return historico
    }),

  reservar: protectedProcedure
    .use(withPermission('MARGEM_RESERVAR'))
    .input(z.object({
      funcionarioId: z.number(),
      valor: z.number().positive(),
      motivo: z.string().optional(),
    }))
    .mutation(async ({ input, ctx }) => {
      // Implementar logica de reserva de margem
      // Esta e uma operacao temporaria que bloqueia parte da margem
      const margem = await ctx.prisma.funcionario.findFirst({
        where: { id: input.funcionarioId, tenantId: ctx.tenantId },
      })

      if (!margem) {
        throw new Error('Funcionario nao encontrado')
      }

      // TODO: Implementar tabela de reservas de margem
      return { success: true, message: 'Margem reservada com sucesso' }
    }),
})
