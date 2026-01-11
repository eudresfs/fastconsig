import { describe, it, expect, vi, beforeEach } from 'vitest'
import {
  computeAuditDiff,
  sanitizeForAudit,
  logAuditAction,
  AuditActions,
} from './audit.middleware'
import type { Context } from '@/trpc/context'

describe('Audit Middleware', () => {
  describe('computeAuditDiff', () => {
    it('should return empty objects when no changes', () => {
      const oldData = { name: 'John', age: 30 }
      const newData = { name: 'John', age: 30 }

      const result = computeAuditDiff(oldData, newData)

      expect(result.dadosAnteriores).toEqual({})
      expect(result.dadosNovos).toEqual({})
    })

    it('should detect changed fields', () => {
      const oldData = { name: 'John', age: 30 }
      const newData = { name: 'John', age: 31 }

      const result = computeAuditDiff(oldData, newData)

      expect(result.dadosAnteriores).toEqual({ age: 30 })
      expect(result.dadosNovos).toEqual({ age: 31 })
    })

    it('should handle multiple changes', () => {
      const oldData = { name: 'John', age: 30, city: 'NY' }
      const newData = { name: 'Jane', age: 31, city: 'NY' }

      const result = computeAuditDiff(oldData, newData)

      expect(result.dadosAnteriores).toEqual({ name: 'John', age: 30 })
      expect(result.dadosNovos).toEqual({ name: 'Jane', age: 31 })
    })
  })

  describe('sanitizeForAudit', () => {
    it('should redact sensitive fields', () => {
      const data = {
        name: 'John',
        password: 'secret123',
        email: 'john@example.com',
        senha: '123',
        token: 'abc-def',
      }

      const result = sanitizeForAudit(data)

      expect(result).toEqual({
        name: 'John',
        password: '[REDACTED]',
        email: 'john@example.com',
        senha: '[REDACTED]',
        token: '[REDACTED]',
      })
    })

    it('should redact nested sensitive fields', () => {
      const data = {
        user: {
          name: 'John',
          credentials: {
            password: 'secret',
            refreshToken: 'token123'
          }
        }
      }

      const result = sanitizeForAudit(data)

      expect(result).toEqual({
        user: {
          name: 'John',
          credentials: {
            password: '[REDACTED]',
            refreshToken: '[REDACTED]'
          }
        }
      })
    })
  })

  describe('logAuditAction', () => {
    const mockCreate = vi.fn()
    const mockCtx = {
      prisma: {
        auditoria: {
          create: mockCreate,
        },
      },
      tenantId: 1,
      userId: 1,
      req: {
        headers: {
          'x-forwarded-for': '127.0.0.1',
          'user-agent': 'TestAgent',
        },
        ip: '127.0.0.1',
      },
    } as unknown as Context

    beforeEach(() => {
      mockCreate.mockClear()
    })

    it('should create audit log entry', async () => {
      const input = {
        entidade: 'User',
        acao: AuditActions.CRIAR,
        entidadeId: 1,
        dadosNovos: { name: 'John' },
      }

      await logAuditAction(mockCtx, input)

      expect(mockCreate).toHaveBeenCalledWith({
        data: {
          tenantId: 1,
          usuarioId: 1,
          entidade: 'User',
          entidadeId: 1,
          acao: 'CRIAR',
          dadosAnteriores: undefined,
          dadosNovos: { name: 'John' },
          ip: '127.0.0.1',
          userAgent: 'TestAgent',
        },
      })
    })

    it('should handle missing headers gracefully', async () => {
      const ctxWithoutHeaders = {
        ...mockCtx,
        req: {
          headers: {},
        },
      } as unknown as Context

      const input = {
        entidade: 'User',
        acao: AuditActions.LOGIN,
      }

      await logAuditAction(ctxWithoutHeaders, input)

      expect(mockCreate).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({
          ip: null,
          userAgent: null,
        }),
      }))
    })

    it('should swallow errors', async () => {
      mockCreate.mockRejectedValueOnce(new Error('DB Error'))
      const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {})

      await expect(logAuditAction(mockCtx, {
        entidade: 'User',
        acao: AuditActions.CRIAR,
      })).resolves.not.toThrow()

      expect(consoleSpy).toHaveBeenCalled()
      consoleSpy.mockRestore()
    })
  })
})
