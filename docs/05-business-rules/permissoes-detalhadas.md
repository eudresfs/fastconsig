# Permissoes de Acesso - FastConsig

**Versao:** 1.0
**Data:** Janeiro 2026
**Status:** Draft

---

## 1. Visao Geral do Sistema de Permissoes

O sistema FastConsig implementa um modelo de controle de acesso baseado em perfis (RBAC - Role-Based Access Control) com permissoes granulares por funcionalidade.

### 1.1 Conceitos Fundamentais

| Conceito | Descricao |
|----------|-----------|
| **Usuario** | Pessoa fisica com acesso ao sistema |
| **Perfil** | Conjunto de permissoes agrupadas |
| **Permissao** | Direito de executar uma acao especifica |
| **Tenant** | Organizacao/cliente isolado no sistema |
| **Escopo** | Abrangencia da permissao (proprio, consignataria, todos) |

### 1.2 Hierarquia de Acesso

```
Tenant (Consignante)
    |
    +-- Usuarios Consignante
    |       |-- Administrador
    |       |-- Operador
    |       |-- Aprovador
    |       +-- Consulta
    |
    +-- Consignatarias Conveniadas
            |
            +-- Usuarios Consignataria
                    |-- Administrador
                    |-- Operador
                    +-- Agente
```

---

## 2. Tipos de Usuario

### 2.1 Usuarios do Consignante

| Tipo | Descricao | Responsabilidades |
|------|-----------|-------------------|
| **Administrador Consignante** | Gestor do orgao publico | Configuracao total, aprovacoes, relatorios gerenciais |
| **Operador Consignante** | Funcionario do RH/DP | Cadastros, importacoes, operacoes do dia-a-dia |
| **Aprovador** | Responsavel por aprovacoes | Analise e aprovacao/rejeicao de averbacoes |
| **Consulta Consignante** | Auditor/Fiscal | Visualizacao de dados e relatorios (somente leitura) |

### 2.2 Usuarios da Consignataria

| Tipo | Descricao | Responsabilidades |
|------|-----------|-------------------|
| **Administrador Consignataria** | Gestor da instituicao | Gerencia usuarios, agentes, relatorios |
| **Operador Consignataria** | Funcionario da instituicao | Averbacoes, simulacoes, consultas |
| **Agente** | Correspondente/Vendedor | Atendimento ao servidor, simulacoes, propostas |
| **Consulta Consignataria** | Auditor interno | Visualizacao de dados proprios |

---

## 3. Catalogo Completo de Permissoes

### 3.1 Modulo: Funcionarios (FUNC)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| FUNC_VISUALIZAR | Visualizar Funcionarios | Consultar dados cadastrais de funcionarios |
| FUNC_CRIAR | Criar Funcionario | Cadastrar novo funcionario |
| FUNC_EDITAR | Editar Funcionario | Alterar dados cadastrais |
| FUNC_EXCLUIR | Excluir Funcionario | Remover funcionario (inativacao logica) |
| FUNC_APOSENTAR | Aposentar Funcionario | Executar processo de aposentadoria |
| FUNC_HISTORICO | Ver Historico | Visualizar historico de alteracoes |
| FUNC_AUTORIZACOES | Gerenciar Autorizacoes | Criar/revogar autorizacoes de desconto |
| FUNC_BLOQUEAR | Bloquear/Desbloquear | Bloquear funcionario para novas averbacoes |
| FUNC_IMPORTAR | Importar Funcionarios | Executar importacao em lote |
| FUNC_EXPORTAR | Exportar Funcionarios | Exportar dados de funcionarios |

### 3.2 Modulo: Margem (MARG)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| MARG_VISUALIZAR | Visualizar Margem | Consultar margem de funcionarios |
| MARG_SIMULAR | Simular Margem | Calcular margem disponivel |
| MARG_RESERVAR | Reservar Margem | Reservar margem manualmente |
| MARG_LIBERAR | Liberar Margem | Liberar margem reservada |
| MARG_HISTORICO | Ver Historico | Visualizar evolucao da margem |
| MARG_EXPORTAR | Exportar Margem | Exportar dados de margem |

