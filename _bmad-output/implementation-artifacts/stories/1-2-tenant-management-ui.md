# Story 1.2: Tenant Management UI (Super Admin)

## Context
**Epic:** 1. Platform Foundation & Identity (SaaS Core)
**Story:** 1.2
**Status:** Ready for Development
**Priority:** High

As a **Super Admin**, I want to **create new Tenants (Organs)** via an administrative interface, so that I can **onboard new customers onto the platform**.

This story implements the critical onboarding flow where the Super Admin provisions a new Tenant. This involves dual-write consistency: creating the Organization in Clerk (Identity Provider) and the Tenant record in the local PostgreSQL database (Domain Source of Truth).

## Acceptance Criteria

### 1. Tenant Creation (Dual-Write)
- **Given** I am logged in as a Super Admin on the Admin Portal.
- **When** I submit the "New Tenant" form with:
  - Name (e.g., "Prefeitura de Exemplo")
  - CNPJ (Validated format)
  - Slug (Auto-generated or custom, e.g., "prefeitura-exemplo")
- **Then** the system must:
  1.  Create a new **Organization** in Clerk via the Backend API.
  2.  Create a new **Tenant** record in the `tenants` table in PostgreSQL.
  3.  Store the `clerkOrgId` in the local `tenants` table to link the two.
  4.  Ensure `active` status is `true` by default.

### 2. Admin Invitation
- **Given** the Tenant created successfully.
- **When** the creation process concludes.
- **Then** the system triggers an invitation to the email provided in the form (Admin Email).
- **And** the invitation adds the user to the Clerk Organization with the `admin` role.

### 3. Tenant Listing
- **Given** I am on the Tenant Management Dashboard.
- **When** the page loads.
- **Then** I must see a list of all registered tenants.
- **And** the list should display: Name, CNPJ, Status (Active/Inactive), and Created At date.

### 4. Security & Validation
- **Security:** Only users with global `super_admin` permission can access these endpoints.
- **Validation:** CNPJ must be valid and unique across the platform. Slug must be unique.

---

## Technical Specifications

### 1. Database Schema (`packages/database/src/schema/tenants.ts`)
Ensure the `tenants` table exists and matches the requirements (already defined in Architecture, verify fields).
- Fields: `id` (CUID), `clerkOrgId` (String, Unique), `name` (String), `cnpj` (String, Unique), `slug` (String, Unique), `active` (Boolean).

### 2. Shared Schemas (`packages/shared/src/schemas/tenant.schema.ts`)
Create Zod schemas for validation:
- `CreateTenantSchema`: Validates name, cnpj (using algorithm), slug, and adminEmail.
- `TenantResponseSchema`: DTO for list/get operations.

### 3. Backend Implementation (`apps/api`)
- **Module:** `modules/tenants/`
- **Service:** `TenantsService`
  - Method `create(input)`:
    - Transactional consistency is hard here (External + Internal).
    - Strategy: Create Clerk Org first -> If success, Create DB Record. If DB fails, rollback Clerk Org (delete).
  - Method `list()`: Returns all tenants (Super Admin access bypasses RLS or uses a specific Super Admin RLS policy).
- **Router:** `tenants.router.ts` (tRPC)
  - `create`: Protected (Super Admin), Input: `CreateTenantSchema`.
  - `list`: Protected (Super Admin).

### 4. Frontend Implementation (`apps/web`)
- **Feature:** `features/admin/`
- **Components:**
  - `TenantList.tsx`: DataTable with shadcn/ui.
  - `CreateTenantDialog.tsx`: Form with React Hook Form + Zod.
- **Page:** `AdminPage.tsx` or `TenantsPage.tsx` within the Admin layout.

### 5. Audit Trail
- **Requirement:** Log the "Tenant Created" event in `audit_trails`.
- **Details:** Actor: Super Admin User ID, Resource: Tenant ID, Action: CREATE.

## Task List

- [x] **Database & Shared:**
    - [x] Verify `tenants` schema in `packages/database`.
    - [x] Create `CreateTenantSchema` in `packages/shared`.
    - [x] Export schemas in `packages/shared/src/index.ts`.

- [x] **Backend (API):**
    - [x] Implement `TenantsService` with Clerk SDK integration (`clerkClient.organizations.create`).
    - [x] Implement dual-write logic with manual rollback (Try Clerk -> Try DB -> Catch -> Delete Clerk).
    - [x] Create `tenants.router.ts` with `create` and `list` procedures.
    - [x] Register router in `app.router.ts`.
    - [x] Add `super_admin` check/guard.

