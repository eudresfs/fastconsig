import { Test, TestingModule } from '@nestjs/testing';
import { TenantsService } from '../tenants.service';
import { ConfigService } from '@nestjs/config';
import { ContextService } from '../../../core/context/context.service';
import { AuditTrailService } from '../../../shared/services/audit-trail.service';
import { clerkClient } from '@clerk/clerk-sdk-node';
import { db } from '@fast-consig/database';
import { BadRequestException } from '@nestjs/common';

// Mocks
jest.mock('@clerk/clerk-sdk-node', () => ({
  clerkClient: {
    organizations: {
      createOrganizationInvitation: jest.fn(),
      deleteOrganizationMembership: jest.fn(),
      getOrganizationMembershipList: jest.fn(),
    },
  },
}));

jest.mock('@fast-consig/database', () => ({
  db: {
    query: {
      tenants: {
        findFirst: jest.fn(),
      },
    },
  },
}));

describe('TenantsService (Team Management)', () => {
  let service: TenantsService;
  let auditTrailService: AuditTrailService;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        TenantsService,
        {
          provide: ConfigService,
          useValue: { get: jest.fn() },
        },
        {
          provide: ContextService,
          useValue: { getUserId: jest.fn().mockReturnValue('user_admin') },
        },
        {
          provide: AuditTrailService,
          useValue: { log: jest.fn() },
        },
      ],
    }).compile();

    service = module.get<TenantsService>(TenantsService);
    auditTrailService = module.get<AuditTrailService>(AuditTrailService);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  describe('inviteMember', () => {
    const mockTenant = { id: 'tenant_1', clerkOrgId: 'org_1' };

    it('should invite a member successfully', async () => {
      // Mock tenant found
      (db.query.tenants.findFirst as jest.Mock).mockResolvedValue(mockTenant);

      // Mock Clerk invitation
      (clerkClient.organizations.createOrganizationInvitation as jest.Mock).mockResolvedValue({
        id: 'invite_1',
      });

      const result = await service.inviteMember('tenant_1', 'new@example.com', 'org:member');

      expect(clerkClient.organizations.createOrganizationInvitation).toHaveBeenCalledWith({
        organizationId: 'org_1',
        emailAddress: 'new@example.com',
        role: 'org:member',
        inviterUserId: 'user_admin',
      });

      expect(auditTrailService.log).toHaveBeenCalledWith(expect.objectContaining({
        action: 'CREATE',
        resourceType: 'team_invite',
      }));

      expect(result).toEqual({ id: 'invite_1', status: 'pending', email: 'new@example.com' });
    });

    it('should throw BadRequestException for invalid role', async () => {
      (db.query.tenants.findFirst as jest.Mock).mockResolvedValue(mockTenant);

      await expect(service.inviteMember('tenant_1', 'email@test.com', 'invalid_role'))
        .rejects.toThrow(BadRequestException);
    });
  });

  describe('removeMember', () => {
    const mockTenant = { id: 'tenant_1', clerkOrgId: 'org_1' };

    it('should remove a member successfully', async () => {
      (db.query.tenants.findFirst as jest.Mock).mockResolvedValue(mockTenant);

      (clerkClient.organizations.deleteOrganizationMembership as jest.Mock).mockResolvedValue({});

      await service.removeMember('tenant_1', 'user_to_remove');

      expect(clerkClient.organizations.deleteOrganizationMembership).toHaveBeenCalledWith({
        organizationId: 'org_1',
        userId: 'user_to_remove',
      });

      expect(auditTrailService.log).toHaveBeenCalledWith(expect.objectContaining({
        action: 'DELETE',
        resourceType: 'team_member',
        resourceId: 'user_to_remove',
      }));
    });
  });
});
