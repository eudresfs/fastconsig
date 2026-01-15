"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const vitest_1 = require("vitest");
const config_1 = require("@nestjs/config");
// 1. Define mocks using vi.hoisted so we can access them in tests
const mocks = vitest_1.vi.hoisted(() => {
    const fromMock = vitest_1.vi.fn();
    const selectMock = vitest_1.vi.fn().mockReturnValue({ from: fromMock });
    const returningMock = vitest_1.vi.fn();
    const valuesMock = vitest_1.vi.fn().mockReturnValue({ returning: returningMock });
    const insertMock = vitest_1.vi.fn().mockReturnValue({ values: valuesMock });
    const findFirstMock = vitest_1.vi.fn();
    return {
        db: {
            select: selectMock,
            insert: insertMock,
            query: {
                tenants: {
                    findFirst: findFirstMock
                }
            },
            // expose inner mocks for test manipulation
            _from: fromMock,
            _returning: returningMock,
            _findFirst: findFirstMock,
        },
        clerk: {
            verifyToken: vitest_1.vi.fn(),
            createOrganization: vitest_1.vi.fn(),
            createOrganizationInvitation: vitest_1.vi.fn(),
            deleteOrganization: vitest_1.vi.fn(),
        }
    };
});
// 2. Mock modules BEFORE imports
vitest_1.vi.mock('@clerk/clerk-sdk-node', () => ({
    verifyToken: mocks.clerk.verifyToken,
    clerkClient: {
        organizations: {
            createOrganization: mocks.clerk.createOrganization,
            createOrganizationInvitation: mocks.clerk.createOrganizationInvitation,
            deleteOrganization: mocks.clerk.deleteOrganization,
        },
    },
}));
vitest_1.vi.mock('@fast-consig/database', () => ({
    db: mocks.db,
    tenants: { id: 'id_field', name: 'name_field' },
    auditTrails: {},
}));
// 3. Imports
const testing_1 = require("@nestjs/testing");
const supertest_1 = __importDefault(require("supertest"));
const app_module_1 = require("../src/app.module");
const platform_fastify_1 = require("@nestjs/platform-fastify");
const fastify_1 = require("@trpc/server/adapters/fastify");
const app_router_1 = require("../src/app.router");
const context_service_1 = require("../src/core/context/context.service");
const trpc_context_1 = require("../src/core/trpc/trpc.context");
(0, vitest_1.describe)('Tenants (e2e)', () => {
    let app;
    (0, vitest_1.beforeEach)(async () => {
        // Reset mocks
        vitest_1.vi.clearAllMocks();
        // Setup default mock behaviors
        mocks.clerk.verifyToken.mockImplementation((token) => {
            if (token === 'valid_token_super_admin') {
                return Promise.resolve({
                    sub: 'user_admin',
                    org_id: 'org_admin',
                    org_role: 'super_admin',
                    org_permissions: ['read:reports'],
                });
            }
            return Promise.reject(new Error('Invalid token'));
        });
        mocks.clerk.createOrganization.mockResolvedValue({ id: 'org_clerk_new' });
        const moduleFixture = await testing_1.Test.createTestingModule({
            imports: [app_module_1.AppModule],
        })
            .overrideProvider(config_1.ConfigService)
            .useValue({
            get: (key) => {
                if (key === 'CLERK_SECRET_KEY')
                    return 'mock_secret';
                return null;
            }
        })
            .compile();
        app = moduleFixture.createNestApplication(new platform_fastify_1.FastifyAdapter());
        const appRouter = app.get(app_router_1.AppRouter);
        const contextService = app.get(context_service_1.ContextService);
        await app.register(fastify_1.fastifyTRPCPlugin, {
            prefix: '/trpc',
            trpcOptions: { router: appRouter.appRouter, createContext: (0, trpc_context_1.createContext)(contextService) },
        });
        await app.init();
        await app.getHttpAdapter().getInstance().ready();
    });
    (0, vitest_1.afterAll)(async () => {
        await app.close();
    });
    (0, vitest_1.it)('/trpc/tenants.list (GET) - Success', async () => {
        const mockTenants = [{ id: 't1', name: 'Tenant 1', active: true, createdAt: new Date().toISOString() }];
        // Configure mock return
        mocks.db._from.mockResolvedValue(mockTenants);
        const response = await (0, supertest_1.default)(app.getHttpServer())
            .get('/trpc/tenants.list')
            .set('Authorization', 'Bearer valid_token_super_admin');
        if (response.status !== 200) {
            console.error('Tenants List Error Response:', JSON.stringify(response.body, null, 2));
        }
        (0, vitest_1.expect)(response.status).toBe(200);
        (0, vitest_1.expect)(response.body.result.data).toEqual(mockTenants);
    });
    (0, vitest_1.it)('/trpc/tenants.create (POST) - Success', async () => {
        const input = {
            name: 'New Tenant',
            cnpj: '33611500000119',
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
        mocks.db._findFirst.mockResolvedValue(null); // No duplicate
        mocks.db._returning.mockResolvedValue([mockDbResponse]);
        const response = await (0, supertest_1.default)(app.getHttpServer())
            .post('/trpc/tenants.create')
            .set('Authorization', 'Bearer valid_token_super_admin')
            .set('Content-Type', 'application/json')
            .send(input);
        if (response.status !== 200) {
            console.error('Tenants Create Error Response:', JSON.stringify(response.body, null, 2));
        }
        (0, vitest_1.expect)(response.status).toBe(200);
        (0, vitest_1.expect)(response.body.result.data).toEqual(mockDbResponse);
        // Verify mocks called
        (0, vitest_1.expect)(mocks.clerk.createOrganization).toHaveBeenCalled();
        (0, vitest_1.expect)(mocks.db.insert).toHaveBeenCalled();
    });
});
