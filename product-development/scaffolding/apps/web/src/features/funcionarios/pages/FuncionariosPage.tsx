import { useState } from 'react'
import { useNavigate } from '@tanstack/react-router'
import { trpc } from '@/lib/trpc'
import { useAuthStore } from '@/stores/auth'
import { Breadcrumb } from '@/components/layout/Breadcrumb'
import { FuncionarioTable } from '../components/FuncionarioTable'
import { FuncionarioForm } from '../components/FuncionarioForm'
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
  Skeleton,
} from '@fastconsig/ui'
import { Plus, Search, Filter, Download, Upload } from 'lucide-react'
import { toast } from 'sonner'

type FuncionarioSituacao = 'ATIVO' | 'INATIVO' | 'AFASTADO' | 'BLOQUEADO' | 'APOSENTADO'

interface FuncionarioFilters {
  search: string
  situacao: FuncionarioSituacao | 'TODOS'
  page: number
  pageSize: number
  orderBy: 'nome' | 'matricula' | 'cpf' | 'createdAt'
  orderDir: 'asc' | 'desc'
}

const situacaoOptions: Array<{ value: FuncionarioSituacao | 'TODOS'; label: string }> = [
  { value: 'TODOS', label: 'Todas as situacoes' },
  { value: 'ATIVO', label: 'Ativo' },
  { value: 'INATIVO', label: 'Inativo' },
  { value: 'AFASTADO', label: 'Afastado' },
  { value: 'BLOQUEADO', label: 'Bloqueado' },
  { value: 'APOSENTADO', label: 'Aposentado' },
]

export function FuncionariosPage(): JSX.Element {
  const navigate = useNavigate()
  const { hasPermission } = useAuthStore()

  const [filters, setFilters] = useState<FuncionarioFilters>({
    search: '',
    situacao: 'TODOS',
    page: 1,
    pageSize: 20,
    orderBy: 'nome',
    orderDir: 'asc',
  })

  const [isFormOpen, setIsFormOpen] = useState(false)
  const [editingFuncionarioId, setEditingFuncionarioId] = useState<number | null>(null)
  const [searchInput, setSearchInput] = useState('')

  // Permissions
  const canCreate = hasPermission('FUNCIONARIOS_CRIAR')
  const canEdit = hasPermission('FUNCIONARIOS_EDITAR')
  const canDelete = hasPermission('FUNCIONARIOS_EXCLUIR')
  const canExport = hasPermission('FUNCIONARIOS_EXPORTAR')
  const canImport = hasPermission('FUNCIONARIOS_IMPORTAR')

  // Query
  const funcionariosQuery = trpc.funcionarios.list.useQuery({
    page: filters.page,
    pageSize: filters.pageSize,
    search: filters.search || undefined,
    situacao: filters.situacao !== 'TODOS' ? filters.situacao : undefined,
    orderBy: filters.orderBy,
    orderDir: filters.orderDir,
  })

  // Mutations
  const deleteMutation = trpc.funcionarios.delete.useMutation({
    onSuccess: () => {
      toast.success('Funcionario excluido com sucesso')
      funcionariosQuery.refetch()
    },
    onError: (error) => {
      toast.error(error.message || 'Erro ao excluir funcionario')
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
      situacao: value as FuncionarioSituacao | 'TODOS',
      page: 1,
    }))
  }

  const handlePageChange = (page: number): void => {
    setFilters((prev) => ({ ...prev, page }))
  }

  const handleSort = (column: string, direction: 'asc' | 'desc'): void => {
    setFilters((prev) => ({
      ...prev,
      orderBy: column as FuncionarioFilters['orderBy'],
      orderDir: direction,
    }))
  }

  const handleCreate = (): void => {
    setEditingFuncionarioId(null)
    setIsFormOpen(true)
  }

  const handleEdit = (id: number): void => {
    setEditingFuncionarioId(id)
    setIsFormOpen(true)
  }

  const handleDelete = async (id: number): Promise<void> => {
    if (window.confirm('Tem certeza que deseja excluir este funcionario?')) {
      deleteMutation.mutate({ id })
    }
  }

  const handleFormSuccess = (): void => {
    setIsFormOpen(false)
    setEditingFuncionarioId(null)
    funcionariosQuery.refetch()
  }

  const handleFormClose = (): void => {
    setIsFormOpen(false)
    setEditingFuncionarioId(null)
  }

  const handleExport = (): void => {
    toast.info('Funcionalidade de exportacao em desenvolvimento')
  }

  const handleImport = (): void => {
    navigate({ to: '/importacao' })
  }

  return (
    <div className="space-y-6">
      {/* Breadcrumb */}
      <Breadcrumb items={[{ label: 'Funcionarios' }]} />

      {/* Page Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Funcionarios</h1>
          <p className="text-muted-foreground">
            Gerencie os funcionarios cadastrados no sistema
          </p>
        </div>

        <div className="flex flex-wrap gap-2">
          {canImport && (
            <Button variant="outline" onClick={handleImport}>
              <Upload className="mr-2 h-4 w-4" aria-hidden="true" />
              Importar
            </Button>
          )}
          {canExport && (
            <Button variant="outline" onClick={handleExport}>
              <Download className="mr-2 h-4 w-4" aria-hidden="true" />
              Exportar
            </Button>
          )}
          {canCreate && (
            <Button onClick={handleCreate}>
              <Plus className="mr-2 h-4 w-4" aria-hidden="true" />
              Novo Funcionario
            </Button>
          )}
        </div>
      </div>

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
                  placeholder="Buscar por nome, CPF ou matricula..."
                  className="pl-10"
                  value={searchInput}
                  onChange={(e) => setSearchInput(e.target.value)}
                  onKeyDown={handleSearchKeyDown}
                  aria-label="Buscar funcionarios"
                />
              </div>
              <Button onClick={handleSearch} variant="secondary">
                Buscar
              </Button>
            </div>

            {/* Situacao filter */}
            <div className="w-full sm:w-48">
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
          {funcionariosQuery.isLoading ? (
            <div className="space-y-4 p-6">
              <Skeleton className="h-10 w-full" />
              <Skeleton className="h-10 w-full" />
              <Skeleton className="h-10 w-full" />
              <Skeleton className="h-10 w-full" />
              <Skeleton className="h-10 w-full" />
            </div>
          ) : funcionariosQuery.isError ? (
            <div className="p-6 text-center text-destructive">
              Erro ao carregar funcionarios: {funcionariosQuery.error.message}
            </div>
          ) : (
            <FuncionarioTable
              data={funcionariosQuery.data?.items ?? []}
              totalCount={funcionariosQuery.data?.total ?? 0}
              page={filters.page}
              pageSize={filters.pageSize}
              onPageChange={handlePageChange}
              onSort={handleSort}
              onEdit={canEdit ? handleEdit : undefined}
              onDelete={canDelete ? handleDelete : undefined}
            />
          )}
        </CardContent>
      </Card>

      {/* Form Dialog */}
      <Dialog open={isFormOpen} onOpenChange={setIsFormOpen}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>
              {editingFuncionarioId ? 'Editar Funcionario' : 'Novo Funcionario'}
            </DialogTitle>
          </DialogHeader>
          <FuncionarioForm
            funcionarioId={editingFuncionarioId}
            onSuccess={handleFormSuccess}
            onCancel={handleFormClose}
          />
        </DialogContent>
      </Dialog>
    </div>
  )
}
