# User Stories - FastConsig

**Versao:** 1.0
**Data:** Janeiro 2026
**Status:** Draft

---

## 1. Visao Geral

Este documento contem as User Stories do sistema FastConsig, organizadas por Epicos. Cada story segue o formato padrao e inclui criterios de aceitacao e referencias as regras de negocio.

### 1.1 Legenda de Prioridade

| Tag | Descricao |
|-----|-----------|
| **MVP** | Funcionalidade essencial para lancamento inicial |
| **Enterprise** | Funcionalidade avancada para versao enterprise |

### 1.2 Personas

| Persona | Descricao |
|---------|-----------|
| **Administrador Consignante** | Gestor do orgao publico |
| **Operador Consignante** | Funcionario do RH/DP |
| **Aprovador** | Responsavel por aprovacoes |
| **Administrador Consignataria** | Gestor da instituicao financeira |
| **Operador Consignataria** | Funcionario da consignataria |
| **Agente** | Correspondente bancario |
| **Sistema** | Acoes automaticas |

---

## 2. Epicos

| ID | Epico | Descricao |
|----|-------|-----------|
| EP-01 | Autenticacao e Seguranca | Login, logout, recuperacao de senha, 2FA |
| EP-02 | Gestao de Funcionarios | CRUD de servidores, situacoes, historico |
| EP-03 | Gestao de Margem | Calculo, reserva, liberacao de margem |
| EP-04 | Averbacoes | Ciclo de vida completo de emprestimos |
| EP-05 | Fluxo de Aprovacao | Aprovacao, rejeicao, workflows |
| EP-06 | Simulacoes | Simulador de emprestimo e compra de divida |
| EP-07 | Conciliacao | Processo de conciliacao mensal |
| EP-08 | Importacao/Exportacao | Arquivos de integracao |
| EP-09 | Consignatarias e Produtos | Gestao de conveniados |
| EP-10 | Relatorios e Dashboards | Visualizacao de dados |
| EP-11 | Usuarios e Permissoes | Controle de acesso |
| EP-12 | Configuracoes | Parametros do sistema |
| EP-13 | Auditoria | Logs e rastreabilidade |
| EP-14 | Notificacoes | Emails, mensagens, webhooks |

---

## 3. User Stories por Epico

---

## EP-01: Autenticacao e Seguranca

### US-001: Login no Sistema

**Epico:** EP-01
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario do sistema,
Quero fazer login com meu usuario e senha,
Para que eu possa acessar as funcionalidades do FastConsig.

**Criterios de Aceitacao:**
- [ ] Formulario com campos usuario e senha
- [ ] Validacao de credenciais no backend
- [ ] Geracao de token JWT apos autenticacao bem-sucedida
- [ ] Mensagem de erro clara para credenciais invalidas
- [ ] Redirecionamento para dashboard apos login
- [ ] Campo de senha com opcao de visualizar/ocultar

**Regras de Negocio Relacionadas:**
- RN-VAL-001: Validacao de credenciais

---

### US-002: Bloqueio por Tentativas Invalidas

**Epico:** EP-01
**Prioridade:** MVP
**Persona:** Sistema

**Historia:**
Como sistema,
Quero bloquear usuarios apos tentativas de login invalidas,
Para que eu proteja o sistema contra ataques de forca bruta.

**Criterios de Aceitacao:**
- [ ] Contador de tentativas invalidas por usuario
- [ ] Bloqueio apos 5 tentativas (configuravel)
- [ ] Duracao do bloqueio de 30 minutos (configuravel)
- [ ] Mensagem informando bloqueio e tempo restante
- [ ] Registro em log de auditoria
- [ ] Desbloqueio manual por administrador

**Regras de Negocio Relacionadas:**
- RN-VAL-002: Politica de bloqueio

---

### US-003: Recuperacao de Senha

**Epico:** EP-01
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario,
Quero recuperar minha senha via email,
Para que eu possa acessar o sistema caso esqueca minha senha.

**Criterios de Aceitacao:**
- [ ] Link "Esqueci minha senha" na tela de login
- [ ] Formulario solicitando email cadastrado
- [ ] Envio de email com link de recuperacao (token expiravel)
- [ ] Token valido por 2 horas
- [ ] Formulario para definir nova senha
- [ ] Validacao de complexidade da nova senha
- [ ] Confirmacao de alteracao por email

---

### US-004: Logout do Sistema

**Epico:** EP-01
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario autenticado,
Quero fazer logout do sistema,
Para que eu encerre minha sessao de forma segura.

**Criterios de Aceitacao:**
- [ ] Botao de logout visivel no header
- [ ] Invalidacao do token JWT no backend
- [ ] Limpeza de dados de sessao no frontend
- [ ] Redirecionamento para tela de login
- [ ] Confirmacao de logout para operacoes em andamento

---

### US-005: Alterar Minha Senha

**Epico:** EP-01
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario autenticado,
Quero alterar minha senha,
Para que eu mantenha a seguranca da minha conta.

**Criterios de Aceitacao:**
- [ ] Opcao em "Meus Dados" ou menu do usuario
- [ ] Solicitar senha atual para confirmacao
- [ ] Campos para nova senha e confirmacao
- [ ] Validacao de complexidade (minimo 8 caracteres, letras e numeros)
- [ ] Nova senha nao pode ser igual as 5 ultimas
- [ ] Mensagem de sucesso apos alteracao

---

### US-006: Primeiro Acesso com Troca Obrigatoria

**Epico:** EP-01
**Prioridade:** MVP
**Persona:** Novos usuarios

**Historia:**
Como novo usuario,
Quero ser obrigado a trocar minha senha no primeiro acesso,
Para que eu tenha uma senha pessoal e segura.

