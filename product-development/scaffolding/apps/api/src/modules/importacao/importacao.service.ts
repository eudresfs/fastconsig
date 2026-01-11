import { prisma } from '@fastconsig/database/client'
import { Queue } from 'bullmq'
import { env } from '@/config/env'
import fs from 'fs'
import path from 'path'
import { pipeline } from 'stream/promises'
import { randomUUID } from 'crypto'
import type { CriarImportacaoInput } from './importacao.schema'
import type { MultipartFile } from '@fastify/multipart'

// Setup queue
const importacaoQueue = new Queue('importacao', {
  connection: {
    url: env.REDIS_URL
  }
})

const UPLOAD_DIR = path.join(process.cwd(), 'uploads')

// Ensure upload directory exists
if (!fs.existsSync(UPLOAD_DIR)) {
  fs.mkdirSync(UPLOAD_DIR, { recursive: true })
}

export async function criarImportacao(
  tenantId: number,
  usuarioId: number,
  input: CriarImportacaoInput,
  file: MultipartFile
) {
  // Generate unique filename
  const fileExt = path.extname(input.nomeArquivo)
  const storedFileName = `${randomUUID()}${fileExt}`
  const storedFilePath = path.join(UPLOAD_DIR, storedFileName)

  // Save file to disk
  await pipeline(file.file, fs.createWriteStream(storedFilePath))

  // Create DB record
  const importacao = await prisma.importacao.create({
    data: {
      tenantId,
      usuarioId,
      tipo: input.tipo,
      nomeArquivo: input.nomeArquivo,
      tamanhoBytes: input.tamanhoBytes,
      status: 'PENDENTE',
    }
  })

  // Dispatch job
  await importacaoQueue.add('processar-arquivo', {
    importacaoId: importacao.id,
    filePath: storedFilePath,
    tenantId,
    tipo: input.tipo
  })

  return importacao
}

export async function listarImportacoes(
  tenantId: number,
  page: number,
  pageSize: number,
  filters: {
    tipo?: 'FUNCIONARIOS' | 'CONTRATOS' | 'RETORNO_FOLHA'
    status?: 'PENDENTE' | 'PROCESSANDO' | 'CONCLUIDO' | 'ERRO' | 'CANCELADO'
  }
) {
  const skip = (page - 1) * pageSize
  const where = {
    tenantId,
    ...filters
  }

  const [items, total] = await Promise.all([
    prisma.importacao.findMany({
      where,
      include: { usuario: { select: { nome: true } } },
      skip,
      take: pageSize,
      orderBy: { createdAt: 'desc' },
    }),
    prisma.importacao.count({ where }),
  ])

  return {
    data: items,
    pagination: {
      page,
      pageSize,
      total,
      totalPages: Math.ceil(total / pageSize)
    }
  }
}

export async function obterImportacao(tenantId: number, id: number) {
  const importacao = await prisma.importacao.findFirst({
    where: { id, tenantId },
    include: { usuario: { select: { nome: true } } },
  })

  if (!importacao) {
    throw new Error('Importacao nao encontrada')
  }

  return importacao
}

export async function cancelarImportacao(tenantId: number, id: number) {
  const importacao = await prisma.importacao.findFirst({
    where: { id, tenantId },
  })

  if (!importacao) {
    throw new Error('Importacao nao encontrada')
  }

  if (importacao.status !== 'PENDENTE') {
    throw new Error('Apenas importacoes pendentes podem ser canceladas')
  }

  return prisma.importacao.update({
    where: { id },
    data: { status: 'CANCELADO' },
  })
}
