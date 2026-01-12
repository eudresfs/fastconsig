import { Test, TestingModule } from '@nestjs/testing';
import request from 'supertest';
import { AppModule } from '../src/app.module';
import { FastifyAdapter, NestFastifyApplication } from '@nestjs/platform-fastify';
import { vi } from 'vitest';

// Mock Clerk SDK
vi.mock('@clerk/clerk-sdk-node', () => ({
  verifyToken: vi.fn().mockImplementation((token) => {
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
  let app: NestFastifyApplication;

  beforeEach(async () => {
    const moduleFixture: TestingModule = await Test.createTestingModule({
      imports: [AppModule],
    }).compile();

    app = moduleFixture.createNestApplication<NestFastifyApplication>(
      new FastifyAdapter(),
    );
    await app.init();
    await app.getHttpAdapter().getInstance().ready();
  });

  afterAll(async () => {
    await app.close();
  });

  it('/ (GET)', () => {
    return request(app.getHttpServer())
      .get('/')
      .expect(200)
      .expect('Hello World from Fast Consig API!');
  });

  it('/me (GET) - Unauthorized without token', () => {
    return request(app.getHttpServer())
      .get('/me')
      .expect(401); // AuthGuard should block it
  });

  it('/me (GET) - Success with valid token', async () => {
    const response = await request(app.getHttpServer())
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
    return request(app.getHttpServer())
      .get('/me')
      .set('Authorization', 'Bearer invalid_token')
      .expect(401); // Middleware fails, context is empty, Guard blocks
  });
});
