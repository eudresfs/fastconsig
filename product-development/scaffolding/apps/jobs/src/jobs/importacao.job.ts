import { type Job } from 'bullmq'
import { prisma } from '@fastconsig/database/client'
import { logger } from '../config/logger'
import ExcelJS from 'exceljs'

interface ImportacaoJobData {
  importacaoId: number
  tenantId: number
  filePath: string
}

export async function processImportacao(job: Job<ImportacaoJobData>): Promise<void> {
  const { importacaoId, tenantId, filePath } = job.data

  logger.info({ importacaoId }, 'Starting importacao processing')

  try {
    // Atualizar status para processando
    await prisma.importacao.update({
      where: { id: importacaoId },
      data: { status: 'PROCESSANDO', iniciadoEm: new Date() },
    })

    const importacao = await prisma.importacao.findUnique({
      where: { id: importacaoId },
    })

    if (!importacao) {
      throw new Error('Importacao nao encontrada')
    }

    let linhasProcessadas = 0
    let linhasSucesso = 0
    let linhasErro = 0
    const erros: Array<{ linha: number; erro: string }> = []

    // Processar arquivo baseado no tipo
    switch (importacao.tipo) {
      case 'FUNCIONARIOS':
        const resultFuncionarios = await processarFuncionarios(filePath, tenantId)
        linhasProcessadas = resultFuncionarios.total
        linhasSucesso = resultFuncionarios.sucesso
        linhasErro = resultFuncionarios.erro
        erros.push(...resultFuncionarios.erros)
        break

      case 'CONTRATOS':
        const resultContratos = await processarContratos(filePath, tenantId)
        linhasProcessadas = resultContratos.total
        linhasSucesso = resultContratos.sucesso
        linhasErro = resultContratos.erro
        erros.push(...resultContratos.erros)
        break

      case 'RETORNO_FOLHA':
        const resultRetorno = await processarRetornoFolha(filePath, tenantId)
        linhasProcessadas = resultRetorno.total
        linhasSucesso = resultRetorno.sucesso
        linhasErro = resultRetorno.erro
        erros.push(...resultRetorno.erros)
        break
    }

    // Atualizar status para concluido
    await prisma.importacao.update({
      where: { id: importacaoId },
      data: {
        status: linhasErro > 0 ? 'ERRO' : 'CONCLUIDO',
        finalizadoEm: new Date(),
        linhasProcessadas,
        linhasSucesso,
        linhasErro,
        erros: erros.length > 0 ? erros : undefined,
      },
    })

    logger.info({ importacaoId, linhasProcessadas, linhasSucesso, linhasErro }, 'Importacao completed')
  } catch (error) {
    logger.error({ importacaoId, error }, 'Importacao failed')

    await prisma.importacao.update({
      where: { id: importacaoId },
      data: {
        status: 'ERRO',
        finalizadoEm: new Date(),
        erros: [{ linha: 0, erro: (error as Error).message }],
      },
    })

    throw error
  }
}

async function processarFuncionarios(
  filePath: string,
  tenantId: number
): Promise<{ total: number; sucesso: number; erro: number; erros: Array<{ linha: number; erro: string }> }> {
  const workbook = new ExcelJS.Workbook()
  await workbook.xlsx.readFile(filePath)

  const worksheet = workbook.getWorksheet(1)
  if (!worksheet) {
    throw new Error('Planilha nao encontrada')
  }

  let total = 0
  let sucesso = 0
  let erro = 0
  const erros: Array<{ linha: number; erro: string }> = []

  worksheet.eachRow({ includeEmpty: false }, async (row, rowNumber) => {
    if (rowNumber === 1) return // Skip header

    total++
    try {
      const cpf = String(row.getCell(1).value).replace(/\D/g, '')
      const nome = String(row.getCell(2).value)
      const matricula = String(row.getCell(3).value)
      const salarioBruto = Number(row.getCell(4).value)

      await prisma.funcionario.upsert({
        where: { tenantId_cpf: { tenantId, cpf } },
        update: { nome, salarioBruto },
        create: {
          tenantId,
          empresaId: 1, // TODO: Mapear empresa
          cpf,
          nome,
          matricula,
          salarioBruto,
          dataNascimento: new Date(),
          dataAdmissao: new Date(),
        },
      })

      sucesso++
    } catch (e) {
      erro++
      erros.push({ linha: rowNumber, erro: (e as Error).message })
    }
  })

  return { total, sucesso, erro, erros }
}

async function processarContratos(
  _filePath: string,
  _tenantId: number
): Promise<{ total: number; sucesso: number; erro: number; erros: Array<{ linha: number; erro: string }> }> {
  // TODO: Implementar processamento de contratos
  return { total: 0, sucesso: 0, erro: 0, erros: [] }
}

async function processarRetornoFolha(
  _filePath: string,
  _tenantId: number
): Promise<{ total: number; sucesso: number; erro: number; erros: Array<{ linha: number; erro: string }> }> {
  // TODO: Implementar processamento de retorno de folha
  return { total: 0, sucesso: 0, erro: 0, erros: [] }
}
