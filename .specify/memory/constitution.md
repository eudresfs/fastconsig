<!--
SYNC IMPACT REPORT
==================
Version Change: 1.0.0 → 1.1.0 (MINOR - Added 10 new principles)
Modified Principles: None
Added Sections:
  - Article XI: Portuguese Domain Language
  - Article XII: Naming Conventions
  - Article XIII: File and Folder Structure
  - Article XIV: Code Documentation
  - Article XV: Error Handling
  - Article XVI: API Design
  - Article XVII: Database Conventions
  - Article XVIII: UI/UX Consistency
  - Article XIX: Git Workflow
  - Article XX: Dependency Management
Removed Sections: None
Templates Requiring Updates:
  - .specify/templates/plan-template.md: ✅ Compatible
  - .specify/templates/spec-template.md: ✅ Compatible
  - .specify/templates/tasks-template.md: ✅ Compatible
Follow-up TODOs: None
-->

# FastConsig Constitution

## Core Principles

### I. Multi-Tenancy First

Todo codigo DEVE suportar multi-tenancy desde o inicio. Isolamento de dados por tenant e inegociavel.

**Regras:**
- Toda entidade de dominio DEVE possuir `tenantId` como campo obrigatorio
- Queries DEVEM sempre filtrar por `tenantId` - nunca confiar apenas em Row Level Security
- Indices compostos DEVEM incluir `tenantId` como primeiro campo
- Testes DEVEM validar isolamento entre tenants
- Cross-tenant queries sao PROIBIDAS exceto para funcionalidades administrativas explicitamente autorizadas

**Rationale:** O sistema gerencia dados financeiros sensiveis de multiplos orgaos publicos. Vazamento de dados entre tenants representa risco legal e reputacional critico.

### II. Type Safety End-to-End

Tipagem estatica e obrigatoria em todo o codebase. Nenhum `any` e permitido em producao.

**Regras:**
- TypeScript strict mode DEVE estar habilitado (`strict: true`)
- Uso de `any` e PROIBIDO - usar `unknown` com type guards quando necessario
- Schemas Zod DEVEM ser a fonte de verdade para tipos de API
- tRPC DEVE ser usado para garantir type safety entre frontend e backend
- Prisma client DEVE ser usado para queries tipadas - raw SQL apenas com justificativa documentada

**Rationale:** Bugs de tipo em sistema financeiro podem causar calculos incorretos de margem e parcelas, resultando em prejuizo financeiro e problemas legais.

### III. Business Rules in Domain Layer

Regras de negocio DEVEM residir exclusivamente na camada de dominio/service, nunca em controllers ou componentes de UI.

**Regras:**
- Validacoes de negocio (margem, situacao funcional, bloqueios) DEVEM estar em services
- Controllers/routers DEVEM apenas orquestrar chamadas e transformar dados
- Componentes React DEVEM apenas exibir dados e capturar input
- Calculos financeiros (margem, parcelas, coeficientes) DEVEM ter testes unitarios com 100% de cobertura
- Regras documentadas em `regras-negocio.md` DEVEM ter implementacao correspondente rastreavel

**Rationale:** Centralizar regras de negocio garante consistencia, facilita auditoria e previne duplicacao de logica critica.

### IV. Audit Trail Mandatory

Toda operacao que modifica estado de averbacao, funcionario ou margem DEVE gerar registro de auditoria.

**Regras:**
- Entidades criticas DEVEM ter campos `createdAt`, `createdBy`, `updatedAt`, `updatedBy`
- Mudancas de situacao de averbacao DEVEM gerar registro em `AverbacaoHistorico`
- Logs estruturados (Pino) DEVEM incluir `tenantId`, `userId`, `action`, `entityId`
- Registros de auditoria sao IMUTAVEIS - nunca atualizar ou deletar
- Retencao minima de 5 anos para dados de auditoria (requisito legal)

**Rationale:** Sistema financeiro regulado exige rastreabilidade completa para auditorias internas e externas.

### V. Permission-Based Access Control

Toda operacao DEVE validar permissoes do usuario. Acesso negado por padrao.

