import { z } from 'zod'

export const averbacaoSchema = z.object({
  funcionarioId: z.number().int().positive(),
  tenantConsignatariaId: z.number().int().positive(),
  produtoId: z.number().int().positive(),
  numeroContrato: z.string().min(1).max(30),
  tipoOperacao: z.enum(['NOVO', 'REFINANCIAMENTO', 'COMPRA_DIVIDA']).default('NOVO'),
  valorTotal: z.coerce.number().positive(),
  valorLiquido: z.coerce.number().positive().optional(),
  valorParcela: z.coerce.number().positive(),
  parcelasTotal: z.number().int().positive(),
  taxaMensal: z.coerce.number().positive(),
  taxaAnual: z.coerce.number().positive().optional(),
  cetMensal: z.coerce.number().positive().optional(),
  cetAnual: z.coerce.number().positive().optional(),
  iof: z.coerce.number().nonnegative().optional(),
  tac: z.coerce.number().nonnegative().optional(),
  dataContrato: z.coerce.date(),
  dataInicioDesconto: z.coerce.date(),
  dataFimDesconto: z.coerce.date(),
  averbacaoVinculadaId: z.number().int().positive().optional(),
  observacao: z.string().optional(),
})

export const averbacaoFiltroSchema = z.object({
  page: z.number().int().positive().default(1),
  pageSize: z.number().int().positive().max(100).default(20),
  search: z.string().optional(),
  situacao: z
    .enum([
      'AGUARDANDO_APROVACAO',
      'APROVADA',
      'REJEITADA',
      'ENVIADA',
      'DESCONTADA',
      'SUSPENSA',
      'BLOQUEADA',
      'LIQUIDADA',
      'CANCELADA',
    ])
    .optional(),
  funcionarioId: z.number().int().positive().optional(),
  tenantConsignatariaId: z.number().int().positive().optional(),
  dataInicio: z.coerce.date().optional(),
  dataFim: z.coerce.date().optional(),
  orderBy: z.enum(['numeroContrato', 'dataContrato', 'valorTotal', 'createdAt']).default('createdAt'),
  orderDir: z.enum(['asc', 'desc']).default('desc'),
})

export const aprovarAverbacaoSchema = z.object({
  id: z.number().int().positive(),
  observacao: z.string().optional(),
})

export const rejeitarAverbacaoSchema = z.object({
  id: z.number().int().positive(),
  motivoRejeicao: z.string().min(10, 'Motivo deve ter no minimo 10 caracteres'),
})

export type AverbacaoInput = z.infer<typeof averbacaoSchema>
export type AverbacaoFiltro = z.infer<typeof averbacaoFiltroSchema>
