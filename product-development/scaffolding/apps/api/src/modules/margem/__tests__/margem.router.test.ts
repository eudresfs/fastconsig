import { describe, it, expect, vi, beforeEach } from 'vitest'
import { margemRouter } from '../margem.router'
import { TRPCError } from '@trpc/server'
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
      tenantConfiguracao: {
        findUnique: vi.fn(),
      },
      averbacao: {
        findMany: vi.fn(),
      },
      margemHistorico: {
        findMany: vi.fn(),
      },
    },
  }
})

vi.mock('@fastconsig/database/client', () => ({
  prisma: mockPrisma,
}))

describe('Margem Router', () => {
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

    // Permissions default
    mockPrisma.usuario.findUnique.mockResolvedValue({
      id: 1,
      perfil: {
        permissoes: [
          { permissao: { codigo: 'MARGEM_VISUALIZAR' } },
          { permissao: { codigo: 'MARGEM_RESERVAR' } },
        ]
      }
    } as any)
  })

  describe('calcular', () => {
    it('should calculate margins correctly', async () => {
      mockPrisma.funcionario.findFirst.mockResolvedValue({
        id: 1,
        salarioBruto: new Decimal(1000)
      } as any)

      mockPrisma.tenantConfiguracao.findUnique.mockResolvedValue({
        percentualMargem: 30,
        percentualEmprestimo: 25,
        percentualCartao: 5
      } as any)

      mockPrisma.averbacao.findMany.mockResolvedValue([
        { produto: { tipo: 'EMPRESTIMO' }, valorParcela: new Decimal(50) },
        { produto: { tipo: 'CARTAO' }, valorParcela: new Decimal(10) }
      ] as any)

      const caller = margemRouter.createCaller(mockCtx)
      const result = await caller.calcular({ funcionarioId: 1 })

      expect(result.salarioBruto).toBe(1000)
      expect(result.margemTotal).toBe(300) // 30% of 1000
      expect(result.emprestimo.total).toBe(250) // 25% of 1000
      expect(result.emprestimo.utilizada).toBe(50)
      expect(result.cartao.total).toBe(50) // 5% of 1000
      expect(result.cartao.utilizada).toBe(10)
    })

    it('should use default percentages if config not found', async () => {
      mockPrisma.funcionario.findFirst.mockResolvedValue({
        id: 1,
        salarioBruto: new Decimal(1000)
      } as any)
      mockPrisma.tenantConfiguracao.findUnique.mockResolvedValue(null)
      mockPrisma.averbacao.findMany.mockResolvedValue([])

      const caller = margemRouter.createCaller(mockCtx)
      const result = await caller.calcular({ funcionarioId: 1 })

      expect(result.margemTotal).toBe(350) // Default 35%
    })

    it('should throw if funcionario not found', async () => {
      mockPrisma.funcionario.findFirst.mockResolvedValue(null)

      const caller = margemRouter.createCaller(mockCtx)
      await expect(caller.calcular({ funcionarioId: 1 })).rejects.toThrow('Funcionario nao encontrado')
    })
  })

  describe('historico', () => {
    it('should return margin history', async () => {
      mockPrisma.margemHistorico.findMany.mockResolvedValue([{ id: 1 }] as any)

      const caller = margemRouter.createCaller(mockCtx)
      const result = await caller.historico({ funcionarioId: 1 })

      expect(result).toHaveLength(1)
      expect(mockPrisma.margemHistorico.findMany).toHaveBeenCalledWith(expect.objectContaining({
        where: { funcionarioId: 1 },
        take: 12
      }))
    })
  })

  describe('reservar', () => {
    it('should reserve margin successfully', async () => {
      mockPrisma.funcionario.findFirst.mockResolvedValue({ id: 1 } as any)

      const caller = margemRouter.createCaller(mockCtx)
      const result = await caller.reservar({
        funcionarioId: 1,
        valor: 100,
        motivo: 'Test'
      })

      expect(result.success).toBe(true)
    })

    it('should throw if funcionario not found', async () => {
      mockPrisma.funcionario.findFirst.mockResolvedValue(null)

      const caller = margemRouter.createCaller(mockCtx)
      await expect(caller.reservar({
        funcionarioId: 1,
        valor: 100
      })).rejects.toThrow('Funcionario nao encontrado')
    })
  })
})