**Regras:**
- Endpoints DEVEM declarar permissoes requeridas via middleware `withPermission()`
- Permissoes DEVEM seguir padrao `MODULO_ACAO` (ex: `AVERBACOES_APROVAR`)
- Verificacao de tenant DEVE ocorrer antes de verificacao de permissao
- Frontend DEVE esconder elementos para os quais usuario nao tem permissao
- Backend DEVE sempre revalidar permissoes - nunca confiar apenas no frontend

**Rationale:** Dados financeiros e pessoais exigem controle granular de acesso conforme LGPD e politicas internas dos orgaos.

### VI. Validation at Boundaries

Toda entrada de dados externa DEVE ser validada na fronteira do sistema.

**Regras:**
- Inputs de API DEVEM ser validados com schemas Zod antes de processamento
- Arquivos importados DEVEM passar por validacao de formato e conteudo
- CPF, CNPJ e valores monetarios DEVEM ter validacao especifica
- Erros de validacao DEVEM retornar mensagens claras e acionaveis
- Nunca confiar em dados vindos do frontend - sempre revalidar no backend

**Rationale:** Prevencao de injection, corrupcao de dados e erros de calculo financeiro.

### VII. Margin Consistency

Calculos de margem DEVEM ser consistentes e atomicos. Margem negativa e PROIBIDA.

**Regras:**
- Reserva de margem DEVE ocorrer em transacao atomica com criacao de averbacao
- Liberacao de margem DEVE ocorrer apenas em transicoes de estado validas
- Calculo de margem disponivel DEVE considerar todos os grupos compartilhados
- Autorizacoes especiais DEVEM ser rastreadas e ter validade definida
- Testes DEVEM cobrir cenarios de concorrencia (multiplas averbacoes simultaneas)

**Rationale:** Margem incorreta pode resultar em averbacoes indevidas, causando prejuizo ao funcionario e problemas legais ao orgao.

### VIII. State Machine for Averbacao

Transicoes de estado de averbacao DEVEM seguir maquina de estados definida. Transicoes invalidas sao PROIBIDAS.

**Regras:**
- Transicoes permitidas DEVEM estar documentadas e implementadas como state machine
- Toda transicao DEVE gerar registro de tramitacao
- Transicoes DEVEM validar pre-condicoes (permissoes, situacao atual, regras de negocio)
- Estados terminais (Cancelado, Concluido, Liquidado) nao permitem transicao de saida
- Rollback de estado DEVE ser explicitamente autorizado e auditado

**Rationale:** Garantir integridade do ciclo de vida de contratos e compliance com regras de negocio.

### IX. Test-Driven Quality

Codigo critico DEVE ter testes antes de ir para producao. Cobertura minima e obrigatoria.

**Regras:**
- Servicos de calculo (margem, parcelas, simulacao) DEVEM ter 90% de cobertura
- Routers/endpoints DEVEM ter testes de integracao
- Fluxos criticos (login, averbacao, aprovacao) DEVEM ter testes E2E
- PRs DEVEM passar em todos os testes antes de merge
- Cobertura geral minima: 70% (statements, branches, functions, lines)

**Rationale:** Sistema financeiro nao tolera bugs em producao. Testes previnem regressoes e documentam comportamento esperado.

### X. Open Source Priority

Tecnologias open source DEVEM ser priorizadas. Vendor lock-in DEVE ser minimizado.

**Regras:**
- Bibliotecas open source DEVEM ser preferidas sobre alternativas proprietarias
- Servicos cloud DEVEM usar APIs padronizadas quando possivel
- Dados DEVEM ser exportaveis em formatos abertos
- Dependencias DEVEM ter licencas compativeis (MIT, Apache 2.0, BSD)
- Alternativas proprietarias requerem justificativa documentada em ADR

**Rationale:** Reducao de custos, independencia de fornecedores e alinhamento com politicas de TI do setor publico.

### XI. Portuguese Domain Language

Termos de dominio DEVEM usar portugues. Codigo tecnico DEVE usar ingles.

**Regras:**
- Entidades de dominio em portugues: `Funcionario`, `Averbacao`, `Consignataria`, `Margem`
- Campos de entidade em portugues: `cpf`, `nome`, `matricula`, `valorParcela`, `situacao`
- Enums de negocio em portugues: `AverbacaoSituacao`, `FuncionarioSituacao`
- Codigo tecnico em ingles: `createRouter`, `withPermission`, `handleError`, `validateInput`
- Mensagens de erro para usuario em portugues brasileiro
- Logs e mensagens internas de sistema em ingles
- Documentacao tecnica em portugues (specs, PRD, regras de negocio)
- Comentarios de codigo em portugues quando explicam regra de negocio, ingles para explicacoes tecnicas

