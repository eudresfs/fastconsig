import { router } from './trpc'
import { authRouter } from '@/modules/auth/auth.router'
import { funcionariosRouter } from '@/modules/funcionarios/funcionarios.router'
import { averbacaoRouter } from '@/modules/averbacoes/averbacoes.router'
import { margemRouter } from '@/modules/margem/margem.router'
import { simulacaoRouter } from '@/modules/simulacao/simulacao.router'
import { consignatariasRouter } from '@/modules/consignatarias/consignatarias.router'
import { conciliacaoRouter } from '@/modules/conciliacao/conciliacao.router'
import { relatoriosRouter } from '@/modules/relatorios/relatorios.router'
import { importacaoRouter } from '@/modules/importacao/importacao.router'
import { auditoriaRouter } from '@/modules/auditoria/auditoria.router'

export const appRouter = router({
  auth: authRouter,
  funcionarios: funcionariosRouter,
  averbacoes: averbacaoRouter,
  margem: margemRouter,
  simulacao: simulacaoRouter,
  consignatarias: consignatariasRouter,
  conciliacao: conciliacaoRouter,
  relatorios: relatoriosRouter,
  importacao: importacaoRouter,
  auditoria: auditoriaRouter,
})

export type AppRouter = typeof appRouter
