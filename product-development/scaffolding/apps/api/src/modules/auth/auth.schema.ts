import { z } from 'zod'

export const loginSchema = z.object({
  login: z.string().min(1, 'Login e obrigatorio'),
  senha: z.string().min(1, 'Senha e obrigatoria'),
})

export const refreshTokenSchema = z.object({
  refreshToken: z.string().min(1, 'Refresh token e obrigatorio'),
})

export const alterarSenhaSchema = z.object({
  senhaAtual: z.string().min(1, 'Senha atual e obrigatoria'),
  novaSenha: z
    .string()
    .min(8, 'A nova senha deve ter no minimo 8 caracteres')
    .regex(/[0-9]/, 'A senha deve conter pelo menos um numero')
    .regex(/[a-zA-Z]/, 'A senha deve conter pelo menos uma letra'),
  confirmarSenha: z.string().min(1, 'Confirmacao de senha e obrigatoria'),
}).refine((data) => data.novaSenha === data.confirmarSenha, {
  message: 'As senhas nao conferem',
  path: ['confirmarSenha'],
})

export const recuperarSenhaSchema = z.object({
  email: z.string().email('Email invalido'),
})

export type LoginInput = z.infer<typeof loginSchema>
export type RefreshTokenInput = z.infer<typeof refreshTokenSchema>
export type AlterarSenhaInput = z.infer<typeof alterarSenhaSchema>
export type RecuperarSenhaInput = z.infer<typeof recuperarSenhaSchema>
