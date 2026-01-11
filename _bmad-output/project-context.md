---
project_name: 'Fast Consig'
user_name: 'Eudres'
date: '2026-01-11'
sections_completed: ['technology_stack', 'language_rules', 'framework_rules', 'testing_rules', 'code_quality_rules', 'workflow_rules', 'critical_rules']
existing_patterns_found: 37
workflow_type: 'generate-project-context'
status: 'complete'
completed_at: '2026-01-11'
source_documents:
  - _bmad-output/planning-artifacts/architecture.md
  - _bmad-output/planning-artifacts/prd.md
  - _bmad-output/planning-artifacts/epics.md
---

# Project Context for AI Agents

_This file contains critical rules and patterns that AI agents must follow when implementing code in this project. Focus on unobvious details that agents might otherwise miss._

---

## Technology Stack & Versions

### Core Technologies

- **Runtime:** Node.js 22+
- **Backend Framework:** NestJS 11 + Fastify Adapter
- **API Layer:** tRPC v11+
- **ORM:** Drizzle ORM v0.36+ (REQUIRED for `.for('update')` support)
- **Database:** PostgreSQL 16+ with Row-Level Security (RLS)
- **Cache/Queue:** Redis 7 + BullMQ
- **Authentication:** Clerk (B2B Organizations mode)

### Frontend Stack

- **Framework:** React 19
- **Language:** TypeScript 5.5+ (strict mode enabled)
- **Build Tool:** Vite
- **Router:** TanStack Router v1.50+
- **State Management:** Zustand v4.5+
- **Forms:** React Hook Form + Zod v3.24+
- **UI Library:** shadcn/ui (Radix UI primitives) + Tailwind CSS 4

### Monorepo Architecture

- **Build Orchestration:** Turborepo
- **Package Manager:** pnpm workspaces
- **Structure:**
  - 3 apps: `api` (NestJS), `jobs` (BullMQ workers), `web` (React)
  - 4 packages: `database`, `shared`, `ui`, `config`

### Infrastructure

- **Hosting:** Oracle Cloud VMs (NOT Vercel)
- **Containers:** Docker Compose
- **Observability:** OpenTelemetry + Grafana (self-hosted)

### Critical Version Dependencies

⚠️ **Drizzle v0.36+ is MANDATORY** - Earlier versions lack native `.for('update')` support required for pessimistic locking in token system and margin updates.

⚠️ **PostgreSQL 16+ is REQUIRED** - Optimized RLS policies and performance improvements critical for multi-tenant isolation.

⚠️ **Use drizzle-zod** - Automatically infer Zod schemas from Drizzle schemas to maintain type-safety across stack.

---

## Critical Implementation Rules

### Language-Specific Rules (TypeScript/Node.js)

#### TypeScript Configuration

- **Strict Mode is MANDATORY** - All packages must have `strict: true`, `strictNullChecks: true`, `noUncheckedIndexedAccess: true`
- **Path Aliases** - ALWAYS use `@fastconsig/*` aliases for imports between packages
- **NEVER use relative imports between packages** - `import { db } from '../../../packages/database'` is FORBIDDEN

```typescript
// ✅ CORRECT
import { db } from '@fastconsig/database'
import { CreateLoanSchema } from '@fastconsig/shared/schemas'
import { Button } from '@fastconsig/ui'

// ❌ NEVER DO THIS
import { db } from '../../../packages/database/src'
```

#### Import/Export Patterns

- **Prefer Named Exports** over default exports (better tree-shaking)
- **Import Order:**
  1. Node.js built-ins (`import { createHash } from 'crypto'`)
  2. External packages (`import { z } from 'zod'`)
  3. Monorepo aliases (`import { db } from '@fastconsig/database'`)
  4. Relative imports (`import { helper } from './utils'`)

#### Async/Await Patterns

- **ALWAYS use async/await** - NEVER use `.then()` chains
- **Error Handling** - Always wrap in try/catch and rethrow as TRPCError

