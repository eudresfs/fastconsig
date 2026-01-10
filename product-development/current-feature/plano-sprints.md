# Plano de Sprints - FastConsig

**Versao:** 1.0
**Data:** Janeiro 2026
**Status:** Draft

---

## 1. Visao Geral do Roadmap

### 1.1 Fases do Projeto

| Fase | Descricao | Sprints |
|------|-----------|---------|
| **Fase 0: Setup** | Configuracao de ambiente, CI/CD, estrutura base | Sprint 0 |
| **Fase 1: Fundacao** | Autenticacao, usuarios, configuracoes basicas | Sprints 1-2 |
| **Fase 2: Core Funcionarios** | Gestao de funcionarios e margem | Sprints 3-4 |
| **Fase 3: Core Averbacoes** | Averbacoes e fluxo de aprovacao | Sprints 5-7 |
| **Fase 4: Simulacoes e Consignatarias** | Simulador e gestao de conveniados | Sprints 8-9 |
| **Fase 5: Conciliacao e Integracao** | Conciliacao mensal e import/export | Sprints 10-11 |
| **Fase 6: Relatorios e Dashboards** | Visualizacao de dados e KPIs | Sprints 12-13 |
| **Fase 7: Finalizacao MVP** | Auditoria, notificacoes, polimento | Sprints 14-15 |
| **Fase Enterprise** | Recursos avancados pos-MVP | Sprints E1-E3 |

### 1.2 Marcos Principais

| Marco | Descricao | Sprint |
|-------|-----------|--------|
| **M0: Ambiente Pronto** | CI/CD funcional, estrutura de projeto definida | Sprint 0 |
| **M1: Login Funcional** | Usuarios podem autenticar e acessar o sistema | Sprint 2 |
| **M2: Gestao de Funcionarios** | CRUD completo de funcionarios com margem | Sprint 4 |
| **M3: Fluxo de Averbacao** | Ciclo completo: criar -> aprovar -> enviar | Sprint 7 |
| **M4: Simulador Operacional** | Simulacoes de emprestimo funcionando | Sprint 9 |
| **M5: Conciliacao Completa** | Processo de conciliacao mensal implementado | Sprint 11 |
| **M6: Dashboards e Relatorios** | Visualizacoes gerenciais disponiveis | Sprint 13 |
| **M7: MVP Release** | Sistema pronto para producao | Sprint 15 |
| **ME: Enterprise Release** | Recursos avancados disponiveis | Sprint E3 |

### 1.3 Criterios de Release

#### Release MVP (apos Sprint 15)

- [ ] Todas as 68 user stories MVP implementadas e testadas
- [ ] Cobertura de testes >= 80%
- [ ] Zero bugs criticos ou bloqueantes
- [ ] Performance: tempo de resposta < 500ms para operacoes comuns
- [ ] Documentacao de usuario completa
- [ ] Treinamento de equipe de suporte realizado
- [ ] Ambiente de producao configurado e validado
- [ ] Backup e disaster recovery testados
- [ ] Penetration testing realizado (vulnerabilidades criticas corrigidas)
- [ ] LGPD compliance verificado

#### Release Enterprise (apos Sprint E3)

- [ ] Todas as 4 user stories Enterprise implementadas
- [ ] SSO integrado com pelo menos um provedor
- [ ] 2FA testado e documentado
- [ ] Webhooks validados com parceiros piloto
- [ ] SLA de 99.9% atingivel

---

## 2. Estimativas

### 2.1 Story Points por Epico

| Epico | Stories | Complexidade Media | Story Points Estimados |
|-------|---------|-------------------|------------------------|
| EP-01: Autenticacao e Seguranca | 6 MVP | Media | 35 |
| EP-02: Gestao de Funcionarios | 8 | Media-Alta | 55 |
| EP-03: Gestao de Margem | 4 | Media | 25 |
| EP-04: Averbacoes | 8 | Alta | 65 |
| EP-05: Fluxo de Aprovacao | 5 | Media | 30 |
| EP-06: Simulacoes | 5 | Media | 30 |
| EP-07: Conciliacao | 5 | Alta | 40 |
| EP-08: Import/Export | 3 MVP | Media-Alta | 30 |
| EP-09: Consignatarias e Produtos | 4 | Media | 25 |
| EP-10: Relatorios e Dashboards | 5 | Media | 35 |
| EP-11: Usuarios e Permissoes | 4 | Media | 25 |
| EP-12: Configuracoes | 3 MVP | Baixa-Media | 15 |
| EP-13: Auditoria | 3 | Media | 20 |
| EP-14: Notificacoes | 5 | Media | 30 |
| **TOTAL MVP** | **68** | - | **~460 pontos** |

