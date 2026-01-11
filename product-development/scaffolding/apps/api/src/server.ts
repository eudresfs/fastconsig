import Fastify from 'fastify'
import cors from '@fastify/cors'
import helmet from '@fastify/helmet'
import rateLimit from '@fastify/rate-limit'
import cookie from '@fastify/cookie'
import jwt from '@fastify/jwt'
import multipart from '@fastify/multipart'
import { fastifyTRPCPlugin } from '@trpc/server/adapters/fastify'
import { appRouter } from './trpc/router'
import { createContext } from './trpc/context'
import { env } from './config/env'

const server = Fastify({
  logger: true,
})

async function main(): Promise<void> {
  // Security plugins
  await server.register(helmet)
  await server.register(cors, {
    origin: env.CORS_ORIGIN,
    credentials: true,
  })
  await server.register(rateLimit, {
    max: 100,
    timeWindow: '1 minute',
  })

  // Auth plugins
  await server.register(cookie)
  await server.register(jwt, {
    secret: env.JWT_ACCESS_SECRET,
  })

  // File upload
  await server.register(multipart, {
    limits: {
      fileSize: 10 * 1024 * 1024, // 10MB
    },
  })

  // tRPC
  await server.register(fastifyTRPCPlugin, {
    prefix: '/trpc',
    trpcOptions: {
      router: appRouter,
      createContext,
    },
  })

  // Health check
  server.get('/health', async () => {
    return { status: 'ok', timestamp: new Date().toISOString() }
  })

  // Start server
  try {
    await server.listen({ port: env.PORT, host: '0.0.0.0' })
    server.log.info(`Server running on http://localhost:${env.PORT}`)
  } catch (err) {
    server.log.error(err)
    process.exit(1)
  }
}

main()