**Criterios de Aceitacao:**
- [ ] Flag "primeiro_acesso" no cadastro do usuario
- [ ] Redirecionamento automatico para tela de troca de senha
- [ ] Impossibilidade de navegar sem trocar a senha
- [ ] Remocao da flag apos troca bem-sucedida

---

### US-007: Autenticacao de Dois Fatores (2FA)

**Epico:** EP-01
**Prioridade:** Enterprise
**Persona:** Todos os usuarios

**Historia:**
Como usuario,
Quero habilitar autenticacao de dois fatores,
Para que minha conta tenha uma camada extra de seguranca.

**Criterios de Aceitacao:**
- [ ] Opcao de habilitar 2FA nas configuracoes do usuario
- [ ] Geracao de QR Code para apps autenticadores (Google Authenticator, Authy)
- [ ] Validacao do codigo TOTP no login
- [ ] Codigos de backup para recuperacao
- [ ] Opcao de desabilitar 2FA (com confirmacao)

---

### US-008: Single Sign-On (SSO)

**Epico:** EP-01
**Prioridade:** Enterprise
**Persona:** Administrador Consignante

**Historia:**
Como administrador,
Quero integrar o login com o Active Directory do orgao,
Para que os usuarios usem suas credenciais corporativas.

**Criterios de Aceitacao:**
- [ ] Configuracao de provedor SAML/OIDC
- [ ] Mapeamento de grupos AD para perfis do sistema
- [ ] Login transparente para usuarios do dominio
- [ ] Fallback para login local quando SSO indisponivel

---

## EP-02: Gestao de Funcionarios

### US-009: Cadastrar Funcionario

**Epico:** EP-02
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero cadastrar um novo funcionario,
Para que ele possa ser elegivel para averbacoes.

**Criterios de Aceitacao:**
- [ ] Formulario com dados pessoais (CPF, nome, data nascimento, sexo)
- [ ] Dados funcionais (matricula, empresa, cargo, data admissao, salario)
- [ ] Dados bancarios (banco, agencia, conta)
- [ ] Validacao de CPF (digitos verificadores)
- [ ] Validacao de matricula unica por empresa
- [ ] Calculo automatico de margem apos salvar
- [ ] Mensagem de sucesso com link para visualizar

**Regras de Negocio Relacionadas:**
- RN-FUN-001: Cadastro de funcionario
- RN-FUN-002: Unicidade de matricula
- RN-MAR-001: Calculo de margem

---

### US-010: Editar Funcionario

**Epico:** EP-02
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero editar os dados de um funcionario,
Para que eu mantenha o cadastro atualizado.

**Criterios de Aceitacao:**
- [ ] Acesso via lista de funcionarios ou busca
- [ ] Formulario pre-preenchido com dados atuais
- [ ] Registro de alteracao no historico
- [ ] Recalculo de margem se salario alterado
- [ ] Validacao de impacto em averbacoes ativas

**Regras de Negocio Relacionadas:**
- RN-FUN-003: Alteracao cadastral
- RN-EXC-006: Impacto em averbacoes

---

### US-011: Buscar Funcionario

**Epico:** EP-02
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario,
Quero buscar funcionarios por diferentes criterios,
Para que eu encontre rapidamente o servidor desejado.

**Criterios de Aceitacao:**
- [ ] Busca por CPF (parcial ou completo)
- [ ] Busca por nome (parcial, case insensitive)
- [ ] Busca por matricula
- [ ] Filtros por empresa, situacao
- [ ] Resultado em lista paginada
- [ ] Acesso rapido a ficha do funcionario

---

### US-012: Visualizar Ficha do Funcionario

**Epico:** EP-02
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario,
Quero visualizar a ficha completa de um funcionario,
Para que eu veja todos os dados e averbacoes.

**Criterios de Aceitacao:**
- [ ] Dados pessoais e funcionais
- [ ] Foto do funcionario (se disponivel)
- [ ] Resumo de margem (total, utilizada, disponivel)
- [ ] Lista de averbacoes ativas
- [ ] Historico de averbacoes encerradas
- [ ] Botoes de acao (editar, nova averbacao, aposentar)

---

### US-013: Alterar Situacao do Funcionario

**Epico:** EP-02
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero alterar a situacao de um funcionario,
Para que reflita sua condicao atual (ativo, afastado, etc).

**Criterios de Aceitacao:**
- [ ] Opcoes: Ativo, Inativo, Afastado, Bloqueado, Aposentado
- [ ] Motivo obrigatorio para alteracao
- [ ] Data de vigencia da alteracao
- [ ] Impacto automatico em averbacoes (suspensao se inativo/afastado)
- [ ] Registro em historico

**Regras de Negocio Relacionadas:**
- RN-FUN-004 a RN-FUN-008: Situacoes do funcionario

---

### US-014: Aposentar Funcionario

**Epico:** EP-02
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero executar o processo de aposentadoria de um funcionario,
Para que suas averbacoes sejam tratadas adequadamente.

**Criterios de Aceitacao:**
- [ ] Wizard de aposentadoria com etapas
- [ ] Informar novo salario (beneficio)
- [ ] Recalculo de margem com novo salario
- [ ] Analise de impacto em averbacoes ativas
- [ ] Opcao de transferir para instituto de previdencia
- [ ] Geracao de termo de aposentadoria
- [ ] Notificacao as consignatarias afetadas

**Regras de Negocio Relacionadas:**
- RN-EXC-002: Aposentadoria

---

### US-015: Visualizar Historico do Funcionario

**Epico:** EP-02
**Prioridade:** MVP
**Persona:** Operador Consignante, Auditor

**Historia:**
Como operador ou auditor,
Quero visualizar o historico de alteracoes de um funcionario,
Para que eu acompanhe a evolucao cadastral.

