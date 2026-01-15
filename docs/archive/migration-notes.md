# Notas de Migração - .NET para Node.js

**Documento:** Guia de migração do sistema legado para o novo sistema
**Data:** Janeiro 2026
**Versão:** 1.0

---

## Visão Geral da Migração

Este documento detalha as principais mudanças, decisões e impactos da migração do sistema FastConsig legado (.NET Framework 4.0) para o novo sistema (Node.js 22 + React 19).

**Tipo de Migração:** Reescrita completa (Rewrite)
**Motivo:** Modernização tecnológica, melhor manutenibilidade, escalabilidade

---

## Comparativo Tecnológico

### Stack Completa

| Componente | Legado | Novo | Justificativa |
|------------|--------|------|---------------|
| **Runtime** | .NET Framework 4.0 | Node.js 22 LTS | Ecossistema moderno, expertise da equipe |
| **Linguagem** | C# | TypeScript 5.5+ | Type safety com ecossistema JS |
| **Frontend** | ASP.NET WebForms + ViewState | React 19 SPA | Componentização, estado reativo |
| **Backend Framework** | WCF + ASMX | Fastify + tRPC | Performance, type safety E2E |
| **ORM** | Entity Framework 4.3 (DB-first) | Prisma ORM (Schema-first) | Migrations, type generation |
| **Banco de Dados** | SQL Server | PostgreSQL 16 | Open source, custo, features avançadas |
| **Autenticação** | Forms Auth + Session | JWT + Clerk | Stateless, SSO, MFA nativo |
| **UI Components** | DevExpress 11.1 | shadcn/ui + Radix | Open source, acessibilidade |
| **Hospedagem** | IIS on-premise | Docker em OCI VMs | Portabilidade, orquestração |
| **Versionamento** | TFS/SVN (legado) | Git + GitHub | Workflows modernos, CI/CD |

---

## Mudanças Arquiteturais

### 1. Padrão Arquitetural

**Antes: N-Tier com Facade**
```
Presentation (WebForms)
    ↓
Facade (FachadaXXX.cs)
    ↓
Business Logic (Static BLL)
    ↓
Data Access (Repository<T>)
    ↓
Database (EF 4.3)
```

**Depois: Clean Architecture (Hexagonal)**
```
UI (React) ─────→ API (tRPC)
                      ↓
              Application Layer
                      ↓
              Domain Layer (Entities + Rules)
                      ↓
              Infrastructure (Prisma, CNAB Adapters)
```

**Benefícios:**
- ✅ Testabilidade: Domain isolado de infraestrutura
- ✅ Flexibilidade: Troca de DB/framework sem impactar domínio
- ✅ Manutenibilidade: Responsabilidades claras

### 2. Multi-Tenancy

**Antes: Multi-Database**
- Cada órgão (tenant) tinha seu próprio banco de dados
- Connection string dinâmica via Session
- Migração: Um script de migração por tenant

**Depois: Single Database + RLS**
- Todos os tenants em um único PostgreSQL
- Isolamento via Row-Level Security (RLS) + `tenant_id`
- Queries automáticas filtram por tenant

**Impactos:**
| Aspecto | Legado | Novo |
|---------|--------|------|
| **Custo** | Alto (N databases) | Baixo (1 database) |
| **Backup** | N backups | 1 backup |
| **Escalabilidade** | Vertical por tenant | Horizontal global |
| **Manutenção** | Complexa (N schemas) | Simples (1 schema) |

**Segurança:** RLS garante isolamento no nível de linha.

### 3. Estado da Aplicação

**Antes: Session State (In-Memory)**
- Dados do usuário em `HttpContext.Session`
- Problema: Sticky sessions, não escala horizontalmente

**Depois: Stateless (JWT)**
- Autenticação via JWT (Clerk)
- Contexto do usuário extraído do token
- Escala horizontalmente sem problemas

---

## Mudanças de Modelo de Dados

### Entidades Renomeadas

| Legado (.NET) | Novo (Node.js) | Motivo |
|---------------|----------------|--------|
| `Averbacao` | `Loan` | Nomenclatura internacional |
| `Funcionario` | `Employee` | Idem |
| `Empresa` | `Tenant` (Consignante) / `FinancialInstitution` (Consignatária) | Separação de responsabilidades |
| `AverbacaoTramitacao` | `LoanStatusHistory` | Clareza no propósito |
| `EmpresaSolicitacao` | `WorkflowTask` | Generalização |

### Schema Changes

#### Antes (SQL Server)
```sql
-- Multi-database, cada tenant isolado
CREATE TABLE Averbacao (
    IDAverbacao INT PRIMARY KEY,
    IDFuncionario INT,
    IDEmpresaConsignataria INT,
    -- sem tenant_id (banco separado)
)
```

