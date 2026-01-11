import { describe, it, expect, vi, beforeEach } from 'vitest'
import { averbacaoRouter } from '../averbacoes.router'
import * as averbacaoService from '../averbacoes.service'
import { TRPCError } from '@trpc/server'
import {
  NotFoundError,
  ConflictError,
  BusinessError,
  StateTransitionError,
  MargemInsuficienteError
} from '@/shared/errors'

const { mockPrisma } = vi.hoisted(() => {
  return {
    mockPrisma: {
      usuario: {
        findUnique: vi.fn(),
      },
    },
  }
})

vi.mock('@fastconsig/database/client', () => ({
  prisma: mockPrisma,
}))

// Mock service
vi.mock('../averbacoes.service', () => ({
  listar: vi.fn(),
  buscarPorId: vi.fn(),
  buscarPorNumeroContrato: vi.fn(),
  listarPorFuncionario: vi.fn(),
  listarHistorico: vi.fn(),
  obterResumo: vi.fn(),
  reservarMargem: vi.fn(),
  criar: vi.fn(),
  atualizar: vi.fn(),
  aprovar: vi.fn(),
  rejeitar: vi.fn(),
  suspender: vi.fn(),
  cancelar: vi.fn(),
  bloquear: vi.fn(),
  reativar: vi.fn(),
  atualizarParcelasPagas: vi.fn(),
}))

