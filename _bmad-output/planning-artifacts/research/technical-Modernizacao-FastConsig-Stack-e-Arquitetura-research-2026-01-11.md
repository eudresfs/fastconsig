---
stepsCompleted: [1, 2, 3, 4]
inputDocuments: []
research_completed: true
workflowType: 'research'
lastStep: 1
research_type: 'technical'
research_topic: 'Modernizacao-FastConsig-Stack-e-Arquitetura'
research_goals: 'Validar Arquitetura Hexagonal em Node/TS, Clerk para B2B e integração CNAB/Open Finance'
user_name: 'Eudres'
date: '2026-01-11'
web_research_enabled: true
source_verification: true
---

# Research Report: Technical Research

**Date:** 2026-01-11
**Author:** Eudres
**Research Type:** technical

---

## Research Overview

[Research overview and methodology will be appended here]

---

## Technology Stack Analysis

### Identity & Access Management (IAM)

**Clerk for B2B/SaaS Multitenancy**
A pesquisa valida fortemente o uso do **Clerk** como solução de identidade para o modelo "Consignantes" (B2B) do Fast Consig.
*   **Recurso "Organizations":** O Clerk possui suporte nativo a "Organizations" (B2B), permitindo que cada empresa Consignante seja uma organização isolada com seus próprios membros (funcionários do RH), roles e permissões.
*   **MFA e Segurança:** Suporta MFA obrigatório, essencial para sistemas financeiros, e oferece proteção contra bots.
*   **Limitações/Riscos:** A migração de usuários existentes pode ser complexa e o custo escala por MAU (Monthly Active User), o que exige atenção no orçamento dado o volume potencial de funcionários.
*   **Alternativas:** Kinde e PropelAuth também focam em B2B, mas Clerk lidera em DX (Developer Experience) e integração com React/Next.js.

