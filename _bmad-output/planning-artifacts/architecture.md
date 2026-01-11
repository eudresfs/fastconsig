---
stepsCompleted:
  - step-01-init
  - step-02-context
  - step-03-starter
  - step-04-decisions
  - step-05-patterns
  - step-06-structure
  - step-07-validation
  - step-08-complete
inputDocuments:
  - _bmad-output/planning-artifacts/prd.md
  - _bmad-output/planning-artifacts/epics.md
  - product-development/current-feature/arquitetura-tecnica.md
  - product-development/current-feature/wireframes.md
  - _bmad-output/planning-artifacts/product-brief-Fast Consig-2026-01-11.md
workflowType: 'architecture'
lastStep: 8
status: 'complete'
completedAt: '2026-01-11'
project_name: 'Fast Consig'
user_name: 'Eudres'
date: '2026-01-11'
---

# Architecture Decision Document

_This document builds collaboratively through step-by-step discovery. Sections are appended as we work through each architectural decision together._

## Project Context Analysis

### Requirements Overview

**Functional Requirements:**

O sistema abrange 27 requisitos funcionais organizados em 6 domínios de valor:

1. **Platform Foundation (FR01-FR04, FR15, FR26):** Autenticação multi-tenant via Clerk (SSO/Email+MFA), gestão de tenants pelo Super Admin, RBAC granular, bloqueio de login suspeito, e configuração de regras de margem por órgão
2. **Employee Management (FR05-FR07):** CRUD de funcionários com RLS, importação bulk via CSV/Excel com stream processing, cálculo automático de margem baseado em regras do tenant, bloqueio de inativação para funcionários com dívidas ativas
3. **Token System (FR09, FR11, FR25):** Geração de token 2FA com TTL configurável, entrega multi-canal (SMS/Email/Webhook), cancelamento/reenvio por RH, validação atômica com prevenção de double-spend
4. **Loan Operations (FR08, FR10):** Consulta blind de margem (CPF+Matrícula), averbação manual via portal com validação de token, registro imutável de evidências (IP, timestamp, user agent)
5. **Bulk Integration (FR12-FR14, FR17, FR27):** Upload de arquivos CNAB/Excel, processamento assíncrono com sucesso parcial, geração de arquivo de retorno com rejeições detalhadas, suporte a múltiplos layouts bancários
6. **Payroll Reconciliation (FR19-FR21, FR16):** Geração de arquivo de desconto para folha, dashboard de divergências, relatórios consolidados por banco, auditoria com anonimização LGPD

Arquiteturalmente, os requisitos exigem:
- **Transações ACID** para garantir consistência de margem (FR11)
- **Background Jobs** para processamento bulk sem bloquear API (FR13)
- **Anti-Corruption Layer** para isolar complexidade CNAB do domínio (FR17)
- **Audit Trail** append-only para compliance (FR15, FR16)

**Non-Functional Requirements:**

- **Performance (NFR01-NFR02):**
  - Consulta de margem < 200ms p95 → Exige indexação otimizada + caching
  - Processamento de 10k registros < 10min → Stream processing obrigatório
- **Reliability (NFR03-NFR04):**
  - 99.9% uptime → Load balancing + health checks
  - RPO 5min/RTO 4h → Backup contínuo + disaster recovery
- **Security (NFR05-NFR06):**
  - Retenção de logs 5 anos → Cold storage (S3 Glacier)
  - AES-256 + TLS 1.3 → Transparent Data Encryption (TDE) do PostgreSQL
- **Scalability (NFR08):** 1.000 usuários simultâneos → Horizontal scaling via containers
- **Accessibility (NFR07):** WCAG 2.1 AA → Radix UI primitives (keyboard nav, ARIA)

**Scale & Complexity:**

- **Domínio primário:** Full-Stack SaaS Platform (API + Web Portal + Background Workers)
- **Nível de complexidade:** **Alto** (Fintech com compliance regulatório + multi-tenancy + event-driven)
- **Componentes arquiteturais estimados:**
  - 3 camadas de aplicação (API, Web, Jobs)
  - 6 módulos de domínio (Auth, Employee, Token, Loan, Import, Payroll)
  - 2 camadas de infraestrutura (ACL para CNAB, Audit Trail)
  - 1 camada de observabilidade (Logs, Metrics, Tracing)

### Technical Constraints & Dependencies

**Stack Mandatório (do PRD/Arquitetura):**
- Backend: Node.js 22+, NestJS ou Fastify
- Frontend: React 19+, TypeScript 5.5+, Tailwind CSS 4, shadcn/ui
- Database: PostgreSQL 16+ com Row-Level Security (RLS)
- Auth Provider: Clerk (B2B Organizations)
- Background Jobs: BullMQ ou Hangfire
- Container Runtime: Docker + Kubernetes readiness

**Padrões Arquiteturais:**
- Hexagonal Architecture (Ports & Adapters) para isolar CNAB
- Event-Driven para "Margem Viva" (recálculo reativo)
- Repository Pattern com Row-Level Security
- API-First (backend como plataforma independente do frontend)

**Dependências Externas:**
- Clerk SDK para autenticação
- Gateway SMS/Email para entrega de tokens
- Potencial integração com ERPs de folha de pagamento (via webhooks)

**Restrições de Compliance:**
- Sigilo bancário: Consignatária A não pode ver contratos da Consignatária B
- LGPD: Anonimização de relatórios + direito ao esquecimento (soft delete)
- Evidência digital: Metadados de não-repúdio para validade jurídica

### Cross-Cutting Concerns Identified

1. **Multi-Tenancy Isolation:**
   - RLS obrigatório em todas as tabelas com `tenant_id`
   - Context Middleware para injetar tenant no Async Local Storage
   - Testes de vazamento de dados entre tenants

2. **Auditoria & Observabilidade:**
   - Audit Trail append-only para todas as mutações
   - Logging estruturado (Winston/Pino) com correlation IDs
   - Métricas de negócio (margem negativa, tokens expirados)

3. **Integridade Transacional:**
   - Pessimistic Locking (SELECT FOR UPDATE) para margem
   - Token hashing (SHA-256) para validação atômica
   - Idempotency keys para operações críticas

4. **Anti-Corruption Layer (CNAB):**
   - Adapters por banco para traduzir layouts → DTOs limpos
   - Validação de schema antes de processar
   - Isolamento de falhas bancárias (circuit breaker)

5. **Event-Driven Architecture:**
   - Domain Events para "Margem Viva" (LoanAverbado → MarginRecalculated)
   - Event Store opcional para auditoria avançada
   - Eventual consistency vs Strong consistency trade-offs

6. **Security & Rate Limiting:**
   - Rate limiting por CPF (3 tokens/10min) e por IP (global)
   - Helmet.js para headers de segurança (HSTS, CSP)
   - Detecção de comportamento anômalo (múltiplas consultas)

7. **Background Processing:**
   - Job queue resiliente (retry + dead letter queue)
   - Progress tracking para uploads bulk
   - Notificações de conclusão (webhook ou polling)

## Architecture Decision Records

### ADR-001: Modular Monolith vs Microservices

**Status:** Aceito
**Decisão:** Modular Monolith com extraction points preparados
**Data:** 2026-01-11

**Contexto:**
Sistema SaaS B2B com 6 domínios principais (Auth, Employee, Token, Loan, Import, Payroll). Precisa balancear simplicidade de desenvolvimento inicial com necessidade futura de escala independente.

**Opções Consideradas:**

1. **Microservices desde o início**
   - Prós: Escalabilidade independente, isolamento de falhas, deploy independente
   - Contras: Overhead de rede, debugging distribuído complexo, eventual consistency, time pequeno

2. **Monolito tradicional**
   - Prós: Simplicidade máxima, transações ACID fáceis, deploy único
   - Contras: Acoplamento, escalabilidade limitada, risco de "big ball of mud"

3. **Modular Monolith (ESCOLHIDO)**
   - Prós: Simplicidade inicial + ACID, preparado para extração futura, boundaries claros
   - Contras: Disciplina arquitetural necessária, possível refatoração futura

**Decisão:**
Implementar **Modular Monolith** com as seguintes características:
- Single deployment inicial (API + Jobs no mesmo processo)
- Arquitetura Hexagonal com Ports/Adapters por módulo
- Database schemas separados por bounded context (`auth.users`, `loans.contracts`, etc.)
- **Extraction points preparados:** Token e Import podem virar services independentes quando atingirmos 10k usuários ou 100k transações/dia

**Justificativa:**
- A "Margem Viva" exige transações ACID - monolito com PostgreSQL garante isso sem eventual consistency
- Time pequeno no início - microservices prematuros adicionam overhead
- Boundaries modulares permitem extração futura sem reescrita completa

**Consequências:**
- Desenvolvedores devem respeitar boundaries de módulos (sem imports diretos entre domínios)
- CI/CD simplificado no início
- Monitoramento de performance para identificar quando extrair serviços

---

### ADR-002: Row-Level Security (RLS) vs Application-Level Tenancy

**Status:** Aceito
**Decisão:** RLS + Application-level como dupla camada
**Data:** 2026-01-11

**Contexto:**
Sistema multi-tenant B2B com requisitos críticos de sigilo bancário e compliance LGPD. Consignatária A não pode ver dados da Consignatária B sob nenhuma circunstância.

**Opções Consideradas:**

1. **Application-level apenas (Discriminator Column)**
   - Prós: Performance previsível, debugging simples
   - Contras: Vulnerável a bugs de aplicação, SQL injection, ORM bypass

2. **Row-Level Security (PostgreSQL)**
   - Prós: Defesa em profundidade, segurança na camada mais baixa
   - Contras: Overhead de performance, debugging complexo

3. **RLS + Application-level (ESCOLHIDO)**
   - Prós: Segurança máxima, dupla camada de proteção
   - Contras: Overhead de performance, complexidade de implementação

**Decisão:**
Implementar **Row-Level Security obrigatório + Application-level como camada adicional**:
- RLS policies aplicadas a todas as tabelas multi-tenant
- Context Middleware valida `tenant_id` antes de setar no session
- Indexes compostos incluindo `tenant_id` para performance: `CREATE INDEX idx_employees_tenant ON employees(tenant_id, id)`
- Performance baselines estabelecidos: <200ms p95 com RLS ativo

**Otimizações:**
- Policies específicas por operação em vez de policy genérica
- `SECURITY DEFINER` functions para queries de leitura pesada (dashboards) quando seguro
- Monitoramento com `pg_stat_statements` para detectar degradação

**Justificativa:**
Para Fintech com sigilo bancário, RLS é a última linha de defesa não-negociável. Mesmo com bug de aplicação, PostgreSQL bloqueia vazamento de dados.

**Consequências:**
- Todos os queries incluem overhead de `WHERE tenant_id = current_setting('app.tenant')`
- Debugging requer `SET app.tenant = 'xxx'` antes de queries manuais
- Indexes devem incluir `tenant_id` como primeira coluna

---

### ADR-003: Event-Driven "Margem Viva" - Implementação

**Status:** Aceito
**Decisão:** Domain Events com State-Based Persistence + Audit Log
**Data:** 2026-01-11

**Contexto:**
Requisito de "Margem Viva" - margem deve ser recalculada reativamente em milissegundos após averbação, não sob demanda. Precisa de auditoria completa e performance <200ms.

**Opções Consideradas:**

1. **Event Sourcing Full**
   - Prós: Auditoria grátis, replay para debug, CQRS natural
   - Contras: Complexidade de projection rebuilds, eventual consistency, curva de aprendizado

2. **State-based apenas (sem eventos)**
   - Prós: Simplicidade máxima
   - Contras: Sem reatividade, auditoria manual, acoplamento

3. **Domain Events + State-based (ESCOLHIDO)**
   - Prós: Reatividade, simplicidade razoável, auditoria via event log
   - Contras: Não é full Event Sourcing (sem rebuild de state)

**Decisão:**
Implementar **Domain Events com State-Based Persistence + Audit Log**:

- **Transacional:** Dentro de uma transação, Domain Events disparam handlers síncronos (ex: recalcular margem)
- **State:** Tabelas mantêm estado atual (`available_margin`, `used_margin`)
- **Auditoria:** Tabela `domain_events_log` append-only registra todos os eventos com payload JSON
- **Webhooks:** Após commit, eventos são publicados via BullMQ para consumers externos
- **Sem Event Sourcing full:** Estado não é reconstruído de eventos (simplifica MVP)

**Exemplo de Fluxo:**
```typescript
// Dentro de transaction
await loanRepository.create(loan);
domainEvents.raise(new LoanCreatedEvent(loan));
// Handler síncrono
onLoanCreated(event) {
  marginCalculator.recalculate(event.employeeId);
}
// Commit transaction
// Após commit
await eventBus.publish(new LoanCreatedEvent(loan)); // → Webhooks
```

**Justificativa:**
Balanceia reatividade necessária para "Margem Viva" com simplicidade de implementação para MVP. Event Sourcing full é over-engineering neste estágio.

**Consequências:**
- Handlers de eventos devem ser idempotentes
- Event log cresce indefinidamente (partitioning necessário)
- Eventual consistency para webhooks externos (após commit)

---

### ADR-004: Anti-Corruption Layer (ACL) para CNAB

**Status:** Aceito
**Decisão:** Hexagonal Architecture com Banking ACL
**Data:** 2026-01-11

