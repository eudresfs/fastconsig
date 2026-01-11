import { TRPCError } from '@trpc/server'
import { prisma } from '@fastconsig/database/client'
import type { Prisma, SituacaoFuncionario } from '@prisma/client'
import type { Context } from '@/trpc/context'
import {
  logAuditAction,
  AuditActions,
  computeAuditDiff,
  withTenantFilter,
} from '@/shared/middleware'
import { NotFoundError, ConflictError } from '@/shared/errors'
import type {
  FuncionarioInput,
  FuncionarioUpdateInput,
  FuncionarioFiltro,
} from './funcionarios.schema'

/**
 * Funcionario with calculated margin information
 */
export interface FuncionarioWithMargem {
  id: number
  tenantId: number
  empresaId: number
  cpf: string
  nome: string
  dataNascimento: Date
  sexo: string | null
  email: string | null
  telefone: string | null
  matricula: string
  cargo: string | null
  dataAdmissao: Date
  salarioBruto: number
  situacao: SituacaoFuncionario
  banco: string | null
  agencia: string | null
  conta: string | null
  tipoConta: string | null
  fotoUrl: string | null
  createdAt: Date
  updatedAt: Date
  margem: {
    total: number
    utilizada: number
    disponivel: number
    percentual: number
  }
}

/**
 * Paginated list response
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
 * Calculates margin for a funcionario based on salary and tenant configuration
 */
export async function calcularMargem(
  tenantId: number,
  funcionarioId: number,
  salarioBruto: number
): Promise<{
  total: number
  utilizada: number
  disponivel: number
  percentual: number
}> {
  // Get tenant configuration
  const config = await prisma.tenantConfiguracao.findUnique({
    where: { tenantId },
  })

  const percentualMargem = config?.percentualMargem
    ? Number(config.percentualMargem)
    : 35

  // Calculate total margin
  const margemTotal = (salarioBruto * percentualMargem) / 100

  // Get utilized margin from active averbacoes
  const averbacoes = await prisma.averbacao.findMany({
    where: {
      funcionarioId,
      situacao: {
        in: ['APROVADA', 'ENVIADA', 'DESCONTADA', 'AGUARDANDO_APROVACAO'],
      },
    },
    select: {
      valorParcela: true,
    },
  })

  const margemUtilizada = averbacoes.reduce(
    (sum, av) => sum + Number(av.valorParcela),
    0
  )

  const margemDisponivel = Math.max(0, margemTotal - margemUtilizada)

  return {
    total: margemTotal,
    utilizada: margemUtilizada,
    disponivel: margemDisponivel,
    percentual: percentualMargem,
  }
}

/**
 * Checks if funcionario has sufficient margin for a new averbacao
 */
export async function verificarMargemDisponivel(
  tenantId: number,
  funcionarioId: number,
  valorParcela: number
): Promise<{
  disponivel: boolean
  margemDisponivel: number
  margemRequerida: number
  diferenca: number
}> {
  const funcionario = await prisma.funcionario.findFirst({
    where: { id: funcionarioId, tenantId },
    select: { salarioBruto: true },
  })

  if (!funcionario) {
    throw new NotFoundError('Funcionario', funcionarioId)
  }

  const margem = await calcularMargem(
    tenantId,
    funcionarioId,
    Number(funcionario.salarioBruto)
  )

  return {
    disponivel: margem.disponivel >= valorParcela,
    margemDisponivel: margem.disponivel,
    margemRequerida: valorParcela,
    diferenca: margem.disponivel - valorParcela,
  }
}

/**
 * Lists funcionarios with pagination and filtering
 */
