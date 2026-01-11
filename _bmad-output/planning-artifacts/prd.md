---
stepsCompleted:
  - step-01-init
  - step-02-discovery
  - step-03-success
  - step-04-journeys
  - step-05-domain
  - step-06-innovation
classification:
  projectType: Web Application (SaaS/Platform)
  domain: Fintech / Payroll Loans (Consignado)
  complexity: High
  projectContext: brownfield
inputDocuments:
  - product-development/current-feature/relatorio-revisao-arquitetural.md
  - product-development/current-feature/PRD.md
  - product-development/current-feature/arquitetura-tecnica.md
  - product-development/current-feature/plano-sprints.md
workflowType: prd
documentCounts:
  briefs: 0
  research: 0
  brainstorming: 0
  projectDocs: 4
---

# Product Requirements Document - Fast Consig

**Author:** Eudres
**Date:** 2026-01-11

## Project Classification

- **Project Type:** Web Application (SaaS/Platform)
- **Domain:** Fintech / Payroll Loans (Consignado)
- **Complexity:** High (Financial transactions, Regulatory compliance, Multi-tenancy)
- **Context:** Brownfield (Modernization of legacy .NET to Node.js/React)

**Current Status:** Foundation Completed (Sprint 0-1), Sprint 2 in progress. Architecture and core modules defined.

## Success Criteria

### User Success
- **RH/Consignante:** "Fechamento de folha em minutos, não dias". Eliminação de processos manuais de conferência.
- **Consignatária/Banco:** Confiança absoluta na margem reservada (Zero "Queda de Reserva" por erro de cálculo).
- **Agente:** Visibilidade em tempo real do status das propostas.

### Business Success
- **Operational Efficiency:** Redução de 80% nos tickets de suporte relacionados a "erro de margem" ou "arquivo travado".
- **Scalability:** Capacidade de onboardar novos órgãos (Tenants) sem intervenção de desenvolvedores.

### Technical Success
- **Stability:** Isolamento total de falhas bancárias (CNAB) através da Anti-Corruption Layer.
- **Performance:** Latência de API < 200ms para consultas de margem (p95).
- **Security:** Auditoria completa e imutável de todas as ações financeiras (LGPD compliant).

## Product Scope

### MVP - Minimum Viable Product
- **Core Modules:** Autenticação (Clerk/Local), Gestão de Funcionários, Cálculo de Margem, Ciclo de Averbação.
- **Integration:** Geração/Leitura de CNAB (Adapter), Portal Administrativo Web.
- **Security:** Multi-tenancy isolado, Auditoria.

### Growth Features (Post-MVP)
- **Real-Time:** "Margem Viva" com recálculo por eventos.
- **Connectivity:** Webhooks para parceiros, API Pública documentada.

## User Journeys

### Journey 1: O Fechamento da Folha Sem Stress (Gestor de RH)
**Cenário:** Final de mês, janela de fechamento da folha de pagamento.
**Dor Atual:** Processo manual de consolidação de arquivos de múltiplos bancos, erros de centavos, risco de descontar errado do salário.
**Nova Experiência:**
1. O Gestor acessa o Dashboard de Conciliação.
2. O sistema já importou automaticamente os retornos bancários (via Adapter CNAB).
3. Um alerta vermelho indica "3 Divergências Encontradas".
4. Ele clica, vê que o Banco X mandou um desconto duplicado.
5. Com um clique em "Rejeitar Duplicidade", o sistema ajusta o saldo.
6. Ele clica em "Gerar Arquivo de Folha". O sistema gera um único arquivo consolidado e validado.
**Resultado:** Processo de 2 dias reduzido para 15 minutos.

### Journey 2: A Venda Garantida (Operador Bancário)
**Cenário:** Tentativa de venda de empréstimo para um servidor com margem apertada.
**Dor Atual:** "Queda de Reserva" - A margem existia na consulta, mas sumiu na hora da efetivação.
**Nova Experiência:**
1. O sistema do banco consulta a API do Fast Consig.
2. A API retorna a margem e cria um "Soft Lock" (Reserva Temporária) de 10 minutos.
3. O operador fecha a venda com o cliente.
4. O banco envia a confirmação de averbação.
5. O Fast Consig converte o Lock em Averbacao Permanente instantaneamente.
**Resultado:** Taxa de aprovação técnica sobe para ~100% nas propostas pré-qualificadas.

### Journey 3: Transparência na Ponta (Agente/Correspondente)
**Cenário:** Acompanhamento de uma proposta crítica.
**Dor Atual:** Falta de visibilidade, dependência de suporte telefônico para saber status.
**Nova Experiência:**
1. O Agente submete a proposta.
2. Ele acompanha o status em tempo real no seu CRM (via Webhook do Fast Consig).
3. Status muda: "Aguardando RH" -> "Averbado" -> "Pago".
4. Ele notifica o cliente proativamente em cada etapa.
**Resultado:** Redução de chamados de suporte e aumento da confiança do cliente final.

## Domain-Specific Requirements

