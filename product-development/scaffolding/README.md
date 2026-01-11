# FastConsig

Sistema de Gestao de Consignados - Nova Stack

## Stack Tecnologica

| Categoria | Tecnologia |
|-----------|------------|
| Runtime | Node.js 22 LTS |
| Linguagem | TypeScript 5.5+ |
| Frontend | React 19 + Vite + TanStack Router |
| Backend | Fastify 5 + tRPC 11 |
| Database | PostgreSQL 18 |
| ORM | Prisma |
| Cache | Redis 7 |
| UI | shadcn/ui + Tailwind CSS |
| Testes | Vitest + Playwright |

## Estrutura do Projeto

```
fastconsig/
├── apps/
│   ├── api/              # Backend Fastify + tRPC
│   ├── web/              # Frontend React SPA
│   └── jobs/             # Background workers (BullMQ)
├── packages/
│   ├── config/           # ESLint, TypeScript configs
│   ├── database/         # Prisma schema e client
│   ├── shared/           # Tipos e utils compartilhados
│   └── ui/               # Componentes React reutilizaveis
└── docker/               # Dockerfiles e configs
```

## Requisitos

- Node.js >= 22.0.0
- pnpm >= 9.0.0
- Docker e Docker Compose
- PostgreSQL 16
- Redis 7

## Instalacao

```bash
# Instalar dependencias
pnpm install

# Configurar variaveis de ambiente
cp .env.example .env

# Gerar client Prisma
pnpm db:generate

# Executar migrations
pnpm db:migrate

# Seed do banco (opcional)
pnpm db:seed
```

## Desenvolvimento

```bash
# Iniciar todos os servicos com Docker
docker compose up -d

# Iniciar em modo desenvolvimento
pnpm dev

# Abrir Prisma Studio
pnpm db:studio
```

## Scripts Disponiveis

| Script | Descricao |
|--------|-----------|
| `pnpm dev` | Inicia todos os apps em modo dev |
| `pnpm build` | Build de producao |
| `pnpm lint` | Executa ESLint |
| `pnpm typecheck` | Verifica tipos TypeScript |
| `pnpm test` | Executa testes unitarios |
| `pnpm test:e2e` | Executa testes E2E |
| `pnpm db:generate` | Gera client Prisma |
| `pnpm db:migrate` | Executa migrations |
| `pnpm db:studio` | Abre Prisma Studio |

## Variaveis de Ambiente

```env
# Database
DATABASE_URL=postgresql://postgres:postgres@localhost:5432/fastconsig

# Redis
REDIS_URL=redis://localhost:6379

# Auth
JWT_ACCESS_SECRET=your-access-secret-min-32-chars
JWT_REFRESH_SECRET=your-refresh-secret-min-32-chars

# Server
PORT=3001
CORS_ORIGIN=http://localhost:3000

# Email (opcional)
SMTP_HOST=localhost
SMTP_PORT=1025
```

## Arquitetura

O sistema utiliza arquitetura de **monolito modular** com separacao clara por features:

- **apps/api**: Backend com Fastify e tRPC
- **apps/web**: SPA React com TanStack Router
- **apps/jobs**: Workers para processamento assincrono

### Modulos do Backend

- `auth` - Autenticacao e autorizacao
- `funcionarios` - Gestao de servidores
- `averbacoes` - Ciclo de vida de emprestimos
- `margem` - Calculo e gestao de margem
- `simulacao` - Simulador de emprestimos
- `consignatarias` - Gestao de conveniados
- `conciliacao` - Processo de conciliacao
- `relatorios` - Geracao de relatorios
- `importacao` - Import/Export de arquivos
- `auditoria` - Logs e rastreabilidade

## CI/CD

O projeto utiliza GitHub Actions para integração e deploy contínuo.

### Workflows

- **CI** (`.github/workflows/ci.yml`): Validação automática (lint, typecheck, test, build, security)
- **Build & Deploy** (`.github/workflows/build-deploy.yml`): Build de imagens e deploy no Docker Swarm

### Ambientes

| Ambiente | Branch | URLs |
|----------|--------|------|
| Production | `main` | `app.fastconsig.com.br` / `api.fastconsig.com.br` |
| Development | `development` | `dev-app.fastconsig.com.br` / `dev-api.fastconsig.com.br` |

### Documentação

- [Setup Completo de CI/CD](./docs/CICD-SETUP.md)
- [Quick Start CI/CD](./docs/QUICK-START-CICD.md)

## Deploy

O sistema e deployado automaticamente no Docker Swarm via GitHub Actions.

### Deploy Manual

```bash
# Definir variáveis
export IMAGE_TAG=$(git rev-parse --short HEAD)
export STACK_NAME="fastconsig-production"
export OCI_REGISTRY="gru.ocir.io/grnvzpym0ltz"
# ... outras variáveis (ver docs/CICD-SETUP.md)

# Login no registry
docker login $OCI_REGISTRY

# Deploy da stack
docker stack deploy -c ops/fastconsig.yml $STACK_NAME --with-registry-auth
```

## Testes

Cobertura atual: **96%** ✅

```bash
# Rodar testes
pnpm test

# Testes com coverage
pnpm test:coverage

# Testes E2E
pnpm test:e2e
```

## Licenca

Proprietary - Todos os direitos reservados