**Exemplos:**
```typescript
// Correto
const funcionario = await prisma.funcionario.findFirst({ where: { cpf } })
const margemDisponivel = calcularMargemDisponivel(funcionario)
throw new BusinessError('Margem insuficiente para esta operacao')

// Incorreto
const employee = await prisma.employee.findFirst({ where: { taxId } })
const availableMargin = calculateAvailableMargin(employee)
```

**Rationale:** Manter terminologia consistente com o dominio de consignados brasileiro, facilitando comunicacao com stakeholders e usuarios.

### XII. Naming Conventions

Nomenclatura DEVE seguir padroes consistentes em todo o codebase.

**Regras:**

| Elemento | Padrao | Exemplo |
|----------|--------|---------|
| Variaveis/Funcoes | camelCase | `valorParcela`, `calcularMargem()` |
| Classes/Types/Interfaces | PascalCase | `Funcionario`, `AverbacaoService` |
| Constantes | SCREAMING_SNAKE_CASE | `MAX_PRAZO`, `SITUACAO_ATIVO` |
| Arquivos TS/TSX | kebab-case | `averbacao.service.ts`, `login-form.tsx` |
| Pastas | kebab-case | `funcionarios/`, `auth-context/` |
| Componentes React | PascalCase (arquivo e export) | `LoginForm.tsx` exporta `LoginForm` |
| Hooks | use + PascalCase | `useAuth`, `useFuncionario` |
| Schemas Zod | nome + Schema | `loginSchema`, `averbacaoCreateSchema` |
| Routers tRPC | nome + Router | `authRouter`, `averbacaoRouter` |
| Tabelas DB | snake_case singular | `funcionario`, `averbacao_historico` |
| Colunas DB | snake_case | `tenant_id`, `valor_parcela`, `created_at` |
| Enums DB | SCREAMING_SNAKE_CASE | `ATIVO`, `AGUARDANDO_APROVACAO` |

**Prefixos e Sufixos Padrao:**
- Services: `*.service.ts`
- Routers: `*.router.ts`
- Schemas: `*.schema.ts`
- Types: `*.types.ts`
- Testes: `*.test.ts` ou `*.spec.ts`
- Componentes: `*.tsx`
- Hooks: `use*.ts`

**Rationale:** Consistencia de nomenclatura reduz carga cognitiva e facilita navegacao no codebase.

### XIII. File and Folder Structure

Estrutura de pastas DEVE seguir organizacao por feature/modulo.

**Backend (apps/api):**
```
src/
├── config/                 # Configuracoes globais
│   ├── env.ts
│   ├── auth.ts
│   └── logger.ts
├── modules/                # Modulos de dominio
│   ├── auth/
│   │   ├── auth.router.ts
│   │   ├── auth.service.ts
│   │   ├── auth.schema.ts
│   │   └── __tests__/
│   ├── funcionarios/
│   ├── averbacoes/
│   └── [modulo]/
├── shared/                 # Codigo compartilhado
│   ├── middleware/
│   ├── utils/
│   └── types/
├── trpc/                   # Configuracao tRPC
│   ├── trpc.ts
│   ├── context.ts
│   └── router.ts
└── server.ts
```

**Frontend (apps/web):**
```
src/
├── components/             # Componentes reutilizaveis
│   ├── ui/                 # Componentes base (shadcn)
│   └── layout/             # Layouts
├── features/               # Features por dominio
│   ├── auth/
│   │   ├── components/
│   │   ├── hooks/
│   │   └── pages/
│   ├── funcionarios/
│   └── [feature]/
├── hooks/                  # Hooks globais
├── lib/                    # Utilitarios e configs
├── routes/                 # TanStack Router
├── stores/                 # Zustand stores
└── styles/
```

**Regras:**
- Um modulo/feature por pasta
- Arquivos relacionados ficam juntos (colocation)
- Testes ficam em `__tests__/` dentro do modulo
- Imports usam path aliases: `@/modules/`, `@/shared/`, `@/components/`
- Maximo 7 arquivos por pasta antes de criar subpastas

