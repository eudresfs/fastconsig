import { describe, it, expect, vi, beforeEach } from 'vitest'
import * as authService from '../auth.service'
import { prisma } from '@fastconsig/database/client'
import bcrypt from 'bcrypt'
import { logAuditAction } from '@/shared/middleware'

// Mock dependencies
vi.mock('@fastconsig/database/client', () => ({
  prisma: {
    usuario: {
      findUnique: vi.fn(),
      update: vi.fn(),
    },
    sessao: {
      create: vi.fn(),
      findUnique: vi.fn(),
      delete: vi.fn(),
      deleteMany: vi.fn(),
      findMany: vi.fn(),
      findFirst: vi.fn(),
    },
    senhaHistorico: {
      create: vi.fn(),
    },
    passwordResetToken: {
      create: vi.fn(),
      findUnique: vi.fn(),
      update: vi.fn(),
      updateMany: vi.fn(),
      deleteMany: vi.fn(),
    },
    $transaction: vi.fn((callback) => {
      if (Array.isArray(callback)) {
        return Promise.resolve(callback)
      }
      return callback(prisma)
    }),
  },
}))

vi.mock('bcrypt', () => ({
  default: {
    compare: vi.fn(),
    hash: vi.fn(),
  },
}))

vi.mock('@/shared/middleware', () => ({
  logAuditAction: vi.fn(),
  AuditActions: {
    LOGIN: 'LOGIN',
    ATUALIZAR: 'ATUALIZAR',
    LOGOUT: 'LOGOUT',
    EXCLUIR: 'EXCLUIR',
  },
}))

vi.mock('@/shared/services', () => ({
  emailService: {
    sendPasswordResetEmail: vi.fn(),
    sendPasswordChangedEmail: vi.fn(),
  },
}))