### Compliance & Regulatory (Consignado)
- **Margem Consignável Configurável:** O sistema deve suportar regras de margem dinâmicas por Tenant (Órgão), permitindo configurações como:
  - Margem Padrão (ex: 30% ou 35%)
  - Margem Cartão Consignado (ex: +5%)
  - Margem Cartão Benefício (ex: +5%)
- **Integridade de Cálculo:** Garantia absoluta de que a soma das parcelas nunca excede a margem configurada, utilizando travamento de banco de dados (Row Locking) para evitar Race Conditions.

### Technical Constraints (Security & Privacy)
- **Segregação de Sigilo Bancário:** Um Operador Bancário (Consignatária A) NUNCA pode visualizar detalhes de contratos de concorrentes (Consignatária B). A API deve retornar apenas "Margem Utilizada Total" e "Margem Disponível", mascarando a origem das reservas existentes.
- **Evidência Digital:** Toda transação de averbação deve persistir metadata de não-repúdio (IP, Timestamp, User Agent, ID do Usuário), atendendo requisitos jurídicos de validade contratual.

### Integration Requirements
- **Isolamento CNAB:** A complexidade dos layouts de arquivo bancário deve ser contida na camada de infraestrutura (Anti-Corruption Layer). O Core Domain deve operar exclusivamente com entidades limpas (`Loan`, `Installment`).

### Risk Mitigations
- **Prevenção à Fraude:** Alertas automáticos para comportamentos anômalos (ex: múltiplas consultas de margem em segundos para o mesmo CPF vindo de IPs diferentes).

## Innovation & Novel Patterns

### Detected Innovation Areas

1.  **Margem Viva (Event-Driven Core):**
    *   **Conceito:** Mudança de paradigma de "Cálculo sob Demanda/Batch" para "Cálculo Reativo".
    *   **Inovação:** A margem não é calculada quando o usuário pede; ela é mantida perpetuamente atualizada por eventos. Se um contrato é averbado, o saldo é reajustado em milissegundos.
    *   **Valor:** Elimina a "falsa disponibilidade" e permite feedback instantâneo para APIs externas.

2.  **Banking Anti-Corruption Layer (ACL):**
    *   **Conceito:** Isolamento radical da infraestrutura bancária legada.
    *   **Inovação:** O domínio da aplicação ignora a existência de arquivos CNAB/texto. Um adaptador de fronteira traduz layouts bancários arcaicos e variados em entidades de domínio puras e padronizadas (`Loan`, `Installment`).
    *   **Valor:** Permite escalar para 10, 20 ou 50 bancos sem contaminar as regras de negócio com "ifs" de layout de arquivo.

3.  **Estratégia API-First (Onipresença):**
    *   **Conceito:** Desacoplamento total entre "Produto" e "Interface".
    *   **Inovação:** O backend é construído como uma Plataforma (PaaS) desde o D0. O Portal Web Administrativo é tratado como apenas um dos clientes da API, com os mesmos privilégios de um futuro Bot de WhatsApp ou Integração de ERP.
    *   **Valor:** Reduz o Time-to-Market para novos canais (Mobile, Chatbots) a quase zero.

4.  **Push Intelligence (Observabilidade de Negócio):**
    *   **Conceito:** Monitoramento ativo de regras de negócio, não apenas de servidores.
    *   **Inovação:** O sistema proativamente notifica o usuário sobre anomalias (ex: "Margem Negativa Detectada", "Desconto não processado pelo Banco") em vez de esperar que o usuário gere um relatório para descobrir o erro.
    *   **Valor:** Transforma o papel do RH de "Caçador de Erros" para "Resolvedor de Problemas", reduzindo drasticamente o tempo de fechamento da folha.

### Market Context & Competitive Landscape

*   **Cenário Atual:** O mercado de consignados é dominado por softwares legados (.NET/Java antigos) focados em processamento batch noturno. A experiência é reativa e lenta.
*   **Diferenciação:** O Fast Consig se posiciona como uma "Fintech Moderna" (Real-time, API-First) em um mundo de "Sistemas Bancários Tradicionais". A velocidade e a precisão da margem em tempo real são diferenciais que concorrentes baseados em batch não conseguem replicar sem reescrever seus núcleos.

### Validation Approach

*   **ACL Proof-of-Concept:** Implementar integração com 1 Banco complexo (ex: BB ou Caixa) usando a arquitetura hexagonal para provar o isolamento.
*   **Stress Test de Margem:** Simular rajadas de averbações concorrentes para validar se o bloqueio de banco de dados (Row Locking) e o recálculo de margem mantêm a consistência (zero margem negativa).
*   **API Dogfooding:** Construir o Frontend usando estritamente a API Pública documentada, garantindo que ela seja completa e funcional para terceiros.

### Risk Mitigation

*   **Complexidade Event-Driven:** O modelo de "Margem Viva" aumenta a complexidade de consistência.
    *   *Mitigação:* Uso estrito de transações ACID no PostgreSQL e design de Agregados DDD para garantir que a margem nunca fique inconsistente, mesmo em falhas.
*   **Resistência Bancária:** Bancos podem mudar layouts sem aviso.
    *   *Mitigação:* O ACL centraliza as mudanças em um único ponto (Adapter), protegendo o resto do sistema de refatorações.
