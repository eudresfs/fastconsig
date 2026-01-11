import { createFileRoute } from '@tanstack/react-router'
import { ImportacaoPage } from '@/features/importacao/pages/ImportacaoPage'

export const Route = createFileRoute('/_authenticated/importacao')({
  component: ImportacaoPage,
})
