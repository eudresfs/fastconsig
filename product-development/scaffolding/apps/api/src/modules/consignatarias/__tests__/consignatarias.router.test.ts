import { describe, it, expect, vi, beforeEach } from 'vitest'
import { consignatariasRouter } from '../consignatarias.router'
import { TRPCError } from '@trpc/server'

const { mockPrisma } = vi.hoisted(() => {
  return {
    mockPrisma: {
      usuario: {
        findUnique: vi.fn(),
      },
      tenantConsignataria: {
        findMany: vi.fn(),
        findFirst: vi.fn(),
      },
      produto: {
        findMany: vi.fn(),
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

describe('Consignatarias Router', () => {
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
          { permissao: { codigo: 'CONSIGNATARIAS_VISUALIZAR' } },
        ]
      }
    } as any)
  })

  describe('list', () => {
    it('should list consignatarias', async () => {
      mockPrisma.tenantConsignataria.findMany.mockResolvedValue([{ id: 1, consignataria: { nome: 'Bank' } }] as any)

      const caller = consignatariasRouter.createCaller(mockCtx)
      const result = await caller.list()

      expect(mockPrisma.tenantConsignataria.findMany).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({ tenantId: 1 }),
        include: expect.objectContaining({ consignataria: true })
      }))
      expect(result).toHaveLength(1)
    })
  })

  describe('getById', () => {
    it('should return consignataria details', async () => {
      mockPrisma.tenantConsignataria.findFirst.mockResolvedValue({ id: 1, consignataria: { nome: 'Bank' } } as any)

      const caller = consignatariasRouter.createCaller(mockCtx)
      const result = await caller.getById({ id: 1 })

      expect(result).toBeDefined()
      expect(mockPrisma.tenantConsignataria.findFirst).toHaveBeenCalled()
    })

    it('should throw if not found', async () => {
      mockPrisma.tenantConsignataria.findFirst.mockResolvedValue(null)

      const caller = consignatariasRouter.createCaller(mockCtx)
      await expect(caller.getById({ id: 1 })).rejects.toThrow('Consignataria nao encontrada')
    })
  })

  describe('produtos', () => {
    it('should list products', async () => {
      mockPrisma.tenantConsignataria.findFirst.mockResolvedValue({ consignatariaId: 10 } as any)
      mockPrisma.produto.findMany.mockResolvedValue([{ id: 1 }] as any)

      const caller = consignatariasRouter.createCaller(mockCtx)
      const result = await caller.produtos({ consignatariaId: 1 })

      expect(result).toHaveLength(1)
      expect(mockPrisma.produto.findMany).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({ consignatariaId: 10 })
      }))
    })

    it('should throw if consignataria not found for tenant', async () => {
      mockPrisma.tenantConsignataria.findFirst.mockResolvedValue(null)

      const caller = consignatariasRouter.createCaller(mockCtx)
      await expect(caller.produtos({ consignatariaId: 1 })).rejects.toThrow('Consignataria nao encontrada')
    })
  })

  describe('tabelasCoeficiente', () => {
    it('should list coefficient tables', async () => {
      mockPrisma.tabelaCoeficiente.findMany.mockResolvedValue([{ id: 1 }] as any)

      const caller = consignatariasRouter.createCaller(mockCtx)
      const result = await caller.tabelasCoeficiente({ produtoId: 1 })

      expect(result).toHaveLength(1)
      expect(mockPrisma.tabelaCoeficiente.findMany).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({ produtoId: 1 })
      }))
    })
  })
})
