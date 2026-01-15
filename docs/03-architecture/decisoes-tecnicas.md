# Decisoes Tecnicas - FastConsig

**Versao:** 1.1
**Data:** Janeiro 2026
**Status:** Aprovado (Revisado com validacao de stakeholder)

---

## 1. Resumo Executivo

Este documento consolida as decisoes tecnicas para o novo sistema FastConsig, baseado nas respostas do stakeholder e analise da stack legada.

### 1.1 Contexto das Decisoes

| Aspecto | Decisao |
|---------|---------|
| **Perfil da Equipe** | Forte em TypeScript |
| **Infraestrutura** | Cloud-first (Oracle Cloud - OCI) |
| **Licenciamento** | Priorizar Open Source |
| **Containerizacao** | Docker + Kubernetes (experiencia) |
| **Provedor Cloud** | Oracle Cloud Infrastructure (OCI) |
| **Modelo de Deploy** | VMs com Docker/Docker Compose |
| **Escala Esperada** | Media (100-1000 usuarios, 5-50 tenants) |
| **Arquitetura** | Monolito modular |
| **Banco de Dados** | Migrar para PostgreSQL |
| **Repositorio** | Monorepo |
| **CI/CD** | GitHub Actions |
| **Testes** | Unitarios + Integracao + E2E (cobertura evolutiva) |

---

## 2. Stack Atual vs Nova Stack

### 2.1 Comparativo Geral

| Componente | Stack Legada | Nova Stack | Justificativa |
|------------|--------------|------------|---------------|
| **Runtime** | .NET Framework 4.0 | Node.js 22 LTS | Equipe JS/TS, performance, ecossistema |
| **Linguagem** | C# | TypeScript 5.5+ | Type safety, produtividade |
| **Frontend** | ASP.NET WebForms | React 19 + Vite | SPA moderna, componentizacao |
| **Backend** | WCF + ASMX | Fastify + tRPC | Performance, type safety end-to-end |
| **ORM** | Entity Framework 4.3 | Prisma ORM | Migrations, type safety, DX |
| **Banco** | SQL Server (multi-db) | PostgreSQL 16 (single + tenant_id) | Open source, features, custo |
| **UI Components** | DevExpress v11.1 | shadcn/ui + Radix | Open source, acessibilidade |
| **Autenticacao** | Forms Auth | JWT + Refresh Tokens | Stateless, escalavel |
| **Hospedagem** | IIS on-premise | Docker em OCI VMs | Cloud, portabilidade |

---

## 3. Decisoes por Categoria

---

### 3.1 ARQUITETURA

#### Stack Legada
- Monolito em camadas (WebApp -> Facade -> BLL -> DAL)
- Acoplamento forte entre camadas
- Multi-database por tenant

#### Alternativas Avaliadas

| Opcao | Descricao | Pros | Contras |
|-------|-----------|------|---------|
| **A) Monolito Modular** | Aplicacao unica com modulos bem definidos | Simplicidade, deploy unico, facil debug | Escala vertical, deploy total |
| **B) Backend for Frontend (BFF)** | API Gateway + Backend unificado | Flexibilidade de clientes, cache | Camada adicional |
| **C) Microsservicos** | Servicos independentes por dominio | Escala independente, isolamento | Complexidade operacional alta |
| **D) Serverless** | Functions para cada operacao | Escala automatica, pay-per-use | Cold start, vendor lock-in |

#### **DECISAO: A) Monolito Modular**

**Justificativa:**
- Escala media (100-1000 usuarios) nao justifica complexidade de microsservicos
- Equipe pode focar em features ao inves de infraestrutura
- Facilita debugging e desenvolvimento local
- Pode evoluir para microsservicos no futuro se necessario

**Estrutura de Modulos:**

```
src/
├── modules/
│   ├── auth/           # Autenticacao e autorizacao
│   ├── funcionarios/   # Gestao de servidores
│   ├── averbacoes/     # Ciclo de vida de emprestimos
│   ├── margem/         # Calculo e gestao de margem
│   ├── simulacao/      # Simulador de emprestimos
│   ├── conciliacao/    # Processo de conciliacao
│   ├── consignatarias/ # Gestao de conveniados
│   ├── relatorios/     # Geracao de relatorios
│   ├── importacao/     # Import/Export de arquivos
│   ├── auditoria/      # Logs e rastreabilidade
│   └── notificacoes/   # Emails e mensagens
├── shared/             # Codigo compartilhado
│   ├── database/
│   ├── middleware/
│   ├── utils/
│   └── types/
└── infrastructure/     # Configuracoes de infra
```

---

### 3.2 ESTRATEGIA DE REPOSITORIOS

#### Alternativas Avaliadas

