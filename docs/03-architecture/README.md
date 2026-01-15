# Arquitetura Técnica

Documentação da arquitetura, decisões técnicas e stack do sistema FastConsig.

## Documentos

### ⭐ [decisoes-tecnicas.md](./decisoes-tecnicas.md) - APROVADO v1.1

**Status:** ✅ Aprovado por stakeholder
**Conteúdo:** Stack completo, ADRs, justificativas técnicas

Documento **crítico** contendo todas as decisões técnicas aprovadas:
- Node.js 22 LTS vs .NET
- Monolito Modular vs Microsserviços
- PostgreSQL com tenant_id vs Multi-Database
- tRPC vs REST
- React 19 + Vite para frontend
- VMs com Docker vs Kubernetes

**Leitura obrigatória** para qualquer desenvolvedor antes de começar.

---

### [arquitetura-tecnica.md](./arquitetura-tecnica.md)

**Conteúdo:** Padrões arquiteturais, Clean Architecture, estrutura de módulos

Detalha a implementação prática:
- C4 Context Diagrams (System, Container, Component)
- Princípios arquiteturais (API-First, Multi-Tenancy, Cloud-Native)
- Stack tecnológica recomendada (backend, frontend, banco)
- Estrutura de módulos do monorepo
- Segurança by design
- Patterns aplicados (Repository, Service Layer, CQRS light)

---

### [diagrama-er.md](./diagrama-er.md)

**Conteúdo:** Modelo PostgreSQL com RLS, relacionamentos, índices

Esquema completo do banco de dados:
- Diagrama Mermaid ER
- Core entities: Tenant, Employee, Loan, Margin
- Relacionamentos (Consignante ↔ Consignatária ↔ Funcionários)
- Row-Level Security (RLS) policies
- Índices de performance
- Constraints e validações

---

### [tech-stack.md](./tech-stack.md)

**Conteúdo:** Tabela resumo de tecnologias (referência rápida)

Extraído de `decisoes-tecnicas.md` seção 4.2:
- Tabela completa: Framework → Versão → Justificativa
- Stack por camada (Frontend, Backend, Jobs, Database)
- Comparativo com stack legada
- Dependências principais

---

## Ordem de Leitura

### Para Novos Desenvolvedores

1. **[decisoes-tecnicas.md](./decisoes-tecnicas.md)** (30 min)
   Entenda *por que* escolhemos Node.js, React, PostgreSQL, etc.

2. **[tech-stack.md](./tech-stack.md)** (10 min)
   Referência rápida de todas as tecnologias.

3. **[arquitetura-tecnica.md](./arquitetura-tecnica.md)** (45 min)
   Aprenda os padrões e como organizar o código.

4. **[diagrama-er.md](./diagrama-er.md)** (20 min)
   Entenda o modelo de dados.

### Para Arquitetos

1. **[decisoes-tecnicas.md](./decisoes-tecnicas.md)** - ADRs e trade-offs
2. **[arquitetura-tecnica.md](./arquitetura-tecnica.md)** - Padrões aplicados
3. **[diagrama-er.md](./diagrama-er.md)** - Data modeling
4. **[../archive/](../archive/)** - Comparação com sistema legado

### Para Product Owners

1. **[tech-stack.md](./tech-stack.md)** - Overview das tecnologias
2. **[decisoes-tecnicas.md](./decisoes-tecnicas.md) - Seção 4** - Stack consolidada
3. **[arquitetura-tecnica.md](./arquitetura-tecnica.md) - Seção 1** - Princípios

---

## Conceitos-Chave

### Clean Architecture (Hexagonal)

```
┌─────────────────────────────────────────┐
│            Presentation                 │
│  (React SPA / CLI / Mobile futuro)      │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│          Application Layer              │
│      (Use Cases / tRPC Routers)         │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│            Domain Layer                 │
│   (Entities + Business Rules)           │
│   ► Loan, Employee, Margin              │
│   ► Cálculos, validações                │
└─────────────────┬───────────────────────┘
                  │
┌─────────────────▼───────────────────────┐
│        Infrastructure Layer             │
│  ► Prisma (Database)                    │
│  ► CNAB Adapters (Banking ACL)          │
│  ► BullMQ (Jobs)                        │
│  ► Redis (Cache)                        │
└─────────────────────────────────────────┘
```

**Benefícios:**
- ✅ Domain isolado (testável sem infraestrutura)
- ✅ Troca de DB/framework sem impactar lógica
- ✅ Múltiplas UI (Web, Mobile, CLI) usando mesma API