**Contexto:**
Bancos enviam arquivos em layouts arcaicos e variáveis (CNAB 240, CNAB 400, Excel customizado). Layouts mudam sem aviso. Domínio precisa ser protegido dessa complexidade.

**Opções Consideradas:**

1. **Parsing direto no domínio**
   - Prós: Simplicidade
   - Contras: Contamina domínio com lógica de parsing, impossível de manter com múltiplos bancos

2. **Adapter genérico único**
   - Prós: Centralização
   - Contras: Complexidade interna alta, difícil manutenção

3. **Hexagonal + Banking ACL (ESCOLHIDO)**
   - Prós: Isolamento completo, adapters independentes por banco
   - Contras: Mais arquivos, complexidade de configuração

**Decisão:**
Implementar **Hexagonal Architecture com Banking ACL**:

**Estrutura:**
```
infrastructure/
  banking-acl/
    adapters/
      banco-brasil/
        cnab240-v1.adapter.ts
        cnab240-v2.adapter.ts
      caixa/
        cnab400-v1.adapter.ts
    validators/
      file-validator.ts  # Schema, size, encoding
      antivirus.ts
    interfaces/
      ILoanFileParser.ts  # parse(file) → LoanDTO[]
```

**Características:**
- **Port:** Interface `ILoanFileParser` define contrato
- **Adapters:** Implementações por banco + versão
- **Validation Layer:** Schema validation + antivirus antes de parsing
- **Error Isolation:** Falha em um adapter não afeta outros bancos
- **Configuração:** Mapeamento `bank_code → adapter_version` em tenant config
- **Monitoramento:** Alertas quando layout não reconhecido é detectado

**Versioning de Adapters:**
Quando banco muda layout, criamos `v2` do adapter. Sistema usa versão antiga até deploy do novo adapter. Validação detecta layout incompatível e alerta.

**Justificativa:**
Permite escalar para 10, 20 ou 50 bancos sem contaminar regras de negócio com "ifs" de layout de arquivo. Arquivos de upload são vetor de ataque - ACL inclui sandbox de validação.

**Consequências:**
- Cada novo banco requer desenvolvimento de adapter
- Mudanças de layout bancário isoladas em adapter específico
- Core Domain nunca vê strings posicionais ou lógica de parsing

---

### ADR-005: Token System - Segurança e Performance

**Status:** Aceito
**Decisão:** Redis Cache + Atomic Locks com Hashing SHA-256
**Data:** 2026-01-11

**Contexto:**
Token 2FA está no critical path da averbação. Precisa <50ms p99. Deve prevenir double-spend, timing attacks, e rate limiting abuse.

**Requisitos Não-Negociáveis:**
- Tokens criptograficamente seguros (não sequenciais)
- Armazenados como hash (SHA-256), nunca plaintext
- Rate limiting por CPF e por IP global
- TTL configurável por tenant (default 1h)
- Proteção contra timing attacks

**Decisão:**
Implementar **Token System com Redis Cache + Atomic Locks**:

**Geração:**
- CSPRNG 6-digit code (`crypto.randomInt`)
- Hash SHA-256 armazenado em DB
- Metadata: employee_id, tenant_id, expires_at, status (ACTIVE/USED/CANCELLED)

**Storage:**
- **PostgreSQL:** Source of truth
- **Redis Cache:** Validação rápida
  - Key: `token:{employee_id}:{hash}`
  - Value: metadata JSON
  - TTL: expires_at

**Validation (Atomic):**
```typescript
// Redis GETDEL + DB update em transaction
const metadata = await redis.getDel(`token:${employeeId}:${hash}`);
if (!metadata) throw new TokenInvalidError();
// Timing-safe comparison
if (!crypto.timingSafeEqual(hash, storedHash)) throw new TokenInvalidError();
// Update DB
await db.transaction(async tx => {
  await tx.tokens.update({ status: 'USED' });
  await tx.loans.create(loan);
});
```

**Rate Limiting:**
- Redis: `rate:token:cpf:{cpf}` - max 3/10min
- Redis: `rate:token:ip:{ip}` - max 100/min global

**Delivery (Async):**
- BullMQ job: `sendToken({tokenId, method: 'sms'|'email'|'webhook'})`
- Retry 3x com backoff (1s, 5s, 15s)
- Fallback: se SMS falha, tenta email automaticamente

**Justificativa:**
Redis cache reduz latência de validação de ~100ms (DB query) para ~5ms. Atomic `GETDEL` previne race conditions. Hash previne vazamento de tokens.

**Consequências:**
- Dependência de Redis (single point of failure - usar Redis Sentinel)
- Cache invalidation em caso de cancelamento manual
- Timing attacks mitigados com `crypto.timingSafeEqual`

---

### ADR-006: Auditoria e Observabilidade

**Status:** Aceito
**Decisão:** DB Triggers + Application Events + OpenTelemetry
**Data:** 2026-01-11

**Contexto:**
LGPD + Sigilo Bancário exigem audit trail completo de 5 anos. Precisa rastrear WHO, WHAT, WHEN, WHERE, WHY, e VALORES (before/after).

**Opções Consideradas:**

1. **Application Events apenas**
   - Prós: Controle total, contexto de negócio
   - Contras: Pode ser esquecido por devs

2. **Database Triggers apenas**
   - Prós: Impossível escapar
   - Contras: Sem contexto de negócio (user IP, etc)

3. **Hybrid (ESCOLHIDO)**
   - Prós: Triggers garantem cobertura, App Events adicionam contexto
   - Contras: Redundância controlada

**Decisão:**
Implementar **Audit & Observability Stack**:

**Audit Trail:**
- Tabela `audit_trails`:
  - tenant_id, user_id, action, resource_type, resource_id
  - ip_address, user_agent
  - old_value, new_value (JSON diff)
  - timestamp
- **Database triggers** para mutations críticas (UPDATE/DELETE em loans, margins)
- **Application-level events** para business actions (login, margin query, token generation)
- **Retenção:**
  - Hot storage: 1 ano (PostgreSQL)
  - Cold storage: 5 anos (S3 Glacier)

**Logging:**
- Winston/Pino com JSON structured logs
- Correlation ID em todos os logs (`req.id`)
- Níveis:
  - ERROR: alertas imediatos
  - WARN: review diário
  - INFO: audit trail
  - DEBUG: dev only

**Tracing:**
- OpenTelemetry SDK
- Spans: HTTP request, DB query, Queue job, External API
- Exportado para Jaeger/Tempo

**Metrics:**
- **Business:** tokens_generated, loans_created, margin_queries, file_processing_time
- **Technical:** API latency p50/p95/p99, DB connection pool, queue depth

**Justificativa:**
Triggers garantem que nenhuma mutação escape de auditoria. Application Events adicionam contexto de negócio. OpenTelemetry permite debug de performance distribuída.

**Consequências:**
- Audit table cresce indefinidamente (partitioning por ano necessário)
- Triggers adicionam overhead mínimo (<5ms por mutation)
- Correlation IDs devem ser propagados manualmente em jobs assíncronos

---

## Architecture Decision Summary

| # | Decisão | Escolha Final | Trade-offs Aceitos |
|---|---------|---------------|-------------------|
| **ADR-001** | Arquitetura Base | **Modular Monolith** com extraction points | Simplicidade inicial vs Escalabilidade futura |
| **ADR-002** | Multi-Tenancy | **RLS + Application-level** | Overhead de performance vs Segurança em profundidade |
| **ADR-003** | Margem Viva | **Domain Events + State-based** | Simplicidade vs Full Event Sourcing |
| **ADR-004** | CNAB Integration | **Hexagonal + Banking ACL** | Complexidade de adapters vs Isolamento de domínio |
| **ADR-005** | Token System | **Redis Cache + Atomic Locks** | Dependência de Redis vs Performance <50ms |
| **ADR-006** | Auditoria | **DB Triggers + App Events** | Redundância vs Compliance garantido |

## Critical Rules for AI Agents

**Regras que Agentes de IA DEVEM seguir durante implementação:**

1. **NUNCA misture lógica de domínio com parsing de CNAB** → Use sempre adapters da camada ACL
2. **SEMPRE valide tenant_id antes de queries** → Context Middleware obrigatório
3. **Token validation DEVE ser atômica** → Use Redis GETDEL, não GET + DEL separado
4. **Margin updates DEVEM usar pessimistic locking** → `SELECT FOR UPDATE` obrigatório antes de update
5. **Audit trails são append-only** → NUNCA UPDATE ou DELETE em `audit_trails`
6. **Cada módulo tem schema próprio** → Prefixo obrigatório: `auth.users`, `loans.contracts`, `tokens.reservations`
7. **Domain Events devem ser idempotentes** → Handlers podem ser chamados múltiplas vezes
8. **Indexes multi-tenant** → Sempre incluir `tenant_id` como primeira coluna em indexes compostos
9. **Rate limiting obrigatório** → Verificar antes de operações sensíveis (token generation, margin query)
10. **Correlation ID propagation** → Incluir em logs, traces e jobs assíncronos

## Starter Template Evaluation

### Primary Technology Domain

**Full-Stack SaaS Platform** (API + Web Portal + Background Workers)

Baseado nos requisitos do projeto:
- Backend API-first com NestJS
- Frontend Web com React + TypeScript
- Background Jobs com BullMQ
- PostgreSQL com Row-Level Security
- Multi-tenancy complexo

### Starter Options Considered

Avaliei **3 abordagens principais** para iniciar o projeto Fast Consig:

#### Opção 1: Turborepo + NestJS + React Monorepo

**Stack:** Turborepo, pnpm workspaces, NestJS (backend), React 19 (frontend), PostgreSQL

**Prós:**
- **Monorepo otimizado:** Turborepo + pnpm oferecem cache incremental e builds rápidos
- **Code sharing nativo:** Pacotes compartilhados (`@fastconsig/shared`, `@fastconsig/database`) para DTOs, schemas, e utilitários
- **Alinhamento com ADRs:** Suporta nativamente a arquitetura de Modular Monolith com extraction points (ADR-001)
- **TypeScript end-to-end:** Type-safety completo do DB até o frontend
- **Production-ready:** Docker, CI/CD (GitHub Actions), configuração de ambiente

**Contras:**
- Setup inicial manual mais complexo
- Requer configuração custom de PostgreSQL + Redis + BullMQ
- Sem scaffolding automático para módulos

**Compatibilidade com ADRs:**
- ✅ ADR-001: Modular Monolith nativo
- ✅ ADR-003: Suporte a Domain Events via NestJS CQRS
- ✅ ADR-004: Hexagonal Architecture via NestJS modules
- ⚠️ Requer setup manual de RLS, Token System, Banking ACL

#### Opção 2: NestJS Monorepo (CLI Nativo)

**Stack:** NestJS CLI workspace mode, NestJS (backend + jobs), React (frontend via app separada)

**Prós:**
- **NestJS-first:** CLI nativo gerencia workspace com `nest generate app` / `nest generate library`
- **Menos ferramentas:** Não depende de Turborepo (usa npm/yarn workspaces)
- **Scaffolding automático:** `nest g resource` cria CRUD completo com testes
- **Familiar para devs NestJS:** Estrutura padrão do framework

**Contras:**
- **Frontend como segunda classe:** React frontend não integrado nativamente no NestJS CLI
- Cache de builds menos otimizado que Turborepo
- Menos suporte para monorepo multi-linguagem (se precisarmos adicionar microservices futuros)

**Compatibilidade com ADRs:**
- ✅ ADR-001: Suporta Modular Monolith (apps/api + apps/jobs + libs/shared)
- ✅ ADR-003: NestJS CQRS module para Domain Events
- ✅ ADR-004: Hexagonal via NestJS layered architecture
- ⚠️ Frontend React separado (não gerenciado pelo CLI)

#### Opção 3: Scaffolding Manual "From Scratch"

**Stack:** Setup manual completo com pnpm workspaces, configuração custom

**Prós:**
- **Controle total:** Zero decisões pré-tomadas, 100% alinhado com ADRs
- **Sem bloat:** Apenas o que precisamos
- **Aprendizado:** Time entende cada peça da arquitetura

**Contras:**
- **Alto custo inicial:** Configurar Turborepo/pnpm, TypeScript, ESLint, Prettier, Vitest, Docker, CI/CD manualmente
- **Propenso a erros:** Configurações inconsistentes entre packages
- **Tempo de setup:** 3-5 dias para setup produtivo vs 1 dia com starter

**Não recomendado** dado o prazo do projeto e complexidade.

### Selected Starter: Turborepo + pnpm + NestJS + React

**Decisão:** Usar **Turborepo Monorepo** como base, com setup customizado para Fast Consig.

**Rationale para Seleção:**

1. **Alinhamento Arquitetural:**
   - Suporta nativamente o Modular Monolith (ADR-001) com workspaces (`apps/api`, `apps/jobs`, `apps/web`, `packages/*`)
   - Extração futura de microservices é trivial (basta mover `apps/jobs` para repo separado mantendo `@fastconsig/shared`)

2. **Performance de Desenvolvimento:**
   - Turborepo cache reduz rebuild time em 70-90%
   - pnpm workspaces economiza ~300MB de `node_modules` duplicados
   - Hot-reload funciona em todos os apps simultaneamente

