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

  // Carregar empresas do tenant para mapeamento
  const empresas = await prisma.empresa.findMany({
    where: { tenantId }
  })

  const empresaMap = new Map<string, number>()
  empresas.forEach(e => empresaMap.set(e.codigo, e.id))

  // Se houver apenas uma empresa, usa como fallback
  const empresaDefaultId = empresas.length === 1 ? empresas[0].id : null

  let total = 0
  let sucesso = 0
  let erro = 0
  const erros: Array<{ linha: number; erro: string }> = []

  // Validar cabeçalhos (opcional, mas recomendado)
  // Assumindo: CPF, Nome, Matricula, Cargo, Salario, Admissao, Nascimento, Codigo Empresa

  // Processar em chunks para evitar sobrecarga de memória se for muito grande,
  // mas como estamos lendo tudo com ExcelJS, já estamos carregando em memória.
  // Para upserts, vamos fazer um por um para coletar erros individuais corretamente.
  // Otimização futura: prisma.$transaction para lotes se performance for crítica.

  // Iterar sobre as linhas
  // ExcelJS worksheet.eachRow começa do 1
  const rows: any[] = []
  worksheet.eachRow({ includeEmpty: false }, (row, rowNumber) => {
    if (rowNumber === 1) return // Skip header
    rows.push({ row, rowNumber })
  })

  for (const { row, rowNumber } of rows) {
    total++
    try {
      // Extrair dados
      // Col 1: CPF
      const cpfRaw = row.getCell(1).value
      if (!cpfRaw) throw new Error('CPF obrigatorio')
      const cpf = String(cpfRaw).replace(/\D/g, '')
      if (cpf.length !== 11) throw new Error('CPF invalido (deve ter 11 digitos)')

      // Col 2: Nome
      const nome = row.getCell(2).text
      if (!nome) throw new Error('Nome obrigatorio')

      // Col 3: Matricula
      const matricula = String(row.getCell(3).text || row.getCell(3).value)
      if (!matricula) throw new Error('Matricula obrigatoria')

      // Col 4: Cargo
      const cargo = row.getCell(4).text || null

      // Col 5: Salario Bruto
      const salarioVal = row.getCell(5).value
      const salarioBruto = Number(salarioVal)
      if (isNaN(salarioBruto) || salarioBruto < 0) throw new Error('Salario invalido')

      // Col 6: Data Admissao
      let dataAdmissao = row.getCell(6).value
      if (typeof dataAdmissao === 'string') dataAdmissao = new Date(dataAdmissao)
      if (!(dataAdmissao instanceof Date) || isNaN(dataAdmissao.getTime())) throw new Error('Data de admissao invalida')

      // Col 7: Data Nascimento
      let dataNascimento = row.getCell(7).value
      if (typeof dataNascimento === 'string') dataNascimento = new Date(dataNascimento)
      if (!(dataNascimento instanceof Date) || isNaN(dataNascimento.getTime())) throw new Error('Data de nascimento invalida')

      // Col 8: Codigo Empresa (Opcional se tiver default)
      const codigoEmpresa = row.getCell(8).text
      let empresaId = empresaDefaultId

      if (codigoEmpresa) {
        empresaId = empresaMap.get(codigoEmpresa) || null
      }

      if (!empresaId) {
        if (codigoEmpresa) throw new Error(`Empresa nao encontrada com codigo: ${codigoEmpresa}`)
        throw new Error('Empresa nao identificada e nao ha empresa unica no tenant')
      }

      // Upsert Funcionario
      await prisma.funcionario.upsert({
        where: {
          tenantId_cpf: { tenantId, cpf }
        },
        update: {
          nome,
          cargo,
          salarioBruto,
          empresaId,
          dataAdmissao,
          dataNascimento,
          // Se mudou de empresa ou matricula, deveria atualizar?
          // O CPF é a chave principal logica aqui.
          matricula // Atualiza matricula se mudou
        },
        create: {
          tenantId,
          empresaId,
          cpf,
          nome,
          matricula,
          cargo,
          salarioBruto,
          dataNascimento,
          dataAdmissao,
          situacao: 'ATIVO' // Default
        },
      })

      sucesso++
    } catch (e) {
      erro++
      erros.push({ linha: rowNumber, erro: (e as Error).message })
    }
  }

  return { total, sucesso, erro, erros }
}

