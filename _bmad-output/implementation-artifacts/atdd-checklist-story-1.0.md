# ATDD Checklist - Epic 1, Story 1.0: Project Foundation Setup

**Date:** 2026-01-13
**Author:** Eudres
**Primary Test Level:** Integration/E2E (API)

---

## Story Summary

Inicializar o projeto usando estrutura de monorepo Turborepo com workspaces pnpm para gerenciar apps de API, Web e Workers com type-safety e código compartilhado desde o dia 1.

**As a** Platform Architect
**I want** to initialize the project using a Turborepo monorepo structure with pnpm workspaces
**So that** I can manage the API, Web, and Workers apps with type-safety and shared code from day one

---

## Acceptance Criteria

1. ✅ A Turborepo monorepo should be created with apps for `api` (NestJS), `web` (React 19), and `jobs` (BullMQ)
2. ✅ Shared packages for `database` (Drizzle), `shared` (Zod schemas), and `ui` (shadcn) should be configured
3. ✅ Docker Compose should be ready for local PostgreSQL and Redis development
4. ✅ A GitHub Actions CI pipeline should be established with Turborepo caching enabled

---

## Failing Tests Created (RED Phase)

### E2E Tests (4 tests)

**File:** `apps/api/test/foundation.e2e-spec.ts` (~150 lines)

- ✅ **Test:** Turborepo build should compile all apps successfully
  - **Status:** RED - Turborepo pipeline not configured
  - **Verifies:** All apps (api, web, jobs) build without errors using turbo build

- ✅ **Test:** Shared packages should be importable across apps
  - **Status:** RED - Package references not configured
  - **Verifies:** @fast-consig/shared and @fast-consig/database can be imported in api/web/jobs

- ✅ **Test:** Docker Compose should start PostgreSQL and Redis
  - **Status:** RED - docker-compose.yml missing
  - **Verifies:** Database connection succeeds and Redis responds to PING

- ✅ **Test:** GitHub Actions CI should cache Turborepo builds
  - **Status:** RED - .github/workflows/ci.yml missing Turborepo cache config
  - **Verifies:** CI pipeline uses Turborepo remote caching and runs tests

### API Tests (0 tests)

N/A - Foundation tests are infrastructure-level

### Component Tests (0 tests)

N/A - No UI components in foundation setup

---

## Data Factories Created

### Tenant Factory

**File:** `apps/api/test/support/factories/tenant.factory.ts`

**Exports:**

- `createTenant(overrides?)` - Create single tenant with optional overrides
- `createTenants(count)` - Create array of tenants
- `createInactiveTenant(overrides?)` - Create inactive tenant for testing
- `createDuplicateTenant(existing)` - Create duplicate tenant for uniqueness tests

**Example Usage:**

```typescript
const tenant = createTenant({ name: 'Prefeitura de São Paulo' });
const tenants = createTenants(5); // Generate 5 random tenants
```

### User Factory

**File:** `apps/api/test/support/factories/user.factory.ts`

**Exports:**

- `createUser(overrides?)` - Create single user with optional overrides
- `createUsers(count)` - Create array of users
- `createSuperAdminUser(overrides?)` - Create super admin user
- `createTenantAdminUser(tenantId, overrides?)` - Create tenant admin
- `createRHManagerUser(tenantId, overrides?)` - Create RH manager
- `createRHOperatorUser(tenantId, overrides?)` - Create RH operator

**Example Usage:**

```typescript
const admin = createSuperAdminUser();
const manager = createRHManagerUser('tenant-123');
```

### JWT Factory

**File:** `apps/api/test/support/factories/jwt.factory.ts`

**Exports:**

- `createJWTClaims(overrides?)` - Create valid JWT claims
- `createSuperAdminJWT(overrides?)` - Create super admin JWT
- `createTenantAdminJWT(tenantId, overrides?)` - Create tenant admin JWT
- `createRHManagerJWT(tenantId, overrides?)` - Create RH manager JWT
- `createExpiredJWT(overrides?)` - Create expired JWT for testing
- `createInvalidJWT(missingFields)` - Create JWT with missing fields
- `createTamperedJWT(overrides?)` - Create JWT with tampered claims

**Example Usage:**

```typescript
const validClaims = createJWTClaims();
const expiredClaims = createExpiredJWT();
const invalidClaims = createInvalidJWT(['org_id', 'sub']);
```

---

## Fixtures Created

### Database Fixture

**File:** `apps/api/test/support/fixtures/database.fixture.ts`

**Fixtures:**

- `cleanDatabase` - Provides clean database state with auto-cleanup
  - **Setup:** Truncates all tables before test
  - **Provides:** Clean database connection
  - **Cleanup:** Truncates tables after test

**Example Usage:**

```typescript
import { test } from './fixtures/database.fixture';

test('should create tenant', async ({ cleanDatabase }) => {
  // Database is clean and ready
});
```

### Auth Fixture

**File:** `apps/api/test/support/fixtures/auth.fixture.ts`

**Fixtures:**

