import { createFileRoute } from '@tanstack/react-router'
import { PasswordResetForm } from '@/features/auth/components/PasswordResetForm'

export const Route = createFileRoute('/alterar-senha')({
  component: AlterarSenhaPage,
})

function AlterarSenhaPage() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4 py-12 sm:px-6 lg:px-8">
      <div className="w-full max-w-md space-y-8">
        <div className="text-center">
          <h2 className="mt-6 text-3xl font-bold tracking-tight text-gray-900">
            Alterar Senha
          </h2>
          <p className="mt-2 text-sm text-gray-600">
            Configure sua nova senha de acesso
          </p>
        </div>

        <PasswordResetForm isFirstAccess />
      </div>
    </div>
  )
}