export async function listar(
  tenantId: number,
  filtro: FuncionarioFiltro
): Promise<PaginatedResponse<FuncionarioWithMargem>> {
  const { page, pageSize, search, situacao, empresaId, orderBy, orderDir } = filtro

  const where: Prisma.FuncionarioWhereInput = {
    tenantId,
    ...(situacao && { situacao }),
    ...(empresaId && { empresaId }),
    ...(search && {
      OR: [
        { nome: { contains: search, mode: 'insensitive' } },
        { cpf: { contains: search } },
        { matricula: { contains: search, mode: 'insensitive' } },
      ],
    }),
  }

  const [funcionarios, total] = await Promise.all([
    prisma.funcionario.findMany({
      where,
      skip: (page - 1) * pageSize,
      take: pageSize,
      orderBy: { [orderBy]: orderDir },
      include: {
        empresa: {
          select: { id: true, nome: true, codigo: true },
        },
      },
    }),
    prisma.funcionario.count({ where }),
  ])

  // Calculate margin for each funcionario
  const funcionariosWithMargem = await Promise.all(
    funcionarios.map(async (f) => {
      const margem = await calcularMargem(tenantId, f.id, Number(f.salarioBruto))
      return {
        id: f.id,
        tenantId: f.tenantId,
        empresaId: f.empresaId,
        cpf: f.cpf,
        nome: f.nome,
        dataNascimento: f.dataNascimento,
        sexo: f.sexo,
        email: f.email,
        telefone: f.telefone,
        matricula: f.matricula,
        cargo: f.cargo,
        dataAdmissao: f.dataAdmissao,
        salarioBruto: Number(f.salarioBruto),
        situacao: f.situacao,
        banco: f.banco,
        agencia: f.agencia,
        conta: f.conta,
        tipoConta: f.tipoConta,
        fotoUrl: f.fotoUrl,
        createdAt: f.createdAt,
        updatedAt: f.updatedAt,
        margem,
      }
    })
  )

  return {
    data: funcionariosWithMargem,
    pagination: {
      page,
      pageSize,
      total,
      totalPages: Math.ceil(total / pageSize),
    },
  }
}

/**
 * Gets a funcionario by ID
 */
export async function buscarPorId(
  tenantId: number,
  id: number
): Promise<FuncionarioWithMargem> {
  const funcionario = await prisma.funcionario.findFirst({
    where: withTenantFilter(tenantId, { id }),
    include: {
      empresa: {
        select: { id: true, nome: true, codigo: true },
      },
    },
  })

  if (!funcionario) {
    throw new NotFoundError('Funcionario', id)
  }

  const margem = await calcularMargem(
    tenantId,
    funcionario.id,
    Number(funcionario.salarioBruto)
  )

  return {
    id: funcionario.id,
    tenantId: funcionario.tenantId,
    empresaId: funcionario.empresaId,
    cpf: funcionario.cpf,
    nome: funcionario.nome,
    dataNascimento: funcionario.dataNascimento,
    sexo: funcionario.sexo,
    email: funcionario.email,
    telefone: funcionario.telefone,
    matricula: funcionario.matricula,
    cargo: funcionario.cargo,
    dataAdmissao: funcionario.dataAdmissao,
    salarioBruto: Number(funcionario.salarioBruto),
    situacao: funcionario.situacao,
    banco: funcionario.banco,
    agencia: funcionario.agencia,
    conta: funcionario.conta,
    tipoConta: funcionario.tipoConta,
    fotoUrl: funcionario.fotoUrl,
    createdAt: funcionario.createdAt,
    updatedAt: funcionario.updatedAt,
    margem,
  }
}

/**
 * Gets a funcionario by CPF
 */
export async function buscarPorCpf(
  tenantId: number,
  cpf: string
): Promise<FuncionarioWithMargem | null> {
  const funcionario = await prisma.funcionario.findFirst({
    where: withTenantFilter(tenantId, { cpf }),
    include: {
      empresa: {
        select: { id: true, nome: true, codigo: true },
      },
    },
  })

  if (!funcionario) {
    return null
  }

  const margem = await calcularMargem(
    tenantId,
    funcionario.id,
    Number(funcionario.salarioBruto)
  )

  return {
    id: funcionario.id,
    tenantId: funcionario.tenantId,
    empresaId: funcionario.empresaId,
    cpf: funcionario.cpf,
    nome: funcionario.nome,
    dataNascimento: funcionario.dataNascimento,
    sexo: funcionario.sexo,
    email: funcionario.email,
    telefone: funcionario.telefone,
    matricula: funcionario.matricula,
    cargo: funcionario.cargo,
    dataAdmissao: funcionario.dataAdmissao,
    salarioBruto: Number(funcionario.salarioBruto),
    situacao: funcionario.situacao,
    banco: funcionario.banco,
    agencia: funcionario.agencia,
    conta: funcionario.conta,
    tipoConta: funcionario.tipoConta,
    fotoUrl: funcionario.fotoUrl,
    createdAt: funcionario.createdAt,
    updatedAt: funcionario.updatedAt,
    margem,
  }
}

