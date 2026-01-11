import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { TRPCError } from '@trpc/server'
import { z } from 'zod'
import {
  funcionarioSchema,
  funcionarioUpdateSchema,
  funcionarioFiltroSchema,
} from './funcionarios.schema'
import * as funcionariosService from './funcionarios.service'
import {
  NotFoundError,
  ConflictError,
  BusinessError,
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

export const funcionariosRouter = router({
  /**
   * Lista funcionarios com paginacao e filtros
   */
  list: protectedProcedure
    .use(withPermission('FUNCIONARIOS_VISUALIZAR'))
    .input(funcionarioFiltroSchema)
    .query(async ({ input, ctx }) => {
      try {
        return await funcionariosService.listar(ctx.tenantId, input)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Busca funcionario por ID com margem calculada
   */
  getById: protectedProcedure
    .use(withPermission('FUNCIONARIOS_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      try {
        return await funcionariosService.buscarPorId(ctx.tenantId, input.id)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Busca funcionario por CPF
   */
  getByCpf: protectedProcedure
    .use(withPermission('FUNCIONARIOS_VISUALIZAR'))
    .input(z.object({ cpf: z.string().length(11) }))
    .query(async ({ input, ctx }) => {
      try {
        const funcionario = await funcionariosService.buscarPorCpf(ctx.tenantId, input.cpf)

        if (!funcionario) {
          throw new TRPCError({
            code: 'NOT_FOUND',
            message: 'Funcionario nao encontrado',
          })
        }

        return funcionario
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Busca funcionario por matricula e empresa
   */
  getByMatricula: protectedProcedure
    .use(withPermission('FUNCIONARIOS_VISUALIZAR'))
    .input(
      z.object({
        matricula: z.string().min(1),
        empresaId: z.number().int().positive(),
      })
    )
    .query(async ({ input, ctx }) => {
      try {
        // Query directly since service doesn't have this specific method
        const funcionario = await ctx.prisma.funcionario.findFirst({
          where: {
            tenantId: ctx.tenantId,
            matricula: input.matricula,
            empresaId: input.empresaId,
          },
        })

        if (!funcionario) {
          throw new TRPCError({
            code: 'NOT_FOUND',
            message: 'Funcionario nao encontrado',
          })
        }

        // Return with margin calculation
        return await funcionariosService.buscarPorId(ctx.tenantId, funcionario.id)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Calcula e retorna a margem do funcionario
   */
  getMargem: protectedProcedure
    .use(withPermission('FUNCIONARIOS_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      try {
        const funcionario = await ctx.prisma.funcionario.findFirst({
          where: { id: input.id, tenantId: ctx.tenantId },
          select: { id: true, salarioBruto: true },
        })

        if (!funcionario) {
          throw new TRPCError({
            code: 'NOT_FOUND',
            message: 'Funcionario nao encontrado',
          })
        }

        return await funcionariosService.calcularMargem(
          ctx.tenantId,
          input.id,
          Number(funcionario.salarioBruto)
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Verifica se funcionario tem margem disponivel para valor de parcela
   */
  verificarMargem: protectedProcedure
    .use(withPermission('FUNCIONARIOS_VISUALIZAR'))
    .input(
      z.object({
        funcionarioId: z.number().int().positive(),
        valorParcela: z.number().positive(),
      })
    )
    .query(async ({ input, ctx }) => {
      try {
        return await funcionariosService.verificarMargemDisponivel(
          ctx.tenantId,
          input.funcionarioId,
          input.valorParcela
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Lista historico de margem do funcionario
   */
  getMargemHistorico: protectedProcedure
    .use(withPermission('FUNCIONARIOS_VISUALIZAR'))
    .input(
      z.object({
        funcionarioId: z.number().int().positive(),
        limit: z.number().int().positive().max(24).default(12),
      })
    )
    .query(async ({ input, ctx }) => {
      try {
        return await funcionariosService.listarMargemHistorico(
          ctx.tenantId,
          input.funcionarioId,
          input.limit
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Cria novo funcionario
   */
  create: protectedProcedure
    .use(withPermission('FUNCIONARIOS_CRIAR'))
    .input(funcionarioSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        return await funcionariosService.criar(ctx.tenantId, input, ctx)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Atualiza funcionario existente
   */
  update: protectedProcedure
    .use(withPermission('FUNCIONARIOS_EDITAR'))
    .input(funcionarioUpdateSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        return await funcionariosService.atualizar(ctx.tenantId, input, ctx)
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Exclui funcionario (soft delete)
   */
  delete: protectedProcedure
    .use(withPermission('FUNCIONARIOS_EXCLUIR'))
    .input(z.object({ id: z.number() }))
    .mutation(async ({ input, ctx }) => {
      try {
        await funcionariosService.excluir(ctx.tenantId, input.id, ctx)
        return { success: true }
      } catch (error) {
        handleServiceError(error)
      }
    }),

  /**
   * Altera situacao do funcionario
   */
  alterarSituacao: protectedProcedure
    .use(withPermission('FUNCIONARIOS_EDITAR'))
    .input(
      z.object({
        id: z.number().int().positive(),
        situacao: z.enum(['ATIVO', 'INATIVO', 'AFASTADO', 'BLOQUEADO', 'APOSENTADO']),
        motivo: z.string().optional(),
      })
    )
    .mutation(async ({ input, ctx }) => {
      try {
        return await funcionariosService.alterarSituacao(
          ctx.tenantId,
          input.id,
          input.situacao,
          input.motivo,
          ctx
        )
      } catch (error) {
        handleServiceError(error)
      }
    }),
})
