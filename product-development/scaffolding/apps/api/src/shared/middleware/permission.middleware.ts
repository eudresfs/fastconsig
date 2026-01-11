import { TRPCError } from '@trpc/server'
import { middleware } from '@/trpc/trpc'
import type { Context } from '@/trpc/context'

/**
 * Permission codes for the system
 * Following the pattern: MODULE_ACTION
 */
export const PermissionCodes = {
  // Funcionarios
  FUNCIONARIOS_VISUALIZAR: 'FUNCIONARIOS_VISUALIZAR',
  FUNCIONARIOS_CRIAR: 'FUNCIONARIOS_CRIAR',
  FUNCIONARIOS_EDITAR: 'FUNCIONARIOS_EDITAR',
  FUNCIONARIOS_EXCLUIR: 'FUNCIONARIOS_EXCLUIR',
  FUNCIONARIOS_IMPORTAR: 'FUNCIONARIOS_IMPORTAR',

  // Averbacoes
  AVERBACOES_VISUALIZAR: 'AVERBACOES_VISUALIZAR',
  AVERBACOES_CRIAR: 'AVERBACOES_CRIAR',
  AVERBACOES_EDITAR: 'AVERBACOES_EDITAR',
  AVERBACOES_APROVAR: 'AVERBACOES_APROVAR',
  AVERBACOES_REJEITAR: 'AVERBACOES_REJEITAR',
  AVERBACOES_SUSPENDER: 'AVERBACOES_SUSPENDER',
  AVERBACOES_CANCELAR: 'AVERBACOES_CANCELAR',

  // Margem
  MARGEM_VISUALIZAR: 'MARGEM_VISUALIZAR',
  MARGEM_CONSULTAR: 'MARGEM_CONSULTAR',
  MARGEM_RESERVAR: 'MARGEM_RESERVAR',

  // Simulacao
  SIMULACAO_EXECUTAR: 'SIMULACAO_EXECUTAR',

  // Consignatarias
  CONSIGNATARIAS_VISUALIZAR: 'CONSIGNATARIAS_VISUALIZAR',
  CONSIGNATARIAS_CRIAR: 'CONSIGNATARIAS_CRIAR',
  CONSIGNATARIAS_EDITAR: 'CONSIGNATARIAS_EDITAR',

  // Conciliacao
  CONCILIACAO_VISUALIZAR: 'CONCILIACAO_VISUALIZAR',
  CONCILIACAO_PROCESSAR: 'CONCILIACAO_PROCESSAR',
  CONCILIACAO_FECHAR: 'CONCILIACAO_FECHAR',

  // Relatorios
  RELATORIOS_VISUALIZAR: 'RELATORIOS_VISUALIZAR',
  RELATORIOS_EXPORTAR: 'RELATORIOS_EXPORTAR',

  // Importacao
  IMPORTACAO_EXECUTAR: 'IMPORTACAO_EXECUTAR',
  IMPORTACAO_VISUALIZAR: 'IMPORTACAO_VISUALIZAR',

  // Auditoria
  AUDITORIA_VISUALIZAR: 'AUDITORIA_VISUALIZAR',

  // Usuarios
  USUARIOS_VISUALIZAR: 'USUARIOS_VISUALIZAR',
  USUARIOS_CRIAR: 'USUARIOS_CRIAR',
  USUARIOS_EDITAR: 'USUARIOS_EDITAR',
  USUARIOS_EXCLUIR: 'USUARIOS_EXCLUIR',

  // Configuracoes
  CONFIGURACOES_VISUALIZAR: 'CONFIGURACOES_VISUALIZAR',
  CONFIGURACOES_EDITAR: 'CONFIGURACOES_EDITAR',
} as const

export type PermissionCode = (typeof PermissionCodes)[keyof typeof PermissionCodes]

/**
 * Cache for user permissions to avoid repeated database queries
 * Key: userId, Value: Set of permission codes
 */
const permissionCache = new Map<number, { permissions: Set<string>; expiresAt: number }>()

const CACHE_TTL_MS = 5 * 60 * 1000 // 5 minutes

/**
 * Fetches user permissions from database with caching
 */
