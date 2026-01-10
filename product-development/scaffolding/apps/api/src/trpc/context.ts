import { type FastifyRequest, type FastifyReply } from 'fastify'
import { prisma } from '@fastconsig/database/client'

export interface Context {
  req: FastifyRequest
  res: FastifyReply
  prisma: typeof prisma
  tenantId: number | null
  userId: number | null
}

export async function createContext({
  req,
  res,
}: {
  req: FastifyRequest
  res: FastifyReply
}): Promise<Context> {
  let tenantId: number | null = null
  let userId: number | null = null

  try {
    const token = req.headers.authorization?.replace('Bearer ', '')
    if (token) {
      const decoded = await req.jwtVerify<{ sub: number; tenantId: number }>()
      userId = decoded.sub
      tenantId = decoded.tenantId
    }
  } catch {
    // Token invalido ou expirado - usuario nao autenticado
  }

  return {
    req,
    res,
    prisma,
    tenantId,
    userId,
  }
}
