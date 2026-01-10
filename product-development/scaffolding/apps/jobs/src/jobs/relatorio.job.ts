import { type Job } from 'bullmq'
import { prisma } from '@fastconsig/database/client'
import { logger } from '../config/logger'
import PDFDocument from 'pdfkit'
import ExcelJS from 'exceljs'
import fs from 'node:fs'
import path from 'node:path'

interface RelatorioJobData {
  tipo: 'AVERBACOES' | 'FUNCIONARIOS' | 'CONCILIACAO' | 'MARGEM'
  formato: 'PDF' | 'EXCEL'
  tenantId: number
  filtros: Record<string, unknown>
  outputPath: string
}

export async function processRelatorio(job: Job<RelatorioJobData>): Promise<void> {
  const { tipo, formato, tenantId, filtros, outputPath } = job.data

  logger.info({ tipo, formato, tenantId }, 'Generating report')

  try {
    const outputDir = path.dirname(outputPath)
    if (!fs.existsSync(outputDir)) {
      fs.mkdirSync(outputDir, { recursive: true })
    }

    switch (tipo) {
      case 'AVERBACOES':
        await gerarRelatorioAverbacoes(tenantId, filtros, formato, outputPath)
        break
      case 'FUNCIONARIOS':
        await gerarRelatorioFuncionarios(tenantId, filtros, formato, outputPath)
        break
      case 'CONCILIACAO':
        await gerarRelatorioConciliacao(tenantId, filtros, formato, outputPath)
        break
      case 'MARGEM':
        await gerarRelatorioMargem(tenantId, filtros, formato, outputPath)
        break
    }

    logger.info({ tipo, formato, outputPath }, 'Report generated successfully')
  } catch (error) {
    logger.error({ tipo, formato, error }, 'Failed to generate report')
    throw error
  }
}

async function gerarRelatorioAverbacoes(
  tenantId: number,
  filtros: Record<string, unknown>,
  formato: 'PDF' | 'EXCEL',
  outputPath: string
): Promise<void> {
  const averbacoes = await prisma.averbacao.findMany({
    where: {
      tenantId,
      ...(filtros.situacao && { situacao: filtros.situacao as string }),
      ...(filtros.dataInicio && { dataContrato: { gte: new Date(filtros.dataInicio as string) } }),
      ...(filtros.dataFim && { dataContrato: { lte: new Date(filtros.dataFim as string) } }),
    },
    include: {
      funcionario: { select: { nome: true, cpf: true, matricula: true } },
      tenantConsignataria: { include: { consignataria: { select: { razaoSocial: true } } } },
      produto: { select: { nome: true } },
    },
    orderBy: { dataContrato: 'desc' },
  })

  if (formato === 'EXCEL') {
    const workbook = new ExcelJS.Workbook()
    const worksheet = workbook.addWorksheet('Averbacoes')

    worksheet.columns = [
      { header: 'Contrato', key: 'contrato', width: 15 },
      { header: 'Funcionario', key: 'funcionario', width: 30 },
      { header: 'CPF', key: 'cpf', width: 15 },
      { header: 'Consignataria', key: 'consignataria', width: 25 },
      { header: 'Produto', key: 'produto', width: 20 },
      { header: 'Valor Total', key: 'valorTotal', width: 15 },
      { header: 'Parcela', key: 'parcela', width: 12 },
      { header: 'Prazo', key: 'prazo', width: 8 },
      { header: 'Situacao', key: 'situacao', width: 15 },
      { header: 'Data Contrato', key: 'dataContrato', width: 12 },
    ]

    averbacoes.forEach((av) => {
      worksheet.addRow({
        contrato: av.numeroContrato,
        funcionario: av.funcionario.nome,
        cpf: av.funcionario.cpf,
        consignataria: av.tenantConsignataria.consignataria.razaoSocial,
        produto: av.produto.nome,
        valorTotal: av.valorTotal.toNumber(),
        parcela: av.valorParcela.toNumber(),
        prazo: av.parcelasTotal,
        situacao: av.situacao,
        dataContrato: av.dataContrato,
      })
    })

    await workbook.xlsx.writeFile(outputPath)
  } else {
    const doc = new PDFDocument({ margin: 50 })
    const stream = fs.createWriteStream(outputPath)
    doc.pipe(stream)

    doc.fontSize(18).text('Relatorio de Averbacoes', { align: 'center' })
    doc.moveDown()

    doc.fontSize(10)
    averbacoes.forEach((av) => {
      doc.text(`Contrato: ${av.numeroContrato} | ${av.funcionario.nome} | R$ ${av.valorTotal.toNumber().toFixed(2)}`)
    })

    doc.end()
  }
}

