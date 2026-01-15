# Documentação FastConsig

Sistema de gestão de empréstimos consignados construído com Node.js 22, React 19 e PostgreSQL 16.

## Navegação Rápida

| Categoria | Descrição | Docs-chave |
|-----------|-----------|-----------|
| **[01-product](./01-product/)** | Visão de produto, PRD | prd.md |
| **[02-planning](./02-planning/)** | Epics, stories, sprints | user-stories.md |
| **[03-architecture](./03-architecture/)** | Decisões técnicas, ER | ⭐ decisoes-tecnicas.md (APROVADO v1.1) |
| **[04-ux-design](./04-ux-design/)** | Wireframes, design system | wireframes.md |
| **[05-business-rules](./05-business-rules/)** | Regras de negócio | regras-negocio.md |
| **[06-integrations](./06-integrations/)** | APIs, layouts de arquivo | api-specification.md |
| **[07-operations](./07-operations/)** | Deploy, infra, monitoring | (em construção) |
| **[08-development](./08-development/)** | Setup, testes, standards | (em construção) |
| **[09-sprints](./09-sprints/)** | Roadmap, planejamento | plano-sprints.md |
| **[archive](./archive/)** | Sistema legado .NET | ⚠️ Referência apenas |

## Quick Start por Papel

### Product Owner
1. [PRD](./01-product/prd.md) - Visão e requisitos completos
2. [User Stories](./02-planning/user-stories.md) - Features detalhadas com acceptance criteria
3. [Roadmap](./09-sprints/plano-sprints.md) - Cronograma de implementação

### Desenvolvedor
1. ⭐ [Decisões Técnicas](./03-architecture/decisoes-tecnicas.md) - Stack completo (APROVADO v1.1)
2. [Arquitetura](./03-architecture/arquitetura-tecnica.md) - Padrões de implementação
3. [Diagrama ER](./03-architecture/diagrama-er.md) - Modelo de dados PostgreSQL
4. [Regras de Negócio](./05-business-rules/regras-negocio.md) - Lógica do sistema
5. [API Specification](./06-integrations/api-specification.md) - Contratos das APIs

### Designer
1. [Wireframes](./04-ux-design/wireframes.md) - Layouts e componentes
2. [PRD - Seção UX](./01-product/prd.md#7-user-experience) - Princípios de design

### Arquiteto
1. ⭐ [Decisões Técnicas](./03-architecture/decisoes-tecnicas.md) - ADRs e justificativas
2. [Arquitetura Técnica](./03-architecture/arquitetura-tecnica.md) - Padrões arquiteturais
3. [Diagrama ER](./03-architecture/diagrama-er.md) - Esquema de dados
4. [Sistema Legado](./archive/) - Comparação e migração

## Sistema Legado vs Novo

| Aspecto | Legado (.NET) | Novo (Node.js) |
|---------|---------------|----------------|
| Runtime | .NET Framework 4.0 | Node.js 22 LTS |
| Linguagem | C# | TypeScript 5.5+ |
| Frontend | ASP.NET WebForms | React 19 SPA |
| Backend | WCF | Fastify + tRPC |
| Database | SQL Server (multi-DB) | PostgreSQL 16 (RLS) |
| ORM | Entity Framework 4.3 | Prisma ORM |
| Auth | Forms Authentication | JWT + Clerk |
| Deploy | IIS on-premise | Docker OCI |

📁 [Documentação completa do sistema legado](./archive/)

## Principais Conceitos

### Glossário Básico

| Termo | Definição |
|-------|-----------|
| **Averbação** | Registro de um empréstimo consignado no sistema |
| **Consignante** | Órgão empregador que processa os descontos em folha |
| **Consignatária** | Instituição financeira que concede o empréstimo |
| **Funcionário/Servidor** | Pessoa física que contrata o empréstimo |
| **Margem** | Percentual do salário disponível para consignação |
| **Competência** | Mês/ano de referência (formato AAAA/MM) |

Ver glossário completo em [01-product/prd.md](./01-product/prd.md#12-glossário)

## Estrutura da Documentação

### 📦 01-product
Definições de produto, visão, PRD com requisitos funcionais e não-funcionais.

### 📋 02-planning
Epics, user stories com acceptance criteria, breakdown de features.

### 🏗️ 03-architecture
**Arquivo crítico:** `decisoes-tecnicas.md` v1.1 APROVADO contém todas as decisões técnicas.
Também inclui arquitetura detalhada, diagrama ER do PostgreSQL.

### 🎨 04-ux-design
Wireframes de todas as telas principais, especificações de design.

### 📜 05-business-rules
Regras de negócio numeradas (RN-XXX-NNN), permissões RBAC, fluxos de aprovação.

### 🔌 06-integrations
Especificação OpenAPI das APIs, layouts de arquivos de importação/exportação.

### 🚀 07-operations
*Em construção* - Guias de deployment, infraestrutura OCI, monitoring.

### 💻 08-development
*Em construção* - Setup local, testes, coding standards.

### 📅 09-sprints
Planejamento de sprints, roadmap, breakdown de fases.

### 📚 archive
Documentação do sistema legado .NET para referência e comparação durante migração.

## Padrões de Inovação

O FastConsig implementa 4 padrões inovadores documentados no [PRD](./01-product/prd.md#14-padrões-de-inovação):

1. **Margem Viva** - Cálculo reativo em tempo real por eventos
2. **Banking ACL** - Isolamento total de layouts CNAB legados
3. **API-First** - Backend como plataforma desde o D0
4. **Push Intelligence** - Alertas proativos de anomalias de negócio

## Status do Projeto

**Fase Atual:** Sprint 2-3 (Épico 2 - Core Employees)

**Épicos Concluídos:**
- ✅ Épico 1: Foundation (Multi-tenancy, Auth, Config)

**Em Andamento:**
- 🔄 Épico 2: Core Employees (CRUD, Margem, Import)

**Próximos:**
- 📅 Épico 3: Core Loan Endorsements (Averbações)

Ver status completo em [_bmad-output/implementation-artifacts/sprint-status.yaml](../_bmad-output/implementation-artifacts/sprint-status.yaml)

## Contribuindo

Esta documentação é mantida de forma colaborativa. Ao atualizar:

1. Mantenha a estrutura de categorias
2. Adicione metadata YAML no início dos arquivos
3. Atualize cross-references quando mover/renomear arquivos
4. Documente decisões arquiteturais em `decisoes-tecnicas.md`

## Links Úteis

- **Monorepo:** `/mnt/c/Users/eudre/OneDrive/Desktop/Backup/Projetos/Fast Consig`
- **Implementation Artifacts:** [../_bmad-output/implementation-artifacts/](../_bmad-output/implementation-artifacts/)
- **Plano de Consolidação:** [/home/eudres/.claude/plans/elegant-juggling-quail.md]

---

*Documentação consolidada em Janeiro 2026*