### 2.2 Velocity Estimada

| Parametro | Valor |
|-----------|-------|
| **Velocity por Sprint** | 40-50 pontos |
| **Sprints para MVP** | 15 sprints (+ Sprint 0) |
| **Duracao de cada Sprint** | 2 semanas |
| **Buffer de contingencia** | ~10% incluido nas estimativas |

### 2.3 Composicao da Equipe

| Papel | Quantidade | Responsabilidades |
|-------|------------|-------------------|
| Tech Lead | 1 | Arquitetura, code review, decisoes tecnicas |
| Backend Developer | 2 | APIs, regras de negocio, integracao |
| Frontend Developer | 2 | Interface, UX, componentes |
| QA Engineer | 1 | Testes, automacao, qualidade |

---

## 3. Plano de Sprints MVP

---

### Sprint 0: Setup e Fundacao Tecnica

**Objetivo:** Preparar toda a infraestrutura e ambiente de desenvolvimento

**Capacidade:** Sprint dedicada a setup (nao conta story points)

#### Entregas

| Item | Responsavel | Criterio de Aceite |
|------|-------------|-------------------|
| Repositorio Git configurado | Tech Lead | Branches, protecao, hooks |
| Pipeline CI/CD | Tech Lead + Backend | Build, test, deploy automatico |
| Ambiente de desenvolvimento | Backend | Docker compose funcional |
| Ambiente de staging | Tech Lead | Deploy automatico funcionando |
| Estrutura do projeto backend | Backend | Arquitetura limpa implementada |
| Estrutura do projeto frontend | Frontend | React/Vue/Angular com Tailwind |
| Design System inicial | Frontend | Cores, tipografia, componentes base |
| Banco de dados modelagem inicial | Backend | Entidades core criadas |
| Documentacao de arquitetura | Tech Lead | ADRs iniciais |
| Configuracao de testes | QA | Jest/Vitest + Cypress/Playwright |

#### Definition of Done Sprint 0

- [ ] `npm run dev` / `dotnet run` funciona localmente
- [ ] Pipeline executa build e testes
- [ ] Deploy para staging automatico
- [ ] Todos os desenvolvedores com ambiente funcional

---

### Sprint 1: Autenticacao - Parte 1

**Objetivo:** Implementar autenticacao basica e estrutura de usuarios

**Epicos:** EP-01 (parcial), EP-11 (parcial)

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-001 | Login no Sistema | 8 | Backend + Frontend |
| US-002 | Bloqueio por Tentativas Invalidas | 5 | Backend |
| US-004 | Logout do Sistema | 3 | Backend + Frontend |
| US-057 | Cadastrar Usuario | 8 | Backend + Frontend |
| - | Infraestrutura JWT e Refresh Token | 8 | Backend |
| - | Layout Master (Header, Sidebar, Footer) | 8 | Frontend |
| - | Tela de Login | 5 | Frontend |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Implementar middleware de autenticacao JWT
- Criar entidade Usuario no banco
- Implementar rate limiting para login
- Criar componentes de formulario reutilizaveis
- Implementar roteamento protegido no frontend

---

### Sprint 2: Autenticacao - Parte 2 e Usuarios

**Objetivo:** Completar autenticacao e gestao basica de usuarios

**Epicos:** EP-01 (conclusao MVP), EP-11 (parcial), EP-12 (parcial)

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-003 | Recuperacao de Senha | 8 | Backend + Frontend |
| US-005 | Alterar Minha Senha | 5 | Backend + Frontend |
| US-006 | Primeiro Acesso com Troca Obrigatoria | 5 | Backend + Frontend |
| US-058 | Gerenciar Perfis de Acesso | 8 | Backend + Frontend |
| US-059 | Inativar Usuario | 3 | Backend + Frontend |
| US-060 | Resetar Senha de Usuario | 3 | Backend |
| US-062 | Configurar Dados do Orgao | 5 | Backend + Frontend |
| US-063 | Configurar Email (SMTP) | 5 | Backend + Frontend |
| - | Servico de envio de email | 3 | Backend |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Implementar sistema de permissoes granulares
- Criar templates de email
- Implementar validacao de complexidade de senha
- Criar tela de configuracoes do sistema

#### Marco M1: Login Funcional

---

### Sprint 3: Gestao de Funcionarios - Parte 1

**Objetivo:** Implementar cadastro e busca de funcionarios