```typescript
// ✅ CORRECT
try {
  const employee = await db.select().from(employees).where(eq(employees.id, id))
} catch (error) {
  if (error instanceof TRPCError) throw error
  throw new TRPCError({
    code: 'INTERNAL_SERVER_ERROR',
    message: 'Erro ao buscar funcionário',
    cause: error
  })
}

// ❌ NEVER DO THIS
db.select().from(employees).then(result => ...).catch(err => ...)
```

#### Type-Safety Rules

- **Zod Schema → TypeScript Type** - ALWAYS infer via `z.infer<typeof Schema>`, NEVER define manually
- **Drizzle → Zod** - Use `drizzle-zod` to infer Zod schemas from Drizzle schemas

```typescript
import { createInsertSchema } from 'drizzle-zod'
import { employees } from '@fastconsig/database/schema'

// ✅ CORRECT - Automatic inference
export const InsertEmployeeSchema = createInsertSchema(employees)
export type InsertEmployee = z.infer<typeof InsertEmployeeSchema>
```

#### Monetary Values (CRITICAL ⚠️)

- **ALWAYS use INTEGER (centavos)** - NEVER use float/number for money
- **Database:** `integer('available_margin').notNull()` stores centavos
- **TypeScript:** `const amountCents = 5000` represents R$ 50,00
- **Display:** Use `Intl.NumberFormat` to format

```typescript
// ✅ CORRECT
const amountCents = 5000 // R$ 50,00
const formatted = new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL'
}).format(amountCents / 100)

// ❌ NEVER - Float loses precision
const amount = 50.00 // ❌ FORBIDDEN
```

---

_Will continue with Framework-Specific Rules next_

### Framework-Specific Rules

#### NestJS (Backend)

**Hexagonal Architecture - Module Boundaries:**
- Each module in `apps/api/src/modules/{domain}/` is a bounded context
- **Mandatory structure:** `{domain}.service.ts`, `{domain}.router.ts`, `{domain}.schema.ts`, `__tests__/`
- **FORBIDDEN:** Direct imports between domain modules (`employees` importing from `loans`)
- **ALLOWED:** Communication via Domain Events or Services injected via DI

**Dependency Injection:**
```typescript
// ✅ CORRECT - Constructor injection
@Injectable()
export class EmployeesService {
  constructor(
    private readonly db: Database,
    private readonly cls: AsyncLocalStorage<{ tenantId: string }>
  ) {}
}
```

**Context Middleware (Multi-Tenancy):**
- ALWAYS extract `tenantId` from JWT via Context Middleware
- Store in AsyncLocalStorage for global access
- NEVER pass `tenantId` manually in each query

#### React (Frontend)

**Component Structure:**
- **File naming:** `PascalCase.tsx` for components (`EmployeeCard.tsx`)
- **Hooks:** `use{Nome}.ts` pattern (`useEmployeeMargin.ts`)
- **Organization:** Feature-based in `apps/web/src/features/{feature}/`

**State Management Rules:**
- **Global State (Zustand):** ONLY for auth (`auth.store.ts`)
- **Server State (tRPC):** Use tRPC hooks for all queries/mutations
- **Local State:** `useState` for UI state within component
- **NEVER:** Share state between features via props drilling

**Hooks Usage:**
```typescript
// ✅ CORRECT - tRPC hooks
const { data, isLoading } = trpc.employees.list.useQuery()
const createEmployee = trpc.employees.create.useMutation({
  onSuccess: () => toast({ title: 'Funcionário criado' }),
  onError: (error) => toast({ variant: 'destructive', title: error.message })
})
```

**Performance Patterns:**
- Use `React.memo()` only when necessary (not by default)
- Loading states with Skeleton components (shadcn/ui)

```typescript
// ✅ CORRECT - Skeleton loading
if (isLoading) {
  return (
    <div className="space-y-4">
      {[...Array(5)].map((_, i) => (
        <Skeleton key={i} className="h-16 w-full" />
      ))}
    </div>
  )
}
```

#### Drizzle ORM

**Pessimistic Locking (CRITICAL ⚠️):**
- ALWAYS use `.for('update')` when updating margin or validating tokens
- ALWAYS within a transaction

