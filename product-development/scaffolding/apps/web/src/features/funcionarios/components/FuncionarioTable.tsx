import { useMemo } from 'react'
import {
  useReactTable,
  getCoreRowModel,
  flexRender,
  type ColumnDef,
} from '@tanstack/react-table'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
  Button,
  Badge,
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@fastconsig/ui'
import { formatCPF, formatCurrency, formatDate } from '@/lib/utils'
import { MoreHorizontal, Pencil, Trash2, Eye, ArrowUpDown, ChevronLeft, ChevronRight } from 'lucide-react'

type FuncionarioSituacao = 'ATIVO' | 'INATIVO' | 'AFASTADO' | 'BLOQUEADO' | 'APOSENTADO'

interface Funcionario {
  id: number
  cpf: string
  nome: string
  matricula: string
  cargo: string | null
  salarioBruto: number
  situacao: FuncionarioSituacao
  dataAdmissao: Date | string
  empresa?: {
    id: number
    razaoSocial: string
  }
}

interface FuncionarioTableProps {
  data: Funcionario[]
  totalCount: number
  page: number
  pageSize: number
  onPageChange: (page: number) => void
  onSort: (column: string, direction: 'asc' | 'desc') => void
  onEdit?: (id: number) => void
  onDelete?: (id: number) => void
  onView?: (id: number) => void
}

const situacaoBadgeVariant: Record<FuncionarioSituacao, 'default' | 'secondary' | 'destructive' | 'outline'> = {
  ATIVO: 'default',
  INATIVO: 'secondary',
  AFASTADO: 'outline',
  BLOQUEADO: 'destructive',
  APOSENTADO: 'secondary',
}

const situacaoLabels: Record<FuncionarioSituacao, string> = {
  ATIVO: 'Ativo',
  INATIVO: 'Inativo',
  AFASTADO: 'Afastado',
  BLOQUEADO: 'Bloqueado',
  APOSENTADO: 'Aposentado',
}