**Rationale:** Estrutura previsivel facilita onboarding e manutencao.

### XIV. Code Documentation

Codigo DEVE ser auto-documentado. Comentarios apenas quando necessario.

**Regras:**
- Funcoes publicas DEVEM ter JSDoc com descricao, params e return
- Regras de negocio complexas DEVEM ter comentario explicativo
- TODOs DEVEM ter formato: `// TODO(autor): descricao`
- FIXMEs DEVEM ter formato: `// FIXME(autor): descricao`
- Comentarios NAO DEVEM explicar o obvio
- Codigo morto DEVE ser removido, nao comentado

**JSDoc Padrao:**
```typescript
/**
 * Calcula a margem disponivel do funcionario para um grupo de produto.
 * Considera margem base menos deducoes de averbacoes ativas.
 *
 * @param funcionarioId - ID do funcionario
 * @param produtoGrupoId - ID do grupo de produto
 * @returns Valor da margem disponivel em reais
 * @throws {BusinessError} Se funcionario nao encontrado
 *
 * @example
 * const margem = await calcularMargemDisponivel(123, 1)
 * // margem = 450.00
 */
async function calcularMargemDisponivel(
  funcionarioId: number,
  produtoGrupoId: number
): Promise<number>
```

**README por Modulo:**
- Cada modulo DEVE ter README.md se tiver logica complexa
- README DEVE explicar responsabilidades e dependencias

**Rationale:** Documentacao adequada facilita manutencao e onboarding sem poluir codigo com comentarios desnecessarios.

### XV. Error Handling

Erros DEVEM ser tratados de forma consistente e informativa.

**Hierarquia de Erros:**
```typescript
// Base
class AppError extends Error {
  code: string
  statusCode: number
  isOperational: boolean
}

// Negocio
class BusinessError extends AppError { statusCode = 400 }
class ValidationError extends AppError { statusCode = 400 }
class NotFoundError extends AppError { statusCode = 404 }
class UnauthorizedError extends AppError { statusCode = 401 }
class ForbiddenError extends AppError { statusCode = 403 }

// Infraestrutura
class DatabaseError extends AppError { statusCode = 500 }
class ExternalServiceError extends AppError { statusCode = 502 }
```

**Regras:**
- Erros de negocio DEVEM usar classes especificas, nunca `throw new Error()`
- Codigos de erro DEVEM seguir padrao: `MODULO_ERRO` (ex: `AVERBACAO_MARGEM_INSUFICIENTE`)
- Mensagens de erro para usuario DEVEM ser claras e acionaveis
- Stack traces NAO DEVEM ser expostos em producao
- Erros DEVEM ser logados com contexto (tenantId, userId, action)
- Erros de validacao DEVEM listar todos os campos invalidos

**Response de Erro Padrao:**
```json
{
  "success": false,
  "error": {
    "code": "AVERBACAO_MARGEM_INSUFICIENTE",
    "message": "Margem insuficiente para esta operacao",
    "details": {
      "margemDisponivel": 300.00,
      "valorSolicitado": 450.00
    }
  }
}
```

**Rationale:** Tratamento consistente de erros facilita debugging e melhora experiencia do usuario.

### XVI. API Design

APIs DEVEM seguir padroes consistentes de design e versionamento.

**tRPC Conventions:**
```typescript
// Nomenclatura de procedures
router({
  // Queries (leitura)
  list: procedure.query(),        // Listar com paginacao
  getById: procedure.query(),     // Buscar por ID
  getByCpf: procedure.query(),    // Buscar por campo especifico

  // Mutations (escrita)
  create: procedure.mutation(),   // Criar
  update: procedure.mutation(),   // Atualizar
  delete: procedure.mutation(),   // Remover (soft delete preferido)

  // Acoes especificas
  aprovar: procedure.mutation(),  // Acao de dominio
  rejeitar: procedure.mutation(),
  suspender: procedure.mutation(),
})
```

**Paginacao Padrao:**
```typescript
// Input
{
  page: number        // 1-indexed
  pageSize: number    // max 100
  orderBy: string     // campo
  orderDir: 'asc' | 'desc'
  search?: string     // busca geral
  filters?: object    // filtros especificos
}

// Output
{
  data: T[]
  pagination: {
    page: number
    pageSize: number
    total: number
    totalPages: number
  }
}
```

