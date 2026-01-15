---
stepsCompleted:
  - step-01-validate-prerequisites
  - step-02-design-epics
  - step-03-create-stories
inputDocuments:
  - _bmad-output/planning-artifacts/prd.md
  - product-development/current-feature/arquitetura-tecnica.md
  - product-development/current-feature/wireframes.md
---

# Fast Consig - Epic Breakdown

## Overview

This document provides the complete epic and story breakdown for Fast Consig, decomposing the requirements from the PRD, UX Design if it exists, and Architecture requirements into implementable stories.

## Requirements Inventory

### Functional Requirements

- FR01: O sistema deve permitir que Órgãos (Tenants) façam login via SSO ou Email/Senha (gerenciado pelo Clerk) com MFA obrigatório para escrita financeira.
- FR02: O Super Admin deve conseguir criar novos Tenants (Órgãos) e configurar seus administradores iniciais.
- FR03: O Gestor de RH deve conseguir criar contas para Operadores de RH com permissões restritas (ex: apenas leitura).
- FR04: O Sistema deve bloquear tentativas de login suspeitas baseadas em IP ou comportamento anômalo (via Clerk).
- FR05: O Gestor de RH deve conseguir cadastrar novos funcionários manualmente (CRUD) e importar em lote via arquivo CSV/Excel padronizado.
- FR06: O Sistema deve calcular automaticamente a Margem Disponível com base nas regras do Tenant.
- FR07: O Sistema deve bloquear a inativação de funcionários que possuam empréstimos ativos com saldo devedor.
- FR08: O Operador Bancário deve conseguir consultar a margem de um funcionário fornecendo CPF e Matrícula exatos (Blind Query).
- FR09: O Sistema deve gerar um Token de Reserva (TTL configurável) e enviá-lo ao funcionário via SMS/Email/Webhook mediante solicitação.
- FR10: O Operador Bancário deve conseguir averbar um contrato (efetivar empréstimo) informando o Token válido.
- FR11: O Sistema deve garantir que a soma das parcelas averbadas nunca exceda a margem consignável configurada para o órgão.
- FR12: O Operador Bancário deve conseguir fazer upload de arquivos de lote (Excel/CNAB) para averbação em massa.
- FR13: O Sistema deve processar arquivos de lote assincronamente e notificar o operador sobre o resultado (Sucesso/Falha linha a linha).
- FR14: O Sistema deve permitir o download de arquivos de retorno processados contendo o status de cada operação.
- FR15: O Sistema deve registrar um log imutável para toda alteração de margem, contendo Usuário, IP, Data e Valores (Antes/Depois).
- FR16: O Gestor de RH deve conseguir extrair um relatório de auditoria filtrado por funcionário ou período e permitir anonimização (LGPD).
- FR17: O Sistema deve suportar múltiplos layouts de arquivo (mapeamento configurável) para diferentes bancos.
- FR18: O Sistema deve processar arquivos de folha (10k linhas) em < 10 minutos. (Implicit in FR2)
- FR19: O Gestor de RH deve conseguir gerar o extrato de consignações para a folha.
- FR20: O Sistema deve destacar divergências de valores entre averbado e descontado.
- FR21: O Gestor de RH deve visualizar dashboard consolidado de repasses.
- FR25: O Gestor de RH deve conseguir cancelar ou reenviar um Token de Reserva ativo.
- FR26: O Super Admin deve conseguir parametrizar as regras de cálculo (Limites %, Tipos de Margem) específicas para cada Tenant via interface administrativa.
- FR27: O processamento de arquivos em lote deve suportar "Sucesso Parcial", efetivando as linhas válidas e gerando um arquivo de rejeições contendo apenas as linhas inválidas com o motivo do erro.
- FR28: O Sistema deve emitir alertas automáticos para comportamentos anômalos de segurança (ex: múltiplas consultas de margem para o mesmo CPF em segundos vindo de IPs diferentes).
- FR29: O Sistema deve notificar proativamente o Gestor de RH sobre anomalias de negócio (Push Intelligence), como descontos não processados ou margens negativas detectadas na folha.

### NonFunctional Requirements

