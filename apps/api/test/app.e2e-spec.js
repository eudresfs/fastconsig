"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const testing_1 = require("@nestjs/testing");
const supertest_1 = __importDefault(require("supertest"));
const app_module_1 = require("../src/app.module");
const platform_fastify_1 = require("@nestjs/platform-fastify");
const vitest_1 = require("vitest");
// Mock Clerk SDK
vitest_1.vi.mock('@clerk/clerk-sdk-node', () => ({
    verifyToken: vitest_1.vi.fn().mockImplementation((token) => {
        if (token === 'valid_token') {
            return Promise.resolve({
                sub: 'user_123',
                org_id: 'org_456',
                org_role: 'admin',
                org_permissions: ['read:reports'],
            });
        }
        return Promise.reject(new Error('Invalid token'));
    }),
}));
describe('AppController (e2e)', () => {
    let app;
    beforeEach(async () => {
        const moduleFixture = await testing_1.Test.createTestingModule({
            imports: [app_module_1.AppModule],
        }).compile();
        app = moduleFixture.createNestApplication(new platform_fastify_1.FastifyAdapter());
        await app.init();
        await app.getHttpAdapter().getInstance().ready();
    });
    afterAll(async () => {
        await app.close();
    });
    it('/ (GET)', () => {
        return (0, supertest_1.default)(app.getHttpServer())
            .get('/')
            .expect(200)
            .expect('Hello World from Fast Consig API!');
    });
    it('/me (GET) - Unauthorized without token', () => {
        return (0, supertest_1.default)(app.getHttpServer())
            .get('/me')
            .expect(401); // AuthGuard should block it
    });
    it('/me (GET) - Success with valid token', async () => {
        const response = await (0, supertest_1.default)(app.getHttpServer())
            .get('/me')
            .set('Authorization', 'Bearer valid_token')
            .expect(200);
        expect(response.body).toEqual({
            userId: 'user_123',
            tenantId: 'org_456',
            roles: ['admin'],
            permissions: ['read:reports'],
        });
    });
    it('/me (GET) - Unauthorized with invalid token', () => {
        return (0, supertest_1.default)(app.getHttpServer())
            .get('/me')
            .set('Authorization', 'Bearer invalid_token')
            .expect(401); // Middleware fails, context is empty, Guard blocks
    });
});
