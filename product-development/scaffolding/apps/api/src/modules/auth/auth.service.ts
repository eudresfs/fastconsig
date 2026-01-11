import bcrypt from 'bcrypt'
import { TRPCError } from '@trpc/server'
import { prisma } from '@fastconsig/database/client'
import { authConfig } from '@/config/auth'
import { type LoginInput, type AlterarSenhaInput } from './auth.schema'
import { logAuditAction, AuditActions } from '@/shared/middleware'
import type { Context } from '@/trpc/context'

/**
 * Token payload structure for JWT
 */
export interface TokenPayload {
  sub: number
  tenantId: number | null
  consignatariaId: number | null
  perfilId: number
}

/**
 * Login response structure
 */
export interface LoginResponse {
  accessToken: string
  refreshToken: string
  usuario: {
    id: number
    nome: string
    email: string
    primeiroAcesso: boolean
    perfil: {
      id: number
      nome: string
      tipo: string
    }
    tenantId: number | null
    consignatariaId: number | null
  }
}

/**
 * Validates user credentials
 * Returns the user if valid, throws if invalid
 */
export async function validateCredentials(
  login: string,
  senha: string
): Promise<{
  id: number
  nome: string
  email: string
  tenantId: number | null
  consignatariaId: number | null
  perfilId: number
  primeiroAcesso: boolean
  senhaHash: string
  perfil: {
    id: number
    nome: string
    tipo: string
  }
}> {
  const usuario = await prisma.usuario.findUnique({
    where: { login },
    include: {
      perfil: {
        select: {
          id: true,
          nome: true,
          tipo: true,
        },
      },
    },
  })

  if (!usuario) {
    throw new TRPCError({
      code: 'UNAUTHORIZED',
      message: 'Credenciais invalidas',
    })
  }

  // Check if user is blocked
  if (usuario.bloqueado) {
    if (usuario.bloqueadoAte && usuario.bloqueadoAte > new Date()) {
      const remainingMinutes = Math.ceil(
        (usuario.bloqueadoAte.getTime() - Date.now()) / (1000 * 60)
      )
      throw new TRPCError({
        code: 'FORBIDDEN',
        message: `Usuario bloqueado temporariamente. Tente novamente em ${remainingMinutes} minutos.`,
      })
    }
    // Unblock if time expired
    await prisma.usuario.update({
      where: { id: usuario.id },
      data: { bloqueado: false, bloqueadoAte: null, tentativasLogin: 0 },
    })
  }

  // Check if user is active
  if (!usuario.ativo) {
    throw new TRPCError({
      code: 'FORBIDDEN',
      message: 'Usuario inativo',
    })
  }

  // Validate password
  const senhaValida = await bcrypt.compare(senha, usuario.senhaHash)

  if (!senhaValida) {
    const tentativas = usuario.tentativasLogin + 1

    if (tentativas >= authConfig.lockout.maxAttempts) {
      const bloqueadoAte = new Date()
      bloqueadoAte.setMinutes(bloqueadoAte.getMinutes() + authConfig.lockout.durationMinutes)

      await prisma.usuario.update({
        where: { id: usuario.id },
        data: { bloqueado: true, bloqueadoAte, tentativasLogin: tentativas },
      })

      throw new TRPCError({
        code: 'FORBIDDEN',
        message: `Usuario bloqueado por ${authConfig.lockout.durationMinutes} minutos apos ${authConfig.lockout.maxAttempts} tentativas invalidas`,
      })
    }

    await prisma.usuario.update({
      where: { id: usuario.id },
      data: { tentativasLogin: tentativas },
    })

    throw new TRPCError({
      code: 'UNAUTHORIZED',
      message: 'Credenciais invalidas',
    })
  }

  return {
    id: usuario.id,
    nome: usuario.nome,
    email: usuario.email,
    tenantId: usuario.tenantId,
    consignatariaId: usuario.consignatariaId,
    perfilId: usuario.perfilId,
    primeiroAcesso: usuario.primeiroAcesso,
    senhaHash: usuario.senhaHash,
    perfil: usuario.perfil,
  }
}

/**
 * Hashes a password using bcrypt
 */
export async function hashPassword(password: string): Promise<string> {
  return bcrypt.hash(password, authConfig.password.saltRounds)
}

/**
 * Compares a plain text password with a hash
 */
export async function comparePassword(password: string, hash: string): Promise<boolean> {
  return bcrypt.compare(password, hash)
}

/**
 * Performs user login
 */
