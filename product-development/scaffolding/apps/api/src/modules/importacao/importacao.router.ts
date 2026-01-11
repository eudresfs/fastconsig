import { router, protectedProcedure, withPermission } from '@/trpc/trpc'
import { z } from 'zod'
import * as ImportacaoService from './importacao.service'

export const importacaoRouter = router({
  list: protectedProcedure
    .use(withPermission('IMPORTACAO_VISUALIZAR'))
    .input(z.object({
      page: z.number().default(1),
      pageSize: z.number().default(20),
      tipo: z.enum(['FUNCIONARIOS', 'CONTRATOS', 'RETORNO_FOLHA']).optional(),
      status: z.enum(['PENDENTE', 'PROCESSANDO', 'CONCLUIDO', 'ERRO', 'CANCELADO']).optional(),
    }))
    .query(async ({ input, ctx }) => {
      return ImportacaoService.listarImportacoes(ctx.tenantId!, input.page, input.pageSize, {
        tipo: input.tipo,
        status: input.status
      })
    }),

  getById: protectedProcedure
    .use(withPermission('IMPORTACAO_VISUALIZAR'))
    .input(z.object({ id: z.number() }))
    .query(async ({ input, ctx }) => {
      return ImportacaoService.obterImportacao(ctx.tenantId!, input.id)
    }),

  cancelar: protectedProcedure
    .use(withPermission('IMPORTACAO_CRIAR'))
    .input(z.object({ id: z.number() }))
    .mutation(async ({ input, ctx }) => {
      return ImportacaoService.cancelarImportacao(ctx.tenantId!, input.id)
    }),
})