**Criterios de Aceitacao:**
- [ ] Lista cronologica de alteracoes
- [ ] Campos alterados com valor anterior e novo
- [ ] Usuario e data/hora da alteracao
- [ ] Filtro por periodo
- [ ] Exportacao do historico

---

### US-016: Gerenciar Autorizacoes de Desconto

**Epico:** EP-02
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero gerenciar as autorizacoes de desconto de um funcionario,
Para que eu controle quais consignatarias podem averbar.

**Criterios de Aceitacao:**
- [ ] Lista de autorizacoes vigentes
- [ ] Adicionar nova autorizacao (consignataria, vigencia)
- [ ] Revogar autorizacao existente (com motivo)
- [ ] Historico de autorizacoes
- [ ] Validacao ao criar averbacao

**Regras de Negocio Relacionadas:**
- RN-FUN-010: Autorizacao especial

---

## EP-03: Gestao de Margem

### US-017: Consultar Margem Disponivel

**Epico:** EP-03
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario,
Quero consultar a margem disponivel de um funcionario,
Para que eu saiba quanto ele pode comprometer em averbacoes.

**Criterios de Aceitacao:**
- [ ] Exibir margem total, utilizada e disponivel
- [ ] Detalhar por tipo de produto (emprestimo, cartao)
- [ ] Mostrar percentuais aplicados
- [ ] Indicar reservas em andamento
- [ ] Atualizar em tempo real

**Regras de Negocio Relacionadas:**
- RN-MAR-001 a RN-MAR-005: Calculo de margem

---

### US-018: Visualizar Composicao da Margem

**Epico:** EP-03
**Prioridade:** MVP
**Persona:** Operador Consignante, Consignataria

**Historia:**
Como usuario,
Quero ver a composicao detalhada da margem utilizada,
Para que eu entenda quais averbacoes comprometem a margem.

**Criterios de Aceitacao:**
- [ ] Lista de averbacoes ativas com valor de parcela
- [ ] Agrupamento por consignataria
- [ ] Soma por tipo de produto
- [ ] Indicacao de reservas pendentes
- [ ] Data prevista de liberacao (para cada averbacao)

---

### US-019: Simular Margem Futura

**Epico:** EP-03
**Prioridade:** MVP
**Persona:** Operador Consignante, Consignataria

**Historia:**
Como usuario,
Quero simular a margem futura de um funcionario,
Para que eu projete a disponibilidade em meses futuros.

**Criterios de Aceitacao:**
- [ ] Selecionar mes/ano futuro
- [ ] Calcular averbacoes que estarao liquidadas
- [ ] Projetar margem disponivel na data
- [ ] Considerar reajustes programados

---

### US-020: Exportar Margem

**Epico:** EP-03
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero exportar a posicao de margem dos funcionarios,
Para que eu disponibilize as consignatarias conveniadas.

**Criterios de Aceitacao:**
- [ ] Selecionar filtros (empresa, situacao)
- [ ] Escolher formato (Excel, CSV, TXT)
- [ ] Escolher layout (padrao ou personalizado)
- [ ] Gerar arquivo para download
- [ ] Registrar exportacao no historico

---

## EP-04: Averbacoes

### US-021: Criar Nova Averbacao

**Epico:** EP-04
**Prioridade:** MVP
**Persona:** Operador Consignataria, Agente

**Historia:**
Como operador da consignataria,
Quero registrar uma nova averbacao,
Para que o emprestimo seja processado na folha.

**Criterios de Aceitacao:**
- [ ] Selecionar funcionario (busca por CPF/nome)
- [ ] Verificar margem disponivel
- [ ] Informar: produto, valor, parcelas, taxa, data inicio
- [ ] Calcular valor da parcela automaticamente
- [ ] Validar contra margem disponivel
- [ ] Reservar margem ao salvar
- [ ] Enviar para aprovacao
- [ ] Gerar protocolo da averbacao

**Regras de Negocio Relacionadas:**
- RN-AVE-001 a RN-AVE-005: Criacao de averbacao
- RN-MAR-006: Reserva de margem

---

### US-022: Criar Refinanciamento

**Epico:** EP-04
**Prioridade:** MVP
**Persona:** Operador Consignataria

**Historia:**
Como operador da consignataria,
Quero criar um refinanciamento,
Para que o cliente renove seu emprestimo com novas condicoes.

**Criterios de Aceitacao:**
- [ ] Selecionar contrato ativo a refinanciar
- [ ] Exibir saldo devedor atual
- [ ] Informar novas condicoes (valor, parcelas, taxa)
- [ ] Calcular troco ao cliente (se houver)
- [ ] Vincular ao contrato original
- [ ] Liquidar contrato original apos aprovacao

**Regras de Negocio Relacionadas:**
- RN-AVE-006: Refinanciamento

---

### US-023: Criar Compra de Divida

**Epico:** EP-04
**Prioridade:** MVP
**Persona:** Operador Consignataria

**Historia:**
Como operador da consignataria,
Quero criar uma compra de divida (portabilidade),
Para que meu banco assuma o contrato de outra instituicao.

**Criterios de Aceitacao:**
- [ ] Identificar contratos ativos de outras consignatarias
- [ ] Solicitar saldo devedor (se nao informado)
- [ ] Informar valor de compra e novas condicoes
- [ ] Calcular troco ao cliente
- [ ] Aguardar confirmacao de quitacao pela cedente
- [ ] Assumir contrato apos quitacao confirmada

**Regras de Negocio Relacionadas:**
- RN-POR-001 a RN-POR-009: Portabilidade

---

### US-024: Visualizar Averbacao

**Epico:** EP-04
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario,
Quero visualizar os detalhes de uma averbacao,
Para que eu acompanhe o status e informacoes do contrato.

