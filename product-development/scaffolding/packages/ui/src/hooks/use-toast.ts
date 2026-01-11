import { toast as sonnerToast } from 'sonner'

type ToastType = 'success' | 'error' | 'info' | 'warning'

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

export function useToast(): UseToastReturn {
  const toast = (options: ToastOptions): void => {
    const { title, description, duration, action } = options

    sonnerToast(title, {
      description,
      duration,
      action: action
        ? {
            label: action.label,
            onClick: action.onClick,
          }
        : undefined,
    })
  }

  const success = (message: string, options?: Omit<ToastOptions, 'title'>): void => {
    sonnerToast.success(message, {
      description: options?.description,
      duration: options?.duration,
      action: options?.action
        ? {
            label: options.action.label,
            onClick: options.action.onClick,
          }
        : undefined,
    })
  }

  const error = (message: string, options?: Omit<ToastOptions, 'title'>): void => {
    sonnerToast.error(message, {
      description: options?.description,
      duration: options?.duration,
      action: options?.action
        ? {
            label: options.action.label,
            onClick: options.action.onClick,
          }
        : undefined,
    })
  }

  const info = (message: string, options?: Omit<ToastOptions, 'title'>): void => {
    sonnerToast.info(message, {
      description: options?.description,
      duration: options?.duration,
      action: options?.action
        ? {
            label: options.action.label,
            onClick: options.action.onClick,
          }
        : undefined,
    })
  }

  const warning = (message: string, options?: Omit<ToastOptions, 'title'>): void => {
    sonnerToast.warning(message, {
      description: options?.description,
      duration: options?.duration,
      action: options?.action
        ? {
            label: options.action.label,
            onClick: options.action.onClick,
          }
        : undefined,
    })
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