| Opcao | Descricao | Pros | Contras |
|-------|-----------|------|---------|
| **A) Monorepo** | Frontend + Backend + Shared no mesmo repo | Refatoracao atomica, compartilhamento de tipos | Repo grande, CI complexo |
| **B) Polyrepo** | Repos separados para cada projeto | Independencia, CI simples | Sincronizacao de versoes, duplicacao |
| **C) Monorepo com Turborepo** | Monorepo com tooling avancado | Cache, builds incrementais | Curva de aprendizado |

#### **DECISAO: A) Monorepo (com pnpm workspaces)**

**Justificativa:**
- Compartilhamento de tipos TypeScript entre frontend e backend (tRPC)
- Refatoracoes atomicas
- Versionamento unificado
- pnpm workspaces e suficiente para escala media

**Estrutura do Monorepo:**

```
fastconsig/
├── apps/
│   ├── web/              # React SPA
│   ├── api/              # Fastify Backend
│   └── jobs/             # Background workers
├── packages/
│   ├── database/         # Prisma schema e client
│   ├── shared/           # Tipos e utils compartilhados
│   ├── ui/               # Componentes React reutilizaveis
│   └── config/           # ESLint, TypeScript configs
├── docker/
│   ├── docker-compose.yml
│   ├── Dockerfile.api
│   └── Dockerfile.web
├── .github/
│   └── workflows/
├── package.json
├── pnpm-workspace.yaml
└── turbo.json            # Opcional para cache
```

---

### 3.3 INFRAESTRUTURA E DEPLOY

#### Stack Legada
- IIS on-premise
- Deploy manual via Visual Studio Publish
- Sem containerizacao

#### Alternativas Avaliadas

| Opcao | Descricao | Pros | Contras |
|-------|-----------|------|---------|
| **A) VMs com Docker Compose** | Containers em VMs gerenciadas | Simples, controle total | Escala manual |
| **B) OKE (Kubernetes)** | Kubernetes gerenciado Oracle | Auto-scaling, HA nativo | Complexidade, custo |
| **C) OCI Container Instances** | Containers serverless | Sem gerenciar VMs | Limitacoes, custo variavel |
| **D) OCI Functions** | Serverless functions | Pay-per-use, escala automatica | Cold start, limites |

#### **DECISAO: A) VMs com Docker Compose**

**Justificativa:**
- Equipe tem experiencia com Docker
- Escala media nao requer Kubernetes
- Menor custo operacional
- Pode migrar para OKE no futuro se necessario

**Arquitetura de Deploy:**