**Regras:**
- IDs DEVEM ser numeros inteiros (bigint no banco)
- Datas DEVEM ser ISO 8601 (UTC no banco, timezone do usuario no frontend)
- Valores monetarios DEVEM ser Decimal no banco, number no TypeScript
- Soft delete preferido: campo `deletedAt` ao inves de remocao fisica
- Bulk operations DEVEM ter limite maximo (ex: 100 itens)

**Rationale:** APIs consistentes reduzem fricao de integracao e bugs de comunicacao.

### XVII. Database Conventions

Banco de dados DEVE seguir padroes de modelagem e nomenclatura.

**Nomenclatura:**
- Tabelas: snake_case singular (`funcionario`, `averbacao`)
- Colunas: snake_case (`tenant_id`, `valor_parcela`)
- PKs: `id` (bigserial)
- FKs: `{tabela}_id` (`funcionario_id`, `consignataria_id`)
- Timestamps: `created_at`, `updated_at`, `deleted_at`
- Indices: `idx_{tabela}_{colunas}` (`idx_funcionario_tenant_cpf`)
- Constraints: `{tabela}_{tipo}_{colunas}` (`funcionario_uk_tenant_cpf`)

**Campos Padrao em Todas Tabelas:**
```sql
id          BIGSERIAL PRIMARY KEY,
tenant_id   BIGINT NOT NULL REFERENCES tenant(id),
created_at  TIMESTAMPTZ NOT NULL DEFAULT NOW(),
updated_at  TIMESTAMPTZ NOT NULL DEFAULT NOW(),
created_by  BIGINT REFERENCES usuario(id),
updated_by  BIGINT REFERENCES usuario(id)
```

**Regras:**
- Toda tabela DEVE ter `tenant_id` (exceto `tenant`)
- Toda tabela DEVE ter timestamps de auditoria
- FKs DEVEM ter indices
- Indices compostos: colunas mais seletivas primeiro
- NUNCA usar `CASCADE DELETE` em tabelas de dominio
- Enums DEVEM ser strings no banco (facilita debugging)
- JSONs DEVEM ser usados apenas para dados nao estruturados

**Migrations:**
- Uma migration por alteracao logica
- Migrations DEVEM ser reversiveis
- Nome: `YYYYMMDDHHMMSS_descricao.sql`
- Testar em staging antes de producao

**Rationale:** Padroes de banco garantem integridade e facilitam queries.

### XVIII. UI/UX Consistency

Interface DEVE seguir padroes visuais e de interacao consistentes.

**Design System:**
- Cores: Usar CSS variables do tema (`--primary`, `--destructive`, etc.)
- Espacamento: Multiplos de 4px (Tailwind spacing scale)
- Tipografia: Font family do sistema, scale do Tailwind
- Bordas: `rounded-md` padrao, `rounded-lg` para cards
- Sombras: `shadow-sm` para elevacao sutil, `shadow-md` para modais

**Componentes Padrao:**
| Acao | Componente |
|------|------------|
| Acao primaria | `<Button>` |
| Acao destrutiva | `<Button variant="destructive">` |
| Formularios | React Hook Form + Zod |
| Tabelas de dados | TanStack Table |
| Feedbacks | Sonner (toasts) |
| Modais | Dialog (Radix) |
| Loading | Skeleton ou Spinner |
| Empty state | Ilustracao + mensagem + acao |

**Padroes de Interacao:**
- Loading: Mostrar skeleton enquanto carrega
- Erro: Toast para erros de acao, inline para erros de form
- Sucesso: Toast para confirmacao de acoes
- Confirmacao: Modal para acoes destrutivas
- Paginacao: Mostrar total e paginas
- Busca: Debounce de 300ms

**Acessibilidade (WCAG 2.1 AA):**
- Contraste minimo 4.5:1 para texto
- Todos elementos interativos acessiveis por teclado
- Labels em todos inputs
- ARIA labels quando necessario
- Focus visible em todos elementos

**Responsividade:**
- Mobile-first: Breakpoints sm, md, lg, xl
- Tabelas: Scroll horizontal em mobile
- Modais: Full screen em mobile
- Navegacao: Menu hamburguer em mobile

**Rationale:** Consistencia visual melhora UX e reduz decisoes de design.

### XIX. Git Workflow

