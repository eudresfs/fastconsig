export interface PaginationParams {
  page: number
  pageSize: number
}

export interface PaginatedResponse<T> {
  data: T[]
  pagination: {
    page: number
    pageSize: number
    total: number
    totalPages: number
  }
}

export interface ApiError {
  code: string
  message: string
  details?: Record<string, unknown>
}

export type SituacaoFuncionario = 'ATIVO' | 'INATIVO' | 'AFASTADO' | 'BLOQUEADO' | 'APOSENTADO'

export type SituacaoAverbacao =
  | 'AGUARDANDO_APROVACAO'
  | 'APROVADA'
  | 'REJEITADA'
  | 'ENVIADA'
  | 'DESCONTADA'
  | 'SUSPENSA'
  | 'BLOQUEADA'
  | 'LIQUIDADA'
  | 'CANCELADA'

export type TipoOperacao = 'NOVO' | 'REFINANCIAMENTO' | 'COMPRA_DIVIDA'

export type TipoProduto = 'EMPRESTIMO' | 'CARTAO' | 'SEGURO'

export type TipoPerfil = 'SISTEMA' | 'CONSIGNANTE' | 'CONSIGNATARIA' | 'AGENTE'

export type StatusConciliacao = 'ABERTA' | 'EM_ANDAMENTO' | 'FECHADA'

export type StatusImportacao = 'PENDENTE' | 'PROCESSANDO' | 'CONCLUIDO' | 'ERRO' | 'CANCELADO'

export type TipoImportacao = 'FUNCIONARIOS' | 'CONTRATOS' | 'RETORNO_FOLHA'

export interface Margem {
  funcionarioId: number
  salarioBruto: number
  margemTotal: number
  emprestimo: {
    percentual: number
    total: number
    utilizada: number
    disponivel: number
  }
  cartao: {
    percentual: number
    total: number
    utilizada: number
    disponivel: number
  }
  totalUtilizada: number
  totalDisponivel: number
}

export interface SimulacaoResult {
  prazo: number
  valorTotal: number
  valorLiquido: number
  valorParcela: number
  taxaMensal: number
  taxaAnual: number
  cetMensal: number
  cetAnual: number
  iofEstimado: number
  tacEstimado: number
  coeficiente: number
  margemDisponivel: number
  parcelaCabeNaMargem: boolean
  produto: {
    id: number
    nome: string
    tipo: TipoProduto
  }
}
