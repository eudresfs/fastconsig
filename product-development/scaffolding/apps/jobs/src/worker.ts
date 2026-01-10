import { Worker, Queue } from 'bullmq'
import { Redis } from 'ioredis'
import { logger } from './config/logger'
import { env } from './config/env'
import { processImportacao } from './jobs/importacao.job'
import { processEmail } from './jobs/email.job'
import { processRelatorio } from './jobs/relatorio.job'

const connection = new Redis(env.REDIS_URL, {
  maxRetriesPerRequest: null,
})

// Queues
export const importacaoQueue = new Queue('importacao', { connection })
export const emailQueue = new Queue('email', { connection })
export const relatorioQueue = new Queue('relatorio', { connection })

// Workers
const importacaoWorker = new Worker('importacao', processImportacao, {
  connection,
  concurrency: 2,
})

const emailWorker = new Worker('email', processEmail, {
  connection,
  concurrency: 5,
})

const relatorioWorker = new Worker('relatorio', processRelatorio, {
  connection,
  concurrency: 1,
})

// Error handlers
importacaoWorker.on('failed', (job, err) => {
  logger.error({ jobId: job?.id, error: err.message }, 'Importacao job failed')
})

emailWorker.on('failed', (job, err) => {
  logger.error({ jobId: job?.id, error: err.message }, 'Email job failed')
})

relatorioWorker.on('failed', (job, err) => {
  logger.error({ jobId: job?.id, error: err.message }, 'Relatorio job failed')
})

// Graceful shutdown
async function shutdown(): Promise<void> {
  logger.info('Shutting down workers...')
  await Promise.all([
    importacaoWorker.close(),
    emailWorker.close(),
    relatorioWorker.close(),
  ])
  await connection.quit()
  process.exit(0)
}

process.on('SIGTERM', shutdown)
process.on('SIGINT', shutdown)

logger.info('Workers started successfully')
