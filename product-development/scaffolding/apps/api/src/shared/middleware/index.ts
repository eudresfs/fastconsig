// Tenant middleware
export {
  withTenant,
  withTenantFilter,
  validateTenantOwnership,
  type TenantContext,
  type ContextWithTenant,
} from './tenant.middleware'

// Permission middleware
export {
  withPermission,
  withAnyPermission,
  withAllPermissions,
  checkPermission,
  getAllUserPermissions,
  clearPermissionCache,
  clearAllPermissionCache,
  PermissionCodes,
  type PermissionCode,
} from './permission.middleware'

// Audit middleware
export {
  withAudit,
  logAuditAction,
  computeAuditDiff,
  sanitizeForAudit,
  AuditActions,
  type AuditAction,
  type AuditLogInput,
  type AuditContext,
} from './audit.middleware'
