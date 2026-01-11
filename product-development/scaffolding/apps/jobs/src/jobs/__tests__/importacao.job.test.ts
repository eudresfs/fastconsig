import { describe, it, expect, vi, beforeEach } from 'vitest'
import { processImportacao } from '../importacao.job'
import { prisma } from '@fastconsig/database/client'
import ExcelJS from 'exceljs'

// Mocks
vi.mock('@fastconsig/database/client', () => ({
  prisma: {
    importacao: {
      update: vi.fn(),
      findUnique: vi.fn(),
    },
    empresa: {
      findMany: vi.fn(),
    },
    funcionario: {
      upsert: vi.fn(),
    },
  },
}))

vi.mock('../config/logger', () => ({
  logger: {
    info: vi.fn(),
    error: vi.fn(),
  },
}))

// Mock ExcelJS
const mockWorksheet = {
  eachRow: vi.fn(),
  getCell: vi.fn(),
}

const mockWorkbook = {
  xlsx: {
    readFile: vi.fn().mockResolvedValue(undefined),
  },
  getWorksheet: vi.fn().mockReturnValue(mockWorksheet),
}

vi.mock('exceljs', () => {
  return {
    default: {
      Workbook: vi.fn().mockImplementation(() => mockWorkbook),
    },
  }
})

describe('Importacao Job', () => {
  const mockJob = {
    data: {
      importacaoId: 1,
      tenantId: 1,
      filePath: 'test.xlsx',
    },
  } as any

  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('should process successful import', async () => {
    // Setup mocks
    vi.mocked(prisma.importacao.findUnique).mockResolvedValue({
      id: 1,
      tipo: 'FUNCIONARIOS',
    } as any)

    vi.mocked(prisma.empresa.findMany).mockResolvedValue([
      { id: 1, codigo: '001', nome: 'Empresa Teste' },
    ] as any)

    // Mock Excel rows
    const mockRows = [
      // Row 1 is header (skipped)
      // Row 2 is data
      {
        rowNumber: 2,
        getCell: vi.fn((col) => {
          const cells = {
            1: { value: '123.456.789-00' }, // CPF
            2: { text: 'Funcionario Teste' }, // Nome
            3: { text: '12345' }, // Matricula
            4: { text: 'Analista' }, // Cargo
            5: { value: 5000 }, // Salario
            6: { value: new Date('2023-01-01') }, // Admissao
            7: { value: new Date('1990-01-01') }, // Nascimento
            8: { text: '001' }, // Codigo Empresa
          }
          return cells[col as keyof typeof cells] || { value: null }
        }),
      },
    ]

    mockWorksheet.eachRow.mockImplementation((opts, callback) => {
      // Simulate iteration
      mockRows.forEach((row) => callback(row, row.rowNumber))
    })

    await processImportacao(mockJob)

    // Verify processing started
    expect(prisma.importacao.update).toHaveBeenCalledWith(
      expect.objectContaining({
        where: { id: 1 },
        data: expect.objectContaining({ status: 'PROCESSANDO' }),
      })
    )

    // Verify upsert called
    expect(prisma.funcionario.upsert).toHaveBeenCalled()

    // Verify completion
    expect(prisma.importacao.update).toHaveBeenCalledWith(
      expect.objectContaining({
        where: { id: 1 },
        data: expect.objectContaining({
          status: 'CONCLUIDO',
          linhasProcessadas: 1,
          linhasSucesso: 1,
          linhasErro: 0,
        }),
      })
    )
  })

  it('should handle errors in rows', async () => {
    // Setup mocks
    vi.mocked(prisma.importacao.findUnique).mockResolvedValue({
      id: 1,
      tipo: 'FUNCIONARIOS',
    } as any)

    vi.mocked(prisma.empresa.findMany).mockResolvedValue([
      { id: 1, codigo: '001', nome: 'Empresa Teste' },
    ] as any)

    // Mock Excel rows with error (invalid CPF)
    const mockRows = [
      {
        rowNumber: 2,
        getCell: vi.fn((col) => {
          const cells = {
            1: { value: '123' }, // CPF invalido
            2: { text: 'Funcionario Teste' },
            3: { text: '12345' },
            4: { text: 'Analista' },
            5: { value: 5000 },
            6: { value: new Date('2023-01-01') },
            7: { value: new Date('1990-01-01') },
            8: { text: '001' },
          }
          return cells[col as keyof typeof cells] || { value: null }
        }),
      },
    ]

    mockWorksheet.eachRow.mockImplementation((opts, callback) => {
      mockRows.forEach((row) => callback(row, row.rowNumber))
    })

    await processImportacao(mockJob)

    // Verify completion with error
    expect(prisma.importacao.update).toHaveBeenCalledWith(
      expect.objectContaining({
        where: { id: 1 },
        data: expect.objectContaining({
          status: 'ERRO',
          linhasProcessadas: 1,
          linhasSucesso: 0,
          linhasErro: 1,
          erros: expect.arrayContaining([
            expect.objectContaining({
              linha: 2,
              erro: expect.stringContaining('CPF invalido'),
            }),
          ]),
        }),
      })
    )
  })
})