```typescript
// ✅ CORRECT - Pessimistic locking
await db.transaction(async (tx) => {
  const [employee] = await tx
    .select()
    .from(employees)
    .where(eq(employees.id, employeeId))
    .for('update') // ⚠️ MANDATORY for margin updates

  await tx.update(employees)
    .set({ availableMargin: newMargin })
    .where(eq(employees.id, employeeId))
})
```

**Multi-Tenancy Queries:**
- ALWAYS filter by `tenantId` in queries
- Use Context Middleware to obtain `tenantId`

```typescript
// ✅ CORRECT
const { tenantId } = this.cls.getStore()!
return db.select()
  .from(employees)
  .where(eq(employees.tenantId, tenantId))

// ❌ NEVER - Query without tenant filter
return db.select().from(employees) // ❌ SECURITY RISK
```

#### tRPC

**Router Naming:**
- Format: `{domain}Router` (`employeesRouter`, `loansRouter`)
- File: `{domain}.router.ts`

**Procedure Naming:**
- Queries: `list`, `getById`, `findByCpf`
- Mutations: `create`, `update`, `delete`, `cancel`

**Error Handling:**
- ALWAYS use `TRPCError` with standardized codes
- Codes: `UNAUTHORIZED`, `FORBIDDEN`, `BAD_REQUEST`, `NOT_FOUND`, `CONFLICT`, `INTERNAL_SERVER_ERROR`

```typescript
// ✅ CORRECT
throw new TRPCError({
  code: 'BAD_REQUEST',
  message: 'Margem insuficiente',
  cause: { available: 1000, required: 1500 }
})
```

#### Zod Validation

**Schema Sharing:**
- Schemas in `@fastconsig/shared/schemas`
- Reuse in backend (tRPC input) AND frontend (React Hook Form)

```typescript
// packages/shared/schemas/employee.schema.ts
export const CreateEmployeeSchema = z.object({
  cpf: z.string().length(11),
  name: z.string().min(1).max(255),
  grossSalary: z.number().int().positive()
})

// Backend - tRPC
.input(CreateEmployeeSchema)

// Frontend - React Hook Form
const form = useForm<CreateEmployeeDTO>({
  resolver: zodResolver(CreateEmployeeSchema)
})
```

---

_Will continue with Testing Rules next_

### Testing Rules

#### Test Organization

**File Structure:**
- **Unit tests:** Co-located in `__tests__/` within each module
- **Integration tests:** `apps/api/__tests__/integration/`
- **E2E tests:** `apps/web/__tests__/e2e/`

**Naming Convention:**
- Use `.test.ts` OR `.spec.ts` (consistent within each app)
- Pattern: `{nome-do-arquivo}.test.ts`

#### Test Frameworks

- **Backend (NestJS):** Jest (native integration)
- **Frontend (React):** Vitest + Testing Library
- **Shared Packages:** Vitest (fast, ESM-native)

#### Critical Testing Patterns (MUST TEST)

**Multi-Tenancy Isolation:**
```typescript
// ✅ CORRECT - Verify tenant isolation
it('should only return employees from current tenant', async () => {
  const tenantA = await createTenant('Tenant A')
  const tenantB = await createTenant('Tenant B')

  await createEmployee({ tenantId: tenantA.id, name: 'Employee A' })
  await createEmployee({ tenantId: tenantB.id, name: 'Employee B' })

  cls.set('tenantId', tenantA.id)
  const employees = await employeesService.list()

  expect(employees).toHaveLength(1)
  expect(employees[0].name).toBe('Employee A')
})
```

**Pessimistic Locking (Race Conditions):**
```typescript
// ✅ CORRECT - Test concurrent updates
it('should prevent race conditions with pessimistic locking', async () => {
  const employee = await createEmployee({ availableMargin: 1000 })

  const updates = Promise.all([
    employeesService.updateMargin(employee.id, -500),
    employeesService.updateMargin(employee.id, -600)
  ])

  await expect(updates).rejects.toThrow() // One should fail

  const updated = await getEmployee(employee.id)
  expect(updated.availableMargin).toBeGreaterThanOrEqual(0)
})
```

