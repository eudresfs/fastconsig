export interface UserContext {
  userId: string;
  tenantId?: string;
  email?: string;
  roles?: string[];
  permissions?: string[];
  ip?: string;
}