**Criterios de Aceitacao:**
- [ ] Dados do funcionario
- [ ] Dados do contrato (valores, prazos, taxas)
- [ ] Status atual e historico de status
- [ ] Parcelas pagas e a vencer
- [ ] Contratos vinculados (se houver)
- [ ] Documentos anexados
- [ ] Acoes disponiveis conforme status

---

### US-025: Cancelar Averbacao

**Epico:** EP-04
**Prioridade:** MVP
**Persona:** Operador Consignataria, Administrador Consignante

**Historia:**
Como operador,
Quero cancelar uma averbacao,
Para que ela deixe de ser processada na folha.

**Criterios de Aceitacao:**
- [ ] Informar motivo do cancelamento
- [ ] Validar se status permite cancelamento
- [ ] Liberar margem reservada/utilizada
- [ ] Registrar usuario e data do cancelamento
- [ ] Notificar partes interessadas

**Regras de Negocio Relacionadas:**
- RN-AVE-020: Cancelamento

---

### US-026: Suspender Averbacao

**Epico:** EP-04
**Prioridade:** MVP
**Persona:** Aprovador, Administrador

**Historia:**
Como aprovador ou administrador,
Quero suspender temporariamente uma averbacao,
Para que os descontos sejam interrompidos por um periodo.

**Criterios de Aceitacao:**
- [ ] Informar motivo da suspensao
- [ ] Informar previsao de retorno (opcional)
- [ ] Manter margem reservada
- [ ] Nao gerar parcela durante suspensao
- [ ] Permitir reativacao posterior

**Regras de Negocio Relacionadas:**
- RN-AVE-015: Suspensao

---

### US-027: Liquidar Averbacao

**Epico:** EP-04
**Prioridade:** MVP
**Persona:** Operador Consignataria

**Historia:**
Como operador da consignataria,
Quero registrar a liquidacao de uma averbacao,
Para que o contrato seja encerrado por quitacao.

**Criterios de Aceitacao:**
- [ ] Informar data da liquidacao
- [ ] Informar valor quitado (opcional)
- [ ] Alterar status para Liquidada
- [ ] Liberar margem imediatamente
- [ ] Registrar no historico

**Regras de Negocio Relacionadas:**
- RN-AVE-018: Liquidacao

---

### US-028: Gerar Termo de Averbacao

**Epico:** EP-04
**Prioridade:** MVP
**Persona:** Operador Consignataria, Agente

**Historia:**
Como operador,
Quero gerar o termo de averbacao em PDF,
Para que o cliente assine a autorizacao de desconto.

**Criterios de Aceitacao:**
- [ ] Template configurado por consignataria
- [ ] Dados do funcionario preenchidos
- [ ] Dados do contrato preenchidos
- [ ] Clausulas legais obrigatorias
- [ ] Geracao em PDF para impressao
- [ ] Registro de geracao no historico

---

## EP-05: Fluxo de Aprovacao

### US-029: Listar Averbacoes Pendentes

**Epico:** EP-05
**Prioridade:** MVP
**Persona:** Aprovador

**Historia:**
Como aprovador,
Quero ver a lista de averbacoes pendentes de aprovacao,
Para que eu possa analisar e decidir sobre cada uma.

**Criterios de Aceitacao:**
- [ ] Lista filtrada por status "Aguardando Aprovacao"
- [ ] Ordenacao por data de criacao (mais antigas primeiro)
- [ ] Indicador de urgencia (prazo proximo)
- [ ] Resumo: funcionario, consignataria, valor, parcela
- [ ] Acesso rapido aos detalhes

---

### US-030: Aprovar Averbacao

**Epico:** EP-05
**Prioridade:** MVP
**Persona:** Aprovador

**Historia:**
Como aprovador,
Quero aprovar uma averbacao,
Para que ela possa ser enviada para desconto em folha.

**Criterios de Aceitacao:**
- [ ] Revisar dados do funcionario e contrato
- [ ] Verificar margem disponivel
- [ ] Botao "Aprovar" com confirmacao
- [ ] Alterar status para "Aprovada"
- [ ] Notificar consignataria por email
- [ ] Registrar aprovador e data/hora

**Regras de Negocio Relacionadas:**
- RN-APR-001: Aprovacao

---

### US-031: Rejeitar Averbacao

**Epico:** EP-05
**Prioridade:** MVP
**Persona:** Aprovador

**Historia:**
Como aprovador,
Quero rejeitar uma averbacao,
Para que contratos irregulares nao sejam processados.

**Criterios de Aceitacao:**
- [ ] Motivo de rejeicao obrigatorio
- [ ] Lista de motivos pre-definidos + texto livre
- [ ] Alterar status para "Rejeitada"
- [ ] Liberar margem reservada
- [ ] Notificar consignataria com motivo
- [ ] Registrar aprovador e data/hora

**Regras de Negocio Relacionadas:**
- RN-APR-002: Rejeicao

---

### US-032: Aprovar em Lote

**Epico:** EP-05
**Prioridade:** MVP
**Persona:** Aprovador

**Historia:**
Como aprovador,
Quero aprovar multiplas averbacoes de uma vez,
Para que eu agilize o processo de aprovacao.

**Criterios de Aceitacao:**
- [ ] Selecionar multiplas averbacoes na lista
- [ ] Botao "Aprovar Selecionadas"
- [ ] Confirmacao com quantidade
- [ ] Processamento em lote
- [ ] Relatorio de resultado (sucesso/erro por item)

---

### US-033: Solicitar Informacoes Adicionais

**Epico:** EP-05
**Prioridade:** MVP
**Persona:** Aprovador

**Historia:**
Como aprovador,
Quero solicitar informacoes adicionais sobre uma averbacao,
Para que eu tome uma decisao informada.

**Criterios de Aceitacao:**
- [ ] Campo para descrever informacao necessaria
- [ ] Alterar status para "Aguardando Informacao"
- [ ] Notificar consignataria
- [ ] Permitir resposta pela consignataria
- [ ] Retornar para analise apos resposta

