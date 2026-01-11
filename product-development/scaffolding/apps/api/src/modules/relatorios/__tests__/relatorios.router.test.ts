import { describe, it, expect, vi, beforeEach } from 'vitest'
import { relatoriosRouter } from '../relatorios.router'
import { Decimal } from '@prisma/client/runtime/library'

const { mockPrisma } = vi.hoisted(() => {
  return {
    mockPrisma: {
      usuario: {
        findUnique: vi.fn(),
      },
      funcionario: {
        count: vi.fn(),
      },
      averbacao: {
        count: vi.fn(),
        aggregate: vi.fn(),
        groupBy: vi.fn(),
        findMany: vi.fn(),
      },
      tenantConsignataria: {
        findMany: vi.fn(),
      },
    },
  }
})

vi.mock('@fastconsig/database/client', () => ({
  prisma: mockPrisma,
}))

describe('Relatorios Router', () => {
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
          { permissao: { codigo: 'RELATORIOS_VISUALIZAR' } },
        ]
      }
    } as any)
  })

  describe('resumoGeral', () => {
    it('should return general summary', async () => {
      mockPrisma.funcionario.count.mockResolvedValue(10)
      mockPrisma.averbacao.count.mockResolvedValue(20)
      mockPrisma.averbacao.aggregate.mockResolvedValue({ _sum: { valorTotal: new Decimal(1000), valorParcela: new Decimal(100) } } as any)

      const caller = relatoriosRouter.createCaller(mockCtx)
      const result = await caller.resumoGeral()

      expect(result.funcionarios.total).toBe(10)
      expect(result.averbacoes.total).toBe(20)
      expect(result.averbacoes.valorTotal).toBe(1000)
    })

    it('should handle null sums in general summary', async () => {
      mockPrisma.funcionario.count.mockResolvedValue(0)
      mockPrisma.averbacao.count.mockResolvedValue(0)
      mockPrisma.averbacao.aggregate.mockResolvedValue({ _sum: { valorTotal: null, valorParcela: null } } as any)

      const caller = relatoriosRouter.createCaller(mockCtx)
      const result = await caller.resumoGeral()

      expect(result.averbacoes.valorTotal).toBe(0)
      expect(result.folha.valorParcelasMes).toBe(0)
    })
  })

  describe('averbacoesPorConsignataria', () => {
    it('should return averbacoes grouped by consignataria', async () => {
      mockPrisma.averbacao.groupBy.mockResolvedValue([
        {
          tenantConsignatariaId: 1,
          _count: { id: 10 },
          _sum: { valorTotal: new Decimal(1000), valorParcela: new Decimal(100) }
        }
      ] as any)

      mockPrisma.tenantConsignataria.findMany.mockResolvedValue([
        { id: 1, consignataria: { razaoSocial: 'Bank A' } }
      ] as any)

      const caller = relatoriosRouter.createCaller(mockCtx)
      const result = await caller.averbacoesPorConsignataria({})

      expect(result).toHaveLength(1)
      expect(result[0].consignataria).toBe('Bank A')
      expect(result[0].quantidade).toBe(10)
    })

    it('should apply date filters', async () => {
      mockPrisma.averbacao.groupBy.mockResolvedValue([])
      mockPrisma.tenantConsignataria.findMany.mockResolvedValue([])

      const caller = relatoriosRouter.createCaller(mockCtx)
      const dataInicio = new Date('2023-01-01')
      const dataFim = new Date('2023-12-31')

      await caller.averbacoesPorConsignataria({ dataInicio, dataFim })

      expect(mockPrisma.averbacao.groupBy).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({
          dataContrato: expect.objectContaining({
            gte: dataInicio,
            lte: dataFim
          })
        })
      }))
    })

    it('should handle unknown consignataria and null sums', async () => {
      mockPrisma.averbacao.groupBy.mockResolvedValue([
        {
          tenantConsignatariaId: 999,
          _count: { id: 5 },
          _sum: { valorTotal: null, valorParcela: null }
        }
      ] as any)

      mockPrisma.tenantConsignataria.findMany.mockResolvedValue([])

      const caller = relatoriosRouter.createCaller(mockCtx)
      const result = await caller.averbacoesPorConsignataria({})

      expect(result).toHaveLength(1)
      expect(result[0].consignataria).toBe('Desconhecida')
      expect(result[0].valorTotal).toBe(0)
      expect(result[0].valorParcelas).toBe(0)
    })
  })

  describe('evolucaoMensal', () => {
    it('should return monthly evolution sorted', async () => {
      const date1 = new Date(2023, 0, 15) // Jan
      const date2 = new Date(2023, 1, 15) // Feb

      mockPrisma.averbacao.findMany.mockResolvedValue([
        {
          createdAt: date2,
          valorTotal: new Decimal(200),
          valorParcela: new Decimal(20)
        },
        {
          createdAt: date1,
          valorTotal: new Decimal(100),
          valorParcela: new Decimal(10)
        }
      ] as any)

      const caller = relatoriosRouter.createCaller(mockCtx)
      const result = await caller.evolucaoMensal({ meses: 6 })

      expect(result).toHaveLength(2)
      // Should be sorted by date
      expect(result[0].mes).toBe('1/2023')
      expect(result[1].mes).toBe('2/2023')
      expect(result[0].valorTotal).toBe(100)
    })

    it('should accumulate values for same month', async () => {
      const date = new Date(2023, 0, 15)

      mockPrisma.averbacao.findMany.mockResolvedValue([
        {
          createdAt: date,
          valorTotal: new Decimal(100),
          valorParcela: new Decimal(10)
        },
        {
          createdAt: date,
          valorTotal: null, // Test null handling
          valorParcela: null
        }
      ] as any)

      const caller = relatoriosRouter.createCaller(mockCtx)
      const result = await caller.evolucaoMensal({})

      expect(result).toHaveLength(1)
      expect(result[0].quantidade).toBe(2)
      expect(result[0].valorTotal).toBe(100)
    })
  })
})
