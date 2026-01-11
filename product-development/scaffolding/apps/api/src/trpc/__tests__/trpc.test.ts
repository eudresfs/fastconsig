import { describe, it, expect, vi, beforeEach } from 'vitest'
import { protectedProcedure, withPermission, router } from '../trpc'

// Mock context
const mockPrisma = {
  usuario: {
    findUnique: vi.fn(),
  },
}

const mockCtx = {
  prisma: mockPrisma,
  userId: null,
  tenantId: null,
  req: {},
  res: {},
} as any

describe('TRPC Middleware', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    mockCtx.userId = null
    mockCtx.tenantId = null
  })

  describe('protectedProcedure', () => {
    it('should throw UNAUTHORIZED if no userId', async () => {
      const caller = router({
        test: protectedProcedure.query(() => 'success')
      }).createCaller(mockCtx)

      await expect(caller.test()).rejects.toThrow(/autenticado/)
    })

    it('should pass if authenticated', async () => {
      mockCtx.userId = 1
      mockCtx.tenantId = 1

      const caller = router({
        test: protectedProcedure.query(() => 'success')
      }).createCaller(mockCtx)

      await expect(caller.test()).resolves.toBe('success')
    })
  })

  describe('withPermission', () => {
    it('should throw FORBIDDEN if user lacks permission', async () => {
      mockCtx.userId = 1
      mockCtx.tenantId = 1

      mockPrisma.usuario.findUnique.mockResolvedValue({
        perfil: { permissoes: [] }
      } as any)

      const caller = router({
        test: protectedProcedure
          .use(withPermission('TEST_PERM'))
          .query(() => 'success')
      }).createCaller(mockCtx)

      await expect(caller.test()).rejects.toThrow(/permissao/)
    })

    it('should pass if user has permission', async () => {
      mockCtx.userId = 1
      mockCtx.tenantId = 1

      mockPrisma.usuario.findUnique.mockResolvedValue({
        perfil: {
          permissoes: [{ permissao: { codigo: 'TEST_PERM' } }]
        }
      } as any)

      const caller = router({
        test: protectedProcedure
          .use(withPermission('TEST_PERM'))
          .query(() => 'success')
      }).createCaller(mockCtx)

      await expect(caller.test()).resolves.toBe('success')
    })
  })
})