- NFR01: O endpoint de consulta de margem deve responder em < 200ms (p95).
- NFR02: O processamento de arquivos de folha/averbação (até 10k registros) deve ser concluído em < 10 minutos.
- NFR03: Disponibilidade de 99.9% durante horário comercial (8h-18h).
- NFR04: RPO de 5 minutos e RTO de 4 horas para falhas catastróficas.
- NFR05: Logs de auditoria devem ser retidos por 5 anos (Cold Storage).
- NFR06: Criptografia em repouso (AES-256) e TLS 1.3 em trânsito.
- NFR07: O Portal do Gestor RH deve atender WCAG 2.1 AA (Acessibilidade).
- NFR08: Suporte a 1.000 usuários simultâneos no Portal RH.

### Additional Requirements

From Architecture:
- Tech Stack: Node.js 22+, NestJS/Fastify, PostgreSQL 16+, React 19+, TypeScript 5.5+.
- Architecture Pattern: Hexagonal Architecture (Ports & Adapters).
- Multi-Tenancy Strategy: Row-Level Security (RLS) + Tenant ID column in all tables.
- Auth Strategy: JWT (handled by Clerk integration).
- Async Processing: BullMQ/Hangfire for jobs (imports, webhooks).
- Security: Headers (HSTS, CSP), Rate Limiting.
- Containerization: Docker, Kubernetes readiness.
- CI/CD: GitHub Actions pipeline.

From UX Design:
- Design System: Tailwind CSS 4+ with shadcn/ui components.
- Responsive Design: Mobile-first approach, specific breakpoints (sm, md, lg, xl).
- Layout: Dashboard layout with Sidebar (collapsible) and Header.
- Accessibility: Focus on contrast, keyboard navigation (Radix UI base).
- Specific UI Patterns:
    - Wizard for "Nova Averbacao" (3 steps).
    - DataTables with sorting, filtering, pagination.
    - KPI Cards and Charts for Dashboards.

### FR Coverage Map

- FR01: Epic 1 - Platform Foundation & Identity (SaaS Core)
- FR02: Epic 1 - Platform Foundation & Identity (SaaS Core)
- FR03: Epic 1 - Platform Foundation & Identity (SaaS Core)
- FR04: Epic 1 - Platform Foundation & Identity (SaaS Core)
- FR05: Epic 2 - Employee Management (People Domain)
- FR06: Epic 2 - Employee Management (People Domain)
- FR07: Epic 2 - Employee Management (People Domain)
- FR08: Epic 4 - Loan Operations (Manual & Portal)
- FR09: Epic 3 - Transactional Integrity (Token System)
- FR10: Epic 4 - Loan Operations (Manual & Portal)
- FR11: Epic 3 - Transactional Integrity (Token System)
- FR12: Epic 5 - Bulk Banking Integration (CNAB Engine)
- FR13: Epic 5 - Bulk Banking Integration (CNAB Engine)
- FR14: Epic 5 - Bulk Banking Integration (CNAB Engine)
- FR15: Epic 1 - Platform Foundation & Identity (SaaS Core)
- FR16: Epic 6 - Payroll Reconciliation (The Closer)
- FR17: Epic 5 - Bulk Banking Integration (CNAB Engine)
- FR19: Epic 6 - Payroll Reconciliation (The Closer)
- FR20: Epic 6 - Payroll Reconciliation (The Closer)
- FR21: Epic 6 - Payroll Reconciliation (The Closer)
- FR25: Epic 3 - Transactional Integrity (Token System)
- FR26: Epic 1 - Platform Foundation & Identity (SaaS Core)
- FR27: Epic 5 - Bulk Banking Integration (CNAB Engine)
- FR28: Epic 1 - Platform Foundation & Identity (SaaS Core)
- FR29: Epic 6 - Payroll Reconciliation (The Closer)

## Epic List

### Epic 1: Platform Foundation & Identity (SaaS Core)
Establish the multi-tenant secure perimeter, audit infrastructure, and tenant configuration rules necessary for all subsequent operations.
**FRs covered:** FR01, FR02, FR03, FR04, FR26, FR28, FR15 (Infrastructure), NFR03, NFR05, NFR06.