---

## EP-06: Simulacoes

### US-034: Simular Emprestimo por Valor

**Epico:** EP-06
**Prioridade:** MVP
**Persona:** Operador Consignataria, Agente

**Historia:**
Como operador,
Quero simular um emprestimo informando o valor desejado,
Para que eu calcule as condicoes de pagamento.

**Criterios de Aceitacao:**
- [ ] Informar valor liquido desejado
- [ ] Selecionar produto/tabela de coeficientes
- [ ] Calcular para multiplos prazos
- [ ] Exibir: valor total, parcela, taxa, CET, IOF
- [ ] Destacar opcoes dentro da margem disponivel
- [ ] Opcao de imprimir/exportar simulacao

**Regras de Negocio Relacionadas:**
- RN-SIM-001 a RN-SIM-003: Simulacao de emprestimo

---

### US-035: Simular Emprestimo por Parcela

**Epico:** EP-06
**Prioridade:** MVP
**Persona:** Operador Consignataria, Agente

**Historia:**
Como operador,
Quero simular um emprestimo informando o valor da parcela,
Para que eu calcule quanto o cliente pode obter.

**Criterios de Aceitacao:**
- [ ] Informar valor da parcela desejada
- [ ] Validar contra margem disponivel
- [ ] Calcular valor liquido para multiplos prazos
- [ ] Exibir: valor total, valor liquido, taxa, CET
- [ ] Destacar melhor opcao

---

### US-036: Simular Compra de Divida

**Epico:** EP-06
**Prioridade:** MVP
**Persona:** Operador Consignataria

**Historia:**
Como operador,
Quero simular uma compra de divida,
Para que eu apresente proposta de portabilidade ao cliente.

**Criterios de Aceitacao:**
- [ ] Informar saldo devedor a quitar
- [ ] Calcular novo emprestimo consolidado
- [ ] Exibir troco ao cliente (se houver)
- [ ] Comparar parcela atual vs nova parcela
- [ ] Mostrar economia mensal
- [ ] Permitir multiplas dividas

**Regras de Negocio Relacionadas:**
- RN-SIM-004 a RN-SIM-006: Simulacao de compra

---

### US-037: Gerenciar Tabelas de Coeficientes

**Epico:** EP-06
**Prioridade:** MVP
**Persona:** Administrador Consignataria

**Historia:**
Como administrador da consignataria,
Quero gerenciar as tabelas de coeficientes,
Para que as simulacoes usem as taxas vigentes.

**Criterios de Aceitacao:**
- [ ] Criar nova tabela com nome e vigencia
- [ ] Informar coeficientes por prazo
- [ ] Importar coeficientes via Excel
- [ ] Ativar/desativar tabelas
- [ ] Historico de versoes

---

### US-038: Imprimir Simulacao

**Epico:** EP-06
**Prioridade:** MVP
**Persona:** Operador Consignataria, Agente

**Historia:**
Como operador,
Quero imprimir ou exportar uma simulacao,
Para que eu entregue ao cliente como proposta.

**Criterios de Aceitacao:**
- [ ] Geracao de PDF formatado
- [ ] Logo da consignataria
- [ ] Dados do cliente e simulacao
- [ ] Validade da proposta
- [ ] Informacoes legais obrigatorias

---

## EP-07: Conciliacao

### US-039: Importar Retorno da Folha

**Epico:** EP-07
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero importar o arquivo de retorno da folha,
Para que o sistema saiba o que foi efetivamente descontado.

**Criterios de Aceitacao:**
- [ ] Upload de arquivo (TXT, CSV, Excel)
- [ ] Selecionar competencia
- [ ] Validacao do formato
- [ ] Processamento do arquivo
- [ ] Relatorio de importacao (processados, erros)

**Regras de Negocio Relacionadas:**
- RN-CON-001: Processo de conciliacao

---

### US-040: Executar Conciliacao

**Epico:** EP-07
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero executar a conciliacao mensal,
Para que eu identifique divergencias entre enviado e descontado.

**Criterios de Aceitacao:**
- [ ] Selecionar competencia
- [ ] Comparar averbacoes enviadas vs retorno
- [ ] Classificar: Conforme, Nao Descontado, Divergente
- [ ] Exibir resumo por consignataria
- [ ] Permitir drill-down para detalhes

**Regras de Negocio Relacionadas:**
- RN-CON-002 a RN-CON-004: Tipos de conciliacao

---

### US-041: Tratar Divergencias

**Epico:** EP-07
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero tratar as divergencias identificadas,
Para que eu resolva as inconsistencias manualmente.

**Criterios de Aceitacao:**
- [ ] Lista de divergencias pendentes
- [ ] Opcoes de tratamento por tipo
- [ ] Justificativa obrigatoria
- [ ] Ajuste de valor quando aplicavel
- [ ] Registro de tratamento

---

### US-042: Fechar Competencia

**Epico:** EP-07
**Prioridade:** MVP
**Persona:** Administrador Consignante

**Historia:**
Como administrador do consignante,
Quero fechar uma competencia,
Para que nao sejam feitas mais alteracoes naquele periodo.

**Criterios de Aceitacao:**
- [ ] Verificar se todas divergencias tratadas
- [ ] Confirmacao de fechamento
- [ ] Bloqueio de alteracoes na competencia
- [ ] Geracao de relatorio final
- [ ] Registro de fechamento

---

### US-043: Reabrir Competencia

**Epico:** EP-07
**Prioridade:** MVP
**Persona:** Administrador Consignante

**Historia:**
Como administrador do consignante,
Quero reabrir uma competencia fechada,
Para que eu corrija algum erro identificado posteriormente.

