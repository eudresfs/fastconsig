import { describe, it, expect, vi, beforeEach } from 'vitest'
import {
  withTenant,
  withTenantFilter,
  validateTenantOwnership
} from './tenant.middleware'
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

describe('Tenant Middleware', () => {
  describe('withTenant', () => {
    const mockFindUnique = vi.fn()
    const mockNext = vi.fn()

    const mockCtx = {
      userId: 1,
      tenantId: 10,
      prisma: {
        usuario: {
          findUnique: mockFindUnique,
        },
      },
    } as unknown as Context

    beforeEach(() => {
      vi.clearAllMocks()
      mockNext.mockImplementation(({ ctx }) => Promise.resolve({ ctx }))
    })

    it('should set tenant context for valid user', async () => {
      mockFindUnique.mockResolvedValue({
        id: 1,
        tenantId: 10,
        consignatariaId: null,
        perfilId: 5,
        ativo: true,
        bloqueado: false,
        tenant: { id: 10, ativo: true }
      })

      const middleware = withTenant
      // @ts-ignore
      await middleware({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' })

      expect(mockNext).toHaveBeenCalledWith(expect.objectContaining({
        ctx: expect.objectContaining({
          tenant: {
            tenantId: 10,
            userId: 1,
            consignatariaId: null,
            perfilId: 5
          }
        })
      }))
    })

    it('should throw UNAUTHORIZED if no userId', async () => {
      const ctxNoUser = { ...mockCtx, userId: undefined } as unknown as Context
      // @ts-ignore
      await expect(withTenant({ ctx: ctxNoUser, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow('autenticado')
    })

    it('should throw FORBIDDEN if no tenantId in context', async () => {
      const ctxNoTenant = { ...mockCtx, tenantId: undefined } as unknown as Context
      // @ts-ignore
      await expect(withTenant({ ctx: ctxNoTenant, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow('nenhum tenant')
    })

    it('should throw UNAUTHORIZED if user not found', async () => {
      mockFindUnique.mockResolvedValue(null)
      // @ts-ignore
      await expect(withTenant({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow('Usuario nao encontrado')
    })

    it('should throw FORBIDDEN if user inactive', async () => {
      mockFindUnique.mockResolvedValue({
        id: 1,
        ativo: false,
        tenant: { ativo: true }
      })
      // @ts-ignore
      await expect(withTenant({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow('Usuario inativo')
    })

    it('should throw FORBIDDEN if user blocked', async () => {
      mockFindUnique.mockResolvedValue({
        id: 1,
        ativo: true,
        bloqueado: true,
        tenant: { ativo: true }
      })
      // @ts-ignore
      await expect(withTenant({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow('Usuario bloqueado')
    })

    it('should throw FORBIDDEN if tenant inactive', async () => {
      mockFindUnique.mockResolvedValue({
        id: 1,
        ativo: true,
        bloqueado: false,
        tenant: { ativo: false }
      })
      // @ts-ignore
      await expect(withTenant({ ctx: mockCtx, next: mockNext, meta: {}, rawInput: {}, path: '', type: 'query' }))
        .rejects.toThrow('Tenant inativo')
    })
  })

  describe('withTenantFilter', () => {
    it('should add tenantId to where clause', () => {
      const where = { name: 'John' }
      const result = withTenantFilter(10, where)

      expect(result).toEqual({
        name: 'John',
        tenantId: 10
      })
    })
  })

  describe('validateTenantOwnership', () => {
    it('should not throw if tenants match', () => {
      expect(() => validateTenantOwnership(10, 10, 'Resource')).not.toThrow()
    })

    it('should throw FORBIDDEN if tenants do not match', () => {
      expect(() => validateTenantOwnership(10, 20, 'Resource'))
        .toThrow('Resource nao pertence ao tenant atual')
    })

    it('should throw FORBIDDEN if resource tenant is null/undefined', () => {
      expect(() => validateTenantOwnership(undefined, 10, 'Resource'))
        .toThrow('Resource nao pertence ao tenant atual')
    })
  })
})