### Epic 2: Employee Management (People Domain)
Enable RH Managers to populate and maintain the employee base with automatic margin calculation based on Tenant rules.
**FRs covered:** FR05, FR06, FR07.

### Epic 3: Transactional Integrity (Token System)
Implement the 2FA Token lifecycle (Generate, Send, Validate, Cancel) to guarantee secure authorization for loans.
**FRs covered:** FR09, FR25, FR11 (Validation Logic).

### Epic 4: Loan Operations (Manual & Portal)
Enable Bank Operators to perform Blind Queries and Register Loans manually using the secure Token validation.
**FRs covered:** FR08, FR10, FR15 (Business Events), NFR01.

### Epic 5: Bulk Banking Integration (CNAB Engine)
Scale the operation by enabling Banks to process high volumes of loans via async file upload with error handling.
**FRs covered:** FR12, FR13, FR14, FR17, FR27, NFR02.

### Epic 6: Payroll Reconciliation (The Closer)
Enable RH Managers to close the month efficiently by generating files and resolving divergences.
**FRs covered:** FR19, FR20, FR21, FR29, FR16 (Reporting).

## Epic 1: Platform Foundation & Identity (SaaS Core)

Establish the multi-tenant secure perimeter, audit infrastructure, and tenant configuration rules necessary for all subsequent operations.

### Story 1.0: Project Foundation Setup (Turborepo Monorepo)

As a Platform Architect,
I want to initialize the project using a Turborepo monorepo structure with pnpm workspaces,
So that I can manage the API, Web, and Workers apps with type-safety and shared code from day one.

**Acceptance Criteria:**

**Given** A new project initialization
**When** I run the foundation setup
**Then** A Turborepo monorepo should be created with apps for `api` (NestJS), `web` (React 19), and `jobs` (BullMQ)
**And** Shared packages for `database` (Drizzle), `shared` (Zod schemas), and `ui` (shadcn) should be configured
**And** Docker Compose should be ready for local PostgreSQL and Redis development
**And** A GitHub Actions CI pipeline should be established with Turborepo caching enabled

### Story 1.1: Multi-tenant Clerk Integration with Context

As a Platform Developer,
I want to integrate Clerk authentication with a global Context Middleware,
So that every request is securely authenticated and automatically scoped to the correct Tenant ID.

**Acceptance Criteria:**

**Given** A user attempts to access a protected route
**When** The request header contains a valid Clerk JWT
**Then** The middleware should decode the token and verify the signature
**And** The middleware should extract `tenant_id` and `user_id` from the token claims
**And** The extracted IDs should be stored in Async Local Storage (ALS) for access by repositories
**And** A generic "Health Check Secure" endpoint (`/api/me`) should return the decoded context for verification

### Story 1.2: Tenant Management UI (Super Admin)

As a Super Admin,
I want to create new Tenants (Organs) via an administrative interface,
So that I can onboard new customers onto the platform.

**Acceptance Criteria:**

**Given** I am logged in as a Super Admin
**When** I submit the "New Tenant" form with Name, Document (CNPJ), and Contact Info
**Then** A new Organization should be created in Clerk via API
**And** A new Tenant record should be created in the local database
**And** The initial Admin User invite should be sent via Clerk
**And** I should see the new Tenant in the list with "Active" status

### Story 1.3: Tenant Configuration Engine

As a Super Admin,
I want to configure business rules (Margin %, Cutoff Date) for a specific Tenant,
So that the system calculates margins correctly according to each Organ's legislation.

**Acceptance Criteria:**

**Given** I am viewing a specific Tenant details
**When** I update the "Margin Rules" (e.g., set Standard Margin to 35% and Benefit Card to 5%)
**Then** The `TenantConfiguration` entity should be updated in the database
**And** An audit log of the configuration change should be recorded
**And** Subsequent margin calculations for this tenant should use the new percentages

### Story 1.4: Audit Infrastructure with Read Access

As a Compliance Officer,
I want the system to automatically log all data mutations and sensitive read access,
So that I can trace any unauthorized activity or data leakage.

