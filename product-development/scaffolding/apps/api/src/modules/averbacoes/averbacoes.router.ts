import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { TRPCError } from '@trpc/server'
import { z } from 'zod'
import {
  averbacaoSchema,
  averbacaoFiltroSchema,
  aprovarAverbacaoSchema,
  rejeitarAverbacaoSchema,
} from './averbacoes.schema'
import * as averbacaoService from './averbacoes.service'
import {
  NotFoundError,
  ConflictError,
  BusinessError,
  StateTransitionError,
  MargemInsuficienteError,
} from '@/shared/errors'

/**
 * Converts service errors to TRPCError
 */
function handleServiceError(error: unknown): never {
  if (error instanceof NotFoundError) {
    throw new TRPCError({
      code: 'NOT_FOUND',
      message: error.message,
    })
  }

  if (error instanceof ConflictError) {
    throw new TRPCError({
      code: 'CONFLICT',
      message: error.message,
    })
  }

  if (error instanceof StateTransitionError) {
    throw new TRPCError({
      code: 'BAD_REQUEST',
      message: error.message,
      cause: {
        code: 'INVALID_STATE_TRANSITION',
        from: error.details?.currentState,
        to: error.details?.targetState,
        allowedTransitions: error.details?.allowedTransitions,
      },
    })
  }

  if (error instanceof MargemInsuficienteError) {
    throw new TRPCError({
      code: 'PRECONDITION_FAILED',
      message: error.message,
      cause: {
        code: 'MARGEM_INSUFICIENTE',
        margemDisponivel: error.details?.margemDisponivel,
        margemRequerida: error.details?.margemRequerida,
      },
    })
  }

  if (error instanceof BusinessError) {
    throw new TRPCError({
      code: 'BAD_REQUEST',
      message: error.message,
      cause: error.details,
    })
  }

  if (error instanceof TRPCError) {
    throw error
  }

  throw new TRPCError({
    code: 'INTERNAL_SERVER_ERROR',
    message: error instanceof Error ? error.message : 'Erro interno do servidor',
  })
}

/**
 * Schema for state change operations
 */
const alterarSituacaoSchema = z.object({
  id: z.number().int().positive(),
  observacao: z.string().min(5, 'Observacao deve ter no minimo 5 caracteres'),
})

/**
 * Schema for reactivation
 */
const reativarSchema = z.object({
  id: z.number().int().positive(),
  targetState: z.enum(['APROVADA', 'ENVIADA', 'DESCONTADA']),
  observacao: z.string().optional(),
})