**Epicos:** EP-02 (parcial)

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-009 | Cadastrar Funcionario | 13 | Backend + Frontend |
| US-010 | Editar Funcionario | 8 | Backend + Frontend |
| US-011 | Buscar Funcionario | 8 | Backend + Frontend |
| US-012 | Visualizar Ficha do Funcionario | 8 | Backend + Frontend |
| - | Componente de busca avancada | 5 | Frontend |
| - | Componente de listagem com paginacao | 3 | Frontend |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Modelar entidade Funcionario completa
- Implementar validacao de CPF
- Criar mascara de campos (CPF, telefone, etc)
- Implementar busca full-text

---

### Sprint 4: Gestao de Funcionarios - Parte 2 e Margem

**Objetivo:** Completar gestao de funcionarios e implementar calculo de margem

**Epicos:** EP-02 (conclusao), EP-03

**Capacidade:** 50 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-013 | Alterar Situacao do Funcionario | 5 | Backend + Frontend |
| US-014 | Aposentar Funcionario | 8 | Backend + Frontend |
| US-015 | Visualizar Historico do Funcionario | 5 | Backend + Frontend |
| US-016 | Gerenciar Autorizacoes de Desconto | 5 | Backend + Frontend |
| US-017 | Consultar Margem Disponivel | 8 | Backend + Frontend |
| US-018 | Visualizar Composicao da Margem | 5 | Backend + Frontend |
| US-019 | Simular Margem Futura | 5 | Backend + Frontend |
| US-020 | Exportar Margem | 5 | Backend + Frontend |
| - | Motor de calculo de margem | 4 | Backend |

**Total:** 50 pontos

#### Tarefas Tecnicas

- Implementar engine de calculo de margem
- Criar sistema de historico (audit trail)
- Implementar geracao de arquivos (Excel, CSV)

#### Marco M2: Gestao de Funcionarios Completa

---

### Sprint 5: Averbacoes - Parte 1

**Objetivo:** Implementar criacao e visualizacao de averbacoes

**Epicos:** EP-04 (parcial)

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-021 | Criar Nova Averbacao | 13 | Backend + Frontend |
| US-022 | Criar Refinanciamento | 8 | Backend + Frontend |
| US-024 | Visualizar Averbacao | 8 | Backend + Frontend |
| US-028 | Gerar Termo de Averbacao | 8 | Backend + Frontend |
| - | Modelagem de Averbacao e relacionamentos | 5 | Backend |
| - | Componente de wizard multi-step | 3 | Frontend |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Modelar entidade Averbacao com todos os status
- Implementar reserva de margem
- Criar geracao de PDF para termo
- Implementar vinculacao de contratos

---

### Sprint 6: Averbacoes - Parte 2

**Objetivo:** Implementar operacoes sobre averbacoes e portabilidade

**Epicos:** EP-04 (continuacao)

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-023 | Criar Compra de Divida | 13 | Backend + Frontend |
| US-025 | Cancelar Averbacao | 5 | Backend + Frontend |
| US-026 | Suspender Averbacao | 5 | Backend + Frontend |
| US-027 | Liquidar Averbacao | 5 | Backend + Frontend |
| - | Fluxo de portabilidade completo | 8 | Backend |
| - | Componente de timeline de status | 5 | Frontend |
| - | Notificacoes de mudanca de status | 4 | Backend |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Implementar maquina de estados para averbacao
- Criar sistema de notificacoes internas
- Implementar liberacao automatica de margem

---

### Sprint 7: Fluxo de Aprovacao

**Objetivo:** Implementar workflow de aprovacao de averbacoes

**Epicos:** EP-05

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-029 | Listar Averbacoes Pendentes | 5 | Backend + Frontend |
| US-030 | Aprovar Averbacao | 8 | Backend + Frontend |
| US-031 | Rejeitar Averbacao | 5 | Backend + Frontend |
| US-032 | Aprovar em Lote | 8 | Backend + Frontend |
| US-033 | Solicitar Informacoes Adicionais | 5 | Backend + Frontend |
| - | Dashboard de aprovacoes | 5 | Frontend |
| - | Notificacoes por email de aprovacao | 5 | Backend |
| - | Testes E2E do fluxo de averbacao | 4 | QA |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Implementar permissoes de aprovador
- Criar sistema de lote com transacao
- Implementar workflow de pedido de informacao

#### Marco M3: Fluxo de Averbacao Completo

---

### Sprint 8: Simulacoes

**Objetivo:** Implementar simulador de emprestimos

