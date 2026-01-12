import { vi, describe, it, expect, beforeEach, afterAll } from 'vitest';
import { ConfigService } from '@nestjs/config';

// 1. Define mocks using vi.hoisted so we can access them in tests
const mocks = vi.hoisted(() => {
  const fromMock = vi.fn();
  const selectMock = vi.fn().mockReturnValue({ from: fromMock });

  const returningMock = vi.fn();
  const valuesMock = vi.fn().mockReturnValue({ returning: returningMock });
  const insertMock = vi.fn().mockReturnValue({ values: valuesMock });

  return {
    db: {
      select: selectMock,
      insert: insertMock,
      // expose inner mocks for test manipulation
      _from: fromMock,
      _returning: returningMock,
    },
    clerk: {
      verifyToken: vi.fn(),
      createOrganization: vi.fn(),
      createOrganizationInvitation: vi.fn(),
      deleteOrganization: vi.fn(),
    }
  };
});

// 2. Mock modules BEFORE imports
vi.mock('@clerk/clerk-sdk-node', () => ({
  verifyToken: mocks.clerk.verifyToken,
  clerkClient: {
    organizations: {
      createOrganization: mocks.clerk.createOrganization,
      createOrganizationInvitation: mocks.clerk.createOrganizationInvitation,
      deleteOrganization: mocks.clerk.deleteOrganization,
    },
  },
}));

vi.mock('@fast-consig/database', () => ({
  db: mocks.db,
  tenants: { id: 'id_field', name: 'name_field' },
  auditTrails: {},
}));

// 3. Imports
import { Test, TestingModule } from '@nestjs/testing';
import request from 'supertest';
import { AppModule } from '../src/app.module';
import { FastifyAdapter, NestFastifyApplication } from '@nestjs/platform-fastify';
import { fastifyTRPCPlugin } from '@trpc/server/adapters/fastify';
import { AppRouter } from '../src/app.router';
import { ContextService } from '../src/core/context/context.service';
import { createContext } from '../src/core/trpc/trpc.context';

describe('Tenants (e2e)', () => {
  let app: NestFastifyApplication;

  beforeEach(async () => {
    // Reset mocks
    vi.clearAllMocks();

    // Setup default mock behaviors
    mocks.clerk.verifyToken.mockImplementation((token) => {
      if (token === 'valid_token_super_admin') {
        return Promise.resolve({
          sub: 'user_admin',
          org_id: 'org_admin',
          org_role: 'admin',
          org_permissions: ['read:reports'],
        });
      }
      return Promise.reject(new Error('Invalid token'));
    });

    mocks.clerk.createOrganization.mockResolvedValue({ id: 'org_clerk_new' });

    const moduleFixture: TestingModule = await Test.createTestingModule({
      imports: [AppModule],
    })
    .overrideProvider(ConfigService)
    .useValue({
        get: (key: string) => {
            if (key === 'CLERK_SECRET_KEY') return 'mock_secret';
            return null;
        }
    })
    .compile();

    app = moduleFixture.createNestApplication<NestFastifyApplication>(
      new FastifyAdapter(),
    );

    const appRouter = app.get(AppRouter);
    const contextService = app.get(ContextService);

    await app.register(fastifyTRPCPlugin, {
      prefix: '/trpc',
      trpcOptions: { router: appRouter.appRouter, createContext: createContext(contextService) },
    });

    await app.init();
    await app.getHttpAdapter().getInstance().ready();
  });

  afterAll(async () => {
    await app.close();
  });

  it('/trpc/tenants.list (GET) - Success', async () => {
    const mockTenants = [{ id: 't1', name: 'Tenant 1', active: true, createdAt: new Date().toISOString() }];

    // Configure mock return
    mocks.db._from.mockResolvedValue(mockTenants);

    const response = await request(app.getHttpServer())
      .get('/trpc/tenants.list')
      .set('Authorization', 'Bearer valid_token_super_admin');

    if (response.status !== 200) {
      console.error('Tenants List Error Response:', JSON.stringify(response.body, null, 2));
    }
    expect(response.status).toBe(200);
    expect(response.body.result.data).toEqual(mockTenants);
  });

  it('/trpc/tenants.create (POST) - Success', async () => {
    const input = {
      name: 'New Tenant',
      cnpj: '12345678000199',
      slug: 'new-tenant',
      adminEmail: 'admin@new.com'
    };

    const mockDbResponse = {
        id: 'new_id',
        clerkOrgId: 'org_clerk_new',
        name: input.name,
        cnpj: input.cnpj,
        slug: input.slug,
        active: true,
        createdAt: new Date().toISOString()
    };

    // Configure mock return
    mocks.db._returning.mockResolvedValue([mockDbResponse]);

    const response = await request(app.getHttpServer())
      .post('/trpc/tenants.create')
      .set('Authorization', 'Bearer valid_token_super_admin')
      .set('Content-Type', 'application/json')
      .send(input);

    if (response.status !== 200) {
      console.error('Tenants Create Error Response:', JSON.stringify(response.body, null, 2));
    }
    expect(response.status).toBe(200);

    expect(response.body.result.data).toEqual(mockDbResponse);

    // Verify mocks called
    expect(mocks.clerk.createOrganization).toHaveBeenCalled();
    expect(mocks.db.insert).toHaveBeenCalled();
  });
});