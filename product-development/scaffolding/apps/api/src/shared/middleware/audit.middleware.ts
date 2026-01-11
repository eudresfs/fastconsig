import { middleware } from '@/trpc/trpc'
import type { Context } from '@/trpc/context'
import type { AcaoAuditoria } from '@prisma/client'

/**
 * Audit action types matching the Prisma enum
 */
export const AuditActions = {
  CRIAR: 'CRIAR',
  ATUALIZAR: 'ATUALIZAR',
  EXCLUIR: 'EXCLUIR',
  LOGIN: 'LOGIN',
  LOGOUT: 'LOGOUT',
  APROVAR: 'APROVAR',
  REJEITAR: 'REJEITAR',
  SUSPENDER: 'SUSPENDER',
  CANCELAR: 'CANCELAR',
  IMPORTAR: 'IMPORTAR',
  EXPORTAR: 'EXPORTAR',
} as const

export type AuditAction = (typeof AuditActions)[keyof typeof AuditActions]

/**
 * Audit log entry input
 */
export interface AuditLogInput {
  entidade: string
  entidadeId?: number | null
  acao: AuditAction
  dadosAnteriores?: Record<string, unknown> | null
  dadosNovos?: Record<string, unknown> | null
  observacao?: string
}

/**
 * Context extension for audit functionality
 */
export interface AuditContext {
  audit: {
    log: (input: AuditLogInput) => Promise<void>
  }
}

/**
 * Extracts IP address from request
 */
function getClientIp(ctx: Context): string | null {
  const req = ctx.req
  const forwardedFor = req.headers['x-forwarded-for']
  if (typeof forwardedFor === 'string') {
    return forwardedFor.split(',')[0]?.trim() ?? null
  }
  if (Array.isArray(forwardedFor)) {
    return forwardedFor[0]?.split(',')[0]?.trim() ?? null
  }
  return req.ip ?? null
}

/**
 * Extracts user agent from request
 */
function getUserAgent(ctx: Context): string | null {
  const userAgent = ctx.req.headers['user-agent']
  if (typeof userAgent === 'string') {
    return userAgent.substring(0, 500) // Limit to database field size
  }
  return null
}

/**
 * Creates an audit log entry in the database
 */
async function createAuditLog(
  ctx: Context,
  input: AuditLogInput
): Promise<void> {
  try {
    await ctx.prisma.auditoria.create({
      data: {
        tenantId: ctx.tenantId,
        usuarioId: ctx.userId,
        entidade: input.entidade,
        entidadeId: input.entidadeId,
        acao: input.acao as AcaoAuditoria,
        dadosAnteriores: input.dadosAnteriores ?? undefined,
        dadosNovos: input.dadosNovos ?? undefined,
        ip: getClientIp(ctx),
        userAgent: getUserAgent(ctx),
      },
    })
  } catch (error) {
    // Log error but don't fail the main operation
    console.error('Failed to create audit log:', error)
  }
}

/**
 * Middleware that adds audit logging capability to context
 * Usage: protectedProcedure.use(withAudit)
 */
export const withAudit = middleware(async ({ ctx, next }) => {
  const auditLog = async (input: AuditLogInput): Promise<void> => {
    await createAuditLog(ctx, input)
  }

  return next({
    ctx: {
      ...ctx,
      audit: {
        log: auditLog,
      },
    },
  })
})

/**
 * Service-level audit logging function
 * Use when you need to log from outside middleware context
 */
export async function logAuditAction(
  ctx: Context,
  input: AuditLogInput
): Promise<void> {
  await createAuditLog(ctx, input)
}

/**
 * Helper to compute diff between old and new data for audit
 * Returns only the fields that changed
 */
export function computeAuditDiff<T extends Record<string, unknown>>(
  oldData: T,
  newData: Partial<T>
): {
  dadosAnteriores: Partial<T>
  dadosNovos: Partial<T>
} {
  const dadosAnteriores: Partial<T> = {}
  const dadosNovos: Partial<T> = {}

  for (const key of Object.keys(newData) as Array<keyof T>) {
    const oldValue = oldData[key]
    const newValue = newData[key]

    // Compare values (handling dates and other types)
    const oldStr = JSON.stringify(oldValue)
    const newStr = JSON.stringify(newValue)

    if (oldStr !== newStr) {
      dadosAnteriores[key] = oldValue
      dadosNovos[key] = newValue as T[keyof T]
    }
  }

  return { dadosAnteriores, dadosNovos }
}

/**
 * Helper to sanitize sensitive data before audit logging
 * Removes password fields and other sensitive information
 */
export function sanitizeForAudit<T extends Record<string, unknown>>(
  data: T,
  sensitiveFields: string[] = ['senha', 'senhaHash', 'password', 'token', 'refreshToken']
): Partial<T> {
  const sanitized: Partial<T> = {}

  for (const [key, value] of Object.entries(data)) {
    if (sensitiveFields.some((field) => key.toLowerCase().includes(field.toLowerCase()))) {
      sanitized[key as keyof T] = '[REDACTED]' as T[keyof T]
    } else if (value !== null && typeof value === 'object' && !Array.isArray(value)) {
      sanitized[key as keyof T] = sanitizeForAudit(
        value as Record<string, unknown>,
        sensitiveFields
      ) as T[keyof T]
    } else {
      sanitized[key as keyof T] = value as T[keyof T]
    }
  }

  return sanitized
}