**Epicos:** EP-06

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-034 | Simular Emprestimo por Valor | 8 | Backend + Frontend |
| US-035 | Simular Emprestimo por Parcela | 5 | Backend + Frontend |
| US-036 | Simular Compra de Divida | 8 | Backend + Frontend |
| US-037 | Gerenciar Tabelas de Coeficientes | 8 | Backend + Frontend |
| US-038 | Imprimir Simulacao | 5 | Backend + Frontend |
| - | Motor de calculo financeiro | 8 | Backend |
| - | Interface interativa do simulador | 3 | Frontend |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Implementar calculos financeiros (Price, SAC)
- Criar calculo de CET e IOF
- Implementar importacao de coeficientes via Excel

---

### Sprint 9: Consignatarias e Produtos

**Objetivo:** Implementar gestao de consignatarias e seus produtos

**Epicos:** EP-09

**Capacidade:** 40 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-048 | Cadastrar Consignataria | 8 | Backend + Frontend |
| US-049 | Gerenciar Produtos | 8 | Backend + Frontend |
| US-050 | Suspender Consignataria | 5 | Backend + Frontend |
| US-051 | Gerenciar Agentes | 8 | Backend + Frontend |
| - | Relatorio de producao por agente | 5 | Backend + Frontend |
| - | Testes de integracao simulador + consignataria | 6 | QA |

**Total:** 40 pontos

#### Tarefas Tecnicas

- Modelar entidades Consignataria, Produto, Agente
- Implementar vinculacao usuario-consignataria
- Criar restricoes de operacao por status

#### Marco M4: Simulador Operacional

---

### Sprint 10: Conciliacao

**Objetivo:** Implementar processo de conciliacao mensal

**Epicos:** EP-07

**Capacidade:** 50 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-039 | Importar Retorno da Folha | 13 | Backend + Frontend |
| US-040 | Executar Conciliacao | 13 | Backend + Frontend |
| US-041 | Tratar Divergencias | 8 | Backend + Frontend |
| US-042 | Fechar Competencia | 5 | Backend + Frontend |
| US-043 | Reabrir Competencia | 5 | Backend + Frontend |
| - | Parser de arquivos de retorno | 6 | Backend |

**Total:** 50 pontos

#### Tarefas Tecnicas

- Implementar engine de conciliacao
- Criar parser de multiplos layouts
- Implementar bloqueio de competencia

---

### Sprint 11: Importacao e Exportacao

**Objetivo:** Implementar funcionalidades de integracao via arquivos

**Epicos:** EP-08 (MVP)

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-044 | Importar Funcionarios | 13 | Backend + Frontend |
| US-045 | Importar Contratos | 13 | Backend + Frontend |
| US-046 | Exportar Remessa para Folha | 8 | Backend + Frontend |
| - | Sistema de validacao de importacao | 5 | Backend |
| - | Processamento em background | 6 | Backend |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Implementar sistema de filas para processamento
- Criar validacao previa com relatorio de erros
- Implementar importacao parcial

#### Marco M5: Conciliacao Completa

---

### Sprint 12: Relatorios

**Objetivo:** Implementar relatorios gerenciais

**Epicos:** EP-10 (parcial)

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-054 | Gerar Relatorio de Producao | 8 | Backend + Frontend |
| US-055 | Gerar Relatorio de Inadimplencia | 8 | Backend + Frontend |
| US-056 | Gerar Relatorio de Auditoria | 8 | Backend + Frontend |
| - | Componente de filtros de relatorio | 5 | Frontend |
| - | Exportacao Excel/PDF | 8 | Backend |
| - | Visualizacao em tela com paginacao | 5 | Frontend |
| - | Testes de relatorios | 3 | QA |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Implementar geracao de relatorios otimizada
- Criar templates de exportacao
- Implementar cache de relatorios

---

### Sprint 13: Dashboards

**Objetivo:** Implementar dashboards gerenciais

**Epicos:** EP-10 (conclusao)

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-052 | Visualizar Dashboard do Consignante | 13 | Backend + Frontend |
| US-053 | Visualizar Dashboard da Consignataria | 13 | Backend + Frontend |
| - | Componentes de graficos (barras, pizza, linha) | 8 | Frontend |
| - | KPIs em tempo real | 5 | Backend |
| - | Filtros de periodo | 3 | Frontend |
| - | Export de dashboard | 3 | Frontend |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Implementar agregacoes para KPIs
- Criar cache de metricas
- Implementar graficos interativos

#### Marco M6: Dashboards e Relatorios

---

### Sprint 14: Auditoria e Notificacoes

**Objetivo:** Implementar auditoria completa e sistema de notificacoes

**Epicos:** EP-13, EP-14 (parcial)

