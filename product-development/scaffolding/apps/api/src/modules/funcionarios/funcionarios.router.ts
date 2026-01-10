import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { z } from 'zod'
import {
  funcionarioSchema,
  funcionarioUpdateSchema,
  funcionarioFiltroSchema,
} from './funcionarios.schema'

export const funcionariosRouter = router({
  list: protectedProcedure
    .use(withPermission('FUNCIONARIOS_VISUALIZAR'))
    .input(funcionarioFiltroSchema)
    .query(async ({ input, ctx }) => {
      const { page, pageSize, search, situacao, empresaId, orderBy, orderDir } = input
      const skip = (page - 1) * pageSize

      const where = {
        tenantId: ctx.tenantId,
        ...(search && {
          OR: [
            { nome: { contains: search, mode: 'insensitive' as const } },
            { cpf: { contains: search } },
            { matricula: { contains: search } },
          ],
        }),
        ...(situacao && { situacao }),
        ...(empresaId && { empresaId }),
      }

      const [funcionarios, total] = await Promise.all([
        ctx.prisma.funcionario.findMany({
          where,
          include: { empresa: true },
          skip,
          take: pageSize,
          orderBy: { [orderBy]: orderDir },
        }),
        ctx.prisma.funcionario.count({ where }),
      ])

      return {
        data: funcionarios,
        pagination: {
          page,
          pageSize,
          total,
          totalPages: Math.ceil(total / pageSize),
        },
      }
    }),

  getById: protectedProcedure
    .use(withPermission('FUNCIONARIOS_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      const funcionario = await ctx.prisma.funcionario.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
        include: {
          empresa: true,
          averbacoes: {
            orderBy: { createdAt: 'desc' },
            take: 10,
            include: {
              tenantConsignataria: { include: { consignataria: true } },
              produto: true,
            },
          },
          margemHistorico: {
            orderBy: { competencia: 'desc' },
            take: 12,
          },
        },
      })

      if (!funcionario) {
        throw new Error('Funcionario nao encontrado')
      }

      return funcionario
    }),

  create: protectedProcedure
    .use(withPermission('FUNCIONARIOS_CRIAR'))
    .input(funcionarioSchema)
    .mutation(async ({ input, ctx }) => {
      const existente = await ctx.prisma.funcionario.findFirst({
        where: {
          tenantId: ctx.tenantId,
          OR: [
            { cpf: input.cpf },
            { empresaId: input.empresaId, matricula: input.matricula },
          ],
        },
      })

      if (existente) {
        throw new Error('Ja existe um funcionario com este CPF ou matricula')
      }

      const funcionario = await ctx.prisma.funcionario.create({
        data: {
          ...input,
          tenantId: ctx.tenantId,
        },
      })

      return funcionario
    }),

  update: protectedProcedure
    .use(withPermission('FUNCIONARIOS_EDITAR'))
    .input(funcionarioUpdateSchema)
    .mutation(async ({ input, ctx }) => {
      const { id, ...data } = input

      const funcionario = await ctx.prisma.funcionario.findFirst({
        where: { id, tenantId: ctx.tenantId },
      })

      if (!funcionario) {
        throw new Error('Funcionario nao encontrado')
      }

      const atualizado = await ctx.prisma.funcionario.update({
        where: { id },
        data,
      })

      return atualizado
    }),

  delete: protectedProcedure
    .use(withPermission('FUNCIONARIOS_EXCLUIR'))
    .input(z.object({ id: z.number() }))
    .mutation(async ({ input, ctx }) => {
      const funcionario = await ctx.prisma.funcionario.findFirst({
        where: { id: input.id, tenantId: ctx.tenantId },
        include: { averbacoes: { take: 1 } },
      })

      if (!funcionario) {
        throw new Error('Funcionario nao encontrado')
      }

      if (funcionario.averbacoes.length > 0) {
        throw new Error('Nao e possivel excluir funcionario com averbacoes')
      }

      await ctx.prisma.funcionario.delete({ where: { id: input.id } })

      return { success: true }
    }),
})
