import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
  Badge,
  Button,
} from '@fastconsig/ui'
import { formatDate } from '@/lib/utils'
import { FileSpreadsheet, Loader2, CheckCircle, XCircle, Ban } from 'lucide-react'

export type ImportacaoStatus = 'PENDENTE' | 'PROCESSANDO' | 'CONCLUIDO' | 'ERRO' | 'CANCELADO'

export interface Importacao {
  id: number
  tipo: 'FUNCIONARIOS' | 'CONTRATOS' | 'RETORNO_FOLHA'
  nomeArquivo: string
  status: ImportacaoStatus
  linhasProcessadas: number | null
  linhasSucesso: number | null
  linhasErro: number | null
  createdAt: Date | string
  usuario: {
    nome: string
  }
}

interface ImportacaoListProps {
  data: Importacao[]
  onCancel?: (id: number) => void
  isCanceling?: number | null
}

const statusConfig: Record<ImportacaoStatus, { label: string; variant: 'default' | 'secondary' | 'destructive' | 'outline'; icon: any }> = {
  PENDENTE: { label: 'Pendente', variant: 'secondary', icon: Loader2 },
  PROCESSANDO: { label: 'Processando', variant: 'default', icon: Loader2 },
  CONCLUIDO: { label: 'Concluido', variant: 'default', icon: CheckCircle },
  ERRO: { label: 'Erro', variant: 'destructive', icon: XCircle },
  CANCELADO: { label: 'Cancelado', variant: 'outline', icon: Ban },
}

export function ImportacaoList({ data, onCancel, isCanceling }: ImportacaoListProps) {
  return (
    <div className="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Arquivo</TableHead>
            <TableHead>Tipo</TableHead>
            <TableHead>Status</TableHead>
            <TableHead>Progresso</TableHead>
            <TableHead>Usuario</TableHead>
            <TableHead>Data</TableHead>
            <TableHead className="text-right">Acoes</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {data.length === 0 ? (
            <TableRow>
              <TableCell colSpan={7} className="h-24 text-center">
                Nenhuma importacao encontrada
              </TableCell>
            </TableRow>
          ) : (
            data.map((item) => {
              const status = statusConfig[item.status]
              const StatusIcon = status.icon

              return (
                <TableRow key={item.id}>
                  <TableCell>
                    <div className="flex items-center gap-2">
                      <FileSpreadsheet className="h-4 w-4 text-muted-foreground" />
                      <span className="font-medium">{item.nomeArquivo}</span>
                    </div>
                  </TableCell>
                  <TableCell>{item.tipo}</TableCell>
                  <TableCell>
                    <Badge variant={status.variant} className="gap-1">
                      <StatusIcon className={`h-3 w-3 ${item.status === 'PROCESSANDO' ? 'animate-spin' : ''}`} />
                      {status.label}
                    </Badge>
                  </TableCell>
                  <TableCell>
                    {item.status === 'CONCLUIDO' || item.status === 'ERRO' ? (
                      <div className="flex flex-col text-xs">
                        <span className="text-green-600 font-medium">{item.linhasSucesso ?? 0} sucesso</span>
                        <span className="text-destructive font-medium">{item.linhasErro ?? 0} erros</span>
                      </div>
                    ) : (
                      <span className="text-muted-foreground">-</span>
                    )}
                  </TableCell>
                  <TableCell>{item.usuario.nome}</TableCell>
                  <TableCell className="text-muted-foreground">
                    {formatDate(item.createdAt)}
                  </TableCell>
                  <TableCell className="text-right">
                    {item.status === 'PENDENTE' && onCancel && (
                      <Button
                        variant="ghost"
                        size="sm"
                        className="text-destructive hover:text-destructive hover:bg-destructive/10"
                        onClick={() => onCancel(item.id)}
                        disabled={isCanceling === item.id}
                      >
                        {isCanceling === item.id ? (
                          <Loader2 className="h-4 w-4 animate-spin" />
                        ) : (
                          'Cancelar'
                        )}
                      </Button>
                    )}
                  </TableCell>
                </TableRow>
              )
            })
          )}
        </TableBody>
      </Table>
    </div>
  )
}
