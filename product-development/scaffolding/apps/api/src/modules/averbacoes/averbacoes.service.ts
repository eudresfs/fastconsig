import { TRPCError } from '@trpc/server'
import { prisma } from '@fastconsig/database/client'
import type { Prisma, SituacaoAverbacao } from '@prisma/client'
import type { Context } from '@/trpc/context'
import {
  logAuditAction,
  AuditActions,
  computeAuditDiff,
  withTenantFilter,
} from '@/shared/middleware'
import {
  NotFoundError,
  ConflictError,
  BusinessError,
  MargemInsuficienteError,
} from '@/shared/errors'
import type { AverbacaoInput, AverbacaoFiltro } from './averbacoes.schema'
import {
  validateTransition,
  isEditableState,
  consumesMargem,
  isTerminalState,
  getAllowedTransitions,
  getTransitionActionName,
  ESTADOS_QUE_CONSOMEM_MARGEM,
} from './averbacoes.state-machine'
import { calcularMargem, verificarMargemDisponivel } from '../funcionarios/funcionarios.service'

/**
 * Averbacao with related data
 */
export interface AverbacaoCompleta {
  id: number
  tenantId: number
  funcionarioId: number
  funcionario: {
    id: number
    nome: string
    cpf: string
    matricula: string
  }
  tenantConsignatariaId: number
  tenantConsignataria: {
    id: number
    codigo: string
    consignataria: {
      id: number
      razaoSocial: string
      nomeFantasia: string | null
    }
  }
  produtoId: number
  produto: {
    id: number
    codigo: string
    nome: string
    tipo: string
  }
  numeroContrato: string
  tipoOperacao: string
  situacao: SituacaoAverbacao
  valorTotal: number
  valorLiquido: number | null
  valorParcela: number
  parcelasTotal: number
  parcelasPagas: number
  taxaMensal: number
  taxaAnual: number | null
  cetMensal: number | null
  cetAnual: number | null
  iof: number | null
  tac: number | null
  saldoDevedor: number | null
  dataContrato: Date
  dataInicioDesconto: Date
  dataFimDesconto: Date
  averbacaoVinculadaId: number | null
  motivoRejeicao: string | null
  observacao: string | null
  dataAprovacao: Date | null
  createdAt: Date
  updatedAt: Date
  transicoesDisponiveis: SituacaoAverbacao[]
}

/**
 * Paginated response
 */
export interface PaginatedResponse<T> {
  data: T[]
  pagination: {
    page: number
    pageSize: number
    total: number
    totalPages: number
  }
}

/**
 * Margin reservation result
 */
export interface ReservaMargemResult {
  sucesso: boolean
  margemDisponivel: number
  margemRequerida: number
  mensagem: string
}

/**
 * Maps averbacao from database to API response
 */
