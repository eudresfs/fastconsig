import { describe, it, expect, vi, beforeEach } from 'vitest'
import { authRouter } from '../auth.router'
import * as authService from '../auth.service'
import { prisma } from '@fastconsig/database/client'

// Mock dependencies
vi.mock('../auth.service', () => ({
  login: vi.fn(),
  alterarSenha: vi.fn(),
  logout: vi.fn(),
}))

vi.mock('@fastconsig/database/client', () => ({
  prisma: {
    sessao: {
      findUnique: vi.fn(),
    },
    usuario: {
      findUnique: vi.fn(),
    },
  },
}))

describe('Auth Router', () => {
  const mockSign = vi.fn()
  const mockCtx = {
    prisma,
    req: {
      server: {
        jwt: {
          sign: mockSign,
        },
      },
    },
    userId: 1,
    tenantId: 1, // Add tenantId required by protectedProcedure
  } as any

  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('login', () => {
    it('should call authService.login and return result', async () => {
      const mockResult = {
        accessToken: 'access',
        refreshToken: 'refresh',
        usuario: { id: 1, nome: 'User' },
      }

      vi.mocked(authService.login).mockImplementation(async (input, signCallback) => {
        // Simulate callback execution
        signCallback({ sub: 1 } as any, { expiresIn: '1h' })
        return mockResult as any
      })

      const caller = authRouter.createCaller(mockCtx)
      const result = await caller.login({ login: 'user', senha: 'password' })

      expect(authService.login).toHaveBeenCalled()
      expect(result).toEqual(mockResult)
      // The sign callback is called inside service, which we mocked to call our signCallback
      // But since we passed a lambda in the router:
      // (payload, options) => ctx.req.server.jwt.sign(payload, options)
      // And we mocked the service to call it.
      expect(mockSign).toHaveBeenCalled()
    })
  })

  describe('refresh', () => {
    it('should refresh token successfully', async () => {
      vi.mocked(prisma.sessao.findUnique).mockResolvedValue({
        id: 1,
        expiresAt: new Date(Date.now() + 10000), // Future
        usuario: {
          id: 1,
          tenantId: 10,
          consignatariaId: null,
          perfilId: 1,
        },
      } as any)

      mockSign.mockReturnValue('new_access_token')

      const caller = authRouter.createCaller(mockCtx)
      const result = await caller.refresh({ refreshToken: 'valid_refresh' })

      expect(prisma.sessao.findUnique).toHaveBeenCalled()
      expect(mockSign).toHaveBeenCalled()
      expect(result.accessToken).toBe('new_access_token')
    })

    it('should throw if session expired', async () => {
      vi.mocked(prisma.sessao.findUnique).mockResolvedValue({
        id: 1,
        expiresAt: new Date(Date.now() - 10000), // Past
        usuario: { id: 1 },
      } as any)

      const caller = authRouter.createCaller(mockCtx)
      await expect(caller.refresh({ refreshToken: 'expired' })).rejects.toThrow('Sessao expirada')
    })

    it('should throw if session not found', async () => {
      vi.mocked(prisma.sessao.findUnique).mockResolvedValue(null)

      const caller = authRouter.createCaller(mockCtx)
      await expect(caller.refresh({ refreshToken: 'invalid' })).rejects.toThrow('Sessao expirada')
    })
  })

  describe('alterarSenha', () => {
    it('should call authService.alterarSenha', async () => {
      const caller = authRouter.createCaller(mockCtx)
      const input = {
        senhaAtual: 'oldPass123',
        novaSenha: 'newPass123',
        confirmarSenha: 'newPass123',
      }
      await caller.alterarSenha(input)
      expect(authService.alterarSenha).toHaveBeenCalledWith(1, input)
    })
  })

  describe('logout', () => {
    it('should call authService.logout', async () => {
      const caller = authRouter.createCaller(mockCtx)
      await caller.logout({ refreshToken: 'token' })
      expect(authService.logout).toHaveBeenCalledWith('token')
    })
  })

  describe('me', () => {
    it('should return user profile', async () => {
      const mockUser = {
        id: 1,
        nome: 'User',
        email: 'email@example.com',
        login: 'user',
        primeiroAcesso: false,
        perfil: {
          id: 1,
          nome: 'Admin',
          tipo: 'SISTEMA',
          permissoes: [
            { permissao: { codigo: 'PERM1' } },
            { permissao: { codigo: 'PERM2' } },
          ],
        },
        tenant: { id: 10, nome: 'Tenant' },
        consignataria: null,
      }

      vi.mocked(prisma.usuario.findUnique).mockResolvedValue(mockUser as any)

      const caller = authRouter.createCaller(mockCtx)
      const result = await caller.me()

      expect(result.id).toBe(1)
      expect(result.nome).toBe('User')
      expect(result.permissoes).toEqual(['PERM1', 'PERM2'])
      expect(result.tenant).toEqual({ id: 10, nome: 'Tenant' })
    })

    it('should return user profile with consignataria', async () => {
      const mockUser = {
        id: 1,
        nome: 'User',
        email: 'email@example.com',
        login: 'user',
        primeiroAcesso: false,
        perfil: {
          id: 1,
          nome: 'Consignataria',
          tipo: 'CONSIGNATARIA',
          permissoes: [],
        },
        tenant: null,
        consignataria: { id: 20, razaoSocial: 'Consignataria Ltd' },
      }

      vi.mocked(prisma.usuario.findUnique).mockResolvedValue(mockUser as any)

      const caller = authRouter.createCaller(mockCtx)
      const result = await caller.me()

      expect(result.id).toBe(1)
      expect(result.tenant).toBeNull()
      expect(result.consignataria).toEqual({ id: 20, razaoSocial: 'Consignataria Ltd' })
    })

    it('should throw if user not found', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue(null)

      const caller = authRouter.createCaller(mockCtx)
      await expect(caller.me()).rejects.toThrow('Usuario nao encontrado')
    })
  })
})