**Token Atomic Validation:**
```typescript
// ✅ CORRECT - Prevent double-spend
it('should prevent double-spend of tokens', async () => {
  const token = await generateToken(employeeId)

  const firstValidation = tokensService.validateAndConsume(token.code, employeeId)
  const secondValidation = tokensService.validateAndConsume(token.code, employeeId)

  await expect(firstValidation).resolves.toBe(true)
  await expect(secondValidation).rejects.toThrow('Token inválido')
})
```

#### Mock Patterns

**Database:** Use in-memory or test database (NOT production)

**External Services:**
```typescript
// ✅ CORRECT - Mock Clerk
jest.mock('@clerk/clerk-sdk-node', () => ({
  ClerkExpressRequireAuth: jest.fn(() => (req, res, next) => {
    req.auth = { userId: 'test_user', orgId: 'test_org' }
    next()
  })
}))
```

**Cleanup:**
```typescript
// ✅ CORRECT - Always cleanup
afterEach(async () => {
  await db.delete(employees)
  await redis.flushdb()
})
```

#### Coverage Requirements

**Mandatory 100% Coverage:**
- Financial operations (margin updates, loans)
- Token system (generation, validation, cancellation)
- Multi-tenancy filters

**Standard Coverage:**
- Services: 80%+
- Controllers/Routers: 70%+
- UI Components: 60%+ (focus on logic)
- Banking ACL adapters: 90%+

#### Test Data Factories

```typescript
// ✅ CORRECT - Factory pattern
const createEmployee = (overrides = {}) => ({
  id: cuid(),
  tenantId: 'default_tenant',
  cpf: '12345678901',
  name: 'Test Employee',
  grossSalary: 500000, // centavos
  availableMargin: 150000,
  ...overrides
})
```

---

_Will continue with Code Quality & Style Rules next_

### Code Quality & Style Rules

#### Naming Conventions

**Database (snake_case):**
- **Tables:** `tenants`, `employees`, `loans` (plural, snake_case)
- **Columns:** `tenant_id`, `created_at`, `available_margin`
- **Indexes:** `{table}_{column(s)}_{tipo}_idx`
  - Examples: `employees_tenant_id_idx`, `employees_tenant_cpf_unique_idx`

**TypeScript/JavaScript:**
- **Variables:** `camelCase` → `availableMargin`, `employeeId`
- **Constants:** `SCREAMING_SNAKE_CASE` → `MAX_TOKEN_ATTEMPTS`
- **Functions:** `camelCase` with verb → `calculateMargin()`, `validateToken()`
- **Classes:** `PascalCase` → `MarginCalculator`, `TokenValidator`
- **Components:** `PascalCase.tsx` → `EmployeeCard.tsx`
- **Hooks:** `use{Nome}.ts` → `useEmployeeMargin.ts`
- **Services:** `{domain}.service.ts` → `auth.service.ts`

**Critical Mapping (snake_case ↔ camelCase):**
```typescript
// ✅ CORRECT - Drizzle automatically maps
export const employees = pgTable('employees', {
  availableMargin: integer('available_margin').notNull(), // DB: snake_case
})

// Usage - TypeScript camelCase
const margin = employee.availableMargin
```

#### File and Folder Structure

**Backend Modules:**
- `apps/api/src/modules/{domain}/`
- Must have: `{domain}.service.ts`, `{domain}.router.ts`, `{domain}.schema.ts`, `__tests__/`

**Frontend Features:**
- `apps/web/src/features/{feature}/`
- Subfolders: `components/`, `pages/`, `hooks/`, `utils/`

**Module Boundaries (CRITICAL ⚠️):**
```typescript
// ❌ FORBIDDEN - Direct cross-module imports
import { LoanService } from '../loans/loans.service'

// ✅ ALLOWED - Via dependency injection or events
@Injectable()
export class EmployeesService {
  constructor(private readonly eventBus: EventBus) {}
}
```

#### Import Order

```typescript
// 1. Node.js built-ins
import { createHash } from 'crypto'

// 2. External packages
import { z } from 'zod'

// 3. Monorepo aliases
import { db } from '@fastconsig/database'
import { CreateLoanSchema } from '@fastconsig/shared/schemas'

// 4. Relative imports
import { helper } from './utils'
```