### 3.3 Modulo: Averbacoes (AVER)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| AVER_VISUALIZAR | Visualizar Averbacoes | Consultar averbacoes |
| AVER_CRIAR | Criar Averbacao | Registrar nova averbacao |
| AVER_EDITAR | Editar Averbacao | Alterar dados da averbacao |
| AVER_APROVAR | Aprovar Averbacao | Aprovar averbacao pendente |
| AVER_REJEITAR | Rejeitar Averbacao | Rejeitar averbacao com motivo |
| AVER_SUSPENDER | Suspender Averbacao | Suspender temporariamente |
| AVER_REATIVAR | Reativar Averbacao | Reativar averbacao suspensa |
| AVER_BLOQUEAR | Bloquear Averbacao | Bloquear administrativamente |
| AVER_DESBLOQUEAR | Desbloquear Averbacao | Remover bloqueio |
| AVER_CANCELAR | Cancelar Averbacao | Cancelar averbacao |
| AVER_LIQUIDAR | Liquidar Averbacao | Registrar liquidacao/quitacao |
| AVER_REAJUSTAR | Reajustar Parcela | Alterar valor da parcela |
| AVER_VINCULAR | Vincular Averbacoes | Vincular refin/compra |
| AVER_TERMO | Gerar Termo | Gerar termo de averbacao |
| AVER_IMPORTAR | Importar Averbacoes | Importar contratos em lote |
| AVER_EXPORTAR | Exportar Averbacoes | Exportar dados de averbacoes |

### 3.4 Modulo: Saldo Devedor (SALD)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| SALD_SOLICITAR | Solicitar Saldo | Solicitar saldo devedor |
| SALD_INFORMAR | Informar Saldo | Informar saldo devedor |
| SALD_CONFIRMAR | Confirmar Quitacao | Confirmar quitacao de divida |
| SALD_VISUALIZAR | Visualizar Solicitacoes | Ver solicitacoes de saldo |

### 3.5 Modulo: Simulacoes (SIMU)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| SIMU_EMPRESTIMO | Simular Emprestimo | Realizar simulacao de emprestimo |
| SIMU_COMPRA | Simular Compra Divida | Realizar simulacao de portabilidade |
| SIMU_COEF_VISUALIZAR | Visualizar Coeficientes | Ver tabelas de coeficientes |
| SIMU_COEF_GERENCIAR | Gerenciar Coeficientes | Criar/editar tabelas de coeficientes |
| SIMU_COEF_IMPORTAR | Importar Coeficientes | Importar coeficientes via arquivo |

### 3.6 Modulo: Conciliacao (CONC)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| CONC_VISUALIZAR | Visualizar Conciliacao | Ver resultados de conciliacao |
| CONC_EXECUTAR | Executar Conciliacao | Processar conciliacao mensal |
| CONC_IMPORTAR | Importar Retorno | Importar arquivo de retorno |
| CONC_TRATAR | Tratar Divergencias | Resolver divergencias manualmente |
| CONC_FECHAR | Fechar Competencia | Fechar competencia para alteracoes |
| CONC_REABRIR | Reabrir Competencia | Reabrir competencia fechada |
| CONC_EXPORTAR | Exportar Conciliacao | Exportar dados de conciliacao |

### 3.7 Modulo: Importacao/Exportacao (IMEX)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| IMEX_FUNC_IMP | Importar Funcionarios | Importar cadastro de funcionarios |
| IMEX_CONT_IMP | Importar Contratos | Importar averbacoes/contratos |
| IMEX_RET_IMP | Importar Retorno | Importar retorno da folha |
| IMEX_PERS_IMP | Importar Personalizado | Importar layout personalizado |
| IMEX_MARG_EXP | Exportar Margem | Exportar arquivo de margem |
| IMEX_REM_EXP | Exportar Remessa | Exportar remessa para folha |
| IMEX_HIST | Ver Historico | Visualizar historico de imports/exports |
| IMEX_LAYOUT | Gerenciar Layouts | Configurar layouts personalizados |