**Capacidade:** 45 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-065 | Consultar Log de Auditoria | 8 | Backend + Frontend |
| US-066 | Consultar Log de Acessos | 5 | Backend + Frontend |
| US-067 | Gerenciar Sessoes Ativas | 5 | Backend + Frontend |
| US-068 | Receber Notificacao de Averbacao Pendente | 5 | Backend |
| US-069 | Receber Notificacao de Aprovacao/Rejeicao | 5 | Backend |
| US-070 | Enviar Mensagem Interna | 5 | Backend + Frontend |
| US-071 | Visualizar Central de Mensagens | 5 | Backend + Frontend |
| US-072 | Configurar Preferencias de Notificacao | 4 | Backend + Frontend |
| - | Infraestrutura de auditoria | 3 | Backend |

**Total:** 45 pontos

#### Tarefas Tecnicas

- Implementar interceptors de auditoria
- Criar sistema de mensageria interna
- Implementar preferencias de usuario

---

### Sprint 15: Polimento e Preparacao para Release

**Objetivo:** Estabilizar sistema, corrigir bugs, preparar documentacao

**Epicos:** Todos (refinamento)

**Capacidade:** 40 pontos (foco em qualidade)

#### Atividades

| Atividade | Responsavel | Pontos |
|-----------|-------------|--------|
| US-061: Configurar Parametros de Margem | Backend + Frontend | 8 |
| Bug fixes priorizados | Todos | 10 |
| Testes E2E completos | QA | 8 |
| Testes de performance | QA + Backend | 5 |
| Documentacao de usuario | Tech Lead | 5 |
| Treinamento interno | Tech Lead | 2 |
| Preparacao de ambiente de producao | Tech Lead + Backend | 2 |

**Total:** 40 pontos

#### Criterios de Saida do Sprint 15

- [ ] Zero bugs P0/P1 abertos
- [ ] Todos os testes E2E passando
- [ ] Performance validada
- [ ] Documentacao completa
- [ ] Ambiente de producao pronto

#### Marco M7: MVP Release

---

## 4. Dependencias entre Sprints

### 4.1 Diagrama de Dependencias

```
Sprint 0 (Setup)
    |
    v
Sprint 1 (Auth Parte 1) ─────────────────────────────────────┐
    |                                                         |
    v                                                         |
Sprint 2 (Auth Parte 2 + Usuarios) ──────────────────────────┤
    |                                                         |
    v                                                         |
Sprint 3 (Funcionarios Parte 1)                               |
    |                                                         |
    v                                                         |
Sprint 4 (Funcionarios Parte 2 + Margem) ─────┐               |
    |                                          |               |
    v                                          |               |
Sprint 5 (Averbacoes Parte 1) <───────────────┘               |
    |                                                         |
    v                                                         |
Sprint 6 (Averbacoes Parte 2)                                 |
    |                                                         |
    v                                                         |
Sprint 7 (Fluxo Aprovacao) <──────────────────────────────────┘
    |
    +──────────────────────────────┐
    |                              |
    v                              v
Sprint 8 (Simulacoes)         Sprint 10 (Conciliacao)
    |                              |
    v                              v
Sprint 9 (Consignatarias)     Sprint 11 (Import/Export)
    |                              |
    +──────────────────────────────+
                |
                v
         Sprint 12 (Relatorios)
                |
                v
         Sprint 13 (Dashboards)
                |
                v
         Sprint 14 (Auditoria + Notificacoes)
                |
                v
         Sprint 15 (Polimento)
                |
                v
           MVP RELEASE
```

### 4.2 Dependencias Criticas

| Sprint | Depende de | Natureza da Dependencia |
|--------|------------|------------------------|
| Sprint 1-2 | Sprint 0 | Ambiente e estrutura base |
| Sprint 3-4 | Sprint 1-2 | Autenticacao e usuarios |
| Sprint 5-7 | Sprint 4 | Funcionarios e margem |
| Sprint 8-9 | Sprint 7 | Averbacoes e consignatarias |
| Sprint 10-11 | Sprint 7 | Averbacoes para conciliacao |
| Sprint 12-13 | Sprints 9 e 11 | Dados para relatorios |
| Sprint 14 | Sprint 13 | Sistema estavel para auditoria |
| Sprint 15 | Sprint 14 | Todos os modulos implementados |

### 4.3 Sprints Paralelizaveis

Os seguintes grupos de sprints podem ter trabalho paralelo entre backend e frontend:

- **Sprints 8-9 e 10-11:** Backend pode adiantar conciliacao enquanto frontend finaliza simulacoes
- **Sprints 12-13:** Relatorios e dashboards podem ter desenvolvimento paralelo