export async function login(
  input: LoginInput,
  signToken: (payload: TokenPayload, options: { expiresIn: string }) => string,
  ctx?: Context
): Promise<LoginResponse> {
  const usuario = await validateCredentials(input.login, input.senha)

  // Reset login attempts and update last login
  await prisma.usuario.update({
    where: { id: usuario.id },
    data: {
      tentativasLogin: 0,
      ultimoLogin: new Date(),
    },
  })

  const tokenPayload: TokenPayload = {
    sub: usuario.id,
    tenantId: usuario.tenantId,
    consignatariaId: usuario.consignatariaId,
    perfilId: usuario.perfilId,
  }

  const accessToken = signToken(tokenPayload, {
    expiresIn: authConfig.accessToken.expiresIn,
  })

  const refreshToken = signToken(tokenPayload, {
    expiresIn: authConfig.refreshToken.expiresIn,
  })

  // Calculate refresh token expiry
  const refreshTokenExpiresAt = new Date()
  const daysMatch = authConfig.refreshToken.expiresIn.match(/(\d+)d/)
  const days = daysMatch ? parseInt(daysMatch[1], 10) : 7
  refreshTokenExpiresAt.setDate(refreshTokenExpiresAt.getDate() + days)

  // Save session
  await prisma.sessao.create({
    data: {
      usuarioId: usuario.id,
      refreshToken,
      expiresAt: refreshTokenExpiresAt,
      ip: ctx?.req.ip ?? null,
      userAgent: ctx?.req.headers['user-agent']?.substring(0, 500) ?? null,
    },
  })

  // Log audit action
  if (ctx) {
    await logAuditAction(ctx, {
      entidade: 'Usuario',
      entidadeId: usuario.id,
      acao: AuditActions.LOGIN,
      dadosNovos: {
        login: input.login,
        timestamp: new Date().toISOString(),
      },
    })
  }

  return {
    accessToken,
    refreshToken,
    usuario: {
      id: usuario.id,
      nome: usuario.nome,
      email: usuario.email,
      primeiroAcesso: usuario.primeiroAcesso,
      perfil: usuario.perfil,
      tenantId: usuario.tenantId,
      consignatariaId: usuario.consignatariaId,
    },
  }
}

/**
 * Refreshes access token using refresh token
 */
export async function refreshToken(
  token: string,
  signToken: (payload: TokenPayload, options: { expiresIn: string }) => string
): Promise<{
  accessToken: string
  refreshToken: string
}> {
  // Find valid session
  const sessao = await prisma.sessao.findUnique({
    where: { refreshToken: token },
    include: {
      usuario: {
        select: {
          id: true,
          tenantId: true,
          consignatariaId: true,
          perfilId: true,
          ativo: true,
          bloqueado: true,
        },
      },
    },
  })

  if (!sessao) {
    throw new TRPCError({
      code: 'UNAUTHORIZED',
      message: 'Sessao invalida ou expirada',
    })
  }

  // Check if session is expired
  if (sessao.expiresAt < new Date()) {
    // Clean up expired session
    await prisma.sessao.delete({ where: { id: sessao.id } })
    throw new TRPCError({
      code: 'UNAUTHORIZED',
      message: 'Sessao expirada. Faca login novamente.',
    })
  }

  // Check if user is still valid
  if (!sessao.usuario.ativo) {
    await prisma.sessao.delete({ where: { id: sessao.id } })
    throw new TRPCError({
      code: 'FORBIDDEN',
      message: 'Usuario inativo',
    })
  }

  if (sessao.usuario.bloqueado) {
    throw new TRPCError({
      code: 'FORBIDDEN',
      message: 'Usuario bloqueado',
    })
  }

  const tokenPayload: TokenPayload = {
    sub: sessao.usuario.id,
    tenantId: sessao.usuario.tenantId,
    consignatariaId: sessao.usuario.consignatariaId,
    perfilId: sessao.usuario.perfilId,
  }

  const newAccessToken = signToken(tokenPayload, {
    expiresIn: authConfig.accessToken.expiresIn,
  })

  const newRefreshToken = signToken(tokenPayload, {
    expiresIn: authConfig.refreshToken.expiresIn,
  })

  // Calculate new refresh token expiry
  const refreshTokenExpiresAt = new Date()
  const daysMatch = authConfig.refreshToken.expiresIn.match(/(\d+)d/)
  const days = daysMatch ? parseInt(daysMatch[1], 10) : 7
  refreshTokenExpiresAt.setDate(refreshTokenExpiresAt.getDate() + days)

  // Rotate refresh token (delete old, create new)
  await prisma.$transaction([
    prisma.sessao.delete({ where: { id: sessao.id } }),
    prisma.sessao.create({
      data: {
        usuarioId: sessao.usuario.id,
        refreshToken: newRefreshToken,
        expiresAt: refreshTokenExpiresAt,
        ip: sessao.ip,
        userAgent: sessao.userAgent,
      },
    }),
  ])

  return {
    accessToken: newAccessToken,
    refreshToken: newRefreshToken,
  }
}

/**
 * Changes user password
 */
