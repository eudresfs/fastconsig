import { trpc } from '@/lib/trpc'
import { formatCurrency } from '@/lib/utils'
import {
  Users,
  FileText,
  Clock,
  DollarSign,
} from 'lucide-react'

export function DashboardPage(): JSX.Element {
  const { data: resumo, isLoading } = trpc.relatorios.resumoGeral.useQuery()

  if (isLoading) {
    return (
      <div className="flex h-full items-center justify-center">
        <div className="h-8 w-8 animate-spin rounded-full border-4 border-primary border-t-transparent" />
      </div>
    )
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Dashboard</h1>
        <p className="mt-1 text-sm text-gray-500">
          Visao geral do sistema de consignados
        </p>
      </div>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
        <StatCard
          icon={<Users className="h-6 w-6" />}
          label="Funcionarios Ativos"
          value={resumo?.funcionarios.ativos ?? 0}
        />
        <StatCard
          icon={<FileText className="h-6 w-6" />}
          label="Total de Averbacoes"
          value={resumo?.averbacoes.total ?? 0}
        />
        <StatCard
          icon={<Clock className="h-6 w-6" />}
          label="Pendentes de Aprovacao"
          value={resumo?.averbacoes.pendentes ?? 0}
        />
        <StatCard
          icon={<DollarSign className="h-6 w-6" />}
          label="Valor em Parcelas/Mes"
          value={formatCurrency(resumo?.folha.valorParcelasMes ?? 0)}
        />
      </div>

      <div className="grid gap-6 lg:grid-cols-2">
        <div className="rounded-lg border bg-white p-6 shadow-sm">
          <h2 className="text-lg font-semibold text-gray-900">
            Averbacoes por Consignataria
          </h2>
          <p className="mt-1 text-sm text-gray-500">
            Distribuicao de contratos por instituicao
          </p>
          {/* Grafico Recharts sera adicionado aqui */}
          <div className="mt-4 flex h-64 items-center justify-center rounded-lg bg-gray-50">
            <p className="text-gray-400">Grafico em desenvolvimento</p>
          </div>
        </div>

        <div className="rounded-lg border bg-white p-6 shadow-sm">
          <h2 className="text-lg font-semibold text-gray-900">
            Evolucao Mensal
          </h2>
          <p className="mt-1 text-sm text-gray-500">
            Quantidade de averbacoes nos ultimos 12 meses
          </p>
          {/* Grafico Recharts sera adicionado aqui */}
          <div className="mt-4 flex h-64 items-center justify-center rounded-lg bg-gray-50">
            <p className="text-gray-400">Grafico em desenvolvimento</p>
          </div>
        </div>
      </div>
    </div>
  )
}

interface StatCardProps {
  icon: React.ReactNode
  label: string
  value: number | string
}

function StatCard({ icon, label, value }: StatCardProps): JSX.Element {
  return (
    <div className="rounded-lg border bg-white p-6 shadow-sm">
      <div className="flex items-center gap-4">
        <div className="rounded-lg bg-primary/10 p-3 text-primary">
          {icon}
        </div>
        <div>
          <p className="text-sm font-medium text-gray-500">{label}</p>
          <p className="text-2xl font-bold text-gray-900">{value}</p>
        </div>
      </div>
    </div>
  )
}