/**
 * Creates a new funcionario
 */
export async function criar(
  tenantId: number,
  input: FuncionarioInput,
  ctx?: Context
): Promise<FuncionarioWithMargem> {
  // Check for duplicate CPF
  const existingByCpf = await prisma.funcionario.findFirst({
    where: { tenantId, cpf: input.cpf },
  })

  if (existingByCpf) {
    throw new ConflictError(`Ja existe um funcionario com o CPF ${input.cpf}`)
  }

  // Check for duplicate matricula in the same empresa
  const existingByMatricula = await prisma.funcionario.findFirst({
    where: {
      tenantId,
      empresaId: input.empresaId,
      matricula: input.matricula,
    },
  })

  if (existingByMatricula) {
    throw new ConflictError(
      `Ja existe um funcionario com a matricula ${input.matricula} nesta empresa`
    )
  }

  // Verify empresa belongs to tenant
  const empresa = await prisma.empresa.findFirst({
    where: { id: input.empresaId, tenantId },
  })

  if (!empresa) {
    throw new NotFoundError('Empresa', input.empresaId)
  }

  const funcionario = await prisma.funcionario.create({
    data: {
      tenantId,
      empresaId: input.empresaId,
      cpf: input.cpf,
      nome: input.nome,
      dataNascimento: input.dataNascimento,
      sexo: input.sexo ?? null,
      email: input.email ?? null,
      telefone: input.telefone ?? null,
      matricula: input.matricula,
      cargo: input.cargo ?? null,
      dataAdmissao: input.dataAdmissao,
      salarioBruto: input.salarioBruto,
      situacao: input.situacao ?? 'ATIVO',
      banco: input.banco ?? null,
      agencia: input.agencia ?? null,
      conta: input.conta ?? null,
      tipoConta: input.tipoConta ?? null,
    },
  })

  // Log audit action
  if (ctx) {
    await logAuditAction(ctx, {
      entidade: 'Funcionario',
      entidadeId: funcionario.id,
      acao: AuditActions.CRIAR,
      dadosNovos: {
        cpf: input.cpf,
        nome: input.nome,
        matricula: input.matricula,
        empresaId: input.empresaId,
      },
    })
  }

  return buscarPorId(tenantId, funcionario.id)
}

/**
 * Updates a funcionario
 */