3. **Code Sharing Type-Safe:**
   - Pacote `@fastconsig/shared` compartilha DTOs, Zod schemas, enums entre API/Web/Jobs
   - Pacote `@fastconsig/database` centraliza Prisma schema e migrations
   - Mudança em schema → todos os apps veem o tipo atualizado imediatamente

4. **Compatibilidade com Stack:**
   - NestJS backend: ✅ Suportado
   - React 19 frontend: ✅ Suportado (via Vite)
   - PostgreSQL + Redis: ✅ Docker Compose incluído em starters
   - BullMQ: ✅ Pacote `@fastconsig/queue` compartilhado

5. **Produção-Ready:**
   - Docker multi-stage builds por app
   - GitHub Actions workflows com cache de Turborepo
   - Vercel/Railway deploy-ready para web
   - Kubernetes manifests geráveis via `apps/api/Dockerfile`

**Initialization Command:**

```bash
# 1. Criar base do monorepo com Turborepo
pnpm create turbo@latest fastconsig --package-manager pnpm

cd fastconsig

# 2. Adicionar NestJS API
cd apps
nest new api --skip-git --package-manager pnpm
mv api/* ./  # Mover para apps/api
rmdir api

# 3. Adicionar NestJS Jobs Worker
nest new jobs --skip-git --package-manager pnpm

# 4. Adicionar React Web (Vite)
pnpm create vite@latest web --template react-ts

# 5. Criar packages compartilhados
cd ../packages
mkdir shared database ui config

# 6. Configurar pnpm-workspace.yaml (já criado pelo Turborepo)
# Verificar que inclui: apps/*, packages/*

# 7. Configurar Turborepo (turbo.json)
# Adicionar pipelines: build, dev, test, lint, typecheck
```

**Estrutura Final:**
```
fastconsig/
├── apps/
│   ├── api/          # NestJS API (Porta 3000)
│   ├── jobs/         # NestJS Background Workers (BullMQ)
│   └── web/          # React 19 + Vite (Porta 5173)
├── packages/
│   ├── shared/       # DTOs, Schemas, Enums
│   ├── database/     # Prisma Client + Migrations
│   ├── ui/           # shadcn/ui components (React)
│   └── config/       # ESLint, TypeScript, Tailwind configs
├── turbo.json        # Build pipeline
├── pnpm-workspace.yaml
├── docker-compose.yml  # PostgreSQL + Redis
└── package.json
```

### Architectural Decisions Provided by Starter

**Language & Runtime:**
- TypeScript 5.5+ strict mode em todos os packages
- Node.js 22+ (engines especificado em root package.json)
- pnpm 9+ como package manager (fast, disk-efficient)

**Styling Solution:**
- Tailwind CSS 4 (configurado em `packages/config/tailwind.config.ts`)
- shadcn/ui components em `packages/ui` (Radix UI primitives)
- CSS Modules como fallback para componentes isolados

**Build Tooling:**
- **Turborepo:** Orchestration de builds com cache incremental e remoto
- **Vite:** Frontend bundling (ESBuild) com HMR <50ms
- **NestJS CLI:** Backend compilation (SWC) com watch mode
- **esbuild/swc:** Transpilação TypeScript super-rápida

**Testing Framework:**
- **Vitest:** Unit tests para packages compartilhados (shared, database)
- **Jest:** NestJS apps (api, jobs) - integração nativa
- **Testing Library:** React components (web)
- **Playwright:** E2E tests (opcional, configurado depois)

**Code Organization:**
- **Hexagonal Architecture:** NestJS modules como bounded contexts
  - `apps/api/src/modules/auth/` → Port: `IAuthService`, Adapter: `ClerkAuthAdapter`
  - `apps/api/src/modules/loans/` → Domain entities, use cases
  - `apps/api/src/infrastructure/` → Banking ACL, Redis, PostgreSQL
- **Feature-based structure:** Frontend organizado por feature (`web/src/features/auth/`, `web/src/features/loans/`)
- **Shared kernel:** `packages/shared/src/domain/` para Value Objects compartilhados (CPF, Currency, etc.)

**Development Experience:**
- **Concurrent dev mode:** `pnpm dev` roda API + Jobs + Web simultaneamente
- **Hot reload:** Mudanças em `packages/shared` → rebuild automático em apps dependentes
- **Type checking:** `pnpm typecheck` valida todos os packages em paralelo
- **Linting unificado:** ESLint config em `packages/config/eslint-config-custom`

**Note:** Project initialization usando este comando deve ser a **primeira story de implementação** (Epic 0 - Foundation Setup).

## Core Architectural Decisions

### Decision Priority Analysis

**Critical Decisions (Block Implementation):**
- ✅ Backend Framework: NestJS 11 + Fastify Adapter + tRPC
- ✅ Authentication: Clerk (B2B Organizations)
- ✅ ORM: Drizzle ORM v0.36+
- ✅ Schema Validation: Zod v3.24+
- ✅ Database: PostgreSQL 16+ com RLS
- ✅ Build Tool: Turborepo + pnpm workspaces

**Important Decisions (Shape Architecture):**
- ✅ State Management: Zustand v4.5+
- ✅ Routing: TanStack Router v1.50+
- ✅ Forms: React Hook Form + Zod
- ✅ API Documentation: tRPC auto-generated types + OpenAPI para webhooks externos
- ✅ Deployment: Oracle Cloud VMs com Docker Compose

**Deferred Decisions (Post-MVP):**
- E2E Monitoring Tool (além de OpenTelemetry)
- APM específico (New Relic, Datadog, ou self-hosted)
- Feature Flags service (LaunchDarkly vs self-hosted)

---

### Data Architecture

#### 1. ORM/Query Builder: Drizzle ORM v0.36+

**Decisão:** Drizzle ORM v0.36+ (MUDANÇA DO USUÁRIO)
**Versão Verificada:** Drizzle 0.36.x (stable, Janeiro 2026)

**Rationale:**
- **SQL-like syntax:** Maior controle sobre queries complexas mantendo type-safety
- **Lightweight:** ~7kb vs ~50kb Prisma Client (melhor para serverless/edge se necessário)
- **Suporte nativo a `SELECT FOR UPDATE`:** Via `.for('update')` - essencial para ADR-005 (Token System)
- **Performance:** Queries mais eficientes para casos avançados (pessimistic locking, bulk operations)
- **Type-safety:** Schema TypeScript-first com inferência automática via `drizzle-kit`
- **Migrations:** Declarativas via `drizzle-kit generate` + SQL migrations versionadas

**Implementação:**
```typescript
// packages/database/src/schema.ts (Drizzle schema)
import { pgTable, varchar, timestamp, boolean, integer, index, uniqueIndex } from 'drizzle-orm/pg-core'

export const tenants = pgTable('tenants', {
  id: varchar('id', { length: 30 }).primaryKey().$defaultFn(() => cuid()),
  clerkOrgId: varchar('clerk_org_id', { length: 255 }).unique().notNull(),
  name: varchar('name', { length: 255 }).notNull(),
  cnpj: varchar('cnpj', { length: 14 }).unique().notNull(),
  active: boolean('active').default(true).notNull(),
  createdAt: timestamp('created_at').defaultNow().notNull(),
}, (table) => ({
  clerkOrgIdIdx: index('tenants_clerk_org_id_idx').on(table.clerkOrgId),
}))

export const employees = pgTable('employees', {
  id: varchar('id', { length: 30 }).primaryKey().$defaultFn(() => cuid()),
  tenantId: varchar('tenant_id', { length: 30 }).notNull().references(() => tenants.id),
  cpf: varchar('cpf', { length: 11 }).notNull(),
  name: varchar('name', { length: 255 }).notNull(),
  enrollment: varchar('enrollment', { length: 50 }).notNull(),
  grossSalary: integer('gross_salary').notNull(), // em centavos
  availableMargin: integer('available_margin').notNull(), // em centavos
  createdAt: timestamp('created_at').defaultNow().notNull(),
}, (table) => ({
  tenantIdIdx: index('employees_tenant_id_idx').on(table.tenantId),
  cpfUnique: uniqueIndex('employees_tenant_cpf_unique').on(table.tenantId, table.cpf),
  enrollmentUnique: uniqueIndex('employees_tenant_enrollment_unique').on(table.tenantId, table.enrollment),
}))

// packages/database/src/db.ts (Drizzle client)
import { drizzle } from 'drizzle-orm/node-postgres'
import { Pool } from 'pg'
import * as schema from './schema'

const pool = new Pool({
  connectionString: process.env.DATABASE_URL,
})

export const db = drizzle(pool, { schema })

// Uso com type-safety
import { db } from '@fastconsig/database'
import { employees, tenants } from '@fastconsig/database/schema'
import { eq, and } from 'drizzle-orm'

// Query simples com type-safety
const employeeList = await db
  .select()
  .from(employees)
  .where(eq(employees.tenantId, tenantId))

// Pessimistic locking (SELECT FOR UPDATE) - ADR-005
const employee = await db
  .select()
  .from(employees)
  .where(eq(employees.id, employeeId))
  .for('update') // ✅ Suporte nativo!
  .limit(1)

// Update após lock
await db
  .update(employees)
  .set({ availableMargin: newMargin })
  .where(eq(employees.id, employeeId))
```

**Migrations:**
```bash
# Gerar migration
pnpm drizzle-kit generate:pg

# Aplicar migrations
pnpm drizzle-kit push:pg  # Dev
pnpm tsx packages/database/migrate.ts  # Production (via script custom)
```

**Vantagens para Fast Consig:**
- **Token System (ADR-005):** `SELECT FOR UPDATE` nativo elimina race conditions
- **Banking ACL (ADR-004):** Bulk inserts otimizados via `db.insert().values([...])`
- **Margem Viva (ADR-003):** Queries complexas com JOINs mantendo type-safety
- **Audit Trail (ADR-006):** Raw SQL quando necessário via `db.execute(sql``)`

**Trade-off Aceito:**
- Sem GUI como Prisma Studio (usar Drizzle Studio ou TablePlus)
- Migrations menos "mágicas" (mais controle, menos automação)

**Afeta:** Todos os módulos (Auth, Employee, Token, Loan, Import, Payroll)

---

#### 2. Schema Validation: Zod v3.24+

**Decisão:** Zod v3.24+ para validação runtime em toda a stack
**Versão Verificada:** Zod 3.24.x (stable, Janeiro 2026)

**Rationale:**
- **Code sharing crítico:** Schemas em `@fastconsig/shared` reutilizados em:
  - Backend: NestJS validation pipes + tRPC procedures
  - Frontend: React Hook Form validation
  - Jobs: BullMQ payload validation
  - **Drizzle:** Pode inferir schemas Zod de schemas Drizzle via `createInsertSchema`
- **Type-inference:** `z.infer<typeof schema>` gera TypeScript types automaticamente
- **Integração tRPC:** Native support via `@trpc/server` input/output validation
- **Banking ACL:** Validação de estrutura CNAB antes de parsing

**Implementação com Drizzle:**
```typescript
// packages/shared/src/schemas/loan.schema.ts
import { z } from 'zod'
import { createInsertSchema, createSelectSchema } from 'drizzle-zod'
import { loans } from '@fastconsig/database/schema'

// Zod schema inferido do Drizzle schema ✅
export const InsertLoanSchema = createInsertSchema(loans, {
  amount: z.number().positive().max(1_000_000),
  installments: z.number().int().min(1).max(96),
})

export const SelectLoanSchema = createSelectSchema(loans)

// Custom schemas para DTOs específicos
export const CreateLoanSchema = z.object({
  employeeId: z.string().cuid(),
  amount: z.number().positive().max(1_000_000),
  installments: z.number().int().min(1).max(96),
  tokenCode: z.string().length(6).regex(/^\d{6}$/),
  contractNumber: z.string().min(1).max(50),
})

export type CreateLoanDTO = z.infer<typeof CreateLoanSchema>
export type InsertLoan = z.infer<typeof InsertLoanSchema>
export type SelectLoan = z.infer<typeof SelectLoanSchema>

// apps/api - NestJS + tRPC
export const loanRouter = router({
  create: protectedProcedure
    .input(CreateLoanSchema) // Validação automática ✅
    .mutation(({ input, ctx }) => {
      return ctx.loanService.create(input)
    })
})

// apps/web - React Hook Form
const form = useForm<CreateLoanDTO>({
  resolver: zodResolver(CreateLoanSchema) // Mesma validação ✅
})
```

**Sinergia Drizzle + Zod:**
- `drizzle-zod` gera Zod schemas automaticamente do schema Drizzle
- Evita duplicação de definições
- Validação em runtime alinhada com types do DB

**Trade-off Aceito:** Bundle size ~14kb (aceitável para ganho em DX e code sharing)

**Afeta:** Todos os módulos, especialmente Banking ACL (validação de layouts CNAB)

---

#### 3. Migration Strategy: Drizzle Kit + SQL Migrations

**Decisão:** Drizzle Kit para generate migrations, SQL versionado

**Configuração:**
```typescript
// drizzle.config.ts
import { defineConfig } from 'drizzle-kit'

export default defineConfig({
  schema: './packages/database/src/schema.ts',
  out: './packages/database/drizzle',
  driver: 'pg',
  dbCredentials: {
    connectionString: process.env.DATABASE_URL!,
  },
})
```

