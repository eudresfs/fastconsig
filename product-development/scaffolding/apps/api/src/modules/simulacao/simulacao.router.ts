import { router, protectedProcedure } from '@/trpc/trpc'
import { z } from 'zod'

const simulacaoInputSchema = z.object({
  funcionarioId: z.number().optional(),
  salarioBruto: z.number().positive().optional(),
  valorSolicitado: z.number().positive().optional(),
  valorParcela: z.number().positive().optional(),
  prazo: z.number().int().positive(),
  produtoId: z.number().int().positive(),
  tabelaCoeficienteId: z.number().int().positive().optional(),
})

export const simulacaoRouter = router({
  simular: protectedProcedure
    .input(simulacaoInputSchema)
    .mutation(async ({ input, ctx }) => {
      let salarioBruto = input.salarioBruto

      // Se informou funcionarioId, buscar salario
      if (input.funcionarioId) {
        const funcionario = await ctx.prisma.funcionario.findFirst({
          where: { id: input.funcionarioId, tenantId: ctx.tenantId },
        })
        if (funcionario) {
          salarioBruto = funcionario.salarioBruto.toNumber()
        }
      }

      if (!salarioBruto) {
        throw new Error('Salario bruto e obrigatorio')
      }

      // Buscar produto e tabela de coeficientes
      const produto = await ctx.prisma.produto.findUnique({
        where: { id: input.produtoId },
        include: {
          coeficientes: {
            where: { ativo: true },
            include: {
              itens: { where: { prazo: input.prazo } },
            },
          },
        },
      })

      if (!produto) {
        throw new Error('Produto nao encontrado')
      }

      // Encontrar coeficiente para o prazo
      let coeficiente: number | null = null
      let taxaMensal: number | null = null

      for (const tabela of produto.coeficientes) {
        const item = tabela.itens.find((i) => i.prazo === input.prazo)
        if (item) {
          coeficiente = item.coeficiente.toNumber()
          taxaMensal = item.taxaMensal.toNumber()
          break
        }
      }

      if (!coeficiente || !taxaMensal) {
        throw new Error('Coeficiente nao encontrado para o prazo informado')
      }

      // Calcular valores
      let valorTotal: number
      let valorParcela: number
      let valorLiquido: number

      if (input.valorSolicitado) {
        valorTotal = input.valorSolicitado
        valorParcela = valorTotal * coeficiente
        valorLiquido = valorTotal * 0.95 // Deduzir IOF e TAC estimados
      } else if (input.valorParcela) {
        valorParcela = input.valorParcela
        valorTotal = valorParcela / coeficiente
        valorLiquido = valorTotal * 0.95
      } else {
        throw new Error('Informe o valor solicitado ou o valor da parcela')
      }

      // Verificar margem
      const config = await ctx.prisma.tenantConfiguracao.findUnique({
        where: { tenantId: ctx.tenantId },
      })

      const percentualMargem = produto.tipo === 'CARTAO'
        ? (config?.percentualCartao ?? 5)
        : (config?.percentualEmprestimo ?? 30)

      const margemDisponivel = salarioBruto * (Number(percentualMargem) / 100)
      const parcelaCabeNaMargem = valorParcela <= margemDisponivel

      // Calcular IOF e CET estimados
      const iofDiario = 0.0082 / 100
      const iofAdicional = 0.38 / 100
      const diasOperacao = input.prazo * 30
      const iofEstimado = valorTotal * (iofDiario * Math.min(diasOperacao, 365) + iofAdicional)
      const tacEstimado = valorTotal * 0.02

      const taxaAnual = Math.pow(1 + taxaMensal / 100, 12) - 1
      const cetMensal = taxaMensal * 1.1 // CET estimado (inclui IOF e TAC)
      const cetAnual = Math.pow(1 + cetMensal / 100, 12) - 1

      return {
        prazo: input.prazo,
        valorTotal: Math.round(valorTotal * 100) / 100,
        valorLiquido: Math.round(valorLiquido * 100) / 100,
        valorParcela: Math.round(valorParcela * 100) / 100,
        taxaMensal: Math.round(taxaMensal * 10000) / 10000,
        taxaAnual: Math.round(taxaAnual * 10000) / 10000,
        cetMensal: Math.round(cetMensal * 10000) / 10000,
        cetAnual: Math.round(cetAnual * 10000) / 10000,
        iofEstimado: Math.round(iofEstimado * 100) / 100,
        tacEstimado: Math.round(tacEstimado * 100) / 100,
        coeficiente,
        margemDisponivel,
        parcelaCabeNaMargem,
        produto: {
          id: produto.id,
          nome: produto.nome,
          tipo: produto.tipo,
        },
      }
    }),

  prazosDisponiveis: protectedProcedure
    .input(z.object({ produtoId: z.number() }))
    .query(async ({ input, ctx }) => {
      const tabelas = await ctx.prisma.tabelaCoeficiente.findMany({
        where: {
          produtoId: input.produtoId,
          ativo: true,
        },
        include: {
          itens: {
            orderBy: { prazo: 'asc' },
          },
        },
      })

      const prazos = new Set<number>()
      for (const tabela of tabelas) {
        for (const item of tabela.itens) {
          prazos.add(item.prazo)
        }
      }

      return Array.from(prazos).sort((a, b) => a - b)
    }),
})