function mapAverbacaoToResponse(
  averbacao: Prisma.AverbacaoGetPayload<{
    include: {
      funcionario: { select: { id: true; nome: true; cpf: true; matricula: true } }
      tenantConsignataria: {
        include: {
          consignataria: { select: { id: true; razaoSocial: true; nomeFantasia: true } }
        }
      }
      produto: { select: { id: true; codigo: true; nome: true; tipo: true } }
    }
  }>
): AverbacaoCompleta {
  return {
    id: averbacao.id,
    tenantId: averbacao.tenantId,
    funcionarioId: averbacao.funcionarioId,
    funcionario: averbacao.funcionario,
    tenantConsignatariaId: averbacao.tenantConsignatariaId,
    tenantConsignataria: {
      id: averbacao.tenantConsignataria.id,
      codigo: averbacao.tenantConsignataria.codigo,
      consignataria: averbacao.tenantConsignataria.consignataria,
    },
    produtoId: averbacao.produtoId,
    produto: averbacao.produto,
    numeroContrato: averbacao.numeroContrato,
    tipoOperacao: averbacao.tipoOperacao,
    situacao: averbacao.situacao,
    valorTotal: Number(averbacao.valorTotal),
    valorLiquido: averbacao.valorLiquido ? Number(averbacao.valorLiquido) : null,
    valorParcela: Number(averbacao.valorParcela),
    parcelasTotal: averbacao.parcelasTotal,
    parcelasPagas: averbacao.parcelasPagas,
    taxaMensal: Number(averbacao.taxaMensal),
    taxaAnual: averbacao.taxaAnual ? Number(averbacao.taxaAnual) : null,
    cetMensal: averbacao.cetMensal ? Number(averbacao.cetMensal) : null,
    cetAnual: averbacao.cetAnual ? Number(averbacao.cetAnual) : null,
    iof: averbacao.iof ? Number(averbacao.iof) : null,
    tac: averbacao.tac ? Number(averbacao.tac) : null,
    saldoDevedor: averbacao.saldoDevedor ? Number(averbacao.saldoDevedor) : null,
    dataContrato: averbacao.dataContrato,
    dataInicioDesconto: averbacao.dataInicioDesconto,
    dataFimDesconto: averbacao.dataFimDesconto,
    averbacaoVinculadaId: averbacao.averbacaoVinculadaId,
    motivoRejeicao: averbacao.motivoRejeicao,
    observacao: averbacao.observacao,
    dataAprovacao: averbacao.dataAprovacao,
    createdAt: averbacao.createdAt,
    updatedAt: averbacao.updatedAt,
    transicoesDisponiveis: getAllowedTransitions(averbacao.situacao),
  }
}

/**
 * Include clause for averbacao queries
 */
const averbacaoInclude = {
  funcionario: {
    select: { id: true, nome: true, cpf: true, matricula: true },
  },
  tenantConsignataria: {
    include: {
      consignataria: {
        select: { id: true, razaoSocial: true, nomeFantasia: true },
      },
    },
  },
  produto: {
    select: { id: true, codigo: true, nome: true, tipo: true },
  },
} as const

/**
 * Lists averbacoes with pagination and filtering
 */
export async function listar(
  tenantId: number,
  filtro: AverbacaoFiltro,
  consignatariaId?: number | null
): Promise<PaginatedResponse<AverbacaoCompleta>> {
  const {
    page,
    pageSize,
    search,
    situacao,
    funcionarioId,
    tenantConsignatariaId,
    dataInicio,
    dataFim,
    orderBy,
    orderDir,
  } = filtro

  const where: Prisma.AverbacaoWhereInput = {
    tenantId,
    ...(situacao && { situacao }),
    ...(funcionarioId && { funcionarioId }),
    ...(tenantConsignatariaId && { tenantConsignatariaId }),
    ...(dataInicio && { dataContrato: { gte: dataInicio } }),
    ...(dataFim && { dataContrato: { lte: dataFim } }),
    ...(search && {
      OR: [
        { numeroContrato: { contains: search, mode: 'insensitive' } },
        { funcionario: { nome: { contains: search, mode: 'insensitive' } } },
        { funcionario: { cpf: { contains: search } } },
      ],
    }),
    // Filter by consignataria if user is from a consignataria
    ...(consignatariaId && {
      tenantConsignataria: { consignatariaId },
    }),
  }

  const [averbacoes, total] = await Promise.all([
    prisma.averbacao.findMany({
      where,
      skip: (page - 1) * pageSize,
      take: pageSize,
      orderBy: { [orderBy]: orderDir },
      include: averbacaoInclude,
    }),
    prisma.averbacao.count({ where }),
  ])

  return {
    data: averbacoes.map(mapAverbacaoToResponse),
    pagination: {
      page,
      pageSize,
      total,
      totalPages: Math.ceil(total / pageSize),
    },
  }
}

/**
 * Gets an averbacao by ID
 */
export async function buscarPorId(
  tenantId: number,
  id: number,
  consignatariaId?: number | null
): Promise<AverbacaoCompleta> {
  const where: Prisma.AverbacaoWhereInput = {
    id,
    tenantId,
    ...(consignatariaId && {
      tenantConsignataria: { consignatariaId },
    }),
  }

  const averbacao = await prisma.averbacao.findFirst({
    where,
    include: averbacaoInclude,
  })

  if (!averbacao) {
    throw new NotFoundError('Averbacao', id)
  }

  return mapAverbacaoToResponse(averbacao)
}