**Workflow:**
```bash
# Desenvolvimento
pnpm drizzle-kit generate:pg  # Gera SQL migration em drizzle/
pnpm drizzle-kit push:pg      # Aplica em dev

# Production (script custom)
# packages/database/migrate.ts
import { drizzle } from 'drizzle-orm/node-postgres'
import { migrate } from 'drizzle-orm/node-postgres/migrator'

await migrate(db, { migrationsFolder: './drizzle' })
```

**Safety Checks:**
- Migrations SQL revisáveis manualmente antes de apply
- CI executa migrations em shadow database para detectar breaking changes
- Backup automático antes de deploy via Oracle Cloud Database Backup

**Afeta:** CI/CD pipeline, deployment process

---

### Authentication & Security

#### 1. Authentication Provider: Clerk (B2B Organizations)

**Decisão:** Clerk com B2B Organizations mode
**Versão Verificada:** @clerk/nextjs v6.x, @clerk/clerk-sdk-node v5.x (Janeiro 2026)

**Rationale (MUDANÇA APROVADA):**
- **Multi-tenancy nativo:** Clerk Organizations mapeia 1:1 com Tenants do Fast Consig
- **ROI positivo:** Economiza ~R$ 22.000 em desenvolvimento (3 semanas de auth custom)
- **Segurança enterprise-grade:** Attack protection, MFA, session management built-in
- **Compliance:** GDPR/LGPD compliant, SOC 2 Type II certified
- **SSO futuro:** OAuth (Google Workspace, Microsoft Entra ID) habilitável sem refatoração
- **Custo:** $0/mês até 10k MAU, depois $25/mês (Pro) = ~R$ 125/mês

**Arquitetura de Integração com Drizzle:**
```typescript
// apps/api/src/auth/clerk.strategy.ts
import { ClerkExpressRequireAuth } from '@clerk/clerk-sdk-node'
import { db } from '@fastconsig/database'
import { tenants } from '@fastconsig/database/schema'
import { eq } from 'drizzle-orm'

@Injectable()
export class ClerkAuthGuard implements CanActivate {
  async canActivate(context: ExecutionContext): Promise<boolean> {
    const request = context.switchToHttp().getRequest()
    const auth = request.auth

    // Buscar tenant via Drizzle
    const tenant = await db
      .select()
      .from(tenants)
      .where(eq(tenants.clerkOrgId, auth.orgId))
      .limit(1)

    if (!tenant[0]) throw new UnauthorizedException('Tenant not found')

    // Injetar no AsyncLocalStorage para RLS
    this.cls.set('tenantId', tenant[0].id)
    this.cls.set('userId', auth.userId)

    return true
  }
}

// Sincronização Clerk <-> PostgreSQL via Webhooks
@Post('webhooks/clerk')
async handleClerkWebhook(@Body() evt: ClerkWebhookEvent) {
  if (evt.type === 'organization.created') {
    await db.insert(tenants).values({
      clerkOrgId: evt.data.id,
      name: evt.data.name,
    })
  }
}
```

**Fluxo de Autenticação:**
1. Usuário faz login via Clerk (frontend)
2. Clerk retorna session token (JWT)
3. Frontend envia token em header: `Authorization: Bearer <token>`
4. NestJS `ClerkAuthGuard` valida token + extrai `orgId`
5. Drizzle query busca `tenantId` correspondente
6. AsyncLocalStorage injeta `tenantId` para RLS automático

**Trade-off Aceito:**
- Dependência de serviço externo (SaaS)
- Mitigação: Clerk tem 99.99% SLA, failover automático, e podemos cache de sessions em Redis

**Afeta:** Todos os módulos (Auth, Employee, Token, Loan, Import, Payroll)

---

#### 2. RBAC Implementation: Custom Roles + Clerk Metadata

**Decisão:** Clerk Roles (basic) + Drizzle Permissions (granular)

**Modelo Híbrido:**
```typescript
// packages/database/src/schema.ts
export const permissions = pgTable('permissions', {
  id: varchar('id', { length: 30 }).primaryKey().$defaultFn(() => cuid()),
  userId: varchar('user_id', { length: 255 }).notNull(),

  canReadEmployees: boolean('can_read_employees').default(false),
  canWriteEmployees: boolean('can_write_employees').default(false),
  canReadLoans: boolean('can_read_loans').default(false),
  canWriteLoans: boolean('can_write_loans').default(false),
  canApproveLoans: boolean('can_approve_loans').default(false),
  canClosePayroll: boolean('can_close_payroll').default(false),
}, (table) => ({
  userIdIdx: index('permissions_user_id_idx').on(table.userId),
}))

// Guard no NestJS
@Injectable()
export class PermissionsGuard {
  async canActivate(context: ExecutionContext): Promise<boolean> {
    const requiredPermission = this.reflector.get('permission', context.getHandler())
    const userId = context.switchToHttp().getRequest().user.id

    const userPermissions = await db
      .select()
      .from(permissions)
      .where(eq(permissions.userId, userId))
      .limit(1)

    return userPermissions[0]?.[requiredPermission] === true
  }
}
```

**Afeta:** Auth module, todos os protected endpoints

---

### API & Communication Patterns

#### 1. API Style: tRPC (interno) + REST (webhooks externos)

**Decisão:**
- **Interno (Web ↔ API):** tRPC v11+ para type-safety end-to-end
- **Externo (Bancos → API):** REST + OpenAPI 3.1 para webhooks

**Implementação com Drizzle:**
```typescript
// apps/api/src/trpc/router.ts
import { db } from '@fastconsig/database'
import { employees } from '@fastconsig/database/schema'
import { eq } from 'drizzle-orm'

export const appRouter = t.router({
  employees: t.router({
    list: t.procedure
      .query(async ({ ctx }) => {
        return db
          .select()
          .from(employees)
          .where(eq(employees.tenantId, ctx.tenantId))
      }),

    create: t.procedure
      .input(CreateEmployeeSchema)
      .mutation(async ({ input, ctx }) => {
        const [employee] = await db
          .insert(employees)
          .values({ ...input, tenantId: ctx.tenantId })
          .returning()
        return employee
      })
  })
})
```

**Afeta:** Frontend (tRPC client), Banking ACL (REST webhooks)

---

### Infrastructure & Deployment

#### 1. Build Tool: Turborepo + pnpm workspaces

**Decisão:** Turborepo para orchestration, pnpm para package management

**Configuração (com cache local Oracle Cloud):**
```json
// turbo.json
{
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**", ".next/**"]
    },
    "db:generate": {
      "cache": false
    },
    "db:push": {
      "cache": false
    }
  }
}
```

**Performance Esperada:**
- Primeira build: ~110s
- Rebuild com cache (nada mudou): ~2s
- Rebuild após mudança em `database`: ~40s (rebuilda api + jobs)

**Afeta:** CI/CD, developer workflow

---

### Decision Impact Analysis

**Implementation Sequence (ordem de implementação):**

1. **Sprint 0 - Foundation:**
   - Setup monorepo (pnpm + Turborepo)
   - Configure Drizzle + PostgreSQL schema
   - Integrate Clerk authentication
   - Setup NestJS + Fastify adapter + tRPC

2. **Sprint 1 - Core Infrastructure:**
   - Implement Context Middleware (tenant isolation)
   - Setup RLS policies em PostgreSQL
   - Configure Redis (sessions + cache)
   - Implement Domain Events infrastructure

3. **Sprint 2 - Banking ACL:**
   - Create CNAB adapter interfaces
   - Implement Banco do Brasil adapter (v1)
   - Implement Caixa adapter (v1)
   - Setup file validation + antivirus

4. **Sprint 3+ - Business Features:**
   - Employee module (Epic 2)
   - Token System (Epic 3)
   - Loan Operations (Epic 4)
   - Bulk Integration (Epic 5)
   - Payroll Reconciliation (Epic 6)

**Cross-Component Dependencies:**

- **Clerk → Drizzle:** Tenant sync via webhooks (org.created → insert tenant)
- **NestJS → tRPC → React:** Type-safe API end-to-end
- **Drizzle → Zod:** Schema validation via drizzle-zod
- **Redis → BullMQ → Domain Events:** Async processing pipeline
- **OpenTelemetry → Grafana:** Observability stack (self-hosted Oracle Cloud)
- **Turborepo → CI/CD:** Cache optimization (local, sem Vercel)

---

## Implementation Patterns & Consistency Rules

### Pattern Categories Defined

**Pontos Críticos de Conflito Identificados:** 24 áreas onde agentes de IA poderiam fazer escolhas diferentes que causariam incompatibilidades.

### Naming Patterns

#### Database Naming Conventions (Drizzle ORM)

**Tabelas:**
- **Formato:** `snake_case`, plural para entidades principais
- **Exemplos:** `tenants`, `employees`, `loans`, `audit_trails`
- **Multi-tenant:** TODAS as tabelas multi-tenant devem incluir coluna `tenant_id VARCHAR(30) NOT NULL`

**Colunas:**
- **Formato:** `snake_case` para todas as colunas
- **IDs:** `id` (primary key), `{entity}_id` para foreign keys
- **Timestamps:** `created_at`, `updated_at`, `deleted_at` (soft delete)
- **Valores monetários:** SEMPRE `INTEGER` (em centavos), sufixo `_cents` opcional mas preferencial
  - Exemplo: `available_margin INTEGER NOT NULL` (armazena centavos)

**Indexes:**
- **Formato:** `{table}_{column(s)}_{tipo}_idx`
- **Exemplos:**
  - `employees_tenant_id_idx` (simple index)
  - `employees_tenant_cpf_unique_idx` (unique compound)
  - `loans_tenant_employee_idx` (composite index)
- **Multi-tenant:** Indexes compostos SEMPRE começam com `tenant_id` como primeira coluna

**Schemas Drizzle:**
```typescript
// ✅ BOM: Convenções consistentes
export const employees = pgTable('employees', {
  id: varchar('id', { length: 30 }).primaryKey(),
  tenantId: varchar('tenant_id', { length: 30 }).notNull(),
  cpf: varchar('cpf', { length: 11 }).notNull(),
  availableMargin: integer('available_margin').notNull(), // centavos
}, (table) => ({
  tenantIdIdx: index('employees_tenant_id_idx').on(table.tenantId),
  cpfUnique: uniqueIndex('employees_tenant_cpf_unique_idx').on(table.tenantId, table.cpf),
}))

// ❌ EVITAR: Inconsistências
export const Employee = pgTable('Employee', { // ❌ PascalCase
  Id: varchar('Id'), // ❌ PascalCase em coluna
  tenant: varchar('tenant'), // ❌ Sem sufixo _id
  availableMarginReais: float('available_margin_reais'), // ❌ Float para dinheiro
})
```

#### API Naming Conventions (tRPC)

**Router Naming:**
- **Formato:** `{domain}Router` em PascalCase
- **Arquivo:** `{domain}.router.ts` em kebab-case
- **Exemplos:** `authRouter`, `employeesRouter`, `loansRouter`, `tokensRouter`

**Procedure Naming:**
- **Formato:** `camelCase`, verbo + substantivo quando aplicável
- **Queries (leitura):** `list`, `getById`, `findByCpf`, `queryMargin`
- **Mutations (escrita):** `create`, `update`, `delete`, `cancel`, `reserve`
- **Exemplos:**
  ```typescript
  export const employeesRouter = router({
    list: protectedProcedure.query(...),
    getById: protectedProcedure.input(...).query(...),
    create: protectedProcedure.input(...).mutation(...),
    importBulk: protectedProcedure.input(...).mutation(...),
  })
  ```

**Endpoint REST (Webhooks externos):**
- **Formato:** `/api/webhooks/{provider}/{action}` em kebab-case
- **Exemplos:** `/api/webhooks/clerk/organization-created`, `/api/webhooks/banking/cnab-received`

#### Code Naming Conventions

**Arquivos TypeScript:**
- **Componentes React:** `PascalCase.tsx` → `EmployeeCard.tsx`, `TokenDialog.tsx`
- **Hooks:** `use{Nome}.ts` → `useEmployeeMargin.ts`, `useTokenValidation.ts`
- **Services:** `{domain}.service.ts` → `auth.service.ts`, `margin.service.ts`
- **Routers tRPC:** `{domain}.router.ts` → `employees.router.ts`
- **Schemas Zod:** `{domain}.schema.ts` → `loan.schema.ts`
- **Drizzle Schema:** `schema.ts` (único arquivo em `packages/database/src/`)

**Variáveis e Funções:**
- **Variáveis:** `camelCase` → `availableMargin`, `employeeId`
- **Constantes:** `SCREAMING_SNAKE_CASE` → `MAX_TOKEN_ATTEMPTS`, `DEFAULT_MARGIN_PERCENT`
- **Funções:** `camelCase` com verbo → `calculateMargin()`, `validateToken()`, `lockEmployee()`
- **Classes:** `PascalCase` → `MarginCalculator`, `TokenValidator`, `CNABAdapter`

**Imports entre packages:**
```typescript
// ✅ BOM: Imports limpos com alias do monorepo
import { db } from '@fastconsig/database'
import { employees, tenants } from '@fastconsig/database/schema'
import { CreateLoanSchema } from '@fastconsig/shared/schemas'
import { Button } from '@fastconsig/ui'

// ❌ EVITAR: Imports relativos entre packages
import { db } from '../../../packages/database/src'
```

### Structure Patterns

#### Project Organization (Monorepo)

