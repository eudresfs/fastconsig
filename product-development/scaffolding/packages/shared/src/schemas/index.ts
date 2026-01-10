import { z } from 'zod'

export const cpfSchema = z
  .string()
  .length(11, 'CPF deve ter 11 digitos')
  .regex(/^\d+$/, 'CPF deve conter apenas numeros')

export const cnpjSchema = z
  .string()
  .length(14, 'CNPJ deve ter 14 digitos')
  .regex(/^\d+$/, 'CNPJ deve conter apenas numeros')

export const emailSchema = z.string().email('Email invalido')

export const phoneSchema = z
  .string()
  .min(10, 'Telefone deve ter no minimo 10 digitos')
  .max(11, 'Telefone deve ter no maximo 11 digitos')
  .regex(/^\d+$/, 'Telefone deve conter apenas numeros')

export const passwordSchema = z
  .string()
  .min(8, 'Senha deve ter no minimo 8 caracteres')
  .regex(/[0-9]/, 'Senha deve conter pelo menos um numero')
  .regex(/[a-zA-Z]/, 'Senha deve conter pelo menos uma letra')

export const competenciaSchema = z
  .string()
  .regex(/^\d{2}\/\d{4}$/, 'Competencia deve estar no formato MM/YYYY')

export const paginationSchema = z.object({
  page: z.coerce.number().int().positive().default(1),
  pageSize: z.coerce.number().int().positive().max(100).default(20),
})

export const dateRangeSchema = z.object({
  dataInicio: z.coerce.date().optional(),
  dataFim: z.coerce.date().optional(),
})

export const currencySchema = z.coerce.number().nonnegative('Valor deve ser positivo')

export const percentSchema = z.coerce
  .number()
  .min(0, 'Percentual deve ser maior ou igual a 0')
  .max(100, 'Percentual deve ser menor ou igual a 100')

export type CPF = z.infer<typeof cpfSchema>
export type CNPJ = z.infer<typeof cnpjSchema>
export type Email = z.infer<typeof emailSchema>
export type Phone = z.infer<typeof phoneSchema>
export type Password = z.infer<typeof passwordSchema>
export type Competencia = z.infer<typeof competenciaSchema>
export type PaginationInput = z.infer<typeof paginationSchema>
export type DateRangeInput = z.infer<typeof dateRangeSchema>