Controle de versao DEVE seguir fluxo padronizado.

**Branches:**
```
main              # Producao - protegida
├── develop       # Desenvolvimento - base para features
├── feature/*     # Features: feature/123-nome-feature
├── fix/*         # Correcoes: fix/456-descricao-bug
├── refactor/*    # Refatoracoes: refactor/melhorar-x
├── hotfix/*      # Hotfixes para producao: hotfix/corrigir-y
└── release/*     # Releases: release/1.2.0
```

**Conventional Commits:**
```
<tipo>(<escopo>): <descricao>

[corpo opcional]

[rodape opcional]
```

**Tipos de Commit:**
| Tipo | Descricao |
|------|-----------|
| `feat` | Nova funcionalidade |
| `fix` | Correcao de bug |
| `docs` | Documentacao |
| `style` | Formatacao (sem mudanca de logica) |
| `refactor` | Refatoracao (sem mudanca de comportamento) |
| `test` | Adicao/correcao de testes |
| `chore` | Manutencao (deps, configs) |
| `perf` | Melhoria de performance |
| `ci` | Mudancas em CI/CD |

**Exemplos:**
```
feat(averbacoes): adicionar fluxo de aprovacao em lote
fix(margem): corrigir calculo com grupos compartilhados
docs(api): documentar endpoints de simulacao
refactor(auth): extrair validacao de senha para service
```

**Regras:**
- Commits DEVEM ser atomicos (uma mudanca logica por commit)
- Mensagens DEVEM ser em portugues, imperativo (adicionar, corrigir, remover)
- PRs DEVEM ter descricao do que foi feito e como testar
- PRs DEVEM passar em CI antes de review
- Squash merge preferido para manter historico limpo
- Force push PROIBIDO em branches compartilhadas

**Rationale:** Historico de commits limpo facilita debugging e geração de changelogs.

### XX. Dependency Management

Dependencias DEVEM ser gerenciadas de forma controlada e segura.

**Regras:**
- pnpm como package manager (obrigatorio)
- Lockfile (`pnpm-lock.yaml`) DEVE estar no repositorio
- Versoes DEVEM ser fixas ou com range restrito (`^` permitido, `*` proibido)
- Dependencias DEVEM ser auditadas semanalmente (`pnpm audit`)
- Atualizacoes de seguranca DEVEM ser aplicadas em 48h
- Novas dependencias DEVEM ser justificadas (evitar bloat)

**Criterios para Nova Dependencia:**
1. Resolve problema real que nao pode ser resolvido facilmente in-house?
2. E ativamente mantida (commits recentes, issues respondidas)?
3. Tem licenca compativel (MIT, Apache 2.0, BSD)?
4. Tem bundle size aceitavel?
5. Tem tipagem TypeScript (nativa ou @types)?
6. Tem alternativa mais leve que atende?

**Dependencias Proibidas:**
- `moment.js` (usar `dayjs`)
- `lodash` completo (usar imports individuais ou nativo)
- `jquery` (usar APIs nativas ou React)
- Qualquer dependencia com vulnerabilidade critica conhecida

**Monorepo Workspaces:**
- Dependencias compartilhadas na raiz
- Dependencias especificas no package do app
- `workspace:*` para deps internas

**Rationale:** Controle de dependencias reduz vulnerabilidades, bundle size e complexidade.

---

## Technical Standards

### Stack Obrigatoria

| Camada | Tecnologia | Versao Minima |
|--------|------------|---------------|
| Runtime | Node.js | 22 LTS |
| Linguagem | TypeScript | 5.5+ |
| Backend | Fastify + tRPC | 5.0+ / 11+ |
| Frontend | React + Vite | 19 / 5.4+ |
| ORM | Prisma | 5.0+ |
| Database | PostgreSQL | 16 |
| Cache | Redis | 7+ |
| Testes | Vitest + Playwright | 2.0+ / 1.45+ |

### Padroes de Codigo

- ESLint com regras strict DEVE estar configurado
- Prettier DEVE formatar codigo automaticamente
- Commits DEVEM seguir Conventional Commits
- PRs DEVEM passar por code review antes de merge
- Branches DEVEM seguir padrao: `feature/`, `fix/`, `refactor/`

### Seguranca