**Estrutura de Pastas:**
```
fastconsig/
├── apps/
│   ├── api/                  # NestJS API
│   │   └── src/
│   │       ├── modules/      # Bounded contexts
│   │       │   ├── auth/
│   │       │   │   ├── auth.service.ts
│   │       │   │   ├── auth.router.ts
│   │       │   │   └── __tests__/
│   │       │   ├── employees/
│   │       │   ├── tokens/
│   │       │   └── loans/
│   │       ├── infrastructure/  # ACL, Redis, etc
│   │       │   ├── banking-acl/
│   │       │   │   └── adapters/
│   │       │   │       ├── banco-brasil/
│   │       │   │       │   └── cnab240-v1.adapter.ts
│   │       │   │       └── caixa/
│   │       │   └── redis/
│   │       └── shared/       # Middleware, guards
│   ├── jobs/                 # BullMQ workers
│   └── web/                  # React frontend
│       └── src/
│           ├── features/     # Por funcionalidade
│           │   ├── auth/
│           │   │   ├── components/
│           │   │   ├── pages/
│           │   │   └── hooks/
│           │   ├── employees/
│           │   └── loans/
│           └── components/   # Componentes globais
└── packages/
    ├── database/             # Drizzle schema + client
    ├── shared/               # DTOs, schemas, utils
    ├── ui/                   # shadcn/ui components
    └── config/               # Configs compartilhadas
```

**Regras de Organização:**

1. **Módulos de Domínio (Backend):**
   - Cada bounded context tem sua pasta em `apps/api/src/modules/{domain}/`
   - Dentro de cada módulo: `{domain}.service.ts`, `{domain}.router.ts`, `{domain}.schema.ts`, `__tests__/`
   - **Proibido:** Imports diretos entre módulos de domínio (ex: `employees` não pode importar diretamente de `loans`)
   - **Permitido:** Comunicação via Domain Events ou via Services injetados

2. **Features (Frontend):**
   - Organização por feature em `apps/web/src/features/{feature}/`
   - Cada feature contém: `components/`, `pages/`, `hooks/`, `utils/`
   - Componentes compartilhados vão em `apps/web/src/components/`

3. **Testes:**
   - **Unit tests:** Co-localizados em `__tests__/` dentro do módulo
   - **Integration tests:** `apps/api/__tests__/integration/`
   - **E2E tests:** `apps/web/__tests__/e2e/`
   - **Naming:** `{nome}.test.ts` ou `{nome}.spec.ts` (consistente dentro de cada app)

#### Banking ACL Adapter Pattern

**Convenção de Naming:**
- **Pasta:** `apps/api/src/infrastructure/banking-acl/adapters/{banco-slug}/`
- **Arquivo:** `{layout}-{versao}.adapter.ts`
- **Classe:** `{Banco}{Layout}V{N}Adapter implements ILoanFileParser`

**Exemplo:**
```typescript
// apps/api/src/infrastructure/banking-acl/adapters/banco-brasil/cnab240-v1.adapter.ts
export class BancoBrasilCNAB240V1Adapter implements ILoanFileParser {
  parse(file: Buffer): Promise<LoanDTO[]> {
    // Implementação específica do layout CNAB240 v1 do Banco do Brasil
  }
}

// apps/api/src/infrastructure/banking-acl/adapters/caixa/excel-v2.adapter.ts
export class CaixaExcelV2Adapter implements ILoanFileParser {
  parse(file: Buffer): Promise<LoanDTO[]> {
    // Implementação específica do layout Excel v2 da Caixa
  }
}
```

**Registro de Adapters:**
```typescript
// apps/api/src/infrastructure/banking-acl/adapter-registry.ts
export const BANKING_ADAPTERS = {
  'banco-brasil': {
    cnab240: {
      v1: BancoBrasilCNAB240V1Adapter,
      v2: BancoBrasilCNAB240V2Adapter,
    },
  },
  caixa: {
    cnab400: {
      v1: CaixaCNAB400V1Adapter,
    },
    excel: {
      v2: CaixaExcelV2Adapter,
    },
  },
}
```

### Format Patterns

#### API Response Formats (tRPC)

**Success Response (tRPC retorna diretamente):**
```typescript
// ✅ BOM: tRPC retorna dados diretos
return {
  id: 'loan_123',
  amount: 50000, // centavos
  installments: 24,
  createdAt: new Date().toISOString(),
}

// ❌ EVITAR: Wrapper desnecessário no tRPC
return { data: { id: 'loan_123' }, error: null } // ❌ tRPC já faz isso
```

**Error Response (tRPC TRPCError):**
```typescript
// ✅ BOM: TRPCError com código específico
throw new TRPCError({
  code: 'BAD_REQUEST',
  message: 'Margem insuficiente',
  cause: { available: 1000, required: 1500 },
})

// Códigos padronizados:
// - UNAUTHORIZED: Token inválido ou expirado
// - FORBIDDEN: Permissão negada (RBAC)
// - BAD_REQUEST: Validação falhou (Zod)
// - NOT_FOUND: Recurso não existe
// - CONFLICT: Race condition (ex: Token já usado)
// - INTERNAL_SERVER_ERROR: Erro inesperado
```

**REST Webhooks (Externos):**
```typescript
// ✅ BOM: Wrapper para APIs externas REST
{
  success: true,
  data: { /* payload */ },
  timestamp: "2026-01-11T10:30:00Z"
}

// ✅ BOM: Erro para APIs externas REST
{
  success: false,
  error: {
    code: "INSUFFICIENT_MARGIN",
    message: "Margem insuficiente para averbação",
    details: { available: 1000, required: 1500 }
  },
  timestamp: "2026-01-11T10:30:00Z"
}
```

#### Data Exchange Formats

**JSON Field Naming:**
- **Internal (tRPC):** `camelCase` em TypeScript, serialização automática via tRPC
- **External (REST/CNAB):** `snake_case` quando comunicando com sistemas bancários legados
- **Database:** `snake_case` em colunas Drizzle, mapeamento automático para `camelCase` em TypeScript

**Date/Time:**
- **Storage (DB):** `TIMESTAMP WITH TIME ZONE` (UTC)
- **API (JSON):** ISO 8601 strings → `"2026-01-11T10:30:00.000Z"`
- **Frontend Display:** Formatado com `date-fns` ou `Intl.DateTimeFormat` para locale pt-BR

**Monetary Values:**
- **Storage:** `INTEGER` (centavos)
- **API:** `INTEGER` (centavos) com sufixo `Cents` no nome do campo quando não óbvio
  - Exemplo: `{ amountCents: 50000, installmentValueCents: 2083 }`
- **Frontend Display:** Formatado com `Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })`

**Boolean:**
- **Storage:** `BOOLEAN` (PostgreSQL nativo)
- **API/JSON:** `true` / `false` (nunca 1/0 ou "true"/"false")

### Communication Patterns

#### Domain Events Pattern

**Event Naming:**
- **Formato:** `{Aggregate}{PastTenseVerb}Event` em PascalCase
- **Exemplos:**
  - `LoanCreatedEvent`
  - `MarginRecalculatedEvent`
  - `TokenValidatedEvent`
  - `EmployeeImportedEvent`

**Event Payload Structure:**
```typescript
// ✅ BOM: Estrutura consistente
export interface DomainEvent {
  eventId: string
  eventType: string // "LoanCreatedEvent"
  aggregateId: string // ID da entidade afetada
  tenantId: string
  userId: string // Quem causou o evento
  timestamp: string // ISO 8601
  payload: Record<string, unknown> // Dados específicos do evento
  metadata: {
    correlationId: string
    causationId?: string
    ipAddress?: string
  }
}

// Exemplo concreto
const event: LoanCreatedEvent = {
  eventId: 'evt_abc123',
  eventType: 'LoanCreatedEvent',
  aggregateId: 'loan_xyz789',
  tenantId: 'tenant_123',
  userId: 'user_456',
  timestamp: '2026-01-11T10:30:00.000Z',
  payload: {
    employeeId: 'emp_789',
    amount: 50000,
    installments: 24,
  },
  metadata: {
    correlationId: 'req_correlation_123',
    ipAddress: '192.168.1.100',
  },
}
```

**Event Storage (Audit Log):**
```typescript
// packages/database/src/schema.ts
export const domainEventsLog = pgTable('domain_events_log', {
  id: varchar('id', { length: 30 }).primaryKey(),
  eventType: varchar('event_type', { length: 100 }).notNull(),
  aggregateId: varchar('aggregate_id', { length: 30 }).notNull(),
  tenantId: varchar('tenant_id', { length: 30 }).notNull(),
  payload: jsonb('payload').notNull(),
  metadata: jsonb('metadata').notNull(),
  createdAt: timestamp('created_at').defaultNow().notNull(),
}, (table) => ({
  eventTypeIdx: index('domain_events_log_event_type_idx').on(table.eventType),
  aggregateIdx: index('domain_events_log_aggregate_idx').on(table.aggregateId),
}))
```

#### Redis Key Naming Pattern

**Convenção:**
- **Formato:** `{namespace}:{entity}:{identifier}:{suffix?}`
- **Namespaces:**
  - `token:` → Sistema de tokens
  - `rate:` → Rate limiting
  - `cache:` → Cache de dados
  - `session:` → Sessions (se usar Redis em vez de Clerk)

**Exemplos:**
```typescript
// Token storage
const tokenKey = `token:${employeeId}:${hashToken}`
await redis.set(tokenKey, JSON.stringify(metadata), 'EX', 3600)

// Rate limiting
const rateLimitKeyCPF = `rate:token:cpf:${cpf}`
const rateLimitKeyIP = `rate:token:ip:${ipAddress}`
await redis.incr(rateLimitKeyCPF)
await redis.expire(rateLimitKeyCPF, 600) // 10 min

// Cache de margem
const marginCacheKey = `cache:margin:${employeeId}`
await redis.set(marginCacheKey, JSON.stringify({ available, used }), 'EX', 300)
```

#### State Management Pattern (Zustand)

**Store Naming:**
- **Formato:** `use{Domain}Store` → `useAuthStore`, `useEmployeeStore`, `useLoanStore`
- **Arquivo:** `{domain}.store.ts` → `auth.store.ts`

**State Structure:**
```typescript
// ✅ BOM: Estrutura consistente
interface EmployeeStore {
  // State
  employees: Employee[]
  selectedEmployee: Employee | null
  isLoading: boolean
  error: string | null

  // Actions
  fetchEmployees: () => Promise<void>
  selectEmployee: (id: string) => void
  clearError: () => void
}

export const useEmployeeStore = create<EmployeeStore>((set, get) => ({
  // Initial state
  employees: [],
  selectedEmployee: null,
  isLoading: false,
  error: null,

  // Actions
  fetchEmployees: async () => {
    set({ isLoading: true, error: null })
    try {
      const data = await trpc.employees.list.query()
      set({ employees: data, isLoading: false })
    } catch (err) {
      set({ error: err.message, isLoading: false })
    }
  },
  // ...
}))
```

### Process Patterns

#### Error Handling Pattern

**Backend (NestJS + tRPC):**
```typescript
// ✅ BOM: TRPCError com código específico e contexto
export const loansRouter = router({
  create: protectedProcedure
    .input(CreateLoanSchema)
    .mutation(async ({ input, ctx }) => {
      try {
        // Validação de negócio
        const employee = await ctx.db.query.employees.findFirst({
          where: eq(employees.id, input.employeeId),
        })

        if (!employee) {
          throw new TRPCError({
            code: 'NOT_FOUND',
            message: 'Funcionário não encontrado',
          })
        }

        if (employee.availableMargin < input.installmentValue) {
          throw new TRPCError({
            code: 'BAD_REQUEST',
            message: 'Margem insuficiente',
            cause: {
              available: employee.availableMargin,
              required: input.installmentValue,
            },
          })
        }

        // Lógica de criação...
        return loan
      } catch (error) {
        // Re-throw TRPCError
        if (error instanceof TRPCError) throw error

        // Wrap erros desconhecidos
        throw new TRPCError({
          code: 'INTERNAL_SERVER_ERROR',
          message: 'Erro ao criar empréstimo',
          cause: error,
        })
      }
    }),
})
```

**Frontend (React):**
```typescript
// ✅ BOM: Error boundary + toast para erros de API
const CreateLoanForm = () => {
  const { toast } = useToast()
  const createLoan = trpc.loans.create.useMutation({
    onSuccess: () => {
      toast({ title: 'Empréstimo criado com sucesso' })
    },
    onError: (error) => {
      // tRPC error já vem tipado
      if (error.data?.code === 'BAD_REQUEST') {
        toast({
          variant: 'destructive',
          title: 'Margem insuficiente',
          description: error.message,
        })
      } else {
        toast({
          variant: 'destructive',
          title: 'Erro ao criar empréstimo',
          description: 'Tente novamente mais tarde',
        })
      }
    },
  })

  return <form onSubmit={...}>...</form>
}
```

#### Loading State Pattern

**Convenção:**
- **Naming:** `isLoading`, `isPending`, `isFetching` (conforme semântica)
- **Global:** Loading em `useAuthStore` para operações críticas (login, etc)
- **Local:** Loading por feature com tRPC hooks (`trpc.employees.list.useQuery({ enabled: !!tenantId })`)

