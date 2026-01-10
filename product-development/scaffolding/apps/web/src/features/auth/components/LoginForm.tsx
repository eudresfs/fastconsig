import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useNavigate } from '@tanstack/react-router'
import { toast } from 'sonner'
import { trpc } from '@/lib/trpc'
import { useAuthStore } from '@/stores/auth'

const loginSchema = z.object({
  login: z.string().min(1, 'Login e obrigatorio'),
  senha: z.string().min(1, 'Senha e obrigatoria'),
})

type LoginFormData = z.infer<typeof loginSchema>

export function LoginForm(): JSX.Element {
  const navigate = useNavigate()
  const setAuth = useAuthStore((state) => state.setAuth)

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  })

  const loginMutation = trpc.auth.login.useMutation({
    onSuccess: (data) => {
      setAuth(data.accessToken, data.refreshToken, data.usuario as never)

      if (data.usuario.primeiroAcesso) {
        toast.info('Por favor, altere sua senha no primeiro acesso')
        navigate({ to: '/alterar-senha' })
      } else {
        toast.success('Login realizado com sucesso!')
        navigate({ to: '/dashboard' })
      }
    },
    onError: (error) => {
      toast.error(error.message)
    },
  })

  const onSubmit = (data: LoginFormData): void => {
    loginMutation.mutate(data)
  }

  return (
    <form className="space-y-6" onSubmit={handleSubmit(onSubmit)}>
      <div>
        <label
          className="block text-sm font-medium text-gray-700"
          htmlFor="login"
        >
          Login
        </label>
        <input
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
          id="login"
          type="text"
          {...register('login')}
        />
        {errors.login && (
          <p className="mt-1 text-sm text-red-600">{errors.login.message}</p>
        )}
      </div>

      <div>
        <label
          className="block text-sm font-medium text-gray-700"
          htmlFor="senha"
        >
          Senha
        </label>
        <input
          className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 shadow-sm focus:border-primary focus:outline-none focus:ring-1 focus:ring-primary"
          id="senha"
          type="password"
          {...register('senha')}
        />
        {errors.senha && (
          <p className="mt-1 text-sm text-red-600">{errors.senha.message}</p>
        )}
      </div>

      <button
        className="w-full rounded-md bg-primary px-4 py-2 text-white hover:bg-primary/90 focus:outline-none focus:ring-2 focus:ring-primary focus:ring-offset-2 disabled:opacity-50"
        disabled={isSubmitting}
        type="submit"
      >
        {isSubmitting ? 'Entrando...' : 'Entrar'}
      </button>

      <div className="text-center">
        <a
          className="text-sm text-primary hover:underline"
          href="/recuperar-senha"
        >
          Esqueci minha senha
        </a>
      </div>
    </form>
  )
}
