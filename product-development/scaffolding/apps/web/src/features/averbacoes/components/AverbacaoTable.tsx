import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
  Button,
  Skeleton,
} from '@fastconsig/ui'
import { SituacaoBadge } from './SituacaoBadge'
import { Eye, MoreHorizontal, Check, X } from 'lucide-react'
import { Link } from '@tanstack/react-router'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@fastconsig/ui'

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

interface Averbacao {
  id: number
  numeroContrato: string
  situacao: SituacaoAverbacao
  valorTotal: number
  valorParcela: number
  parcelasTotal: number
  parcelasPagas: number
  dataContrato: string | Date
  funcionario: {
    id: number
    nome: string
    cpf: string
    matricula: string
  }
  tenantConsignataria: {
    consignataria: {
      razaoSocial: string
    }
  }
  produto: {
    nome: string
    tipo: string
  }
}

interface AverbacaoTableProps {
  data: Averbacao[]
  isLoading?: boolean
  onAprovar?: (id: number) => void
  onRejeitar?: (id: number) => void
  canAprovar?: boolean
}

function formatCurrency(value: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(value)
}

function formatDate(date: string | Date): string {
  return new Intl.DateTimeFormat('pt-BR').format(new Date(date))
}

function formatCpf(cpf: string): string {
  return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4')
}

export function AverbacaoTable({
  data,
  isLoading,
  onAprovar,
  onRejeitar,
  canAprovar,
}: AverbacaoTableProps): JSX.Element {
  if (isLoading) {
    return (
      <div className="space-y-4 p-6">
        {Array.from({ length: 5 }).map((_, i) => (
          <Skeleton key={i} className="h-12 w-full" />
        ))}
      </div>
    )
  }

  if (data.length === 0) {
    return (
      <div className="p-8 text-center text-muted-foreground">
        Nenhuma averbacao encontrada
      </div>
    )
  }

  return (
    <div className="overflow-x-auto">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Contrato</TableHead>
            <TableHead>Funcionario</TableHead>
            <TableHead>Consignataria</TableHead>
            <TableHead>Produto</TableHead>
            <TableHead className="text-right">Valor Total</TableHead>
            <TableHead className="text-right">Parcela</TableHead>
            <TableHead className="text-center">Parcelas</TableHead>
            <TableHead>Situacao</TableHead>
            <TableHead>Data</TableHead>
            <TableHead className="w-[100px]">Acoes</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data.map((averbacao) => (
            <TableRow key={averbacao.id}>
              <TableCell className="font-medium">
                <Link
                  to="/averbacoes/$id"
                  params={{ id: String(averbacao.id) }}
                  className="hover:underline text-primary"
                >
                  {averbacao.numeroContrato}
                </Link>
              </TableCell>
              <TableCell>
                <div>
                  <p className="font-medium">{averbacao.funcionario.nome}</p>
                  <p className="text-sm text-muted-foreground">
                    {formatCpf(averbacao.funcionario.cpf)} | Mat: {averbacao.funcionario.matricula}
                  </p>
                </div>
              </TableCell>
              <TableCell>
                {averbacao.tenantConsignataria.consignataria.razaoSocial}
              </TableCell>
              <TableCell>
                <div>
                  <p>{averbacao.produto.nome}</p>
                  <p className="text-sm text-muted-foreground">{averbacao.produto.tipo}</p>
                </div>
              </TableCell>
              <TableCell className="text-right font-medium">
                {formatCurrency(averbacao.valorTotal)}
              </TableCell>
              <TableCell className="text-right">
                {formatCurrency(averbacao.valorParcela)}
              </TableCell>
              <TableCell className="text-center">
                {averbacao.parcelasPagas}/{averbacao.parcelasTotal}
              </TableCell>
              <TableCell>
                <SituacaoBadge situacao={averbacao.situacao} />
              </TableCell>
              <TableCell>{formatDate(averbacao.dataContrato)}</TableCell>
              <TableCell>
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button
                      variant="ghost"
                      size="icon"
                      aria-label="Abrir menu de acoes"
                    >
                      <MoreHorizontal className="h-4 w-4" />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                    <DropdownMenuItem asChild>
                      <Link to="/averbacoes/$id" params={{ id: String(averbacao.id) }}>
                        <Eye className="mr-2 h-4 w-4" />
                        Visualizar
                      </Link>
                    </DropdownMenuItem>
                    {canAprovar && averbacao.situacao === 'AGUARDANDO_APROVACAO' && (
                      <>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem
                          onClick={() => onAprovar?.(averbacao.id)}
                          className="text-green-600"
                        >
                          <Check className="mr-2 h-4 w-4" />
                          Aprovar
                        </DropdownMenuItem>
                        <DropdownMenuItem
                          onClick={() => onRejeitar?.(averbacao.id)}
                          className="text-red-600"
                        >
                          <X className="mr-2 h-4 w-4" />
                          Rejeitar
                        </DropdownMenuItem>
                      </>
                    )}
                  </DropdownMenuContent>
                </DropdownMenu>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  )
}