**Skeleton Loading:**
```typescript
// ✅ BOM: Skeleton consistente
const EmployeeList = () => {
  const { data, isLoading } = trpc.employees.list.useQuery()

  if (isLoading) {
    return (
      <div className="space-y-4">
        {[...Array(5)].map((_, i) => (
          <Skeleton key={i} className="h-16 w-full" />
        ))}
      </div>
    )
  }

  return <div>{data.map(employee => <EmployeeCard key={employee.id} {...employee} />)}</div>
}
```

### Enforcement Guidelines

#### Regras Obrigatórias para TODOS os Agentes de IA:

1. **Multi-Tenancy Obrigatório:**
   - SEMPRE incluir `tenant_id` em queries de leitura
   - SEMPRE usar Context Middleware para extrair `tenant_id` do JWT
   - NUNCA permitir queries cross-tenant sem permissão explícita de Super Admin

2. **Pessimistic Locking para Margem:**
   - SEMPRE usar `.for('update')` (Drizzle) ao atualizar margem
   - SEMPRE recalcular margem dentro da mesma transação do update

3. **Token Validation Atômica:**
   - SEMPRE usar `redis.getDel()` (atomic get + delete) para validar tokens
   - NUNCA usar GET seguido de DEL separadamente (race condition)

4. **Audit Trail Append-Only:**
   - SEMPRE criar registro em `audit_trails` para mutations financeiras
   - NUNCA fazer UPDATE ou DELETE em `audit_trails` (append-only)

5. **Domain Events Idempotentes:**
   - SEMPRE incluir `eventId` único nos Domain Events
   - Handlers DEVEM ser idempotentes (podem ser chamados múltiplas vezes)

6. **Banking ACL Isolation:**
   - NUNCA misturar lógica de parsing CNAB com lógica de domínio
   - SEMPRE usar adapters da camada `infrastructure/banking-acl/`
   - Adapters DEVEM retornar DTOs limpos (`LoanDTO`, não raw data)

7. **Monetary Values em Centavos:**
   - SEMPRE armazenar valores monetários como `INTEGER` (centavos)
   - SEMPRE formatar para exibição usando `Intl.NumberFormat`

8. **Type-Safety End-to-End:**
   - SEMPRE usar schemas Zod para validação de inputs
   - SEMPRE inferir types via `z.infer<typeof Schema>`
   - SEMPRE compartilhar schemas via `@fastconsig/shared`

9. **Error Handling Consistente:**
   - Backend: SEMPRE usar `TRPCError` com códigos padronizados
   - Frontend: SEMPRE usar `useToast` para feedback de erro

10. **Correlation IDs:**
    - SEMPRE propagar `correlationId` em logs, events e traces
    - SEMPRE incluir em metadata de Domain Events

#### Pattern Enforcement

**Verificação via Linting:**
```json
// .eslintrc.json (root)
{
  "rules": {
    "no-restricted-imports": [
      "error",
      {
        "patterns": [
          {
            "group": ["../../*"],
            "message": "Use package aliases (@fastconsig/*) instead of relative imports between packages"
          }
        ]
      }
    ]
  }
}
```

**Verificação via Tests:**
- Unit tests DEVEM verificar conformidade com padrões (ex: `expect(table.columns).toContain('tenant_id')`)
- Integration tests DEVEM verificar multi-tenancy isolation
- E2E tests DEVEM verificar error handling e loading states

**Documentação de Violações:**
- Criar issues no GitHub com label `pattern-violation`
- Incluir referência à regra violada e código problemático
- Propor fix alinhado com padrões documentados

**Atualização de Padrões:**
- Propor mudanças via PR no arquivo `architecture.md`
- Requerer aprovação do arquiteto antes de merge
- Atualizar todos os agentes com novo padrão após merge

### Pattern Examples

#### Exemplos Corretos (✅ BOM):

**1. Multi-tenant Query com Drizzle:**
```typescript
// apps/api/src/modules/employees/employees.service.ts
export class EmployeesService {
  constructor(
    private db: Database,
    private cls: AsyncLocalStorage<{ tenantId: string }>,
  ) {}

  async list() {
    const { tenantId } = this.cls.getStore()!

    return this.db
      .select()
      .from(employees)
      .where(eq(employees.tenantId, tenantId)) // ✅ Filtro por tenant
  }
}
```

**2. Token Validation Atômica:**
```typescript
// apps/api/src/modules/tokens/tokens.service.ts
async validateAndConsume(code: string, employeeId: string): Promise<boolean> {
  const hash = createHash('sha256').update(code).digest('hex')
  const key = `token:${employeeId}:${hash}`

  // ✅ Atomic get + delete
  const metadata = await this.redis.getDel(key)

  if (!metadata) {
    throw new TRPCError({ code: 'UNAUTHORIZED', message: 'Token inválido ou expirado' })
  }

  // ✅ Update em DB dentro de transaction
  await this.db.transaction(async (tx) => {
    await tx.update(tokens).set({ status: 'USED' }).where(eq(tokens.id, metadata.tokenId))
  })

  return true
}
```

**3. Banking ACL Adapter:**
```typescript
// apps/api/src/infrastructure/banking-acl/adapters/banco-brasil/cnab240-v1.adapter.ts
export class BancoBrasilCNAB240V1Adapter implements ILoanFileParser {
  async parse(file: Buffer): Promise<LoanDTO[]> {
    const lines = file.toString('latin1').split('\n')
    const loans: LoanDTO[] = []

    for (const line of lines) {
      if (line.substring(7, 8) === '3') { // Registro tipo 3 = Detalhe
        loans.push({
          cpf: line.substring(10, 21),
          enrollment: line.substring(22, 32),
          contractNumber: line.substring(33, 53),
          amountCents: parseInt(line.substring(54, 68)), // ✅ Centavos
          installments: parseInt(line.substring(69, 71)),
        })
      }
    }

    return loans // ✅ DTOs limpos, sem lógica de domínio aqui
  }
}
```

#### Anti-Patterns (❌ EVITAR):

**1. Query sem filtro de tenant:**
```typescript
// ❌ MAU: Vaza dados cross-tenant
async list() {
  return this.db.select().from(employees) // ❌ Sem WHERE tenant_id
}
```

**2. Token validation com race condition:**
```typescript
// ❌ MAU: GET e DEL separados permitem double-spend
const metadata = await this.redis.get(key) // ❌ Não atômico
if (metadata) {
  await this.redis.del(key) // ❌ Outro request pode validar entre GET e DEL
}
```

**3. Valores monetários em Float:**
```typescript
// ❌ MAU: Float perde precisão
export const loans = pgTable('loans', {
  amount: real('amount'), // ❌ Float/Real não é adequado para dinheiro
})
```

**4. Lógica de CNAB no domínio:**
```typescript
// ❌ MAU: Parsing CNAB no service de domínio
export class LoansService {
  async importCNAB(file: Buffer) {
    const lines = file.toString('latin1').split('\n') // ❌ Parsing aqui contamina domínio
    // ...
  }
}

// ✅ BOM: Use adapter
export class LoansService {
  async importCNAB(file: Buffer) {
    const adapter = this.adapterRegistry.get('banco-brasil', 'cnab240', 'v1')
    const loans = await adapter.parse(file) // ✅ Adapter isola complexidade
    // ...
  }
}
```

**5. Imports relativos entre packages:**
```typescript
// ❌ MAU: Import relativo entre packages
import { db } from '../../../packages/database/src/db'

// ✅ BOM: Import via alias do monorepo
import { db } from '@fastconsig/database'
```

---

## Project Structure & Boundaries

### Complete Project Directory Structure

