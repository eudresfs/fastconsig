# Product Requirements Document (PRD)
# FastConsig - Sistema de Gestão de Consignados

**Versão:** 1.0
**Data:** Janeiro 2026
**Status:** Draft
**Autor:** Product Team

---

## Sumário Executivo

O FastConsig é um sistema completo de gestão de empréstimos consignados destinado a órgãos públicos (Consignantes), instituições financeiras (Consignatárias) e seus operadores. Este PRD documenta os requisitos para a reconstrução completa do sistema utilizando tecnologias modernas, mantendo a funcionalidade comprovada do sistema legado enquanto adiciona capacidades contemporâneas.

### Visão do Produto

> Ser a plataforma de referência para gestão de consignados no setor público brasileiro, oferecendo transparência, eficiência e conformidade regulatória através de uma experiência digital moderna.

### Escopo do Documento

- **Sistema Completo:** Todas as funcionalidades do sistema legado
- **MVP Destacado:** Funcionalidades essenciais para lançamento inicial
- **Enterprise:** Recursos avançados para fases posteriores

---

## 1. Problema e Contexto

### 1.1 Contexto de Negócio

O mercado de crédito consignado no Brasil movimenta bilhões de reais anualmente, especialmente no setor público. Órgãos públicos (prefeituras, autarquias, institutos de previdência) precisam gerenciar:

- **Margens consignáveis** dos servidores (limite de desconto em folha)
- **Averbações** (autorização de desconto para empréstimos)
- **Conciliação mensal** entre valores enviados e descontados
- **Múltiplas instituições financeiras** conveniadas
- **Conformidade** com legislação e órgãos de controle

### 1.2 Problema a Resolver

Atualmente, muitos órgãos públicos:

1. Utilizam planilhas Excel para controle de consignados
2. Não possuem visibilidade em tempo real das margens
3. Têm processos manuais de aprovação sujeitos a erros
4. Enfrentam dificuldades na conciliação mensal
5. Não conseguem auditar adequadamente as operações
6. Dependem de múltiplos sistemas não integrados

### 1.3 Solução Proposta

O FastConsig oferece uma plataforma unificada que:

- Centraliza toda gestão de consignados em um único sistema
- Automatiza cálculos de margem e validações
- Oferece fluxos de aprovação configuráveis
- Integra com folha de pagamento via importação/exportação
- Fornece dashboards e relatórios gerenciais
- Mantém trilha de auditoria completa
- Suporta múltiplos clientes (multi-tenant)

---

## 2. Usuários e Personas

### 2.1 Tipos de Usuário

#### 2.1.1 Consignante (Órgão Público)

**Quem são:** Prefeituras, Autarquias, Institutos de Previdência, Câmaras Municipais

**Responsabilidades:**
- Cadastro e manutenção de servidores (funcionários)
- Aprovação de averbações
- Conciliação mensal com folha de pagamento
- Gestão de convênios com consignatárias
- Configuração de regras e parâmetros do sistema

**Necessidades Principais:**
- Visibilidade total das margens dos servidores
- Controle sobre quais consignatárias podem operar
- Relatórios para prestação de contas
- Auditoria de todas as operações

#### 2.1.2 Consignatária (Instituição Financeira)

**Quem são:** Bancos, Cooperativas de Crédito, Financeiras

**Responsabilidades:**
- Registro de novas averbações (empréstimos)
- Gestão de contratos ativos
- Importação de retornos da folha
- Gestão de carteira de clientes

**Necessidades Principais:**
- Consulta de margem disponível em tempo real
- Simulação de empréstimos
- Acompanhamento de status das averbações
- Relatórios de produção e inadimplência

#### 2.1.3 Operador/Agente

**Quem são:** Correspondentes bancários, funcionários de agências, operadores de telemarketing

**Responsabilidades:**
- Atendimento direto ao servidor
- Registro de propostas de empréstimo
- Simulações de crédito
- Acompanhamento de solicitações

**Necessidades Principais:**
- Interface simples e rápida
- Acesso móvel (fase Enterprise)
- Visibilidade de suas operações
- Metas e comissionamento

### 2.2 Matriz de Funcionalidades por Perfil

| Módulo | Consignante | Consignatária | Operador |
|--------|-------------|---------------|----------|
| Dashboard | Gerencial | Produção | Pessoal |
| Funcionários | CRUD Completo | Consulta | Consulta |
| Averbações | Aprovar/Rejeitar | CRUD | Criar/Consultar |
| Simulação | Visualizar | Usar | Usar |
| Conciliação | CRUD | Visualizar | - |
| Relatórios | Todos | Próprios | Próprios |
| Configurações | Sistema | Próprias | - |
| Usuários | Gerenciar Todos | Próprios | - |

---

## 3. Requisitos Funcionais

### 3.1 Módulo: Autenticação e Autorização

