import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { toast } from 'sonner'
import { trpc } from '@/lib/trpc'
import { Button, Input, Label, Card, CardHeader, CardTitle, CardDescription, CardContent, CardFooter } from '@fastconsig/ui'
import { Mail, ArrowLeft, Loader2 } from 'lucide-react'
import { Link } from '@tanstack/react-router'
import { useState } from 'react'

const forgotPasswordSchema = z.object({
  email: z.string().email('Digite um e-mail valido'),
})

type ForgotPasswordFormData = z.infer<typeof forgotPasswordSchema>

export function ForgotPasswordForm(): JSX.Element {
  const [isSuccess, setIsSuccess] = useState(false)

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<ForgotPasswordFormData>({
    resolver: zodResolver(forgotPasswordSchema),
  })

  const forgotPasswordMutation = trpc.auth.recuperarSenha.useMutation({
    onSuccess: () => {
      setIsSuccess(true)
    },
    onError: (error: any) => {
      toast.error(error.message || 'Erro ao enviar e-mail de recuperacao')
    },
  })

  const onSubmit = async (data: ForgotPasswordFormData): Promise<void> => {
    forgotPasswordMutation.mutate({ email: data.email })
  }

  if (isSuccess) {
    return (
      <Card className="w-full max-w-md">
        <CardHeader className="space-y-1">
          <div className="flex items-center justify-center mb-4">
            <div className="flex h-12 w-12 items-center justify-center rounded-full bg-green-100">
              <Mail className="h-6 w-6 text-green-600" aria-hidden="true" />
            </div>
          </div>
          <CardTitle className="text-2xl text-center">E-mail enviado!</CardTitle>
          <CardDescription className="text-center">
            Se o e-mail informado estiver cadastrado, voce recebera um link para redefinir sua senha.
          </CardDescription>
        </CardHeader>
        <CardFooter>
          <Link
            to="/login"
            className="w-full"
          >
            <Button variant="outline" className="w-full">
              Voltar para o login
            </Button>
          </Link>
        </CardFooter>
      </Card>
    )
  }

  return (
    <Card className="w-full max-w-md">
      <CardHeader className="space-y-1">
        <div className="flex items-center justify-center mb-4">
          <div className="flex h-12 w-12 items-center justify-center rounded-full bg-primary/10">
            <Mail className="h-6 w-6 text-primary" aria-hidden="true" />
          </div>
        </div>
        <CardTitle className="text-2xl text-center">Recuperar senha</CardTitle>
        <CardDescription className="text-center">
          Digite seu e-mail abaixo e enviaremos um link para voce redefinir sua senha.
        </CardDescription>
      </CardHeader>

      <form onSubmit={handleSubmit(onSubmit)}>
        <CardContent className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="email">E-mail</Label>
            <Input
              id="email"
              type="email"
              placeholder="seu.email@exemplo.com"
              aria-invalid={errors.email ? 'true' : 'false'}
              {...register('email')}
            />
            {errors.email && (
              <p className="text-sm text-destructive" role="alert">
                {errors.email.message}
              </p>
            )}
          </div>
        </CardContent>

        <CardFooter className="flex flex-col gap-4">
          <Button
            type="submit"
            className="w-full"
            disabled={isSubmitting}
          >
            {isSubmitting && (
              <Loader2 className="mr-2 h-4 w-4 animate-spin" aria-hidden="true" />
            )}
            {isSubmitting ? 'Enviando...' : 'Enviar link de recuperacao'}
          </Button>

          <Link
            to="/login"
            className="flex items-center justify-center gap-2 text-sm text-muted-foreground hover:text-foreground transition-colors"
          >
            <ArrowLeft className="h-4 w-4" aria-hidden="true" />
            Voltar para o login
          </Link>
        </CardFooter>
      </form>
    </Card>
  )
}
