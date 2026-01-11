import { describe, it, expect, vi, beforeEach } from 'vitest'
import * as funcionariosService from '../funcionarios.service'
import { prisma } from '@fastconsig/database/client'
import { NotFoundError, ConflictError } from '@/shared/errors'
import { TRPCError } from '@trpc/server'

// Mock dependencies
vi.mock('@fastconsig/database/client', () => ({
  prisma: {
    funcionario: {
      findUnique: vi.fn(),
      findFirst: vi.fn(),
      findMany: vi.fn(),
      count: vi.fn(),
      create: vi.fn(),
      update: vi.fn(),
    },
    tenantConfiguracao: {
      findUnique: vi.fn(),
    },
    averbacao: {
      findMany: vi.fn(),
      count: vi.fn(),
    },
    empresa: {
      findFirst: vi.fn(),
    },
    funcionarioHistorico: {
      create: vi.fn(),
    },
    margemHistorico: {
      upsert: vi.fn(),
      findMany: vi.fn(),
    },
    $transaction: vi.fn((callback) => {
      if (Array.isArray(callback)) {
        return Promise.resolve(callback)
      }
      return callback(prisma)
    }),
  },
}))

vi.mock('@/shared/middleware', () => ({
  logAuditAction: vi.fn(),
  AuditActions: {
    CRIAR: 'CRIAR',
    ATUALIZAR: 'ATUALIZAR',
    EXCLUIR: 'EXCLUIR',
  },
  computeAuditDiff: vi.fn(() => ({ dadosAnteriores: {}, dadosNovos: {} })),
  withTenantFilter: vi.fn((tenantId, where) => ({ ...where, tenantId })),
}))

