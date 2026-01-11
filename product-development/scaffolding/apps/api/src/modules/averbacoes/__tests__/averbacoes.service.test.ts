import { describe, it, expect, vi, beforeEach } from 'vitest'
import * as averbacoesService from '../averbacoes.service'
import { prisma } from '@fastconsig/database/client'
import { NotFoundError, ConflictError, BusinessError, MargemInsuficienteError } from '@/shared/errors'
import * as funcionariosService from '../../funcionarios/funcionarios.service'

// Mock dependencies
vi.mock('@fastconsig/database/client', () => ({
  prisma: {
    averbacao: {
      findFirst: vi.fn(),
      findMany: vi.fn(),
      count: vi.fn(),
      create: vi.fn(),
      update: vi.fn(),
      aggregate: vi.fn(),
    },
    funcionario: {
      findFirst: vi.fn(),
    },
    tenantConsignataria: {
      findFirst: vi.fn(),
    },
    produto: {
      findFirst: vi.fn(),
    },
    averbacaoHistorico: {
      create: vi.fn(),
      findMany: vi.fn(),
    },
    $transaction: vi.fn((callback) => {
      if (Array.isArray(callback)) {
        return Promise.resolve(callback)
      }
      return callback(prisma)
    }),
  },
}))

vi.mock('@/shared/middleware', () => ({
  logAuditAction: vi.fn(),
  AuditActions: {
    CRIAR: 'CRIAR',
    ATUALIZAR: 'ATUALIZAR',
    EXCLUIR: 'EXCLUIR',
    APROVAR: 'APROVAR',
    REJEITAR: 'REJEITAR',
  },
  computeAuditDiff: vi.fn(() => ({ dadosAnteriores: {}, dadosNovos: {} })),
  withTenantFilter: vi.fn((tenantId, where) => ({ ...where, tenantId })),
}))

vi.mock('../../funcionarios/funcionarios.service', () => ({
  verificarMargemDisponivel: vi.fn(),
  calcularMargem: vi.fn(),
}))