#### 3.1.1 Autenticação [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| AUTH-001 | Login com usuário e senha | MVP |
| AUTH-002 | Autenticação via JWT com refresh tokens | MVP |
| AUTH-003 | Recuperação de senha via email | MVP |
| AUTH-004 | Bloqueio após tentativas inválidas (configurável) | MVP |
| AUTH-005 | Logout com invalidação de token | MVP |
| AUTH-006 | Sessão com timeout configurável | MVP |

#### 3.1.2 Autenticação [Enterprise]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| AUTH-007 | Autenticação de dois fatores (2FA) via TOTP | Enterprise |
| AUTH-008 | Single Sign-On (SSO) via SAML/OIDC | Enterprise |
| AUTH-009 | Integração com Active Directory/LDAP | Enterprise |
| AUTH-010 | Login via certificado digital (ICP-Brasil) | Enterprise |

#### 3.1.3 Autorização [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| AUTHZ-001 | Sistema de perfis de acesso | MVP |
| AUTHZ-002 | Permissões granulares por funcionalidade | MVP |
| AUTHZ-003 | Segregação de dados por tenant | MVP |
| AUTHZ-004 | Herança de permissões entre perfis | MVP |

### 3.2 Módulo: Gestão de Funcionários (Servidores)

#### 3.2.1 Cadastro de Funcionários [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| FUNC-001 | Cadastro completo de funcionário (dados pessoais, funcionais, bancários) | MVP |
| FUNC-002 | Validação de CPF único por tenant | MVP |
| FUNC-003 | Validação de matrícula única por empresa | MVP |
| FUNC-004 | Suporte a múltiplos vínculos (mesmo CPF, matrículas diferentes) | MVP |
| FUNC-005 | Situações: Ativo, Inativo, Afastado, Aposentado, Exonerado | MVP |
| FUNC-006 | Histórico de alterações cadastrais | MVP |
| FUNC-007 | Upload de foto do funcionário | MVP |
| FUNC-008 | Busca por CPF, matrícula, nome | MVP |

#### 3.2.2 Gestão de Margem [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| MARG-001 | Cálculo automático de margem consignável | MVP |
| MARG-002 | Margem base = percentual do salário bruto (configurável) | MVP |
| MARG-003 | Margem disponível = Margem base - Averbações ativas | MVP |
| MARG-004 | Margem separada por produto (Empréstimo, Cartão, Seguros) | MVP |
| MARG-005 | Reserva de margem durante processo de averbação | MVP |
| MARG-006 | Liberação automática de margem em cancelamentos | MVP |
| MARG-007 | Histórico de evolução da margem | MVP |

#### 3.2.3 Autorizações de Funcionário [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| AUTF-001 | Registro de autorização para desconto em folha | MVP |
| AUTF-002 | Autorização por consignatária específica | MVP |
| AUTF-003 | Vigência da autorização (data início/fim) | MVP |
| AUTF-004 | Revogação de autorização com motivo | MVP |

#### 3.2.4 Aposentadoria e Afastamentos [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| APOS-001 | Processo de aposentadoria com transferência de averbações | MVP |
| APOS-002 | Cálculo de impacto da aposentadoria na margem | MVP |
| APOS-003 | Impressão de termo de aposentadoria | MVP |
| APOS-004 | Suspensão temporária por afastamento | MVP |

### 3.3 Módulo: Averbações (Empréstimos)

#### 3.3.1 Registro de Averbação [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| AVER-001 | Criação de nova averbação vinculada a funcionário | MVP |
| AVER-002 | Tipos: Empréstimo Novo, Refinanciamento, Compra de Dívida, Cartão | MVP |
| AVER-003 | Campos obrigatórios: valor, parcelas, taxa, data início desconto | MVP |
| AVER-004 | Validação de margem disponível antes de criar | MVP |
| AVER-005 | Cálculo automático de valor da parcela | MVP |
| AVER-006 | Vinculação de averbações (refinanciamento liquida anterior) | MVP |
| AVER-007 | Número de contrato único por consignatária | MVP |
| AVER-008 | Upload de documentos comprobatórios | MVP |

#### 3.3.2 Fluxo de Aprovação [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| FLOW-001 | Status: Aguardando Aprovação → Aprovada → Enviada → Descontada | MVP |
| FLOW-002 | Status adicional: Rejeitada, Suspensa, Bloqueada, Liquidada, Cancelada | MVP |
| FLOW-003 | Aprovação individual ou em lote | MVP |
| FLOW-004 | Rejeição com motivo obrigatório | MVP |
| FLOW-005 | Registro de usuário e data de cada mudança de status | MVP |
| FLOW-006 | Notificação por email nas mudanças de status | MVP |
| FLOW-007 | Workflow configurável por consignante | Enterprise |

#### 3.3.3 Operações sobre Averbações [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| OPER-001 | Suspensão temporária (com motivo e previsão) | MVP |
| OPER-002 | Bloqueio administrativo (judicial, fraude, etc.) | MVP |
| OPER-003 | Cancelamento (com motivo e estorno de margem) | MVP |
| OPER-004 | Liquidação total (quitação antecipada) | MVP |
| OPER-005 | Liquidação parcial (amortização extraordinária) | MVP |
| OPER-006 | Reajuste de parcela (por determinação judicial/legal) | MVP |
| OPER-007 | Informação de saldo devedor pela consignatária | MVP |

