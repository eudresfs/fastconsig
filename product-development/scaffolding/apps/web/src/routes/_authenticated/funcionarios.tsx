import { createFileRoute } from '@tanstack/react-router'
import { FuncionariosPage } from '@/features/funcionarios'

export const Route = createFileRoute('/_authenticated/funcionarios')({
  component: FuncionariosPage,
})