---

## 5. Riscos e Mitigacoes por Sprint

### 5.1 Riscos por Fase

#### Sprint 0: Setup

| Risco | Probabilidade | Impacto | Mitigacao |
|-------|---------------|---------|-----------|
| Ambiente nao configurado a tempo | Media | Alto | Iniciar antes do sprint oficial |
| Divergencias de stack tecnologico | Baixa | Alto | ADRs definidos previamente |

#### Sprints 1-2: Autenticacao

| Risco | Probabilidade | Impacto | Mitigacao |
|-------|---------------|---------|-----------|
| Complexidade do sistema de permissoes | Media | Medio | Design upfront detalhado |
| Problemas com envio de email | Media | Baixo | Usar servico gerenciado (SendGrid, SES) |

#### Sprints 3-4: Funcionarios

| Risco | Probabilidade | Impacto | Mitigacao |
|-------|---------------|---------|-----------|
| Regras de margem complexas | Alta | Alto | Validar com especialista de dominio |
| Performance em buscas | Media | Medio | Indexacao adequada, full-text search |

#### Sprints 5-7: Averbacoes

| Risco | Probabilidade | Impacto | Mitigacao |
|-------|---------------|---------|-----------|
| Maquina de estados complexa | Alta | Alto | Modelar com cuidado, testes extensivos |
| Integridade de margem | Alta | Critico | Transacoes atomicas, validacao dupla |
| Portabilidade com muitos cenarios | Media | Alto | Casos de teste detalhados |

#### Sprints 8-9: Simulacoes e Consignatarias

| Risco | Probabilidade | Impacto | Mitigacao |
|-------|---------------|---------|-----------|
| Calculos financeiros incorretos | Media | Critico | Validacao com formulas conhecidas |
| Performance do simulador | Baixa | Medio | Cache de coeficientes |

#### Sprints 10-11: Conciliacao

| Risco | Probabilidade | Impacto | Mitigacao |
|-------|---------------|---------|-----------|
| Layouts de arquivo desconhecidos | Alta | Alto | Coletar exemplos reais previamente |
| Volume de dados em importacao | Media | Medio | Processamento em background |

#### Sprints 12-15: Finalizacao

| Risco | Probabilidade | Impacto | Mitigacao |
|-------|---------------|---------|-----------|
| Acumulo de bugs | Alta | Alto | Code review rigoroso, testes |
| Escopo adicional | Media | Alto | Congelar escopo apos Sprint 13 |
| Performance em dashboards | Media | Medio | Agregacoes pre-calculadas |

### 5.2 Matriz Geral de Riscos do Projeto

| Risco | Probabilidade | Impacto | Mitigacao |
|-------|---------------|---------|-----------|
| Falta de especialista de dominio | Alta | Critico | Consultar clientes reais, documentacao legado |
| Requisitos incompletos | Media | Alto | Refinamento continuo com PO |
| Saida de membro da equipe | Media | Alto | Documentacao, pair programming |
| Subestimativa de complexidade | Alta | Alto | Buffer de 10% em cada sprint |
| Dependencias externas (APIs, servicos) | Baixa | Medio | Mocks para desenvolvimento |

---

## 6. Criterios de Done

### 6.1 Definition of Done - Por User Story

Toda user story deve atender aos seguintes criterios antes de ser considerada "Done":

**Desenvolvimento:**
- [ ] Codigo implementado seguindo padroes do projeto
- [ ] Code review aprovado por pelo menos 1 desenvolvedor
- [ ] Sem warnings ou erros de linting
- [ ] Cobertura de testes unitarios >= 80%
- [ ] Testes de integracao para APIs

**Qualidade:**
- [ ] Todos os criterios de aceitacao da story atendidos
- [ ] Testado em ambiente de staging
- [ ] Sem bugs conhecidos de severidade alta ou critica
- [ ] Acessibilidade basica verificada (contraste, navegacao por teclado)

**Documentacao:**
- [ ] API documentada no Swagger/OpenAPI
- [ ] Comentarios em codigo complexo
- [ ] README atualizado se necessario

**Deploy:**
- [ ] Merge na branch principal sem conflitos
- [ ] Deploy automatico para staging executado com sucesso
- [ ] Smoke tests passando

### 6.2 Definition of Done - Por Sprint

Ao final de cada sprint:

**Produto:**
- [ ] Todas as stories planejadas estao "Done" ou movidas para backlog com justificativa
- [ ] Incremento de produto potencialmente entregavel
- [ ] Demo realizada com stakeholders

