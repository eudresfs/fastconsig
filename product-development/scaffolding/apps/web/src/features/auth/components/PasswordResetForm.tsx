import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useNavigate, useSearch } from '@tanstack/react-router'
import { toast } from 'sonner'
import { trpc } from '@/lib/trpc'
import { Button, Input, Label, Card, CardHeader, CardTitle, CardDescription, CardContent, CardFooter } from '@fastconsig/ui'
import { KeyRound, ArrowLeft, Loader2 } from 'lucide-react'
import { Link } from '@tanstack/react-router'

const passwordResetSchema = z
  .object({
    novaSenha: z
      .string()
      .min(8, 'A senha deve ter no minimo 8 caracteres')
      .regex(/[A-Z]/, 'A senha deve conter pelo menos uma letra maiuscula')
      .regex(/[a-z]/, 'A senha deve conter pelo menos uma letra minuscula')
      .regex(/[0-9]/, 'A senha deve conter pelo menos um numero')
      .regex(/[^A-Za-z0-9]/, 'A senha deve conter pelo menos um caractere especial'),
    confirmarSenha: z.string().min(1, 'Confirmacao de senha e obrigatoria'),
  })
  .refine((data) => data.novaSenha === data.confirmarSenha, {
    message: 'As senhas nao coincidem',
    path: ['confirmarSenha'],
  })

type PasswordResetFormData = z.infer<typeof passwordResetSchema>

interface PasswordResetFormProps {
  /** Token for password reset (from email link) */
  token?: string
  /** Whether this is a first-access password change */
  isFirstAccess?: boolean
  /** Callback after successful password reset */
  onSuccess?: () => void
}

export function PasswordResetForm({
  token,
  isFirstAccess = false,
  onSuccess,
}: PasswordResetFormProps): JSX.Element {
  const navigate = useNavigate()

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    watch,
  } = useForm<PasswordResetFormData>({
    resolver: zodResolver(passwordResetSchema),
  })

  const novaSenha = watch('novaSenha', '')

  // Password strength indicators
  const passwordChecks = [
    { label: 'Minimo 8 caracteres', valid: novaSenha.length >= 8 },
    { label: 'Letra maiuscula', valid: /[A-Z]/.test(novaSenha) },
    { label: 'Letra minuscula', valid: /[a-z]/.test(novaSenha) },
    { label: 'Numero', valid: /[0-9]/.test(novaSenha) },
    { label: 'Caractere especial', valid: /[^A-Za-z0-9]/.test(novaSenha) },
  ]

  const resetMutation = trpc.auth.resetPassword.useMutation({
    onSuccess: () => {
      toast.success('Senha alterada com sucesso!')
      if (onSuccess) {
        onSuccess()
      } else {
        navigate({ to: '/login' })
      }
    },
    onError: (error) => {
      toast.error(error.message || 'Erro ao alterar senha')
    },
  })

  const onSubmit = (data: PasswordResetFormData): void => {
    resetMutation.mutate({
      token: token || '',
      novaSenha: data.novaSenha,
    })
  }

  return (
    <Card className="w-full max-w-md">
      <CardHeader className="space-y-1">
        <div className="flex items-center justify-center mb-4">
          <div className="flex h-12 w-12 items-center justify-center rounded-full bg-primary/10">
            <KeyRound className="h-6 w-6 text-primary" aria-hidden="true" />
          </div>
        </div>
        <CardTitle className="text-2xl text-center">
          {isFirstAccess ? 'Criar nova senha' : 'Redefinir senha'}
        </CardTitle>
        <CardDescription className="text-center">
          {isFirstAccess
            ? 'Por favor, crie uma nova senha para seu primeiro acesso ao sistema.'
            : 'Digite sua nova senha abaixo. Certifique-se de que ela atenda aos requisitos de seguranca.'}
        </CardDescription>
      </CardHeader>

      <form onSubmit={handleSubmit(onSubmit)}>
        <CardContent className="space-y-4">
          {/* New Password */}
          <div className="space-y-2">
            <Label htmlFor="novaSenha">Nova senha</Label>
            <Input
              id="novaSenha"
              type="password"
              placeholder="Digite sua nova senha"
              aria-describedby="password-requirements"
              aria-invalid={errors.novaSenha ? 'true' : 'false'}
              {...register('novaSenha')}
            />
            {errors.novaSenha && (
              <p className="text-sm text-destructive" role="alert">
                {errors.novaSenha.message}
              </p>
            )}
          </div>

          {/* Password Requirements */}
          <div
            id="password-requirements"
            className="rounded-lg bg-muted p-3 space-y-2"
            aria-label="Requisitos da senha"
          >
            <p className="text-sm font-medium text-muted-foreground">
              A senha deve conter:
            </p>
            <ul className="space-y-1">
              {passwordChecks.map((check) => (
                <li
                  key={check.label}
                  className={`flex items-center gap-2 text-sm ${
                    check.valid ? 'text-green-600' : 'text-muted-foreground'
                  }`}
                >
                  <span
                    className={`flex h-4 w-4 items-center justify-center rounded-full text-xs ${
                      check.valid
                        ? 'bg-green-100 text-green-600'
                        : 'bg-muted-foreground/20'
                    }`}
                    aria-hidden="true"
                  >
                    {check.valid ? '✓' : ''}
                  </span>
                  <span>{check.label}</span>
                </li>
              ))}
            </ul>
          </div>

          {/* Confirm Password */}
          <div className="space-y-2">
            <Label htmlFor="confirmarSenha">Confirmar senha</Label>
            <Input
              id="confirmarSenha"
              type="password"
              placeholder="Confirme sua nova senha"
              aria-invalid={errors.confirmarSenha ? 'true' : 'false'}
              {...register('confirmarSenha')}
            />
            {errors.confirmarSenha && (
              <p className="text-sm text-destructive" role="alert">
                {errors.confirmarSenha.message}
              </p>
            )}
          </div>
        </CardContent>

        <CardFooter className="flex flex-col gap-4">
          <Button
            type="submit"
            className="w-full"
            disabled={isSubmitting || resetMutation.isPending}
          >
            {(isSubmitting || resetMutation.isPending) && (
              <Loader2 className="mr-2 h-4 w-4 animate-spin" aria-hidden="true" />
            )}
            {isSubmitting || resetMutation.isPending
              ? 'Alterando senha...'
              : 'Alterar senha'}
          </Button>

          {!isFirstAccess && (
            <Link
              to="/login"
              className="flex items-center justify-center gap-2 text-sm text-muted-foreground hover:text-foreground transition-colors"
            >
              <ArrowLeft className="h-4 w-4" aria-hidden="true" />
              Voltar para o login
            </Link>
          )}
        </CardFooter>
      </form>
    </Card>
  )
}
