import { faker } from '@faker-js/faker';

/**
 * JWT Factory - Generates test JWT token data for authentication testing
 *
 * Usage:
 *   const token = createJWTClaims(); // Random valid claims
 *   const adminToken = createSuperAdminJWT(); // Super admin claims
 *   const invalidToken = createExpiredJWT(); // Expired token claims
 */

export interface JWTClaimsData {
  sub?: string; // User ID
  org_id?: string; // Tenant/Organization ID
  org_role?: string; // Role within organization
  org_permissions?: string[]; // Permissions
  iat?: number; // Issued at
  exp?: number; // Expiration
  iss?: string; // Issuer
  aud?: string; // Audience
}

/**
 * Create valid JWT claims with optional overrides
 */
export const createJWTClaims = (overrides: JWTClaimsData = {}): JWTClaimsData => {
  const now = Math.floor(Date.now() / 1000);

  return {
    sub: `user_${faker.string.alphanumeric(24)}`,
    org_id: `org_${faker.string.alphanumeric(24)}`,
    org_role: 'rh_operator',
    org_permissions: ['read:employees', 'write:employees'],
    iat: now,
    exp: now + 3600, // 1 hour from now
    iss: 'https://clerk.fast-consig.com',
    aud: 'fast-consig-api',
    ...overrides,
  };
};

/**
 * Create super admin JWT claims
 */
export const createSuperAdminJWT = (overrides: JWTClaimsData = {}): JWTClaimsData =>
  createJWTClaims({
    org_role: 'super_admin',
    org_permissions: [
      'read:all',
      'write:all',
      'manage:tenants',
      'manage:users',
    ],
    ...overrides,
  });

/**
 * Create tenant admin JWT claims
 */
export const createTenantAdminJWT = (tenantId: string, overrides: JWTClaimsData = {}): JWTClaimsData =>
  createJWTClaims({
    org_id: tenantId,
    org_role: 'tenant_admin',
    org_permissions: [
      'read:employees',
      'write:employees',
      'read:loans',
      'write:loans',
      'manage:users',
    ],
    ...overrides,
  });

/**
 * Create RH manager JWT claims
 */
export const createRHManagerJWT = (tenantId: string, overrides: JWTClaimsData = {}): JWTClaimsData =>
  createJWTClaims({
    org_id: tenantId,
    org_role: 'rh_manager',
    org_permissions: [
      'read:employees',
      'write:employees',
      'read:loans',
      'read:reports',
    ],
    ...overrides,
  });

/**
 * Create expired JWT claims
 */
export const createExpiredJWT = (overrides: JWTClaimsData = {}): JWTClaimsData => {
  const now = Math.floor(Date.now() / 1000);

  return createJWTClaims({
    iat: now - 7200, // 2 hours ago
    exp: now - 3600, // 1 hour ago (expired)
    ...overrides,
  });
};

/**
 * Create JWT claims without required fields (for testing validation)
 */
export const createInvalidJWT = (missingFields: (keyof JWTClaimsData)[] = []): Partial<JWTClaimsData> => {
  const claims = createJWTClaims();

  missingFields.forEach(field => {
    delete claims[field];
  });

  return claims;
};

/**
 * Create JWT claims with tampered signature (for testing signature verification)
 */
export const createTamperedJWT = (overrides: JWTClaimsData = {}): JWTClaimsData =>
  createJWTClaims({
    ...overrides,
    // This would be detected by signature verification
    org_role: 'super_admin', // Escalated privileges
  });