**Acceptance Criteria:**

**Given** Any API request that modifies data (POST/PUT/DELETE) OR accesses sensitive data (GET Margin)
**When** The request completes successfully
**Then** A record should be inserted into the `AuditTrails` table (Append-only)
**And** The record must include: Actor ID, Tenant ID, IP Address, Resource Affected, Action Type, and Timestamp
**And** For mutations, the "Old Value" and "New Value" (diff) should be stored in JSON format

### Story 1.6: Security Anomaly Detection (Anti-Fraud)

As a Security Officer,
I want the system to monitor and alert on suspicious behavior (e.g., rapid margin queries for the same CPF from different IPs),
So that I can proactively prevent fraudulent loan applications and enumeration attacks.

**Acceptance Criteria:**

**Given** A sequence of margin query requests for the same CPF
**When** The frequency exceeds the threshold (e.g., >3 queries in 1 minute from different source IPs)
**Then** The system should trigger a "Security Anomaly" event
**And** An alert should be sent to the System Admin and logged in the high-priority audit trail
**And** The source IPs or user accounts should be temporarily throttled/locked depending on the risk score

## Epic 2: Employee Management (People Domain)

Enable RH Managers to populate and maintain the employee base with automatic margin calculation based on Tenant rules.

### Story 2.1: Employee CRUD with RLS & Optimistic Lock

As an RH Manager,
I want to manage employee records with safety against concurrent edits,
So that I can keep the registration data up to date without data loss.

**Acceptance Criteria:**

**Given** I am logged in as Tenant A
**When** I list employees via API
**Then** I should only see employees belonging to Tenant A (RLS Verified)
**And** Tenant B's employees must be invisible even if I try to access by ID
**Given** Two users try to update the same employee simultaneously
**When** The second user submits their change
**Then** The system should detect a version conflict (Optimistic Lock) and reject the update with a 409 error

### Story 2.2: Dynamic Margin Calculation Engine

As an RH Manager,
I want the system to automatically calculate the available margin when I update a salary,
So that the margin information is always accurate according to the configured rules.

**Acceptance Criteria:**

**Given** An employee has a Gross Salary of R$ 5.000,00 and R$ 1.000,00 in mandatory discounts
**When** The Tenant Rule is set to 30% margin on Net Salary
**Then** The system should calculate Available Margin = (5000 - 1000) * 0.30 = R$ 1.200,00
**And** This calculation should happen automatically on Employee Create/Update
**And** The calculated values should be persisted in the `MarginCache` or similar efficient structure

### Story 2.3: Bulk Employee Import (Stream Processing)

As an RH Manager,
I want to upload a CSV file with thousands of employees,
So that I can update the entire payroll base quickly without manual entry.

**Acceptance Criteria:**

**Given** I upload a valid CSV file with 10.000 employee rows
**When** The import process starts
**Then** The system should process the file using Streams (chunk by chunk) to avoid memory overflow
**And** Valid rows should be inserted/updated in the database
**And** Invalid rows (e.g., bad CPF) should be skipped and logged
**And** A final report file containing only the rejected rows and error messages should be generated for download

### Story 2.4: Soft Delete & Safety Block

As an RH Manager,
I want to inactivate employees who leave the Organ, but prevent deletion if they have debts,
So that I maintain historical integrity and prevent financial loss.

**Acceptance Criteria:**

**Given** An employee has an active loan with non-zero balance
**When** I attempt to delete/inactivate this employee
**Then** The system should block the action and return a validation error "Cannot remove employee with active loans"
**Given** An employee has NO active loans
**When** I delete them
**Then** The record should be Soft Deleted (`deleted_at` timestamp set), remaining available for audit but hidden from active lists

## Epic 3: Transactional Integrity (Token System)

Implement the 2FA Token lifecycle (Generate, Send, Validate, Cancel) to guarantee secure authorization for loans.

### Story 3.1: Secure Token Generator Service

As a Backend System,
I want to generate secure, random, short-lived tokens with rate limiting,
So that I can issue authorization codes that are hard to guess and protected against abuse.

**Acceptance Criteria:**

