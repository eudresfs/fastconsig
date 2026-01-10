import bcrypt from 'bcrypt'
import { TRPCError } from '@trpc/server'
import { prisma } from '@fastconsig/database/client'
import { authConfig } from '@/config/auth'
import { type LoginInput, type AlterarSenhaInput } from './auth.schema'

interface TokenPayload {
  sub: number
  tenantId: number | null
  consignatariaId: number | null
  perfilId: number
}

export async function login(
  input: LoginInput,
  signToken: (payload: TokenPayload, options: { expiresIn: string }) => string
): Promise<{
  accessToken: string
  refreshToken: string
  usuario: {
    id: number
    nome: string
    email: string
    primeiroAcesso: boolean
  }
}> {
  const usuario = await prisma.usuario.findUnique({
    where: { login: input.login },
    include: {
      perfil: true,
    },
  })

  if (!usuario) {
    throw new TRPCError({
      code: 'UNAUTHORIZED',
      message: 'Credenciais invalidas',
    })
  }

  if (usuario.bloqueado) {
    if (usuario.bloqueadoAte && usuario.bloqueadoAte > new Date()) {
      throw new TRPCError({
        code: 'FORBIDDEN',
        message: 'Usuario bloqueado temporariamente. Tente novamente mais tarde.',
      })
    }
    // Desbloquear se o tempo expirou
    await prisma.usuario.update({
      where: { id: usuario.id },
      data: { bloqueado: false, bloqueadoAte: null, tentativasLogin: 0 },
    })
  }

  if (!usuario.ativo) {
    throw new TRPCError({
      code: 'FORBIDDEN',
      message: 'Usuario inativo',
    })
  }

  const senhaValida = await bcrypt.compare(input.senha, usuario.senhaHash)

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
        message: `Usuario bloqueado por ${authConfig.lockout.durationMinutes} minutos`,
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

  // Reset tentativas e atualizar ultimo login
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

  // Salvar sessao
  await prisma.sessao.create({
    data: {
      usuarioId: usuario.id,
      refreshToken,
      expiresAt: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000), // 7 dias
    },
  })

  return {
    accessToken,
    refreshToken,
    usuario: {
      id: usuario.id,
      nome: usuario.nome,
      email: usuario.email,
      primeiroAcesso: usuario.primeiroAcesso,
    },
  }
}

export async function alterarSenha(
  userId: number,
  input: AlterarSenhaInput
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

  // Verificar historico de senhas
  for (const senhaAnterior of usuario.senhaHistorico) {
    const senhaRepetida = await bcrypt.compare(input.novaSenha, senhaAnterior.senhaHash)
    if (senhaRepetida) {
      throw new TRPCError({
        code: 'BAD_REQUEST',
        message: `Nao e permitido reutilizar as ultimas ${authConfig.password.historyCount} senhas`,
      })
    }
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
}

export async function logout(refreshToken: string): Promise<void> {
  await prisma.sessao.deleteMany({
    where: { refreshToken },
  })
}