#### 3.3.4 Compra de Dívida / Portabilidade [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| PORT-001 | Solicitação de saldo devedor | MVP |
| PORT-002 | Registro de saldo devedor informado | MVP |
| PORT-003 | Confirmação de quitação pela cedente | MVP |
| PORT-004 | Criação de nova averbação pela cessionária | MVP |
| PORT-005 | Fluxo completo de portabilidade com rastreabilidade | MVP |

#### 3.3.5 Termo de Averbação [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| TERM-001 | Geração de termo/contrato em PDF | MVP |
| TERM-002 | Template configurável por consignatária | MVP |
| TERM-003 | Campos dinâmicos (dados funcionário, valores, datas) | MVP |
| TERM-004 | Assinatura digital do termo | Enterprise |

### 3.4 Módulo: Simulações

#### 3.4.1 Simulação de Empréstimo [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| SIM-001 | Simulação por valor desejado | MVP |
| SIM-002 | Simulação por valor de parcela desejada | MVP |
| SIM-003 | Múltiplos prazos simultâneos | MVP |
| SIM-004 | Uso de coeficientes/tabelas de taxas | MVP |
| SIM-005 | Consideração de IOF e TAC | MVP |
| SIM-006 | Exibição de CET (Custo Efetivo Total) | MVP |
| SIM-007 | Impressão/export da simulação | MVP |

#### 3.4.2 Simulação de Compra de Dívida [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| SIMC-001 | Informar dívidas a serem quitadas | MVP |
| SIMC-002 | Cálculo de troco ao cliente | MVP |
| SIMC-003 | Comparação de cenários | MVP |
| SIMC-004 | Múltiplas dívidas consolidadas | MVP |

#### 3.4.3 Gestão de Coeficientes [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| COEF-001 | Cadastro de tabelas de coeficientes | MVP |
| COEF-002 | Coeficientes por prazo | MVP |
| COEF-003 | Vigência das tabelas (data início/fim) | MVP |
| COEF-004 | Importação de coeficientes via Excel | MVP |
| COEF-005 | Coeficientes diferenciados por perfil de cliente | Enterprise |

### 3.5 Módulo: Conciliação Mensal

#### 3.5.1 Processo de Conciliação [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| CONC-001 | Conciliação por competência (mês/ano) | MVP |
| CONC-002 | Comparação: Averbações Enviadas vs Descontadas na Folha | MVP |
| CONC-003 | Identificação automática de divergências | MVP |
| CONC-004 | Categorias: Conforme, Não Descontado, Valor Divergente, Não Enviado | MVP |
| CONC-005 | Tratamento manual de exceções | MVP |
| CONC-006 | Fechamento de competência com bloqueio de alterações | MVP |

#### 3.5.2 Relatórios de Conciliação [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| RELC-001 | Resumo por consignatária | MVP |
| RELC-002 | Detalhamento de divergências | MVP |
| RELC-003 | Histórico de conciliações | MVP |
| RELC-004 | Export para Excel | MVP |

### 3.6 Módulo: Importação e Exportação

#### 3.6.1 Importação de Dados [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| IMP-001 | Importação de funcionários (Excel/CSV) | MVP |
| IMP-002 | Importação de averbações/contratos (Excel/CSV) | MVP |
| IMP-003 | Importação de retorno da folha (descontos efetivados) | MVP |
| IMP-004 | Layouts configuráveis por cliente | MVP |
| IMP-005 | Validação prévia com relatório de erros | MVP |
| IMP-006 | Importação parcial (ignorar linhas com erro) | MVP |
| IMP-007 | Log detalhado de importações | MVP |
| IMP-008 | Agendamento de importações | Enterprise |

#### 3.6.2 Exportação de Dados [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| EXP-001 | Exportação de margem para consignatárias | MVP |
| EXP-002 | Exportação de averbações para desconto em folha | MVP |
| EXP-003 | Layouts configuráveis (txt posicional, CSV, Excel) | MVP |
| EXP-004 | Filtros por competência, consignatária, status | MVP |
| EXP-005 | Histórico de exportações | MVP |
| EXP-006 | Geração automática mensal | Enterprise |

### 3.7 Módulo: Consignatárias

#### 3.7.1 Cadastro de Consignatárias [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| CONS-001 | Cadastro completo (razão social, CNPJ, contatos) | MVP |
| CONS-002 | Dados bancários para repasse | MVP |
| CONS-003 | Configuração de produtos oferecidos | MVP |
| CONS-004 | Limites operacionais (valor máximo, prazo máximo) | MVP |
| CONS-005 | Situação: Ativa, Suspensa, Inativa | MVP |
| CONS-006 | Convênio com vigência | MVP |

#### 3.7.2 Gestão de Agentes/Operadores [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| AGEN-001 | Cadastro de agentes vinculados à consignatária | MVP |
| AGEN-002 | Permissões específicas por agente | MVP |
| AGEN-003 | Metas e limites por agente | Enterprise |
| AGEN-004 | Relatório de produção por agente | MVP |

