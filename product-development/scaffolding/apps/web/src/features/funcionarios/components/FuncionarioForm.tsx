import { useEffect } from 'react'
import { useForm, Controller } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { trpc } from '@/lib/trpc'
import { toast } from 'sonner'
import {
  Button,
  Input,
  Label,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
  Skeleton,
} from '@fastconsig/ui'
import { Loader2 } from 'lucide-react'

// Form validation schema
const funcionarioFormSchema = z.object({
  cpf: z
    .string()
    .min(11, 'CPF deve ter 11 digitos')
    .max(14, 'CPF invalido')
    .transform((val) => val.replace(/\D/g, '')),
  nome: z.string().min(3, 'Nome deve ter no minimo 3 caracteres').max(100),
  dataNascimento: z.string().min(1, 'Data de nascimento e obrigatoria'),
  sexo: z.enum(['M', 'F']).optional(),
  email: z.string().email('Email invalido').optional().or(z.literal('')),
  telefone: z.string().max(20).optional(),
  matricula: z.string().min(1, 'Matricula e obrigatoria').max(20),
  cargo: z.string().max(100).optional(),
  dataAdmissao: z.string().min(1, 'Data de admissao e obrigatoria'),
  salarioBruto: z.coerce.number().positive('Salario deve ser positivo'),
  situacao: z.enum(['ATIVO', 'INATIVO', 'AFASTADO', 'BLOQUEADO', 'APOSENTADO']),
  empresaId: z.coerce.number().int().positive('Empresa e obrigatoria'),
  banco: z.string().length(3).optional().or(z.literal('')),
  agencia: z.string().max(10).optional(),
  conta: z.string().max(20).optional(),
  tipoConta: z.enum(['CORRENTE', 'POUPANCA', 'SALARIO']).optional(),
})

type FuncionarioFormData = z.infer<typeof funcionarioFormSchema>

interface FuncionarioFormProps {
  funcionarioId?: number | null
  onSuccess: () => void
  onCancel: () => void
}

const situacaoOptions = [
  { value: 'ATIVO', label: 'Ativo' },
  { value: 'INATIVO', label: 'Inativo' },
  { value: 'AFASTADO', label: 'Afastado' },
  { value: 'BLOQUEADO', label: 'Bloqueado' },
  { value: 'APOSENTADO', label: 'Aposentado' },
]

const sexoOptions = [
  { value: 'M', label: 'Masculino' },
  { value: 'F', label: 'Feminino' },
]

const tipoContaOptions = [
  { value: 'CORRENTE', label: 'Conta Corrente' },
  { value: 'POUPANCA', label: 'Poupanca' },
  { value: 'SALARIO', label: 'Conta Salario' },
]