### Multi-Tenancy com RLS

**Estratégia:** Single Database + tenant_id + Row-Level Security

```sql
-- Todas as tabelas têm tenant_id
CREATE TABLE employees (
  id UUID PRIMARY KEY,
  tenant_id UUID NOT NULL,
  ...
);

-- RLS garante isolamento automático
CREATE POLICY tenant_isolation ON employees
  USING (tenant_id = current_setting('app.current_tenant_id')::uuid);

ALTER TABLE employees ENABLE ROW LEVEL SECURITY;
```

**Queries automáticas:**
```typescript
// Backend seta o tenant_id da sessão
await prisma.$executeRaw`SET app.current_tenant_id = ${tenantId}`;

// Todas as queries filtram automaticamente por tenant
const employees = await prisma.employee.findMany(); // RLS filtra!
```

### Type Safety End-to-End (tRPC)

```typescript
// Backend: apps/api/src/routers/employees.ts
export const employeesRouter = router({
  list: publicProcedure
    .input(z.object({ search: z.string().optional() }))
    .query(async ({ input }) => {
      return await prisma.employee.findMany({ /* ... */ });
    }),
});

// Frontend: apps/web/src/pages/EmployeesPage.tsx
const { data } = trpc.employees.list.useQuery({ search: "João" });
//    ^? { id: string; fullName: string; ... }[] - Tipos inferidos!
```

**Zero duplicação de tipos.**

### Margem Viva (Event-Driven)

**Antes (Legado):** Margem calculada sob demanda
**Depois (Novo):** Margem atualizada em tempo real por eventos

```typescript
// Event emitido quando loan é criado
class LoanCreatedEvent {
  employeeId: string;
  installmentAmount: Decimal;
}

// Handler atualiza margem automaticamente
class UpdateMarginHandler {
  async handle(event: LoanCreatedEvent) {
    const margin = await marginRepo.findByEmployee(event.employeeId);
    margin.decreaseAvailable(event.installmentAmount);
    await marginRepo.save(margin); // Margem sempre atualizada!
  }
}
```

---

## Padrões Aplicados

| Padrão | Onde | Benefício |
|--------|------|-----------|
| **Repository** | Infrastructure layer | Abstração de acesso a dados |
| **Service Layer** | Application layer | Orquestração de use cases |
| **DTO (Data Transfer Object)** | tRPC inputs/outputs | Validação + type safety |
| **Event Sourcing (light)** | Margin updates | Consistência eventual |
| **Anti-Corruption Layer** | CNAB adapters | Isola domínio de infra legada |
| **Factory** | Prisma client creation | Dependency injection |
| **Singleton** | Database connection | Reuso de conexão |

---

## Decisões Arquiteturais (ADRs)

Ver [decisoes-tecnicas.md](./decisoes-tecnicas.md) Seção 5 para lista completa.

**Principais ADRs:**
- **ADR-001:** Node.js ao invés de .NET
- **ADR-002:** Monolito Modular ao invés de Microsserviços
- **ADR-003:** PostgreSQL com tenant_id ao invés de Multi-Database
- **ADR-004:** tRPC ao invés de REST puro
- **ADR-005:** VMs com Docker ao invés de Kubernetes
- **ADR-006:** React 19 + Vite para Frontend

---

## Estrutura do Monorepo

```
fastconsig/
├── apps/
│   ├── web/              # React 19 SPA
│   ├── api/              # Fastify + tRPC Backend
│   └── jobs/             # BullMQ Workers
├── packages/
│   ├── database/         # Prisma schema + client
│   ├── shared/           # Types + utils compartilhados
│   ├── ui/               # shadcn/ui components
│   └── config/           # ESLint, TS configs
├── docker/
├── .github/workflows/
├── pnpm-workspace.yaml
└── turbo.json
```

---

## Relacionados

- **Regras de Negócio:** [../05-business-rules/](../05-business-rules/)
- **Integrações:** [../06-integrations/](../06-integrations/)
- **Sistema Legado:** [../archive/](../archive/)
- **PRD:** [../01-product/prd.md](../01-product/prd.md)

---

## Suporte

**Para dúvidas técnicas:**
1. Consulte este diretório primeiro
2. Revise ADRs em `decisoes-tecnicas.md`
3. Compare com sistema legado em `/docs/archive/`

**Para decisões novas:**
1. Discuta com o time
2. Documente como novo ADR em `decisoes-tecnicas.md`
3. Atualize este README se necessário

---

*Última atualização: Janeiro 2026*
