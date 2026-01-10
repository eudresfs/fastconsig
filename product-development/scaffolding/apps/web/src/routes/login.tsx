import { createFileRoute } from '@tanstack/react-router'
import { LoginForm } from '@/features/auth/components/LoginForm'

export const Route = createFileRoute('/login')({
  component: LoginPage,
})

function LoginPage(): JSX.Element {
  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50">
      <div className="w-full max-w-md">
        <div className="rounded-lg bg-white p-8 shadow-lg">
          <div className="mb-8 text-center">
            <h1 className="text-2xl font-bold text-gray-900">FastConsig</h1>
            <p className="mt-2 text-sm text-gray-600">
              Sistema de Gestao de Consignados
            </p>
          </div>
          <LoginForm />
        </div>
      </div>
    </div>
  )
}