describe('AverbacoesService', () => {
  const tenantId = 1
  const usuarioId = 1
  const ctx = { userId: 1 } as any

  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('reservarMargem', () => {
    it('should return success if margin is available', async () => {
      vi.mocked(funcionariosService.verificarMargemDisponivel).mockResolvedValue({
        disponivel: true,
        margemDisponivel: 100,
        margemRequerida: 50,
        diferenca: 50,
      })

      const result = await averbacoesService.reservarMargem(tenantId, 1, 50)
      expect(result.sucesso).toBe(true)
    })

    it('should return failure if margin is insufficient', async () => {
      vi.mocked(funcionariosService.verificarMargemDisponivel).mockResolvedValue({
        disponivel: false,
        margemDisponivel: 10,
        margemRequerida: 50,
        diferenca: -40,
      })

      const result = await averbacoesService.reservarMargem(tenantId, 1, 50)
      expect(result.sucesso).toBe(false)
    })
  })

  describe('criar', () => {
    const input = {
      funcionarioId: 1,
      tenantConsignatariaId: 1,
      produtoId: 1,
      numeroContrato: '123',
      valorParcela: 100,
      valorTotal: 1000,
      parcelasTotal: 10,
      taxaMensal: 1,
      dataContrato: new Date(),
      dataInicioDesconto: new Date(),
      dataFimDesconto: new Date(),
    }

    it('should create averbacao successfully', async () => {
      vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ situacao: 'ATIVO' } as any)
      vi.mocked(prisma.tenantConsignataria.findFirst).mockResolvedValue({ id: 1 } as any)
      vi.mocked(prisma.produto.findFirst).mockResolvedValue({ id: 1 } as any)
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue(null) // No duplicate
      vi.mocked(funcionariosService.verificarMargemDisponivel).mockResolvedValue({
        disponivel: true,
        margemDisponivel: 200,
        margemRequerida: 100,
        diferenca: 100,
      })
      vi.mocked(prisma.averbacao.create).mockResolvedValue({
        id: 1,
        ...input,
        situacao: 'AGUARDANDO_APROVACAO',
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      await averbacoesService.criar(tenantId, input as any, usuarioId, ctx)

      expect(prisma.averbacao.create).toHaveBeenCalled()
      expect(prisma.averbacaoHistorico.create).toHaveBeenCalled()
    })

    it('should throw if funcionario inactive', async () => {
      vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ situacao: 'INATIVO' } as any)
      await expect(averbacoesService.criar(tenantId, input as any, usuarioId)).rejects.toThrow(BusinessError)
    })

    it('should throw if margin insufficient', async () => {
      vi.mocked(prisma.funcionario.findFirst).mockResolvedValue({ situacao: 'ATIVO' } as any)
      vi.mocked(prisma.tenantConsignataria.findFirst).mockResolvedValue({ id: 1 } as any)
      vi.mocked(prisma.produto.findFirst).mockResolvedValue({ id: 1 } as any)
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue(null)
      vi.mocked(funcionariosService.verificarMargemDisponivel).mockResolvedValue({
        disponivel: false,
        margemDisponivel: 50,
        margemRequerida: 100,
        diferenca: -50,
      })

      await expect(averbacoesService.criar(tenantId, input as any, usuarioId)).rejects.toThrow(MargemInsuficienteError)
    })
  })

  describe('atualizar', () => {
    it('should update successfully', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({
        id: 1,
        situacao: 'AGUARDANDO_APROVACAO',
        valorParcela: 100,
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)
      vi.mocked(prisma.averbacao.update).mockResolvedValue({
        id: 1,
        situacao: 'AGUARDANDO_APROVACAO',
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      await averbacoesService.atualizar(tenantId, 1, { observacao: 'Update' }, ctx)
      expect(prisma.averbacao.update).toHaveBeenCalled()
    })

    it('should check margin if parcel value increases', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({
        id: 1,
        situacao: 'AGUARDANDO_APROVACAO',
        valorParcela: 100,
        funcionarioId: 1,
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      vi.mocked(funcionariosService.verificarMargemDisponivel).mockResolvedValue({
        disponivel: true,
        margemDisponivel: 100,
        margemRequerida: 50,
        diferenca: 50,
      })

      vi.mocked(prisma.averbacao.update).mockResolvedValue({
        id: 1,
        situacao: 'AGUARDANDO_APROVACAO',
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      await averbacoesService.atualizar(tenantId, 1, { valorParcela: 150 }, ctx)
      expect(funcionariosService.verificarMargemDisponivel).toHaveBeenCalled()
    })

    it('should throw if state not editable', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({
        id: 1,
        situacao: 'APROVADA',
      } as any)

      await expect(averbacoesService.atualizar(tenantId, 1, {})).rejects.toThrow(BusinessError)
    })
  })

  describe('alterarSituacao', () => {
    it('should change state successfully', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({
        id: 1,
        situacao: 'AGUARDANDO_APROVACAO',
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      vi.mocked(prisma.averbacao.update).mockResolvedValue({
        id: 1,
        situacao: 'APROVADA',
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      await averbacoesService.aprovar(tenantId, 1, usuarioId, 'Ok', ctx)

      expect(prisma.averbacao.update).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({ situacao: 'APROVADA' })
      }))
      expect(prisma.averbacaoHistorico.create).toHaveBeenCalled()
    })

    it('should throw if invalid transition', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({
        id: 1,
        situacao: 'REJEITADA',
      } as any)

      await expect(averbacoesService.aprovar(tenantId, 1, usuarioId)).rejects.toThrow()
    })

    it('should require reason for rejection', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({
        id: 1,
        situacao: 'AGUARDANDO_APROVACAO',
      } as any)

      // Calling alterarSituacao directly to pass empty motive
      await expect(
        averbacoesService.alterarSituacao(tenantId, 1, 'REJEITADA', usuarioId)
      ).rejects.toThrow(BusinessError)
    })
  })

  describe('atualizarParcelasPagas', () => {
    it('should update paid parcels', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({
        id: 1,
        situacao: 'DESCONTADA',
        parcelasTotal: 10,
        valorParcela: 100,
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      vi.mocked(prisma.averbacao.update).mockResolvedValue({
        id: 1,
        parcelasPagas: 5,
        situacao: 'DESCONTADA',
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      await averbacoesService.atualizarParcelasPagas(tenantId, 1, 5, ctx)
      expect(prisma.averbacao.update).toHaveBeenCalled()
    })

    it('should liquidate if all parcels paid', async () => {
       vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({
        id: 1,
        situacao: 'DESCONTADA',
        parcelasTotal: 10,
        valorParcela: 100,
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      vi.mocked(prisma.averbacao.update).mockResolvedValue({
        id: 1,
        situacao: 'LIQUIDADA',
        funcionario: {},
        tenantConsignataria: { consignataria: {} },
        produto: {},
      } as any)

      await averbacoesService.atualizarParcelasPagas(tenantId, 1, 10, ctx)

      expect(prisma.averbacao.update).toHaveBeenCalledWith(expect.objectContaining({
          data: expect.objectContaining({ situacao: 'LIQUIDADA' })
      }))
    })

    it('should throw if invalid parcels count', async () => {
       vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({
        id: 1,
        parcelasTotal: 10,
      } as any)

      await expect(averbacoesService.atualizarParcelasPagas(tenantId, 1, 11)).rejects.toThrow(BusinessError)
    })
  })

  describe('listar', () => {
    it('should return paginated list', async () => {
      vi.mocked(prisma.averbacao.findMany).mockResolvedValue([{ id: 1, valorTotal: 1000, situacao: 'AGUARDANDO_APROVACAO', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} }] as any)
      vi.mocked(prisma.averbacao.count).mockResolvedValue(1)

      const result = await averbacoesService.listar(tenantId, {
        page: 1,
        pageSize: 10,
        orderBy: 'createdAt',
        orderDir: 'desc'
      })

      expect(result.data).toHaveLength(1)
      expect(result.pagination.total).toBe(1)
    })

    it('should apply filters', async () => {
      vi.mocked(prisma.averbacao.findMany).mockResolvedValue([])
      vi.mocked(prisma.averbacao.count).mockResolvedValue(0)

      await averbacoesService.listar(tenantId, {
        page: 1,
        pageSize: 10,
        search: 'test',
        situacao: 'APROVADA',
        orderBy: 'createdAt',
        orderDir: 'desc'
      })

      expect(prisma.averbacao.findMany).toHaveBeenCalledWith(expect.objectContaining({
        where: expect.objectContaining({
          situacao: 'APROVADA',
          OR: expect.arrayContaining([
            { numeroContrato: { contains: 'test', mode: 'insensitive' } }
          ])
        })
      }))
    })
  })

  describe('buscarPorId', () => {
    it('should return averbacao', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({ id: 1, situacao: 'AGUARDANDO_APROVACAO', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)
      const result = await averbacoesService.buscarPorId(tenantId, 1)
      expect(result.id).toBe(1)
    })

    it('should throw if not found', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue(null)
      await expect(averbacoesService.buscarPorId(tenantId, 1)).rejects.toThrow(NotFoundError)
    })
  })

  describe('buscarPorNumeroContrato', () => {
    it('should return averbacao', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({ id: 1, situacao: 'AGUARDANDO_APROVACAO', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)
      const result = await averbacoesService.buscarPorNumeroContrato(tenantId, '123')
      expect(result).not.toBeNull()
    })

    it('should return null if not found', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue(null)
      const result = await averbacoesService.buscarPorNumeroContrato(tenantId, '123')
      expect(result).toBeNull()
    })
  })

  describe('listarPorFuncionario', () => {
    it('should list averbacoes for funcionario', async () => {
      vi.mocked(prisma.averbacao.findMany).mockResolvedValue([{ id: 1, situacao: 'AGUARDANDO_APROVACAO', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} }] as any)
      const result = await averbacoesService.listarPorFuncionario(tenantId, 1)
      expect(result).toHaveLength(1)
    })
  })

  describe('listarHistorico', () => {
    it('should list history', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({ id: 1 } as any)
      vi.mocked(prisma.averbacaoHistorico.findMany).mockResolvedValue([{ id: 1, usuario: { id: 1, nome: 'User' } }] as any)

      const result = await averbacoesService.listarHistorico(tenantId, 1)
      expect(result).toHaveLength(1)
    })

    it('should throw if averbacao not found', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue(null)
      await expect(averbacoesService.listarHistorico(tenantId, 1)).rejects.toThrow(NotFoundError)
    })
  })

  describe('obterResumo', () => {
    it('should return summary stats', async () => {
      vi.mocked(prisma.averbacao.count).mockResolvedValue(10)
      vi.mocked(prisma.averbacao.aggregate).mockResolvedValue({
        _sum: { valorTotal: 1000, valorParcela: 100 }
      } as any)

      const result = await averbacoesService.obterResumo(tenantId)

      expect(result.total).toBe(10)
      expect(result.valorTotalContratado).toBe(1000)
    })
  })

  describe('Other state transitions', () => {
    it('should reject averbacao', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({ id: 1, situacao: 'AGUARDANDO_APROVACAO', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)
      vi.mocked(prisma.averbacao.update).mockResolvedValue({ id: 1, situacao: 'REJEITADA', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)

      await averbacoesService.rejeitar(tenantId, 1, usuarioId, 'Reason', ctx)
      expect(prisma.averbacao.update).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({ situacao: 'REJEITADA', motivoRejeicao: 'Reason' })
      }))
    })

    it('should suspend averbacao', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({ id: 1, situacao: 'APROVADA', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)
      vi.mocked(prisma.averbacao.update).mockResolvedValue({ id: 1, situacao: 'SUSPENSA', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)

      await averbacoesService.suspender(tenantId, 1, usuarioId, 'Reason', ctx)
      expect(prisma.averbacao.update).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({ situacao: 'SUSPENSA' })
      }))
    })

    it('should cancel averbacao', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({ id: 1, situacao: 'APROVADA', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)
      vi.mocked(prisma.averbacao.update).mockResolvedValue({ id: 1, situacao: 'CANCELADA', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)

      await averbacoesService.cancelar(tenantId, 1, usuarioId, 'Reason', ctx)
      expect(prisma.averbacao.update).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({ situacao: 'CANCELADA' })
      }))
    })

    it('should block averbacao', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({ id: 1, situacao: 'APROVADA', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)
      vi.mocked(prisma.averbacao.update).mockResolvedValue({ id: 1, situacao: 'BLOQUEADA', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)

      await averbacoesService.bloquear(tenantId, 1, usuarioId, 'Reason', ctx)
      expect(prisma.averbacao.update).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({ situacao: 'BLOQUEADA' })
      }))
    })

    it('should reactivate averbacao', async () => {
      vi.mocked(prisma.averbacao.findFirst).mockResolvedValue({ id: 1, situacao: 'SUSPENSA', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)
      vi.mocked(prisma.averbacao.update).mockResolvedValue({ id: 1, situacao: 'APROVADA', funcionario: {}, tenantConsignataria: { consignataria: {} }, produto: {} } as any)

      await averbacoesService.reativar(tenantId, 1, usuarioId, 'APROVADA', 'Reason', ctx)
      expect(prisma.averbacao.update).toHaveBeenCalledWith(expect.objectContaining({
        data: expect.objectContaining({ situacao: 'APROVADA' })
      }))
    })
  })
})