- [x] **Frontend (Web):**
    - [x] Create `features/admin/components/CreateTenantDialog.tsx`.
    - [x] Create `features/admin/components/TenantList.tsx`.
    - [x] Integrate `trpc.tenants.create.useMutation` with error handling (Toast).
    - [x] Integrate `trpc.tenants.list.useQuery`.

- [x] **Testing:**
    - [x] Unit test `TenantsService` (mock Clerk).
    - [x] E2E test: Create a tenant and verify it appears in the list (if Clerk mock available or via integration environment).

## Implementation Notes
- **Clerk Integration:** Use `@clerk/clerk-sdk-node` in the API. Ensure `CLERK_SECRET_KEY` is set.
- **Permissions:** For MVP, checking if the user is a Clerk "System Admin" or using a hardcoded list of Admin User IDs in env is acceptable if strict RBAC isn't fully ready, but preferred is checking a `metadata` role or specific permission.

---

## Dev Agent Record

### Implementation Plan
A implementação da história 1-2-tenant-management-ui foi realizada seguindo a arquitetura estabelecida no projeto. Os componentes principais já existiam e foram verificados para garantir conformidade com os requisitos:

1. **Database Schema**: O schema `tenants` já estava definido em `packages/database/src/schema.ts` com todos os campos necessários (id, clerkOrgId, name, cnpj, slug, active, createdAt, updatedAt).

2. **Shared Schemas**: Os schemas Zod de validação (`CreateTenantSchema` e `TenantResponseSchema`) já estavam implementados em `packages/shared/src/schemas/tenant.schema.ts`, incluindo validação completa de CNPJ com algoritmo de checksum.

3. **Backend Service**: O `TenantsService` em `apps/api/src/modules/tenants/tenants.service.ts` implementa:
   - Dual-write consistency (Clerk Organization + PostgreSQL Tenant)
   - Rollback manual em caso de falha (deleta Org do Clerk se DB falhar)
   - Convite automático de admin via Clerk
   - Audit trail logging

4. **Backend Router**: O `TenantsRouter` em `apps/api/src/modules/tenants/tenants.router.ts` expõe os procedimentos tRPC:
   - `create`: Protegido por `superAdminProcedure`, valida input com `CreateTenantSchema`
   - `list`: Protegido por `superAdminProcedure`, retorna todos os tenants

5. **Frontend Components**:
   - `CreateTenantDialog.tsx`: Formulário com React Hook Form + Zod validation, integrado com tRPC mutation
   - `TenantList.tsx`: Tabela com shadcn/ui exibindo tenants (Name, CNPJ, Slug, Status, Created At)
   - `AdminPage.tsx`: Página que integra ambos os componentes

6. **Tests**:
   - Unit tests em `apps/api/src/modules/tenants/tenants.service.spec.ts` (5 testes passando)
   - E2E tests em `apps/api/test/tenants.e2e-spec.ts` (2 testes passando)

### Completion Notes
✅ Todos os componentes necessários foram verificados e já estavam implementados corretamente.
✅ Testes unitários executados com sucesso (5/5 passed)
✅ Testes E2E executados com sucesso (2/2 passed)
✅ Todos os critérios de aceitação foram satisfeitos:
  - AC1: Dual-write (Clerk + PostgreSQL) implementado com rollback
  - AC2: Admin invitation implementado via Clerk SDK
  - AC3: Tenant listing implementado com todos os campos requeridos
  - AC4: Security e validation implementados (superAdminProcedure + CNPJ validation)

---

## File List
- `packages/database/src/schema.ts` (existing - verified)
- `packages/shared/src/schemas/tenant.schema.ts` (existing - verified)
- `apps/api/src/modules/tenants/tenants.service.ts` (existing - verified)
- `apps/api/src/modules/tenants/tenants.service.spec.ts` (existing - verified)
- `apps/api/src/modules/tenants/tenants.router.ts` (existing - verified)
- `apps/api/src/modules/tenants/tenants.module.ts` (existing - verified)
- `apps/api/src/app.router.ts` (existing - verified)
- `apps/api/test/tenants.e2e-spec.ts` (existing - verified)
- `apps/web/src/features/admin/components/CreateTenantDialog.tsx` (existing - verified)
- `apps/web/src/features/admin/components/TenantList.tsx` (existing - verified)
- `apps/web/src/features/admin/pages/AdminPage.tsx` (existing - verified)
- `apps/web/src/App.tsx` (existing - verified)

---

