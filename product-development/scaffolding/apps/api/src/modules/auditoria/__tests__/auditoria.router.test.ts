import { describe, it, expect, vi, beforeEach } from 'vitest'
import { auditoriaRouter } from '../auditoria.router'
import { TRPCError } from '@trpc/server'

const { mockPrisma } = vi.hoisted(() => {
  return {
    mockPrisma: {
      usuario: {
        findUnique: vi.fn(),
      },
      auditoria: {
        findMany: vi.fn(),
        count: vi.fn(),
        findFirst: vi.fn(),
      },
    },
  }
})

vi.mock('@fastconsig/database/client', () => ({
  prisma: mockPrisma,
}))

describe('Auditoria Router', () => {
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
          { permissao: { codigo: 'AUDITORIA_VISUALIZAR' } },
        ]
      }
    } as any)
  })

  describe('list', () => {
    it('should list audit logs', async () => {
      mockPrisma.auditoria.findMany.mockResolvedValue([{ id: 1 }] as any)
      mockPrisma.auditoria.count.mockResolvedValue(1)

      const caller = auditoriaRouter.createCaller(mockCtx)
      const result = await caller.list({ page: 1, pageSize: 10 })

      expect(mockPrisma.auditoria.findMany).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({ tenantId: 1 }),
        take: 10,
        skip: 0
      }))
      expect(result.data).toHaveLength(1)
      expect(result.pagination.total).toBe(1)
    })

    it('should filter by fields', async () => {
      mockPrisma.auditoria.findMany.mockResolvedValue([])
      mockPrisma.auditoria.count.mockResolvedValue(0)

      const caller = auditoriaRouter.createCaller(mockCtx)
      await caller.list({
        entidade: 'User',
        acao: 'CRIAR',
        usuarioId: 2
      })

      expect(mockPrisma.auditoria.findMany).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({
          entidade: 'User',
          acao: 'CRIAR',
          usuarioId: 2
        })
      }))
    })
  })

  describe('getById', () => {
    it('should return audit log', async () => {
      mockPrisma.auditoria.findFirst.mockResolvedValue({ id: 1 } as any)

      const caller = auditoriaRouter.createCaller(mockCtx)
      const result = await caller.getById({ id: 1 })

      expect(result.id).toBe(1)
    })

    it('should throw if not found', async () => {
      mockPrisma.auditoria.findFirst.mockResolvedValue(null)

      const caller = auditoriaRouter.createCaller(mockCtx)
      await expect(caller.getById({ id: 1 })).rejects.toThrow('Registro de auditoria nao encontrado')
    })
  })

  describe('entidades', () => {
    it('should return distinct entities', async () => {
      mockPrisma.auditoria.findMany.mockResolvedValue([
        { entidade: 'User' },
        { entidade: 'Post' }
      ] as any)

      const caller = auditoriaRouter.createCaller(mockCtx)
      const result = await caller.entidades()

      expect(result).toEqual(['User', 'Post'])
      expect(mockPrisma.auditoria.findMany).toHaveBeenCalledWith(expect.objectContaining({
        distinct: ['entidade']
      }))
    })
  })
})
