import { z } from 'zod'

export const funcionarioSchema = z.object({
  cpf: z.string().length(11, 'CPF deve ter 11 digitos'),
  nome: z.string().min(3, 'Nome deve ter no minimo 3 caracteres').max(100),
  dataNascimento: z.coerce.date(),
  sexo: z.enum(['M', 'F']).optional(),
  email: z.string().email('Email invalido').optional().nullable(),
  telefone: z.string().max(20).optional().nullable(),
  matricula: z.string().min(1, 'Matricula e obrigatoria').max(20),
  cargo: z.string().max(100).optional().nullable(),
  dataAdmissao: z.coerce.date(),
  salarioBruto: z.coerce.number().positive('Salario deve ser positivo'),
  situacao: z.enum(['ATIVO', 'INATIVO', 'AFASTADO', 'BLOQUEADO', 'APOSENTADO']).default('ATIVO'),
  empresaId: z.number().int().positive(),
  banco: z.string().length(3).optional().nullable(),
  agencia: z.string().max(10).optional().nullable(),
  conta: z.string().max(20).optional().nullable(),
  tipoConta: z.enum(['CORRENTE', 'POUPANCA', 'SALARIO']).optional().nullable(),
})

export const funcionarioUpdateSchema = funcionarioSchema.partial().extend({
  id: z.number().int().positive(),
})

export const funcionarioFiltroSchema = z.object({
  page: z.number().int().positive().default(1),
  pageSize: z.number().int().positive().max(100).default(20),
  search: z.string().optional(),
  situacao: z.enum(['ATIVO', 'INATIVO', 'AFASTADO', 'BLOQUEADO', 'APOSENTADO']).optional(),
  empresaId: z.number().int().positive().optional(),
  orderBy: z.enum(['nome', 'matricula', 'cpf', 'createdAt']).default('nome'),
  orderDir: z.enum(['asc', 'desc']).default('asc'),
})

export type FuncionarioInput = z.infer<typeof funcionarioSchema>
export type FuncionarioUpdateInput = z.infer<typeof funcionarioUpdateSchema>
export type FuncionarioFiltro = z.infer<typeof funcionarioFiltroSchema>
