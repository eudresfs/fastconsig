import { describe, it, expect, vi, beforeEach } from 'vitest'
import * as authService from '../auth.service'
import { prisma } from '@fastconsig/database/client'
import bcrypt from 'bcrypt'

// Mock dependencies
vi.mock('@fastconsig/database/client', () => ({
  prisma: {
    usuario: {
      findUnique: vi.fn(),
      update: vi.fn(),
    },
  },
}))

vi.mock('bcrypt', () => ({
  default: {
    compare: vi.fn(),
    hash: vi.fn(),
  },
}))

describe('AuthService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('validateCredentials', () => {
    it('should throw error if user not found', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue(null)

      await expect(
        authService.validateCredentials('wrong', 'pass')
      ).rejects.toThrow('Credenciais invalidas')
    })

    it('should throw error if user is inactive', async () => {
      vi.mocked(prisma.usuario.findUnique).mockResolvedValue({
        id: 1,
        login: 'user',
        senhaHash: 'hash',
        ativo: false,
        bloqueado: false,
        tentativasLogin: 0,
        perfil: { id: 1, nome: 'Admin', tipo: 'SISTEMA' },
      } as any)

      await expect(
        authService.validateCredentials('user', 'pass')
      ).rejects.toThrow('Usuario inativo')
    })
  })
})