#### Documentation Requirements

**Comments - Use Sparingly:**
- Write self-documenting code
- Add comments ONLY for:
  - Complex business logic
  - Security-critical sections
  - Performance optimizations (explain WHY)

```typescript
// ✅ GOOD - Explains WHY
// Pessimistic locking prevents race conditions in concurrent margin updates (ADR-005)
const [employee] = await tx.select().from(employees).where(eq(employees.id, id)).for('update')

// ❌ BAD - Just repeats code
// Select employee from database
const employee = await db.select().from(employees)
```

**JSDoc - Required for:**
- Public API functions (exported from packages)
- Complex utilities
- Banking ACL interfaces

#### ESLint Rules (Enforced)

```json
{
  "rules": {
    "no-restricted-imports": ["error", {
      "patterns": [{
        "group": ["../../*"],
        "message": "Use @fastconsig/* aliases"
      }]
    }],
    "@typescript-eslint/no-explicit-any": "error",
    "@typescript-eslint/no-unused-vars": ["error", { "argsIgnorePattern": "^_" }]
  }
}
```

#### Anti-Patterns to Avoid

```typescript
// ❌ NEVER - Float for money
const amount = 50.00

// ❌ NEVER - Query without tenant filter
db.select().from(employees)

// ❌ NEVER - Non-atomic token operations
const token = await redis.get(key)
await redis.del(key) // Race condition!

// ❌ NEVER - N+1 queries
for (const employee of employees) {
  const loans = await db.select().from(loans).where(eq(loans.employeeId, employee.id))
}
```

---

_Will continue with Development Workflow Rules next_

### Development Workflow Rules

#### Git/Repository Rules

**Commit Message Format:**
- **NO emojis** in commit messages (per CLAUDE.md)
- **NO signatures** in commits
- Use descriptive, concise messages in Portuguese
- Pattern (optional): `tipo: descrição`

```bash
# ✅ CORRECT
git commit -m "feat: adiciona validação de token com Redis atomic operations"
git commit -m "fix: corrige race condition em atualização de margem"

# ❌ NEVER
git commit -m "✨ feat: adiciona validação 🎉" # No emojis
git commit -m "feat: add feature

Signed-off-by: Dev <dev@email.com>" # No signatures
```

**Branch Naming:**
- Pattern: `{tipo}/{descricao-curta}`
- Types: `feature`, `bugfix`, `hotfix`, `refactor`

```bash
# ✅ CORRECT
git checkout -b feature/token-validation-atomic
git checkout -b bugfix/margin-calculation-negative
```

#### Monorepo Development Workflow

**Development Commands:**
```bash
# Start all services concurrently
pnpm dev

# Individual services
pnpm --filter api dev       # NestJS (port 3000)
pnpm --filter web dev       # React (port 5173)
pnpm --filter jobs dev      # BullMQ workers

# Quality checks
pnpm typecheck              # All packages
pnpm lint                   # ESLint
pnpm test                   # All tests
```

**Database Workflow:**
```bash
pnpm --filter database db:generate  # Generate migration
pnpm --filter database db:push      # Apply (dev)
pnpm --filter database db:studio    # Open Drizzle Studio
```

**Build Process:**
```bash
pnpm build                          # All packages (Turborepo)
pnpm --filter @fastconsig/database build  # Specific package
```

#### Code Review Checklist

**Before Creating PR:**
- [ ] Tests passing (`pnpm test`)
- [ ] Type checking passes (`pnpm typecheck`)
- [ ] ESLint passes (`pnpm lint`)
- [ ] Multi-tenancy tests included (if applicable)
- [ ] Pessimistic locking for margin/tokens (if applicable)
- [ ] No emojis/signatures in commits

**PR Description Must Include:**
- Summary of changes
- Related epic/story reference
- Testing approach
- Database migration impact (if any)

#### Deployment Rules

**Environment Variables:**
- NEVER commit `.env` files
- Use `.env.example` as template
- Oracle Cloud Secrets Manager for production

**Database Migrations:**
- ALWAYS test migrations on staging first
- NEVER modify existing migrations
- Create new migration for schema changes