export function FuncionarioForm({
  funcionarioId,
  onSuccess,
  onCancel,
}: FuncionarioFormProps): JSX.Element {
  const isEditing = funcionarioId !== null && funcionarioId !== undefined

  // Queries
  const funcionarioQuery = trpc.funcionarios.getById.useQuery(
    { id: funcionarioId! },
    { enabled: isEditing }
  )

  const empresasQuery = trpc.consignatarias.listEmpresas.useQuery()

  // Form setup
  const {
    register,
    handleSubmit,
    control,
    reset,
    formState: { errors },
  } = useForm<FuncionarioFormData>({
    resolver: zodResolver(funcionarioFormSchema),
    defaultValues: {
      situacao: 'ATIVO',
    },
  })

  // Populate form when editing
  useEffect(() => {
    if (funcionarioQuery.data) {
      const data = funcionarioQuery.data
      reset({
        cpf: data.cpf,
        nome: data.nome,
        dataNascimento: (data.dataNascimento
          ? new Date(data.dataNascimento).toISOString().split('T')[0]
          : '') as string,
        sexo: data.sexo as 'M' | 'F' | undefined,
        email: data.email || '',
        telefone: data.telefone || '',
        matricula: data.matricula,
        cargo: data.cargo || '',
        dataAdmissao: (data.dataAdmissao
          ? new Date(data.dataAdmissao).toISOString().split('T')[0]
          : '') as string,
        salarioBruto: data.salarioBruto,
        situacao: data.situacao as FuncionarioFormData['situacao'],
        empresaId: data.empresaId,
        banco: data.banco || '',
        agencia: data.agencia || '',
        conta: data.conta || '',
        tipoConta: data.tipoConta as FuncionarioFormData['tipoConta'],
      })
    }
  }, [funcionarioQuery.data, reset])

  // Mutations
  const createMutation = trpc.funcionarios.create.useMutation({
    onSuccess: () => {
      toast.success('Funcionario criado com sucesso')
      onSuccess()
    },
    onError: (error) => {
      toast.error(error.message || 'Erro ao criar funcionario')
    },
  })

  const updateMutation = trpc.funcionarios.update.useMutation({
    onSuccess: () => {
      toast.success('Funcionario atualizado com sucesso')
      onSuccess()
    },
    onError: (error) => {
      toast.error(error.message || 'Erro ao atualizar funcionario')
    },
  })

  const onSubmit = (data: FuncionarioFormData): void => {
    const payload = {
      ...data,
      dataNascimento: new Date(data.dataNascimento),
      dataAdmissao: new Date(data.dataAdmissao),
      email: data.email || null,
      banco: data.banco || null,
    }

    if (isEditing) {
      updateMutation.mutate({ id: funcionarioId!, ...payload })
    } else {
      createMutation.mutate(payload)
    }
  }

  const isPending = createMutation.isPending || updateMutation.isPending

  if (isEditing && funcionarioQuery.isLoading) {
    return (
      <div className="space-y-4">
        <Skeleton className="h-10 w-full" />
        <Skeleton className="h-10 w-full" />
        <Skeleton className="h-10 w-full" />
        <Skeleton className="h-10 w-full" />
      </div>
    )
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
      {/* Personal Information */}
      <div className="space-y-4">
        <h3 className="font-semibold text-lg">Dados Pessoais</h3>

        <div className="grid gap-4 sm:grid-cols-2">
          {/* CPF */}
          <div className="space-y-2">
            <Label htmlFor="cpf">CPF *</Label>
            <Input
              id="cpf"
              placeholder="000.000.000-00"
              aria-invalid={errors.cpf ? 'true' : 'false'}
              disabled={isEditing}
              {...register('cpf')}
            />
            {errors.cpf && (
              <p className="text-sm text-destructive" role="alert">
                {errors.cpf.message}
              </p>
            )}
          </div>

          {/* Nome */}
          <div className="space-y-2">
            <Label htmlFor="nome">Nome Completo *</Label>
            <Input
              id="nome"
              placeholder="Nome do funcionario"
              aria-invalid={errors.nome ? 'true' : 'false'}
              {...register('nome')}
            />
            {errors.nome && (
              <p className="text-sm text-destructive" role="alert">
                {errors.nome.message}
              </p>
            )}
          </div>

          {/* Data Nascimento */}
          <div className="space-y-2">
            <Label htmlFor="dataNascimento">Data de Nascimento *</Label>
            <Input
              id="dataNascimento"
              type="date"
              aria-invalid={errors.dataNascimento ? 'true' : 'false'}
              {...register('dataNascimento')}
            />
            {errors.dataNascimento && (
              <p className="text-sm text-destructive" role="alert">
                {errors.dataNascimento.message}
              </p>
            )}
          </div>

          {/* Sexo */}
          <div className="space-y-2">
            <Label htmlFor="sexo">Sexo</Label>
            <Controller
              name="sexo"
              control={control}
              render={({ field }) => (
                <Select value={field.value || ''} onValueChange={field.onChange}>
                  <SelectTrigger id="sexo">
                    <SelectValue placeholder="Selecione" />
                  </SelectTrigger>
                  <SelectContent>
                    {sexoOptions.map((option) => (
                      <SelectItem key={option.value} value={option.value}>
                        {option.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              )}
            />
          </div>

          {/* Email */}
          <div className="space-y-2">
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              placeholder="email@exemplo.com"
              aria-invalid={errors.email ? 'true' : 'false'}
              {...register('email')}
            />
            {errors.email && (
              <p className="text-sm text-destructive" role="alert">
                {errors.email.message}
              </p>
            )}
          </div>

          {/* Telefone */}
          <div className="space-y-2">
            <Label htmlFor="telefone">Telefone</Label>
            <Input
              id="telefone"
              placeholder="(00) 00000-0000"
              {...register('telefone')}
            />
          </div>
        </div>
      </div>

      {/* Employment Information */}
      <div className="space-y-4">
        <h3 className="font-semibold text-lg">Dados Funcionais</h3>

        <div className="grid gap-4 sm:grid-cols-2">
          {/* Matricula */}
          <div className="space-y-2">
            <Label htmlFor="matricula">Matricula *</Label>
            <Input
              id="matricula"
              placeholder="Numero da matricula"
              aria-invalid={errors.matricula ? 'true' : 'false'}
              {...register('matricula')}
            />
            {errors.matricula && (
              <p className="text-sm text-destructive" role="alert">
                {errors.matricula.message}
              </p>
            )}
          </div>

          {/* Cargo */}
          <div className="space-y-2">
            <Label htmlFor="cargo">Cargo</Label>
            <Input
              id="cargo"
              placeholder="Cargo do funcionario"
              {...register('cargo')}
            />
          </div>

          {/* Empresa */}
          <div className="space-y-2">
            <Label htmlFor="empresaId">Empresa *</Label>
            <Controller
              name="empresaId"
              control={control}
              render={({ field }) => (
                <Select
                  value={field.value?.toString()}
                  onValueChange={(value) => field.onChange(parseInt(value, 10))}
                >
                  <SelectTrigger id="empresaId" aria-invalid={errors.empresaId ? 'true' : 'false'}>
                    <SelectValue placeholder="Selecione a empresa" />
                  </SelectTrigger>
                  <SelectContent>
                    {empresasQuery.data?.map((empresa) => (
                      <SelectItem key={empresa.id} value={empresa.id.toString()}>
                        {empresa.nome}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              )}
            />
            {errors.empresaId && (
              <p className="text-sm text-destructive" role="alert">
                {errors.empresaId.message}
              </p>
            )}
          </div>

          {/* Data Admissao */}
          <div className="space-y-2">
            <Label htmlFor="dataAdmissao">Data de Admissao *</Label>
            <Input
              id="dataAdmissao"
              type="date"
              aria-invalid={errors.dataAdmissao ? 'true' : 'false'}
              {...register('dataAdmissao')}
            />
            {errors.dataAdmissao && (
              <p className="text-sm text-destructive" role="alert">
                {errors.dataAdmissao.message}
              </p>
            )}
          </div>

          {/* Salario */}
          <div className="space-y-2">
            <Label htmlFor="salarioBruto">Salario Bruto *</Label>
            <Input
              id="salarioBruto"
              type="number"
              step="0.01"
              min="0"
              placeholder="0.00"
              aria-invalid={errors.salarioBruto ? 'true' : 'false'}
              {...register('salarioBruto')}
            />
            {errors.salarioBruto && (
              <p className="text-sm text-destructive" role="alert">
                {errors.salarioBruto.message}
              </p>
            )}
          </div>

          {/* Situacao */}
          <div className="space-y-2">
            <Label htmlFor="situacao">Situacao *</Label>
            <Controller
              name="situacao"
              control={control}
              render={({ field }) => (
                <Select value={field.value} onValueChange={field.onChange}>
                  <SelectTrigger id="situacao">
                    <SelectValue placeholder="Selecione" />
                  </SelectTrigger>
                  <SelectContent>
                    {situacaoOptions.map((option) => (
                      <SelectItem key={option.value} value={option.value}>
                        {option.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              )}
            />
          </div>
        </div>
      </div>

      {/* Bank Information */}
      <div className="space-y-4">
        <h3 className="font-semibold text-lg">Dados Bancarios</h3>

        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
          {/* Banco */}
          <div className="space-y-2">
            <Label htmlFor="banco">Banco</Label>
            <Input
              id="banco"
              placeholder="000"
              maxLength={3}
              {...register('banco')}
            />
          </div>

          {/* Agencia */}
          <div className="space-y-2">
            <Label htmlFor="agencia">Agencia</Label>
            <Input
              id="agencia"
              placeholder="0000"
              {...register('agencia')}
            />
          </div>

          {/* Conta */}
          <div className="space-y-2">
            <Label htmlFor="conta">Conta</Label>
            <Input
              id="conta"
              placeholder="00000-0"
              {...register('conta')}
            />
          </div>

          {/* Tipo Conta */}
          <div className="space-y-2">
            <Label htmlFor="tipoConta">Tipo de Conta</Label>
            <Controller
              name="tipoConta"
              control={control}
              render={({ field }) => (
                <Select value={field.value || ''} onValueChange={field.onChange}>
                  <SelectTrigger id="tipoConta">
                    <SelectValue placeholder="Selecione" />
                  </SelectTrigger>
                  <SelectContent>
                    {tipoContaOptions.map((option) => (
                      <SelectItem key={option.value} value={option.value}>
                        {option.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              )}
            />
          </div>
        </div>
      </div>

      {/* Actions */}
      <div className="flex justify-end gap-3 pt-4 border-t">
        <Button type="button" variant="outline" onClick={onCancel} disabled={isPending}>
          Cancelar
        </Button>
        <Button type="submit" disabled={isPending}>
          {isPending && <Loader2 className="mr-2 h-4 w-4 animate-spin" aria-hidden="true" />}
          {isPending ? 'Salvando...' : isEditing ? 'Atualizar' : 'Criar'}
        </Button>
      </div>
    </form>
  )
}