### 3.8 Modulo: Consignatarias (CONS)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| CONS_VISUALIZAR | Visualizar Consignatarias | Ver consignatarias cadastradas |
| CONS_CRIAR | Criar Consignataria | Cadastrar nova consignataria |
| CONS_EDITAR | Editar Consignataria | Alterar dados da consignataria |
| CONS_SUSPENDER | Suspender Consignataria | Suspender operacoes |
| CONS_ATIVAR | Ativar Consignataria | Reativar consignataria |
| CONS_CONVENIO | Gerenciar Convenio | Configurar convenio |

### 3.9 Modulo: Agentes (AGEN)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| AGEN_VISUALIZAR | Visualizar Agentes | Ver agentes cadastrados |
| AGEN_CRIAR | Criar Agente | Cadastrar novo agente |
| AGEN_EDITAR | Editar Agente | Alterar dados do agente |
| AGEN_INATIVAR | Inativar Agente | Desativar agente |
| AGEN_METAS | Gerenciar Metas | Configurar metas (Enterprise) |

### 3.10 Modulo: Produtos (PROD)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| PROD_VISUALIZAR | Visualizar Produtos | Ver produtos cadastrados |
| PROD_CRIAR | Criar Produto | Cadastrar novo produto |
| PROD_EDITAR | Editar Produto | Alterar dados do produto |
| PROD_INATIVAR | Inativar Produto | Desativar produto |

### 3.11 Modulo: Empresas (EMPR)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| EMPR_VISUALIZAR | Visualizar Empresas | Ver empresas do consignante |
| EMPR_CRIAR | Criar Empresa | Cadastrar nova empresa/orgao |
| EMPR_EDITAR | Editar Empresa | Alterar dados da empresa |
| EMPR_SUSPENDER | Suspender Empresa | Suspender para novas averbacoes |
| EMPR_ATIVAR | Ativar Empresa | Reativar empresa |

### 3.12 Modulo: Relatorios (RELA)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| RELA_PRODUCAO | Relatorio Producao | Acessar relatorio de producao |
| RELA_MARGEM | Relatorio Margem | Acessar relatorio de margem |
| RELA_INADIMPLENCIA | Relatorio Inadimplencia | Acessar relatorio de inadimplencia |
| RELA_CONCILIACAO | Relatorio Conciliacao | Acessar relatorio de conciliacao |
| RELA_CONTRATOS | Relatorio Contratos | Acessar relatorio de contratos |
| RELA_IMPACTO | Relatorio Impacto | Acessar relatorio de impacto |
| RELA_AUDITORIA | Relatorio Auditoria | Acessar relatorio de auditoria |
| RELA_RANKING | Relatorio Ranking | Acessar ranking de consignatarias |
| RELA_FLUXO | Relatorio Fluxo | Acessar fluxo de refinanciamento |
| RELA_VOLUME | Relatorio Volume | Acessar volume por competencia |
| RELA_EXPORTAR | Exportar Relatorios | Exportar relatorios (Excel, PDF) |
| RELA_AGENDAR | Agendar Relatorios | Agendar envio automatico (Enterprise) |

### 3.13 Modulo: Dashboard (DASH)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| DASH_CONSIGNANTE | Dashboard Consignante | Ver dashboard do orgao |
| DASH_CONSIGNATARIA | Dashboard Consignataria | Ver dashboard da instituicao |
| DASH_PESSOAL | Dashboard Pessoal | Ver dashboard individual |
| DASH_EXPORTAR | Exportar Dashboard | Exportar dados do dashboard |

