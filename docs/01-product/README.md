# Produto

Documentação de produto, visão, PRD e requisitos do FastConsig.

## Documentos

### [prd.md](./prd.md) - Product Requirements Document

**Conteúdo:** PRD completo com requisitos funcionais e não-funcionais

Documento consolidado (merge de 2 versões) contendo:

**Seções principais:**
1. **Problema e Contexto** - Mercado de consignados, dores atuais
2. **Usuários e Personas** - Consignantes, Consignatárias, Operadores
3. **Requisitos Funcionais** - FR-001 a FR-029 (tabela completa)
4. **Requisitos Não-Funcionais** - Performance, segurança, escalabilidade
5. **Integrações** - Folha, CNAB, Webhooks
6. **Escopo MVP vs Enterprise** - Fases de entrega
7. **User Experience** - Princípios de design, fluxos principais
8. **Métricas de Sucesso** - KPIs e objetivos mensuráveis
9. **Considerações Técnicas** - Stack, arquitetura (visão alto nível)
10. **Riscos e Mitigações** - Identificação proativa
11. **Roadmap Sugerido** - Fases e milestones
12. **Glossário** - Termos do domínio
13. **Anexos** - Referências a docs técnicos
14. **✨ Padrões de Inovação** - Margem Viva, Banking ACL, API-First, Push Intelligence
15. **✨ User Journeys Detalhadas** - 3 jornadas narrativas (RH, Banco, Agente)

**Formato:** 1080+ linhas, tabelas estruturadas, referências cruzadas

---

## Visão do Produto

> **Ser a plataforma de referência para gestão de consignados no setor público brasileiro, oferecendo transparência, eficiência e conformidade regulatória através de uma experiência digital moderna.**

### Problema que Resolvemos

**Órgãos públicos hoje:**
- ❌ Usam planilhas Excel para controle
- ❌ Não têm visibilidade em tempo real das margens
- ❌ Processos manuais de aprovação sujeitos a erros
- ❌ Dificuldades na conciliação mensal
- ❌ Não conseguem auditar adequadamente

**FastConsig oferece:**
- ✅ Plataforma unificada
- ✅ Cálculo automático de margem em tempo real
- ✅ Fluxos de aprovação configuráveis
- ✅ Integração com folha de pagamento
- ✅ Dashboards e relatórios gerenciais
- ✅ Trilha de auditoria completa

---

## Personas Principais

### 1. Gestor de RH (Consignante)

**Quem é:** Servidor público responsável pela folha de pagamento

**Dores:**
- Fechamento da folha leva dias
- Erros de centavos causam retrabalho
- Falta de visibilidade das averbações pendentes
- Dependência de múltiplos sistemas

**Jobs to be Done:**
- Aprovar averbações com confiança
- Conciliar folha em minutos, não dias
- Gerar relatórios para TCE/CGU
- Auditar todas as operações

---

### 2. Operador Bancário (Consignatária)

**Quem é:** Funcionário de banco/financeira que opera empréstimos

**Dores:**
- "Queda de reserva" - margem some na hora de efetivar
- Falta de visibilidade do status de propostas
- Processos manuais de averbação
- Dependência de suporte do órgão

**Jobs to be Done:**
- Consultar margem com garantia de disponibilidade
- Averbar contratos rapidamente
- Acompanhar status em tempo real
- Simular empréstimos antes de ofertar

---

### 3. Agente/Correspondente

**Quem é:** Intermediário que vende empréstimos para bancos

**Dores:**
- Zero visibilidade do status das propostas
- Dependência de ligações para saber andamento
- Cliente pergunta "quando vai cair?" e ele não sabe

**Jobs to be Done:**
- Acompanhar propostas em tempo real
- Notificar cliente proativamente
- Integrar com seu CRM via webhooks

---

## Requisitos Essenciais

### Funcionais (Top 10)

| ID | Requisito | Prioridade |
|----|-----------|------------|
| FR-01 | Login multi-tenant (SSO + Email/Senha) com MFA | 🔴 Crítico |
| FR-05 | CRUD de funcionários + import CSV/Excel | 🔴 Crítico |
| FR-06 | Cálculo automático de margem disponível | 🔴 Crítico |
| FR-09 | Geração de Token de Reserva (TTL configurável) | 🔴 Crítico |
| FR-10 | Averbação de contrato com Token | 🔴 Crítico |
| FR-11 | Validação: parcelas ≤ margem disponível | 🔴 Crítico |
| FR-15 | Log imutável de alterações de margem | 🟡 Alto |
| FR-19 | Geração de extrato para folha | 🟡 Alto |
| FR-20 | Destacar divergências na conciliação | 🟡 Alto |
| FR-26 | Parametrização de regras por tenant | 🟡 Alto |

