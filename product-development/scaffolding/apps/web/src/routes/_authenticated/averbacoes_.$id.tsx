import { createFileRoute } from '@tanstack/react-router'
import { AverbacaoDetailPage } from '@/features/averbacoes/pages/AverbacaoDetailPage'

export const Route = createFileRoute('/_authenticated/averbacoes/$id')({
  component: AverbacaoDetailPage,
})