export async function alterarSenha(
  userId: number,
  input: AlterarSenhaInput,
  ctx?: Context
): Promise<void> {
  const usuario = await prisma.usuario.findUnique({
    where: { id: userId },
    include: {
      senhaHistorico: {
        orderBy: { createdAt: 'desc' },
        take: authConfig.password.historyCount,
      },
    },
  })

  if (!usuario) {
    throw new TRPCError({
      code: 'NOT_FOUND',
      message: 'Usuario nao encontrado',
    })
  }

  const senhaAtualValida = await bcrypt.compare(input.senhaAtual, usuario.senhaHash)

  if (!senhaAtualValida) {
    throw new TRPCError({
      code: 'BAD_REQUEST',
      message: 'Senha atual incorreta',
    })
  }

  // Check password history
  for (const senhaAnterior of usuario.senhaHistorico) {
    const senhaRepetida = await bcrypt.compare(input.novaSenha, senhaAnterior.senhaHash)
    if (senhaRepetida) {
      throw new TRPCError({
        code: 'BAD_REQUEST',
        message: `Nao e permitido reutilizar as ultimas ${authConfig.password.historyCount} senhas`,
      })
    }
  }

  // Also check current password
  const mesmaQueAtual = await bcrypt.compare(input.novaSenha, usuario.senhaHash)
  if (mesmaQueAtual) {
    throw new TRPCError({
      code: 'BAD_REQUEST',
      message: 'A nova senha deve ser diferente da senha atual',
    })
  }

  const novaSenhaHash = await bcrypt.hash(input.novaSenha, authConfig.password.saltRounds)

  await prisma.$transaction([
    prisma.usuario.update({
      where: { id: userId },
      data: {
        senhaHash: novaSenhaHash,
        primeiroAcesso: false,
      },
    }),
    prisma.senhaHistorico.create({
      data: {
        usuarioId: userId,
        senhaHash: usuario.senhaHash,
      },
    }),
  ])

  // Log audit action
  if (ctx) {
    await logAuditAction(ctx, {
      entidade: 'Usuario',
      entidadeId: userId,
      acao: AuditActions.ATUALIZAR,
      dadosNovos: {
        campo: 'senha',
        alteradoEm: new Date().toISOString(),
      },
    })
  }
}

/**
 * Logs out user by invalidating refresh token
 */
export async function logout(token: string, ctx?: Context): Promise<void> {
  const sessao = await prisma.sessao.findUnique({
    where: { refreshToken: token },
    select: { usuarioId: true },
  })

  await prisma.sessao.deleteMany({
    where: { refreshToken: token },
  })

  // Log audit action
  if (ctx && sessao) {
    await logAuditAction(ctx, {
      entidade: 'Usuario',
      entidadeId: sessao.usuarioId,
      acao: AuditActions.LOGOUT,
      dadosNovos: {
        timestamp: new Date().toISOString(),
      },
    })
  }
}

/**
 * Logs out user from all sessions
 */
export async function logoutAll(userId: number, ctx?: Context): Promise<number> {
  const result = await prisma.sessao.deleteMany({
    where: { usuarioId: userId },
  })

  // Log audit action
  if (ctx) {
    await logAuditAction(ctx, {
      entidade: 'Usuario',
      entidadeId: userId,
      acao: AuditActions.LOGOUT,
      dadosNovos: {
        tipo: 'logout_all',
        sessoesEncerradas: result.count,
        timestamp: new Date().toISOString(),
      },
    })
  }

  return result.count
}

/**
 * Gets user's active sessions
 */
export async function getActiveSessions(
  userId: number
): Promise<
  Array<{
    id: number
    ip: string | null
    userAgent: string | null
    createdAt: Date
    expiresAt: Date
  }>
> {
  return prisma.sessao.findMany({
    where: {
      usuarioId: userId,
      expiresAt: { gt: new Date() },
    },
    select: {
      id: true,
      ip: true,
      userAgent: true,
      createdAt: true,
      expiresAt: true,
    },
    orderBy: { createdAt: 'desc' },
  })
}

/**
 * Revokes a specific session
 */
export async function revokeSession(
  userId: number,
  sessionId: number,
  ctx?: Context
): Promise<void> {
  const sessao = await prisma.sessao.findFirst({
    where: {
      id: sessionId,
      usuarioId: userId,
    },
  })

  if (!sessao) {
    throw new TRPCError({
      code: 'NOT_FOUND',
      message: 'Sessao nao encontrada',
    })
  }

  await prisma.sessao.delete({
    where: { id: sessionId },
  })

  // Log audit action
  if (ctx) {
    await logAuditAction(ctx, {
      entidade: 'Sessao',
      entidadeId: sessionId,
      acao: AuditActions.EXCLUIR,
      dadosAnteriores: {
        usuarioId: userId,
        ip: sessao.ip,
      },
    })
  }
}

/**
 * Cleans up expired sessions (for scheduled job)
 */
export async function cleanupExpiredSessions(): Promise<number> {
  const result = await prisma.sessao.deleteMany({
    where: {
      expiresAt: { lt: new Date() },
    },
  })

  return result.count
}