/**
 * Gets an averbacao by contract number
 */
export async function buscarPorNumeroContrato(
  tenantId: number,
  numeroContrato: string
): Promise<AverbacaoCompleta | null> {
  const averbacao = await prisma.averbacao.findFirst({
    where: withTenantFilter(tenantId, { numeroContrato }),
    include: averbacaoInclude,
  })

  if (!averbacao) {
    return null
  }

  return mapAverbacaoToResponse(averbacao)
}

/**
 * Reserves margin for a new averbacao
 */
export async function reservarMargem(
  tenantId: number,
  funcionarioId: number,
  valorParcela: number
): Promise<ReservaMargemResult> {
  const verificacao = await verificarMargemDisponivel(
    tenantId,
    funcionarioId,
    valorParcela
  )

  if (!verificacao.disponivel) {
    return {
      sucesso: false,
      margemDisponivel: verificacao.margemDisponivel,
      margemRequerida: verificacao.margemRequerida,
      mensagem: `Margem insuficiente. Disponivel: R$ ${verificacao.margemDisponivel.toFixed(2)}, Requerida: R$ ${verificacao.margemRequerida.toFixed(2)}`,
    }
  }

  return {
    sucesso: true,
    margemDisponivel: verificacao.margemDisponivel,
    margemRequerida: verificacao.margemRequerida,
    mensagem: 'Margem reservada com sucesso',
  }
}

/**
 * Creates a new averbacao
 */