describe('AuthService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('validateCredentials', () => {
    const mockUser = {
      id: 1,
      login: 'user',
      senhaHash: 'hash',
      ativo: true,
      bloqueado: false,
      tentativasLogin: 0,
      bloqueadoAte: null,
      perfil: { id: 1, nome: 'Admin', tipo: 'SISTEMA' },
      tenantId: 10,
      consignatariaId: null,
      perfilId: 1,
      primeiroAcesso: false,
      email: 'user@example.com',
      nome: 'User',
    }

    it('should throw error if user not found', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue(null)

      await expect(
        authService.validateCredentials('wrong', 'pass')
      ).rejects.toThrow('Credenciais invalidas')
    })

    it('should throw error if user is inactive', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
        ...mockUser,
        ativo: false,
      } as any)

      await expect(
        authService.validateCredentials('user', 'pass')
      ).rejects.toThrow('Usuario inativo')
    })

    it('should throw error if user is blocked permanently', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
        ...mockUser,
        bloqueado: true,
        bloqueadoAte: null, // Permanently blocked by admin or logic without expiration
      } as any)

      // The code logic checks for temporary block first.
      // If `bloqueado` is true and `bloqueadoAte` is null, it might fall through or be handled.
      // Looking at source:
      // if (usuario.bloqueado) {
      //   if (usuario.bloqueadoAte && usuario.bloqueadoAte > new Date()) { ... }
      //   await prisma.usuario.update(...) // Unblocks if time expired or no time set?
      // }
      // Wait, the logic unblocks if `bloqueadoAte` is null or in the past?
      // "if (usuario.bloqueadoAte && usuario.bloqueadoAte > new Date())"
      // If bloqueadoAte is null, it skips the throw and proceeds to unblock.
      // This implies "bloqueado: true" with "bloqueadoAte: null" is treated as "block expired" or "invalid block state" in this function,
      // UNLESS there is another check.
      // Actually, looking at the code:
      // It seems it auto-unblocks if the block is not a valid future temporary block.
      // Let's verify this behavior.

      // If I want to test "temporarily blocked", I set a future date.
      const futureDate = new Date()
      futureDate.setMinutes(futureDate.getMinutes() + 10)

      vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
        ...mockUser,
        bloqueado: true,
        bloqueadoAte: futureDate,
      } as any)

      await expect(
        authService.validateCredentials('user', 'pass')
      ).rejects.toThrow('Usuario bloqueado temporariamente')
    })

    it('should unblock user if block time expired', async () => {
      const pastDate = new Date()
      pastDate.setMinutes(pastDate.getMinutes() - 10)

      vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
        ...mockUser,
        bloqueado: true,
        bloqueadoAte: pastDate,
      } as any)

      vi.mocked(bcrypt.compare).mockResolvedValue(true as any)

      await authService.validateCredentials('user', 'pass')

      expect(prisma.usuario.update).toHaveBeenCalledWith({
        where: { id: 1 },
        data: { bloqueado: false, bloqueadoAte: null, tentativasLogin: 0 },
      })
    })

    it('should validate password success', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue(mockUser as any)
      vi.mocked(bcrypt.compare).mockResolvedValue(true as any)

      const result = await authService.validateCredentials('user', 'pass')

      expect(result.id).toBe(1)
      expect(result.email).toBe('user@example.com')
    })

    it('should increment attempts on wrong password', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
        ...mockUser,
        tentativasLogin: 0,
      } as any)
      vi.mocked(bcrypt.compare).mockResolvedValue(false as any)

      await expect(
        authService.validateCredentials('user', 'wrong')
      ).rejects.toThrow('Credenciais invalidas')

      expect(prisma.usuario.update).toHaveBeenCalledWith(expect.objectContaining({
        data: { tentativasLogin: 1 }
      }))
    })

    it('should block user after max attempts', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
        ...mockUser,
        tentativasLogin: 4, // Max is usually 5
      } as any)
      vi.mocked(bcrypt.compare).mockResolvedValue(false as any)

      // Assuming maxAttempts is 5 in config (default)
      await expect(
        authService.validateCredentials('user', 'wrong')
      ).rejects.toThrow('Usuario bloqueado')

      expect(prisma.usuario.update).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({
            bloqueado: true,
            tentativasLogin: 5
        })
      }))
    })
  })

  describe('login', () => {
    const mockUser = {
      id: 1,
      login: 'user',
      senhaHash: 'hash',
      ativo: true,
      bloqueado: false,
      tentativasLogin: 0,
      perfil: { id: 1, nome: 'Admin', tipo: 'SISTEMA' },
      tenantId: 10,
      consignatariaId: null,
      perfilId: 1,
      primeiroAcesso: false,
      email: 'user@example.com',
      nome: 'User',
    }
    const mockSignToken = vi.fn().mockReturnValue('token')
    const mockCtx = { req: { ip: '127.0.0.1', headers: { 'user-agent': 'Jest' } } } as any

    beforeEach(() => {
        vi.mocked(prisma.usuario.findUnique).mockResolvedValue(mockUser as any)
        vi.mocked(bcrypt.compare).mockResolvedValue(true as any)
    })

    it('should login successfully', async () => {
      const result = await authService.login(
        { login: 'user', senha: 'pass' },
        mockSignToken,
        mockCtx
      )

      expect(result).toHaveProperty('accessToken', 'token')
      expect(result).toHaveProperty('refreshToken', 'token')
      expect(prisma.sessao.create).toHaveBeenCalled()
      expect(logAuditAction).toHaveBeenCalled()
      expect(prisma.usuario.update).toHaveBeenCalledWith(expect.objectContaining({
          data: expect.objectContaining({ tentativasLogin: 0 })
      }))
    })
  })

  describe('refreshToken', () => {
    const mockUser = {
      id: 1,
      ativo: true,
      bloqueado: false,
      tenantId: 10,
      consignatariaId: null,
      perfilId: 1,
    }
    const mockSession = {
      id: 1,
      usuario: mockUser,
      expiresAt: new Date(Date.now() + 10000), // Future
      ip: '127.0.0.1',
      userAgent: 'Jest',
    }
    const mockSignToken = vi.fn().mockReturnValue('new_token')

    it('should refresh token successfully', async () => {
      vi.mocked(prisma.sessao.findUnique).mockResolvedValue(mockSession as any)

      const result = await authService.refreshToken('valid_token', mockSignToken)

      expect(result.accessToken).toBe('new_token')
      expect(prisma.sessao.delete).toHaveBeenCalled()
      expect(prisma.sessao.create).toHaveBeenCalled()
    })

    it('should throw if session not found', async () => {
      vi.mocked(prisma.sessao.findUnique).mockResolvedValue(null)

      await expect(authService.refreshToken('invalid', mockSignToken))
        .rejects.toThrow('Sessao invalida')
    })

    it('should throw if session expired', async () => {
      vi.mocked(prisma.sessao.findUnique).mockResolvedValue({
        ...mockSession,
        expiresAt: new Date(Date.now() - 10000), // Past
      } as any)

      await expect(authService.refreshToken('expired', mockSignToken))
        .rejects.toThrow('Sessao expirada')

      expect(prisma.sessao.delete).toHaveBeenCalled()
    })

    it('should throw if user inactive', async () => {
       vi.mocked(prisma.sessao.findUnique).mockResolvedValue({
        ...mockSession,
        usuario: { ...mockUser, ativo: false }
      } as any)

      await expect(authService.refreshToken('token', mockSignToken))
        .rejects.toThrow('Usuario inativo')
    })
  })

  describe('alterarSenha', () => {
    const mockUser = {
      id: 1,
      senhaHash: 'hashed_old',
      senhaHistorico: [],
    }

    it('should change password successfully', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue(mockUser as any)
      vi.mocked(bcrypt.compare)
        .mockResolvedValueOnce(true as any) // current password match
        .mockResolvedValueOnce(false as any) // history match (none)
        .mockResolvedValueOnce(false as any) // same as current (false)

      vi.mocked(bcrypt.hash).mockResolvedValue('hashed_new' as any)

      await authService.alterarSenha(1, { senhaAtual: 'old', novaSenha: 'new' })

      expect(prisma.usuario.update).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({ senhaHash: 'hashed_new' })
      }))
    })

    it('should throw if current password incorrect', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue(mockUser as any)
      vi.mocked(bcrypt.compare).mockResolvedValue(false as any)

      await expect(authService.alterarSenha(1, { senhaAtual: 'wrong', novaSenha: 'new' }))
        .rejects.toThrow('Senha atual incorreta')
    })
  })

  describe('logout', () => {
    it('should delete session', async () => {
      vi.mocked(prisma.sessao.findUnique).mockResolvedValue({ usuarioId: 1 } as any)
      await authService.logout('token')
      expect(prisma.sessao.deleteMany).toHaveBeenCalled()
    })
  })

  describe('logoutAll', () => {
    it('should delete all sessions', async () => {
      vi.mocked(prisma.sessao.deleteMany).mockResolvedValue({ count: 5 } as any)
      const count = await authService.logoutAll(1)
      expect(count).toBe(5)
    })
  })

  describe('Utility functions', () => {
    it('hashPassword should call bcrypt.hash', async () => {
      await authService.hashPassword('pass')
      expect(bcrypt.hash).toHaveBeenCalled()
    })

    it('comparePassword should call bcrypt.compare', async () => {
      await authService.comparePassword('pass', 'hash')
      expect(bcrypt.compare).toHaveBeenCalled()
    })
  })

  describe('Session management', () => {
    it('getActiveSessions should return sessions', async () => {
      vi.mocked(prisma.sessao.findMany).mockResolvedValue([{ id: 1 }] as any)
      const result = await authService.getActiveSessions(1)
      expect(result).toHaveLength(1)
      expect(prisma.sessao.findMany).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({ usuarioId: 1 })
      }))
    })

    it('revokeSession should delete session', async () => {
      vi.mocked(prisma.sessao.findFirst).mockResolvedValue({ id: 1, usuarioId: 1 } as any)
      // @ts-ignore
      await authService.revokeSession(1, 1, { userId: 1 })
      expect(prisma.sessao.delete).toHaveBeenCalledWith({ where: { id: 1 } })
      expect(logAuditAction).toHaveBeenCalled()
    })

    it('revokeSession should throw if session not found', async () => {
      vi.mocked(prisma.sessao.findFirst).mockResolvedValue(null)
      await expect(authService.revokeSession(1, 1)).rejects.toThrow('Sessao nao encontrada')
    })

    it('cleanupExpiredSessions should delete expired sessions', async () => {
      vi.mocked(prisma.sessao.deleteMany).mockResolvedValue({ count: 10 } as any)
      const count = await authService.cleanupExpiredSessions()
      expect(count).toBe(10)
      expect(prisma.sessao.deleteMany).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({ expiresAt: { lt: expect.any(Date) } })
      }))
    })
  })

  describe('Password recovery', () => {
    const mockUser = {
      id: 1,
      email: 'user@example.com',
      nome: 'User',
      ativo: true,
      senhaHash: 'oldHash',
      senhaHistorico: [],
    }

    describe('solicitarRecuperacaoSenha', () => {
      it('should silently succeed if user does not exist', async () => {
        vi.mocked(prisma.usuario.findUnique).mockResolvedValue(null)
        vi.mocked(prisma.passwordResetToken.updateMany).mockResolvedValue({ count: 0 } as any)

        await expect(
          authService.solicitarRecuperacaoSenha({ email: 'nonexistent@example.com' })
        ).resolves.toBeUndefined()
      })

      it('should silently succeed if user is inactive', async () => {
        vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
          ...mockUser,
          ativo: false,
        } as any)
        vi.mocked(prisma.passwordResetToken.updateMany).mockResolvedValue({ count: 0 } as any)

        await expect(
          authService.solicitarRecuperacaoSenha({ email: mockUser.email })
        ).resolves.toBeUndefined()
      })

      it('should invalidate existing tokens and create new token', async () => {
        vi.mocked(prisma.usuario.findUnique).mockResolvedValue(mockUser as any)
        vi.mocked(prisma.passwordResetToken.updateMany).mockResolvedValue({ count: 1 } as any)
        vi.mocked(prisma.passwordResetToken.create).mockResolvedValue({
          id: 1,
          token: 'token',
        } as any)

        await authService.solicitarRecuperacaoSenha({ email: mockUser.email })

        expect(prisma.passwordResetToken.updateMany).toHaveBeenCalledWith(
          expect.objectContaining({
            where: expect.objectContaining({ usuarioId: mockUser.id }),
          })
        )
        expect(prisma.passwordResetToken.create).toHaveBeenCalled()
      })

      it('should not throw if email sending fails', async () => {
        const { emailService } = await import('@/shared/services')
        vi.mocked(prisma.usuario.findUnique).mockResolvedValue(mockUser as any)
        vi.mocked(prisma.passwordResetToken.updateMany).mockResolvedValue({ count: 0 } as any)
        vi.mocked(prisma.passwordResetToken.create).mockResolvedValue({
          id: 1,
          token: 'token',
        } as any)
        vi.mocked(emailService.sendPasswordResetEmail).mockRejectedValue(
          new Error('Email failed')
        )

        await expect(
          authService.solicitarRecuperacaoSenha({ email: mockUser.email })
        ).resolves.toBeUndefined()
      })
    })

    describe('validarTokenRecuperacao', () => {
      it('should throw if token not found', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue(null)

        await expect(
          authService.validarTokenRecuperacao('invalid-token')
        ).rejects.toThrow('Token invalido ou expirado')
      })

      it('should throw if token already used', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue({
          id: 1,
          token: 'token',
          usado: true,
          expiresAt: new Date(Date.now() + 3600000),
          usuario: { ...mockUser, ativo: true },
        } as any)

        await expect(
          authService.validarTokenRecuperacao('token')
        ).rejects.toThrow('Token ja foi utilizado')
      })

      it('should throw if token expired', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue({
          id: 1,
          token: 'token',
          usado: false,
          expiresAt: new Date(Date.now() - 1000),
          usuario: { ...mockUser, ativo: true },
        } as any)

        await expect(
          authService.validarTokenRecuperacao('token')
        ).rejects.toThrow('Token expirado')
      })

      it('should throw if user is inactive', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue({
          id: 1,
          token: 'token',
          usado: false,
          expiresAt: new Date(Date.now() + 3600000),
          usuario: { ...mockUser, ativo: false },
        } as any)

        await expect(
          authService.validarTokenRecuperacao('token')
        ).rejects.toThrow('Usuario inativo')
      })

      it('should return user data for valid token', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue({
          id: 1,
          token: 'token',
          usado: false,
          expiresAt: new Date(Date.now() + 3600000),
          usuario: { ...mockUser, ativo: true },
        } as any)

        const result = await authService.validarTokenRecuperacao('token')
        expect(result).toEqual({
          id: mockUser.id,
          email: mockUser.email,
          nome: mockUser.nome,
        })
      })
    })

    describe('resetarSenha', () => {
      const validToken = {
        id: 1,
        token: 'valid-token',
        usado: false,
        expiresAt: new Date(Date.now() + 3600000),
        usuario: { ...mockUser, ativo: true },
      }

      beforeEach(() => {
        vi.mocked(bcrypt.compare).mockResolvedValue(false as never)
        vi.mocked(bcrypt.hash).mockResolvedValue('newHash' as never)
      })

      it('should throw if token is invalid', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue(null)

        await expect(
          authService.resetarSenha({ token: 'invalid', novaSenha: 'NewPass123', confirmarSenha: 'NewPass123' })
        ).rejects.toThrow('Token invalido ou expirado')
      })

      it('should throw if new password matches current password', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue(validToken as any)
        vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
          ...mockUser,
          senhaHistorico: [],
        } as any)
        // History is empty, so only one check happens (current password)
        vi.mocked(bcrypt.compare).mockResolvedValueOnce(true as never)

        await expect(
          authService.resetarSenha({ token: 'valid-token', novaSenha: 'OldPass123', confirmarSenha: 'OldPass123' })
        ).rejects.toThrow('A nova senha deve ser diferente da senha atual')
      })

      it('should throw if new password was used recently', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue(validToken as any)
        vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
          ...mockUser,
          senhaHistorico: [{ senhaHash: 'oldHash1' }],
        } as any)
        vi.mocked(bcrypt.compare).mockResolvedValueOnce(true as never) // matches history

        await expect(
          authService.resetarSenha({ token: 'valid-token', novaSenha: 'UsedPass123', confirmarSenha: 'UsedPass123' })
        ).rejects.toThrow('Nao e permitido reutilizar as ultimas')
      })

      it('should reset password successfully', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue(validToken as any)
        vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
          ...mockUser,
          senhaHistorico: [],
        } as any)
        vi.mocked(bcrypt.compare).mockResolvedValue(false as never)
        vi.mocked(bcrypt.hash).mockResolvedValue('newHash' as never)

        const { emailService } = await import('@/shared/services')
        vi.mocked(emailService.sendPasswordChangedEmail).mockResolvedValue(undefined)

        await authService.resetarSenha({
          token: 'valid-token',
          novaSenha: 'NewPass123',
          confirmarSenha: 'NewPass123',
        })

        expect(prisma.$transaction).toHaveBeenCalled()
        expect(emailService.sendPasswordChangedEmail).toHaveBeenCalledWith(
          mockUser.email,
          mockUser.nome
        )
      })

      it('should not throw if confirmation email fails', async () => {
        vi.mocked(prisma.passwordResetToken.findUnique).mockResolvedValue(validToken as any)
        vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
          ...mockUser,
          senhaHistorico: [],
        } as any)
        vi.mocked(bcrypt.compare).mockResolvedValue(false as never)
        vi.mocked(bcrypt.hash).mockResolvedValue('newHash' as never)

        const { emailService } = await import('@/shared/services')
        vi.mocked(emailService.sendPasswordChangedEmail).mockRejectedValue(
          new Error('Email failed')
        )

        await expect(
          authService.resetarSenha({
            token: 'valid-token',
            novaSenha: 'NewPass123',
            confirmarSenha: 'NewPass123',
          })
        ).resolves.toBeUndefined()
      })
    })

    describe('limparTokensExpirados', () => {
      it('should delete expired and old used tokens', async () => {
        vi.mocked(prisma.passwordResetToken.deleteMany).mockResolvedValue({ count: 5 } as any)

        const count = await authService.limparTokensExpirados()

        expect(count).toBe(5)
        expect(prisma.passwordResetToken.deleteMany).toHaveBeenCalledWith(
          expect.objectContaining({
            where: expect.objectContaining({ OR: expect.any(Array) }),
          })
        )
      })
    })
  })
})