export async function atualizar(
  tenantId: number,
  input: FuncionarioUpdateInput,
  ctx?: Context
): Promise<FuncionarioWithMargem> {
  const { id, ...updateData } = input

  // Get existing funcionario
  const existing = await prisma.funcionario.findFirst({
    where: withTenantFilter(tenantId, { id }),
  })

  if (!existing) {
    throw new NotFoundError('Funcionario', id)
  }

  // Check for duplicate CPF if changing
  if (updateData.cpf && updateData.cpf !== existing.cpf) {
    const existingByCpf = await prisma.funcionario.findFirst({
      where: { tenantId, cpf: updateData.cpf, id: { not: id } },
    })

    if (existingByCpf) {
      throw new ConflictError(`Ja existe um funcionario com o CPF ${updateData.cpf}`)
    }
  }

  // Check for duplicate matricula if changing
  if (
    (updateData.matricula && updateData.matricula !== existing.matricula) ||
    (updateData.empresaId && updateData.empresaId !== existing.empresaId)
  ) {
    const empresaId = updateData.empresaId ?? existing.empresaId
    const matricula = updateData.matricula ?? existing.matricula

    const existingByMatricula = await prisma.funcionario.findFirst({
      where: {
        tenantId,
        empresaId,
        matricula,
        id: { not: id },
      },
    })

    if (existingByMatricula) {
      throw new ConflictError(
        `Ja existe um funcionario com a matricula ${matricula} nesta empresa`
      )
    }
  }

  // If changing empresa, verify it belongs to tenant
  if (updateData.empresaId) {
    const empresa = await prisma.empresa.findFirst({
      where: { id: updateData.empresaId, tenantId },
    })

    if (!empresa) {
      throw new NotFoundError('Empresa', updateData.empresaId)
    }
  }

  // Track salary changes for history
  const salaryChanged =
    updateData.salarioBruto !== undefined &&
    Number(updateData.salarioBruto) !== Number(existing.salarioBruto)

  const funcionario = await prisma.funcionario.update({
    where: { id },
    data: {
      ...updateData,
      // Only update fields that are provided
      cpf: updateData.cpf ?? undefined,
      nome: updateData.nome ?? undefined,
      dataNascimento: updateData.dataNascimento ?? undefined,
      sexo: updateData.sexo ?? undefined,
      email: updateData.email ?? undefined,
      telefone: updateData.telefone ?? undefined,
      matricula: updateData.matricula ?? undefined,
      cargo: updateData.cargo ?? undefined,
      dataAdmissao: updateData.dataAdmissao ?? undefined,
      salarioBruto: updateData.salarioBruto ?? undefined,
      situacao: updateData.situacao ?? undefined,
      banco: updateData.banco ?? undefined,
      agencia: updateData.agencia ?? undefined,
      conta: updateData.conta ?? undefined,
      tipoConta: updateData.tipoConta ?? undefined,
      empresaId: updateData.empresaId ?? undefined,
    },
  })

  // Record salary change in history if applicable
  if (salaryChanged) {
    await prisma.funcionarioHistorico.create({
      data: {
        funcionarioId: id,
        usuarioId: ctx?.userId ?? null,
        campo: 'salarioBruto',
        valorAnterior: String(existing.salarioBruto),
        valorNovo: String(updateData.salarioBruto),
        motivo: 'Atualizacao de salario',
      },
    })
  }

  // Log audit action
  if (ctx) {
    const { dadosAnteriores, dadosNovos } = computeAuditDiff(
      existing as Record<string, unknown>,
      updateData as Record<string, unknown>
    )

    if (Object.keys(dadosNovos).length > 0) {
      await logAuditAction(ctx, {
        entidade: 'Funcionario',
        entidadeId: id,
        acao: AuditActions.ATUALIZAR,
        dadosAnteriores,
        dadosNovos,
      })
    }
  }

  return buscarPorId(tenantId, funcionario.id)
}

/**
 * Deletes a funcionario (soft delete by setting situacao to INATIVO)
 */
export async function excluir(
  tenantId: number,
  id: number,
  ctx?: Context
): Promise<void> {
  const funcionario = await prisma.funcionario.findFirst({
    where: withTenantFilter(tenantId, { id }),
  })

  if (!funcionario) {
    throw new NotFoundError('Funcionario', id)
  }

  // Check for active averbacoes
  const averbacoes = await prisma.averbacao.count({
    where: {
      funcionarioId: id,
      situacao: {
        in: ['AGUARDANDO_APROVACAO', 'APROVADA', 'ENVIADA', 'DESCONTADA'],
      },
    },
  })

  if (averbacoes > 0) {
    throw new TRPCError({
      code: 'PRECONDITION_FAILED',
      message: `Funcionario possui ${averbacoes} averbacao(oes) ativa(s) e nao pode ser excluido`,
    })
  }

  // Soft delete
  await prisma.funcionario.update({
    where: { id },
    data: { situacao: 'INATIVO' },
  })

  // Log audit action
  if (ctx) {
    await logAuditAction(ctx, {
      entidade: 'Funcionario',
      entidadeId: id,
      acao: AuditActions.EXCLUIR,
      dadosAnteriores: {
        cpf: funcionario.cpf,
        nome: funcionario.nome,
        matricula: funcionario.matricula,
        situacaoAnterior: funcionario.situacao,
      },
    })
  }
}