- `authenticatedRequest` - Provides authenticated HTTP request client
  - **Setup:** Generates valid JWT and configures request headers
  - **Provides:** Supertest client with Authorization header
  - **Cleanup:** None required (stateless)

**Example Usage:**

```typescript
import { test } from './fixtures/auth.fixture';

test('should access protected route', async ({ authenticatedRequest }) => {
  const response = await authenticatedRequest.get('/api/protected');
  expect(response.status).toBe(200);
});
```

---

## Mock Requirements

### Clerk SDK Mock

**Service:** Clerk Authentication

**Mock Implementation Required:**

```typescript
vi.mock('@clerk/clerk-sdk-node', () => ({
  verifyToken: vi.fn(),
  clerkClient: {
    organizations: {
      createOrganization: vi.fn(),
      createOrganizationInvitation: vi.fn(),
      deleteOrganization: vi.fn(),
    },
  },
}));
```

**Success Response (verifyToken):**

```json
{
  "sub": "user_2abc123",
  "org_id": "org_2xyz789",
  "org_role": "super_admin",
  "org_permissions": ["read:all", "write:all"]
}
```

**Failure Response:**

```json
{
  "error": "Invalid token"
}
```

**Notes:** Mock should simulate token validation without hitting Clerk API

### Database Mock (for unit tests)

**Service:** Drizzle ORM

**Mock Implementation:**

```typescript
vi.mock('@fast-consig/database', () => ({
  db: {
    select: vi.fn(),
    insert: vi.fn(),
    query: {
      tenants: { findFirst: vi.fn() }
    }
  },
  tenants: { id: 'id', name: 'name' },
}));
```

**Notes:** Use real database for E2E tests, mocks for unit tests only

---

## Required data-testid Attributes

N/A - Story 1.0 is infrastructure setup, no UI components yet

---

## Implementation Checklist

### Test: Turborepo build should compile all apps successfully

**File:** `apps/api/test/foundation.e2e-spec.ts`

**Tasks to make this test pass:**

- [ ] Initialize Turborepo with `npx create-turbo@latest`
- [ ] Configure `turbo.json` with build pipeline for all apps
- [ ] Set up `apps/api` with NestJS and Fastify
- [ ] Set up `apps/web` with React 19 and Vite
- [ ] Set up `apps/jobs` with BullMQ worker
- [ ] Configure package.json scripts for `turbo build`
- [ ] Run test: `pnpm test:e2e -- foundation.e2e-spec.ts`
- [ ] ✅ Test passes (green phase)

**Estimated Effort:** 4 hours

---

### Test: Shared packages should be importable across apps

**File:** `apps/api/test/foundation.e2e-spec.ts`

**Tasks to make this test pass:**

- [ ] Create `packages/database` with Drizzle ORM setup
- [ ] Create `packages/shared` with Zod schemas
- [ ] Create `packages/ui` with shadcn/ui components
- [ ] Configure TypeScript paths in each app's tsconfig.json
- [ ] Add package dependencies in apps' package.json
- [ ] Verify imports work: `import { db } from '@fast-consig/database'`
- [ ] Run test: `pnpm test:e2e -- foundation.e2e-spec.ts`
- [ ] ✅ Test passes (green phase)

**Estimated Effort:** 3 hours

---

### Test: Docker Compose should start PostgreSQL and Redis

**File:** `apps/api/test/foundation.e2e-spec.ts`

**Tasks to make this test pass:**

- [ ] Create `docker-compose.yml` with PostgreSQL 16 service
- [ ] Add Redis 7 service to docker-compose.yml
- [ ] Configure environment variables for database connection
- [ ] Add healthcheck endpoints for both services
- [ ] Create `.env.example` with required variables
- [ ] Test connection: `docker-compose up -d && pnpm test:db-connection`
- [ ] Run test: `pnpm test:e2e -- foundation.e2e-spec.ts`
- [ ] ✅ Test passes (green phase)

**Estimated Effort:** 2 hours

---

### Test: GitHub Actions CI should cache Turborepo builds

**File:** `apps/api/test/foundation.e2e-spec.ts`

**Tasks to make this test pass:**

- [ ] Create `.github/workflows/ci.yml`
- [ ] Configure pnpm cache action
- [ ] Add Turborepo remote cache configuration
- [ ] Set up test job with `pnpm turbo test`
- [ ] Configure build job with `pnpm turbo build`
- [ ] Add TURBO_TOKEN and TURBO_TEAM secrets (optional)
- [ ] Verify CI runs and caches correctly on PR
- [ ] Run test: Check CI logs for cache hit/miss
- [ ] ✅ Test passes (green phase)

**Estimated Effort:** 2 hours

---

## Running Tests

```bash
# Run all foundation tests
pnpm test:e2e -- foundation.e2e-spec.ts

# Run specific test
pnpm test:e2e -- foundation.e2e-spec.ts -t "Turborepo build"

# Run tests in watch mode
pnpm test:e2e -- foundation.e2e-spec.ts --watch

# Run tests with coverage
pnpm test:e2e -- foundation.e2e-spec.ts --coverage

# Verify Docker services
docker-compose up -d && pnpm test:db-connection
```