- HTTPS obrigatorio em todos os ambientes (exceto localhost)
- Senhas DEVEM usar bcrypt com salt rounds >= 12
- JWT access tokens DEVEM expirar em 15 minutos
- Secrets DEVEM estar em variaveis de ambiente, nunca no codigo
- Dependencias DEVEM ser auditadas regularmente (`pnpm audit`)

---

## Development Workflow

### Processo de Desenvolvimento

1. **Especificacao**: Toda feature DEVE ter spec aprovada antes de desenvolvimento
2. **Planejamento**: Features complexas DEVEM ter plan.md com decisoes tecnicas
3. **Implementacao**: Codigo DEVE seguir principios desta constituicao
4. **Code Review**: PRs DEVEM ter aprovacao de pelo menos 1 reviewer
5. **Testes**: Todos os testes DEVEM passar antes de merge
6. **Deploy**: Staging DEVE ser validado antes de producao

### Definition of Done

- [ ] Codigo implementado seguindo principios da constituicao
- [ ] Testes unitarios para logica de negocio
- [ ] Testes de integracao para endpoints
- [ ] Documentacao atualizada (se aplicavel)
- [ ] Code review aprovado
- [ ] CI/CD passando
- [ ] Sem vulnerabilidades criticas de seguranca

### Ambientes

| Ambiente | Proposito | Deploy |
|----------|-----------|--------|
| Development | Desenvolvimento local | Manual |
| Staging | Testes de integracao | Automatico (develop) |
| Production | Producao | Automatico (main) + aprovacao |

---

## Governance

### Autoridade da Constituicao

Esta constituicao SUPERA todas as outras praticas e documentos. Em caso de conflito, a constituicao prevalece.

### Processo de Emenda

1. Proposta de alteracao DEVE ser documentada com justificativa
2. Impacto em codigo existente DEVE ser avaliado
3. Aprovacao requer consenso da equipe tecnica
4. Alteracoes DEVEM ter plano de migracao quando aplicavel
5. Versao DEVE ser incrementada seguindo SemVer:
   - MAJOR: Remocao ou redefinicao de principios
   - MINOR: Adicao de novos principios ou secoes
   - PATCH: Clarificacoes e correcoes textuais

### Verificacao de Compliance

- Code reviews DEVEM verificar aderencia aos principios
- Linting e testes automatizados DEVEM enforcar regras tecnicas
- Auditorias periodicas DEVEM avaliar compliance geral
- Violacoes DEVEM ser corrigidas antes de merge

### Documentos Relacionados

- `decisoes-tecnicas.md`: ADRs e decisoes de stack
- `regras-negocio.md`: Regras de negocio detalhadas
- `arquitetura-tecnica.md`: Arquitetura do sistema
- `permissoes-detalhadas.md`: Sistema de permissoes

---

## Quick Reference

### Checklist de Compliance

**Antes de Commit:**
- [ ] Codigo segue naming conventions (Art. XII)
- [ ] Sem `any` no TypeScript (Art. II)
- [ ] Erros usam classes apropriadas (Art. XV)
- [ ] Commit message segue Conventional Commits (Art. XIX)

**Antes de PR:**
- [ ] Testes passando localmente (Art. IX)
- [ ] Endpoints validam permissoes (Art. V)
- [ ] Inputs validados com Zod (Art. VI)
- [ ] Queries filtram por tenantId (Art. I)

**Antes de Merge:**
- [ ] Code review aprovado
- [ ] CI passando
- [ ] Documentacao atualizada se necessario (Art. XIV)
- [ ] Sem vulnerabilidades de seguranca

### Glossario de Termos

| Termo Portugues | Uso em Codigo | Descricao |
|-----------------|---------------|-----------|
| Averbacao | `averbacao` | Contrato de emprestimo consignado |
| Funcionario | `funcionario` | Servidor publico tomador do emprestimo |
| Consignataria | `consignataria` | Banco/financeira que concede emprestimo |
| Consignante | `consignante` | Orgao empregador que processa desconto |
| Margem | `margem` | Valor disponivel para consignacao |
| Parcela | `parcela` | Prestacao mensal |
| Competencia | `competencia` | Mes/ano de referencia (AAAA/MM) |
| Tramitacao | `tramitacao` | Mudanca de estado da averbacao |

**Version**: 1.1.0 | **Ratified**: 2026-01-10 | **Last Amended**: 2026-01-10
