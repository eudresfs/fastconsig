import { TRPCError } from '@trpc/server'
import { middleware } from '@/trpc/trpc'
import type { Context } from '@/trpc/context'

/**
 * Tenant context after validation
 */
export interface TenantContext {
  tenantId: number
  userId: number
  consignatariaId: number | null
  perfilId: number
}

/**
 * Extended context with validated tenant information
 */
export interface ContextWithTenant extends Context {
  tenant: TenantContext
}

/**
 * Middleware that extracts and validates tenant information from JWT
 * Sets tenant context for downstream procedures
 */
export const withTenant = middleware(async ({ ctx, next }) => {
  if (!ctx.userId) {
    throw new TRPCError({
      code: 'UNAUTHORIZED',
      message: 'Voce precisa estar autenticado para acessar este recurso',
    })
  }

  if (!ctx.tenantId) {
    throw new TRPCError({
      code: 'FORBIDDEN',
      message: 'Usuario nao esta associado a nenhum tenant',
    })
  }

  // Fetch user with tenant and profile information
  const usuario = await ctx.prisma.usuario.findUnique({
    where: { id: ctx.userId },
    select: {
      id: true,
      tenantId: true,
      consignatariaId: true,
      perfilId: true,
      ativo: true,
      bloqueado: true,
      tenant: {
        select: {
          id: true,
          ativo: true,
        },
      },
    },
  })

  if (!usuario) {
    throw new TRPCError({
      code: 'UNAUTHORIZED',
      message: 'Usuario nao encontrado',
    })
  }

  if (!usuario.ativo) {
    throw new TRPCError({
      code: 'FORBIDDEN',
      message: 'Usuario inativo',
    })
  }

  if (usuario.bloqueado) {
    throw new TRPCError({
      code: 'FORBIDDEN',
      message: 'Usuario bloqueado',
    })
  }

  if (usuario.tenant && !usuario.tenant.ativo) {
    throw new TRPCError({
      code: 'FORBIDDEN',
      message: 'Tenant inativo',
    })
  }

  const tenant: TenantContext = {
    tenantId: ctx.tenantId,
    userId: ctx.userId,
    consignatariaId: usuario.consignatariaId,
    perfilId: usuario.perfilId,
  }

  return next({
    ctx: {
      ...ctx,
      tenant,
    },
  })
})

/**
 * Helper to create tenant-filtered where clause
 * Ensures all queries are scoped to the current tenant
 */
export function withTenantFilter<T extends Record<string, unknown>>(
  tenantId: number,
  where: T
): T & { tenantId: number } {
  return {
    ...where,
    tenantId,
  }
}

/**
 * Helper to validate that a resource belongs to the current tenant
 */
export function validateTenantOwnership(
  resourceTenantId: number | null | undefined,
  currentTenantId: number,
  resourceName: string
): void {
  if (resourceTenantId !== currentTenantId) {
    throw new TRPCError({
      code: 'FORBIDDEN',
      message: `${resourceName} nao pertence ao tenant atual`,
    })
  }
}
