# FastConsig

Sistema de Gestao de Consignados - Nova Stack

## Stack Tecnologica

| Categoria | Tecnologia |
|-----------|------------|
| Runtime | Node.js 22 LTS |
| Linguagem | TypeScript 5.5+ |
| Frontend | React 19 + Vite + TanStack Router |
| Backend | Fastify 5 + tRPC 11 |
| Database | PostgreSQL 16 |
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

## Deploy

O sistema e projetado para deploy em Oracle Cloud (OCI) usando Docker Compose em VMs.

```bash
# Build das imagens
docker build -t fastconsig-api -f docker/Dockerfile.api .
docker build -t fastconsig-web -f docker/Dockerfile.web .

# Deploy com Docker Compose
docker compose -f docker-compose.prod.yml up -d
```

## Licenca

Proprietary - Todos os direitos reservados