describe('Averbacoes Router', () => {
  const mockCtx = {
    prisma: mockPrisma,
    userId: 1,
    tenantId: 1,
    req: {
      server: {
        jwt: {
          sign: vi.fn(),
        }
      }
    }
  } as any

  beforeEach(() => {
    vi.clearAllMocks()

    // Setup permissions
    mockPrisma.usuario.findUnique.mockResolvedValue({
      id: 1,
      consignatariaId: null, // Test default case (Admin/Gestor)
      perfil: {
        permissoes: [
          { permissao: { codigo: 'AVERBACOES_VISUALIZAR' } },
          { permissao: { codigo: 'AVERBACOES_CRIAR' } },
          { permissao: { codigo: 'AVERBACOES_EDITAR' } },
          { permissao: { codigo: 'AVERBACOES_APROVAR' } },
          { permissao: { codigo: 'AVERBACOES_GERENCIAR' } },
        ]
      }
    } as any)
  })

  describe('list', () => {
    it('should call service.listar', async () => {
      const mockResult = { data: [], pagination: { total: 0 } }
      vi.mocked(averbacaoService.listar).mockResolvedValue(mockResult as any)

      const caller = averbacaoRouter.createCaller(mockCtx)
      const result = await caller.list({ page: 1, pageSize: 10 })

      expect(averbacaoService.listar).toHaveBeenCalledWith(1, expect.objectContaining({ page: 1 }), null)
      expect(result).toEqual(mockResult)
    })
  })

  describe('getById', () => {
    it('should call service.buscarPorId', async () => {
      const mockAverbacao = { id: 1 }
      vi.mocked(averbacaoService.buscarPorId).mockResolvedValue(mockAverbacao as any)

      const caller = averbacaoRouter.createCaller(mockCtx)
      const result = await caller.getById({ id: 1 })

      expect(averbacaoService.buscarPorId).toHaveBeenCalledWith(1, 1, null)
      expect(result).toEqual(mockAverbacao)
    })
  })

  describe('getByNumeroContrato', () => {
    it('should find by contract number', async () => {
      const mockAverbacao = { id: 1, numeroContrato: '123' }
      vi.mocked(averbacaoService.buscarPorNumeroContrato).mockResolvedValue(mockAverbacao as any)

      const caller = averbacaoRouter.createCaller(mockCtx)
      const result = await caller.getByNumeroContrato({ numeroContrato: '123' })

      expect(averbacaoService.buscarPorNumeroContrato).toHaveBeenCalledWith(1, '123')
      expect(result).toEqual(mockAverbacao)
    })

    it('should throw if not found', async () => {
      vi.mocked(averbacaoService.buscarPorNumeroContrato).mockResolvedValue(null)
      const caller = averbacaoRouter.createCaller(mockCtx)
      await expect(caller.getByNumeroContrato({ numeroContrato: '123' })).rejects.toThrow('Averbacao nao encontrada')
    })
  })

  describe('listByFuncionario', () => {
    it('should list by funcionario', async () => {
      vi.mocked(averbacaoService.listarPorFuncionario).mockResolvedValue([] as any)
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.listByFuncionario({ funcionarioId: 1 })
      expect(averbacaoService.listarPorFuncionario).toHaveBeenCalledWith(1, 1, false)
    })
  })

  describe('getHistorico', () => {
    it('should get history', async () => {
      vi.mocked(averbacaoService.listarHistorico).mockResolvedValue([] as any)
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.getHistorico({ averbacaoId: 1 })
      expect(averbacaoService.listarHistorico).toHaveBeenCalledWith(1, 1)
    })
  })

  describe('getResumo', () => {
    it('should get summary', async () => {
      vi.mocked(averbacaoService.obterResumo).mockResolvedValue({ total: 10 } as any)
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.getResumo()
      expect(averbacaoService.obterResumo).toHaveBeenCalledWith(1, null)
    })
  })

  describe('reservarMargem', () => {
    it('should reserve margin', async () => {
      vi.mocked(averbacaoService.reservarMargem).mockResolvedValue({ sucesso: true } as any)
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.reservarMargem({ funcionarioId: 1, valorParcela: 100 })
      expect(averbacaoService.reservarMargem).toHaveBeenCalledWith(1, 1, 100)
    })
  })

  describe('create', () => {
    const input = {
      funcionarioId: 1,
      tenantConsignatariaId: 1,
      produtoId: 1,
      numeroContrato: '123',
      valorParcela: 100,
      valorTotal: 1000,
      parcelasTotal: 10,
      taxaMensal: 1,
      dataContrato: new Date(),
      dataInicioDesconto: new Date(),
      dataFimDesconto: new Date()
    }

    it('should create averbacao', async () => {
      vi.mocked(averbacaoService.criar).mockResolvedValue({ id: 1 } as any)
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.create(input)
      expect(averbacaoService.criar).toHaveBeenCalled()
    })
  })

  describe('update', () => {
    it('should update averbacao', async () => {
      vi.mocked(averbacaoService.atualizar).mockResolvedValue({ id: 1 } as any)
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.update({ id: 1, observacao: 'Update' })
      expect(averbacaoService.atualizar).toHaveBeenCalledWith(1, 1, { observacao: 'Update' }, expect.any(Object))
    })
  })

  describe('transitions', () => {
    it('should approve', async () => {
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.aprovar({ id: 1, observacao: 'Aprovação realizada com sucesso' })
      expect(averbacaoService.aprovar).toHaveBeenCalled()
    })

    it('should reject', async () => {
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.rejeitar({ id: 1, motivoRejeicao: 'Motivo da rejeição insuficiente para aprovação' })
      expect(averbacaoService.rejeitar).toHaveBeenCalled()
    })

    it('should suspend', async () => {
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.suspender({ id: 1, observacao: 'Suspensão temporária solicitada' })
      expect(averbacaoService.suspender).toHaveBeenCalled()
    })

    it('should cancel', async () => {
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.cancelar({ id: 1, observacao: 'Cancelamento definitivo solicitado' })
      expect(averbacaoService.cancelar).toHaveBeenCalled()
    })

    it('should block', async () => {
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.bloquear({ id: 1, observacao: 'Bloqueio administrativo por pendência' })
      expect(averbacaoService.bloquear).toHaveBeenCalled()
    })

    it('should reactivate', async () => {
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.reativar({ id: 1, targetState: 'APROVADA', observacao: 'Reativação autorizada' })
      expect(averbacaoService.reativar).toHaveBeenCalled()
    })

    it('should update paid parcels', async () => {
      const caller = averbacaoRouter.createCaller(mockCtx)
      await caller.atualizarParcelasPagas({ id: 1, parcelasPagas: 5 })
      expect(averbacaoService.atualizarParcelasPagas).toHaveBeenCalled()
    })
  })

  describe('Error Handling', () => {
    it('should handle NotFoundError', async () => {
      vi.mocked(averbacaoService.listar).mockRejectedValue(new NotFoundError('Averbacao'))
      const caller = averbacaoRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (error: any) {
        expect(error).toBeInstanceOf(TRPCError)
        expect(error.code).toBe('NOT_FOUND')
        expect(error.message).toBe('Averbacao nao encontrado')
      }
    })

    it('should handle ConflictError', async () => {
      vi.mocked(averbacaoService.listar).mockRejectedValue(new ConflictError('Conflict'))
      const caller = averbacaoRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (error: any) {
        expect(error).toBeInstanceOf(TRPCError)
        expect(error.code).toBe('CONFLICT')
        expect(error.message).toBe('Conflict')
      }
    })

    it('should handle StateTransitionError', async () => {
      const error = new StateTransitionError('Invalid transition', {
        currentState: 'A',
        targetState: 'B',
        allowedTransitions: ['C']
      })
      vi.mocked(averbacaoService.listar).mockRejectedValue(error)
      const caller = averbacaoRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (err: any) {
        expect(err).toBeInstanceOf(TRPCError)
        expect(err.code).toBe('BAD_REQUEST')
        expect(err.cause.code).toBe('INVALID_STATE_TRANSITION')
      }
    })

    it('should handle MargemInsuficienteError', async () => {
      const error = new MargemInsuficienteError(100, 200)
      vi.mocked(averbacaoService.listar).mockRejectedValue(error)
      const caller = averbacaoRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (err: any) {
        expect(err).toBeInstanceOf(TRPCError)
        expect(err.code).toBe('PRECONDITION_FAILED')
        expect(err.cause.code).toBe('MARGEM_INSUFICIENTE')
      }
    })

    it('should handle BusinessError', async () => {
      vi.mocked(averbacaoService.listar).mockRejectedValue(new BusinessError('Business logic error'))
      const caller = averbacaoRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (err: any) {
        expect(err).toBeInstanceOf(TRPCError)
        expect(err.code).toBe('BAD_REQUEST')
      }
    })

    it('should handle generic Error', async () => {
      vi.mocked(averbacaoService.listar).mockRejectedValue(new Error('Unknown error'))
      const caller = averbacaoRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (err: any) {
        expect(err).toBeInstanceOf(TRPCError)
        expect(err.code).toBe('INTERNAL_SERVER_ERROR')
      }
    })

    it('should rethrow TRPCError', async () => {
      vi.mocked(averbacaoService.listar).mockRejectedValue(new TRPCError({ code: 'UNAUTHORIZED' }))
      const caller = averbacaoRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (err: any) {
        expect(err).toBeInstanceOf(TRPCError)
        expect(err.code).toBe('UNAUTHORIZED')
      }
    })
  })
})
