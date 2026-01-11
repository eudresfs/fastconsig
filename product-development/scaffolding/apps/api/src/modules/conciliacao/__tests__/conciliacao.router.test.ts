import { describe, it, expect, vi, beforeEach } from 'vitest'
import { conciliacaoRouter } from '../conciliacao.router'
import { TRPCError } from '@trpc/server'
import { Decimal } from '@prisma/client/runtime/library'

const { mockPrisma } = vi.hoisted(() => {
  return {
    mockPrisma: {
      usuario: {
        findUnique: vi.fn(),
      },
      conciliacao: {
        findMany: vi.fn(),
        count: vi.fn(),
        findFirst: vi.fn(),
        create: vi.fn(),
        update: vi.fn(),
      },
      averbacao: {
        findMany: vi.fn(),
      },
      conciliacaoItem: {
        createMany: vi.fn(),
      },
    },
  }
})

vi.mock('@fastconsig/database/client', () => ({
  prisma: mockPrisma,
}))

describe('Conciliacao Router', () => {
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
          { permissao: { codigo: 'CONCILIACAO_VISUALIZAR' } },
          { permissao: { codigo: 'CONCILIACAO_CRIAR' } },
          { permissao: { codigo: 'CONCILIACAO_FECHAR' } },
        ]
      }
    } as any)
  })

  describe('list', () => {
    it('should list conciliacoes', async () => {
      mockPrisma.conciliacao.findMany.mockResolvedValue([{ id: 1 }] as any)
      mockPrisma.conciliacao.count.mockResolvedValue(1)

      const caller = conciliacaoRouter.createCaller(mockCtx)
      const result = await caller.list({})

      expect(mockPrisma.conciliacao.findMany).toHaveBeenCalled()
      expect(result.data).toHaveLength(1)
    })
  })

  describe('getById', () => {
    it('should return conciliacao details', async () => {
      mockPrisma.conciliacao.findFirst.mockResolvedValue({ id: 1 } as any)

      const caller = conciliacaoRouter.createCaller(mockCtx)
      const result = await caller.getById({ id: 1 })

      expect(result.id).toBe(1)
    })

    it('should throw if not found', async () => {
      mockPrisma.conciliacao.findFirst.mockResolvedValue(null)

      const caller = conciliacaoRouter.createCaller(mockCtx)
      await expect(caller.getById({ id: 1 })).rejects.toThrow('Conciliacao nao encontrada')
    })
  })

  describe('criar', () => {
    it('should create conciliacao', async () => {
      mockPrisma.conciliacao.findFirst.mockResolvedValue(null) // Not exists
      mockPrisma.averbacao.findMany.mockResolvedValue([
        { id: 1, valorParcela: new Decimal(100) }
      ] as any)
      mockPrisma.conciliacao.create.mockResolvedValue({ id: 1 } as any)

      const caller = conciliacaoRouter.createCaller(mockCtx)
      const result = await caller.criar({ competencia: '01/2024' })

      expect(mockPrisma.conciliacao.create).toHaveBeenCalled()
      expect(mockPrisma.conciliacaoItem.createMany).toHaveBeenCalled()
      expect(result.id).toBe(1)
    })

    it('should throw if conciliacao exists', async () => {
      mockPrisma.conciliacao.findFirst.mockResolvedValue({ id: 1 } as any)

      const caller = conciliacaoRouter.createCaller(mockCtx)
      await expect(caller.criar({ competencia: '01/2024' })).rejects.toThrow('Ja existe uma conciliacao')
    })
  })

  describe('fechar', () => {
    it('should close conciliacao', async () => {
      mockPrisma.conciliacao.findFirst.mockResolvedValue({ id: 1, status: 'ABERTA' } as any)
      mockPrisma.conciliacao.update.mockResolvedValue({ id: 1, status: 'FECHADA' } as any)

      const caller = conciliacaoRouter.createCaller(mockCtx)
      const result = await caller.fechar({ id: 1 })

      expect(mockPrisma.conciliacao.update).toHaveBeenCalledWith(expect.objectContaining({
        where: { id: 1 },
        data: expect.objectContaining({ status: 'FECHADA' })
      }))
      expect(result.status).toBe('FECHADA')
    })

    it('should throw if already closed', async () => {
      mockPrisma.conciliacao.findFirst.mockResolvedValue({ id: 1, status: 'FECHADA' } as any)

      const caller = conciliacaoRouter.createCaller(mockCtx)
      await expect(caller.fechar({ id: 1 })).rejects.toThrow('Conciliacao ja esta fechada')
    })
  })
})