### 3.14 Modulo: Usuarios (USER)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| USER_VISUALIZAR | Visualizar Usuarios | Ver usuarios cadastrados |
| USER_CRIAR | Criar Usuario | Cadastrar novo usuario |
| USER_EDITAR | Editar Usuario | Alterar dados do usuario |
| USER_INATIVAR | Inativar Usuario | Desativar usuario |
| USER_ATIVAR | Ativar Usuario | Reativar usuario |
| USER_RESET_SENHA | Resetar Senha | Forcar reset de senha |
| USER_SESSOES | Gerenciar Sessoes | Ver/encerrar sessoes ativas |

### 3.15 Modulo: Perfis (PERF)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| PERF_VISUALIZAR | Visualizar Perfis | Ver perfis cadastrados |
| PERF_CRIAR | Criar Perfil | Cadastrar novo perfil |
| PERF_EDITAR | Editar Perfil | Alterar permissoes do perfil |
| PERF_EXCLUIR | Excluir Perfil | Remover perfil |
| PERF_COPIAR | Copiar Perfil | Duplicar perfil existente |

### 3.16 Modulo: Configuracoes (CONF)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| CONF_PARAMETROS | Configurar Parametros | Alterar parametros do sistema |
| CONF_TENANT | Configurar Tenant | Alterar dados do orgao |
| CONF_EMAIL | Configurar Email | Configurar SMTP |
| CONF_INTEGRACAO | Configurar Integracoes | Webhooks, APIs (Enterprise) |

### 3.17 Modulo: Auditoria (AUDI)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| AUDI_VISUALIZAR | Visualizar Auditoria | Consultar logs de auditoria |
| AUDI_EXPORTAR | Exportar Auditoria | Exportar logs |
| AUDI_ACESSOS | Ver Acessos | Ver log de acessos |
| AUDI_CONFIGURAR | Configurar Retencao | Configurar politica de retencao |

### 3.18 Modulo: Mensagens (MENS)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| MENS_VISUALIZAR | Visualizar Mensagens | Ver caixa de entrada |
| MENS_ENVIAR | Enviar Mensagens | Enviar mensagens internas |
| MENS_BROADCAST | Enviar Broadcast | Enviar mensagem para todos |
| MENS_GERENCIAR | Gerenciar Mensagens | Administrar mensagens do sistema |

### 3.19 Modulo: Notificacoes (NOTI)

| ID | Permissao | Descricao |
|----|-----------|-----------|
| NOTI_CONFIGURAR | Configurar Notificacoes | Definir preferencias |
| NOTI_TEMPLATES | Gerenciar Templates | Editar templates de email |
| NOTI_WEBHOOKS | Gerenciar Webhooks | Configurar webhooks (Enterprise) |

---

## 4. Perfis Pre-definidos

### 4.1 Perfis do Consignante

#### PERFIL: Administrador Consignante

**Descricao:** Acesso total ao sistema para gestores do orgao publico.

**Permissoes Incluidas:** TODAS as permissoes dos modulos:
- Funcionarios, Margem, Averbacoes, Saldo Devedor
- Simulacoes, Conciliacao, Import/Export
- Consignatarias, Agentes, Produtos, Empresas
- Relatorios, Dashboard, Usuarios, Perfis
- Configuracoes, Auditoria, Mensagens, Notificacoes

---

#### PERFIL: Operador Consignante

**Descricao:** Acesso operacional para funcionarios do RH/DP.

| Modulo | Permissoes |
|--------|------------|
| Funcionarios | Visualizar, Criar, Editar, Historico, Autorizacoes, Importar |
| Margem | Visualizar, Simular, Historico, Exportar |
| Averbacoes | Visualizar, Editar, Termo, Exportar |
| Saldo Devedor | Visualizar |
| Simulacoes | Emprestimo, Compra, Coef_Visualizar |
| Conciliacao | Visualizar, Executar, Importar, Tratar, Exportar |
| Import/Export | Todas |
| Consignatarias | Visualizar |
| Empresas | Visualizar |
| Relatorios | Producao, Margem, Conciliacao, Contratos, Volume, Exportar |
| Dashboard | Consignante |
| Mensagens | Visualizar, Enviar |

