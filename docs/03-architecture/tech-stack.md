---
title: "Stack Tecnológica"
version: "1.1"
date: "2026-01-15"
status: "Approved"
language: "pt-BR"
category: "architecture"
related:
  - decisoes-tecnicas.md
  - arquitetura-tecnica.md
---

# Stack Tecnológica - FastConsig

**Extraído de:** [decisoes-tecnicas.md](./decisoes-tecnicas.md) v1.1 (Seção 4.2)

Este documento apresenta uma referência rápida da stack tecnológica completa do FastConsig.

## Visão Geral

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

## Tabela Completa

| Categoria | Tecnologia | Versão | Justificativa |
|-----------|------------|--------|---------------|
| **Runtime** | Node.js | 22 LTS | Performance, ecossistema, expertise da equipe |
| **Linguagem** | TypeScript | 5.5+ | Type safety, produtividade, integração com ferramentas |
| **Frontend Framework** | React | 19 | Ecossistema maduro, comunidade, expertise |
| **Build Tool** | Vite | 5.4+ | HMR rápido, build otimizado |
| **Routing** | TanStack Router | 1.50+ | Type-safe routing |
| **State Management** | Zustand | 4.5+ | Simplicidade, performance |
| **Data Fetching** | TanStack Query + tRPC | 5.50+ / 11+ | Caching inteligente, type safety E2E |
| **UI Components** | shadcn/ui + Radix | Latest | Acessibilidade, customização, open source |
| **CSS Framework** | Tailwind CSS | 3.4+ | Utility-first, produtividade |
| **Forms** | React Hook Form + Zod | 7.52+ / 3.23+ | Performance, validação type-safe |
| **Tables** | TanStack Table | 8.19+ | Flexibilidade, features avançadas |
| **Charts** | Recharts | 2.12+ | Integração React, customização |
| **Backend Framework** | Fastify | 5.0+ | Performance superior, plugin ecosystem |
| **API Layer** | tRPC | 11+ | Type safety end-to-end, DX excepcional |
| **ORM** | Prisma | 5.0+ | Type generation, migrations declarativas |
| **Validation** | Zod | 3.23+ | Runtime + compile-time validation |
| **Background Jobs** | BullMQ | 5.0+ | Confiabilidade, retry automático |
| **Logging** | Pino | 9.0+ | Performance (JSON logs) |
| **Database** | PostgreSQL | 16 | RLS, JSONB, confiabilidade, open source |
| **Cache** | Redis | 7+ | Sessions, rate limiting, BullMQ |
| **Containerization** | Docker + Compose | Latest | Portabilidade, consistência |
| **CI/CD** | GitHub Actions | - | Integração com repo, workflows customizáveis |
| **Cloud** | Oracle Cloud (OCI) | - | Custo-benefício, VMs flexíveis |
| **Package Manager** | pnpm | 9+ | Eficiência de disco, workspaces |
| **Testing (Unit)** | Vitest | 2.0+ | Performance, compatibilidade Vite |
| **Testing (E2E)** | Playwright | 1.45+ | Multi-browser, paralelo, confiável |
| **Linting** | ESLint | 9+ | Qualidade de código, padrões |
| **Formatting** | Prettier | 3.3+ | Consistência de formatação |

## Stack por Camada

### Frontend (apps/web)
```
React 19
├── Vite (build)
├── TypeScript 5.5+
├── TanStack Router (routing)
├── Zustand (state)
├── TanStack Query + tRPC (data)
├── shadcn/ui (components)
│   └── Radix UI (primitives)
├── Tailwind CSS (styling)
├── React Hook Form + Zod (forms)
├── TanStack Table (tables)
└── Recharts (charts)
```

### Backend (apps/api)
```
Node.js 22 LTS
├── TypeScript 5.5+
├── Fastify 5 (server)
├── tRPC 11 (API layer)
├── Prisma 5 (ORM)
├── Zod (validation)
├── BullMQ (jobs)
├── Pino (logging)
└── JWT (auth - implementação própria)
```

### Background Jobs (apps/jobs)
```
Node.js 22 LTS
├── TypeScript 5.5+
├── BullMQ (queue processor)
├── Prisma 5 (database)
└── Pino (logging)
```

### Database (packages/database)
```
PostgreSQL 16
├── Prisma schema
├── Row-Level Security (RLS)
├── Migrations automáticas
└── Type generation
```

### Shared (packages/shared)
```
TypeScript 5.5+
├── Zod schemas
├── Types compartilhados
├── Constants
└── Utilities
```

## Infraestrutura

### Desenvolvimento Local
- Docker Compose (PostgreSQL + Redis)
- Node.js 22 LTS
- pnpm workspaces

### Produção (Oracle Cloud)
- VMs (Compute instances)
- Docker + Docker Compose
- Nginx (reverse proxy)
- PostgreSQL 16 (managed ou self-hosted)
- Redis 7 (managed ou self-hosted)
- Object Storage (arquivos)

### CI/CD (GitHub Actions)
- Build: TypeScript compilation, Vite build
- Test: Vitest (unit), Playwright (E2E)
- Lint: ESLint, Prettier
- Deploy: Docker images → OCI registry → VMs

## Comparativo com Stack Legada

| Componente | Legado | Novo | Ganho Principal |
|------------|--------|------|-----------------|
| Runtime | .NET Framework 4.0 | Node.js 22 LTS | Ecossistema moderno |
| Linguagem | C# | TypeScript 5.5+ | Compartilhamento frontend/backend |
| Frontend | WebForms | React 19 SPA | UX moderna, reatividade |
| Backend | WCF | Fastify + tRPC | Type safety E2E |
| Database | SQL Server | PostgreSQL 16 | Open source, RLS |
| ORM | EF 4.3 (DB-first) | Prisma (Schema-first) | Migrations + types |
| Auth | Forms | JWT | Stateless, escalável |
| UI | DevExpress 11.1 | shadcn/ui | Open source, moderno |
| Deploy | IIS | Docker | Portabilidade |

## Dependências Principais

### Produção
```json
{
  "dependencies": {
    "@fastify/cors": "^9.0.0",
    "@prisma/client": "^5.0.0",
    "@trpc/server": "^11.0.0",
    "bullmq": "^5.0.0",
    "fastify": "^5.0.0",
    "pino": "^9.0.0",
    "react": "^19.0.0",
    "react-dom": "^19.0.0",
    "zod": "^3.23.0"
  }
}
```

### Desenvolvimento
```json
{
  "devDependencies": {
    "@playwright/test": "^1.45.0",
    "@types/node": "^20.0.0",
    "@types/react": "^19.0.0",
    "eslint": "^9.0.0",
    "prettier": "^3.3.0",
    "prisma": "^5.0.0",
    "typescript": "^5.5.0",
    "vite": "^5.4.0",
    "vitest": "^2.0.0"
  }
}
```

## Notas de Versão

- **Node.js 22 LTS:** Suporte até Abril 2027
- **React 19:** Compiler embutido, concurrent features
- **PostgreSQL 16:** Melhorias em RLS e JSON
- **TypeScript 5.5+:** Decorators, performance

## Referências

- **Decisões Completas:** [decisoes-tecnicas.md](./decisoes-tecnicas.md)
- **Arquitetura:** [arquitetura-tecnica.md](./arquitetura-tecnica.md)
- **Diagrama ER:** [diagrama-er.md](./diagrama-er.md)

---

*Última atualização: Janeiro 2026 (extraído de decisoes-tecnicas.md v1.1)*
