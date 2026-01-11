import { useState } from 'react'
import { trpc } from '@/lib/trpc'
import { useAuthStore } from '@/stores/auth'
import { Breadcrumb } from '@/components/layout/Breadcrumb'
import { AverbacaoTable } from '../components/AverbacaoTable'
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Input,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogFooter,
  Label,
} from '@fastconsig/ui'
import { Search, Filter, Plus } from 'lucide-react'
import { toast } from 'sonner'

type SituacaoAverbacao =
  | 'AGUARDANDO_APROVACAO'
  | 'APROVADA'
  | 'REJEITADA'
  | 'ENVIADA'
  | 'DESCONTADA'
  | 'SUSPENSA'
  | 'BLOQUEADA'
  | 'LIQUIDADA'
  | 'CANCELADA'
  | 'TODAS'

interface AverbacaoFilters {
  search: string
  situacao: SituacaoAverbacao
  page: number
  pageSize: number
  orderBy: 'numeroContrato' | 'dataContrato' | 'valorTotal' | 'createdAt'
  orderDir: 'asc' | 'desc'
}

const situacaoOptions: Array<{ value: SituacaoAverbacao; label: string }> = [
  { value: 'TODAS', label: 'Todas as situacoes' },
  { value: 'AGUARDANDO_APROVACAO', label: 'Aguardando Aprovacao' },
  { value: 'APROVADA', label: 'Aprovada' },
  { value: 'REJEITADA', label: 'Rejeitada' },
  { value: 'ENVIADA', label: 'Enviada' },
  { value: 'DESCONTADA', label: 'Descontada' },
  { value: 'SUSPENSA', label: 'Suspensa' },
  { value: 'BLOQUEADA', label: 'Bloqueada' },
  { value: 'LIQUIDADA', label: 'Liquidada' },
  { value: 'CANCELADA', label: 'Cancelada' },
]