#### 3.7.3 Produtos [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| PROD-001 | Cadastro de produtos (Empréstimo, Cartão, Seguro) | MVP |
| PROD-002 | Configuração de margem específica por produto | MVP |
| PROD-003 | Regras de prazo mínimo/máximo | MVP |
| PROD-004 | Taxas e coeficientes por produto | MVP |

### 3.8 Módulo: Empresas/Órgãos (Estrutura Organizacional)

#### 3.8.1 Cadastro de Empresas [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| EMP-001 | Cadastro de órgãos/empresas do consignante | MVP |
| EMP-002 | Tipos: Prefeitura, Autarquia, Câmara, Instituto de Previdência, Fundo, Consórcio, Fundação, Outros | MVP |
| EMP-003 | Hierarquia de empresas (empresa pai) | MVP |
| EMP-004 | Configuração de margem por empresa | MVP |
| EMP-005 | Suspensão de empresa (bloqueia novas averbações) | MVP |

### 3.9 Módulo: Relatórios e Dashboards

#### 3.9.1 Dashboards [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| DASH-001 | Dashboard do Consignante: visão geral do órgão | MVP |
| DASH-002 | Dashboard da Consignatária: produção e carteira | MVP |
| DASH-003 | Indicadores em tempo real | MVP |
| DASH-004 | Gráficos interativos (barras, pizza, linha, área) | MVP |
| DASH-005 | Filtros por período, empresa, consignatária | MVP |
| DASH-006 | Export de dashboards | MVP |

#### 3.9.2 KPIs do Dashboard Consignante

| ID | Indicador | Descrição |
|----|-----------|-----------|
| KPI-C01 | Total de Servidores Ativos | Quantidade de funcionários ativos |
| KPI-C02 | Margem Total Disponível | Soma das margens disponíveis |
| KPI-C03 | Margem Total Comprometida | Soma das averbações ativas |
| KPI-C04 | % Utilização de Margem | Comprometida / (Comprometida + Disponível) |
| KPI-C05 | Averbações Pendentes | Aguardando aprovação |
| KPI-C06 | Volume Mensal por Consignatária | Ranking de produção |
| KPI-C07 | Evolução Mensal | Série histórica de averbações |
| KPI-C08 | Taxa de Inadimplência | Não descontados / Enviados |

#### 3.9.3 KPIs do Dashboard Consignatária

| ID | Indicador | Descrição |
|----|-----------|-----------|
| KPI-B01 | Produção do Mês | Valor total de averbações criadas |
| KPI-B02 | Quantidade de Contratos | Número de averbações ativas |
| KPI-B03 | Carteira Total | Saldo devedor total |
| KPI-B04 | Taxa Média | Taxa média ponderada |
| KPI-B05 | Prazo Médio | Prazo médio ponderado |
| KPI-B06 | Produção por Agente | Ranking de operadores |
| KPI-B07 | Pipeline | Averbações em aprovação |
| KPI-B08 | Cancelamentos | Volume de cancelamentos no mês |

#### 3.9.4 Relatórios Gerenciais [MVP]

| ID | Relatório | Descrição |
|----|-----------|-----------|
| REL-001 | Análise de Produção | Averbações por período, consignatária, produto |
| REL-002 | Posição de Margem | Margem por funcionário/empresa |
| REL-003 | Contratos Ativos | Listagem de averbações com filtros |
| REL-004 | Inadimplência | Contratos não descontados |
| REL-005 | Conciliação | Resultado da conciliação mensal |
| REL-006 | Impacto de Alterações | Mudanças cadastrais e impacto em averbações |
| REL-007 | Auditoria | Log de operações do sistema |
| REL-008 | Ranking de Consignatárias | Comparativo de produção |
| REL-009 | Fluxo de Refinanciamento | Operações de refin e compra de dívida |
| REL-010 | Volume por Competência | Série histórica de descontos |

#### 3.9.5 Exportação de Relatórios [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| REXP-001 | Exportação para Excel (.xlsx) | MVP |
| REXP-002 | Exportação para PDF | MVP |
| REXP-003 | Exportação para CSV | MVP |
| REXP-004 | Agendamento de relatórios por email | Enterprise |

### 3.10 Módulo: Usuários e Permissões

#### 3.10.1 Gestão de Usuários [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| USR-001 | CRUD de usuários | MVP |
| USR-002 | Vinculação a perfil de acesso | MVP |
| USR-003 | Vinculação a consignatária (se aplicável) | MVP |
| USR-004 | Ativação/Inativação de usuário | MVP |
| USR-005 | Reset de senha por administrador | MVP |
| USR-006 | Primeiro acesso com troca de senha obrigatória | MVP |

#### 3.10.2 Perfis de Acesso [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| PERF-001 | Cadastro de perfis customizados | MVP |
| PERF-002 | Atribuição de permissões ao perfil | MVP |
| PERF-003 | Perfis pré-definidos (Admin, Operador, Consulta) | MVP |
| PERF-004 | Cópia de perfil existente | MVP |

