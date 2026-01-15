import { faker } from '@faker-js/faker';

/**
 * User Factory - Generates test user data using faker
 *
 * Usage:
 *   const user = createUser(); // Random user
 *   const admin = createSuperAdminUser(); // Super admin user
 *   const users = createUsers(10); // Array of 10 users
 */

export interface UserFactoryData {
  id?: string;
  email?: string;
  name?: string;
  role?: 'super_admin' | 'tenant_admin' | 'rh_manager' | 'rh_operator';
  tenantId?: string;
  clerkUserId?: string;
  createdAt?: string;
  updatedAt?: string;
}

/**
 * Create a single user with optional overrides
 */
export const createUser = (overrides: UserFactoryData = {}): UserFactoryData => ({
  id: faker.string.uuid(),
  email: faker.internet.email(),
  name: faker.person.fullName(),
  role: 'rh_operator',
  tenantId: faker.string.uuid(),
  clerkUserId: `user_${faker.string.alphanumeric(24)}`,
  createdAt: faker.date.recent().toISOString(),
  updatedAt: faker.date.recent().toISOString(),
  ...overrides,
});

/**
 * Create an array of users
 */
export const createUsers = (count: number): UserFactoryData[] =>
  Array.from({ length: count }, () => createUser());

/**
 * Create a super admin user
 */
export const createSuperAdminUser = (overrides: UserFactoryData = {}): UserFactoryData =>
  createUser({
    role: 'super_admin',
    tenantId: null, // Super admins don't belong to a tenant
    ...overrides,
  });

/**
 * Create a tenant admin user
 */
export const createTenantAdminUser = (tenantId: string, overrides: UserFactoryData = {}): UserFactoryData =>
  createUser({
    role: 'tenant_admin',
    tenantId,
    ...overrides,
  });

/**
 * Create an RH manager user
 */
export const createRHManagerUser = (tenantId: string, overrides: UserFactoryData = {}): UserFactoryData =>
  createUser({
    role: 'rh_manager',
    tenantId,
    ...overrides,
  });

/**
 * Create an RH operator user
 */
export const createRHOperatorUser = (tenantId: string, overrides: UserFactoryData = {}): UserFactoryData =>
  createUser({
    role: 'rh_operator',
    tenantId,
    ...overrides,
  });