export function AverbacoesPage(): JSX.Element {
  const { hasPermission } = useAuthStore()

  const [filters, setFilters] = useState<AverbacaoFilters>({
    search: '',
    situacao: 'TODAS',
    page: 1,
    pageSize: 20,
    orderBy: 'createdAt',
    orderDir: 'desc',
  })

  const [searchInput, setSearchInput] = useState('')
  const [rejectDialogOpen, setRejectDialogOpen] = useState(false)
  const [rejectingId, setRejectingId] = useState<number | null>(null)
  const [motivoRejeicao, setMotivoRejeicao] = useState('')

  // Permissions
  const canCreate = hasPermission('AVERBACOES_CRIAR')
  const canAprovar = hasPermission('AVERBACOES_APROVAR')

  // Query
  const averbacoesQuery = trpc.averbacoes.list.useQuery({
    page: filters.page,
    pageSize: filters.pageSize,
    search: filters.search || undefined,
    situacao: filters.situacao !== 'TODAS' ? filters.situacao : undefined,
    orderBy: filters.orderBy,
    orderDir: filters.orderDir,
  })

  // Mutations
  const aprovarMutation = trpc.averbacoes.aprovar.useMutation({
    onSuccess: () => {
      toast.success('Averbacao aprovada com sucesso')
      averbacoesQuery.refetch()
    },
    onError: (error) => {
      toast.error(error.message || 'Erro ao aprovar averbacao')
    },
  })

  const rejeitarMutation = trpc.averbacoes.rejeitar.useMutation({
    onSuccess: () => {
      toast.success('Averbacao rejeitada')
      setRejectDialogOpen(false)
      setRejectingId(null)
      setMotivoRejeicao('')
      averbacoesQuery.refetch()
    },
    onError: (error) => {
      toast.error(error.message || 'Erro ao rejeitar averbacao')
    },
  })

  // Handlers
  const handleSearch = (): void => {
    setFilters((prev) => ({ ...prev, search: searchInput, page: 1 }))
  }

  const handleSearchKeyDown = (e: React.KeyboardEvent): void => {
    if (e.key === 'Enter') {
      handleSearch()
    }
  }

  const handleSituacaoChange = (value: string): void => {
    setFilters((prev) => ({
      ...prev,
      situacao: value as SituacaoAverbacao,
      page: 1,
    }))
  }

  const handlePageChange = (page: number): void => {
    setFilters((prev) => ({ ...prev, page }))
  }

  const handleAprovar = (id: number): void => {
    if (window.confirm('Tem certeza que deseja aprovar esta averbacao?')) {
      aprovarMutation.mutate({ id })
    }
  }

  const handleRejeitar = (id: number): void => {
    setRejectingId(id)
    setRejectDialogOpen(true)
  }

  const handleConfirmRejeitar = (): void => {
    if (rejectingId && motivoRejeicao.length >= 10) {
      rejeitarMutation.mutate({ id: rejectingId, motivoRejeicao })
    }
  }

  const handleCreate = (): void => {
    toast.info('Formulario de nova averbacao em desenvolvimento')
  }

  // Stats query
  const resumoQuery = trpc.averbacoes.getResumo.useQuery()

  return (
    <div className="space-y-6">
      {/* Breadcrumb */}
      <Breadcrumb items={[{ label: 'Averbacoes' }]} />

      {/* Page Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Averbacoes</h1>
          <p className="text-muted-foreground">
            Gerencie as averbacoes de emprestimos consignados
          </p>
        </div>

        {canCreate && (
          <Button onClick={handleCreate}>
            <Plus className="mr-2 h-4 w-4" aria-hidden="true" />
            Nova Averbacao
          </Button>
        )}
      </div>

      {/* Stats Cards */}
      {resumoQuery.data && (
        <div className="grid gap-4 md:grid-cols-4">
          <Card>
            <CardHeader className="pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                Aguardando Aprovacao
              </CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-2xl font-bold text-yellow-600">
                {resumoQuery.data.aguardandoAprovacao}
              </p>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                Aprovadas
              </CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-2xl font-bold text-green-600">
                {resumoQuery.data.aprovadas}
              </p>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                Em Desconto
              </CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-2xl font-bold text-indigo-600">
                {resumoQuery.data.descontadas}
              </p>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                Total Contratado
              </CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-2xl font-bold">
                {new Intl.NumberFormat('pt-BR', {
                  style: 'currency',
                  currency: 'BRL',
                  notation: 'compact',
                }).format(resumoQuery.data.valorTotalContratado)}
              </p>
            </CardContent>
          </Card>
        </div>
      )}

      {/* Filters */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2 text-lg">
            <Filter className="h-5 w-5" aria-hidden="true" />
            Filtros
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="flex flex-col gap-4 sm:flex-row">
            {/* Search */}
            <div className="flex flex-1 gap-2">
              <div className="relative flex-1">
                <Search
                  className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground"
                  aria-hidden="true"
                />
                <Input
                  type="search"
                  placeholder="Buscar por contrato, funcionario ou CPF..."
                  className="pl-10"
                  value={searchInput}
                  onChange={(e) => setSearchInput(e.target.value)}
                  onKeyDown={handleSearchKeyDown}
                  aria-label="Buscar averbacoes"
                />
              </div>
              <Button onClick={handleSearch} variant="secondary">
                Buscar
              </Button>
            </div>

            {/* Situacao filter */}
            <div className="w-full sm:w-56">
              <Select value={filters.situacao} onValueChange={handleSituacaoChange}>
                <SelectTrigger aria-label="Filtrar por situacao">
                  <SelectValue placeholder="Situacao" />
                </SelectTrigger>
                <SelectContent>
                  {situacaoOptions.map((option) => (
                    <SelectItem key={option.value} value={option.value}>
                      {option.label}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Table */}
      <Card>
        <CardContent className="p-0">
          <AverbacaoTable
            data={averbacoesQuery.data?.data ?? []}
            isLoading={averbacoesQuery.isLoading}
            onAprovar={handleAprovar}
            onRejeitar={handleRejeitar}
            canAprovar={canAprovar}
          />

          {/* Pagination */}
          {averbacoesQuery.data && averbacoesQuery.data.pagination.totalPages > 1 && (
            <div className="flex items-center justify-between border-t px-4 py-3">
              <p className="text-sm text-muted-foreground">
                Pagina {averbacoesQuery.data.pagination.page} de{' '}
                {averbacoesQuery.data.pagination.totalPages} ({averbacoesQuery.data.pagination.total} registros)
              </p>
              <div className="flex gap-2">
                <Button
                  variant="outline"
                  size="sm"
                  disabled={filters.page <= 1}
                  onClick={() => handlePageChange(filters.page - 1)}
                >
                  Anterior
                </Button>
                <Button
                  variant="outline"
                  size="sm"
                  disabled={filters.page >= averbacoesQuery.data.pagination.totalPages}
                  onClick={() => handlePageChange(filters.page + 1)}
                >
                  Proxima
                </Button>
              </div>
            </div>
          )}
        </CardContent>
      </Card>

      {/* Reject Dialog */}
      <Dialog open={rejectDialogOpen} onOpenChange={setRejectDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Rejeitar Averbacao</DialogTitle>
          </DialogHeader>
          <div className="space-y-4 py-4">
            <div className="space-y-2">
              <Label htmlFor="motivoRejeicao">Motivo da Rejeicao *</Label>
              <textarea
                id="motivoRejeicao"
                className="w-full min-h-[100px] rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring"
                placeholder="Informe o motivo da rejeicao (minimo 10 caracteres)"
                value={motivoRejeicao}
                onChange={(e) => setMotivoRejeicao(e.target.value)}
              />
              {motivoRejeicao.length > 0 && motivoRejeicao.length < 10 && (
                <p className="text-sm text-destructive">
                  Motivo deve ter no minimo 10 caracteres
                </p>
              )}
            </div>
          </div>
          <DialogFooter>
            <Button
              variant="outline"
              onClick={() => {
                setRejectDialogOpen(false)
                setMotivoRejeicao('')
              }}
            >
              Cancelar
            </Button>
            <Button
              variant="destructive"
              onClick={handleConfirmRejeitar}
              disabled={motivoRejeicao.length < 10 || rejeitarMutation.isPending}
            >
              {rejeitarMutation.isPending ? 'Rejeitando...' : 'Confirmar Rejeicao'}
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  )
}