```
┌─────────────────────────────────────────────────────────────┐
│                    Oracle Cloud (OCI)                       │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌──────────────────┐    ┌──────────────────┐               │
│  │   Load Balancer  │    │   WAF/DDoS       │               │
│  │   (OCI LB)       │────│   Protection     │               │
│  └────────┬─────────┘    └──────────────────┘               │
│           │                                                 │
│  ┌────────┴────────┐                                        │
│  │                 │                                        │
│  ▼                 ▼                                        │
│  ┌─────────────────────────────────────────────────────┐    │
│  │              VM: Application Server                 │    │
│  │  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌────────┐  │    │
│  │  │  Nginx  │  │   API   │  │   Web   │  │  Jobs  │  │    │
│  │  │ (proxy) │  │ Fastify │  │  React  │  │ Worker │  │    │
│  │  └─────────┘  └─────────┘  └─────────┘  └────────┘  │    │
│  │                    Docker Compose                   │    │
│  └─────────────────────────────────────────────────────┘    │
│           │                    │                            │
│           ▼                    ▼                            │
│  ┌─────────────────┐  ┌─────────────────┐                   │
│  │   PostgreSQL    │  │     Redis       │                   │
│  │   (OCI DB ou    │  │   (Cache/       │                   │
│  │    container)   │  │    Sessions)    │                   │
│  └─────────────────┘  └─────────────────┘                   │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐    │
│  │              OCI Object Storage                     │    │
│  │         (Uploads, Backups, Arquivos)                │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

**Configuracao de VMs Recomendada:**

| Componente | Shape OCI | vCPUs | RAM | Storage |
|------------|-----------|-------|-----|---------|
| App Server (Prod) | VM.Standard.E4.Flex | 4 | 16GB | 100GB |
| App Server (Staging) | VM.Standard.E4.Flex | 2 | 8GB | 50GB |
| Database (se container) | VM.Standard.E4.Flex | 2 | 8GB | 200GB |

---

### 3.4 LINGUAGENS E RUNTIMES

#### Stack Legada
- C# / .NET Framework 4.0
- JavaScript (jQuery) no frontend
- VBScript em alguns componentes legados

#### Alternativas Avaliadas

| Opcao | Runtime Backend | Runtime Frontend | Pros | Contras |
|-------|-----------------|------------------|------|---------|
| **A) Node.js + React** | Node.js 22 LTS | Vite + React | Stack unificada, equipe JS/TS | Single-threaded |
| **B) Deno + Fresh** | Deno 2.x | Fresh/Preact | Seguro por padrao, moderno | Ecossistema menor |
| **C) Bun + React** | Bun 1.x | Vite + React | Performance extrema | Menos maduro |
| **D) .NET 8 + React** | .NET 8 | Vite + React | Performance, familiar | Equipe nao e forte em .NET |

#### **DECISAO: A) Node.js 22 LTS + React 19**

**Justificativa:**
- Equipe forte em JavaScript/TypeScript
- Ecossistema maduro e vasto
- LTS com suporte ate 2027
- Compartilhamento de tipos via tRPC

**Versoes Especificas:**

| Tecnologia | Versao | Justificativa |
|------------|--------|---------------|
| Node.js | 22.x LTS | Suporte longo, ESM nativo |
| TypeScript | 5.5+ | Inferencia avancada, decorators |
| pnpm | 9.x | Workspaces, performance |

---

### 3.5 FRAMEWORKS E BIBLIOTECAS

#### 3.5.1 Backend

| Alternativa | Framework | Pros | Contras |
|-------------|-----------|------|---------|
| **A) Fastify + tRPC** | Fastify 5.x | Mais rapido, plugins, type safety | Menos popular que Express |
| **B) NestJS** | NestJS 11.x | Estrutura opinativa, decorators | Boilerplate, curva aprendizado |
| **C) Express + tRPC** | Express 5.x | Mais popular, muitos middlewares | Performance menor |
| **D) Hono** | Hono 4.x | Ultra leve, edge-ready | Ecossistema menor |

#### **DECISAO: A) Fastify 5 + tRPC**

**Stack Backend Completa:**

```typescript
// Dependencias principais
{
  "dependencies": {
    // Core
    "fastify": "^5.0.0",
    "@trpc/server": "^11.0.0",
    "@fastify/cors": "^9.0.0",
    "@fastify/helmet": "^11.0.0",
    "@fastify/rate-limit": "^9.0.0",
    "@fastify/jwt": "^8.0.0",
    "@fastify/cookie": "^9.0.0",
    "@fastify/multipart": "^8.0.0",

    // Database
    "@prisma/client": "^5.0.0",
    "redis": "^4.6.0",

    // Validation
    "zod": "^3.23.0",

    // Utils
    "dayjs": "^1.11.0",
    "uuid": "^9.0.0",
    "bcrypt": "^5.1.0",
    "pino": "^9.0.0",
    "pino-pretty": "^11.0.0",

    // Background Jobs
    "bullmq": "^5.0.0",

    // Email
    "nodemailer": "^6.9.0",
    "@react-email/components": "^0.0.20",

    // PDF/Excel
    "pdfkit": "^0.15.0",
    "exceljs": "^4.4.0"
  }
}
```

#### 3.5.2 Frontend

| Alternativa | Framework | Pros | Contras |
|-------------|-----------|------|---------|
| **A) React 19 + Vite** | React 19 | Mais popular, ecossistema | Decisoes de arquitetura |
| **B) Vue 4 + Vite** | Vue 4 | Mais opinativo, facil | Menor ecossistema |
| **C) Svelte 5** | Svelte 5 | Performance, menos boilerplate | Ecossistema menor |
| **D) Next.js 15** | Next.js | Full-stack, SSR | Overhead para SPA |

#### **DECISAO: A) React 19 + Vite + TanStack Router**

**Stack Frontend Completa:**

```typescript
{
  "dependencies": {
    // Core
    "react": "^19.0.0",
    "react-dom": "^19.0.0",

    // Routing
    "@tanstack/react-router": "^1.50.0",

    // Data Fetching (tRPC)
    "@trpc/client": "^11.0.0",
    "@trpc/react-query": "^11.0.0",
    "@tanstack/react-query": "^5.50.0",

    // State Management
    "zustand": "^4.5.0",

    // UI Components
    "@radix-ui/react-*": "latest",
    "class-variance-authority": "^0.7.0",
    "clsx": "^2.1.0",
    "tailwind-merge": "^2.4.0",
    "lucide-react": "^0.400.0",

    // Forms
    "react-hook-form": "^7.52.0",
    "@hookform/resolvers": "^3.9.0",
    "zod": "^3.23.0",

    // Tables
    "@tanstack/react-table": "^8.19.0",

    // Charts
    "recharts": "^2.12.0",

    // Date
    "dayjs": "^1.11.0",
    "react-day-picker": "^8.10.0",

    // Utils
    "sonner": "^1.5.0",       // Toasts
    "cmdk": "^1.0.0",         // Command palette
    "vaul": "^0.9.0"          // Drawer
  },
  "devDependencies": {
    // Build
    "vite": "^5.4.0",
    "@vitejs/plugin-react-swc": "^3.7.0",

    // Styling
    "tailwindcss": "^3.4.0",
    "autoprefixer": "^10.4.0",
    "postcss": "^8.4.0"
  }
}
```

---

### 3.6 PADROES DE CODIGO E ORGANIZACAO

#### Alternativas de Arquitetura Backend

| Opcao | Padrao | Pros | Contras |
|-------|--------|------|---------|
| **A) Feature-based** | Codigo por feature/modulo | Coesao, facil navegacao | Pode ter duplicacao |
| **B) Layer-based** | Codigo por camada (controllers, services) | Padrao conhecido | Features espalhadas |
| **C) Clean Architecture** | Camadas com inversao de dependencia | Testabilidade, desacoplamento | Mais boilerplate |
| **D) Vertical Slices** | Cada feature e independente | Autonomia total | Pode ter inconsistencia |

#### **DECISAO: A) Feature-based com camadas internas**

**Estrutura de um Modulo:**

```
src/modules/averbacoes/
├── averbacoes.router.ts      # Rotas tRPC
├── averbacoes.service.ts     # Logica de negocio
├── averbacoes.repository.ts  # Acesso a dados
├── averbacoes.schema.ts      # Validacoes Zod
├── averbacoes.types.ts       # Tipos TypeScript
├── averbacoes.utils.ts       # Funcoes auxiliares
├── averbacoes.constants.ts   # Constantes do modulo
└── __tests__/
    ├── averbacoes.service.test.ts
    └── averbacoes.router.test.ts
