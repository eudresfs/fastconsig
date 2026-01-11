import { useState } from 'react'
import { trpc } from '@/lib/trpc'
import { Breadcrumb } from '@/components/layout/Breadcrumb'
import { ImportacaoList, type Importacao } from '../components/ImportacaoList'
import { FileUpload } from '@/components/ui/file-upload'
import { useAuthStore } from '@/stores/auth'
import {
  Button,
  Card,
  CardContent,
  CardHeader,
  CardTitle,
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
  Label,
} from '@fastconsig/ui'
import { Plus, Upload, RefreshCw } from 'lucide-react'
import { toast } from 'sonner'

export function ImportacaoPage() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const [selectedFile, setSelectedFile] = useState<File | null>(null)
  const [importType, setImportType] = useState<'FUNCIONARIOS' | 'CONTRATOS' | 'RETORNO_FOLHA'>('FUNCIONARIOS')
  const [cancelingId, setCancelingId] = useState<number | null>(null)
  const { accessToken } = useAuthStore()

  // Queries
  const { data, refetch, isLoading } = trpc.importacao.list.useQuery({
    page: 1,
    pageSize: 20,
  })

  // Mutations
  const cancelMutation = trpc.importacao.cancelar.useMutation({
    onSuccess: () => {
      toast.success('Importacao cancelada')
      setCancelingId(null)
      refetch()
    },
    onError: (error: any) => {
      toast.error(error.message || 'Erro ao cancelar importacao')
      setCancelingId(null)
    },
  })

  // Handlers
  const handleUpload = async () => {
    if (!selectedFile) return
    if (!accessToken) {
      toast.error('Usuario nao autenticado')
      return
    }

    try {
      const formData = new FormData()
      formData.append('file', selectedFile)
      formData.append('tipo', importType)
      formData.append('nomeArquivo', selectedFile.name)
      formData.append('tamanhoBytes', selectedFile.size.toString())

      const response = await fetch(`${import.meta.env.VITE_API_URL}/upload/importacao`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${accessToken}`
        },
        body: formData
      })

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}))
        throw new Error(errorData.message || 'Falha no upload')
      }

      toast.success('Arquivo enviado com sucesso')
      setIsDialogOpen(false)
      setSelectedFile(null)
      refetch()

    } catch (error: any) {
      toast.error(error.message || 'Erro ao enviar arquivo')
      console.error(error)
    }
  }

  const handleCancel = (id: number) => {
    setCancelingId(id)
    cancelMutation.mutate({ id })
  }

  return (
    <div className="space-y-6">
      <Breadcrumb items={[{ label: 'Importacao' }]} />

      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-bold tracking-tight">Importacao de Dados</h1>
          <p className="text-muted-foreground">
            Importe funcionarios, contratos e retornos de folha.
          </p>
        </div>

        <div className="flex gap-2">
          <Button variant="outline" onClick={() => refetch()} disabled={isLoading}>
            <RefreshCw className={`h-4 w-4 ${isLoading ? 'animate-spin' : ''}`} />
          </Button>
          <Button onClick={() => setIsDialogOpen(true)}>
            <Plus className="mr-2 h-4 w-4" />
            Nova Importacao
          </Button>
        </div>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Historico de Importacoes</CardTitle>
        </CardHeader>
        <CardContent>
          <ImportacaoList
            data={(data?.data as Importacao[]) ?? []}
            onCancel={handleCancel}
            isCanceling={cancelingId}
          />
        </CardContent>
      </Card>

      <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Nova Importacao</DialogTitle>
          </DialogHeader>

          <div className="space-y-4 py-4">
            <div className="space-y-2">
              <Label>Tipo de Arquivo</Label>
              <Select
                value={importType}
                onValueChange={(val: any) => setImportType(val)}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="FUNCIONARIOS">Funcionarios (CSV/XLSX)</SelectItem>
                  <SelectItem value="CONTRATOS">Contratos (CSV/XLSX)</SelectItem>
                  <SelectItem value="RETORNO_FOLHA">Retorno de Folha (CSV/XLSX)</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label>Arquivo</Label>
              <FileUpload
                value={selectedFile}
                onChange={setSelectedFile}
                accept={{
                  'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet': ['.xlsx'],
                  'text/csv': ['.csv']
                }}
              />
            </div>

            <div className="flex justify-end gap-2 pt-4">
              <Button variant="outline" onClick={() => setIsDialogOpen(false)}>
                Cancelar
              </Button>
              <Button
                onClick={handleUpload}
                disabled={!selectedFile}
              >
                {'Importar'}
                <Upload className="ml-2 h-4 w-4" />
              </Button>
            </div>
          </div>
        </DialogContent>
      </Dialog>
    </div>
  )
}
