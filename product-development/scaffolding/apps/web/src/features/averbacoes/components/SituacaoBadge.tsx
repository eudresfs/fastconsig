import { Badge } from '@fastconsig/ui'
import { cn } from '@/lib/utils'

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

interface SituacaoBadgeProps {
  situacao: SituacaoAverbacao
  className?: string
}

const situacaoConfig: Record<
  SituacaoAverbacao,
  { label: string; className: string }
> = {
  AGUARDANDO_APROVACAO: {
    label: 'Aguardando Aprovacao',
    className: 'bg-yellow-100 text-yellow-800 border-yellow-300',
  },
  APROVADA: {
    label: 'Aprovada',
    className: 'bg-green-100 text-green-800 border-green-300',
  },
  REJEITADA: {
    label: 'Rejeitada',
    className: 'bg-red-100 text-red-800 border-red-300',
  },
  ENVIADA: {
    label: 'Enviada',
    className: 'bg-blue-100 text-blue-800 border-blue-300',
  },
  DESCONTADA: {
    label: 'Descontada',
    className: 'bg-indigo-100 text-indigo-800 border-indigo-300',
  },
  SUSPENSA: {
    label: 'Suspensa',
    className: 'bg-orange-100 text-orange-800 border-orange-300',
  },
  BLOQUEADA: {
    label: 'Bloqueada',
    className: 'bg-gray-100 text-gray-800 border-gray-300',
  },
  LIQUIDADA: {
    label: 'Liquidada',
    className: 'bg-emerald-100 text-emerald-800 border-emerald-300',
  },
  CANCELADA: {
    label: 'Cancelada',
    className: 'bg-red-100 text-red-800 border-red-300',
  },
}

export function SituacaoBadge({
  situacao,
  className,
}: SituacaoBadgeProps): JSX.Element {
  const config = situacaoConfig[situacao]

  return (
    <Badge
      variant="outline"
      className={cn(config.className, className)}
      aria-label={`Situacao: ${config.label}`}
    >
      {config.label}
    </Badge>
  )
}
