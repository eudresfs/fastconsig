import { describe, it, expect, vi, beforeEach } from 'vitest'
import { simulacaoRouter } from '../simulacao.router'
import { Decimal } from '@prisma/client/runtime/library'

const { mockPrisma } = vi.hoisted(() => {
  return {
    mockPrisma: {
      usuario: {
        findUnique: vi.fn(),
      },
      funcionario: {
        findFirst: vi.fn(),
      },
      produto: {
        findUnique: vi.fn(),
      },
      tenantConfiguracao: {
        findUnique: vi.fn(),
      },
      tabelaCoeficiente: {
        findMany: vi.fn(),
      },
    },
  }
})

vi.mock('@fastconsig/database/client', () => ({
  prisma: mockPrisma,
}))

describe('Simulacao Router', () => {
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

    // Mock authenticated user (no specific permissions needed for simulacao as per current router)
    mockPrisma.usuario.findUnique.mockResolvedValue({
      id: 1,
      perfil: { permissoes: [] }
    } as any)
  })

  describe('simular', () => {
    const input = {
      salarioBruto: 1000,
      valorSolicitado: 1000,
      prazo: 12,
      produtoId: 1
    }

    it('should simulate successfully with valorSolicitado', async () => {
      mockPrisma.produto.findUnique.mockResolvedValue({
        id: 1,
        tipo: 'EMPRESTIMO',
        coeficientes: [{
          ativo: true,
          itens: [{
            prazo: 12,
            coeficiente: new Decimal(0.1),
            taxaMensal: new Decimal(2.0)
          }]
        }]
      } as any)

      mockPrisma.tenantConfiguracao.findUnique.mockResolvedValue({
        percentualEmprestimo: 30
      } as any)

      const caller = simulacaoRouter.createCaller(mockCtx)
      const result = await caller.simular(input)

      expect(result.valorParcela).toBe(100) // 1000 * 0.1
      expect(result.margemDisponivel).toBe(300) // 1000 * 30%
      expect(result.parcelaCabeNaMargem).toBe(true)
    })

    it('should simulate successfully with valorParcela', async () => {
      mockPrisma.produto.findUnique.mockResolvedValue({
        id: 1,
        tipo: 'EMPRESTIMO',
        coeficientes: [{
          ativo: true,
          itens: [{
            prazo: 12,
            coeficiente: new Decimal(0.1),
            taxaMensal: new Decimal(2.0)
          }]
        }]
      } as any)

      mockPrisma.tenantConfiguracao.findUnique.mockResolvedValue(null)

      const caller = simulacaoRouter.createCaller(mockCtx)
      const result = await caller.simular({
        salarioBruto: 1000,
        valorParcela: 100,
        prazo: 12,
        produtoId: 1
      })

      expect(result.valorTotal).toBe(1000) // 100 / 0.1
    })

    it('should fetch salary from funcionarioId', async () => {
      mockPrisma.funcionario.findFirst.mockResolvedValue({
        salarioBruto: new Decimal(2000)
      } as any)

      mockPrisma.produto.findUnique.mockResolvedValue({
        id: 1,
        tipo: 'EMPRESTIMO',
        coeficientes: [{
          ativo: true,
          itens: [{
            prazo: 12,
            coeficiente: new Decimal(0.1),
            taxaMensal: new Decimal(2.0)
          }]
        }]
      } as any)

      const caller = simulacaoRouter.createCaller(mockCtx)
      const result = await caller.simular({
        funcionarioId: 1,
        valorSolicitado: 1000,
        prazo: 12,
        produtoId: 1
      })

      expect(result.margemDisponivel).toBe(600) // 2000 * 30% (default)
    })

    it('should throw if coefficient not found', async () => {
      mockPrisma.produto.findUnique.mockResolvedValue({
        id: 1,
        coeficientes: [{ itens: [] }]
      } as any)

      const caller = simulacaoRouter.createCaller(mockCtx)
      await expect(caller.simular(input)).rejects.toThrow('Coeficiente nao encontrado')
    })
  })

  describe('prazosDisponiveis', () => {
    it('should return sorted available terms', async () => {
      mockPrisma.tabelaCoeficiente.findMany.mockResolvedValue([
        {
          itens: [{ prazo: 24 }, { prazo: 12 }]
        }
      ] as any)

      const caller = simulacaoRouter.createCaller(mockCtx)
      const result = await caller.prazosDisponiveis({ produtoId: 1 })

      expect(result).toEqual([12, 24])
    })
  })
})
