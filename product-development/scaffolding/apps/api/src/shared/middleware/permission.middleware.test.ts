import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import {
  withPermission,
  withAnyPermission,
  withAllPermissions,
  PermissionCodes,
  clearAllPermissionCache,
  checkPermission,
  getAllUserPermissions
} from './permission.middleware'
import { TRPCError } from '@trpc/server'
import type { Context } from '@/trpc/context'

// Mock the middleware function from trpc
vi.mock('@/trpc/trpc', () => ({
  middleware: (fn: any) => fn,
  initTRPC: {
    context: () => ({
      create: () => ({
        router: vi.fn(),
        procedure: vi.fn(),
        middleware: (fn: any) => fn
      })
    })
  }
}))

describe('Permission Middleware', () => {
  const mockFindUnique = vi.fn()
  const mockNext = vi.fn()

  const mockCtx = {
    userId: 1,
    prisma: {
      usuario: {
        findUnique: mockFindUnique,
      },
    },
  } as unknown as Context

  beforeEach(() => {
    vi.clearAllMocks()
    clearAllPermissionCache()
    mockNext.mockImplementation(({ ctx }) => Promise.resolve({ ctx }))
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  // Helper to setup permissions for a user
  const setupPermissions = (permissionCodes: string[]) => {
    mockFindUnique.mockResolvedValue({
      perfil: {
        permissoes: permissionCodes.map(code => ({
          permissao: { codigo: code }
        }))
      }
    })
  }

  describe('withPermission', () => {
    it('should allow access when user has required permission', async () => {
      setupPermissions([PermissionCodes.FUNCIONARIOS_VISUALIZAR])

      const middleware = withPermission(PermissionCodes.FUNCIONARIOS_VISUALIZAR)
      // @ts-ignore - simulating TRPC middleware call
      await middleware({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' })

      expect(mockNext).toHaveBeenCalled()
    })

    it('should deny access when user missing permission', async () => {
      setupPermissions(['OTHER_PERMISSION'])

      const middleware = withPermission(PermissionCodes.FUNCIONARIOS_VISUALIZAR)

      // @ts-ignore
      await expect(middleware({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow(TRPCError)
    })

    it('should throw UNAUTHORIZED if no userId', async () => {
      const ctxNoUser = { ...mockCtx, userId: undefined } as unknown as Context
      const middleware = withPermission(PermissionCodes.FUNCIONARIOS_VISUALIZAR)

      // @ts-ignore
      await expect(middleware({ ctx: ctxNoUser, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow(/autenticado/)
    })
  })

  describe('withAnyPermission', () => {
    it('should allow access when user has one of the permissions', async () => {
      setupPermissions([PermissionCodes.FUNCIONARIOS_VISUALIZAR])

      const middleware = withAnyPermission([
        PermissionCodes.FUNCIONARIOS_VISUALIZAR,
        PermissionCodes.FUNCIONARIOS_CRIAR
      ])

      // @ts-ignore
      await middleware({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' })
      expect(mockNext).toHaveBeenCalled()
    })

    it('should deny access when user has none of the permissions', async () => {
      setupPermissions(['OTHER_PERMISSION'])

      const middleware = withAnyPermission([
        PermissionCodes.FUNCIONARIOS_VISUALIZAR,
        PermissionCodes.FUNCIONARIOS_CRIAR
      ])

      // @ts-ignore
      await expect(middleware({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow(TRPCError)
    })
  })

  describe('withAllPermissions', () => {
    it('should allow access when user has ALL permissions', async () => {
      setupPermissions([
        PermissionCodes.FUNCIONARIOS_VISUALIZAR,
        PermissionCodes.FUNCIONARIOS_CRIAR
      ])

      const middleware = withAllPermissions([
        PermissionCodes.FUNCIONARIOS_VISUALIZAR,
        PermissionCodes.FUNCIONARIOS_CRIAR
      ])

      // @ts-ignore
      await middleware({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' })
      expect(mockNext).toHaveBeenCalled()
    })

    it('should deny access when user is missing one permission', async () => {
      setupPermissions([PermissionCodes.FUNCIONARIOS_VISUALIZAR])

      const middleware = withAllPermissions([
        PermissionCodes.FUNCIONARIOS_VISUALIZAR,
        PermissionCodes.FUNCIONARIOS_CRIAR
      ])

      // @ts-ignore
      await expect(middleware({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow(TRPCError)
    })
  })

  describe('Helper Functions', () => {
    it('checkPermission should return true if user has permission', async () => {
      setupPermissions([PermissionCodes.FUNCIONARIOS_VISUALIZAR])
      const result = await checkPermission(mockCtx, 1, PermissionCodes.FUNCIONARIOS_VISUALIZAR)
      expect(result).toBe(true)
    })

    it('checkPermission should return false if user missing permission', async () => {
      setupPermissions(['OTHER'])
      const result = await checkPermission(mockCtx, 1, PermissionCodes.FUNCIONARIOS_VISUALIZAR)
      expect(result).toBe(false)
    })

    it('getAllUserPermissions should return all permissions', async () => {
      const perms = ['PERM_A', 'PERM_B']
      setupPermissions(perms)
      const result = await getAllUserPermissions(mockCtx, 1)
      expect(result).toEqual(expect.arrayContaining(perms))
      expect(result.length).toBe(2)
    })
  })

  describe('Caching', () => {
    it('should use cache for subsequent requests', async () => {
      setupPermissions([PermissionCodes.FUNCIONARIOS_VISUALIZAR])

      // First call hits DB
      await checkPermission(mockCtx, 1, PermissionCodes.FUNCIONARIOS_VISUALIZAR)
      expect(mockFindUnique).toHaveBeenCalledTimes(1)

      // Second call should hit cache
      await checkPermission(mockCtx, 1, PermissionCodes.FUNCIONARIOS_VISUALIZAR)
      expect(mockFindUnique).toHaveBeenCalledTimes(1)
    })

    it('should clear cache when requested', async () => {
      setupPermissions([PermissionCodes.FUNCIONARIOS_VISUALIZAR])

      await checkPermission(mockCtx, 1, PermissionCodes.FUNCIONARIOS_VISUALIZAR)
      clearAllPermissionCache()
      await checkPermission(mockCtx, 1, PermissionCodes.FUNCIONARIOS_VISUALIZAR)

      expect(mockFindUnique).toHaveBeenCalledTimes(2)
    })
  })
})
