import { toast as sonnerToast, type ExternalToast } from 'sonner'

interface ToastOptions {
  title?: string
  description?: string
  duration?: number
  action?: {
    label: string
    onClick: () => void
  }
}

interface UseToastReturn {
  toast: (options: ToastOptions) => void
  success: (message: string, options?: Omit<ToastOptions, 'title'>) => void
  error: (message: string, options?: Omit<ToastOptions, 'title'>) => void
  info: (message: string, options?: Omit<ToastOptions, 'title'>) => void
  warning: (message: string, options?: Omit<ToastOptions, 'title'>) => void
  dismiss: (toastId?: string | number) => void
}

function mapOptionsToSonner(options?: Omit<ToastOptions, 'title'>): ExternalToast {
  if (!options) return {}

  const result: ExternalToast = {}

  if (options.description) result.description = options.description
  if (options.duration) result.duration = options.duration
  if (options.action) {
    result.action = {
      label: options.action.label,
      onClick: options.action.onClick,
    }
  }

  return result
}

export function useToast(): UseToastReturn {
  const toast = (options: ToastOptions): void => {
    const { title, ...rest } = options
    sonnerToast(title, mapOptionsToSonner(rest))
  }

  const success = (message: string, options?: Omit<ToastOptions, 'title'>): void => {
    sonnerToast.success(message, mapOptionsToSonner(options))
  }

  const error = (message: string, options?: Omit<ToastOptions, 'title'>): void => {
    sonnerToast.error(message, mapOptionsToSonner(options))
  }

  const info = (message: string, options?: Omit<ToastOptions, 'title'>): void => {
    sonnerToast.info(message, mapOptionsToSonner(options))
  }

  const warning = (message: string, options?: Omit<ToastOptions, 'title'>): void => {
    sonnerToast.warning(message, mapOptionsToSonner(options))
  }

  const dismiss = (toastId?: string | number): void => {
    sonnerToast.dismiss(toastId)
  }

  return {
    toast,
    success,
    error,
    info,
    warning,
    dismiss,
  }
}