async function processarContratos(
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

  // Pre-carregar dados auxiliares para evitar queries em loop
  // Em cenario real com muitos dados, usar cache ou batch processing
  const consignatarias = await prisma.tenantConsignataria.findMany({
    where: { tenantId },
    include: { consignataria: true }
  })

  // Mapa de codigo -> id
  const consignatariaMap = new Map<string, number>()
  consignatarias.forEach(tc => consignatariaMap.set(tc.codigo, tc.id))

  // Carregar produtos
  const produtos = await prisma.produto.findMany({
    where: {
      consignatariaId: { in: consignatarias.map(tc => tc.consignatariaId) }
    }
  })
  const produtoMap = new Map<string, number>() // codigo -> id
  produtos.forEach(p => produtoMap.set(p.codigo, p.id))

  const rows: any[] = []
  worksheet.eachRow({ includeEmpty: false }, (row, rowNumber) => {
    if (rowNumber === 1) return // Skip header
    rows.push({ row, rowNumber })
  })

  for (const { row, rowNumber } of rows) {
    total++
    try {
      // Mapeamento de Colunas (Assumindo layout padrao)
      // 1: CPF Funcionario
      // 2: Codigo Consignataria
      // 3: Codigo Produto
      // 4: Numero Contrato
      // 5: Data Contrato
      // 6: Valor Total
      // 7: Valor Parcela
      // 8: Qtd Parcelas
      // 9: Taxa Mensal
      // 10: Data Inicio Desconto
      // 11: Data Fim Desconto

      // Validar Funcionario
      const cpfRaw = row.getCell(1).value
      if (!cpfRaw) throw new Error('CPF obrigatorio')
      const cpf = String(cpfRaw).replace(/\D/g, '')

      const funcionario = await prisma.funcionario.findUnique({
        where: { tenantId_cpf: { tenantId, cpf } }
      })

      if (!funcionario) {
        throw new Error(`Funcionario nao encontrado: ${cpf}`)
      }

      // Validar Consignataria
      const codConsignataria = String(row.getCell(2).text || row.getCell(2).value)
      const tenantConsignatariaId = consignatariaMap.get(codConsignataria)
      if (!tenantConsignatariaId) {
        throw new Error(`Consignataria nao encontrada: ${codConsignataria}`)
      }

      // Validar Produto
      const codProduto = String(row.getCell(3).text || row.getCell(3).value)
      const produtoId = produtoMap.get(codProduto)
      if (!produtoId) {
        throw new Error(`Produto nao encontrado: ${codProduto}`)
      }

      const numeroContrato = String(row.getCell(4).text || row.getCell(4).value)
      if (!numeroContrato) throw new Error('Numero do contrato obrigatorio')

      // Datas
      let dataContrato = row.getCell(5).value
      if (typeof dataContrato === 'string') dataContrato = new Date(dataContrato)
      if (!(dataContrato instanceof Date) || isNaN(dataContrato.getTime())) throw new Error('Data contrato invalida')

      let dataInicio = row.getCell(10).value
      if (typeof dataInicio === 'string') dataInicio = new Date(dataInicio)
      if (!(dataInicio instanceof Date) || isNaN(dataInicio.getTime())) throw new Error('Data inicio desconto invalida')

      let dataFim = row.getCell(11).value
      if (typeof dataFim === 'string') dataFim = new Date(dataFim)
      if (!(dataFim instanceof Date) || isNaN(dataFim.getTime())) throw new Error('Data fim desconto invalida')

      // Valores
      const valorTotal = Number(row.getCell(6).value)
      const valorParcela = Number(row.getCell(7).value)
      const parcelasTotal = Number(row.getCell(8).value)
      const taxaMensal = Number(row.getCell(9).value)

      if (isNaN(valorTotal) || isNaN(valorParcela) || isNaN(parcelasTotal)) {
        throw new Error('Valores numericos invalidos')
      }

      // Upsert Averbacao
      await prisma.averbacao.upsert({
        where: {
          tenantId_numeroContrato: {
            tenantId,
            numeroContrato
          }
        },
        update: {
          funcionarioId: funcionario.id,
          tenantConsignatariaId,
          produtoId,
          valorTotal,
          valorParcela,
          parcelasTotal,
          taxaMensal,
          dataContrato,
          dataInicioDesconto: dataInicio,
          dataFimDesconto: dataFim,
          situacao: 'ATIVO' as any // Ajustar conforme Enum ou regra de negocio
        },
        create: {
          tenantId,
          funcionarioId: funcionario.id,
          tenantConsignatariaId,
          produtoId,
          numeroContrato,
          valorTotal,
          valorParcela,
          parcelasTotal,
          taxaMensal,
          dataContrato,
          dataInicioDesconto: dataInicio,
          dataFimDesconto: dataFim,
          situacao: 'ATIVO' as any, // Ajustar conforme Enum
          tipoOperacao: 'NOVO'
        }
      })

      sucesso++
    } catch (e) {
      erro++
      erros.push({ linha: rowNumber, erro: (e as Error).message })
    }
  }

  return { total, sucesso, erro, erros }
}

async function processarRetornoFolha(
  _filePath: string,
  _tenantId: number
): Promise<{ total: number; sucesso: number; erro: number; erros: Array<{ linha: number; erro: string }> }> {
  // TODO: Implementar processamento de retorno de folha
  return { total: 0, sucesso: 0, erro: 0, erros: [] }
}