async function getUserPermissions(
  ctx: Context,
  userId: number
): Promise<Set<string>> {
  const now = Date.now()
  const cached = permissionCache.get(userId)

  if (cached && cached.expiresAt > now) {
    return cached.permissions
  }

  const userWithPermissions = await ctx.prisma.usuario.findUnique({
    where: { id: userId },
    include: {
      perfil: {
        include: {
          permissoes: {
            include: {
              permissao: true,
            },
          },
        },
      },
    },
  })

  const permissions = new Set<string>(
    userWithPermissions?.perfil.permissoes.map((pp) => pp.permissao.codigo) ?? []
  )

  permissionCache.set(userId, {
    permissions,
    expiresAt: now + CACHE_TTL_MS,
  })

  return permissions
}

/**
 * Clears permission cache for a specific user
 * Call this when user permissions are updated
 */
export function clearPermissionCache(userId: number): void {
  permissionCache.delete(userId)
}

/**
 * Clears entire permission cache
 * Call this when permissions are bulk updated
 */
export function clearAllPermissionCache(): void {
  permissionCache.clear()
}

/**
 * Creates a middleware that checks for a specific permission
 * Usage: protectedProcedure.use(withPermission('AVERBACOES_CRIAR'))
 */
export const withPermission = (requiredPermission: PermissionCode | string) =>
  middleware(async ({ ctx, next }) => {
    if (!ctx.userId) {
      throw new TRPCError({
        code: 'UNAUTHORIZED',
        message: 'Voce precisa estar autenticado',
      })
    }

    const permissions = await getUserPermissions(ctx, ctx.userId)

    if (!permissions.has(requiredPermission)) {
      throw new TRPCError({
        code: 'FORBIDDEN',
        message: `Voce nao tem permissao para esta acao. Permissao requerida: ${requiredPermission}`,
      })
    }

    return next({ ctx })
  })

/**
 * Creates a middleware that checks for any of the specified permissions
 * Usage: protectedProcedure.use(withAnyPermission(['AVERBACOES_CRIAR', 'AVERBACOES_EDITAR']))
 */
export const withAnyPermission = (requiredPermissions: Array<PermissionCode | string>) =>
  middleware(async ({ ctx, next }) => {
    if (!ctx.userId) {
      throw new TRPCError({
        code: 'UNAUTHORIZED',
        message: 'Voce precisa estar autenticado',
      })
    }

    const permissions = await getUserPermissions(ctx, ctx.userId)

    const hasAnyPermission = requiredPermissions.some((p) => permissions.has(p))

    if (!hasAnyPermission) {
      throw new TRPCError({
        code: 'FORBIDDEN',
        message: `Voce nao tem permissao para esta acao. Permissoes requeridas (uma de): ${requiredPermissions.join(', ')}`,
      })
    }

    return next({ ctx })
  })

/**
 * Creates a middleware that checks for all specified permissions
 * Usage: protectedProcedure.use(withAllPermissions(['AVERBACOES_VISUALIZAR', 'AVERBACOES_APROVAR']))
 */
export const withAllPermissions = (requiredPermissions: Array<PermissionCode | string>) =>
  middleware(async ({ ctx, next }) => {
    if (!ctx.userId) {
      throw new TRPCError({
        code: 'UNAUTHORIZED',
        message: 'Voce precisa estar autenticado',
      })
    }

    const permissions = await getUserPermissions(ctx, ctx.userId)

    const missingPermissions = requiredPermissions.filter((p) => !permissions.has(p))

    if (missingPermissions.length > 0) {
      throw new TRPCError({
        code: 'FORBIDDEN',
        message: `Voce nao tem as seguintes permissoes: ${missingPermissions.join(', ')}`,
      })
    }

    return next({ ctx })
  })

/**
 * Helper to check permission in service layer (non-middleware context)
 */
export async function checkPermission(
  ctx: Context,
  userId: number,
  requiredPermission: PermissionCode | string
): Promise<boolean> {
  const permissions = await getUserPermissions(ctx, userId)
  return permissions.has(requiredPermission)
}

/**
 * Helper to get all permissions for a user
 */
export async function getAllUserPermissions(
  ctx: Context,
  userId: number
): Promise<string[]> {
  const permissions = await getUserPermissions(ctx, userId)
  return Array.from(permissions)
}
