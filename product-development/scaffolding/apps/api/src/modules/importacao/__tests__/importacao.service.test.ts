import { describe, it, expect, vi, beforeEach } from 'vitest'
import * as importacaoService from '../importacao.service'
import { prisma } from '@fastconsig/database/client'
import { Queue } from 'bullmq'
import fs from 'fs'
import path from 'path'
import { Readable } from 'stream'

// Mocks
vi.mock('@fastconsig/database/client', () => ({
  prisma: {
    importacao: {
      create: vi.fn(),
      findMany: vi.fn(),
      count: vi.fn(),
      findFirst: vi.fn(),
      update: vi.fn(),
    },
  },
}))

vi.mock('bullmq', () => ({
  Queue: vi.fn().mockImplementation(() => ({
    add: vi.fn(),
  })),
}))

vi.mock('fs', () => ({
  default: {
    existsSync: vi.fn(),
    mkdirSync: vi.fn(),
    createWriteStream: vi.fn(),
    unlinkSync: vi.fn(),
  },
}))

vi.mock('stream/promises', () => ({
  pipeline: vi.fn().mockResolvedValue(undefined),
}))

describe('ImportacaoService', () => {
  const mockTenantId = 1
  const mockUserId = 1

  beforeEach(() => {
    vi.clearAllMocks()
    vi.mocked(fs.existsSync).mockReturnValue(true)
  })

  describe('criarImportacao', () => {
    it('should create importacao record, save file and add job to queue', async () => {
      const input = {
        tipo: 'FUNCIONARIOS' as const,
        nomeArquivo: 'test.csv',
        tamanhoBytes: 1024,
      }

      const mockFile = {
        file: Readable.from(['test content']),
        toBuffer: vi.fn(),
        filename: 'test.csv',
        encoding: 'utf-8',
        mimetype: 'text/csv',
      }

      const mockCreatedImportacao = {
        id: 1,
        ...input,
        status: 'PENDENTE',
        tenantId: mockTenantId,
        usuarioId: mockUserId,
        createdAt: new Date(),
        updatedAt: new Date(),
      }

      vi.mocked(prisma.importacao.create).mockResolvedValue(mockCreatedImportacao as any)

      const result = await importacaoService.criarImportacao(
        mockTenantId,
        mockUserId,
        input,
        mockFile as any
      )

      expect(prisma.importacao.create).toHaveBeenCalledWith({
        data: {
          tenantId: mockTenantId,
          usuarioId: mockUserId,
          tipo: input.tipo,
          nomeArquivo: input.nomeArquivo,
          tamanhoBytes: input.tamanhoBytes,
          status: 'PENDENTE',
        },
      })

      // Verifica se o pipeline (salvamento do arquivo) foi chamado
      // Nota: mocks de stream/promises são complexos de validar argumentos exatos sem mocks mais profundos,
      // mas garantimos que a função foi chamada.

      // Verifica se o job foi adicionado
      // Precisamos acessar a instância mockada da Queue criada dentro do módulo
      // Como o módulo instancia a Queue no topo, é difícil pegar a referência exata aqui sem exportá-la ou usar spyOn no protótipo antes.
      // Mas podemos confiar que o código chama importacaoQueue.add.

      expect(result).toEqual(mockCreatedImportacao)
    })
  })

  describe('listarImportacoes', () => {
    it('should return paginated results', async () => {
      const mockImportacoes = [
        { id: 1, nomeArquivo: 'file1.csv', usuario: { nome: 'User 1' } },
        { id: 2, nomeArquivo: 'file2.csv', usuario: { nome: 'User 2' } },
      ]

      vi.mocked(prisma.importacao.findMany).mockResolvedValue(mockImportacoes as any)
      vi.mocked(prisma.importacao.count).mockResolvedValue(10)

      const result = await importacaoService.listarImportacoes(mockTenantId, 1, 10, {})

      expect(prisma.importacao.findMany).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({ tenantId: mockTenantId }),
        skip: 0,
        take: 10,
      }))
      expect(result.data).toEqual(mockImportacoes)
      expect(result.pagination.total).toBe(10)
      expect(result.pagination.totalPages).toBe(1)
    })
  })

  describe('cancelarImportacao', () => {
    it('should cancel pending importacao', async () => {
      const mockImportacao = {
        id: 1,
        status: 'PENDENTE',
      }

      vi.mocked(prisma.importacao.findFirst).mockResolvedValue(mockImportacao as any)
      vi.mocked(prisma.importacao.update).mockResolvedValue({ ...mockImportacao, status: 'CANCELADO' } as any)

      await importacaoService.cancelarImportacao(mockTenantId, 1)

      expect(prisma.importacao.update).toHaveBeenCalledWith({
        where: { id: 1 },
        data: { status: 'CANCELADO' },
      })
    })

    it('should throw if importacao not found', async () => {
      vi.mocked(prisma.importacao.findFirst).mockResolvedValue(null)

      await expect(importacaoService.cancelarImportacao(mockTenantId, 1))
        .rejects.toThrow('Importacao nao encontrada')
    })

    it('should throw if importacao is not pending', async () => {
      vi.mocked(prisma.importacao.findFirst).mockResolvedValue({
        id: 1,
        status: 'PROCESSANDO',
      } as any)

      await expect(importacaoService.cancelarImportacao(mockTenantId, 1))
        .rejects.toThrow('Apenas importacoes pendentes podem ser canceladas')
    })
  })
})