describe('FuncionariosService', () => {
  const tenantId = 1
  const mockCtx = { userId: 1 } as any

  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('calcularMargem', () => {
    it('should calculate margin correctly with default percentage', async () => {
      vi.mocked(prisma.tenantConfiguracao.findUnique).mockResolvedValue(null)
      vi.mocked(prisma.averbacao.findMany).mockResolvedValue([])

      const result = await funcionariosService.calcularMargem(tenantId, 1, 1000)

      expect(result).toEqual({
        total: 350, // 35% of 1000
        utilizada: 0,
        disponivel: 350,
        percentual: 35,
      })
    })

    it('should calculate margin correctly with custom percentage and usage', async () => {
      vi.mocked(prisma.tenantConfiguracao.findUnique).mockResolvedValue({
        percentualMargem: 30,
      } as any)
      vi.mocked(prisma.averbacao.findMany).mockResolvedValue([
        { valorParcela: 50 },
        { valorParcela: 50 },
      ] as any)

      const result = await funcionariosService.calcularMargem(tenantId, 1, 1000)

      expect(result).toEqual({
        total: 300, // 30% of 1000
        utilizada: 100,
        disponivel: 200,
        percentual: 30,
      })
    })
  })

  describe('verificarMargemDisponivel', () => {
    it('should return available true when margin is sufficient', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ salarioBruto: 1000 } as any)
        vi.mocked(prisma.tenantConfiguracao.findUnique).mockResolvedValue(null) // 35%
        vi.mocked(prisma.averbacao.findMany).mockResolvedValue([])

        const result = await funcionariosService.verificarMargemDisponivel(tenantId, 1, 100)
        expect(result.disponivel).toBe(true)
    })

    it('should return available false when margin is insufficient', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ salarioBruto: 1000 } as any)
        vi.mocked(prisma.tenantConfiguracao.findUnique).mockResolvedValue(null) // 35% = 350
        vi.mocked(prisma.averbacao.findMany).mockResolvedValue([
            { valorParcela: 300 }
        ] as any) // used 300, avail 50

        const result = await funcionariosService.verificarMargemDisponivel(tenantId, 1, 100) // need 100
        expect(result.disponivel).toBe(false)
    })

    it('should throw NotFoundError if funcionario not found', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue(null)
        await expect(funcionariosService.verificarMargemDisponivel(tenantId, 1, 100))
            .rejects.toThrow(NotFoundError)
    })
  })

  describe('listar', () => {
      it('should return paginated list', async () => {
          vi.mocked(prisma.funcionario.findMany).mockResolvedValue([
              { id: 1, salarioBruto: 1000 }
          ] as any)
          vi.mocked(prisma.funcionario.count).mockResolvedValue(1)
          vi.mocked(prisma.tenantConfiguracao.findUnique).mockResolvedValue(null)
          vi.mocked(prisma.averbacao.findMany).mockResolvedValue([])

          const result = await funcionariosService.listar(tenantId, { page: 1, pageSize: 10, orderBy: 'nome', orderDir: 'asc' })

          expect(result.data).toHaveLength(1)
          expect(result.pagination.total).toBe(1)
      })
  })

  describe('buscarPorId', () => {
      it('should return funcionario with margin', async () => {
          vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ id: 1, salarioBruto: 1000 } as any)
          vi.mocked(prisma.tenantConfiguracao.findUnique).mockResolvedValue(null)
          vi.mocked(prisma.averbacao.findMany).mockResolvedValue([])

          const result = await funcionariosService.buscarPorId(tenantId, 1)
          expect(result.id).toBe(1)
          expect(result.margem).toBeDefined()
      })

      it('should throw NotFoundError if not found', async () => {
          vi.mocked(prisma.funcionario.findFirst).mockResolvedValue(null)
          await expect(funcionariosService.buscarPorId(tenantId, 1)).rejects.toThrow(NotFoundError)
      })
  })

  describe('buscarPorCpf', () => {
    it('should return funcionario with margin', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ id: 1, salarioBruto: 1000 } as any)
        vi.mocked(prisma.tenantConfiguracao.findUnique).mockResolvedValue(null)
        vi.mocked(prisma.averbacao.findMany).mockResolvedValue([])

        const result = await funcionariosService.buscarPorCpf(tenantId, '123')
        expect(result).not.toBeNull()
    })

    it('should return null if not found', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue(null)
        const result = await funcionariosService.buscarPorCpf(tenantId, '123')
        expect(result).toBeNull()
    })
  })

  describe('criar', () => {
      const input = {
          cpf: '123',
          nome: 'Test',
          matricula: 'M1',
          empresaId: 1,
          dataNascimento: new Date(),
          dataAdmissao: new Date(),
          salarioBruto: 1000,
      }

      it('should create funcionario successfully', async () => {
          vi.mocked(prisma.funcionario.findFirst).mockResolvedValue(null) // No dupes
          vi.mocked(prisma.empresa.findFirst).mockResolvedValue({ id: 1 } as any)
          vi.mocked(prisma.funcionario.create).mockResolvedValue({ id: 1, ...input } as any)

          // For buscarPorId called at end - need to be careful with mock calls order
          // buscarPorId calls findFirst with specific args.
          // In 'criar', we call findFirst multiple times:
          // 1. by cpf
          // 2. by matricula
          // 3. (empresa check) by id
          // 4. (inside buscarPorId) by id

          // Let's implement mock based on args if possible, or sequence
          let callCount = 0
          vi.mocked(prisma.funcionario.findFirst).mockImplementation(async (args) => {
             // 1. cpf check
             if (args?.where?.cpf) return null
             // 2. matricula check
             if (args?.where?.matricula) return null
             // 4. buscarPorId
             if (args?.where?.id) return { id: 1, ...input, salarioBruto: 1000 } as any
             return null
          })

          const result = await funcionariosService.criar(tenantId, input, mockCtx)
          expect(prisma.funcionario.create).toHaveBeenCalled()
      })

      it('should throw ConflictError if CPF exists', async () => {
          vi.mocked(prisma.funcionario.findFirst).mockResolvedValueOnce({ id: 2 } as any)
          await expect(funcionariosService.criar(tenantId, input)).rejects.toThrow(ConflictError)
      })

      it('should throw ConflictError if Matricula exists', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockImplementation(async (args) => {
             if (args?.where?.cpf) return null
             if (args?.where?.matricula) return { id: 2 } as any
             return null
        })

        await expect(funcionariosService.criar(tenantId, input)).rejects.toThrow(ConflictError)
      })

      it('should throw NotFoundError if Empresa not found', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue(null)
        vi.mocked(prisma.empresa.findFirst).mockResolvedValue(null) // Empresa not found

        await expect(funcionariosService.criar(tenantId, input)).rejects.toThrow(NotFoundError)
      })
  })

  describe('atualizar', () => {
      it('should update successfully', async () => {
          vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ id: 1, salarioBruto: 1000 } as any) // Existing
          vi.mocked(prisma.funcionario.update).mockResolvedValue({ id: 1 } as any)

          await funcionariosService.atualizar(tenantId, { id: 1, nome: 'New' }, mockCtx)
          expect(prisma.funcionario.update).toHaveBeenCalled()
      })

      it('should record salary history if changed', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ id: 1, salarioBruto: 1000 } as any) // Existing
        vi.mocked(prisma.funcionario.update).mockResolvedValue({ id: 1 } as any)

        await funcionariosService.atualizar(tenantId, { id: 1, salarioBruto: 2000 }, mockCtx)
        expect(prisma.funcionarioHistorico.create).toHaveBeenCalled()
      })
  })

  describe('excluir', () => {
      it('should soft delete successfully', async () => {
          vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ id: 1 } as any)
          vi.mocked(prisma.averbacao.count).mockResolvedValue(0)

          await funcionariosService.excluir(tenantId, 1, mockCtx)
          expect(prisma.funcionario.update).toHaveBeenCalledWith(expect.objectContaining({
              data: { situacao: 'INATIVO' }
          }))
      })

      it('should throw if active averbacoes exist', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ id: 1 } as any)
        vi.mocked(prisma.averbacao.count).mockResolvedValue(1)

        await expect(funcionariosService.excluir(tenantId, 1)).rejects.toThrow(TRPCError)
      })
  })

  describe('alterarSituacao', () => {
      it('should update situacao and history', async () => {
          vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ id: 1, situacao: 'ATIVO' } as any)
          vi.mocked(prisma.funcionario.update).mockResolvedValue({ id: 1 } as any)

          await funcionariosService.alterarSituacao(tenantId, 1, 'FERIAS', 'ferias', mockCtx)
          expect(prisma.funcionarioHistorico.create).toHaveBeenCalled()
      })
  })

  describe('atualizarMargemHistorico', () => {
      it('should upsert history', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ id: 1, salarioBruto: 1000 } as any)
        vi.mocked(prisma.tenantConfiguracao.findUnique).mockResolvedValue(null)
        vi.mocked(prisma.averbacao.findMany).mockResolvedValue([])

        await funcionariosService.atualizarMargemHistorico(tenantId, 1, '01/2024')
        expect(prisma.margemHistorico.upsert).toHaveBeenCalled()
      })
  })

  describe('listarMargemHistorico', () => {
    it('should return history', async () => {
        vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ id: 1 } as any)
        vi.mocked(prisma.margemHistorico.findMany).mockResolvedValue([{
            competencia: '01/2024',
            salarioBruto: 1000,
            margemTotal: 300,
            margemUtilizada: 0,
            margemDisponivel: 300,
            createdAt: new Date()
        }] as any)

        const result = await funcionariosService.listarMargemHistorico(tenantId, 1)
        expect(result).toHaveLength(1)
    })
  })

})