---

#### PERFIL: Aprovador

**Descricao:** Foco em aprovacao de averbacoes.

| Modulo | Permissoes |
|--------|------------|
| Funcionarios | Visualizar, Historico |
| Margem | Visualizar, Simular |
| Averbacoes | Visualizar, Aprovar, Rejeitar, Suspender, Reativar, Bloquear, Desbloquear, Cancelar, Termo |
| Saldo Devedor | Visualizar, Confirmar |
| Simulacoes | Emprestimo, Compra, Coef_Visualizar |
| Consignatarias | Visualizar |
| Relatorios | Producao, Contratos, Exportar |
| Dashboard | Consignante |
| Mensagens | Visualizar, Enviar |

---

#### PERFIL: Consulta Consignante

**Descricao:** Somente leitura para auditores e fiscais.

| Modulo | Permissoes |
|--------|------------|
| Funcionarios | Visualizar, Historico |
| Margem | Visualizar, Historico |
| Averbacoes | Visualizar |
| Saldo Devedor | Visualizar |
| Conciliacao | Visualizar |
| Consignatarias | Visualizar |
| Empresas | Visualizar |
| Relatorios | Todas (somente visualizar e exportar) |
| Dashboard | Consignante |
| Auditoria | Visualizar, Exportar, Acessos |
| Mensagens | Visualizar |

---

### 4.2 Perfis da Consignataria

#### PERFIL: Administrador Consignataria

**Descricao:** Gestor da instituicao financeira no convenio.

| Modulo | Permissoes |
|--------|------------|
| Funcionarios | Visualizar |
| Margem | Visualizar, Simular |
| Averbacoes | Visualizar, Criar, Editar, Cancelar, Liquidar, Vincular, Termo, Importar, Exportar |
| Saldo Devedor | Todas |
| Simulacoes | Emprestimo, Compra, Coef_Visualizar, Coef_Gerenciar, Coef_Importar |
| Consignatarias | Visualizar, Editar (propria) |
| Agentes | Todas |
| Produtos | Todas |
| Relatorios | Producao, Inadimplencia, Contratos, Ranking, Fluxo, Volume, Exportar |
| Dashboard | Consignataria |
| Usuarios | Visualizar, Criar, Editar, Inativar, Ativar, Reset_Senha (proprios) |
| Perfis | Visualizar |
| Auditoria | Visualizar, Exportar (proprios) |
| Mensagens | Todas |

---

#### PERFIL: Operador Consignataria

**Descricao:** Funcionario da instituicao para operacoes.

| Modulo | Permissoes |
|--------|------------|
| Funcionarios | Visualizar |
| Margem | Visualizar, Simular |
| Averbacoes | Visualizar, Criar, Editar, Cancelar, Liquidar, Vincular, Termo |
| Saldo Devedor | Solicitar, Informar, Visualizar |
| Simulacoes | Emprestimo, Compra, Coef_Visualizar |
| Agentes | Visualizar |
| Relatorios | Producao, Contratos, Exportar |
| Dashboard | Consignataria |
| Mensagens | Visualizar, Enviar |

---

#### PERFIL: Agente

**Descricao:** Correspondente bancario com acesso minimo.

| Modulo | Permissoes |
|--------|------------|
| Funcionarios | Visualizar (CPF, nome, margem apenas) |
| Margem | Visualizar, Simular |
| Averbacoes | Visualizar (proprias), Criar |
| Simulacoes | Emprestimo, Compra |
| Dashboard | Pessoal |
| Mensagens | Visualizar, Enviar |

---