#### 3.10.3 Catálogo de Permissões [MVP]

O sistema deve implementar permissões granulares organizadas por módulo:

**Funcionários:**
- Visualizar, Criar, Editar, Excluir, Aposentar, Ver Histórico

**Averbações:**
- Visualizar, Criar, Aprovar, Rejeitar, Suspender, Bloquear, Cancelar, Liquidar, Editar

**Simulações:**
- Realizar Simulação, Gerenciar Coeficientes

**Conciliação:**
- Visualizar, Executar, Fechar Competência

**Importação/Exportação:**
- Importar Funcionários, Importar Contratos, Importar Retorno, Exportar Margem, Exportar Desconto

**Consignatárias:**
- Visualizar, Criar, Editar, Suspender

**Relatórios:**
- Acessar por módulo (Produção, Margem, Inadimplência, Auditoria, etc.)

**Configurações:**
- Parâmetros, Usuários, Perfis, Empresas

### 3.11 Módulo: Mensagens e Notificações

#### 3.11.1 Central de Mensagens [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| MSG-001 | Caixa de entrada de mensagens internas | MVP |
| MSG-002 | Envio de mensagens entre usuários | MVP |
| MSG-003 | Mensagens do sistema (avisos, alertas) | MVP |
| MSG-004 | Marcação de lido/não lido | MVP |
| MSG-005 | Arquivamento de mensagens | MVP |

#### 3.11.2 Notificações por Email [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| NOTF-001 | Notificação de nova averbação para aprovação | MVP |
| NOTF-002 | Notificação de aprovação/rejeição | MVP |
| NOTF-003 | Notificação de solicitação de saldo devedor | MVP |
| NOTF-004 | Configuração de preferências de notificação | MVP |
| NOTF-005 | Templates de email customizáveis | Enterprise |

#### 3.11.3 Webhooks [Enterprise]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| HOOK-001 | Configuração de endpoints de webhook | Enterprise |
| HOOK-002 | Eventos: Averbação criada, aprovada, rejeitada, liquidada | Enterprise |
| HOOK-003 | Retry automático em caso de falha | Enterprise |
| HOOK-004 | Log de webhooks enviados | Enterprise |
| HOOK-005 | Assinatura HMAC para validação | Enterprise |

### 3.12 Módulo: Auditoria e Logs

#### 3.12.1 Trilha de Auditoria [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| AUD-001 | Log de todas as operações CRUD | MVP |
| AUD-002 | Registro de usuário, data/hora, IP, navegador | MVP |
| AUD-003 | Registro de valores antes/depois (alterações) | MVP |
| AUD-004 | Consulta de auditoria com filtros | MVP |
| AUD-005 | Retenção configurável de logs | MVP |
| AUD-006 | Logs imutáveis (append-only) | MVP |

#### 3.12.2 Auditoria de Acesso [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| AUDA-001 | Log de login/logout | MVP |
| AUDA-002 | Log de tentativas de acesso inválidas | MVP |
| AUDA-003 | Relatório de acessos por usuário | MVP |
| AUDA-004 | Sessões ativas com opção de encerramento | MVP |

### 3.13 Módulo: Configurações

#### 3.13.1 Parâmetros do Sistema [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| CONF-001 | Percentual de margem padrão | MVP |
| CONF-002 | Percentual de margem por produto | MVP |
| CONF-003 | Prazo máximo de empréstimo | MVP |
| CONF-004 | Idade máxima ao final do contrato | MVP |
| CONF-005 | Dias para expirar aprovação | MVP |
| CONF-006 | Obrigatoriedade de documentos | MVP |

#### 3.13.2 Configurações de Tenant [MVP]

| ID | Requisito | Prioridade |
|----|-----------|------------|
| TENT-001 | Logo do órgão | MVP |
| TENT-002 | Cores personalizadas | Enterprise |
| TENT-003 | Dados do órgão (nome, CNPJ, endereço) | MVP |
| TENT-004 | Configurações de SMTP para emails | MVP |

---

## 4. Requisitos Não-Funcionais

### 4.1 Performance

| ID | Requisito | Meta |
|----|-----------|------|
| PERF-001 | Tempo de resposta para consultas simples | < 500ms |
| PERF-002 | Tempo de resposta para relatórios | < 5s |
| PERF-003 | Tempo de processamento de importação | < 1000 registros/segundo |
| PERF-004 | Suporte a usuários simultâneos | A ser dimensionado por cliente |

### 4.2 Disponibilidade

| ID | Requisito | Descrição |
|----|-----------|-----------|
| DISP-001 | Disponibilidade mínima | 99.5% (MVP) / 99.9% (Enterprise) |
| DISP-002 | Janela de manutenção | Fora do horário comercial |
| DISP-003 | Backup automático | Diário com retenção de 30 dias |
| DISP-004 | Disaster Recovery | RTO 4h / RPO 1h (Enterprise) |

