# Documentação do Sistema Legado

⚠️ **ATENÇÃO: Esta documentação é SOMENTE para referência**

Esta pasta contém a documentação do sistema LEGADO FastConsig construído em .NET Framework 4.0 / ASP.NET WebForms.

**Para documentação do NOVO sistema (Node.js/TypeScript), volte para [/docs](../)**

## Estrutura

- **[legacy-system-overview.md](./legacy-system-overview.md)**: Visão geral do sistema .NET
- **[migration-notes.md](./migration-notes.md)**: Principais mudanças na migração
- **[dotnet/](./dotnet/)**: Documentação técnica detalhada (14 arquivos)

## Quando Consultar

✅ **Use esta documentação para:**
- Entender regras de negócio do sistema atual em produção
- Comparar comportamentos durante migração
- Identificar algoritmos críticos (margem, parcelas, conciliação)
- Mapear entidades existentes para o novo modelo
- Consultar integrações WCF e layouts CNAB atuais

❌ **NÃO use para:**
- Implementar novas features (use [docs/03-architecture/](../03-architecture/))
- Definir stack técnico (use [docs/03-architecture/decisoes-tecnicas.md](../03-architecture/decisoes-tecnicas.md))
- Design de APIs (use [docs/06-integrations/](../06-integrations/))
- Padrões de código (sistema legado usa padrões antigos)

## Principais Diferenças

| Conceito | Legado (.NET) | Novo (Node.js) | Motivo da Mudança |
|----------|---------------|----------------|-------------------|
| **Multi-tenancy** | Multi-database (1 DB por tenant) | Single DB + RLS (tenant_id) | Custos de infraestrutura, manutenção simplificada |
| **Autenticação** | Forms Authentication + Session | JWT + Clerk | Stateless, SSO, MFA nativo |
| **ORM** | Entity Framework 4.3 (DB-first) | Prisma ORM (Schema-first) | Type safety, migrations declarativas |
| **API** | SOAP/WCF | tRPC | Type safety end-to-end, DX moderno |
| **Camadas** | N-Tier (Facade pattern) | Clean Architecture (Hexagonal) | Testabilidade, baixo acoplamento |
| **Frontend** | ASP.NET WebForms | React 19 SPA | Componentização, estado reativo |
| **Deploy** | IIS on-premise | Docker em OCI | Portabilidade, orquestração |
| **Concorrência** | Locks de banco + Session | Optimistic locking + versioning | Escalabilidade horizontal |

## Arquitetura Legada

### Camadas (N-Tier)
```
CP.FastConsig.WebApplication (Presentation)
    ↓
CP.FastConsig.Facade (Facade Pattern)
    ↓
CP.FastConsig.BLL (Business Logic - Static Classes)
    ↓
CP.FastConsig.DAL (Data Access - EF 4.3 + Repository)
    ↓
SQL Server (Multi-Database)
```

### Tecnologias Principais
- **.NET Framework 4.0** (2010)
- **ASP.NET WebForms** com ViewState
- **DevExpress 11.1** para grids e controles
- **Entity Framework 4.3.1** (Database First)
- **WCF Services** (SOAP)
- **SQL Server 2008 R2+**
- **IIS 7.0+** para hosting

### Padrões Identificados
1. **Facade para cada página** (ex: `FachadaAverbacoes.cs` → `Averbacoes.aspx`)
2. **Static BLL classes** com métodos estáticos
3. **Repository genérico** `Repositorio<T>` para acesso a dados
4. **Session-based state** via `DadosSessao`
5. **EDMX único** com todas as entidades

## Documentação Técnica Detalhada

📁 **[dotnet/](./dotnet/)** contém:

1. **[architecture.md](./dotnet/architecture.md)** - Arquitetura N-Tier, camadas, patterns
2. **[business-rules.md](./dotnet/business-rules.md)** - Regras implementadas no BLL
3. **[data-models.md](./dotnet/data-models.md)** - Entidades EF, relacionamentos
4. **[api-contracts.md](./dotnet/api-contracts.md)** - WCF services (SOAP)
5. **[component-inventory.md](./dotnet/component-inventory.md)** - User controls, grids
6. **[critical-algorithms.md](./dotnet/critical-algorithms.md)** - Cálculo de margem, parcelas
7. **[technology-stack.md](./dotnet/technology-stack.md)** - Versões e dependências
8. **[development-guide.md](./dotnet/development-guide.md)** - Setup VS 2010, build
9. **[deployment-guide.md](./dotnet/deployment-guide.md)** - Deploy IIS, RedGate
10. **[source-tree-analysis.md](./dotnet/source-tree-analysis.md)** - Estrutura do código