**Criterios de Aceitacao:**
- [ ] Justificativa obrigatoria
- [ ] Aprovacao de nivel superior (se configurado)
- [ ] Desbloqueio de alteracoes
- [ ] Registro de reabertura em auditoria

---

## EP-08: Importacao/Exportacao

### US-044: Importar Funcionarios

**Epico:** EP-08
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero importar funcionarios via arquivo,
Para que eu cadastre em lote a partir do sistema de folha.

**Criterios de Aceitacao:**
- [ ] Upload de arquivo (Excel, CSV)
- [ ] Validacao previa com relatorio de erros
- [ ] Opcao de importar parcial (ignorar erros)
- [ ] Processamento em background para grandes volumes
- [ ] Notificacao ao concluir
- [ ] Log de importacao detalhado

---

### US-045: Importar Contratos

**Epico:** EP-08
**Prioridade:** MVP
**Persona:** Administrador Consignataria

**Historia:**
Como administrador da consignataria,
Quero importar contratos via arquivo,
Para que eu registre averbacoes em lote.

**Criterios de Aceitacao:**
- [ ] Upload de arquivo com layout definido
- [ ] Validacao de funcionarios, margem
- [ ] Relatorio de erros detalhado
- [ ] Contratos importados em status "Aguardando Aprovacao"
- [ ] Log de importacao

---

### US-046: Exportar Remessa para Folha

**Epico:** EP-08
**Prioridade:** MVP
**Persona:** Operador Consignante

**Historia:**
Como operador do consignante,
Quero exportar o arquivo de remessa para a folha,
Para que os descontos sejam processados.

**Criterios de Aceitacao:**
- [ ] Selecionar competencia
- [ ] Filtrar por consignataria (opcional)
- [ ] Escolher formato (TXT, CSV, Excel)
- [ ] Gerar arquivo para download
- [ ] Marcar averbacoes como "Enviadas"
- [ ] Registrar exportacao

---

### US-047: Gerenciar Layouts Personalizados

**Epico:** EP-08
**Prioridade:** Enterprise
**Persona:** Administrador Consignante

**Historia:**
Como administrador,
Quero configurar layouts personalizados de importacao/exportacao,
Para que eu integre com sistemas especificos.

**Criterios de Aceitacao:**
- [ ] Criar novo layout
- [ ] Definir campos e posicoes
- [ ] Configurar delimitadores e encoding
- [ ] Testar layout com arquivo exemplo
- [ ] Ativar/desativar layouts

---

## EP-09: Consignatarias e Produtos

### US-048: Cadastrar Consignataria

**Epico:** EP-09
**Prioridade:** MVP
**Persona:** Administrador Consignante

**Historia:**
Como administrador do consignante,
Quero cadastrar uma nova consignataria,
Para que ela possa operar no convenio.

**Criterios de Aceitacao:**
- [ ] Dados cadastrais (CNPJ, razao social, contatos)
- [ ] Dados bancarios para repasse
- [ ] Vigencia do convenio
- [ ] Limites operacionais
- [ ] Status inicial "Ativa"

---

### US-049: Gerenciar Produtos

**Epico:** EP-09
**Prioridade:** MVP
**Persona:** Administrador Consignante, Administrador Consignataria

**Historia:**
Como administrador,
Quero gerenciar os produtos oferecidos,
Para que eu defina tipos de emprestimo disponiveis.

**Criterios de Aceitacao:**
- [ ] CRUD de produtos
- [ ] Configurar margem especifica por produto
- [ ] Limites de prazo e valor
- [ ] Vincular a consignatarias

---

### US-050: Suspender Consignataria

**Epico:** EP-09
**Prioridade:** MVP
**Persona:** Administrador Consignante

**Historia:**
Como administrador do consignante,
Quero suspender uma consignataria,
Para que ela nao possa criar novas averbacoes.

**Criterios de Aceitacao:**
- [ ] Motivo de suspensao obrigatorio
- [ ] Bloquear novas averbacoes
- [ ] Manter averbacoes existentes ativas
- [ ] Notificar consignataria
- [ ] Permitir reativacao posterior

---

### US-051: Gerenciar Agentes

**Epico:** EP-09
**Prioridade:** MVP
**Persona:** Administrador Consignataria

**Historia:**
Como administrador da consignataria,
Quero gerenciar os agentes vinculados,
Para que eu controle quem pode operar em nome da instituicao.

**Criterios de Aceitacao:**
- [ ] CRUD de agentes
- [ ] Definir permissoes por agente
- [ ] Ativar/desativar agentes
- [ ] Relatorio de producao por agente

---

## EP-10: Relatorios e Dashboards

### US-052: Visualizar Dashboard do Consignante

**Epico:** EP-10
**Prioridade:** MVP
**Persona:** Administrador Consignante

**Historia:**
Como administrador do consignante,
Quero visualizar o dashboard gerencial,
Para que eu acompanhe os indicadores do orgao.

**Criterios de Aceitacao:**
- [ ] Total de servidores ativos
- [ ] Margem total, utilizada, disponivel
- [ ] % de utilizacao de margem
- [ ] Averbacoes pendentes de aprovacao
- [ ] Volume mensal por consignataria (grafico)
- [ ] Evolucao historica (grafico linha)
- [ ] Filtros por periodo e empresa

---

### US-053: Visualizar Dashboard da Consignataria

**Epico:** EP-10
**Prioridade:** MVP
**Persona:** Administrador Consignataria

**Historia:**
Como administrador da consignataria,
Quero visualizar o dashboard de producao,
Para que eu acompanhe o desempenho da instituicao.

**Criterios de Aceitacao:**
- [ ] Producao do mes (valor e quantidade)
- [ ] Carteira ativa (saldo)
- [ ] Taxa media ponderada
- [ ] Ranking de agentes
- [ ] Pipeline de aprovacao
- [ ] Evolucao de producao (grafico)

---

