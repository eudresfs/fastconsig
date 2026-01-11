import { useParams, useNavigate } from '@tanstack/react-router'
import { trpc } from '@/lib/trpc'
import { useAuthStore } from '@/stores/auth'
import { Breadcrumb } from '@/components/layout/Breadcrumb'
import { SituacaoBadge } from '../components/SituacaoBadge'
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  CardDescription,
  Skeleton,
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
  Label,
} from '@fastconsig/ui'
import {
  ArrowLeft,
  Check,
  X,
  Pause,
  Ban,
  User,
  Building2,
  FileText,
  Calendar,
  DollarSign,
  History,
} from 'lucide-react'
import { toast } from 'sonner'
import { useState } from 'react'

function formatCurrency(value: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(value)
}

function formatDate(date: string | Date): string {
  return new Intl.DateTimeFormat('pt-BR').format(new Date(date))
}

function formatDateTime(date: string | Date): string {
  return new Intl.DateTimeFormat('pt-BR', {
    dateStyle: 'short',
    timeStyle: 'short',
  }).format(new Date(date))
}

function formatCpf(cpf: string): string {
  return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4')
}

function formatPercent(value: number): string {
  return `${value.toFixed(2)}%`
}

export function AverbacaoDetailPage(): JSX.Element {
  const { id } = useParams({ from: '/_authenticated/averbacoes_/$id' })
  const navigate = useNavigate()
  const { hasPermission } = useAuthStore()

  const [actionDialogOpen, setActionDialogOpen] = useState(false)
  const [actionType, setActionType] = useState<'rejeitar' | 'suspender' | 'cancelar' | 'bloquear' | null>(null)
  const [actionText, setActionText] = useState('')

  const canAprovar = hasPermission('AVERBACOES_APROVAR')

  // Query
  const averbacaoQuery = trpc.averbacoes.getById.useQuery({ id: Number(id) })
  const historicoQuery = trpc.averbacoes.getHistorico.useQuery({ averbacaoId: Number(id) })

  // Mutations
  const aprovarMutation = trpc.averbacoes.aprovar.useMutation({
    onSuccess: () => {
      toast.success('Averbacao aprovada com sucesso')
      averbacaoQuery.refetch()
      historicoQuery.refetch()
    },
    onError: (error: any) => {
      toast.error(error.message || 'Erro ao aprovar averbacao')
    },
  })

  const rejeitarMutation = trpc.averbacoes.rejeitar.useMutation({
    onSuccess: () => {
      toast.success('Averbacao rejeitada')
      closeActionDialog()
      averbacaoQuery.refetch()
      historicoQuery.refetch()
    },
    onError: (error: any) => {
      toast.error(error.message || 'Erro ao rejeitar averbacao')
    },
  })

  const suspenderMutation = trpc.averbacoes.suspender.useMutation({
    onSuccess: () => {
      toast.success('Averbacao suspensa')
      closeActionDialog()
      averbacaoQuery.refetch()
      historicoQuery.refetch()
    },
    onError: (error: any) => {
      toast.error(error.message || 'Erro ao suspender averbacao')
    },
  })

  const cancelarMutation = trpc.averbacoes.cancelar.useMutation({
    onSuccess: () => {
      toast.success('Averbacao cancelada')
      closeActionDialog()
      averbacaoQuery.refetch()
      historicoQuery.refetch()
    },
    onError: (error: any) => {
      toast.error(error.message || 'Erro ao cancelar averbacao')
    },
  })

  const bloquearMutation = trpc.averbacoes.bloquear.useMutation({
    onSuccess: () => {
      toast.success('Averbacao bloqueada')
      closeActionDialog()
      averbacaoQuery.refetch()
      historicoQuery.refetch()
    },
    onError: (error: any) => {
      toast.error(error.message || 'Erro ao bloquear averbacao')
    },
  })

  const openActionDialog = (type: 'rejeitar' | 'suspender' | 'cancelar' | 'bloquear'): void => {
    setActionType(type)
    setActionText('')
    setActionDialogOpen(true)
  }

  const closeActionDialog = (): void => {
    setActionDialogOpen(false)
    setActionType(null)
    setActionText('')
  }

  const handleAprovar = (): void => {
    if (window.confirm('Tem certeza que deseja aprovar esta averbacao?')) {
      aprovarMutation.mutate({ id: Number(id) })
    }
  }

  const handleConfirmAction = (): void => {
    if (!actionType || actionText.length < 10) return

    switch (actionType) {
      case 'rejeitar':
        rejeitarMutation.mutate({ id: Number(id), motivoRejeicao: actionText })
        break
      case 'suspender':
        suspenderMutation.mutate({ id: Number(id), observacao: actionText })
        break
      case 'cancelar':
        cancelarMutation.mutate({ id: Number(id), observacao: actionText })
        break
      case 'bloquear':
        bloquearMutation.mutate({ id: Number(id), observacao: actionText })
        break
    }
  }

  const getActionLabel = (): { title: string; placeholder: string; button: string } => {
    switch (actionType) {
      case 'rejeitar':
        return {
          title: 'Rejeitar Averbacao',
          placeholder: 'Informe o motivo da rejeicao',
          button: 'Confirmar Rejeicao',
        }
      case 'suspender':
        return {
          title: 'Suspender Averbacao',
          placeholder: 'Informe o motivo da suspensao',
          button: 'Confirmar Suspensao',
        }
      case 'cancelar':
        return {
          title: 'Cancelar Averbacao',
          placeholder: 'Informe o motivo do cancelamento',
          button: 'Confirmar Cancelamento',
        }
      case 'bloquear':
        return {
          title: 'Bloquear Averbacao',
          placeholder: 'Informe o motivo do bloqueio',
          button: 'Confirmar Bloqueio',
        }
      default:
        return { title: '', placeholder: '', button: '' }
    }
  }

  const isPending =
    aprovarMutation.isPending ||
    rejeitarMutation.isPending ||
    suspenderMutation.isPending ||
    cancelarMutation.isPending ||
    bloquearMutation.isPending

  if (averbacaoQuery.isLoading) {
    return (
      <div className="space-y-6">
        <Skeleton className="h-8 w-64" />
        <div className="grid gap-6 md:grid-cols-2">
          <Skeleton className="h-64" />
          <Skeleton className="h-64" />
        </div>
      </div>
    )
  }

  if (averbacaoQuery.isError || !averbacaoQuery.data) {
    return (
      <div className="space-y-6">
        <Button variant="ghost" onClick={() => navigate({ to: '/averbacoes' })}>
          <ArrowLeft className="mr-2 h-4 w-4" />
          Voltar
        </Button>
        <Card>
          <CardContent className="py-8 text-center text-destructive">
            Averbacao nao encontrada
          </CardContent>
        </Card>
      </div>
    )
  }

  const averbacao = averbacaoQuery.data
  const canPerformActions = canAprovar && averbacao.situacao !== 'LIQUIDADA' && averbacao.situacao !== 'CANCELADA' && averbacao.situacao !== 'REJEITADA'

  return (
    <div className="space-y-6">
      {/* Breadcrumb */}
      <Breadcrumb
        items={[
          { label: 'Averbacoes', href: '/averbacoes' },
          { label: averbacao.numeroContrato },
        ]}
      />

      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div className="flex items-center gap-4">
          <Button variant="ghost" size="icon" onClick={() => navigate({ to: '/averbacoes' })}>
            <ArrowLeft className="h-5 w-5" />
          </Button>
          <div>
            <div className="flex items-center gap-3">
              <h1 className="text-2xl font-bold">{averbacao.numeroContrato}</h1>
              <SituacaoBadge situacao={averbacao.situacao} />
            </div>
            <p className="text-muted-foreground">
              Contrato criado em {formatDateTime(averbacao.createdAt)}
            </p>
          </div>
        </div>

        {/* Actions */}
        {canPerformActions && (
          <div className="flex flex-wrap gap-2">
            {averbacao.situacao === 'AGUARDANDO_APROVACAO' && (
              <>
                <Button onClick={handleAprovar} disabled={isPending}>
                  <Check className="mr-2 h-4 w-4" />
                  Aprovar
                </Button>
                <Button variant="destructive" onClick={() => openActionDialog('rejeitar')} disabled={isPending}>
                  <X className="mr-2 h-4 w-4" />
                  Rejeitar
                </Button>
              </>
            )}
            {['APROVADA', 'ENVIADA', 'DESCONTADA'].includes(averbacao.situacao) && (
              <>
                <Button variant="secondary" onClick={() => openActionDialog('suspender')} disabled={isPending}>
                  <Pause className="mr-2 h-4 w-4" />
                  Suspender
                </Button>
                <Button variant="secondary" onClick={() => openActionDialog('bloquear')} disabled={isPending}>
                  <Ban className="mr-2 h-4 w-4" />
                  Bloquear
                </Button>
                <Button variant="destructive" onClick={() => openActionDialog('cancelar')} disabled={isPending}>
                  <X className="mr-2 h-4 w-4" />
                  Cancelar
                </Button>
              </>
            )}
          </div>
        )}
      </div>

      {/* Content */}
      <div className="grid gap-6 lg:grid-cols-2">
        {/* Funcionario */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <User className="h-5 w-5" />
              Funcionario
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-3">
            <div>
              <p className="text-sm text-muted-foreground">Nome</p>
              <p className="font-medium">{averbacao.funcionario.nome}</p>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <p className="text-sm text-muted-foreground">CPF</p>
                <p className="font-medium">{formatCpf(averbacao.funcionario.cpf)}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Matricula</p>
                <p className="font-medium">{averbacao.funcionario.matricula}</p>
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Consignataria e Produto */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Building2 className="h-5 w-5" />
              Consignataria e Produto
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-3">
            <div>
              <p className="text-sm text-muted-foreground">Consignataria</p>
              <p className="font-medium">{averbacao.tenantConsignataria.consignataria.razaoSocial}</p>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <p className="text-sm text-muted-foreground">Produto</p>
                <p className="font-medium">{averbacao.produto.nome}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Tipo</p>
                <p className="font-medium">{averbacao.produto.tipo}</p>
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Valores */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <DollarSign className="h-5 w-5" />
              Valores
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <p className="text-sm text-muted-foreground">Valor Total</p>
                <p className="text-xl font-bold">{formatCurrency(averbacao.valorTotal)}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Valor Parcela</p>
                <p className="text-xl font-bold">{formatCurrency(averbacao.valorParcela)}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Parcelas</p>
                <p className="font-medium">
                  {averbacao.parcelasPagas} / {averbacao.parcelasTotal}
                </p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Saldo Devedor</p>
                <p className="font-medium">{formatCurrency(averbacao.saldoDevedor ?? 0)}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Taxa Mensal</p>
                <p className="font-medium">{formatPercent(averbacao.taxaMensal)}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Taxa Anual</p>
                <p className="font-medium">{formatPercent(averbacao.taxaAnual ?? 0)}</p>
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Datas */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Calendar className="h-5 w-5" />
              Datas
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <p className="text-sm text-muted-foreground">Data do Contrato</p>
                <p className="font-medium">{formatDate(averbacao.dataContrato)}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Inicio Desconto</p>
                <p className="font-medium">{formatDate(averbacao.dataInicioDesconto)}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Fim Desconto</p>
                <p className="font-medium">{formatDate(averbacao.dataFimDesconto)}</p>
              </div>
              {averbacao.dataAprovacao && (
                <div>
                  <p className="text-sm text-muted-foreground">Data Aprovacao</p>
                  <p className="font-medium">{formatDateTime(averbacao.dataAprovacao)}</p>
                </div>
              )}
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Observacoes */}
      {(averbacao.observacao || averbacao.motivoRejeicao) && (
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <FileText className="h-5 w-5" />
              Observacoes
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            {averbacao.observacao && (
              <div>
                <p className="text-sm text-muted-foreground">Observacao</p>
                <p>{averbacao.observacao}</p>
              </div>
            )}
            {averbacao.motivoRejeicao && (
              <div>
                <p className="text-sm text-muted-foreground text-destructive">Motivo da Rejeicao</p>
                <p className="text-destructive">{averbacao.motivoRejeicao}</p>
              </div>
            )}
          </CardContent>
        </Card>
      )}

      {/* Historico */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <History className="h-5 w-5" />
            Historico
          </CardTitle>
          <CardDescription>Registro de alteracoes de situacao</CardDescription>
        </CardHeader>
        <CardContent>
          {historicoQuery.isLoading ? (
            <div className="space-y-4">
              <Skeleton className="h-12 w-full" />
              <Skeleton className="h-12 w-full" />
            </div>
          ) : historicoQuery.data && historicoQuery.data.length > 0 ? (
            <div className="space-y-4">
              {historicoQuery.data.map((item, index) => (
                <div
                  key={item.id}
                  className="flex gap-4 pb-4 border-b last:border-0 last:pb-0"
                >
                  <div className="flex h-8 w-8 items-center justify-center rounded-full bg-primary/10 text-primary text-sm font-medium">
                    {historicoQuery.data!.length - index}
                  </div>
                  <div className="flex-1">
                    <div className="flex items-center gap-2">
                      {item.situacaoAnterior && (
                        <>
                          <SituacaoBadge situacao={item.situacaoAnterior} />
                          <span className="text-muted-foreground">→</span>
                        </>
                      )}
                      <SituacaoBadge situacao={item.situacaoNova} />
                    </div>
                    <p className="mt-1 text-sm text-muted-foreground">
                      {item.usuario?.nome ?? 'Sistema'} em {formatDateTime(item.createdAt)}
                    </p>
                    {item.observacao && (
                      <p className="mt-1 text-sm">{item.observacao}</p>
                    )}
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <p className="text-muted-foreground">Nenhum historico disponivel</p>
          )}
        </CardContent>
      </Card>

      {/* Action Dialog */}
      <Dialog open={actionDialogOpen} onOpenChange={setActionDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>{getActionLabel().title}</DialogTitle>
          </DialogHeader>
          <div className="space-y-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="actionText">
                {actionType === 'rejeitar' ? 'Motivo *' : 'Observacao *'}
              </Label>
              <textarea
                id="actionText"
                className="w-full min-h-[100px] rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring"
                placeholder={getActionLabel().placeholder}
                value={actionText}
                onChange={(e) => setActionText(e.target.value)}
              />
              {actionText.length > 0 && actionText.length < 10 && (
                <p className="text-sm text-destructive">
                  Texto deve ter no minimo 10 caracteres
                </p>
              )}
            </div>
          </div>
          <DialogFooter>
            <Button variant="outline" onClick={closeActionDialog}>
              Cancelar
            </Button>
            <Button
              variant={actionType === 'rejeitar' || actionType === 'cancelar' ? 'destructive' : 'default'}
              onClick={handleConfirmAction}
              disabled={actionText.length < 10 || isPending}
            >
              {isPending ? 'Processando...' : getActionLabel().button}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}