Ver todos os requisitos em [prd.md](./prd.md#3-requisitos-funcionais)

### Não-Funcionais (Top 5)

| ID | Requisito | Meta |
|----|-----------|------|
| NFR-01 | Latência de consulta de margem | < 200ms (p95) |
| NFR-02 | Processamento de arquivo (10k linhas) | < 10 minutos |
| NFR-03 | Disponibilidade | 99.9% (8h-18h) |
| NFR-06 | Segurança | AES-256 repouso + TLS 1.3 |
| NFR-08 | Concorrência | 1.000 usuários simultâneos |

---

## Padrões de Inovação 🚀

### 1. Margem Viva (Event-Driven Core)

**Problema legado:** Margem calculada sob demanda → "falsa disponibilidade"
**Solução:** Margem atualizada em tempo real por eventos

```
Evento: LoanCreated → Handler: UpdateMargin → Margem sempre correta
```

**Benefício:** Zero "queda de reserva", feedback instantâneo

---

### 2. Banking Anti-Corruption Layer (ACL)

**Problema legado:** Lógica de negócio contaminada com parsing de CNAB
**Solução:** Adapters isolam layouts bancários

```
CNAB (Banco X) → Adapter → Domain Entity (Loan)
```

**Benefício:** Adicionar 50 bancos sem contaminar domínio

---

### 3. API-First (Onipresença)

**Problema legado:** Backend acoplado ao frontend WebForms
**Solução:** Backend como Plataforma (PaaS)

```
Portal Web
    ↓
  tRPC API ← Mobile App (futuro)
    ↓
 Backend   ← WhatsApp Bot (futuro)
```

**Benefício:** Time-to-market para novos canais = zero

---

### 4. Push Intelligence (Observabilidade de Negócio)

**Problema legado:** Usuário precisa gerar relatório para descobrir erro
**Solução:** Sistema notifica proativamente

```
Margem Negativa Detectada → Push Notification → Gestor RH age imediatamente
```

**Benefício:** RH vira "Resolvedor" ao invés de "Caçador de Erros"

---

## User Journeys Detalhadas

### Journey 1: Fechamento da Folha Sem Stress

**Persona:** Gestor de RH
**Antes:** 2 dias de trabalho manual
**Depois:** 15 minutos automatizados

Ver narrativa completa em [prd.md](./prd.md#15-user-journeys-detalhadas)

### Journey 2: A Venda Garantida

**Persona:** Operador Bancário
**Antes:** 30% de "queda de reserva"
**Depois:** 100% de aprovação técnica

### Journey 3: Transparência na Ponta

**Persona:** Agente/Correspondente
**Antes:** Dependência de suporte telefônico
**Depois:** Status em tempo real via webhook

---

## Métricas de Sucesso

### Usuários

| Métrica | Baseline | Meta (6 meses) |
|---------|----------|----------------|
| **Tempo de fechamento de folha** | 2 dias | < 2 horas |
| **Taxa de erro em margem** | 5% | < 0.1% |
| **Satisfação (NPS)** | - | > 50 |

### Negócio

| Métrica | Meta |
|---------|------|
| **Redução de tickets de suporte** | 80% |
| **Onboarding de novo tenant** | < 1 dia (sem dev) |
| **Uptime** | 99.9% |

### Técnica

| Métrica | Meta |
|---------|------|
| **Latência API (p95)** | < 200ms |
| **Cobertura de testes** | > 80% |
| **Deploy frequency** | 1x/semana |

---

## Escopo MVP vs Enterprise

### MVP (3-4 meses)

**Core Features:**
- ✅ Autenticação multi-tenant
- ✅ Gestão de funcionários (CRUD + import)
- ✅ Cálculo de margem automático
- ✅ Ciclo de averbação completo
- ✅ Aprovações simples
- ✅ Portal web administrativo

**Objetivo:** Provar valor, 1-2 pilotos

---

### Growth (6-9 meses)

**Expansão:**
- ✨ Margem Viva (tempo real)
- ✨ Webhooks para parceiros
- ✨ Simulador público
- ✨ Mobile app

**Objetivo:** Escalar para 10+ tenants

---

### Enterprise (12+ meses)

**Recursos Avançados:**
- 🔐 2FA, SSO empresarial
- 📱 Apps nativos iOS/Android
- 🔗 Integrações avançadas
- 🎨 White-label customização
- 📊 Analytics avançado

**Objetivo:** Dominar mercado

---

## Relacionados

- **Planejamento:** [../02-planning/user-stories.md](../02-planning/user-stories.md)
- **Arquitetura:** [../03-architecture/decisoes-tecnicas.md](../03-architecture/decisoes-tecnicas.md)
- **Regras de Negócio:** [../05-business-rules/regras-negocio.md](../05-business-rules/regras-negocio.md)
- **UX:** [../04-ux-design/wireframes.md](../04-ux-design/wireframes.md)

---

*Última atualização: Janeiro 2026*