### US-054: Gerar Relatorio de Producao

**Epico:** EP-10
**Prioridade:** MVP
**Persona:** Administrador Consignante, Administrador Consignataria

**Historia:**
Como administrador,
Quero gerar relatorio de producao,
Para que eu analise as averbacoes por periodo.

**Criterios de Aceitacao:**
- [ ] Filtros: periodo, consignataria, produto, status
- [ ] Agrupamento configuravel
- [ ] Totalizadores
- [ ] Exportacao Excel/PDF
- [ ] Visualizacao em tela

---

### US-055: Gerar Relatorio de Inadimplencia

**Epico:** EP-10
**Prioridade:** MVP
**Persona:** Administrador Consignante, Administrador Consignataria

**Historia:**
Como administrador,
Quero gerar relatorio de inadimplencia,
Para que eu identifique averbacoes nao descontadas.

**Criterios de Aceitacao:**
- [ ] Listar averbacoes com status divergente
- [ ] Filtro por competencia
- [ ] Motivos de nao desconto
- [ ] Valor total nao descontado
- [ ] Exportacao Excel/PDF

---

### US-056: Gerar Relatorio de Auditoria

**Epico:** EP-10
**Prioridade:** MVP
**Persona:** Administrador Consignante, Auditor

**Historia:**
Como administrador ou auditor,
Quero gerar relatorio de auditoria,
Para que eu verifique as operacoes realizadas.

**Criterios de Aceitacao:**
- [ ] Filtro por periodo, usuario, tipo de operacao
- [ ] Detalhes: usuario, acao, data/hora, IP
- [ ] Valores anteriores e novos (alteracoes)
- [ ] Exportacao Excel/PDF

---

## EP-11: Usuarios e Permissoes

### US-057: Cadastrar Usuario

**Epico:** EP-11
**Prioridade:** MVP
**Persona:** Administrador

**Historia:**
Como administrador,
Quero cadastrar um novo usuario,
Para que ele tenha acesso ao sistema.

**Criterios de Aceitacao:**
- [ ] Dados: nome, email, login
- [ ] Atribuir perfil de acesso
- [ ] Vincular a consignataria (se aplicavel)
- [ ] Enviar email com senha temporaria
- [ ] Marcar como primeiro acesso

---

### US-058: Gerenciar Perfis de Acesso

**Epico:** EP-11
**Prioridade:** MVP
**Persona:** Administrador Consignante

**Historia:**
Como administrador,
Quero gerenciar os perfis de acesso,
Para que eu controle as permissoes dos usuarios.

**Criterios de Aceitacao:**
- [ ] CRUD de perfis
- [ ] Atribuir/remover permissoes
- [ ] Copiar perfil existente
- [ ] Visualizar usuarios do perfil
- [ ] Impedir exclusao de perfil em uso

---

### US-059: Inativar Usuario

**Epico:** EP-11
**Prioridade:** MVP
**Persona:** Administrador

**Historia:**
Como administrador,
Quero inativar um usuario,
Para que ele perca acesso ao sistema.

**Criterios de Aceitacao:**
- [ ] Alterar status para inativo
- [ ] Encerrar sessoes ativas
- [ ] Invalidar tokens
- [ ] Manter historico de acoes
- [ ] Permitir reativacao

---

### US-060: Resetar Senha de Usuario

**Epico:** EP-11
**Prioridade:** MVP
**Persona:** Administrador

**Historia:**
Como administrador,
Quero resetar a senha de um usuario,
Para que ele possa acessar caso tenha perdido.

**Criterios de Aceitacao:**
- [ ] Gerar nova senha temporaria
- [ ] Enviar por email
- [ ] Marcar como primeiro acesso
- [ ] Registrar acao em auditoria

---

## EP-12: Configuracoes

### US-061: Configurar Parametros de Margem

**Epico:** EP-12
**Prioridade:** MVP
**Persona:** Administrador Consignante

**Historia:**
Como administrador,
Quero configurar os parametros de margem,
Para que o sistema calcule corretamente.

**Criterios de Aceitacao:**
- [ ] Percentual de margem geral
- [ ] Percentual por tipo de produto
- [ ] Limites de prazo
- [ ] Idade maxima ao final do contrato
- [ ] Vigencia das configuracoes

---

### US-062: Configurar Dados do Orgao

**Epico:** EP-12
**Prioridade:** MVP
**Persona:** Administrador Consignante

**Historia:**
Como administrador,
Quero configurar os dados do orgao,
Para que aparecam corretamente em documentos.

**Criterios de Aceitacao:**
- [ ] Nome, CNPJ, endereco
- [ ] Logo para relatorios
- [ ] Informacoes de contato
- [ ] Dados do responsavel

---

### US-063: Configurar Email (SMTP)

**Epico:** EP-12
**Prioridade:** MVP
**Persona:** Administrador Consignante

**Historia:**
Como administrador,
Quero configurar o servidor de email,
Para que o sistema envie notificacoes.

**Criterios de Aceitacao:**
- [ ] Servidor SMTP, porta, SSL/TLS
- [ ] Usuario e senha
- [ ] Email remetente
- [ ] Botao de teste de envio

---

### US-064: Configurar Webhooks

**Epico:** EP-12
**Prioridade:** Enterprise
**Persona:** Administrador Consignataria

**Historia:**
Como administrador da consignataria,
Quero configurar webhooks,
Para que meu sistema seja notificado de eventos.

**Criterios de Aceitacao:**
- [ ] Cadastrar URL do endpoint
- [ ] Selecionar eventos a notificar
- [ ] Definir secret para assinatura
- [ ] Testar webhook
- [ ] Visualizar log de envios

---

## EP-13: Auditoria

### US-065: Consultar Log de Auditoria

**Epico:** EP-13
**Prioridade:** MVP
**Persona:** Administrador, Auditor