**Hot Reload (Monorepo):**
- Changes in `packages/database` → rebuilds `api` + `jobs`
- Changes in `packages/shared` → rebuilds all apps
- Changes in `packages/ui` → rebuilds `web`

---

### Critical Don't-Miss Rules ⚠️

#### Multi-Tenancy Security (NON-NEGOTIABLE)

**SEMPRE incluir `tenant_id` em queries:**
```typescript
// ❌ NUNCA - Vaza dados cross-tenant
async list() {
  return this.db.select().from(employees) // ❌ SEM WHERE tenant_id
}

// ✅ CORRETO - Filtro obrigatório
async list() {
  const { tenantId } = this.cls.getStore()!
  return this.db.select()
    .from(employees)
    .where(eq(employees.tenantId, tenantId))
}
```

**SEMPRE usar Context Middleware:**
- NUNCA passar `tenantId` manualmente em cada query
- Context Middleware DEVE injetar no AsyncLocalStorage
- Queries DEVEM ler de `this.cls.getStore()!`

#### Financial Operations Edge Cases

**Pessimistic Locking (MANDATORY):**
```typescript
// ❌ NUNCA - Race condition em margem
const employee = await db.select().from(employees).where(eq(employees.id, id))
await db.update(employees).set({ availableMargin: newMargin })

// ✅ CORRETO - Lock antes de update
await db.transaction(async (tx) => {
  const [employee] = await tx.select()
    .from(employees)
    .where(eq(employees.id, id))
    .for('update') // ⚠️ OBRIGATÓRIO

  await tx.update(employees)
    .set({ availableMargin: employee.availableMargin - loanValue })
    .where(eq(employees.id, id))
})
```

**SEMPRE validar margem negativa:**
```typescript
// ✅ CORRETO - Validação antes de commit
if (employee.availableMargin < installmentValue) {
  throw new TRPCError({
    code: 'BAD_REQUEST',
    message: 'Margem insuficiente',
    cause: { available: employee.availableMargin, required: installmentValue }
  })
}
```

#### Token System Security

**Atomic Redis Operations:**
```typescript
// ❌ NUNCA - Race condition (double-spend)
const metadata = await redis.get(key)
if (metadata) {
  await redis.del(key) // ❌ Outro request pode validar entre GET e DEL
}

// ✅ CORRETO - Operação atômica
const metadata = await redis.getDel(key) // ✅ Atomic get + delete
if (!metadata) throw new TRPCError({ code: 'UNAUTHORIZED' })
```

**Hashing Obrigatório:**
```typescript
// ❌ NUNCA - Token em plaintext
await db.insert(tokens).values({ code: '123456' }) // ❌ SECURITY RISK

// ✅ CORRETO - SHA-256 hash
const hash = createHash('sha256').update(code).digest('hex')
await db.insert(tokens).values({ codeHash: hash })
```

**Rate Limiting:**
- Max 3 tokens por CPF a cada 10 minutos
- Max 100 tokens por IP a cada minuto (global)
- SEMPRE verificar antes de gerar novo token

#### Banking ACL Isolation

**NUNCA misturar parsing CNAB com domínio:**
```typescript
// ❌ NUNCA - Parsing no service de domínio
export class LoansService {
  async importCNAB(file: Buffer) {
    const lines = file.toString('latin1').split('\n') // ❌ CONTAMINA DOMÍNIO
    // ...
  }
}

// ✅ CORRETO - Usar adapter da camada ACL
export class LoansService {
  async importCNAB(file: Buffer) {
    const adapter = this.adapterRegistry.get('banco-brasil', 'cnab240', 'v1')
    const loans = await adapter.parse(file) // ✅ DTOs limpos
    // ...
  }
}
```

**Adapters DEVEM retornar DTOs limpos:**
- NUNCA retornar strings posicionais
- NUNCA incluir lógica de negócio no adapter
- SEMPRE validar schema antes de retornar

#### Audit Trail Integrity