export function FuncionarioTable({
  data,
  totalCount,
  page,
  pageSize,
  onPageChange,
  onSort,
  onEdit,
  onDelete,
  onView,
}: FuncionarioTableProps): JSX.Element {
  const totalPages = Math.ceil(totalCount / pageSize)
  const hasActions = onEdit || onDelete || onView

  const columns = useMemo<ColumnDef<Funcionario>[]>(
    () => [
      {
        accessorKey: 'matricula',
        header: ({ column }) => (
          <Button
            variant="ghost"
            onClick={() => onSort('matricula', column.getIsSorted() === 'asc' ? 'desc' : 'asc')}
            className="-ml-4"
          >
            Matricula
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        ),
        cell: ({ row }) => (
          <span className="font-mono text-sm">{row.getValue('matricula')}</span>
        ),
      },
      {
        accessorKey: 'cpf',
        header: ({ column }) => (
          <Button
            variant="ghost"
            onClick={() => onSort('cpf', column.getIsSorted() === 'asc' ? 'desc' : 'asc')}
            className="-ml-4"
          >
            CPF
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        ),
        cell: ({ row }) => (
          <span className="font-mono text-sm">{formatCPF(row.getValue('cpf'))}</span>
        ),
      },
      {
        accessorKey: 'nome',
        header: ({ column }) => (
          <Button
            variant="ghost"
            onClick={() => onSort('nome', column.getIsSorted() === 'asc' ? 'desc' : 'asc')}
            className="-ml-4"
          >
            Nome
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        ),
        cell: ({ row }) => (
          <div className="max-w-[200px] truncate font-medium" title={row.getValue('nome')}>
            {row.getValue('nome')}
          </div>
        ),
      },
      {
        accessorKey: 'cargo',
        header: 'Cargo',
        cell: ({ row }) => (
          <span className="text-muted-foreground">
            {row.getValue('cargo') || '-'}
          </span>
        ),
      },
      {
        accessorKey: 'salarioBruto',
        header: 'Salario',
        cell: ({ row }) => (
          <span className="font-mono">
            {formatCurrency(row.getValue('salarioBruto'))}
          </span>
        ),
      },
      {
        accessorKey: 'situacao',
        header: 'Situacao',
        cell: ({ row }) => {
          const situacao = row.getValue('situacao') as FuncionarioSituacao
          return (
            <Badge variant={situacaoBadgeVariant[situacao]}>
              {situacaoLabels[situacao]}
            </Badge>
          )
        },
      },
      {
        accessorKey: 'dataAdmissao',
        header: 'Admissao',
        cell: ({ row }) => (
          <span className="text-sm text-muted-foreground">
            {formatDate(row.getValue('dataAdmissao'))}
          </span>
        ),
      },
      ...(hasActions
        ? [
            {
              id: 'actions',
              header: () => <span className="sr-only">Acoes</span>,
              cell: ({ row }: { row: { original: Funcionario } }) => {
                const funcionario = row.original

                return (
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button
                        variant="ghost"
                        size="icon"
                        aria-label={`Acoes para ${funcionario.nome}`}
                      >
                        <MoreHorizontal className="h-4 w-4" />
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                      {onView && (
                        <DropdownMenuItem onClick={() => onView(funcionario.id)}>
                          <Eye className="mr-2 h-4 w-4" />
                          Visualizar
                        </DropdownMenuItem>
                      )}
                      {onEdit && (
                        <DropdownMenuItem onClick={() => onEdit(funcionario.id)}>
                          <Pencil className="mr-2 h-4 w-4" />
                          Editar
                        </DropdownMenuItem>
                      )}
                      {onDelete && (
                        <DropdownMenuItem
                          onClick={() => onDelete(funcionario.id)}
                          className="text-destructive focus:text-destructive"
                        >
                          <Trash2 className="mr-2 h-4 w-4" />
                          Excluir
                        </DropdownMenuItem>
                      )}
                    </DropdownMenuContent>
                  </DropdownMenu>
                )
              },
            } as ColumnDef<Funcionario>,
          ]
        : []),
    ],
    [onSort, onEdit, onDelete, onView, hasActions]
  )

  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
    manualPagination: true,
    manualSorting: true,
    pageCount: totalPages,
  })

  return (
    <div>
      {/* Table */}
      <div className="overflow-x-auto">
        <Table>
          <TableHeader>
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <TableHead key={header.id}>
                    {header.isPlaceholder
                      ? null
                      : flexRender(header.column.columnDef.header, header.getContext())}
                  </TableHead>
                ))}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {table.getRowModel().rows.length === 0 ? (
              <TableRow>
                <TableCell colSpan={columns.length} className="h-24 text-center">
                  Nenhum funcionario encontrado
                </TableCell>
              </TableRow>
            ) : (
              table.getRowModel().rows.map((row) => (
                <TableRow key={row.id} data-state={row.getIsSelected() && 'selected'}>
                  {row.getVisibleCells().map((cell) => (
                    <TableCell key={cell.id}>
                      {flexRender(cell.column.columnDef.cell, cell.getContext())}
                    </TableCell>
                  ))}
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </div>

      {/* Pagination */}
      <div className="flex items-center justify-between border-t px-4 py-4">
        <div className="text-sm text-muted-foreground">
          Mostrando {Math.min((page - 1) * pageSize + 1, totalCount)} a{' '}
          {Math.min(page * pageSize, totalCount)} de {totalCount} registros
        </div>

        <div className="flex items-center gap-2">
          <Button
            variant="outline"
            size="sm"
            onClick={() => onPageChange(page - 1)}
            disabled={page <= 1}
            aria-label="Pagina anterior"
          >
            <ChevronLeft className="h-4 w-4" />
            Anterior
          </Button>

          <div className="flex items-center gap-1">
            {Array.from({ length: Math.min(5, totalPages) }, (_, i) => {
              let pageNum: number
              if (totalPages <= 5) {
                pageNum = i + 1
              } else if (page <= 3) {
                pageNum = i + 1
              } else if (page >= totalPages - 2) {
                pageNum = totalPages - 4 + i
              } else {
                pageNum = page - 2 + i
              }

              return (
                <Button
                  key={pageNum}
                  variant={pageNum === page ? 'default' : 'outline'}
                  size="sm"
                  onClick={() => onPageChange(pageNum)}
                  aria-label={`Pagina ${pageNum}`}
                  aria-current={pageNum === page ? 'page' : undefined}
                >
                  {pageNum}
                </Button>
              )
            })}
          </div>

          <Button
            variant="outline"
            size="sm"
            onClick={() => onPageChange(page + 1)}
            disabled={page >= totalPages}
            aria-label="Proxima pagina"
          >
            Proximo
            <ChevronRight className="h-4 w-4" />
          </Button>
        </div>
      </div>
    </div>
  )
}