export async function criar(
  tenantId: number,
  input: AverbacaoInput,
  usuarioId: number,
  ctx?: Context
): Promise<AverbacaoCompleta> {
  // Verify funcionario exists and belongs to tenant
  const funcionario = await prisma.funcionario.findFirst({
    where: withTenantFilter(tenantId, { id: input.funcionarioId }),
    select: { id: true, salarioBruto: true, situacao: true },
  })

  if (!funcionario) {
    throw new NotFoundError('Funcionario', input.funcionarioId)
  }

  if (funcionario.situacao !== 'ATIVO') {
    throw new BusinessError('Funcionario nao esta ativo', {
      code: 'FUNCIONARIO_INATIVO',
      details: { situacao: funcionario.situacao },
    })
  }

  // Verify tenantConsignataria exists
  const tenantConsignataria = await prisma.tenantConsignataria.findFirst({
    where: {
      id: input.tenantConsignatariaId,
      tenantId,
      situacao: 'ATIVO',
    },
  })

  if (!tenantConsignataria) {
    throw new NotFoundError('Consignataria', input.tenantConsignatariaId)
  }

  // Verify produto exists and belongs to consignataria
  const produto = await prisma.produto.findFirst({
    where: {
      id: input.produtoId,
      consignatariaId: tenantConsignataria.consignatariaId,
      ativo: true,
    },
  })

  if (!produto) {
    throw new NotFoundError('Produto', input.produtoId)
  }

  // Check for duplicate contract number
  const existingContract = await prisma.averbacao.findFirst({
    where: { tenantId, numeroContrato: input.numeroContrato },
  })

  if (existingContract) {
    throw new ConflictError(
      `Ja existe uma averbacao com o numero de contrato ${input.numeroContrato}`
    )
  }

  // Verify margin availability
  const margemCheck = await reservarMargem(
    tenantId,
    input.funcionarioId,
    input.valorParcela
  )

  if (!margemCheck.sucesso) {
    throw new MargemInsuficienteError(
      margemCheck.margemDisponivel,
      margemCheck.margemRequerida,
      { funcionarioId: input.funcionarioId }
    )
  }

  // Verify linked averbacao if refinancing
  if (input.tipoOperacao !== 'NOVO' && input.averbacaoVinculadaId) {
    const averbacaoVinculada = await prisma.averbacao.findFirst({
      where: {
        id: input.averbacaoVinculadaId,
        tenantId,
        funcionarioId: input.funcionarioId,
        situacao: { in: ['APROVADA', 'ENVIADA', 'DESCONTADA'] },
      },
    })

    if (!averbacaoVinculada) {
      throw new BusinessError(
        'Averbacao vinculada nao encontrada ou nao esta em situacao valida para refinanciamento',
        { code: 'AVERBACAO_VINCULADA_INVALIDA' }
      )
    }
  }

  // Calculate annual rate if not provided
  const taxaAnual = input.taxaAnual ?? Math.pow(1 + input.taxaMensal / 100, 12) * 100 - 100

  // Create averbacao
  const averbacao = await prisma.averbacao.create({
    data: {
      tenantId,
      funcionarioId: input.funcionarioId,
      tenantConsignatariaId: input.tenantConsignatariaId,
      produtoId: input.produtoId,
      numeroContrato: input.numeroContrato,
      tipoOperacao: input.tipoOperacao ?? 'NOVO',
      situacao: 'AGUARDANDO_APROVACAO',
      valorTotal: input.valorTotal,
      valorLiquido: input.valorLiquido ?? null,
      valorParcela: input.valorParcela,
      parcelasTotal: input.parcelasTotal,
      parcelasPagas: 0,
      taxaMensal: input.taxaMensal,
      taxaAnual,
      cetMensal: input.cetMensal ?? null,
      cetAnual: input.cetAnual ?? null,
      iof: input.iof ?? null,
      tac: input.tac ?? null,
      saldoDevedor: input.valorTotal,
      dataContrato: input.dataContrato,
      dataInicioDesconto: input.dataInicioDesconto,
      dataFimDesconto: input.dataFimDesconto,
      averbacaoVinculadaId: input.averbacaoVinculadaId ?? null,
      observacao: input.observacao ?? null,
      usuarioCriacaoId: usuarioId,
    },
    include: averbacaoInclude,
  })

  // Create history entry
  await prisma.averbacaoHistorico.create({
    data: {
      averbacaoId: averbacao.id,
      situacaoAnterior: null,
      situacaoNova: 'AGUARDANDO_APROVACAO',
      usuarioId,
      observacao: 'Averbacao criada',
    },
  })

  // Log audit action
  if (ctx) {
    await logAuditAction(ctx, {
      entidade: 'Averbacao',
      entidadeId: averbacao.id,
      acao: AuditActions.CRIAR,
      dadosNovos: {
        numeroContrato: input.numeroContrato,
        funcionarioId: input.funcionarioId,
        valorTotal: input.valorTotal,
        valorParcela: input.valorParcela,
        parcelasTotal: input.parcelasTotal,
      },
    })
  }

  return mapAverbacaoToResponse(averbacao)
}

/**
 * Updates an averbacao (only allowed in editable states)
 */
