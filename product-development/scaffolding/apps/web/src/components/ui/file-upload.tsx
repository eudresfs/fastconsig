import * as React from 'react'
import { useDropzone, type DropzoneOptions } from 'react-dropzone'
import { Upload, X, FileSpreadsheet, AlertCircle } from 'lucide-react'
import { cn } from '@/lib/utils'
import { Button } from '@fastconsig/ui'

interface FileUploadProps extends DropzoneOptions {
  value?: File | null
  onChange?: (file: File | null) => void
  onRemove?: () => void
  className?: string
  label?: string
  error?: string
}

export function FileUpload({
  value,
  onChange,
  onRemove,
  className,
  label = 'Clique para selecionar ou arraste o arquivo aqui',
  error,
  ...dropzoneProps
}: FileUploadProps) {
  const onDrop = React.useCallback(
    (acceptedFiles: File[]) => {
      if (acceptedFiles.length > 0 && onChange) {
        onChange(acceptedFiles[0] || null)
      }
    },
    [onChange]
  )

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    multiple: false,
    ...dropzoneProps,
  })

  const handleRemove = (e: React.MouseEvent) => {
    e.stopPropagation()
    if (onChange) onChange(null)
    if (onRemove) onRemove()
  }

  return (
    <div className={cn('w-full space-y-2', className)}>
      <div
        {...getRootProps()}
        className={cn(
          'relative flex min-h-[150px] cursor-pointer flex-col items-center justify-center rounded-lg border-2 border-dashed border-muted-foreground/25 px-6 py-4 text-center transition-colors hover:bg-muted/50',
          isDragActive && 'border-primary bg-muted',
          error && 'border-destructive/50',
          value && 'border-primary/50 bg-muted/30 cursor-default'
        )}
      >
        <input {...getInputProps()} />

        {value ? (
          <div className="flex flex-col items-center gap-2">
            <div className="rounded-full bg-primary/10 p-3">
              <FileSpreadsheet className="h-6 w-6 text-primary" />
            </div>
            <div className="space-y-1">
              <p className="text-sm font-medium text-foreground">{value.name}</p>
              <p className="text-xs text-muted-foreground">
                {(value.size / 1024).toFixed(2)} KB
              </p>
            </div>
            <Button
              type="button"
              variant="ghost"
              size="sm"
              className="absolute top-2 right-2 h-8 w-8 p-0"
              onClick={handleRemove}
            >
              <X className="h-4 w-4" />
              <span className="sr-only">Remover arquivo</span>
            </Button>
          </div>
        ) : (
          <div className="flex flex-col items-center gap-2">
            <div className="rounded-full bg-muted p-3">
              <Upload className="h-6 w-6 text-muted-foreground" />
            </div>
            <div className="space-y-1">
              <p className="text-sm font-medium text-foreground">
                {isDragActive ? 'Solte o arquivo aqui' : label}
              </p>
              <p className="text-xs text-muted-foreground">
                Suporta: .xlsx, .csv (max 10MB)
              </p>
            </div>
          </div>
        )}
      </div>
      {error && (
        <div className="flex items-center gap-2 text-sm text-destructive">
          <AlertCircle className="h-4 w-4" />
          <span>{error}</span>
        </div>
      )}
    </div>
  )
}