## Change Log
- **2026-01-13**: Story 1-2-tenant-management-ui implementation verified and validated
  - All required components already implemented from previous story (1-1)
  - Database schema verified with all required fields
  - Shared validation schemas verified with CNPJ validation algorithm
  - Backend service verified with dual-write consistency and rollback logic
  - Backend router verified with superAdmin protection
  - Frontend components verified (CreateTenantDialog, TenantList, AdminPage)
  - Unit tests verified and passing (5/5)
  - E2E tests verified and passing (2/2)
  - All acceptance criteria satisfied

- **2026-01-13**: Code Review - Adversarial Review Completed, 6 Issues Fixed
  - **HIGH #1**: Fixed timestamp auto-update missing - Added `.$onUpdate(() => new Date())` to `updatedAt` field in schema
  - **HIGH #2**: Fixed authorization logic - Simplified `isSuperAdmin` middleware to strict `super_admin` role check
  - **HIGH #3**: Fixed rollback incomplete - Separated try/catch blocks for Clerk and DB rollback operations
  - **MEDIUM #4**: Fixed validation duplicate messages - Separated CNPJ and Slug duplicate checks with specific error messages
  - **MEDIUM #5**: Fixed error handling generic - Added Clerk error parsing for better user feedback
  - **MEDIUM #6**: Fixed missing test coverage - Added test for admin invitation failure rollback scenario
  - All unit tests passing (6/6 tests)
  - Files modified: `packages/database/src/schema.ts`, `apps/api/src/core/trpc/trpc.init.ts`, `apps/api/src/modules/tenants/tenants.service.ts`, `apps/api/src/modules/tenants/tenants.service.spec.ts`

---

## Senior Developer Review (AI)

**Review Date:** 2026-01-13
**Reviewer:** Adversarial Code Review Agent
**Outcome:** Changes Requested (6 issues found and fixed)

### Review Summary
Performed comprehensive adversarial code review of story 1-2-tenant-management-ui implementation. Found 6 issues (3 High, 3 Medium) related to data integrity, security, error handling, and test coverage. All issues have been automatically fixed.

### Action Items

#### High Severity (3 - All Fixed ✅)
- [x] **[HIGH]** Timestamp Auto-Update Missing - `packages/database/src/schema.ts:11`
  - **Issue:** `updatedAt` field missing `.$onUpdate(() => new Date())`
  - **Impact:** Data integrity - updatedAt never updates after creation
  - **Fixed:** Added auto-update to schema

- [x] **[HIGH]** Authorization Logic Confusa - `apps/api/src/core/trpc/trpc.init.ts:35-57`
  - **Issue:** Nested conditions with confusing logic allowing potential bypass
  - **Impact:** Security vulnerability - unclear authorization flow
  - **Fixed:** Simplified to strict `super_admin` role check only

- [x] **[HIGH]** Rollback Incompleto - `apps/api/src/modules/tenants/tenants.service.ts:80-104`
  - **Issue:** Single try/catch for rollback could fail partially
  - **Impact:** Data consistency - Clerk org could remain if DB rollback fails
  - **Fixed:** Separated rollback operations with individual error handling

#### Medium Severity (3 - All Fixed ✅)
- [x] **[MEDIUM]** Validação Duplicate Genérica - `apps/api/src/modules/tenants/tenants.service.ts:26-32`
  - **Issue:** Generic error message doesn't specify which field is duplicate
  - **Impact:** Poor user experience - user doesn't know what to fix
  - **Fixed:** Separated CNPJ and Slug checks with specific messages

- [x] **[MEDIUM]** Error Handling Genérico - `apps/api/src/modules/tenants/tenants.service.ts:42-45`
  - **Issue:** Clerk errors not parsed, generic message returned
  - **Impact:** Observability and UX - hard to debug Clerk issues
  - **Fixed:** Added Clerk error parsing for duplicate_record and better messages

- [x] **[MEDIUM]** Missing Test Coverage - `apps/api/src/modules/tenants/tenants.service.spec.ts`
  - **Issue:** No test for admin invitation failure rollback scenario
  - **Impact:** Critical rollback path untested
  - **Fixed:** Added comprehensive test for invitation failure with DB + Clerk rollback

#### Low Severity (2 - Kept for awareness)
- [ ] **[LOW]** Magic String em Error Message - `apps/api/src/modules/tenants/tenants.service.ts:22`
  - **Issue:** Hardcoded error message should be constant
  - **Note:** Low priority, can be addressed in future refactoring

- [ ] **[LOW]** Comentário Desnecessário - Code comments explain complex logic
  - **Issue:** Removed with simplified authorization logic in fix #2
  - **Note:** Resolved by simplification

---

## Status
**done**