#### PERFIL: Consulta Consignataria

**Descricao:** Somente leitura para auditorias internas.

| Modulo | Permissoes |
|--------|------------|
| Funcionarios | Visualizar |
| Margem | Visualizar |
| Averbacoes | Visualizar |
| Saldo Devedor | Visualizar |
| Agentes | Visualizar |
| Relatorios | Todas (somente visualizar e exportar) |
| Dashboard | Consignataria |
| Auditoria | Visualizar, Exportar |
| Mensagens | Visualizar |

---

## 5. Matriz de Permissoes

### 5.1 Perfis Consignante

| Permissao | Admin | Operador | Aprovador | Consulta |
|-----------|:-----:|:--------:|:---------:|:--------:|
| **FUNCIONARIOS** |||||
| FUNC_VISUALIZAR | X | X | X | X |
| FUNC_CRIAR | X | X | - | - |
| FUNC_EDITAR | X | X | - | - |
| FUNC_EXCLUIR | X | - | - | - |
| FUNC_APOSENTAR | X | - | - | - |
| FUNC_HISTORICO | X | X | X | X |
| FUNC_AUTORIZACOES | X | X | - | - |
| FUNC_BLOQUEAR | X | - | - | - |
| FUNC_IMPORTAR | X | X | - | - |
| FUNC_EXPORTAR | X | X | - | X |
| **MARGEM** |||||
| MARG_VISUALIZAR | X | X | X | X |
| MARG_SIMULAR | X | X | X | - |
| MARG_RESERVAR | X | - | - | - |
| MARG_LIBERAR | X | - | - | - |
| MARG_HISTORICO | X | X | - | X |
| MARG_EXPORTAR | X | X | - | X |
| **AVERBACOES** |||||
| AVER_VISUALIZAR | X | X | X | X |
| AVER_CRIAR | X | - | - | - |
| AVER_EDITAR | X | X | - | - |
| AVER_APROVAR | X | - | X | - |
| AVER_REJEITAR | X | - | X | - |
| AVER_SUSPENDER | X | - | X | - |
| AVER_REATIVAR | X | - | X | - |
| AVER_BLOQUEAR | X | - | X | - |
| AVER_DESBLOQUEAR | X | - | X | - |
| AVER_CANCELAR | X | - | X | - |
| AVER_LIQUIDAR | X | - | - | - |
| AVER_REAJUSTAR | X | - | - | - |
| AVER_VINCULAR | X | - | - | - |
| AVER_TERMO | X | X | X | - |
| AVER_IMPORTAR | X | - | - | - |
| AVER_EXPORTAR | X | X | - | X |
| **CONCILIACAO** |||||
| CONC_VISUALIZAR | X | X | - | X |
| CONC_EXECUTAR | X | X | - | - |
| CONC_IMPORTAR | X | X | - | - |
| CONC_TRATAR | X | X | - | - |
| CONC_FECHAR | X | - | - | - |
| CONC_REABRIR | X | - | - | - |
| CONC_EXPORTAR | X | X | - | X |
| **CONFIGURACOES** |||||
| CONF_PARAMETROS | X | - | - | - |
| CONF_TENANT | X | - | - | - |
| CONF_EMAIL | X | - | - | - |
| CONF_INTEGRACAO | X | - | - | - |
| **USUARIOS** |||||
| USER_VISUALIZAR | X | - | - | - |
| USER_CRIAR | X | - | - | - |
| USER_EDITAR | X | - | - | - |
| USER_INATIVAR | X | - | - | - |
| USER_ATIVAR | X | - | - | - |
| USER_RESET_SENHA | X | - | - | - |
| USER_SESSOES | X | - | - | - |
| **AUDITORIA** |||||
| AUDI_VISUALIZAR | X | - | - | X |
| AUDI_EXPORTAR | X | - | - | X |
| AUDI_ACESSOS | X | - | - | X |
| AUDI_CONFIGURAR | X | - | - | - |