### 4.3 Segurança

#### 4.3.1 Segurança MVP

| ID | Requisito |
|----|-----------|
| SEC-001 | Comunicação exclusivamente via HTTPS/TLS 1.3 |
| SEC-002 | Senhas com hash bcrypt (não MD5) |
| SEC-003 | Tokens JWT com expiração e refresh |
| SEC-004 | Rate limiting para prevenir brute force |
| SEC-005 | Headers de segurança (CSP, HSTS, X-Frame-Options) |
| SEC-006 | Sanitização de inputs (prevenção de SQL Injection, XSS) |
| SEC-007 | CORS configurado por ambiente |

#### 4.3.2 Segurança Enterprise

| ID | Requisito |
|----|-----------|
| SEC-008 | Autenticação de dois fatores (2FA) |
| SEC-009 | Single Sign-On (SSO) |
| SEC-010 | Criptografia de dados sensíveis em repouso (AES-256) |
| SEC-011 | Certificação de segurança (penetration testing) |
| SEC-012 | WAF (Web Application Firewall) |

### 4.4 Compliance

#### 4.4.1 LGPD (MVP)

| ID | Requisito |
|----|-----------|
| LGPD-001 | Consentimento para coleta de dados |
| LGPD-002 | Direito de acesso aos dados pessoais |
| LGPD-003 | Direito de retificação |
| LGPD-004 | Direito de exclusão (quando aplicável) |
| LGPD-005 | Registro de tratamento de dados |
| LGPD-006 | Notificação de incidentes |

#### 4.4.2 TCE - Tribunal de Contas (Enterprise)

| ID | Requisito |
|----|-----------|
| TCE-001 | Relatórios no formato exigido pelo TCE estadual |
| TCE-002 | Exportação de dados para prestação de contas |
| TCE-003 | Trilha de auditoria completa para fiscalização |
| TCE-004 | Retenção de dados conforme legislação |

### 4.5 Usabilidade

| ID | Requisito |
|----|-----------|
| UX-001 | Interface responsiva (desktop e tablets) |
| UX-002 | Suporte a navegadores modernos (Chrome, Firefox, Edge, Safari) |
| UX-003 | Acessibilidade WCAG 2.1 nível AA |
| UX-004 | Internacionalização preparada (pt-BR inicial) |
| UX-005 | Feedback visual para operações longas |
| UX-006 | Mensagens de erro claras e acionáveis |

### 4.6 Escalabilidade

| ID | Requisito |
|----|-----------|
| SCAL-001 | Arquitetura preparada para escala horizontal |
| SCAL-002 | Banco de dados com particionamento por tenant |
| SCAL-003 | Cache distribuído para dados frequentes |
| SCAL-004 | Filas para processamento assíncrono |

---

## 5. Integrações

### 5.1 APIs REST

O sistema deve expor APIs REST documentadas com OpenAPI/Swagger para:

#### 5.1.1 APIs Públicas (para Consignatárias)

| Endpoint | Método | Descrição | Prioridade |
|----------|--------|-----------|------------|
| /api/v1/funcionarios/{cpf}/margem | GET | Consultar margem disponível | MVP |
| /api/v1/averbacoes | POST | Criar nova averbação | MVP |
| /api/v1/averbacoes/{id} | GET | Consultar averbação | MVP |
| /api/v1/averbacoes/{id}/status | GET | Consultar status | MVP |
| /api/v1/simulacoes/emprestimo | POST | Simular empréstimo | MVP |
| /api/v1/simulacoes/compra-divida | POST | Simular compra de dívida | MVP |
| /api/v1/saldo-devedor/solicitar | POST | Solicitar saldo devedor | MVP |
| /api/v1/saldo-devedor/{id}/informar | PUT | Informar saldo devedor | MVP |

#### 5.1.2 APIs Internas (para Frontend)

Todas as funcionalidades do sistema devem ter APIs correspondentes para consumo pelo frontend SPA.

### 5.2 Importação/Exportação de Arquivos

| Tipo | Formatos | Descrição |
|------|----------|-----------|
| Funcionários | Excel, CSV | Carga/atualização em lote |
| Averbações | Excel, CSV | Importação de contratos |
| Retorno Folha | TXT posicional, CSV | Descontos efetivados |
| Margem | TXT posicional, CSV, Excel | Exportação para consignatárias |
| Remessa | TXT posicional, CSV | Averbações para folha |

### 5.3 Webhooks (Enterprise)

| Evento | Payload |
|--------|---------|
| averbacao.criada | ID, funcionário, valor, parcelas |
| averbacao.aprovada | ID, data aprovação |
| averbacao.rejeitada | ID, motivo |
| averbacao.liquidada | ID, data liquidação |
| saldo_devedor.solicitado | ID averbação, consignatária solicitante |

### 5.4 Email (SMTP)

| Evento | Template |
|--------|----------|
| Bem-vindo | Novo usuário cadastrado |
| Recuperar Senha | Link de reset |
| Averbação Pendente | Nova averbação para aprovar |
| Averbação Aprovada | Confirmação de aprovação |
| Averbação Rejeitada | Notificação com motivo |
| Saldo Devedor Solicitado | Solicitação de portabilidade |