export const averbacaoRouter = router({
  /**
   * Lista averbacoes com paginacao e filtros
   */
  list: protectedProcedure
    .use(withPermission('AVERBACOES_VISUALIZAR'))
    .input(averbacaoFiltroSchema)
    .query(async ({ input, ctx }) => {
      try {
        // Get consignatariaId from user context if user is from a consignataria
        const consignatariaId = await getConsignatariaIdFromUser(ctx)

        return await averbacaoService.listar(ctx.tenantId, input, consignatariaId)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Busca averbacao por ID
   */
  getById: protectedProcedure
    .use(withPermission('AVERBACOES_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      try {
        const consignatariaId = await getConsignatariaIdFromUser(ctx)

        return await averbacaoService.buscarPorId(ctx.tenantId, input.id, consignatariaId)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Busca averbacao por numero de contrato
   */
  getByNumeroContrato: protectedProcedure
    .use(withPermission('AVERBACOES_VISUALIZAR'))
    .input(z.object({ numeroContrato: z.string().min(1) }))
    .query(async ({ input, ctx }) => {
      try {
        const averbacao = await averbacaoService.buscarPorNumeroContrato(
          ctx.tenantId,
          input.numeroContrato
        )

        if (!averbacao) {
          throw new TRPCError({
            code: 'NOT_FOUND',
            message: 'Averbacao nao encontrada',
          })
        }

        return averbacao
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Lista averbacoes de um funcionario
   */
  listByFuncionario: protectedProcedure
    .use(withPermission('AVERBACOES_VISUALIZAR'))
    .input(
      z.object({
        funcionarioId: z.number().int().positive(),
        includeInactive: z.boolean().default(false),
      })
    )
    .query(async ({ input, ctx }) => {
      try {
        return await averbacaoService.listarPorFuncionario(
          ctx.tenantId,
          input.funcionarioId,
          input.includeInactive
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Obtem historico de alteracoes da averbacao
   */
  getHistorico: protectedProcedure
    .use(withPermission('AVERBACOES_VISUALIZAR'))
    .input(z.object({ averbacaoId: z.number().int().positive() }))
    .query(async ({ input, ctx }) => {
      try {
        return await averbacaoService.listarHistorico(ctx.tenantId, input.averbacaoId)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Obtem resumo estatistico das averbacoes (para dashboard)
   */
  getResumo: protectedProcedure
    .use(withPermission('AVERBACOES_VISUALIZAR'))
    .query(async ({ ctx }) => {
      try {
        const consignatariaId = await getConsignatariaIdFromUser(ctx)

        return await averbacaoService.obterResumo(ctx.tenantId, consignatariaId)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Reserva margem para nova averbacao (pre-validacao)
   */
  reservarMargem: protectedProcedure
    .use(withPermission('AVERBACOES_CRIAR'))
    .input(
      z.object({
        funcionarioId: z.number().int().positive(),
        valorParcela: z.number().positive(),
      })
    )
    .query(async ({ input, ctx }) => {
      try {
        return await averbacaoService.reservarMargem(
          ctx.tenantId,
          input.funcionarioId,
          input.valorParcela
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Cria nova averbacao
   */
  create: protectedProcedure
    .use(withPermission('AVERBACOES_CRIAR'))
    .input(averbacaoSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        return await averbacaoService.criar(ctx.tenantId, input, ctx.userId, ctx)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Atualiza averbacao (apenas em estados editaveis)
   */
  update: protectedProcedure
    .use(withPermission('AVERBACOES_EDITAR'))
    .input(
      averbacaoSchema.partial().extend({
        id: z.number().int().positive(),
      })
    )
    .mutation(async ({ input, ctx }) => {
      try {
        const { id, ...updateData } = input
        return await averbacaoService.atualizar(ctx.tenantId, id, updateData, ctx)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Aprova averbacao
   */
  aprovar: protectedProcedure
    .use(withPermission('AVERBACOES_APROVAR'))
    .input(aprovarAverbacaoSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        return await averbacaoService.aprovar(
          ctx.tenantId,
          input.id,
          ctx.userId,
          input.observacao,
          ctx
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Rejeita averbacao
   */
  rejeitar: protectedProcedure
    .use(withPermission('AVERBACOES_APROVAR'))
    .input(rejeitarAverbacaoSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        return await averbacaoService.rejeitar(
          ctx.tenantId,
          input.id,
          ctx.userId,
          input.motivoRejeicao,
          ctx
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Suspende averbacao
   */
  suspender: protectedProcedure
    .use(withPermission('AVERBACOES_GERENCIAR'))
    .input(alterarSituacaoSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        return await averbacaoService.suspender(
          ctx.tenantId,
          input.id,
          ctx.userId,
          input.observacao,
          ctx
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Cancela averbacao
   */
  cancelar: protectedProcedure
    .use(withPermission('AVERBACOES_GERENCIAR'))
    .input(alterarSituacaoSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        return await averbacaoService.cancelar(
          ctx.tenantId,
          input.id,
          ctx.userId,
          input.observacao,
          ctx
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Bloqueia averbacao
   */
  bloquear: protectedProcedure
    .use(withPermission('AVERBACOES_GERENCIAR'))
    .input(alterarSituacaoSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        return await averbacaoService.bloquear(
          ctx.tenantId,
          input.id,
          ctx.userId,
          input.observacao,
          ctx
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Reativa averbacao suspensa ou bloqueada
   */
  reativar: protectedProcedure
    .use(withPermission('AVERBACOES_GERENCIAR'))
    .input(reativarSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        return await averbacaoService.reativar(
          ctx.tenantId,
          input.id,
          ctx.userId,
          input.targetState,
          input.observacao,
          ctx
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Atualiza contagem de parcelas pagas
   */
  atualizarParcelasPagas: protectedProcedure
    .use(withPermission('AVERBACOES_GERENCIAR'))
    .input(
      z.object({
        id: z.number().int().positive(),
        parcelasPagas: z.number().int().nonnegative(),
      })
    )
    .mutation(async ({ input, ctx }) => {
      try {
        return await averbacaoService.atualizarParcelasPagas(
          ctx.tenantId,
          input.id,
          input.parcelasPagas,
          ctx
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),
})

/**
 * Helper to get consignatariaId from user context
 * Returns null if user is not associated with a consignataria
 */
async function getConsignatariaIdFromUser(ctx: {
  prisma: typeof import('@fastconsig/database/client').prisma
  userId: number
  tenantId: number
}): Promise<number | null> {
  const usuario = await ctx.prisma.usuario.findUnique({
    where: { id: ctx.userId },
    select: { consignatariaId: true },
  })

  return usuario?.consignatariaId ?? null
}