async function gerarRelatorioFuncionarios(
  tenantId: number,
  _filtros: Record<string, unknown>,
  formato: 'PDF' | 'EXCEL',
  outputPath: string
): Promise<void> {
  const funcionarios = await prisma.funcionario.findMany({
    where: { tenantId },
    include: { empresa: { select: { nome: true } } },
    orderBy: { nome: 'asc' },
  })

  if (formato === 'EXCEL') {
    const workbook = new ExcelJS.Workbook()
    const worksheet = workbook.addWorksheet('Funcionarios')

    worksheet.columns = [
      { header: 'Matricula', key: 'matricula', width: 15 },
      { header: 'Nome', key: 'nome', width: 35 },
      { header: 'CPF', key: 'cpf', width: 15 },
      { header: 'Empresa', key: 'empresa', width: 25 },
      { header: 'Cargo', key: 'cargo', width: 25 },
      { header: 'Salario Bruto', key: 'salario', width: 15 },
      { header: 'Situacao', key: 'situacao', width: 12 },
    ]

    funcionarios.forEach((f) => {
      worksheet.addRow({
        matricula: f.matricula,
        nome: f.nome,
        cpf: f.cpf,
        empresa: f.empresa.nome,
        cargo: f.cargo,
        salario: f.salarioBruto.toNumber(),
        situacao: f.situacao,
      })
    })

    await workbook.xlsx.writeFile(outputPath)
  } else {
    const doc = new PDFDocument({ margin: 50 })
    const stream = fs.createWriteStream(outputPath)
    doc.pipe(stream)

    doc.fontSize(18).text('Relatorio de Funcionarios', { align: 'center' })
    doc.moveDown()

    doc.fontSize(10)
    funcionarios.forEach((f) => {
      doc.text(`${f.matricula} | ${f.nome} | ${f.cpf} | R$ ${f.salarioBruto.toNumber().toFixed(2)}`)
    })

    doc.end()
  }
}

async function gerarRelatorioConciliacao(
  tenantId: number,
  filtros: Record<string, unknown>,
  formato: 'PDF' | 'EXCEL',
  outputPath: string
): Promise<void> {
  const conciliacaoId = filtros.conciliacaoId as number
  const conciliacao = await prisma.conciliacao.findFirst({
    where: { id: conciliacaoId, tenantId },
    include: {
      itens: {
        include: {
          averbacao: {
            include: {
              funcionario: { select: { nome: true, cpf: true } },
              tenantConsignataria: { include: { consignataria: { select: { razaoSocial: true } } } },
            },
          },
        },
      },
    },
  })

  if (!conciliacao) {
    throw new Error('Conciliacao nao encontrada')
  }

  if (formato === 'EXCEL') {
    const workbook = new ExcelJS.Workbook()
    const worksheet = workbook.addWorksheet('Conciliacao')

    worksheet.columns = [
      { header: 'Contrato', key: 'contrato', width: 15 },
      { header: 'Funcionario', key: 'funcionario', width: 30 },
      { header: 'Consignataria', key: 'consignataria', width: 25 },
      { header: 'Valor Enviado', key: 'enviado', width: 15 },
      { header: 'Valor Descontado', key: 'descontado', width: 15 },
      { header: 'Status', key: 'status', width: 15 },
    ]

    conciliacao.itens.forEach((item) => {
      worksheet.addRow({
        contrato: item.averbacao.numeroContrato,
        funcionario: item.averbacao.funcionario.nome,
        consignataria: item.averbacao.tenantConsignataria.consignataria.razaoSocial,
        enviado: item.valorEnviado.toNumber(),
        descontado: item.valorDescontado.toNumber(),
        status: item.status,
      })
    })

    await workbook.xlsx.writeFile(outputPath)
  } else {
    const doc = new PDFDocument({ margin: 50 })
    const stream = fs.createWriteStream(outputPath)
    doc.pipe(stream)

    doc.fontSize(18).text(`Relatorio de Conciliacao - ${conciliacao.competencia}`, { align: 'center' })
    doc.moveDown()

    doc.end()
  }
}

async function gerarRelatorioMargem(
  tenantId: number,
  _filtros: Record<string, unknown>,
  formato: 'PDF' | 'EXCEL',
  outputPath: string
): Promise<void> {
  const funcionarios = await prisma.funcionario.findMany({
    where: { tenantId, situacao: 'ATIVO' },
    include: {
      averbacoes: {
        where: { situacao: { in: ['APROVADA', 'ENVIADA', 'DESCONTADA'] } },
      },
    },
    orderBy: { nome: 'asc' },
  })

  if (formato === 'EXCEL') {
    const workbook = new ExcelJS.Workbook()
    const worksheet = workbook.addWorksheet('Margem')

    worksheet.columns = [
      { header: 'Matricula', key: 'matricula', width: 15 },
      { header: 'Nome', key: 'nome', width: 35 },
      { header: 'Salario Bruto', key: 'salario', width: 15 },
      { header: 'Margem Total', key: 'margemTotal', width: 15 },
      { header: 'Margem Utilizada', key: 'margemUtilizada', width: 15 },
      { header: 'Margem Disponivel', key: 'margemDisponivel', width: 15 },
    ]

    funcionarios.forEach((f) => {
      const margemTotal = f.salarioBruto.toNumber() * 0.35
      const margemUtilizada = f.averbacoes.reduce((sum, av) => sum + av.valorParcela.toNumber(), 0)
      const margemDisponivel = Math.max(0, margemTotal - margemUtilizada)

      worksheet.addRow({
        matricula: f.matricula,
        nome: f.nome,
        salario: f.salarioBruto.toNumber(),
        margemTotal,
        margemUtilizada,
        margemDisponivel,
      })
    })

    await workbook.xlsx.writeFile(outputPath)
  } else {
    const doc = new PDFDocument({ margin: 50 })
    const stream = fs.createWriteStream(outputPath)
    doc.pipe(stream)

    doc.fontSize(18).text('Relatorio de Margem', { align: 'center' })
    doc.moveDown()

    doc.end()
  }
}