---

## 6. Escopo MVP vs Enterprise

### 6.1 MVP - Minimum Viable Product

O MVP contempla as funcionalidades essenciais para operação de um órgão público com suas consignatárias conveniadas:

#### Módulos Incluídos no MVP:

| Módulo | Funcionalidades Principais |
|--------|---------------------------|
| **Autenticação** | Login JWT, recuperação de senha, bloqueio por tentativas |
| **Funcionários** | CRUD completo, gestão de margem, histórico |
| **Averbações** | Fluxo completo (criar → aprovar → enviar → descontar → liquidar) |
| **Simulações** | Empréstimo e compra de dívida |
| **Conciliação** | Processo básico de conciliação mensal |
| **Import/Export** | Funcionários, contratos, retorno folha, remessa |
| **Consignatárias** | Cadastro e gestão |
| **Relatórios** | Relatórios essenciais + dashboard básico |
| **Usuários** | Gestão de usuários e perfis |
| **Auditoria** | Trilha de auditoria completa |
| **Configurações** | Parâmetros básicos |

#### Características Técnicas MVP:

- Web responsivo (desktop/tablet)
- Single database multi-tenant
- JWT authentication
- REST APIs
- LGPD compliance básico
- Email notifications (SMTP)

### 6.2 Enterprise - Recursos Avançados

| Categoria | Recursos |
|-----------|----------|
| **Segurança** | 2FA, SSO, criptografia em repouso, WAF |
| **Mobile** | Aplicativos nativos iOS/Android |
| **Integrações** | Webhooks, APIs avançadas |
| **Compliance** | TCE, auditorias externas |
| **Automação** | Agendamento de tarefas, workflows configuráveis |
| **Customização** | White-label, temas personalizados |
| **Suporte** | SLA diferenciado, suporte prioritário |

---

## 7. User Experience

### 7.1 Princípios de Design

1. **Clareza:** Interface limpa, sem elementos desnecessários
2. **Eficiência:** Tarefas frequentes com mínimo de cliques
3. **Consistência:** Padrões visuais e de interação uniformes
4. **Feedback:** Resposta clara para cada ação do usuário
5. **Prevenção de erros:** Validações em tempo real, confirmações para ações destrutivas

### 7.2 Fluxos Principais

#### 7.2.1 Fluxo: Criar Nova Averbação

```
1. Operador busca funcionário (CPF ou nome)
2. Sistema exibe dados e margem disponível
3. Operador clica em "Nova Averbação"
4. Sistema exibe formulário com campos obrigatórios
5. Operador preenche: tipo, valor, parcelas, data início
6. Sistema calcula e exibe parcela automaticamente
7. Operador confirma e envia para aprovação
8. Sistema notifica aprovador por email
9. Aprovador acessa lista de pendências
10. Aprovador revisa e aprova/rejeita
11. Sistema atualiza status e notifica operador
```

#### 7.2.2 Fluxo: Conciliação Mensal

```
1. Administrador acessa módulo de conciliação
2. Seleciona competência (mês/ano)
3. Importa arquivo de retorno da folha
4. Sistema processa e identifica divergências
5. Sistema exibe resumo: conformes, divergentes, não descontados
6. Administrador trata exceções manualmente
7. Administrador fecha competência
8. Sistema bloqueia alterações na competência fechada
```

#### 7.2.3 Fluxo: Portabilidade de Dívida

```
1. Consignatária cessionária consulta funcionário
2. Identifica averbação ativa de outra consignatária
3. Solicita saldo devedor pelo sistema
4. Consignatária cedente recebe notificação
5. Cedente informa saldo devedor
6. Cessionária cria nova averbação tipo "Compra de Dívida"
7. Vincula à averbação original
8. Após aprovação, averbação original é liquidada
9. Nova averbação assume lugar
```

### 7.3 Wireframes de Referência

*Nota: Wireframes detalhados serão desenvolvidos na fase de design.*

#### Telas Principais:

1. **Login:** Simples, logo centralizado, campos de usuário/senha
2. **Dashboard:** Cards com KPIs, gráficos, lista de pendências
3. **Lista de Funcionários:** Tabela com filtros, busca, paginação
4. **Ficha do Funcionário:** Dados pessoais, margem, lista de averbações
5. **Lista de Averbações:** Filtros avançados, ações em lote
6. **Formulário de Averbação:** Wizard de 3 passos
7. **Simulador:** Interface interativa com resultado em tempo real
8. **Relatórios:** Filtros + visualização + exportação

---

## 8. Métricas de Sucesso

### 8.1 KPIs de Produto