```
fastconsig/
├── .github/
│   ├── workflows/
│   │   ├── ci.yml                          # CI pipeline com Turborepo cache
│   │   ├── deploy-api.yml                  # Deploy API para Oracle Cloud
│   │   ├── deploy-web.yml                  # Deploy Web para Oracle Cloud
│   │   └── test.yml                        # Test suite completo
│   └── dependabot.yml
│
├── .vscode/
│   ├── settings.json                       # Workspace settings
│   ├── extensions.json                     # Recommended extensions
│   └── launch.json                         # Debug configurations
│
├── apps/
│   ├── api/                                # NestJS API + tRPC
│   │   ├── src/
│   │   │   ├── main.ts                     # Entry point (Fastify adapter)
│   │   │   ├── app.module.ts               # Root module
│   │   │   ├── app.ts                      # App configuration
│   │   │   ├── exports.ts                  # Public exports
│   │   │   │
│   │   │   ├── config/
│   │   │   │   ├── env.ts                  # Environment validation (Zod)
│   │   │   │   ├── database.config.ts      # Drizzle config
│   │   │   │   ├── redis.config.ts         # Redis config
│   │   │   │   └── clerk.config.ts         # Clerk SDK config
│   │   │   │
│   │   │   ├── modules/                    # Bounded Contexts (Hexagonal)
│   │   │   │   ├── auth/
│   │   │   │   │   ├── auth.service.ts
│   │   │   │   │   ├── auth.router.ts      # tRPC router
│   │   │   │   │   ├── auth.schema.ts      # Zod schemas
│   │   │   │   │   ├── clerk.strategy.ts   # Clerk integration
│   │   │   │   │   └── __tests__/
│   │   │   │   │       ├── auth.service.test.ts
│   │   │   │   │       └── auth.router.test.ts
│   │   │   │   │
│   │   │   │   ├── tenants/
│   │   │   │   │   ├── tenants.service.ts
│   │   │   │   │   ├── tenants.router.ts
│   │   │   │   │   ├── tenants.schema.ts
│   │   │   │   │   └── __tests__/
│   │   │   │   │
│   │   │   │   ├── employees/
│   │   │   │   │   ├── employees.service.ts
│   │   │   │   │   ├── employees.router.ts
│   │   │   │   │   ├── employees.schema.ts
│   │   │   │   │   ├── margin-calculator.service.ts
│   │   │   │   │   └── __tests__/
│   │   │   │   │
│   │   │   │   ├── tokens/
│   │   │   │   │   ├── tokens.service.ts
│   │   │   │   │   ├── tokens.router.ts
│   │   │   │   │   ├── tokens.schema.ts
│   │   │   │   │   ├── token-generator.service.ts
│   │   │   │   │   ├── token-validator.service.ts
│   │   │   │   │   └── __tests__/
│   │   │   │   │
│   │   │   │   ├── loans/
│   │   │   │   │   ├── loans.service.ts
│   │   │   │   │   ├── loans.router.ts
│   │   │   │   │   ├── loans.schema.ts
│   │   │   │   │   ├── margin-query.service.ts
│   │   │   │   │   └── __tests__/
│   │   │   │   │
│   │   │   │   ├── importacao/
│   │   │   │   │   ├── importacao.service.ts
│   │   │   │   │   ├── importacao.router.ts
│   │   │   │   │   ├── importacao.schema.ts
│   │   │   │   │   ├── file-validator.service.ts
│   │   │   │   │   └── __tests__/
│   │   │   │   │
│   │   │   │   └── payroll/
│   │   │   │       ├── payroll.service.ts
│   │   │   │       ├── payroll.router.ts
│   │   │   │       ├── payroll.schema.ts
│   │   │   │       ├── reconciliation.service.ts
│   │   │   │       └── __tests__/
│   │   │   │
│   │   │   ├── infrastructure/             # ACL, External Services
│   │   │   │   ├── banking-acl/
│   │   │   │   │   ├── interfaces/
│   │   │   │   │   │   └── ILoanFileParser.ts
│   │   │   │   │   ├── adapters/
│   │   │   │   │   │   ├── banco-brasil/
│   │   │   │   │   │   │   ├── cnab240-v1.adapter.ts
│   │   │   │   │   │   │   ├── cnab240-v2.adapter.ts
│   │   │   │   │   │   │   └── __tests__/
│   │   │   │   │   │   ├── caixa/
│   │   │   │   │   │   │   ├── cnab400-v1.adapter.ts
│   │   │   │   │   │   │   ├── excel-v2.adapter.ts
│   │   │   │   │   │   │   └── __tests__/
│   │   │   │   │   │   └── bradesco/
│   │   │   │   │   │       └── cnab240-v1.adapter.ts
│   │   │   │   │   ├── validators/
│   │   │   │   │   │   ├── file-validator.ts
│   │   │   │   │   │   └── schema-validator.ts
│   │   │   │   │   └── adapter-registry.ts
│   │   │   │   │
│   │   │   │   ├── redis/
│   │   │   │   │   ├── redis.service.ts
│   │   │   │   │   └── redis.module.ts
│   │   │   │   │
│   │   │   │   ├── storage/
│   │   │   │   │   ├── storage.service.ts  # Oracle Cloud Object Storage
│   │   │   │   │   └── storage.module.ts
│   │   │   │   │
│   │   │   │   └── notifications/
│   │   │   │       ├── sms.service.ts
│   │   │   │       ├── email.service.ts
│   │   │   │       └── webhook.service.ts
│   │   │   │
│   │   │   ├── shared/                     # Cross-cutting concerns
│   │   │   │   ├── middleware/
│   │   │   │   │   ├── tenant-context.middleware.ts
│   │   │   │   │   ├── correlation-id.middleware.ts
│   │   │   │   │   └── logger.middleware.ts
│   │   │   │   │
│   │   │   │   ├── guards/
│   │   │   │   │   ├── clerk-auth.guard.ts
│   │   │   │   │   ├── permissions.guard.ts
│   │   │   │   │   └── rate-limit.guard.ts
│   │   │   │   │
│   │   │   │   ├── decorators/
│   │   │   │   │   ├── current-user.decorator.ts
│   │   │   │   │   ├── current-tenant.decorator.ts
│   │   │   │   │   └── permissions.decorator.ts
│   │   │   │   │
│   │   │   │   ├── interceptors/
│   │   │   │   │   ├── audit-log.interceptor.ts
│   │   │   │   │   └── transform.interceptor.ts
│   │   │   │   │
│   │   │   │   └── services/
│   │   │   │       ├── audit-trail.service.ts
│   │   │   │       ├── domain-events.service.ts
│   │   │   │       └── margin-calculator.service.ts
│   │   │   │
│   │   │   ├── trpc/
│   │   │   │   ├── trpc.module.ts
│   │   │   │   ├── trpc.router.ts          # Root tRPC router
│   │   │   │   ├── trpc.context.ts         # Context builder
│   │   │   │   └── trpc.middleware.ts      # Auth middleware
│   │   │   │
│   │   │   └── types/
│   │   │       ├── express.d.ts
│   │   │       └── clerk.d.ts
│   │   │
│   │   ├── __tests__/
│   │   │   ├── integration/
│   │   │   │   ├── auth.integration.test.ts
│   │   │   │   ├── employees.integration.test.ts
│   │   │   │   └── loans.integration.test.ts
│   │   │   └── e2e/
│   │   │       └── api.e2e.test.ts
│   │   │
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   ├── jest.config.js
│   │   └── nest-cli.json
│   │
│   ├── jobs/                               # BullMQ Background Workers
│   │   ├── src/
│   │   │   ├── main.ts                     # Worker entry point
│   │   │   ├── jobs/
│   │   │   │   ├── employee-import.job.ts  # Bulk employee import
│   │   │   │   ├── token-delivery.job.ts   # Token SMS/Email delivery
│   │   │   │   ├── bulk-import.job.ts      # CNAB bulk processing
│   │   │   │   ├── margin-recalc.job.ts    # Margin recalculation
│   │   │   │   └── __tests__/
│   │   │   │       └── employee-import.job.test.ts
│   │   │   │
│   │   │   ├── processors/
│   │   │   │   ├── stream-processor.ts     # CSV/Excel stream processing
│   │   │   │   └── batch-processor.ts      # Batch operations
│   │   │   │
│   │   │   └── config/
│   │   │       ├── queue.config.ts
│   │   │       └── redis.config.ts
│   │   │
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   └── jest.config.js
│   │
│   └── web/                                # React 19 Frontend
│       ├── src/
│       │   ├── main.tsx                    # Entry point
│       │   ├── App.tsx
│       │   │
│       │   ├── features/                   # Feature-based organization
│       │   │   ├── auth/
│       │   │   │   ├── components/
│       │   │   │   │   ├── LoginForm.tsx
│       │   │   │   │   ├── ForgotPasswordForm.tsx
│       │   │   │   │   └── PasswordResetForm.tsx
│       │   │   │   ├── pages/
│       │   │   │   │   ├── LoginPage.tsx
│       │   │   │   │   └── RegisterPage.tsx
│       │   │   │   ├── hooks/
│       │   │   │   │   └── useAuth.ts
│       │   │   │   └── stores/
│       │   │   │       └── auth.store.ts   # Zustand store
│       │   │   │
│       │   │   ├── employees/
│       │   │   │   ├── components/
│       │   │   │   │   ├── EmployeeCard.tsx
│       │   │   │   │   ├── EmployeeList.tsx
│       │   │   │   │   ├── EmployeeForm.tsx
│       │   │   │   │   └── BulkImportDialog.tsx
│       │   │   │   ├── pages/
│       │   │   │   │   ├── EmployeesPage.tsx
│       │   │   │   │   └── EmployeeDetailPage.tsx
│       │   │   │   ├── hooks/
│       │   │   │   │   └── useEmployees.ts
│       │   │   │   └── stores/
│       │   │   │       └── employees.store.ts
│       │   │   │
│       │   │   ├── tokens/
│       │   │   │   ├── components/
│       │   │   │   │   ├── TokenManagement.tsx
│       │   │   │   │   └── TokenHistory.tsx
│       │   │   │   ├── pages/
│       │   │   │   │   └── TokensPage.tsx
│       │   │   │   └── hooks/
│       │   │   │       └── useTokens.ts
│       │   │   │
│       │   │   ├── loans/
│       │   │   │   ├── components/
│       │   │   │   │   ├── LoanForm.tsx
│       │   │   │   │   ├── LoanCard.tsx
│       │   │   │   │   └── MarginDisplay.tsx
│       │   │   │   ├── pages/
│       │   │   │   │   ├── LoansPage.tsx
│       │   │   │   │   └── NewLoanPage.tsx
│       │   │   │   └── hooks/
│       │   │   │       └── useLoans.ts
│       │   │   │
│       │   │   ├── importacao/
│       │   │   │   ├── components/
│       │   │   │   │   ├── FileUpload.tsx
│       │   │   │   │   ├── ImportProgress.tsx
│       │   │   │   │   └── ImportResults.tsx
│       │   │   │   ├── pages/
│       │   │   │   │   └── ImportPage.tsx
│       │   │   │   └── hooks/
│       │   │   │       └── useImport.ts
│       │   │   │
│       │   │   ├── payroll/
│       │   │   │   ├── components/
│       │   │   │   │   ├── PayrollDashboard.tsx
│       │   │   │   │   ├── DivergenceList.tsx
│       │   │   │   │   └── ConsolidatedReport.tsx
│       │   │   │   ├── pages/
│       │   │   │   │   └── PayrollPage.tsx
│       │   │   │   └── hooks/
│       │   │   │       └── usePayroll.ts
│       │   │   │
│       │   │   └── admin/
│       │   │       ├── components/
│       │   │   │       ├── TenantForm.tsx
│       │   │   │       └── ConfigurationPanel.tsx
│       │   │       └── pages/
│       │   │           └── AdminPage.tsx
│       │   │
│       │   ├── components/                 # Shared components
│       │   │   ├── ui/                     # shadcn/ui components
│       │   │   │   ├── button.tsx
│       │   │   │   ├── dialog.tsx
│       │   │   │   ├── input.tsx
│       │   │   │   ├── table.tsx
│       │   │   │   └── toast.tsx
│       │   │   ├── layout/
│       │   │   │   ├── DashboardLayout.tsx
│       │   │   │   ├── Sidebar.tsx
│       │   │   │   └── Header.tsx
│       │   │   └── common/
│       │   │       ├── DataTable.tsx
│       │   │       ├── LoadingSpinner.tsx
│       │   │       └── ErrorBoundary.tsx
│       │   │
│       │   ├── lib/
│       │   │   ├── trpc.ts                 # tRPC client setup
│       │   │   ├── clerk.ts                # Clerk React integration
│       │   │   └── utils.ts                # Utility functions
│       │   │
│       │   ├── routes/                     # TanStack Router routes
│       │   │   ├── __root.tsx
│       │   │   ├── _authenticated/
│       │   │   │   ├── employees.tsx
│       │   │   │   ├── loans.tsx
│       │   │   │   ├── tokens.tsx
│       │   │   │   ├── importacao.tsx
│       │   │   │   └── payroll.tsx
│       │   │   └── _public/
│       │   │       ├── login.tsx
│       │   │       └── register.tsx
│       │   │
│       │   ├── hooks/                      # Global hooks
│       │   │   ├── useToast.ts
│       │   │   └── useDebounce.ts
│       │   │
│       │   ├── styles/
│       │   │   └── globals.css
│       │   │
│       │   └── types/
│       │       └── global.d.ts
│       │
│       ├── public/
│       │   ├── favicon.ico
│       │   └── assets/
│       │
│       ├── index.html
│       ├── package.json
│       ├── tsconfig.json
│       ├── vite.config.ts
│       ├── tailwind.config.ts
│       └── postcss.config.js
│
├── packages/
│   ├── database/                           # Drizzle ORM + Schema
│   │   ├── src/
│   │   │   ├── index.ts                    # Export db client
│   │   │   ├── db.ts                       # Drizzle client
│   │   │   ├── schema.ts                   # Main schema file
│   │   │   ├── schema/                     # Schema por domínio
│   │   │   │   ├── tenants.ts
│   │   │   │   ├── employees.ts
│   │   │   │   ├── loans.ts
│   │   │   │   ├── tokens.ts
│   │   │   │   ├── audit-trails.ts
│   │   │   │   └── domain-events-log.ts
│   │   │   └── migrations/
│   │   │       └── migrate.ts              # Migration runner
│   │   │
│   │   ├── drizzle/                        # Generated migrations
│   │   │   └── meta/
│   │   │
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   └── drizzle.config.ts
│   │
│   ├── shared/                             # Shared DTOs, Schemas, Utils
│   │   ├── src/
│   │   │   ├── index.ts
│   │   │   ├── schemas/                    # Zod schemas
│   │   │   │   ├── auth.schema.ts
│   │   │   │   ├── employee.schema.ts
│   │   │   │   ├── loan.schema.ts
│   │   │   │   ├── token.schema.ts
│   │   │   │   └── payroll.schema.ts
│   │   │   │
│   │   │   ├── types/                      # TypeScript types
│   │   │   │   ├── auth.types.ts
│   │   │   │   ├── employee.types.ts
│   │   │   │   └── loan.types.ts
│   │   │   │
│   │   │   ├── constants/
│   │   │   │   ├── permissions.ts
│   │   │   │   ├── roles.ts
│   │   │   │   └── error-codes.ts
│   │   │   │
│   │   │   └── utils/
│   │   │       ├── cpf.ts                  # CPF validation/formatting
│   │   │       ├── currency.ts             # Currency formatting (centavos)
│   │   │       └── date.ts                 # Date utilities
│   │   │
│   │   ├── package.json
│   │   └── tsconfig.json
│   │
│   ├── ui/                                 # shadcn/ui components
│   │   ├── src/
│   │   │   ├── components/
│   │   │   │   ├── button.tsx
│   │   │   │   ├── dialog.tsx
│   │   │   │   ├── input.tsx
│   │   │   │   ├── table.tsx
│   │   │   │   ├── toast.tsx
│   │   │   │   └── dropdown-menu.tsx
│   │   │   │
│   │   │   ├── hooks/
│   │   │   │   └── use-toast.ts
│   │   │   │
│   │   │   └── lib/
│   │   │       └── utils.ts
│   │   │
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   └── tailwind.config.ts
│   │
│   └── config/                             # Shared configs
│       ├── eslint-config-custom/
│       │   ├── index.js
│       │   └── package.json
│       │
│       ├── typescript-config/
│       │   ├── base.json
│       │   ├── nextjs.json
│       │   ├── react.json
│       │   └── package.json
│       │
│       └── tailwind-config/
│           ├── index.ts
│           └── package.json
│
├── docker/
│   ├── Dockerfile.api                      # API production image
│   ├── Dockerfile.jobs                     # Jobs worker image
│   ├── Dockerfile.web                      # Web frontend image
│   └── docker-compose.yml                  # Local development stack
│
├── docs/
│   ├── architecture/
│   │   ├── ADRs/
│   │   ├── diagrams/
│   │   └── patterns.md
│   ├── api/
│   │   └── openapi.yml                     # REST webhooks docs
│   └── development/
│       └── getting-started.md
│
├── scripts/
│   ├── setup.sh                            # Initial project setup
│   ├── seed-db.ts                          # Database seeding
│   └── generate-types.ts                   # Type generation
│
├── .gitignore
├── .env.example
├── .nvmrc
├── package.json                            # Root package.json
├── pnpm-workspace.yaml                     # pnpm workspaces
├── pnpm-lock.yaml
├── turbo.json                              # Turborepo pipeline
├── tsconfig.json                           # Root TypeScript config
├── README.md
└── LICENSE
```

### Architectural Boundaries

#### API Boundaries

**External API (tRPC - Frontend ↔ API):**
- **Endpoint:** `http://api.fastconsig.com/trpc`
- **Protocol:** HTTP POST (tRPC over HTTP)
- **Authentication:** JWT via Clerk (header `Authorization: Bearer <token>`)
- **Routers:**
  - `auth.*` → Authentication & session management
  - `tenants.*` → Tenant CRUD (Super Admin only)
  - `employees.*` → Employee management
  - `tokens.*` → Token lifecycle
  - `loans.*` → Loan operations
  - `importacao.*` → Bulk upload operations
  - `payroll.*` → Payroll reconciliation

**REST Webhooks (External Systems → API):**
- **Clerk Webhooks:** `POST /api/webhooks/clerk/*`
  - `organization.created` → Create tenant
  - `user.created` → Sync user metadata
- **Banking Webhooks:** `POST /api/webhooks/banking/*`
  - `cnab-received` → Process incoming CNAB files

**Internal Service Boundaries:**
- **Modules communicate via:**
  - NestJS Dependency Injection (Services)
  - Domain Events (via `domain-events.service.ts`)
- **Proibido:** Imports diretos entre módulos (`employees` → `loans`)
- **Permitido:** Shared services em `shared/services/`

#### Component Boundaries (Frontend)

**Feature Isolation:**
- Cada feature em `apps/web/src/features/{feature}/` é auto-contida
- Features compartilham apenas via:
  - `@fastconsig/shared` (types, schemas)
  - `@fastconsig/ui` (UI components)
  - Global stores (auth apenas)

