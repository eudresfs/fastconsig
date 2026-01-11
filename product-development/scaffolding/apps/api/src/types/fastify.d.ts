import '@fastify/jwt'

declare module 'fastify' {
  interface FastifyRequest {
    jwtVerify<Decoded extends { sub: number; tenantId: number } = { sub: number; tenantId: number }>(): Promise<Decoded>
  }
}
