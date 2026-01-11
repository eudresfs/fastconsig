import { describe, it, expect, vi, beforeEach } from 'vitest'
import { funcionariosRouter } from '../funcionarios.router'
import * as funcionariosService from '../funcionarios.service'
import { TRPCError } from '@trpc/server'
import { NotFoundError, ConflictError, BusinessError } from '@/shared/errors'

const { mockPrisma } = vi.hoisted(() => {
  return {
    mockPrisma: {
      funcionario: {
        findFirst: vi.fn(),
      },
      usuario: {
        findUnique: vi.fn(),
      },
    },
  }
})

vi.mock('@fastconsig/database/client', () => ({
  prisma: mockPrisma,
}))

// Mock dependencies
vi.mock('../funcionarios.service', () => ({
  listar: vi.fn(),
  buscarPorId: vi.fn(),
  buscarPorCpf: vi.fn(),
  calcularMargem: vi.fn(),
  verificarMargemDisponivel: vi.fn(),
  listarMargemHistorico: vi.fn(),
  criar: vi.fn(),
  atualizar: vi.fn(),
  excluir: vi.fn(),
  alterarSituacao: vi.fn(),
}))

describe('Funcionarios Router', () => {
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

    // Setup permissions for all tests
    mockPrisma.usuario.findUnique.mockResolvedValue({
      id: 1,
      perfil: {
        permissoes: [
          { permissao: { codigo: 'FUNCIONARIOS_VISUALIZAR' } },
          { permissao: { codigo: 'FUNCIONARIOS_CRIAR' } },
          { permissao: { codigo: 'FUNCIONARIOS_EDITAR' } },
          { permissao: { codigo: 'FUNCIONARIOS_EXCLUIR' } },
        ]
      }
    } as any)
  })

  describe('list', () => {
    it('should call service.listar', async () => {
      const mockResult = { data: [], pagination: { total: 0, pages: 0, current: 1, pageSize: 10 } }
      vi.mocked(funcionariosService.listar).mockResolvedValue(mockResult)

      const caller = funcionariosRouter.createCaller(mockCtx)
      const result = await caller.list({ page: 1, pageSize: 10 })

      expect(funcionariosService.listar).toHaveBeenCalledWith(1, expect.objectContaining({ page: 1 }))
      expect(result).toEqual(mockResult)
    })
  })

  describe('getById', () => {
    it('should call service.buscarPorId', async () => {
      const mockFunc = { id: 1, nome: 'Test' }
      vi.mocked(funcionariosService.buscarPorId).mockResolvedValue(mockFunc as any)

      const caller = funcionariosRouter.createCaller(mockCtx)
      const result = await caller.getById({ id: 1 })

      expect(funcionariosService.buscarPorId).toHaveBeenCalledWith(1, 1)
      expect(result).toEqual(mockFunc)
    })
  })

  describe('getByCpf', () => {
    it('should call service.buscarPorCpf', async () => {
      const mockFunc = { id: 1, cpf: '12345678901' }
      vi.mocked(funcionariosService.buscarPorCpf).mockResolvedValue(mockFunc as any)

      const caller = funcionariosRouter.createCaller(mockCtx)
      const result = await caller.getByCpf({ cpf: '12345678901' })

      expect(funcionariosService.buscarPorCpf).toHaveBeenCalledWith(1, '12345678901')
      expect(result).toEqual(mockFunc)
    })

    it('should throw NOT_FOUND if not found', async () => {
      vi.mocked(funcionariosService.buscarPorCpf).mockResolvedValue(null)

      const caller = funcionariosRouter.createCaller(mockCtx)
      await expect(caller.getByCpf({ cpf: '12345678901' })).rejects.toThrow('Funcionario nao encontrado')
    })
  })

  describe('getByMatricula', () => {
    it('should find by matricula and return with margin', async () => {
      mockPrisma.funcionario.findFirst.mockResolvedValue({ id: 1 } as any)
      vi.mocked(funcionariosService.buscarPorId).mockResolvedValue({ id: 1, margem: {} } as any)

      const caller = funcionariosRouter.createCaller(mockCtx)
      const result = await caller.getByMatricula({ matricula: '123', empresaId: 1 })

      expect(mockPrisma.funcionario.findFirst).toHaveBeenCalled()
      expect(funcionariosService.buscarPorId).toHaveBeenCalledWith(1, 1)
      expect(result).toBeDefined()
    })

    it('should throw NOT_FOUND if not found', async () => {
      mockPrisma.funcionario.findFirst.mockResolvedValue(null)

      const caller = funcionariosRouter.createCaller(mockCtx)
      await expect(caller.getByMatricula({ matricula: '123', empresaId: 1 })).rejects.toThrow('Funcionario nao encontrado')
    })
  })

  describe('getMargem', () => {
    it('should calculate margin', async () => {
      mockPrisma.funcionario.findFirst.mockResolvedValue({ id: 1, salarioBruto: 1000 } as any)
      vi.mocked(funcionariosService.calcularMargem).mockResolvedValue({ disponivel: 300 } as any)

      const caller = funcionariosRouter.createCaller(mockCtx)
      const result = await caller.getMargem({ id: 1 })

      expect(funcionariosService.calcularMargem).toHaveBeenCalledWith(1, 1, 1000)
      expect(result).toEqual({ disponivel: 300 })
    })

    it('should throw NOT_FOUND if funcionario not found', async () => {
        mockPrisma.funcionario.findFirst.mockResolvedValue(null)
        const caller = funcionariosRouter.createCaller(mockCtx)
        await expect(caller.getMargem({ id: 1 })).rejects.toThrow('Funcionario nao encontrado')
    })
  })

  describe('verificarMargem', () => {
      it('should verify margin', async () => {
          vi.mocked(funcionariosService.verificarMargemDisponivel).mockResolvedValue({ disponivel: true } as any)
          const caller = funcionariosRouter.createCaller(mockCtx)
          await caller.verificarMargem({ funcionarioId: 1, valorParcela: 100 })
          expect(funcionariosService.verificarMargemDisponivel).toHaveBeenCalledWith(1, 1, 100)
      })
  })

  describe('getMargemHistorico', () => {
      it('should list history', async () => {
          vi.mocked(funcionariosService.listarMargemHistorico).mockResolvedValue([] as any)
          const caller = funcionariosRouter.createCaller(mockCtx)
          await caller.getMargemHistorico({ funcionarioId: 1 })
          expect(funcionariosService.listarMargemHistorico).toHaveBeenCalledWith(1, 1, 12)
      })
  })

  describe('create', () => {
      const input = {
          nome: 'Test',
          cpf: '12345678901',
          matricula: '123',
          empresaId: 1,
          dataNascimento: new Date(),
          dataAdmissao: new Date(),
          salarioBruto: 1000
      }

      it('should create funcionario', async () => {
          vi.mocked(funcionariosService.criar).mockResolvedValue({ id: 1 } as any)
          const caller = funcionariosRouter.createCaller(mockCtx)
          await caller.create(input)
          expect(funcionariosService.criar).toHaveBeenCalledWith(1, expect.any(Object), expect.any(Object))
      })
  })

  describe('update', () => {
      it('should update funcionario', async () => {
          vi.mocked(funcionariosService.atualizar).mockResolvedValue({ id: 1 } as any)
          const caller = funcionariosRouter.createCaller(mockCtx)
          await caller.update({ id: 1, nome: 'Updated' })
          expect(funcionariosService.atualizar).toHaveBeenCalled()
      })
  })

  describe('delete', () => {
      it('should delete funcionario', async () => {
          const caller = funcionariosRouter.createCaller(mockCtx)
          await caller.delete({ id: 1 })
          expect(funcionariosService.excluir).toHaveBeenCalledWith(1, 1, expect.any(Object))
      })
  })

  describe('alterarSituacao', () => {
      it('should change status', async () => {
          const caller = funcionariosRouter.createCaller(mockCtx)
          await caller.alterarSituacao({ id: 1, situacao: 'INATIVO' })
          expect(funcionariosService.alterarSituacao).toHaveBeenCalledWith(1, 1, 'INATIVO', undefined, expect.any(Object))
      })
  })

  describe('Error Handling', () => {
    it('should handle NotFoundError', async () => {
      vi.mocked(funcionariosService.listar).mockRejectedValue(new NotFoundError('Funcionario'))
      const caller = funcionariosRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (error: any) {
        expect(error).toBeInstanceOf(TRPCError)
        expect(error.code).toBe('NOT_FOUND')
        expect(error.message).toBe('Funcionario nao encontrado')
      }
    })

    it('should handle ConflictError', async () => {
      vi.mocked(funcionariosService.listar).mockRejectedValue(new ConflictError('Conflict'))
      const caller = funcionariosRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (error: any) {
        expect(error).toBeInstanceOf(TRPCError)
        expect(error.code).toBe('CONFLICT')
        expect(error.message).toBe('Conflict')
      }
    })

    it('should handle BusinessError', async () => {
      vi.mocked(funcionariosService.listar).mockRejectedValue(new BusinessError('Business logic error'))
      const caller = funcionariosRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (err: any) {
        expect(err).toBeInstanceOf(TRPCError)
        expect(err.code).toBe('BAD_REQUEST')
      }
    })

    it('should handle generic Error', async () => {
      vi.mocked(funcionariosService.listar).mockRejectedValue(new Error('Unknown error'))
      const caller = funcionariosRouter.createCaller(mockCtx)

      try {
        await caller.list({ page: 1 })
        expect.fail('Should have thrown')
      } catch (err: any) {
        expect(err).toBeInstanceOf(TRPCError)
        expect(err.code).toBe('INTERNAL_SERVER_ERROR')
      }
    })

    it('should rethrow TRPCError', async () => {
      vi.mocked(funcionariosService.listar).mockRejectedValue(new TRPCError({ code: 'UNAUTHORIZED' }))
      const caller = funcionariosRouter.createCaller(mockCtx)

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