```

**Padroes de Codigo:**

| Aspecto | Padrao |
|---------|--------|
| **Naming** | camelCase para variaveis/funcoes, PascalCase para tipos/classes |
| **Exports** | Named exports (evitar default) |
| **Imports** | Path aliases (@/modules, @/shared) |
| **Error Handling** | Custom errors com codigos, try/catch em services |
| **Logging** | Pino com contexto estruturado |
| **Validation** | Zod schemas para input/output |
| **Documentation** | TSDoc para funcoes publicas |

**Configuracoes de Qualidade:**

```json
// .eslintrc.json
{
  "extends": [
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:@typescript-eslint/strict-type-checked",
    "prettier"
  ],
  "rules": {
    "@typescript-eslint/no-unused-vars": "error",
    "@typescript-eslint/explicit-function-return-type": "warn",
    "@typescript-eslint/no-explicit-any": "error",
    "no-console": "warn"
  }
}
```

```json
// .prettierrc
{
  "semi": false,
  "singleQuote": true,
  "tabWidth": 2,
  "trailingComma": "es5",
  "printWidth": 100
}
```

---

### 3.7 BANCO DE DADOS E ORMs

#### Stack Legada
- SQL Server 2008/2012
- Entity Framework 4.3.1 (Database First)
- Multi-database por tenant
- Conexao via string de sessao

#### Alternativas Avaliadas - Banco

| Opcao | Banco | Pros | Contras |
|-------|-------|------|---------|
| **A) PostgreSQL 16** | PostgreSQL | Open source, JSONB, RLS, particionamento | Migracao de dados |
| **B) SQL Server** | SQL Server | Ja em uso, familiar | Custo licenca, vendor lock |
| **C) MySQL 8** | MySQL | Simples, popular | Menos features enterprise |
| **D) CockroachDB** | CockroachDB | Distribuido, PostgreSQL-compativel | Complexidade, custo |

#### **DECISAO: A) PostgreSQL 16**

**Justificativa:**
- Open source (prioridade do stakeholder)
- Row Level Security nativo para multi-tenancy
- JSONB para dados semi-estruturados
- Excelente performance
- Suporte nativo no OCI (Autonomous Database ou container)

#### Alternativas Avaliadas - ORM

| Opcao | ORM | Pros | Contras |
|-------|-----|------|---------|
| **A) Prisma** | Prisma | Type safety, migrations, DX | Overhead de geracao |
| **B) Drizzle** | Drizzle | Leve, SQL-like, type safe | Menos features |
| **C) TypeORM** | TypeORM | Decorators, relations | Menos type safe |
| **D) Kysely** | Kysely | Query builder type safe | Sem migrations |

#### **DECISAO: A) Prisma ORM**

**Configuracao Multi-Tenancy:**

```prisma
// schema.prisma
generator client {
  provider = "prisma-client-js"
}

datasource db {
  provider = "postgresql"
  url      = env("DATABASE_URL")
}

model Tenant {
  id        Int      @id @default(autoincrement())
  nome      String
  cnpj      String   @unique
  ativo     Boolean  @default(true)
  createdAt DateTime @default(now())

  funcionarios  Funcionario[]
  averbacoes    Averbacao[]
  usuarios      Usuario[]
  // ... outras relacoes
}