**Given** A token generation request for an employee
**When** The service executes
**Then** A 6-digit cryptographically secure random code should be generated
**And** The token should be stored in the database as a Hash (SHA-256), NOT plain text
**And** A Rate Limit check should enforce max 3 tokens per 10 minutes per CPF
**And** The token expiration should be set to the configured TTL (default 1 hour)

### Story 3.2: Token Delivery System

As an Employee,
I want to receive my authorization token via my registered contact method,
So that I can present it to the bank to authorize my loan.

**Acceptance Criteria:**

**Given** A token has been generated successfully
**When** The delivery process triggers
**Then** The system should retrieve the employee's verified email or phone
**And** Send the plain-text code via the configured provider (SMTP/SMS Gateway)
**And** The message content should include the Code and the Expiration Time
**And** A "Notification Sent" event should be logged in the audit trail

### Story 3.3: Token Lifecycle Management (RH)

As an RH Manager,
I want to view and manage active tokens for my employees,
So that I can help them if they lose a token or suspect fraud.

**Acceptance Criteria:**

**Given** An employee reports a lost or compromised token
**When** I access their token history in the RH Portal
**Then** I should see a list of Active and Expired tokens
**And** I should be able to click "Cancel" on an active token
**And** The token status should update to `CANCELLED` immediately, invalidating it for use
**And** I should optionally be able to trigger a "Resend" of the active token

### Story 3.4: Token Validation & Locking Logic

As a Domain Service,
I want to validate a provided token and atomically lock it,
So that the token cannot be used twice (Double Spend) or used after expiration.

**Acceptance Criteria:**

**Given** A validation request with Code 123456 and EmployeeID
**When** The validation logic runs
**Then** It should verify if a token exists for that Employee with `status=ACTIVE` and `expires_at > now`
**And** It should verify if `Hash(123456)` matches the stored hash
**And** If valid, it should ATOMICALLY update the status to `USED` in the same transaction context
**And** If invalid or expired, it should return a specific error reason

## Epic 4: Loan Operations (Manual & Portal)

Enable Bank Operators to perform Blind Queries and Register Loans manually using the secure Token validation.

### Story 4.1: Blind Margin Query API

As a Bank Operator,
I want to consult the available margin of a servant using their exact credentials,
So that I can offer a loan compatible with their capacity without violating privacy.

**Acceptance Criteria:**

**Given** I send a GET request with `CPF` and `Matricula`
**When** The data matches an active employee exactly
**Then** The API returns the `AvailableMargin` value masked (no other personal details)
**And** An audit log "Margin Consulted" is recorded
**Given** The data does not match or implies fuzzy search
**When** The request is processed
**Then** The API returns a generic 404/403 error to prevent enumeration attacks

### Story 4.2: Manual Averbacao UI

As a Bank Operator,
I want a user-friendly form to enter loan details and the authorization token,
So that I can register a new contract quickly.

**Acceptance Criteria:**

**Given** I am on the "New Loan" screen
**When** I enter the Loan Amount, Installments, Contract Number, and the Token provided by the client
**Then** The frontend should call a pre-validation endpoint to check if the Token is valid (Story 3.4)
**And** If valid, the "Submit" button should become enabled
**And** The form should display a summary of the installments vs margin usage

### Story 4.3: Loan Creation Transaction

As a Backend System,
I want to process the loan creation in a strict ACID transaction,
So that I guarantee the money is reserved and the token is consumed without race conditions.

**Acceptance Criteria:**

**Given** A valid loan submission payload
**When** The transaction starts
**Then** The system should lock the Employee row (SELECT FOR UPDATE)
**And** Validate the Token and mark it USED
**And** Re-calculate margin availability ensuring `Installment <= Available`
**And** Insert the `Loan` record and update the `UsedMargin`
**And** Commit the transaction only if ALL steps succeed

### Story 4.4: Loan Receipt & Evidence

As a Bank Operator,
I want to receive a digital receipt of the successful averbacao,
So that I have proof of the transaction and authorization.

**Acceptance Criteria:**