### 5.2 Perfis Consignataria

| Permissao | Admin | Operador | Agente | Consulta |
|-----------|:-----:|:--------:|:------:|:--------:|
| **FUNCIONARIOS** |||||
| FUNC_VISUALIZAR | X | X | X* | X |
| **MARGEM** |||||
| MARG_VISUALIZAR | X | X | X | X |
| MARG_SIMULAR | X | X | X | - |
| **AVERBACOES** |||||
| AVER_VISUALIZAR | X | X | X* | X |
| AVER_CRIAR | X | X | X | - |
| AVER_EDITAR | X | X | - | - |
| AVER_CANCELAR | X | X | - | - |
| AVER_LIQUIDAR | X | X | - | - |
| AVER_VINCULAR | X | X | - | - |
| AVER_TERMO | X | X | - | - |
| AVER_IMPORTAR | X | - | - | - |
| AVER_EXPORTAR | X | - | - | X |
| **SALDO DEVEDOR** |||||
| SALD_SOLICITAR | X | X | - | - |
| SALD_INFORMAR | X | X | - | - |
| SALD_CONFIRMAR | X | - | - | - |
| SALD_VISUALIZAR | X | X | - | X |
| **SIMULACOES** |||||
| SIMU_EMPRESTIMO | X | X | X | - |
| SIMU_COMPRA | X | X | X | - |
| SIMU_COEF_VISUALIZAR | X | X | - | - |
| SIMU_COEF_GERENCIAR | X | - | - | - |
| SIMU_COEF_IMPORTAR | X | - | - | - |
| **AGENTES** |||||
| AGEN_VISUALIZAR | X | X | - | X |
| AGEN_CRIAR | X | - | - | - |
| AGEN_EDITAR | X | - | - | - |
| AGEN_INATIVAR | X | - | - | - |
| **USUARIOS** |||||
| USER_VISUALIZAR | X | - | - | - |
| USER_CRIAR | X | - | - | - |
| USER_EDITAR | X | - | - | - |
| USER_RESET_SENHA | X | - | - | - |

**Notas:**
- `X*` = Acesso restrito (apenas proprios ou dados limitados)

---

## 6. Regras de Aplicacao de Permissoes

### 6.1 Regras Gerais

| ID | Regra |
|----|-------|
| R-01 | Usuario deve ter pelo menos um perfil ativo |
| R-02 | Permissoes sao cumulativas entre perfis |
| R-03 | Negacao explicita tem precedencia sobre concessao |
| R-04 | Administrador nao pode remover proprio acesso de administrador |
| R-05 | Usuario inativo perde todas as permissoes |

### 6.2 Regras de Escopo

| ID | Regra |
|----|-------|
| R-06 | Usuario Consignataria so ve dados de sua consignataria |
| R-07 | Agente so ve averbacoes que ele criou |
| R-08 | Dados de funcionarios para Consignataria sao limitados (sem endereco, sem telefone pessoal) |
| R-09 | Usuario Consignante ve dados de todas consignatarias conveniadas |
| R-10 | Relatorios respeitam escopo do usuario |

### 6.3 Regras de Seguranca

| ID | Regra |
|----|-------|
| R-11 | Tentativa de acesso negado e registrada em auditoria |
| R-12 | Alteracao de permissoes requer confirmacao de senha |
| R-13 | Perfis de sistema nao podem ser excluidos |
| R-14 | Permissoes sensiveis requerem 2FA (Enterprise) |
| R-15 | Sessao expira apos inatividade (configuravel) |

---

## 7. Historico de Revisoes

| Versao | Data | Autor | Descricao |
|--------|------|-------|-----------|
| 1.0 | Janeiro 2026 | Product Team | Versao inicial |

---

*Este documento define o modelo de permissoes do sistema FastConsig e deve ser mantido atualizado conforme evolucao do produto.*