model Funcionario {
  id        Int      @id @default(autoincrement())
  tenantId  Int
  tenant    Tenant   @relation(fields: [tenantId], references: [id])

  cpf         String
  nome        String
  matricula   String
  // ... outros campos

  @@unique([tenantId, cpf])
  @@unique([tenantId, matricula])
  @@index([tenantId])
}
```

**Middleware de Tenant:**

```typescript
// shared/database/tenant-middleware.ts
import { Prisma } from '@prisma/client'

export function tenantMiddleware(tenantId: number): Prisma.Middleware {
  return async (params, next) => {
    // Inject tenantId em queries
    if (params.action === 'findMany' || params.action === 'findFirst') {
      params.args.where = {
        ...params.args.where,
        tenantId,
      }
    }

    if (params.action === 'create') {
      params.args.data.tenantId = tenantId
    }

    return next(params)
  }
}
```

---

### 3.8 CACHE E SESSOES

#### Alternativas Avaliadas

| Opcao | Solucao | Pros | Contras |
|-------|---------|------|---------|
| **A) Redis** | Redis 7 | Padrao industria, pub/sub, persistencia | Servidor adicional |
| **B) Memcached** | Memcached | Simples, rapido | Sem persistencia |
| **C) In-Memory** | Node.js Map | Sem dependencia | Nao escala, perde em restart |
| **D) KeyDB** | KeyDB | Fork Redis, multi-threaded | Menos suporte |

#### **DECISAO: A) Redis 7**

**Casos de Uso:**

| Uso | Configuracao |
|-----|--------------|
| **Sessoes** | Prefix `session:`, TTL 24h |
| **Cache de Margem** | Prefix `margem:`, TTL 5min |
| **Rate Limiting** | Sliding window counters |
| **Blacklist JWT** | Tokens revogados |
| **Filas** | BullMQ para background jobs |

---

### 3.9 AUTENTICACAO E AUTORIZACAO

#### Stack Legada
- ASP.NET Forms Authentication
- Cookies de sessao
- Roles em banco de dados

#### Alternativas Avaliadas

| Opcao | Solucao | Pros | Contras |
|-------|---------|------|---------|
| **A) JWT + Refresh Token** | Tokens proprios | Controle total, stateless | Implementar do zero |
| **B) Auth0** | SaaS | Pronto, seguro, SSO facil | Custo, vendor lock |
| **C) Keycloak** | Self-hosted | Open source, completo | Complexidade |
| **D) Lucia Auth** | Library | Leve, flexivel | Menos features |

#### **DECISAO: A) JWT + Refresh Token (implementacao propria)**

**Justificativa:**
- Open source (prioridade)
- Controle total sobre fluxos
- Sem custo adicional
- Requisitos bem definidos

**Configuracao:**

```typescript
// auth/auth.config.ts
export const authConfig = {
  accessToken: {
    secret: process.env.JWT_ACCESS_SECRET!,
    expiresIn: '15m',
  },
  refreshToken: {
    secret: process.env.JWT_REFRESH_SECRET!,
    expiresIn: '7d',
  },
  password: {
    saltRounds: 12,
    minLength: 8,
    requireNumbers: true,
    requireLetters: true,
    historyCount: 5,
  },
  lockout: {
    maxAttempts: 5,
    durationMinutes: 30,
  },
}
```

---

### 3.10 INTEGRACOES EXTERNAS

#### Alternativas - Email

| Opcao | Servico | Pros | Contras |
|-------|---------|------|---------|
| **A) SMTP Proprio** | Nodemailer + SMTP | Controle total | Deliverability |
| **B) AWS SES** | Amazon SES | Barato, confiavel | Vendor AWS |
| **C) SendGrid** | Twilio SendGrid | API simples, analytics | Custo |
| **D) Resend** | Resend | Moderno, React Email | Novo no mercado |

#### **DECISAO: A) SMTP Configuravel + React Email**

**Justificativa:**
- Cliente pode usar SMTP proprio
- Templates com React Email (type safe)
- Fallback para servicos externos

#### Webhooks

```typescript
// integrations/webhook.service.ts
interface WebhookPayload {
  event: string
  timestamp: string
  data: Record<string, unknown>
  signature: string
}

// Eventos suportados
const WEBHOOK_EVENTS = [
  'averbacao.criada',
  'averbacao.aprovada',
  'averbacao.rejeitada',
  'averbacao.descontada',
  'funcionario.criado',
  'funcionario.atualizado',
  'conciliacao.concluida',
] as const
```

---

### 3.11 TESTES

#### **DECISAO: Estrategia de Testes em Camadas**

| Tipo | Framework | Cobertura Alvo | Foco |
|------|-----------|----------------|------|
| **Unitarios** | Vitest | 80% | Services, utils, validacoes |
| **Integracao** | Vitest + Supertest | Rotas principais | Routers, database |
| **E2E** | Playwright | Fluxos criticos | Login, averbacao, aprovacao |

**Estrutura de Testes:**

```
__tests__/
├── unit/
│   ├── services/
│   └── utils/
├── integration/
│   ├── routers/
│   └── database/
└── e2e/
    ├── auth.spec.ts
    ├── averbacao.spec.ts
    └── conciliacao.spec.ts