/**
 * Alters funcionario situacao
 */
export async function alterarSituacao(
  tenantId: number,
  id: number,
  novaSituacao: SituacaoFuncionario,
  motivo?: string,
  ctx?: Context
): Promise<FuncionarioWithMargem> {
  const funcionario = await prisma.funcionario.findFirst({
    where: withTenantFilter(tenantId, { id }),
  })

  if (!funcionario) {
    throw new NotFoundError('Funcionario', id)
  }

  const situacaoAnterior = funcionario.situacao

  await prisma.$transaction([
    prisma.funcionario.update({
      where: { id },
      data: { situacao: novaSituacao },
    }),
    prisma.funcionarioHistorico.create({
      data: {
        funcionarioId: id,
        usuarioId: ctx?.userId ?? null,
        campo: 'situacao',
        valorAnterior: situacaoAnterior,
        valorNovo: novaSituacao,
        motivo,
      },
    }),
  ])

  // Log audit action
  if (ctx) {
    await logAuditAction(ctx, {
      entidade: 'Funcionario',
      entidadeId: id,
      acao: AuditActions.ATUALIZAR,
      dadosAnteriores: { situacao: situacaoAnterior },
      dadosNovos: { situacao: novaSituacao, motivo },
    })
  }

  return buscarPorId(tenantId, id)
}

/**
 * Updates margin history for a funcionario (used during payroll processing)
 */
export async function atualizarMargemHistorico(
  tenantId: number,
  funcionarioId: number,
  competencia: string,
  ctx?: Context
): Promise<void> {
  const funcionario = await prisma.funcionario.findFirst({
    where: withTenantFilter(tenantId, { id: funcionarioId }),
    select: { id: true, salarioBruto: true },
  })

  if (!funcionario) {
    throw new NotFoundError('Funcionario', funcionarioId)
  }

  const margem = await calcularMargem(
    tenantId,
    funcionarioId,
    Number(funcionario.salarioBruto)
  )

  await prisma.margemHistorico.upsert({
    where: {
      funcionarioId_competencia: {
        funcionarioId,
        competencia,
      },
    },
    create: {
      funcionarioId,
      competencia,
      salarioBruto: funcionario.salarioBruto,
      margemTotal: margem.total,
      margemUtilizada: margem.utilizada,
      margemDisponivel: margem.disponivel,
    },
    update: {
      salarioBruto: funcionario.salarioBruto,
      margemTotal: margem.total,
      margemUtilizada: margem.utilizada,
      margemDisponivel: margem.disponivel,
    },
  })
}

/**
 * Gets margin history for a funcionario
 */
export async function listarMargemHistorico(
  tenantId: number,
  funcionarioId: number,
  limit = 12
): Promise<
  Array<{
    competencia: string
    salarioBruto: number
    margemTotal: number
    margemUtilizada: number
    margemDisponivel: number
    createdAt: Date
  }>
> {
  // Verify funcionario belongs to tenant
  const funcionario = await prisma.funcionario.findFirst({
    where: withTenantFilter(tenantId, { id: funcionarioId }),
    select: { id: true },
  })

  if (!funcionario) {
    throw new NotFoundError('Funcionario', funcionarioId)
  }

  const historico = await prisma.margemHistorico.findMany({
    where: { funcionarioId },
    orderBy: { competencia: 'desc' },
    take: limit,
  })

  return historico.map((h) => ({
    competencia: h.competencia,
    salarioBruto: Number(h.salarioBruto),
    margemTotal: Number(h.margemTotal),
    margemUtilizada: Number(h.margemUtilizada),
    margemDisponivel: Number(h.margemDisponivel),
    createdAt: h.createdAt,
  }))
}