| Métrica | Definição | Meta MVP | Meta Enterprise |
|---------|-----------|----------|-----------------|
| **Taxa de Adoção** | % usuários ativos / usuários cadastrados | > 70% | > 85% |
| **Tempo Médio de Aprovação** | Horas entre criação e aprovação | < 24h | < 8h |
| **Taxa de Erro em Importação** | % linhas com erro / total importado | < 5% | < 2% |
| **Satisfação do Usuário** | NPS dos usuários | > 30 | > 50 |
| **Uptime** | Disponibilidade do sistema | 99.5% | 99.9% |

### 8.2 KPIs de Negócio

| Métrica | Definição |
|---------|-----------|
| **Clientes Ativos** | Número de tenants em produção |
| **Volume Transacionado** | Valor total de averbações processadas |
| **Receita Recorrente** | MRR/ARR por cliente |
| **Churn** | Taxa de cancelamento de clientes |
| **CAC** | Custo de aquisição de cliente |
| **LTV** | Lifetime value do cliente |

---

## 9. Considerações Técnicas

### 9.1 Recomendações de Arquitetura (2026)

*Nota: Estas são recomendações, a decisão final é da equipe técnica.*

#### Backend:
- **.NET 8+** ou **Node.js 22+** para APIs
- **Entity Framework Core** ou **Prisma** para ORM
- **PostgreSQL** como banco de dados principal
- **Redis** para cache e filas
- **RabbitMQ** ou **Amazon SQS** para mensageria

#### Frontend:
- **React 19+** ou **Vue 4+** ou **Angular 19+**
- **TypeScript** obrigatório
- **Tailwind CSS** ou **MUI** para UI
- **React Query** ou **SWR** para data fetching

#### Infraestrutura:
- **Docker** e **Kubernetes** para containerização
- **CI/CD** com GitHub Actions ou GitLab CI
- **Terraform** para IaC
- Cloud-agnostic (AWS, Azure, GCP)

### 9.2 Multi-Tenancy

Implementar multi-tenancy com:
- Tenant ID em todas as tabelas de dados
- Middleware de identificação de tenant (subdomain ou header)
- Isolamento de dados garantido em nível de query
- Possibilidade futura de database sharding por tenant

### 9.3 Migração do Legado

Como este é um projeto de "ressurreição" (não migração de dados):

- Não há necessidade de ferramentas de migração de dados
- Sistema legado serve apenas como referência funcional
- Banco de dados será criado do zero
- Usuários serão recadastrados

---

## 10. Riscos e Mitigações

| Risco | Probabilidade | Impacto | Mitigação |
|-------|---------------|---------|-----------|
| Escopo do MVP muito grande | Alta | Alto | Priorização rigorosa, entregas incrementais |
| Requisitos regulatórios não mapeados | Média | Alto | Consultoria jurídica, validação com clientes |
| Performance com grande volume | Média | Médio | Testes de carga desde o início |
| Resistência à mudança dos usuários | Média | Médio | Treinamento, UX intuitiva, migração gradual |
| Integração com sistemas legados dos clientes | Alta | Médio | APIs flexíveis, múltiplos formatos de arquivo |

---

## 11. Roadmap Sugerido

### Fase 1: Fundação (MVP Core)
- Autenticação e autorização
- Gestão de funcionários e margem
- Averbações (fluxo básico)
- Usuários e perfis

### Fase 2: MVP Completo
- Simulações
- Conciliação
- Importação/Exportação
- Relatórios básicos
- Dashboard

### Fase 3: Consolidação
- Relatórios avançados
- Melhorias de UX baseadas em feedback
- Otimizações de performance
- Documentação e treinamento

### Fase 4: Enterprise
- 2FA e SSO
- Aplicativo mobile
- Webhooks
- Compliance TCE
- White-label

---

## 12. Glossário

| Termo | Definição |
|-------|-----------|
| **Averbação** | Registro de autorização de desconto em folha para empréstimo consignado |
| **Consignante** | Órgão público que processa a folha de pagamento e efetua os descontos |
| **Consignatária** | Instituição financeira que concede o empréstimo |
| **Margem Consignável** | Percentual do salário que pode ser comprometido com consignados |
| **Competência** | Mês/ano de referência para processamento da folha |
| **Conciliação** | Processo de comparação entre averbações enviadas e descontos efetivados |
| **Portabilidade** | Transferência de dívida de uma consignatária para outra |
| **Refinanciamento** | Liquidação de empréstimo existente e criação de novo com condições diferentes |
| **Compra de Dívida** | Quando uma consignatária quita dívida do cliente em outra instituição |
| **Termo de Averbação** | Documento que formaliza a autorização de desconto |

---

## 13. Anexos

### Anexo A: Mapa de Permissões Detalhado

*Ver documento separado: permissoes-detalhadas.md*

### Anexo B: Layouts de Importação/Exportação

*Ver documento separado: layouts-arquivos.md*

### Anexo C: Regras de Negócio Detalhadas

*Ver documento separado: regras-negocio.md*

---

## Histórico de Revisões

| Versão | Data | Autor | Descrição |
|--------|------|-------|-----------|
| 1.0 | Janeiro 2026 | Product Team | Versão inicial do PRD |

---

*Este documento é uma referência viva e deve ser atualizado conforme o produto evolui.*