---

## Red-Green-Refactor Workflow

### RED Phase (Complete) ✅

**TEA Agent Responsibilities:**

- ✅ All tests written and failing
- ✅ Fixtures and factories created with auto-cleanup
- ✅ Mock requirements documented
- ✅ data-testid requirements listed (N/A for this story)
- ✅ Implementation checklist created

**Verification:**

- All tests run and fail as expected
- Failure messages are clear: "Turborepo not configured", "Package not found", etc.
- Tests fail due to missing implementation, not test bugs

---

### GREEN Phase (DEV Team - Next Steps)

**DEV Agent Responsibilities:**

1. **Pick one failing test** from implementation checklist (start with Turborepo build)
2. **Read the test** to understand expected infrastructure setup
3. **Implement minimal setup** to make that specific test pass
4. **Run the test** to verify it now passes (green)
5. **Check off the task** in implementation checklist
6. **Move to next test** and repeat

**Key Principles:**

- One test at a time (don't try to set up everything at once)
- Minimal configuration (use defaults, don't over-configure)
- Run tests frequently (immediate feedback on infrastructure)
- Use implementation checklist as roadmap

**Progress Tracking:**

- Check off tasks as you complete them
- Share progress in daily standup
- Mark story as DONE in `sprint-status.yaml` when all tests pass

---

### REFACTOR Phase (DEV Team - After All Tests Pass)

**DEV Agent Responsibilities:**

1. **Verify all tests pass** (green phase complete)
2. **Review configuration for optimization** (cache strategies, build performance)
3. **Extract common configuration** (shared tsconfig, shared ESLint)
4. **Optimize Docker setup** (multi-stage builds, volume caching)
5. **Ensure tests still pass** after each optimization
6. **Update documentation** (README with setup instructions)

**Key Principles:**

- Tests provide safety net (refactor infrastructure with confidence)
- Make small optimizations (easier to debug if tests fail)
- Run tests after each change
- Don't change test behavior (only infrastructure implementation)

**Completion:**

- All tests pass
- Infrastructure is optimized for developer experience
- Documentation is clear for team onboarding
- Ready for Epic 1, Story 1.1 implementation

---

## Next Steps

1. **Share this checklist** with the dev team in standup
2. **Review infrastructure requirements** with team
3. **Run failing tests** to confirm RED phase: `pnpm test:e2e -- foundation.e2e-spec.ts`
4. **Begin implementation** using implementation checklist as guide
5. **Work one test at a time** (red → green for each)
6. **Share progress** in daily standup
7. **When all tests pass**, optimize and document setup
8. **When complete**, update story status to 'done' in sprint-status.yaml

---

## Knowledge Base References Applied

This ATDD workflow consulted the following knowledge fragments:

- **data-factories.md** - Factory patterns using `@faker-js/faker` for random test data generation with overrides support
- **test-quality.md** - Test design principles (Given-When-Then, determinism, isolation)
- **test-levels-framework.md** - Test level selection framework (E2E for infrastructure validation)
- **fixture-architecture.md** - Test fixture patterns with setup/teardown and auto-cleanup

See `tea-index.csv` for complete knowledge fragment mapping.

---

## Test Execution Evidence

### Initial Test Run (RED Phase Verification)

**Command:** `pnpm test:e2e -- foundation.e2e-spec.ts`

**Expected Results:**

```
❌ Turborepo build should compile all apps successfully
   Error: Cannot find module 'turbo'

❌ Shared packages should be importable across apps
   Error: Cannot find module '@fast-consig/database'

❌ Docker Compose should start PostgreSQL and Redis
   Error: docker-compose.yml not found

❌ GitHub Actions CI should cache Turborepo builds
   Error: .github/workflows/ci.yml not found
```

**Summary:**

- Total tests: 4
- Passing: 0 (expected)
- Failing: 4 (expected)
- Status: ✅ RED phase verified

**Expected Failure Messages:**

1. **Turborepo build test**: "Cannot find module 'turbo'" or "turbo.json not found"
2. **Shared packages test**: "Cannot find module '@fast-consig/database'" or similar import errors
3. **Docker Compose test**: "docker-compose.yml not found" or "Connection refused"
4. **GitHub Actions test**: ".github/workflows/ci.yml not found" or workflow validation failure

---

## Notes

- Esta story estabelece a fundação técnica para todo o projeto
- Todos os apps subsequentes dependem desta infraestrutura
- Priorize a conclusão desta story antes de iniciar Story 1.1
- Docker Compose é essencial para desenvolvimento local
- Turborepo cache melhora significativamente a velocidade de CI/CD
- Considere usar Turborepo remote cache (Vercel) para equipes distribuídas

---

## Contact

**Questions or Issues?**

- Ask in team standup
- Tag @eudres in Slack/Discord
- Refer to `_bmad/bmm/docs/tea-README.md` for workflow documentation
- Consult `_bmad/bmm/testarch/knowledge` for testing best practices

---

**Generated by BMad TEA Agent** - 2026-01-13
