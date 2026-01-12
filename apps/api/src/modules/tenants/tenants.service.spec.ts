import 'reflect-metadata';
import { Test, TestingModule } from '@nestjs/testing';
import { TenantsService } from './tenants.service';
import { ConfigService } from '@nestjs/config';
import { ContextService } from '../../core/context/context.service';
import { clerkClient } from '@clerk/clerk-sdk-node';
import { db } from '@fast-consig/database';
import { InternalServerErrorException } from '@nestjs/common';
import { vi, describe, it, expect, beforeEach } from 'vitest';

// Mock dependencies
vi.mock('@clerk/clerk-sdk-node', () => ({
  clerkClient: {
    organizations: {
      createOrganization: vi.fn(),
      createOrganizationInvitation: vi.fn(),
      deleteOrganization: vi.fn(),
    },
  },
}));

vi.mock('@fast-consig/database', () => ({
  db: {
    insert: vi.fn().mockReturnThis(),
    values: vi.fn().mockReturnThis(),
    returning: vi.fn(),
    select: vi.fn().mockReturnThis(),
    from: vi.fn(),
  },
  tenants: {},
  auditTrails: {},
}));

describe('TenantsService', () => {
  let service: TenantsService;
  let configService: ConfigService;
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  let contextService: ContextService;

  beforeEach(async () => {
    const mockConfigService = {
      get: vi.fn((key: string) => {
        if (key === 'CLERK_SECRET_KEY') return 'mock_secret_key';
        return null;
      }),
    };

    const mockContextService = {
      getUserId: vi.fn().mockReturnValue('user_123'),
    };

    // Manual instantiation to bypass potential DI issues in test environment
    service = new TenantsService(mockConfigService as any, mockContextService as any);
    configService = mockConfigService as any;
    contextService = mockContextService as any;

    vi.clearAllMocks();
  });

  it('should be defined', () => {
    expect(service).toBeDefined();
  });

  describe('create', () => {
    const input = {
      name: 'Test Org',
      slug: 'test-org',
      cnpj: '12345678000199',
      adminEmail: 'admin@test.com',
    };

    it('should create a tenant successfully (Dual-Write)', async () => {
      // Mock Clerk response
      const mockClerkOrg = { id: 'org_clerk_123' };
      (clerkClient.organizations.createOrganization as any).mockResolvedValue(mockClerkOrg);

      // Mock DB response
      const mockDbTenant = {
        id: 'tenant_123',
        clerkOrgId: 'org_clerk_123',
        name: 'Test Org',
        cnpj: '12345678000199',
        slug: 'test-org',
        active: true,
        createdAt: new Date(),
        updatedAt: new Date(),
      };

      // Mock db.insert().values().returning() chain
      const returningMock = vi.fn().mockResolvedValue([mockDbTenant]);
      const valuesMock = vi.fn().mockReturnValue({ returning: returningMock });
      (db.insert as any).mockReturnValue({ values: valuesMock });

      const result = await service.create(input);

      // Verify Clerk call
      expect(clerkClient.organizations.createOrganization).toHaveBeenCalledWith({
        name: input.name,
        slug: input.slug,
        createdBy: 'user_123',
      });

      // Verify DB call
      expect(db.insert).toHaveBeenCalled();

      // Verify Invitation
      expect(clerkClient.organizations.createOrganizationInvitation).toHaveBeenCalledWith({
        organizationId: 'org_clerk_123',
        emailAddress: input.adminEmail,
        role: 'org:admin',
        inviterUserId: 'user_123',
      });

      expect(result).toEqual(mockDbTenant);
    });

    it('should rollback Clerk creation if DB insert fails', async () => {
      // Mock Clerk success
      const mockClerkOrg = { id: 'org_clerk_123' };
      (clerkClient.organizations.createOrganization as any).mockResolvedValue(mockClerkOrg);

      // Mock DB failure
      const returningMock = vi.fn().mockRejectedValue(new Error('DB Error'));
      const valuesMock = vi.fn().mockReturnValue({ returning: returningMock });
      (db.insert as any).mockReturnValue({ values: valuesMock });

      await expect(service.create(input)).rejects.toThrow(InternalServerErrorException);

      // Verify Rollback
      expect(clerkClient.organizations.deleteOrganization).toHaveBeenCalledWith('org_clerk_123');
    });

    it('should throw error if CLERK_SECRET_KEY is missing', async () => {
        (configService.get as any).mockReturnValue(null);
        await expect(service.create(input)).rejects.toThrow(InternalServerErrorException);
    });
  });

  describe('list', () => {
    it('should return a list of tenants', async () => {
        const mockTenants = [{ id: '1', name: 'T1' }];

        // Mock db.select().from()
        const fromMock = vi.fn().mockResolvedValue(mockTenants);
        (db.select as any).mockReturnValue({ from: fromMock });

        const result = await service.list();
        expect(result).toEqual(mockTenants);
    });
  });
});