**Processo:**
- [ ] Sprint Review realizada
- [ ] Sprint Retrospective realizada
- [ ] Backlog do proximo sprint refinado
- [ ] Velocity atualizada

**Qualidade:**
- [ ] Build estavel na branch principal
- [ ] Cobertura de testes geral >= 80%
- [ ] Zero bugs P0 abertos
- [ ] Ambiente de staging funcional

### 6.3 Definition of Done - Por Release

Para release MVP (apos Sprint 15):

**Funcionalidade:**
- [ ] Todas as 68 user stories MVP implementadas
- [ ] Todos os fluxos criticos testados end-to-end
- [ ] Integracao com email funcionando

**Qualidade:**
- [ ] Zero bugs P0 (bloqueantes)
- [ ] Maximo 5 bugs P1 (criticos) com workaround documentado
- [ ] Performance: tempo resposta P95 < 500ms para operacoes comuns
- [ ] Performance: tempo resposta P95 < 5s para relatorios
- [ ] Load test: suporte a 100 usuarios simultaneos

**Seguranca:**
- [ ] Penetration test realizado
- [ ] Vulnerabilidades criticas corrigidas
- [ ] HTTPS configurado
- [ ] Headers de seguranca implementados

**Operacional:**
- [ ] Ambiente de producao configurado
- [ ] Monitoramento e alertas configurados
- [ ] Backup automatico funcionando
- [ ] Runbook de operacoes documentado
- [ ] Plano de rollback definido

**Documentacao:**
- [ ] Manual do usuario disponivel
- [ ] Documentacao de API completa
- [ ] Guia de administracao do sistema
- [ ] FAQ inicial

---

## 7. Plano Enterprise (Pos-MVP)

### 7.1 Visao Geral

Apos a entrega do MVP, as seguintes sprints implementam recursos Enterprise:

| Sprint | Foco | User Stories |
|--------|------|--------------|
| E1 | Seguranca Avancada | US-007, US-008 |
| E2 | Layouts Personalizados | US-047 |
| E3 | Webhooks | US-064 |

### 7.2 Sprint E1: Seguranca Avancada

**Objetivo:** Implementar 2FA e SSO

**Capacidade:** 40 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-007 | Autenticacao de Dois Fatores (2FA) | 13 | Backend + Frontend |
| US-008 | Single Sign-On (SSO) | 21 | Backend |
| - | Integracao com provedores SAML/OIDC | 6 | Backend |

**Total:** 40 pontos

#### Tarefas Tecnicas

- Implementar TOTP (Time-based One-Time Password)
- Criar geracao de QR Code
- Implementar codigos de backup
- Integrar com Azure AD / Google Workspace
- Criar mapeamento de grupos AD para perfis

### 7.3 Sprint E2: Layouts Personalizados

**Objetivo:** Permitir configuracao de layouts de importacao/exportacao

**Capacidade:** 35 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-047 | Gerenciar Layouts Personalizados | 21 | Backend + Frontend |
| - | Designer visual de layout | 8 | Frontend |
| - | Testes com arquivos reais | 6 | QA |

**Total:** 35 pontos

#### Tarefas Tecnicas

- Criar engine de parsing configuravel
- Implementar interface de mapeamento de campos
- Criar validador de layouts

### 7.4 Sprint E3: Webhooks

**Objetivo:** Implementar sistema de webhooks para integracao

**Capacidade:** 35 pontos

#### User Stories

| ID | Story | Pontos | Responsavel |
|----|-------|--------|-------------|
| US-064 | Configurar Webhooks | 21 | Backend + Frontend |
| - | Sistema de retry com backoff | 5 | Backend |
| - | Assinatura HMAC | 3 | Backend |
| - | Dashboard de logs de webhook | 6 | Frontend |

**Total:** 35 pontos

#### Tarefas Tecnicas

- Implementar sistema de eventos
- Criar fila de envio de webhooks
- Implementar retry com exponential backoff
- Criar sistema de logs e debugging

### 7.5 Roadmap Enterprise Futuro

Alem das sprints E1-E3, funcionalidades Enterprise futuras incluem:

| Funcionalidade | Descricao | Estimativa |
|----------------|-----------|------------|
| Aplicativo Mobile | Apps iOS/Android nativos | 4-6 sprints |
| White Label | Customizacao completa de marca | 2-3 sprints |
| Compliance TCE | Relatorios especificos por estado | 2 sprints |
| API Publica | APIs para parceiros externos | 2 sprints |
| Machine Learning | Deteccao de fraudes, scoring | 3-4 sprints |

---

## 8. Metricas de Acompanhamento

### 8.1 Metricas de Sprint