**Append-only (NUNCA modificar):**
```typescript
// ❌ NUNCA - UPDATE/DELETE em audit_trails
await db.update(auditTrails).set({ reviewed: true })
await db.delete(auditTrails).where(lt(auditTrails.createdAt, oneYearAgo))

// ✅ CORRETO - Apenas INSERT
await db.insert(auditTrails).values({
  userId,
  tenantId,
  action: 'MARGIN_UPDATE',
  resourceId: employeeId,
  oldValue: JSON.stringify({ margin: 1000 }),
  newValue: JSON.stringify({ margin: 500 }),
  ipAddress,
  userAgent
})
```

#### Performance Gotchas

**Evitar N+1 queries:**
```typescript
// ❌ NUNCA - N+1 query
for (const employee of employees) {
  const loans = await db.select()
    .from(loans)
    .where(eq(loans.employeeId, employee.id))
}

// ✅ CORRETO - Query única com JOIN
const employeesWithLoans = await db.select()
  .from(employees)
  .leftJoin(loans, eq(loans.employeeId, employees.id))
  .where(eq(employees.tenantId, tenantId))
```

**Indexes compostos começam com tenant_id:**
```sql
-- ❌ NUNCA
CREATE INDEX idx_employees_cpf ON employees(cpf);

-- ✅ CORRETO - tenant_id primeiro
CREATE INDEX idx_employees_tenant_cpf ON employees(tenant_id, cpf);
```

#### Drizzle ORM Gotchas

**Versão v0.36+ é OBRIGATÓRIA:**
- Versões anteriores NÃO suportam `.for('update')` nativamente
- Sistema de tokens e margem DEPENDE de pessimistic locking
- SEMPRE verificar versão no package.json

**drizzle-zod para schemas:**
```typescript
// ✅ CORRETO - Inferir Zod de Drizzle
import { createInsertSchema } from 'drizzle-zod'
import { employees } from '@fastconsig/database/schema'

export const InsertEmployeeSchema = createInsertSchema(employees)

// ❌ EVITAR - Definir schema manualmente
export const InsertEmployeeSchema = z.object({
  cpf: z.string().length(11),
  // ... duplicação desnecessária
})
```

#### Domain Events Gotchas

**Handlers DEVEM ser idempotentes:**
```typescript
// ✅ CORRETO - Handler idempotente
async onLoanCreated(event: LoanCreatedEvent) {
  // Verificar se já processou este evento
  const existing = await db.select()
    .from(processedEvents)
    .where(eq(processedEvents.eventId, event.eventId))

  if (existing.length > 0) return // ✅ Já processado

  // Processar recálculo de margem...
  await this.marginCalculator.recalculate(event.employeeId)

  // Marcar como processado
  await db.insert(processedEvents).values({ eventId: event.eventId })
}
```

**SEMPRE propagar correlation ID:**
```typescript
// ✅ CORRETO - Correlation ID em todos os logs e eventos
const correlationId = req.headers['x-correlation-id'] || cuid()
logger.info({ correlationId, action: 'loan_created' })
await eventBus.publish(new LoanCreatedEvent({
  ...data,
  metadata: { correlationId }
}))
```

---

## Usage Guidelines

### For AI Agents

**Before implementing ANY code:**
1. Read this entire file to understand project-specific constraints
2. Follow ALL rules exactly as documented - they exist to prevent production issues
3. When in doubt between approaches, prefer the MORE restrictive option (security-first)
4. If a new pattern emerges during implementation, update this file immediately

**Critical Priorities:**
- Multi-tenancy filtering is NON-NEGOTIABLE (tenant_id on ALL queries)
- Financial operations MUST use pessimistic locking (race conditions = money loss)
- Token validation MUST be atomic (double-spend prevention)
- NEVER use floats for monetary values (precision loss = accounting errors)

### For Human Developers

**Maintenance:**
- Review this file quarterly to remove rules that have become obvious
- Update when technology stack versions change (especially Drizzle, Node, React)
- Add new patterns discovered during code reviews or post-mortems
- Keep file lean - remove redundant or obvious information

**When onboarding new AI agents:**
- Point them to this file before their first task
- Use this as the source of truth for project conventions
- Update based on feedback from agents who miss critical details

---

**Last Updated:** 2026-01-11
**Project:** Fast Consig
**Version:** 1.0 - Initial Complete Context
