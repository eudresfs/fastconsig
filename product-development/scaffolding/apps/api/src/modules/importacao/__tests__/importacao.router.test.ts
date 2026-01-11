import { describe, it, expect, vi, beforeEach } from 'vitest'
import { importacaoRouter } from '../importacao.router'
import { TRPCError } from '@trpc/server'

const { mockPrisma } = vi.hoisted(() => {
  return {
    mockPrisma: {
      usuario: {
        findUnique: vi.fn(),
      },
      importacao: {
        findMany: vi.fn(),
        count: vi.fn(),
        findFirst: vi.fn(),
        create: vi.fn(),
        update: vi.fn(),
      },
    },
  }
})

vi.mock('@fastconsig/database/client', () => ({
  prisma: mockPrisma,
}))

describe('Importacao Router', () => {
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
          { permissao: { codigo: 'IMPORTACAO_VISUALIZAR' } },
          { permissao: { codigo: 'IMPORTACAO_CRIAR' } },
        ]
      }
    } as any)
  })

  describe('list', () => {
    it('should list importacoes', async () => {
      mockPrisma.importacao.findMany.mockResolvedValue([{ id: 1 }] as any)
      mockPrisma.importacao.count.mockResolvedValue(1)

      const caller = importacaoRouter.createCaller(mockCtx)
      const result = await caller.list({})

      expect(mockPrisma.importacao.findMany).toHaveBeenCalled()
      expect(result.data).toHaveLength(1)
    })
  })

  describe('getById', () => {
    it('should return importacao details', async () => {
      mockPrisma.importacao.findFirst.mockResolvedValue({ id: 1 } as any)

      const caller = importacaoRouter.createCaller(mockCtx)
      const result = await caller.getById({ id: 1 })

      expect(result.id).toBe(1)
    })

    it('should throw if not found', async () => {
      mockPrisma.importacao.findFirst.mockResolvedValue(null)

      const caller = importacaoRouter.createCaller(mockCtx)
      await expect(caller.getById({ id: 1 })).rejects.toThrow('Importacao nao encontrada')
    })
  })

  describe('criar', () => {
    it('should create importacao', async () => {
      mockPrisma.importacao.create.mockResolvedValue({ id: 1, status: 'PENDENTE' } as any)

      const caller = importacaoRouter.createCaller(mockCtx)
      const result = await caller.criar({
        tipo: 'FUNCIONARIOS',
        nomeArquivo: 'test.csv',
        tamanhoBytes: 1000
      })

      expect(mockPrisma.importacao.create).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({
          tipo: 'FUNCIONARIOS',
          status: 'PENDENTE'
        })
      }))
      expect(result.id).toBe(1)
    })
  })

  describe('cancelar', () => {
    it('should cancel pending importacao', async () => {
      mockPrisma.importacao.findFirst.mockResolvedValue({ id: 1, status: 'PENDENTE' } as any)
      mockPrisma.importacao.update.mockResolvedValue({ id: 1, status: 'CANCELADO' } as any)

      const caller = importacaoRouter.createCaller(mockCtx)
      const result = await caller.cancelar({ id: 1 })

      expect(mockPrisma.importacao.update).toHaveBeenCalledWith(expect.objectContaining({
        where: { id: 1 },
        data: { status: 'CANCELADO' }
      }))
      expect(result.status).toBe('CANCELADO')
    })

    it('should throw if not found', async () => {
      mockPrisma.importacao.findFirst.mockResolvedValue(null)

      const caller = importacaoRouter.createCaller(mockCtx)
      await expect(caller.cancelar({ id: 1 })).rejects.toThrow('Importacao nao encontrada')
    })

    it('should throw if not pending', async () => {
      mockPrisma.importacao.findFirst.mockResolvedValue({ id: 1, status: 'PROCESSANDO' } as any)

      const caller = importacaoRouter.createCaller(mockCtx)
      await expect(caller.cancelar({ id: 1 })).rejects.toThrow('Apenas importacoes pendentes')
    })
  })
})