#### Burndown Chart

Acompanhar diariamente:
- Story points restantes vs. planejados
- Tendencia de conclusao
- Identificar bloqueios cedo

**Meta:** Linha de burndown consistente, sem picos de entrega no final

#### Velocity

Medir ao final de cada sprint:
- Story points entregues
- Media movel das ultimas 3 sprints
- Variacao percentual

**Meta:** Velocity estavel entre 40-50 pontos apos Sprint 3

#### Metricas de Qualidade por Sprint

| Metrica | Meta | Alerta |
|---------|------|--------|
| Bugs encontrados em code review | < 2/story | > 4/story |
| Bugs encontrados em QA | < 1/story | > 3/story |
| Cobertura de testes | >= 80% | < 70% |
| Tempo de code review | < 4 horas | > 24 horas |

### 8.2 Metricas de Projeto

#### Progresso Geral

| Metrica | Calculo |
|---------|---------|
| % Stories Completas | Stories done / Total stories |
| % Pontos Entregues | Pontos done / Total pontos estimados |
| Sprints Restantes | Pontos restantes / Velocity media |

#### Bug Rate

| Metrica | Meta MVP | Meta Enterprise |
|---------|----------|-----------------|
| Bugs por Sprint | < 10 | < 5 |
| Bugs P0/P1 por Sprint | < 2 | 0 |
| Tempo medio de correcao P0 | < 4 horas | < 2 horas |
| Tempo medio de correcao P1 | < 24 horas | < 8 horas |

#### Technical Debt

| Metrica | Meta |
|---------|------|
| Itens de debt criados por sprint | < 3 |
| Itens de debt resolvidos por sprint | >= 2 |
| Debt acumulado | < 20 itens |

### 8.3 Cerimonias e Cadencia

| Cerimonia | Frequencia | Duracao | Participantes |
|-----------|------------|---------|---------------|
| Daily Standup | Diaria | 15 min | Equipe dev |
| Sprint Planning | Inicio de sprint | 2-4 horas | Todos |
| Sprint Review | Fim de sprint | 1-2 horas | Todos + stakeholders |
| Sprint Retrospective | Fim de sprint | 1-2 horas | Equipe dev |
| Backlog Refinement | Meio de sprint | 1-2 horas | PO + Tech Lead + 1 dev |
| Tech Sync | Semanal | 30 min | Tech Lead + devs |

### 8.4 Ferramentas Sugeridas

| Proposito | Ferramenta |
|-----------|------------|
| Gestao de Sprints | Jira / Azure DevOps / Linear |
| Burndown/Velocity | Integrado na ferramenta de gestao |
| Qualidade de Codigo | SonarQube / CodeClimate |
| Cobertura de Testes | Integrado no CI (Jest/xUnit coverage) |
| Monitoramento | Datadog / New Relic / Grafana |

---

## 9. Resumo Executivo

### 9.1 Numeros Chave

| Item | Valor |
|------|-------|
| Total de User Stories MVP | 68 |
| Total de User Stories Enterprise | 4 |
| Total de Story Points MVP | ~460 |
| Sprints para MVP | 16 (Sprint 0 + Sprints 1-15) |
| Velocity estimada | 40-50 pontos/sprint |
| Equipe | 6 pessoas (1 TL, 2 BE, 2 FE, 1 QA) |

### 9.2 Marcos Resumidos

| Marco | Sprint | Descricao |
|-------|--------|-----------|
| M0 | 0 | Ambiente pronto |
| M1 | 2 | Login funcional |
| M2 | 4 | Gestao de funcionarios completa |
| M3 | 7 | Fluxo de averbacao completo |
| M4 | 9 | Simulador operacional |
| M5 | 11 | Conciliacao completa |
| M6 | 13 | Dashboards e relatorios |
| M7 | 15 | MVP Release |
| ME | E3 | Enterprise Release |

### 9.3 Fatores Criticos de Sucesso

1. **Comprometimento da equipe:** Velocidade constante e previsivel
2. **Acesso a especialista de dominio:** Validar regras de negocio complexas
3. **Infraestrutura estavel:** CI/CD funcionando desde o Sprint 0
4. **Comunicacao clara:** Stakeholders informados do progresso
5. **Qualidade desde o inicio:** Testes como parte do desenvolvimento

---

## 10. Historico de Revisoes

| Versao | Data | Autor | Descricao |
|--------|------|-------|-----------|
| 1.0 | Janeiro 2026 | Product Team | Versao inicial do plano de sprints |

---

*Este documento deve ser revisado e atualizado ao final de cada sprint para refletir a realidade do projeto.*