```

**Configuracao Vitest:**

```typescript
// vitest.config.ts
import { defineConfig } from 'vitest/config'

export default defineConfig({
  test: {
    globals: true,
    environment: 'node',
    coverage: {
      provider: 'v8',
      reporter: ['text', 'html', 'lcov'],
      exclude: ['**/*.test.ts', '**/types.ts'],
      thresholds: {
        statements: 70,
        branches: 70,
        functions: 70,
        lines: 70,
      },
    },
  },
})
```

---

### 3.12 CI/CD

#### **DECISAO: GitHub Actions**

**Pipeline de CI:**

```yaml
# .github/workflows/ci.yml
name: CI

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main]

jobs:
  lint:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: pnpm/action-setup@v3
      - uses: actions/setup-node@v4
        with:
          node-version: 22
          cache: 'pnpm'
      - run: pnpm install --frozen-lockfile
      - run: pnpm lint

  typecheck:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: pnpm/action-setup@v3
      - uses: actions/setup-node@v4
        with:
          node-version: 22
          cache: 'pnpm'
      - run: pnpm install --frozen-lockfile
      - run: pnpm typecheck

  test:
    runs-on: ubuntu-latest
    services:
      postgres:
        image: postgres:16
        env:
          POSTGRES_PASSWORD: test
          POSTGRES_DB: fastconsig_test
        ports:
          - 5432:5432
      redis:
        image: redis:7
        ports:
          - 6379:6379
    steps:
      - uses: actions/checkout@v4
      - uses: pnpm/action-setup@v3
      - uses: actions/setup-node@v4
        with:
          node-version: 22
          cache: 'pnpm'
      - run: pnpm install --frozen-lockfile
      - run: pnpm test:coverage
      - uses: codecov/codecov-action@v4

  build:
    runs-on: ubuntu-latest
    needs: [lint, typecheck, test]
    steps:
      - uses: actions/checkout@v4
      - uses: pnpm/action-setup@v3
      - uses: actions/setup-node@v4
        with:
          node-version: 22
          cache: 'pnpm'
      - run: pnpm install --frozen-lockfile
      - run: pnpm build
```

**Pipeline de Deploy:**

```yaml
# .github/workflows/deploy.yml
name: Deploy

on:
  push:
    branches: [main]

jobs:
  deploy-staging:
    runs-on: ubuntu-latest
    environment: staging
    steps:
      - uses: actions/checkout@v4

      - name: Build Docker images
        run: |
          docker build -t fastconsig-api:${{ github.sha }} -f docker/Dockerfile.api .
          docker build -t fastconsig-web:${{ github.sha }} -f docker/Dockerfile.web .

      - name: Push to OCI Registry
        run: |
          docker push ${{ secrets.OCI_REGISTRY }}/fastconsig-api:${{ github.sha }}
          docker push ${{ secrets.OCI_REGISTRY }}/fastconsig-web:${{ github.sha }}

      - name: Deploy to Staging VM
        uses: appleboy/ssh-action@v1
        with:
          host: ${{ secrets.STAGING_HOST }}
          username: ${{ secrets.STAGING_USER }}
          key: ${{ secrets.STAGING_SSH_KEY }}
          script: |
            cd /app
            docker compose pull
            docker compose up -d --remove-orphans

  deploy-production:
    runs-on: ubuntu-latest
    needs: deploy-staging
    environment: production
    steps:
      # Similar ao staging com approval manual