**State Management:**
- **Global State (Zustand):**
  - `auth.store.ts` → Authentication state (único global)
- **Local State (tRPC hooks):**
  - Cada feature usa `trpc.{domain}.{procedure}.useQuery()` localmente
  - Não compartilha state entre features

**Component Communication:**
- Via URL routing (TanStack Router)
- Via tRPC mutations (server state)
- NUNCA via props drilling entre features

#### Service Boundaries (Backend)

**Module Boundaries:**
- Cada módulo em `apps/api/src/modules/{domain}/` é um bounded context
- **Interface pública:** `{domain}.router.ts` (tRPC procedures)
- **Interface privada:** `{domain}.service.ts` (injetado em outros serviços quando necessário)

**Banking ACL Boundary:**
- **Entrada:** Raw file buffer (CNAB/Excel)
- **Saída:** Clean DTOs (`LoanDTO[]`, `EmployeeDTO[]`)
- **Isolamento:** Nenhuma lógica de domínio dentro dos adapters
- **Registro:** `adapter-registry.ts` mapeia `{bank}-{layout}-{version}` → Adapter class

**Infrastructure Services:**
- Redis, Storage, Notifications são serviços de infraestrutura
- Acessados via injeção de dependência
- Nunca acessados diretamente por módulos de domínio

#### Data Boundaries

**Database Schema Isolation:**
- Todas as tabelas multi-tenant TÊM `tenant_id`
- RLS policies aplicadas automaticamente via PostgreSQL
- Context Middleware injeta `tenant_id` em session via `SET LOCAL app.tenant_id = '{id}'`

**Drizzle ORM Access:**
- Package `@fastconsig/database` exporta:
  - `db` (client)
  - Schema tables (`tenants`, `employees`, etc.)
- Queries SEMPRE filtradas por `tenant_id` via `where(eq(table.tenantId, ctx.tenantId))`

**Redis Cache Boundaries:**
- Namespaces obrigatórios: `token:`, `rate:`, `cache:`, `session:`
- Keys SEMPRE incluem `tenant_id` quando aplicável
- TTL obrigatório para todos os caches

**Event Store:**
- `domain_events_log` table (append-only)
- Todos os Domain Events persistidos aqui
- Partitioning por `created_at` (mensal) quando volume crescer

### Requirements to Structure Mapping

#### Epic 1: Platform Foundation & Identity
**Backend:**
- `apps/api/src/modules/auth/` → FR01, FR04 (Clerk integration, login blocking)
- `apps/api/src/modules/tenants/` → FR02, FR26 (Tenant CRUD, configuration)
- `apps/api/src/shared/guards/permissions.guard.ts` → FR03 (RBAC)
- `apps/api/src/shared/interceptors/audit-log.interceptor.ts` → FR15 (Audit trail)

**Frontend:**
- `apps/web/src/features/auth/` → Login, password reset
- `apps/web/src/features/admin/` → Tenant management (Super Admin)

**Database:**
- `packages/database/schema/tenants.ts`
- `packages/database/schema/audit-trails.ts`

#### Epic 2: Employee Management
**Backend:**
- `apps/api/src/modules/employees/` → FR05, FR06, FR07 (CRUD, margin calc, soft delete)
- `apps/api/src/modules/employees/margin-calculator.service.ts` → FR06

**Jobs:**
- `apps/jobs/src/jobs/employee-import.job.ts` → FR05 (Bulk import)

**Frontend:**
- `apps/web/src/features/employees/` → Employee list, form, bulk upload

**Database:**
- `packages/database/schema/employees.ts`

#### Epic 3: Transactional Integrity (Token System)
**Backend:**
- `apps/api/src/modules/tokens/` → FR09, FR11, FR25 (Generate, validate, cancel)
- `apps/api/src/infrastructure/redis/` → Token cache + rate limiting

**Jobs:**
- `apps/jobs/src/jobs/token-delivery.job.ts` → FR09 (SMS/Email delivery)

**Frontend:**
- `apps/web/src/features/tokens/` → Token management (RH)

**Database:**
- `packages/database/schema/tokens.ts`

#### Epic 4: Loan Operations
**Backend:**
- `apps/api/src/modules/loans/` → FR08, FR10 (Blind query, loan creation)
- `apps/api/src/modules/loans/margin-query.service.ts` → FR08

**Frontend:**
- `apps/web/src/features/loans/` → Loan forms, margin display

**Database:**
- `packages/database/schema/loans.ts`

#### Epic 5: Bulk Banking Integration
**Backend:**
- `apps/api/src/modules/importacao/` → FR12, FR13, FR14, FR27 (Upload, async processing)
- `apps/api/src/infrastructure/banking-acl/` → FR17 (Multi-layout support)

**Jobs:**
- `apps/jobs/src/jobs/bulk-import.job.ts` → FR13 (Async processing)

**Frontend:**
- `apps/web/src/features/importacao/` → File upload, progress tracking

**Infrastructure:**
- `apps/api/src/infrastructure/storage/` → File storage (Oracle Cloud Object Storage)

#### Epic 6: Payroll Reconciliation
**Backend:**
- `apps/api/src/modules/payroll/` → FR19, FR20, FR21 (File gen, divergence, reports)
- `apps/api/src/modules/payroll/reconciliation.service.ts` → FR20

**Frontend:**
- `apps/web/src/features/payroll/` → Payroll dashboard, divergence list

### Integration Points

#### Internal Communication

**Frontend → Backend:**
- **Protocol:** tRPC over HTTP
- **Authentication:** Clerk JWT in `Authorization` header
- **Client:** `apps/web/src/lib/trpc.ts` configures tRPC client
- **Type-safety:** Automatic via tRPC inference

**Backend → Jobs:**
- **Protocol:** BullMQ (Redis-backed queues)
- **Queues:**
  - `employee-import` → Bulk CSV processing
  - `token-delivery` → SMS/Email sending
  - `bulk-import` → CNAB file processing
  - `margin-recalc` → Reactive margin updates
- **Communication:** Jobs poll queues, API enqueues tasks

**Module → Module (Backend):**
- **Via Dependency Injection:** `EmployeesService` injected into `LoansService`
- **Via Domain Events:** `LoanCreatedEvent` → `MarginRecalculatedEvent` (handler)
- **Event Bus:** `domain-events.service.ts` coordinates event dispatch

#### External Integrations

**Clerk (Authentication):**
- **API:** `@clerk/clerk-sdk-node` in backend
- **React:** `@clerk/clerk-react` in frontend
- **Webhook:** `POST /api/webhooks/clerk/*` for sync events

**Redis (Cache & Queues):**
- **Client:** `ioredis`
- **Usage:** Token cache, rate limiting, BullMQ queues
- **Host:** Oracle Cloud VM (self-hosted)

**PostgreSQL (Database):**
- **Client:** Drizzle ORM (`drizzle-orm/node-postgres`)
- **Host:** Oracle Cloud Database service
- **Migrations:** Drizzle Kit (`drizzle-kit push` in dev, `migrate.ts` in prod)

**Oracle Cloud Object Storage (File Storage):**
- **SDK:** Oracle Cloud Infrastructure SDK
- **Usage:** CNAB files, import results, receipts
- **Service:** `apps/api/src/infrastructure/storage/storage.service.ts`

**SMS/Email Providers:**
- **SMS:** Twilio SDK (configured in `notifications/sms.service.ts`)
- **Email:** SendGrid SDK (configured in `notifications/email.service.ts`)

#### Data Flow

**Typical Loan Creation Flow:**
1. **User (Frontend):** Submits loan form → `trpc.loans.create.useMutation()`
2. **tRPC Middleware:** Validates JWT → extracts `tenantId` + `userId`
3. **Loans Service:** Validates token → locks employee → creates loan
4. **Domain Event:** Raises `LoanCreatedEvent`
5. **Margin Service:** Handler recalculates margin reactively
6. **Audit Interceptor:** Logs mutation to `audit_trails`
7. **Response:** Returns loan DTO to frontend
8. **Frontend:** Updates UI + shows toast

**Bulk Import Flow:**
1. **User (Frontend):** Uploads CSV → `trpc.importacao.upload.useMutation()`
2. **Importacao Service:** Validates file → stores in Object Storage → enqueues job
3. **BullMQ:** Job picked up by `apps/jobs` worker
4. **Job Worker:** Streams file → processes line-by-line → uses Banking ACL if needed
5. **Progress Updates:** Job updates progress in Redis
6. **Frontend:** Polls `trpc.importacao.getProgress.useQuery()` for status
7. **Completion:** Generates result file → stores in Object Storage → notifies user

### File Organization Patterns

#### Configuration Files

**Root Level:**
- `package.json` → Workspaces, scripts, dev dependencies
- `pnpm-workspace.yaml` → Define apps/* e packages/*
- `turbo.json` → Build pipeline (build, dev, test, lint, db:*)
- `tsconfig.json` → Base TypeScript config (extended by apps/packages)
- `.env.example` → Template for environment variables

**Per-App Configs:**
- `apps/api/tsconfig.json` → Extends root, adds NestJS paths
- `apps/web/vite.config.ts` → Vite bundler config
- `apps/web/tailwind.config.ts` → Tailwind CSS config
- `packages/database/drizzle.config.ts` → Drizzle migrations config

#### Source Organization

**Backend (Hexagonal Architecture):**
- `modules/` → Bounded contexts (1 pasta = 1 domínio)
- `infrastructure/` → External integrations (ACL, Redis, Storage)
- `shared/` → Cross-cutting (middleware, guards, decorators)
- `trpc/` → tRPC router configuration

**Frontend (Feature-based):**
- `features/` → Organized by user-facing feature
- `components/` → Shared UI components (layout, common)
- `routes/` → TanStack Router route definitions
- `lib/` → Client setup (tRPC, Clerk)

**Packages:**
- `database/` → Drizzle schema + client
- `shared/` → DTOs, schemas (Zod), types, utils
- `ui/` → shadcn/ui components
- `config/` → Shareable configs (ESLint, TypeScript, Tailwind)

#### Test Organization

**Backend Tests:**
- **Unit:** Co-located `__tests__/` inside each module
  - `modules/employees/__tests__/employees.service.test.ts`
- **Integration:** `apps/api/__tests__/integration/`
  - Tests full tRPC procedure flow (auth → validation → DB)
- **E2E:** `apps/api/__tests__/e2e/`
  - Tests via HTTP requests to running server

**Frontend Tests:**
- **Component:** Co-located `__tests__/` (future)
- **E2E:** `apps/web/__tests__/e2e/` with Playwright

**Jobs Tests:**
- **Unit:** Co-located `__tests__/` inside each job
  - `jobs/__tests__/employee-import.job.test.ts`

#### Asset Organization

**Static Assets:**
- `apps/web/public/assets/` → Images, fonts, static files
- Served directly by Vite in dev, by CDN in production

**Generated Assets:**
- `apps/api/dist/` → Compiled NestJS (gitignored)
- `apps/web/dist/` → Bundled React app (gitignored)
- `packages/database/drizzle/` → Generated migrations (committed)

### Development Workflow Integration

#### Development Server Structure

**Start All Services:**
```bash
pnpm dev  # Turborepo runs all dev scripts in parallel
```

**Individual Services:**
- `pnpm --filter api dev` → NestJS watch mode (port 3000)
- `pnpm --filter web dev` → Vite dev server (port 5173)
- `pnpm --filter jobs dev` → BullMQ worker watch mode

**Database:**
```bash
docker-compose up postgres redis  # Local PostgreSQL + Redis
pnpm db:push  # Apply Drizzle schema to DB
pnpm db:studio  # Open Drizzle Studio (GUI)
```

#### Build Process Structure

**Turborepo Pipeline (`turbo.json`):**
```json
{
  "pipeline": {
    "build": {
      "dependsOn": ["^build"],
      "outputs": ["dist/**", ".next/**"]
    },
    "db:generate": {
      "cache": false
    },
    "test": {
      "dependsOn": ["^build"]
    }
  }
}
```

**Build Order (automatic via Turborepo):**
1. `packages/database` → Drizzle client
2. `packages/shared` → DTOs, schemas
3. `packages/ui` → Components
4. `apps/api` → NestJS API
5. `apps/jobs` → Workers
6. `apps/web` → React app

#### Deployment Structure

**Oracle Cloud VMs:**
- **API Server:** Docker container from `docker/Dockerfile.api`
  - Image: `fastconsig-api:latest`
  - Port: 3000 (exposed via load balancer)
- **Jobs Worker:** Docker container from `docker/Dockerfile.jobs`
  - Image: `fastconsig-jobs:latest`
  - Connects to same Redis as API
- **Web Server:** Static files from `apps/web/dist/`
  - Served by Nginx
  - CDN: Oracle Cloud CDN for assets

**CI/CD (GitHub Actions):**
- `.github/workflows/ci.yml` → Run tests + build
- `.github/workflows/deploy-api.yml` → Build Docker image → push to registry → deploy to VM
- `.github/workflows/deploy-web.yml` → Build static site → upload to Object Storage → invalidate CDN

**Environment Variables:**
- **Development:** `.env.local` (gitignored)
- **Production:** Oracle Cloud Secrets Manager
- **CI/CD:** GitHub Secrets