## Entidades Principais (Legacy)

| Entidade Legacy | Entidade Nova | Mudanças |
|-----------------|---------------|----------|
| `Averbacao` | `Loan` | Nome em inglês, campos simplificados |
| `Funcionario` + `Pessoa` | `Employee` | Merged em uma entidade |
| `Empresa` (Consignante + Consignatária) | `Tenant` + `FinancialInstitution` | Separação clara de responsabilidades |
| `AverbacaoTramitacao` | `LoanStatusHistory` | Audit trail explícito |
| `Margem` (calculada) | `Margin` (entidade persistida) | Margem Viva (event-driven) |

## Regras de Negócio Preservadas

Estas regras foram mantidas intactas na migração:

✅ **Cálculo de Margem:**
- Margem Bruta = Salário × Percentual Configurado
- Margem Disponível = Margem Bruta - Parcelas Averbadas
- Bloqueios por funcionário/produto respeitados

✅ **Fluxo de Averbação:**
- Estados: Pré-Reserva → Aguardando Aprovação → Averbado → Liquidado
- Validações de margem antes de averbar
- Geração de parcelas somente após confirmação

✅ **Conciliação Mensal:**
- Importação de retornos bancários
- Comparação com valores averbados
- Destaque de divergências

✅ **Aprovações:**
- Fluxos configuráveis por produto
- Múltiplos aprovadores (Consignante, Consignatária, Funcionário)
- Histórico completo de tramitações

## Mudanças Conceituais

### 1. Multi-Tenancy
**Antes:** Cada órgão tinha seu próprio banco de dados.
**Depois:** Todos os órgãos compartilham um banco com isolamento por RLS + tenant_id.

**Impacto:** Queries sempre filtram por tenant_id automaticamente.

### 2. Margem Viva
**Antes:** Margem calculada sob demanda a cada consulta.
**Depois:** Margem atualizada em tempo real por eventos (Event Sourcing).

**Impacto:** Performance superior, zero "falsa disponibilidade".

### 3. API-First
**Antes:** SOAP/WCF como serviço auxiliar.
**Depois:** tRPC API como core, WebApp é apenas um cliente.

**Impacto:** Integração com mobile/bots sem esforço adicional.

## Migrando Conhecimento

### Para Desenvolvedores

**Ao implementar uma feature:**
1. Consulte o BLL legado para entender a regra (`CP.FastConsig.BLL/`)
2. Leia `business-rules.md` para contexto
3. Implemente no novo sistema seguindo Clean Architecture
4. Mantenha testes unitários (legado não tinha)

**Exemplo:**
Implementando cálculo de margem → veja `CP.FastConsig.BLL/Margem.cs` e `critical-algorithms.md`

### Para Arquitetos

**Ao tomar decisões:**
1. Consulte `architecture.md` para entender constraints legados
2. Veja `decisoes-tecnicas.md` (novo sistema) para ADRs
3. Use `migration-notes.md` para entender trade-offs

## Limitações Conhecidas do Sistema Legado

Estes problemas foram resolvidos no novo sistema:

1. ❌ **Concorrência:** Race conditions em averbações simultâneas
2. ❌ **Performance:** N+1 queries em grids de funcionários
3. ❌ **Escalabilidade:** Session state impede load balancing
4. ❌ **Testabilidade:** BLL com static methods dificulta mocks
5. ❌ **Manutenção:** EDMX monolítico com 100+ entidades
6. ❌ **DevEx:** Recompilação completa a cada mudança

Ver detalhes em [migration-notes.md](./migration-notes.md#limitações-resolvidas)

## Suporte

Este sistema legado ainda está em produção durante a fase de transição.

**Para dúvidas sobre o sistema legado:**
- Consulte esta documentação primeiro
- Entre em contato com a equipe de manutenção legacy

**Para desenvolvimento do novo sistema:**
- Use [/docs](../) (documentação principal)
- Não replique padrões legados - siga Clean Architecture

---

📅 **Última atualização:** Janeiro 2026
🔖 **Sistema Legado:** CP.FastConsig (2010-2026)
🚀 **Novo Sistema:** FastConsig v2 (Node.js/React)