```

---

## 4. Stack Consolidada

### 4.1 Visao Geral

```
┌─────────────────────────────────────────────────────────────────────┐
│                         FASTCONSIG - STACK 2026                      │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │                        FRONTEND                              │   │
│  │  React 19 + TypeScript + Vite + TanStack Router             │   │
│  │  shadcn/ui + Tailwind CSS + React Hook Form + Zod           │   │
│  └─────────────────────────────────────────────────────────────┘   │
│                              │                                       │
│                              │ tRPC (Type-safe API)                 │
│                              ▼                                       │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │                        BACKEND                               │   │
│  │  Node.js 22 + TypeScript + Fastify 5 + tRPC                 │   │
│  │  Prisma ORM + Zod + BullMQ + Pino                           │   │
│  └─────────────────────────────────────────────────────────────┘   │
│                    │                    │                            │
│                    ▼                    ▼                            │
│  ┌──────────────────────┐  ┌──────────────────────┐                │
│  │     PostgreSQL 16    │  │      Redis 7         │                │
│  │  (Multi-tenant RLS)  │  │  (Cache/Sessions)    │                │
│  └──────────────────────┘  └──────────────────────┘                │
│                                                                      │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │                    INFRAESTRUTURA                            │   │
│  │  Oracle Cloud (OCI) + Docker Compose + Nginx                │   │
│  │  GitHub Actions + Object Storage                             │   │
│  └─────────────────────────────────────────────────────────────┘   │
│                                                                      │
└─────────────────────────────────────────────────────────────────────┘
```

### 4.2 Tabela Resumo

| Categoria | Tecnologia | Versao |
|-----------|------------|--------|
| **Runtime** | Node.js | 22 LTS |
| **Linguagem** | TypeScript | 5.5+ |
| **Frontend Framework** | React | 19 |
| **Build Tool** | Vite | 5.4+ |
| **Routing** | TanStack Router | 1.50+ |
| **State Management** | Zustand | 4.5+ |
| **Data Fetching** | TanStack Query + tRPC | 5.50+ / 11+ |
| **UI Components** | shadcn/ui + Radix | Latest |
| **CSS Framework** | Tailwind CSS | 3.4+ |
| **Forms** | React Hook Form + Zod | 7.52+ / 3.23+ |
| **Tables** | TanStack Table | 8.19+ |
| **Charts** | Recharts | 2.12+ |
| **Backend Framework** | Fastify | 5.0+ |
| **API Layer** | tRPC | 11+ |
| **ORM** | Prisma | 5.0+ |
| **Validation** | Zod | 3.23+ |
| **Background Jobs** | BullMQ | 5.0+ |
| **Logging** | Pino | 9.0+ |
| **Database** | PostgreSQL | 16 |
| **Cache** | Redis | 7+ |
| **Containerization** | Docker + Compose | Latest |
| **CI/CD** | GitHub Actions | - |
| **Cloud** | Oracle Cloud (OCI) | - |
| **Package Manager** | pnpm | 9+ |
| **Testing (Unit)** | Vitest | 2.0+ |
| **Testing (E2E)** | Playwright | 1.45+ |
| **Linting** | ESLint | 9+ |
| **Formatting** | Prettier | 3.3+ |

---

## 5. ADRs (Architecture Decision Records)

### ADR-001: Node.js ao inves de .NET

**Status:** Aceito

**Contexto:** Escolher runtime principal para o backend.

**Decisao:** Usar Node.js 22 LTS com TypeScript.

**Justificativa:**
- Equipe forte em JavaScript/TypeScript
- Compartilhamento de tipos com frontend via tRPC
- Ecossistema open source alinhado com prioridades
- Performance adequada para escala media

**Consequencias:**
- (+) Stack unificada, menor curva de aprendizado
- (+) Contratacao mais facil
- (-) Concorrencia single-threaded (mitigado com workers)

---

### ADR-002: Monolito Modular ao inves de Microsservicos

**Status:** Aceito

**Contexto:** Definir arquitetura geral do sistema.

**Decisao:** Implementar como monolito modular com separacao clara por features.

**Justificativa:**
- Escala media (100-1000 usuarios) nao justifica microsservicos
- Menor complexidade operacional
- Deploy e debug simplificados
- Pode evoluir para microsservicos se necessario

**Consequencias:**
- (+) Desenvolvimento mais rapido
- (+) Menos infraestrutura
- (-) Escala apenas vertical inicialmente

---

### ADR-003: PostgreSQL com tenant_id ao inves de Multi-Database

**Status:** Aceito

**Contexto:** Migrar de multi-database para arquitetura multi-tenant.

**Decisao:** Banco unico com coluna tenant_id e Row Level Security.

**Justificativa:**
- Migrations unificadas
- Queries cross-tenant para analytics
- Menor custo operacional
- RLS garante isolamento

**Consequencias:**
- (+) Gerenciamento simplificado
- (+) Backup unificado
- (-) Risco se RLS mal configurado (mitigado com testes)

---

### ADR-004: tRPC ao inves de REST puro

**Status:** Aceito

**Contexto:** Escolher padrao de comunicacao frontend-backend.

**Decisao:** Usar tRPC para APIs internas, REST para webhooks externos.

**Justificativa:**
- Type safety end-to-end
- Menos boilerplate
- Validacao automatica
- Excelente DX

**Consequencias:**
- (+) Zero tipos duplicados
- (+) Autocompletar no frontend
- (-) Menos familiar para novos devs

---

### ADR-005: VMs com Docker ao inves de Kubernetes

**Status:** Aceito

**Contexto:** Escolher modelo de deploy no Oracle Cloud.

**Decisao:** Usar VMs com Docker Compose.

**Justificativa:**
- Escala media nao requer K8s
- Menor custo operacional
- Equipe pode focar em features
- Migrar para OKE e possivel no futuro

**Consequencias:**
- (+) Simplicidade operacional
- (+) Menor custo
- (-) Escala manual (aceitavel para 100-1000 usuarios)

---

### ADR-006: React 19 + Vite para Frontend

**Status:** Aceito

**Contexto:** Escolher framework e build tool para o frontend SPA.

**Decisao:** React 19 com Vite como build tool e TanStack Router para roteamento.

**Justificativa:**
- Maior ecossistema e comunidade
- Equipe forte em TypeScript/React
- Vite oferece HMR rapido e build otimizado
- TanStack Router oferece type-safety completo

**Alternativas Consideradas:**
- Vue 4: Menor ecossistema, equipe menos familiarizada
- Svelte 5: Ecossistema ainda em crescimento
- Next.js: Overhead desnecessario para SPA pura

**Consequencias:**
- (+) Maior disponibilidade de bibliotecas e componentes
- (+) Facil contratacao de desenvolvedores
- (-) Mais decisoes arquiteturais necessarias (resolvido com padroes definidos)

---

### ADR-007: shadcn/ui + Tailwind CSS para UI

**Status:** Aceito

**Contexto:** Escolher sistema de componentes e estilizacao.

**Decisao:** shadcn/ui (baseado em Radix) com Tailwind CSS.

**Justificativa:**
- Componentes copiaveis e customizaveis (nao e dependencia)
- Radix primitives garantem acessibilidade (WCAG 2.1 AA)
- Tailwind CSS oferece produtividade e consistencia
- Open source sem custos de licenca

**Alternativas Consideradas:**
- MUI: Menos customizavel, Material Design opinativo
- DaisyUI: Menos flexivel para designs customizados
- Radix puro: Mais trabalho de estilizacao

**Consequencias:**
- (+) Total controle sobre design
- (+) Bundle otimizado (apenas componentes usados)
- (+) Acessibilidade built-in
- (-) Requer setup inicial dos componentes

---

### ADR-008: Zod para Validacao

**Status:** Aceito

**Contexto:** Escolher biblioteca de validacao de schemas.

**Decisao:** Zod para validacao tanto no frontend quanto backend.

**Justificativa:**
- Inferencia de tipos TypeScript excelente
- API funcional e composavel
- Integracao nativa com tRPC
- Integracao com React Hook Form via @hookform/resolvers

**Alternativas Consideradas:**
- class-validator: Baseado em decorators, menos type-safe
- Joi: Menos integrado com TypeScript
- Yup: API similar mas menos type-safe

**Consequencias:**
- (+) Schemas compartilhados entre frontend e backend
- (+) Tipos inferidos automaticamente
- (+) Mensagens de erro customizaveis

---

### ADR-009: Prisma como ORM

**Status:** Aceito

**Contexto:** Escolher ORM/Query Builder para PostgreSQL.

**Decisao:** Prisma ORM com schema declarativo.

**Justificativa:**
- Geracao de client 100% type-safe
- Sistema de migrations robusto
- Excelente developer experience
- Suporte nativo a PostgreSQL features

**Alternativas Consideradas:**
- Drizzle: Mais leve mas menos features
- TypeORM: Decorators, menos type-safe
- Kysely: Sem sistema de migrations

**Consequencias:**
- (+) Zero queries com tipos errados em runtime
- (+) Migrations versionadas e reversiveis
- (-) Overhead de geracao do client (mitigado em CI)

---

### ADR-010: Vitest + Playwright para Testes

**Status:** Aceito

**Contexto:** Escolher frameworks de testes unitarios e E2E.

**Decisao:** Vitest para testes unitarios/integracao, Playwright para E2E.

**Justificativa:**
- Vitest: Compativel com Vite, API Jest-like, ESM nativo
- Playwright: Multi-browser, auto-wait, trace viewer, mantido pela Microsoft

**Alternativas Consideradas:**
- Jest: Mais lento, configuracao ESM complexa
- Cypress: Mais lento, menos browsers suportados

**Consequencias:**
- (+) Testes rapidos em desenvolvimento
- (+) Debugging facilitado com trace viewer
- (+) CI/CD otimizado com paralelizacao

---

## 6. Proximos Passos

1. **Setup do Monorepo:** Criar estrutura inicial com pnpm workspaces
2. **Schema Prisma:** Definir modelos baseado no diagrama ER
3. **Scaffolding:** Gerar estrutura de modulos do backend
4. **CI/CD:** Configurar GitHub Actions
5. **Docker:** Criar Dockerfiles e docker-compose
6. **Ambiente OCI:** Provisionar VMs e recursos

---

## 7. Historico de Revisoes

| Versao | Data | Autor | Descricao |
|--------|------|-------|-----------|
| 1.0 | Janeiro 2026 | Tech Team | Versao inicial com decisoes consolidadas |

---

*Este documento consolida as decisoes tecnicas do projeto FastConsig e deve ser mantido atualizado conforme evolucao do projeto.*