_Source: [Clerk Organizations Documentation](https://clerk.com/docs/organizations/overview), [Clerk Pricing](https://clerk.com/pricing)_

### Architecture & Backend Patterns

**Hexagonal Architecture (Ports & Adapters) em Node.js**
A arquitetura hexagonal é altamente recomendada para isolar o *Core Domain* (regras de consignado) de detalhes de implementação voláteis (frameworks web, bancos de dados, integrações bancárias).
*   **Estrutura de Pastas Recomendada:**
    *   `src/domain`: Entidades e interfaces de Repositórios/Gateways (sem dependências externas).
    *   `src/application`: Casos de uso (Services) que implementam a lógica orquestradora.
    *   `src/infrastructure`: Implementações concretas (Adapters) para Banco (Postgres/Prisma), Web (Express/Fastify) e Serviços Externos (Clerk, CNAB).
*   **Injeção de Dependência:** Ferramentas como **NestJS** (com seu container de DI nativo) ou bibliotecas como **InversifyJS** (para Express puro) são essenciais para gerenciar a injeção dos adapters nos services do domínio.

_Source: [Hexagonal Architecture in Node.js - Best Practices](https://blog.logrocket.com/hexagonal-architecture-node-js/), [NestJS Architecture](https://docs.nestjs.com/)_

### Integration & Banking (Anti-Corruption Layer)

**CNAB vs. Open Finance**
A integração bancária continua sendo um desafio híbrido.
*   **CNAB (Legado Indispensável):** A troca de arquivos (CNAB 240/400) ainda é o padrão dominante para folhas de pagamento e repasses em massa.
    *   **Libs Node.js:** Existem bibliotecas como `cnab-nodejs` e projetos open-source específicos para bancos brasileiros, mas a recomendação é criar um **Adapter Customizado** na camada de infraestrutura que use essas libs apenas como auxiliares de parse, encapsulando as regras de negócio de cada banco (Layouts variam muito).
*   **Open Finance (Futuro):** APIs de Iniciação de Pagamento (ITP) estão crescendo, mas a adoção para *averbação de consignado* ainda é incipiente e depende de convênios específicos. O sistema deve estar pronto para isso (via API-First), mas o CNAB será o "trilho" principal no curto prazo.

_Source: [Febraban CNAB Standards](https://cms.febraban.org.br/), [Open Finance Brasil Developers](https://openfinancebrasil.org.br/)_

### Database Strategy

**PostgreSQL (Relacional/ACID)**
Confirmado como a escolha superior ao NoSQL para este caso de uso.
*   **Integridade Financeira:** O suporte a transações ACID robustas é inegociável para evitar "dinheiro duplo" ou saldo de margem inconsistente.
*   **Flexibilidade:** O suporte a colunas `JSONB` no Postgres permite armazenar dados semi-estruturados (ex: logs de webhooks, configurações de layouts bancários variados) sem perder a rigidez do schema para as tabelas financeiras principais (`Loans`, `Wallets`).

_Source: [PostgreSQL for Financial Systems](https://www.postgresql.org/about/)_

### Technology Adoption Trends

## Integration Patterns Analysis

### API Design Patterns

**API-First e OpenAPI (Swagger)**
A abordagem "API-First" é essencial para a estratégia de "Onipresença".
*   **RESTful JSON:** Padrão primário para comunicação com Frontend (React), App Mobile e Parceiros. Simples, cacheável e amplamente suportado.
*   **Contratos OpenAPI 3.0:** Definir a API *antes* de codificar (Spec-First) permite gerar clientes (SDKs) para parceiros e mocks para o time de frontend trabalhar em paralelo.
*   **Webhook Pattern:** Crucial para o "Push Intelligence" e notificações assíncronas de pagamentos (retorno bancário). O sistema deve ter um módulo robusto de envio/retry de webhooks para parceiros.

_Source: [API Design Patterns - REST vs GraphQL](https://restfulapi.net/), [OpenAPI Specification](https://www.openapis.org/)_

### Communication Protocols

**Híbrido: Síncrono e Assíncrono**
*   **HTTP/HTTPS (Síncrono):** Para todas as interações de usuário (UX) onde a resposta imediata é esperada (ex: Simulação de Empréstimo, Login).
*   **Message Queues (Assíncrono):** Essencial para desacoplar o "Core" do "Mundo Lento" (Bancos).
    *   Uso de **BullMQ (Redis)** ou **RabbitMQ** para filas de processamento de margem, geração de CNAB e envio de e-mails. Isso garante que o servidor web nunca trave esperando um terceiro.

_Source: [Microservices Communication Patterns](https://microservices.io/patterns/communication-style/messaging.html)_

### Data Formats and Standards

**CNAB 240 (O Padrão Oculto)**
Embora o JSON reine internamente, o CNAB 240 (Febraban) é o protocolo de fato para movimentação financeira em lote.
*   **Estratégia de Isolamento:** O formato posicional arcaico do CNAB deve ser confinado à camada de Infraestrutura (Adapter). Nenhuma classe de domínio deve saber o que é um "Header de Arquivo" ou "Trailer de Lote".
*   **Libs Recomendadas:** `cnab-nodejs` para parse genérico, mas com validação de schema customizada via `zod` para garantir a integridade dos dados antes de gerar o arquivo.

_Source: [Febraban Layouts](https://cms.febraban.org.br/)_

### System Interoperability Approaches

**Adapter Pattern para Legado Bancário**
A interoperabilidade com bancos legados não será via API REST elegante, mas via troca de arquivos.
*   **SFTP/VAN Automation:** O sistema deve incluir adaptadores para conectar via SFTP seguro nos servidores dos bancos para upload/download automático de remessas/retornos, eliminando a intervenção manual do operador.

_Source: [Enterprise Integration Patterns - File Transfer](https://www.enterpriseintegrationpatterns.com/patterns/messaging/FileTransfer.html)_

### Microservices Integration Patterns

**Monolito Modular (Modular Monolith)**
Para o estágio inicial (Greenfield), microserviços distribuídos adicionam complexidade operacional desnecessária (Kubernetes, Tracing Distribuído).
*   **Recomendação:** Iniciar como um **Monolito Modular** bem estruturado (via NestJS Modules ou Nx Workspace).
*   **Separação Lógica:** Módulos de "Empréstimo", "Identidade" e "Integração Bancária" são separados logicamente e comunicam-se via interfaces públicas (in-process calls), mas deployados juntos.
*   **Evolução:** Se um módulo (ex: Processamento de Margem) precisar de escala independente no futuro, ele pode ser extraído para um microserviço com pouco esforço graças à arquitetura hexagonal.

_Source: [Modular Monolith: A Primer](https://khalilstemmler.com/articles/software-design-architecture/modular-monolith/)_

### Integration Security Patterns

**OAuth 2.0 & API Keys**
*   **User Context (Clerk):** Tokens JWT (OIDC) para sessões de usuários humanos.
*   **Service Context (M2M):** API Keys rotacionáveis para Bots de WhatsApp e integrações de parceiros back-to-back.
*   **Rate Limiting:** Implementação de `ThrottlerGuard` (NestJS) ou middleware de Rate Limit no Redis para proteger a API pública contra abuso de parceiros ou ataques.

_Source: [OAuth 2.0 Client Credentials Flow](https://oauth.net/2/grant-types/client-credentials/)_