#### Depois (PostgreSQL)
```sql
-- Single database, RLS
CREATE TABLE loans (
    id UUID PRIMARY KEY,
    tenant_id UUID NOT NULL,
    employee_id UUID,
    financial_institution_id UUID,
    version INT DEFAULT 1, -- Optimistic locking
    CONSTRAINT fk_tenant FOREIGN KEY (tenant_id) REFERENCES tenants(id)
);

-- RLS Policy
CREATE POLICY tenant_isolation ON loans
    USING (tenant_id = current_setting('app.current_tenant_id')::uuid);
```

**Mudanças-chave:**
- ✅ UUIDs substituem INTs autoincrementais
- ✅ `tenant_id` obrigatório em todas as tabelas
- ✅ `version` para optimistic locking (substituiu locks de banco)
- ✅ Timestamps (`created_at`, `updated_at`) em todas as entidades
- ✅ Soft deletes (`deleted_at`) substituem hard deletes

### Merge de Tabelas

**Pessoa + Funcionario → Employee**

**Antes:** Dados pessoais em `Pessoa`, dados funcionais em `Funcionario` (1:N)

**Depois:** Unificado em `Employee` com `PersonalData` embedded

```typescript
// Novo modelo
interface Employee {
  id: string;
  tenantId: string;
  registration: string; // Matrícula
  personalData: {
    fullName: string;
    cpf: string;
    email?: string;
  };
  employment: {
    status: EmploymentStatus;
    category: string;
    salary: Decimal;
  };
}
```

---

## Mudanças de Regras de Negócio

### 1. Cálculo de Margem

**Legado: Sob Demanda**
```csharp
// BLL/Margem.cs
public static decimal CalcularMargemDisponivel(int idFuncionario) {
    var func = Repositorio<Funcionario>.ObterPorId(idFuncionario);
    var averbacoes = Repositorio<Averbacao>.Listar(a => a.IDFuncionario == idFuncionario);
    return func.Salario * 0.35m - averbacoes.Sum(a => a.ValorParcela);
}
```

**Novo: Event-Driven (Margem Viva)**
```typescript
// Domain event
class LoanCreatedEvent {
  employeeId: string;
  installmentAmount: Decimal;
}

// Event handler
class UpdateMarginHandler {
  async handle(event: LoanCreatedEvent) {
    const margin = await this.marginRepository.findByEmployee(event.employeeId);
    margin.decreaseAvailable(event.installmentAmount);
    await this.marginRepository.save(margin); // Atualiza em tempo real
  }
}
```

**Impacto:**
- ✅ Margem sempre atualizada (zero "falsa disponibilidade")
- ✅ Consultas mais rápidas (leitura direta)
- ❌ Complexidade: Event consistency (mitigado por transações ACID)

### 2. Concorrência em Averbações

**Legado: Pessimistic Locking**
```csharp
using (var transaction = db.BeginTransaction()) {
    var func = db.Funcionarios
        .WithLock(LockMode.Exclusive) // Lock de linha
        .Single(f => f.IDFuncionario == id);

    // Lógica de averbação
    transaction.Commit();
}
```

**Novo: Optimistic Locking**
```typescript
// Prisma schema
model Employee {
  id      String
  version Int @default(1)
  @@map("employees")
}

// Service
async createLoan(data) {
  const employee = await prisma.employee.findUnique({ where: { id } });

  await prisma.loan.create({
    data: { ...data, employeeVersion: employee.version }
  });

  await prisma.employee.update({
    where: {
      id: employee.id,
      version: employee.version // Falha se versão mudou
    },
    data: { version: { increment: 1 } }
  });
}
```

**Benefício:** Melhor escalabilidade horizontal (menos locks).

### 3. Fluxos de Aprovação

**Antes:** Lógica hardcoded em `ProcessarAprovacao()`

**Depois:** Workflow Engine configurável

```typescript
// Workflow definition (YAML-based)
{
  steps: [
    { actor: "consignante", action: "approve", required: true },
    { actor: "consignataria", action: "approve", required: true },
    { actor: "employee", action: "confirm", required: false }
  ]
}
```

---

## Integrações

### CNAB/Arquivos Bancários

**Legado: Parsing direto no BLL**
```csharp
// BLL/Importacao.cs
public void ImportarCNAB(string arquivo) {
    var linhas = File.ReadAllLines(arquivo);
    foreach (var linha in linhas) {
        if (linha.Substring(0, 1) == "1") { // Header
            // Parse manual
        }
    }
}
```