**Given** A loan has been successfully registered
**When** The response is returned
**Then** It should include a `TransactionID` and a `ReceiptToken`
**And** I should be able to download a PDF receipt containing: Contract Details, Timestamp, User IP, and the Authorization Token used
**And** This receipt serves as the legal non-repudiation evidence

## Epic 5: Bulk Banking Integration (CNAB Engine)

Scale the operation by enabling Banks to process high volumes of loans via async file upload with error handling.

### Story 5.1: Bulk Upload Interface

As a Bank Operator,
I want to upload a standard Excel or CNAB file with multiple loan requests,
So that I can process my daily production in one go.

**Acceptance Criteria:**

**Given** I have a spreadsheet with 500 loan requests
**When** I drag and drop it into the "Bulk Upload" area
**Then** The system should validate the file format and structure immediately
**And** Upload the file to secure storage (S3/MinIO)
**And** Queue a background job for processing
**And** Return a "Job ID" for tracking

### Story 5.2: Async Bulk Processor

As a Background Worker,
I want to process the uploaded file line by line without blocking the main API,
So that large files don't cause timeouts or performance degradation.

**Acceptance Criteria:**

**Given** A queued bulk import job
**When** The worker picks up the job
**Then** It should read the file stream
**And** For each line, execute the "Loan Creation Transaction" logic (Story 4.3)
**And** If a line fails (e.g., Insufficient Margin), it should log the error but CONTINUE to the next line (Partial Success)
**And** Update the Job Status progress (e.g., "Processed 150/500")

### Story 5.3: Bulk Result Report

As a Bank Operator,
I want to download a results file after the bulk processing finishes,
So that I know which loans were accepted and which were rejected.

**Acceptance Criteria:**

**Given** A bulk job has finished
**When** I view the job details
**Then** I should see a summary (Total: 500, Success: 480, Error: 20)
**And** I should be able to download a "Return File" (CSV/Excel)
**And** The return file must contain the original data PLUS a "Status" column and a "Message" column explaining any rejections

## Epic 6: Payroll Reconciliation (The Closer)

Enable RH Managers to close the month efficiently by generating files and resolving divergences.

### Story 6.1: Payroll File Generation

As an RH Manager,
I want to generate the monthly discount file for my payroll system,
So that the discounts can be applied to the employees' paychecks.

**Acceptance Criteria:**

**Given** It is time to close the payroll (e.g., day 20)
**When** I click "Generate Payroll File" for the current period
**Then** The system should query all ACTIVE loans for the tenant
**And** Sum the installments for each employee
**And** Generate a standard file (TXT/CSV) compatible with the payroll software layout
**And** Lock the period preventing new retroactive loans

### Story 6.2: Divergence Dashboard

As an RH Manager,
I want to import the return file from the payroll system and see any differences,
So that I can fix errors like "Employee terminated but loan active".

**Acceptance Criteria:**

**Given** I import the "Payroll Return" file
**When** The system processes the file
**Then** It should compare `ExpectedDiscount` vs `ActualDiscount` for each loan
**And** Highlight any records where the values differ
**And** Present these divergences in a dashboard for manual action (e.g., "Cancel Loan", "Re-queue Balance")

### Story 6.3: Consolidated Reports

As an RH Manager,
I want to view a consolidated report of total amounts passed to each bank,
So that I can authorize the financial transfer (TED) to the institutions.

**Acceptance Criteria:**

**Given** The payroll is closed and validated
**When** I access the Financial Reports
**Then** I should see a grouping by Bank (Consignataria)
**And** The total amount to be transferred to each bank
**And** I should be able to export this report to PDF for the Finance Department

### Story 6.4: Push Intelligence (Proactive Business Notifications)

As an RH Manager,
I want the system to proactively notify me about business anomalies (e.g., negative margins or unapplied discounts),
So that I can resolve issues before the final payroll closing deadline.

**Acceptance Criteria:**

**Given** A business event occurs (e.g., a bank return file processing completes)
**When** An anomaly is detected (e.g., a discount was rejected by the payroll system)
**Then** The system should generate a pro-active notification in the RH Portal
**And** An email/push alert should be sent to the assigned RH Manager
**And** The notification should include a deep link to the "Divergence Dashboard" for immediate resolution
