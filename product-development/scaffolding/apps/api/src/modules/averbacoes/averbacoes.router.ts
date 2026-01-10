import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { z } from 'zod'
import {
  averbacaoSchema,
  averbacaoFiltroSchema,
  aprovarAverbacaoSchema,
  rejeitarAverbacaoSchema,
} from './averbacoes.schema'

export const averbacaoRouter = router({
  list: protectedProcedure
    .use(withPermission('AVERBACOES_VISUALIZAR'))
    .input(averbacaoFiltroSchema)
    .query(async ({ input, ctx }) => {
      const { page, pageSize, search, situacao, funcionarioId, tenantConsignatariaId, dataInicio, dataFim, orderBy, orderDir } = input
      const skip = (page - 1) * pageSize

      const where = {
        tenantId: ctx.tenantId,
        ...(search && {
          OR: [
            { numeroContrato: { contains: search } },
            { funcionario: { nome: { contains: search, mode: 'insensitive' as const } } },
            { funcionario: { cpf: { contains: search } } },
          ],
        }),
        ...(situacao && { situacao }),
        ...(funcionarioId && { funcionarioId }),
        ...(tenantConsignatariaId && { tenantConsignatariaId }),
        ...(dataInicio && { dataContrato: { gte: dataInicio } }),
        ...(dataFim && { dataContrato: { lte: dataFim } }),
      }

      const [averbacoes, total] = await Promise.all([
        ctx.prisma.averbacao.findMany({
          where,
          include: {
            funcionario: { select: { id: true, nome: true, cpf: true, matricula: true } },
            tenantConsignataria: { include: { consignataria: { select: { razaoSocial: true } } } },
            produto: { select: { id: true, nome: true, tipo: true } },
          },
          skip,
          take: pageSize,
          orderBy: { [orderBy]: orderDir },
        }),
        ctx.prisma.averbacao.count({ where }),
      ])

      return {
        data: averbacoes,
        pagination: { page, pageSize, total, totalPages: Math.ceil(total / pageSize) },
      }
    }),

  getById: protectedProcedure
    .use(withPermission('AVERBACOES_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      const averbacao = await ctx.prisma.averbacao.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
        include: {
          funcionario: true,
          tenantConsignataria: { include: { consignataria: true } },
          produto: true,
          averbacaoVinculada: true,
          historico: { orderBy: { createdAt: 'desc' }, include: { usuario: { select: { nome: true } } } },
          usuarioCriacao: { select: { nome: true } },
          usuarioAprovacao: { select: { nome: true } },
        },
      })

      if (!averbacao) {
        throw new Error('Averbacao nao encontrada')
      }

      return averbacao
    }),

  create: protectedProcedure
    .use(withPermission('AVERBACOES_CRIAR'))
    .input(averbacaoSchema)
    .mutation(async ({ input, ctx }) => {
      // Verificar se contrato ja existe
      const existente = await ctx.prisma.averbacao.findFirst({
        where: { tenantId: ctx.tenantId, numeroContrato: input.numeroContrato },
      })

      if (existente) {
        throw new Error('Ja existe uma averbacao com este numero de contrato')
      }

      const averbacao = await ctx.prisma.averbacao.create({
        data: {
          ...input,
          tenantId: ctx.tenantId,
          situacao: 'AGUARDANDO_APROVACAO',
          usuarioCriacaoId: ctx.userId,
        },
      })

      // Criar historico
      await ctx.prisma.averbacaoHistorico.create({
        data: {
          averbacaoId: averbacao.id,
          situacaoNova: 'AGUARDANDO_APROVACAO',
          usuarioId: ctx.userId,
          observacao: 'Averbacao criada',
        },
      })

      return averbacao
    }),

  aprovar: protectedProcedure
    .use(withPermission('AVERBACOES_APROVAR'))
    .input(aprovarAverbacaoSchema)
    .mutation(async ({ input, ctx }) => {
      const averbacao = await ctx.prisma.averbacao.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
      })

      if (!averbacao) {
        throw new Error('Averbacao nao encontrada')
      }

      if (averbacao.situacao !== 'AGUARDANDO_APROVACAO') {
        throw new Error('Apenas averbacoes aguardando aprovacao podem ser aprovadas')
      }

      const atualizada = await ctx.prisma.averbacao.update({
        where: { id: input.id },
        data: {
          situacao: 'APROVADA',
          usuarioAprovacaoId: ctx.userId,
          dataAprovacao: new Date(),
        },
      })

      await ctx.prisma.averbacaoHistorico.create({
        data: {
          averbacaoId: input.id,
          situacaoAnterior: 'AGUARDANDO_APROVACAO',
          situacaoNova: 'APROVADA',
          usuarioId: ctx.userId,
          observacao: input.observacao,
        },
      })

      return atualizada
    }),

  rejeitar: protectedProcedure
    .use(withPermission('AVERBACOES_APROVAR'))
    .input(rejeitarAverbacaoSchema)
    .mutation(async ({ input, ctx }) => {
      const averbacao = await ctx.prisma.averbacao.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
      })

      if (!averbacao) {
        throw new Error('Averbacao nao encontrada')
      }

      if (averbacao.situacao !== 'AGUARDANDO_APROVACAO') {
        throw new Error('Apenas averbacoes aguardando aprovacao podem ser rejeitadas')
      }

      const atualizada = await ctx.prisma.averbacao.update({
        where: { id: input.id },
        data: {
          situacao: 'REJEITADA',
          motivoRejeicao: input.motivoRejeicao,
        },
      })

      await ctx.prisma.averbacaoHistorico.create({
        data: {
          averbacaoId: input.id,
          situacaoAnterior: 'AGUARDANDO_APROVACAO',
          situacaoNova: 'REJEITADA',
          usuarioId: ctx.userId,
          observacao: input.motivoRejeicao,
        },
      })

      return atualizada
    }),
})