export async function atualizar(
  tenantId: number,
  id: number,
  input: Partial<AverbacaoInput>,
  ctx?: Context
): Promise<AverbacaoCompleta> {
  const existing = await prisma.averbacao.findFirst({
    where: withTenantFilter(tenantId, { id }),
  })

  if (!existing) {
    throw new NotFoundError('Averbacao', id)
  }

  if (!isEditableState(existing.situacao)) {
    throw new BusinessError(
      `Averbacao em situacao '${existing.situacao}' nao pode ser editada`,
      { code: 'AVERBACAO_NAO_EDITAVEL' }
    )
  }

  // If changing valorParcela, verify margin
  if (input.valorParcela && input.valorParcela !== Number(existing.valorParcela)) {
    const diferencaParcela = input.valorParcela - Number(existing.valorParcela)

    if (diferencaParcela > 0) {
      const margemCheck = await reservarMargem(
        tenantId,
        existing.funcionarioId,
        diferencaParcela
      )

      if (!margemCheck.sucesso) {
        throw new MargemInsuficienteError(
          margemCheck.margemDisponivel,
          diferencaParcela,
          { funcionarioId: existing.funcionarioId }
        )
      }
    }
  }

  const averbacao = await prisma.averbacao.update({
    where: { id },
    data: {
      valorTotal: input.valorTotal ?? undefined,
      valorLiquido: input.valorLiquido ?? undefined,
      valorParcela: input.valorParcela ?? undefined,
      parcelasTotal: input.parcelasTotal ?? undefined,
      taxaMensal: input.taxaMensal ?? undefined,
      taxaAnual: input.taxaAnual ?? undefined,
      cetMensal: input.cetMensal ?? undefined,
      cetAnual: input.cetAnual ?? undefined,
      iof: input.iof ?? undefined,
      tac: input.tac ?? undefined,
      dataContrato: input.dataContrato ?? undefined,
      dataInicioDesconto: input.dataInicioDesconto ?? undefined,
      dataFimDesconto: input.dataFimDesconto ?? undefined,
      observacao: input.observacao ?? undefined,
    },
    include: averbacaoInclude,
  })

  // Log audit action
  if (ctx) {
    const { dadosAnteriores, dadosNovos } = computeAuditDiff(
      existing as unknown as Record<string, unknown>,
      input as Record<string, unknown>
    )

    if (Object.keys(dadosNovos).length > 0) {
      await logAuditAction(ctx, {
        entidade: 'Averbacao',
        entidadeId: id,
        acao: AuditActions.ATUALIZAR,
        dadosAnteriores,
        dadosNovos,
      })
    }
  }

  return mapAverbacaoToResponse(averbacao)
}

/**
 * Changes averbacao state with validation
 */
export async function alterarSituacao(
  tenantId: number,
  id: number,
  novaSituacao: SituacaoAverbacao,
  usuarioId: number,
  options?: {
    observacao?: string
    motivoRejeicao?: string
  },
  ctx?: Context
): Promise<AverbacaoCompleta> {
  const averbacao = await prisma.averbacao.findFirst({
    where: withTenantFilter(tenantId, { id }),
  })

  if (!averbacao) {
    throw new NotFoundError('Averbacao', id)
  }

  // Validate state transition
  validateTransition(averbacao.situacao, novaSituacao)

  // If rejecting, require motivo
  if (novaSituacao === 'REJEITADA' && !options?.motivoRejeicao) {
    throw new BusinessError('Motivo de rejeicao e obrigatorio', {
      code: 'MOTIVO_REJEICAO_OBRIGATORIO',
    })
  }

  const situacaoAnterior = averbacao.situacao

  // Prepare update data
  const updateData: Prisma.AverbacaoUpdateInput = {
    situacao: novaSituacao,
    observacao: options?.observacao ?? averbacao.observacao,
  }

  // Set additional fields based on target state
  if (novaSituacao === 'APROVADA') {
    updateData.dataAprovacao = new Date()
    updateData.usuarioAprovacao = { connect: { id: usuarioId } }
  } else if (novaSituacao === 'REJEITADA') {
    updateData.motivoRejeicao = options?.motivoRejeicao
  }

  // Update averbacao
  const updated = await prisma.averbacao.update({
    where: { id },
    data: updateData,
    include: averbacaoInclude,
  })

  // Create history entry
  await prisma.averbacaoHistorico.create({
    data: {
      averbacaoId: id,
      situacaoAnterior,
      situacaoNova: novaSituacao,
      usuarioId,
      observacao: options?.observacao ?? options?.motivoRejeicao ?? null,
    },
  })

  // Log audit action
  if (ctx) {
    const actionName = getTransitionActionName(novaSituacao)
    await logAuditAction(ctx, {
      entidade: 'Averbacao',
      entidadeId: id,
      acao: actionName as keyof typeof AuditActions,
      dadosAnteriores: { situacao: situacaoAnterior },
      dadosNovos: {
        situacao: novaSituacao,
        ...(options?.motivoRejeicao && { motivoRejeicao: options.motivoRejeicao }),
      },
    })
  }

  return mapAverbacaoToResponse(updated)
}

