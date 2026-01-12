import { describe, it, expect } from 'vitest';
import { CreateTenantSchema } from './tenant.schema';

describe('CreateTenantSchema', () => {
  it('should validate a correct CNPJ', () => {
    const validInput = {
      name: 'Valid Tenant',
      slug: 'valid-tenant',
      cnpj: '33611500000119', // Example valid CNPJ
      adminEmail: 'admin@test.com',
    };
    const result = CreateTenantSchema.safeParse(validInput);
    expect(result.success).toBe(true);
  });

  it('should reject an invalid CNPJ checksum', () => {
    const invalidInput = {
      name: 'Invalid Tenant',
      slug: 'invalid-tenant',
      cnpj: '11111111111111', // Invalid checksum (and repeated digits usually)
      adminEmail: 'admin@test.com',
    };
    const result = CreateTenantSchema.safeParse(invalidInput);
    expect(result.success).toBe(false);
    if (!result.success) {
      expect(result.error.issues[0].message).toBe('Invalid CNPJ checksum');
    }
  });

  it('should reject a CNPJ with wrong length', () => {
    const invalidInput = {
      name: 'Invalid Tenant',
      slug: 'invalid-tenant',
      cnpj: '123',
      adminEmail: 'admin@test.com',
    };
    const result = CreateTenantSchema.safeParse(invalidInput);
    expect(result.success).toBe(false);
  });
});
