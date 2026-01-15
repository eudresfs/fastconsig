import { faker } from '@faker-js/faker';

/**
 * Tenant Factory - Generates test tenant data using faker
 *
 * Usage:
 *   const tenant = createTenant(); // Random tenant
 *   const specificTenant = createTenant({ name: 'Specific Name' }); // With overrides
 *   const tenants = createTenants(5); // Array of 5 tenants
 */

export interface TenantFactoryData {
  id?: string;
  clerkOrgId?: string;
  name?: string;
  cnpj?: string;
  slug?: string;
  active?: boolean;
  createdAt?: string;
  updatedAt?: string;
}

/**
 * Generates a valid Brazilian CNPJ (Cadastro Nacional da Pessoa Jurídica)
 * Format: XX.XXX.XXX/XXXX-XX
 */
function generateCNPJ(): string {
  const randomDigits = () => faker.string.numeric(1);
  const cnpj = Array.from({ length: 14 }, randomDigits).join('');

  // Format: XX.XXX.XXX/XXXX-XX
  return cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, '$1.$2.$3/$4-$5');
}

/**
 * Generates a URL-safe slug from a string
 */
function generateSlug(name: string): string {
  return name
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '') // Remove accents
    .replace(/[^a-z0-9]+/g, '-') // Replace non-alphanumeric with dash
    .replace(/^-+|-+$/g, ''); // Remove leading/trailing dashes
}

/**
 * Create a single tenant with optional overrides
 */
export const createTenant = (overrides: TenantFactoryData = {}): TenantFactoryData => {
  const name = overrides.name || faker.company.name();
  const slug = overrides.slug || generateSlug(name);

  return {
    id: faker.string.uuid(),
    clerkOrgId: `org_${faker.string.alphanumeric(24)}`,
    name,
    cnpj: generateCNPJ(),
    slug,
    active: true,
    createdAt: faker.date.recent().toISOString(),
    updatedAt: faker.date.recent().toISOString(),
    ...overrides,
  };
};

/**
 * Create an array of tenants
 */
export const createTenants = (count: number): TenantFactoryData[] =>
  Array.from({ length: count }, () => createTenant());

/**
 * Create a tenant with specific configuration
 */
export const createInactiveTenant = (overrides: TenantFactoryData = {}): TenantFactoryData =>
  createTenant({ active: false, ...overrides });

/**
 * Create a tenant for testing duplicate scenarios
 */
export const createDuplicateTenant = (existingTenant: TenantFactoryData): TenantFactoryData =>
  createTenant({
    name: existingTenant.name,
    cnpj: existingTenant.cnpj,
    slug: existingTenant.slug,
  });