/**
 * Approves an averbacao
 */
export async function aprovar(
  tenantId: number,
  id: number,
  usuarioId: number,
  observacao?: string,
  ctx?: Context
): Promise<AverbacaoCompleta> {
  return alterarSituacao(tenantId, id, 'APROVADA', usuarioId, { observacao }, ctx)
}

/**
 * Rejects an averbacao
 */
export async function rejeitar(
  tenantId: number,
  id: number,
  usuarioId: number,
  motivoRejeicao: string,
  ctx?: Context
): Promise<AverbacaoCompleta> {
  return alterarSituacao(tenantId, id, 'REJEITADA', usuarioId, { motivoRejeicao }, ctx)
}

/**
 * Suspends an averbacao
 */
export async function suspender(
  tenantId: number,
  id: number,
  usuarioId: number,
  observacao: string,
  ctx?: Context
): Promise<AverbacaoCompleta> {
  return alterarSituacao(tenantId, id, 'SUSPENSA', usuarioId, { observacao }, ctx)
}

/**
 * Cancels an averbacao
 */
export async function cancelar(
  tenantId: number,
  id: number,
  usuarioId: number,
  observacao: string,
  ctx?: Context
): Promise<AverbacaoCompleta> {
  return alterarSituacao(tenantId, id, 'CANCELADA', usuarioId, { observacao }, ctx)
}

/**
 * Blocks an averbacao
 */
export async function bloquear(
  tenantId: number,
  id: number,
  usuarioId: number,
  observacao: string,
  ctx?: Context
): Promise<AverbacaoCompleta> {
  return alterarSituacao(tenantId, id, 'BLOQUEADA', usuarioId, { observacao }, ctx)
}

/**
 * Reactivates an averbacao from suspended/blocked state
 */
export async function reativar(
  tenantId: number,
  id: number,
  usuarioId: number,
  targetState: 'APROVADA' | 'ENVIADA' | 'DESCONTADA',
  observacao?: string,
  ctx?: Context
): Promise<AverbacaoCompleta> {
  return alterarSituacao(tenantId, id, targetState, usuarioId, { observacao }, ctx)
}

/**
 * Lists averbacoes for a specific funcionario
 */
export async function listarPorFuncionario(
  tenantId: number,
  funcionarioId: number,
  includeInactive = false
): Promise<AverbacaoCompleta[]> {
  const where: Prisma.AverbacaoWhereInput = {
    tenantId,
    funcionarioId,
    ...(!includeInactive && {
      situacao: { in: ESTADOS_QUE_CONSOMEM_MARGEM },
    }),
  }

  const averbacoes = await prisma.averbacao.findMany({
    where,
    orderBy: { createdAt: 'desc' },
    include: averbacaoInclude,
  })

  return averbacoes.map(mapAverbacaoToResponse)
}

/**
 * Gets averbacao history
 */
export async function listarHistorico(
  tenantId: number,
  averbacaoId: number
): Promise<
  Array<{
    id: number
    situacaoAnterior: SituacaoAverbacao | null
    situacaoNova: SituacaoAverbacao
    usuario: { id: number; nome: string } | null
    observacao: string | null
    createdAt: Date
  }>
> {
  // Verify averbacao belongs to tenant
  const averbacao = await prisma.averbacao.findFirst({
    where: withTenantFilter(tenantId, { id: averbacaoId }),
    select: { id: true },
  })

  if (!averbacao) {
    throw new NotFoundError('Averbacao', averbacaoId)
  }

  const historico = await prisma.averbacaoHistorico.findMany({
    where: { averbacaoId },
    orderBy: { createdAt: 'desc' },
    include: {
      usuario: {
        select: { id: true, nome: true },
      },
    },
  })

  return historico.map((h) => ({
    id: h.id,
    situacaoAnterior: h.situacaoAnterior,
    situacaoNova: h.situacaoNova,
    usuario: h.usuario,
    observacao: h.observacao,
    createdAt: h.createdAt,
  }))
}

