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
    query: {
      tenants: {
        findFirst: vi.fn(),
      },
    },
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
      // Mock Duplicate Check (No duplicate found)
      (db.query.tenants.findFirst as any).mockResolvedValue(null);

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
      // Mock Duplicate Check (No duplicate found)
      (db.query.tenants.findFirst as any).mockResolvedValue(null);

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

    it('should rollback both Clerk and DB if admin invitation fails', async () => {
      // Mock Duplicate Checks (No duplicates found) - now we have 2 separate checks for CNPJ and Slug
      (db.query.tenants.findFirst as any)
        .mockResolvedValueOnce(null)  // First call: CNPJ duplicate check
        .mockResolvedValueOnce(null)  // Second call: Slug duplicate check
        .mockResolvedValueOnce({ id: 'tenant_123' });  // Third call: rollback check

      // Mock Clerk Org creation success
      const mockClerkOrg = { id: 'org_clerk_123' };
      (clerkClient.organizations.createOrganization as any).mockResolvedValue(mockClerkOrg);

      // Mock DB insert success
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

      const returningMock = vi.fn().mockResolvedValue([mockDbTenant]);
      const valuesMock = vi.fn().mockReturnValue({ returning: returningMock });
      (db.insert as any).mockReturnValue({ values: valuesMock });

      // Mock admin invitation FAILURE
      (clerkClient.organizations.createOrganizationInvitation as any).mockRejectedValue(
        new Error('Invitation failed')
      );

      // Mock DB delete for rollback
      const deleteSpy = vi.fn().mockResolvedValue({});
      const whereSpy = vi.fn().mockReturnValue(deleteSpy);
      (db.delete as any) = vi.fn().mockReturnValue({ where: whereSpy });

      await expect(service.create(input)).rejects.toThrow(InternalServerErrorException);

      // Verify rollback of BOTH Clerk Org and DB record
      expect(clerkClient.organizations.deleteOrganization).toHaveBeenCalledWith('org_clerk_123');
      expect(db.query.tenants.findFirst).toHaveBeenCalledTimes(3); // CNPJ check + Slug check + rollback check
      expect(db.delete).toHaveBeenCalled(); // Delete DB record
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
