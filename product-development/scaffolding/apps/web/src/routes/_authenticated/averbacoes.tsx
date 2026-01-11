import { createFileRoute } from '@tanstack/react-router'
import { AverbacoesPage } from '@/features/averbacoes/pages/AverbacoesPage'

export const Route = createFileRoute('/_authenticated/averbacoes')({
  component: AverbacoesPage,
})