/**
 * Updates parcelas pagas count
 */
export async function atualizarParcelasPagas(
  tenantId: number,
  id: number,
  parcelasPagas: number,
  ctx?: Context
): Promise<AverbacaoCompleta> {
  const averbacao = await prisma.averbacao.findFirst({
    where: withTenantFilter(tenantId, { id }),
  })

  if (!averbacao) {
    throw new NotFoundError('Averbacao', id)
  }

  if (parcelasPagas < 0 || parcelasPagas > averbacao.parcelasTotal) {
    throw new BusinessError(
      `Numero de parcelas pagas deve estar entre 0 e ${averbacao.parcelasTotal}`,
      { code: 'PARCELAS_PAGAS_INVALIDO' }
    )
  }

  // Calculate new saldo devedor
  const valorParcela = Number(averbacao.valorParcela)
  const parcelasRestantes = averbacao.parcelasTotal - parcelasPagas
  const saldoDevedor = valorParcela * parcelasRestantes

  const updateData: Prisma.AverbacaoUpdateInput = {
    parcelasPagas,
    saldoDevedor,
  }

  // If all parcelas paid, mark as liquidada
  if (parcelasPagas >= averbacao.parcelasTotal && averbacao.situacao === 'DESCONTADA') {
    updateData.situacao = 'LIQUIDADA'

    // Create history entry for liquidation
    await prisma.averbacaoHistorico.create({
      data: {
        averbacaoId: id,
        situacaoAnterior: averbacao.situacao,
        situacaoNova: 'LIQUIDADA',
        usuarioId: ctx?.userId ?? null,
        observacao: 'Averbacao liquidada automaticamente apos pagamento de todas as parcelas',
      },
    })
  }

  const updated = await prisma.averbacao.update({
    where: { id },
    data: updateData,
    include: averbacaoInclude,
  })

  return mapAverbacaoToResponse(updated)
}

/**
 * Gets summary statistics for averbacoes
 */
export async function obterResumo(
  tenantId: number,
  consignatariaId?: number | null
): Promise<{
  total: number
  aguardandoAprovacao: number
  aprovadas: number
  enviadas: number
  descontadas: number
  rejeitadas: number
  canceladas: number
  valorTotalContratado: number
  valorTotalParcelas: number
}> {
  const baseWhere: Prisma.AverbacaoWhereInput = {
    tenantId,
    ...(consignatariaId && {
      tenantConsignataria: { consignatariaId },
    }),
  }

  const [
    total,
    aguardandoAprovacao,
    aprovadas,
    enviadas,
    descontadas,
    rejeitadas,
    canceladas,
    totais,
  ] = await Promise.all([
    prisma.averbacao.count({ where: baseWhere }),
    prisma.averbacao.count({
      where: { ...baseWhere, situacao: 'AGUARDANDO_APROVACAO' },
    }),
    prisma.averbacao.count({
      where: { ...baseWhere, situacao: 'APROVADA' },
    }),
    prisma.averbacao.count({
      where: { ...baseWhere, situacao: 'ENVIADA' },
    }),
    prisma.averbacao.count({
      where: { ...baseWhere, situacao: 'DESCONTADA' },
    }),
    prisma.averbacao.count({
      where: { ...baseWhere, situacao: 'REJEITADA' },
    }),
    prisma.averbacao.count({
      where: { ...baseWhere, situacao: 'CANCELADA' },
    }),
    prisma.averbacao.aggregate({
      where: {
        ...baseWhere,
        situacao: { in: ESTADOS_QUE_CONSOMEM_MARGEM },
      },
      _sum: {
        valorTotal: true,
        valorParcela: true,
      },
    }),
  ])

  return {
    total,
    aguardandoAprovacao,
    aprovadas,
    enviadas,
    descontadas,
    rejeitadas,
    canceladas,
    valorTotalContratado: Number(totais._sum.valorTotal ?? 0),
    valorTotalParcelas: Number(totais._sum.valorParcela ?? 0),
  }
}
