import { type FastifyInstance } from 'fastify'
import { z } from 'zod'
import { criarImportacao } from './importacao.service'
import { criarImportacaoSchema } from './importacao.schema'

export async function importacaoUploadRoutes(app: FastifyInstance) {
  app.post('/upload/importacao', async (req, reply) => {
    // Verify authentication
    try {
      await req.jwtVerify()
    } catch (err) {
      return reply.status(401).send({ message: 'Unauthorized' })
    }

    const data = await req.file()

    if (!data) {
      return reply.status(400).send({ message: 'File is required' })
    }

    // Parse fields
    // fastify-multipart puts fields on the parts object or we can parse them from fields
    // But req.file() returns the first file.
    // If we want fields + file, we might need req.parts() or similar,
    // BUT fastify-multipart attaches fields to data.fields if configured, or we iterate.

    // Easier way with fastify-multipart for mixed fields/files:
    // const parts = req.parts()
    // However, since I used `await req.file()`, it gets the file.
    // The fields might be available if they came BEFORE the file in the stream,
    // or we use `attachFieldsToBody: true` configuration, but that buffers files.

    // Let's check how `app.ts` configured multipart.
    // limits: { fileSize: 10 * 1024 * 1024 }
    // It didn't set attachFieldsToBody.

    // So we should iterate parts or use a helper.
    // Actually, `data.fields` property exists on the MultipartFile object if they were parsed?
    // No, usually `req.file()` gives the first file.

    // Let's use `req.parts()` to handle both fields and file.

    const parts = req.parts()
    let filePart
    const fields: Record<string, any> = {}

    for await (const part of parts) {
      if (part.type === 'file') {
        filePart = part
      } else {
        fields[part.fieldname] = part.value
      }
    }

    if (!filePart) {
      return reply.status(400).send({ message: 'File is required' })
    }

    // Validate fields
    const payload = {
      tipo: fields.tipo,
      nomeArquivo: fields.nomeArquivo,
      tamanhoBytes: Number(fields.tamanhoBytes)
    }

    const parseResult = criarImportacaoSchema.safeParse(payload)

    if (!parseResult.success) {
      return reply.status(400).send({ message: 'Invalid data', errors: parseResult.error.format() })
    }

    // Get user context
    // @ts-ignore
    const { sub: userId, tenantId } = req.user as { sub: number, tenantId: number }

    try {
      const result = await criarImportacao(tenantId, userId, parseResult.data, filePart)
      return reply.status(201).send(result)
    } catch (error) {
      req.log.error(error)
      return reply.status(500).send({ message: 'Internal server error' })
    }
  })
}