**Historia:**
Como administrador ou auditor,
Quero consultar o log de auditoria,
Para que eu rastreie as acoes no sistema.

**Criterios de Aceitacao:**
- [ ] Filtros: periodo, usuario, entidade, acao
- [ ] Detalhes: data/hora, IP, navegador
- [ ] Valores antes/depois para alteracoes
- [ ] Paginacao e ordenacao
- [ ] Exportacao

---

### US-066: Consultar Log de Acessos

**Epico:** EP-13
**Prioridade:** MVP
**Persona:** Administrador

**Historia:**
Como administrador,
Quero consultar o log de acessos,
Para que eu monitore logins e tentativas.

**Criterios de Aceitacao:**
- [ ] Logins bem-sucedidos
- [ ] Tentativas invalidas
- [ ] IP e navegador
- [ ] Localizacao geografica (Enterprise)

---

### US-067: Gerenciar Sessoes Ativas

**Epico:** EP-13
**Prioridade:** MVP
**Persona:** Administrador

**Historia:**
Como administrador,
Quero visualizar e gerenciar sessoes ativas,
Para que eu encerre sessoes suspeitas.

**Criterios de Aceitacao:**
- [ ] Listar sessoes ativas por usuario
- [ ] Detalhes: IP, navegador, inicio
- [ ] Encerrar sessao remotamente
- [ ] Encerrar todas sessoes de um usuario

---

## EP-14: Notificacoes

### US-068: Receber Notificacao de Averbacao Pendente

**Epico:** EP-14
**Prioridade:** MVP
**Persona:** Aprovador

**Historia:**
Como aprovador,
Quero receber notificacao quando houver averbacao pendente,
Para que eu nao deixe acumular aprovacoes.

**Criterios de Aceitacao:**
- [ ] Email ao criar averbacao
- [ ] Resumo diario de pendencias
- [ ] Configuracao de frequencia
- [ ] Link direto para aprovacao

---

### US-069: Receber Notificacao de Aprovacao/Rejeicao

**Epico:** EP-14
**Prioridade:** MVP
**Persona:** Operador Consignataria

**Historia:**
Como operador da consignataria,
Quero receber notificacao quando minha averbacao for aprovada ou rejeitada,
Para que eu acompanhe o status.

**Criterios de Aceitacao:**
- [ ] Email imediato na mudanca de status
- [ ] Indicar motivo (se rejeicao)
- [ ] Link para visualizar averbacao

---

### US-070: Enviar Mensagem Interna

**Epico:** EP-14
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario,
Quero enviar mensagem para outro usuario,
Para que eu me comunique dentro do sistema.

**Criterios de Aceitacao:**
- [ ] Selecionar destinatario
- [ ] Assunto e corpo da mensagem
- [ ] Anexar arquivos (opcional)
- [ ] Notificacao ao destinatario
- [ ] Copia na caixa de enviados

---

### US-071: Visualizar Central de Mensagens

**Epico:** EP-14
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario,
Quero visualizar minhas mensagens,
Para que eu acompanhe comunicacoes.

**Criterios de Aceitacao:**
- [ ] Caixa de entrada
- [ ] Mensagens enviadas
- [ ] Indicador de nao lidas
- [ ] Marcar como lida/nao lida
- [ ] Arquivar mensagens

---

### US-072: Configurar Preferencias de Notificacao

**Epico:** EP-14
**Prioridade:** MVP
**Persona:** Todos os usuarios

**Historia:**
Como usuario,
Quero configurar minhas preferencias de notificacao,
Para que eu receba apenas o que me interessa.

**Criterios de Aceitacao:**
- [ ] Habilitar/desabilitar por tipo de evento
- [ ] Escolher canal (email, sistema)
- [ ] Frequencia de resumos
- [ ] Salvar preferencias

---

## 4. Resumo de User Stories

### 4.1 Por Epico

| Epico | Quantidade | MVP | Enterprise |
|-------|------------|-----|------------|
| EP-01: Autenticacao | 8 | 6 | 2 |
| EP-02: Funcionarios | 8 | 8 | 0 |
| EP-03: Margem | 4 | 4 | 0 |
| EP-04: Averbacoes | 8 | 8 | 0 |
| EP-05: Aprovacao | 5 | 5 | 0 |
| EP-06: Simulacoes | 5 | 5 | 0 |
| EP-07: Conciliacao | 5 | 5 | 0 |
| EP-08: Import/Export | 4 | 3 | 1 |
| EP-09: Consignatarias | 4 | 4 | 0 |
| EP-10: Relatorios | 5 | 5 | 0 |
| EP-11: Usuarios | 4 | 4 | 0 |
| EP-12: Configuracoes | 4 | 3 | 1 |
| EP-13: Auditoria | 3 | 3 | 0 |
| EP-14: Notificacoes | 5 | 5 | 0 |
| **TOTAL** | **72** | **68** | **4** |

### 4.2 Por Prioridade

| Prioridade | Quantidade | Percentual |
|------------|------------|------------|
| MVP | 68 | 94% |
| Enterprise | 4 | 6% |

---

## 5. Proximos Passos

1. **Refinamento:** Detalhar stories com equipe de desenvolvimento
2. **Estimativas:** Aplicar story points ou t-shirt sizing
3. **Sprint Planning:** Priorizar e distribuir em sprints
4. **Prototipos:** Criar wireframes para stories principais
5. **Testes:** Derivar casos de teste dos criterios de aceitacao

---

## 6. Historico de Revisoes

| Versao | Data | Autor | Descricao |
|--------|------|-------|-----------|
| 1.0 | Janeiro 2026 | Product Team | Versao inicial com 72 user stories |

---

*Este documento contem as user stories do sistema FastConsig e deve ser mantido atualizado conforme evolucao do backlog.*