**Novo: Anti-Corruption Layer (ACL)**
```typescript
// Infrastructure/CNAB/BancoBrasilAdapter.ts
class BancoBrasilCNABAdapter implements BankFileAdapter {
  parse(file: Buffer): LoanInstallment[] {
    // Parse específico do layout BB
    return installments; // Entidades de domínio puras
  }
}

// Domain não conhece CNAB
interface LoanService {
  processInstallments(installments: LoanInstallment[]): void;
}
```

**Benefício:** Adicionar novo banco não contamina domínio.

### APIs Externas

**Antes:** WCF SOAP
```xml
<soapenv:Envelope>
  <soapenv:Body>
    <ConsultarMargem>
      <CPF>12345678900</CPF>
    </ConsultarMargem>
  </soapenv:Body>
</soapenv:Envelope>
```

**Depois:** tRPC Type-Safe
```typescript
// Servidor
export const appRouter = router({
  consultMargin: publicProcedure
    .input(z.object({ cpf: z.string() }))
    .query(async ({ input }) => {
      return await marginService.getMargin(input.cpf);
    }),
});

// Cliente (type-safe automático)
const margin = await trpc.consultMargin.query({ cpf: "12345678900" });
//    ^? { available: Decimal, total: Decimal }
```

---

## Mudanças de UX/UI

| Aspecto | Legado | Novo |
|---------|--------|------|
| **Paradigma** | Server-side rendering (postbacks) | SPA (client-side) |
| **Estado** | ViewState (hidden fields) | React state + Zustand |
| **Navegação** | Full page reload | Client-side routing |
| **Feedback** | Síncrono (bloqueante) | Assíncrono (loading states) |
| **Responsividade** | Desktop-only | Mobile-first (Tailwind) |
| **Acessibilidade** | Limitada | WCAG 2.1 AA (Radix UI) |

---

## Limitações Resolvidas

### 1. Problemas de Concorrência
**Legado:** Race conditions em averbações simultâneas causavam margem negativa.
**Novo:** Optimistic locking + event-driven garantem consistência.

### 2. Performance
**Legado:** N+1 queries em grids, margem recalculada a cada acesso.
**Novo:** Query optimization (joins), margem pré-calculada.

### 3. Testabilidade
**Legado:** BLL com métodos estáticos impossibilitava unit tests.
**Novo:** Dependency Injection permite mocks completos.

### 4. Escalabilidade
**Legado:** Session state impedia load balancing.
**Novo:** Stateless permite escala horizontal ilimitada.

---

## Roadmap de Migração

### Fase 1: Convivência (Atual)
- ✅ Novo sistema em desenvolvimento
- ✅ Sistema legado em produção
- 🔄 Dados não sincronizados (ambientes separados)

### Fase 2: Migração de Dados
- 📅 ETL de SQL Server → PostgreSQL
- 📅 Transformação de schemas
- 📅 Validação de integridade

### Fase 3: Transição
- 📅 Piloto com 1-2 tenants no novo sistema
- 📅 Feedback e ajustes
- 📅 Rollout gradual

### Fase 4: Descomissionamento
- 📅 Todos os tenants migrados
- 📅 Sistema legado somente leitura (6 meses)
- 📅 Desligamento final

---

## Riscos e Mitigações

| Risco | Probabilidade | Impacto | Mitigação |
|-------|---------------|---------|-----------|
| **Diferenças em cálculos** | Média | Alto | Testes side-by-side com dados reais |
| **Perda de dados na migração** | Baixa | Crítico | ETL com rollback, validação rigorosa |
| **Curva de aprendizado (TypeScript)** | Alta | Médio | Treinamento, pair programming |
| **Bugs em ACL de CNAB** | Alta | Alto | Testes extensivos, comparação com legado |
| **Resistência dos usuários** | Média | Médio | UX melhorada, treinamento, suporte |

---

## Checklist de Migração

### Para Cada Feature
- [ ] Regra de negócio documentada (comparar com legado)
- [ ] Testes unitários (cobertura > 80%)
- [ ] Testes E2E (fluxos principais)
- [ ] Validação side-by-side com legado
- [ ] Documentação atualizada

### Para Cada Tenant
- [ ] Dados migrados (ETL executado)
- [ ] Validação de integridade (checksums)
- [ ] Treinamento de usuários
- [ ] Suporte dedicado (1 semana)
- [ ] Rollback plan testado

---

## Recursos

- **Sistema Legado:** [/docs/archive/dotnet/](./dotnet/)
- **Novo Sistema:** [/docs/03-architecture/](../03-architecture/)
- **Decisões Técnicas:** [decisoes-tecnicas.md](../03-architecture/decisoes-tecnicas.md)
- **Regras de Negócio:** [regras-negocio.md](../05-business-rules/regras-negocio.md)

---

**Última atualização:** Janeiro 2026
**Responsável:** Product & Engineering Team
