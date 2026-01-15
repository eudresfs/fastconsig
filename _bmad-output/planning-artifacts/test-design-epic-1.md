# Test Design: Epic 1 - Platform Foundation & Identity

**Date:** 2026-01-13
**Author:** Eudres
**Status:** Draft

---

## Executive Summary

**Scope:** Test design completo para Epic 1 - Platform Foundation & Identity (SaaS Core)

**Risk Summary:**

- Total de riscos identificados: 12
- Riscos de alta prioridade (≥6): 5
- Categorias críticas: SEC (Security), DATA (Data Integrity), TECH (Architecture)

**Coverage Summary:**

- Cenários P0: 18 (36 horas)
- Cenários P1: 12 (12 horas)
- Cenários P2/P3: 8 (4 horas)
- **Esforço total**: 52 horas (~6.5 dias)

---

## Risk Assessment

### High-Priority Risks (Score ≥6)

| Risk ID | Category | Description | Probability | Impact | Score | Mitigation | Owner | Timeline |
| ------- | -------- | ----------- | ----------- | ------ | ----- | ---------- | ----- | -------- |
| R-001 | SEC | JWT signature bypass permitindo acesso cross-tenant | 2 | 3 | 6 | Testes de token manipulation + validação de claims obrigatórios | DEV | Sprint 1 |
| R-002 | DATA | Vazamento de dados entre tenants via RLS bypass ou SQL injection | 2 | 3 | 6 | Testes de isolamento por tenant + penetration testing | QA | Sprint 1 |
| R-003 | SEC | MFA bypass em operações financeiras críticas | 2 | 3 | 6 | Testes de enforcement de MFA em todas rotas de escrita | DEV | Sprint 1 |
| R-004 | DATA | Audit trail perdido ou corrompido (logs não imutáveis) | 2 | 3 | 6 | Testes de integridade de audit trail + append-only validation | DEV | Sprint 1 |
| R-005 | TECH | Race condition em criação simultânea de tenants causando duplicação | 2 | 3 | 6 | Testes de concorrência + unique constraints no DB | DEV | Sprint 1 |

### Medium-Priority Risks (Score 3-4)

| Risk ID | Category | Description | Probability | Impact | Score | Mitigation | Owner |
| ------- | -------- | ----------- | ----------- | ------ | ----- | ---------- | ----- |
| R-006 | OPS | Falha na integração com Clerk API causando downtime de autenticação | 2 | 2 | 4 | Circuit breaker + retry logic + testes de failover | OPS |
| R-007 | BUS | Configuração incorreta de regras de margem por tenant causando cálculos errados | 2 | 2 | 4 | Validação de schema + testes de cálculo com diferentes configs | QA |
| R-008 | TECH | Memory leak em Async Local Storage acumulando contextos não limpos | 1 | 3 | 3 | Testes de stress + monitoramento de memória | DEV |
| R-009 | SEC | IP throttling inadequado permitindo enumeration attacks | 1 | 3 | 3 | Testes de rate limiting + behavioral analysis | SEC |

### Low-Priority Risks (Score 1-2)

| Risk ID | Category | Description | Probability | Impact | Score | Action |
| ------- | -------- | ----------- | ----------- | ------ | ----- | ------ |
| R-010 | OPS | Logs de auditoria com formato inconsistente dificultando análise | 1 | 2 | 2 | Monitor |
| R-011 | BUS | UX confusa na interface de configuração de tenant | 1 | 1 | 1 | Monitor |
| R-012 | PERF | Latência elevada na primeira consulta após cold start | 1 | 1 | 1 | Monitor |

### Risk Category Legend

- **TECH**: Technical/Architecture (falhas, integração, escalabilidade)
- **SEC**: Security (controles de acesso, autenticação, exposição de dados)
- **PERF**: Performance (violações de SLA, degradação, limites de recursos)
- **DATA**: Data Integrity (perda, corrupção, inconsistência)
- **BUS**: Business Impact (degradação UX, erros de lógica, receita)
- **OPS**: Operations (falhas de deploy, configuração, monitoramento)

---

## Test Coverage Plan

### P0 (Critical) - Run on every commit

**Critérios**: Bloqueia jornada core + Alto risco (≥6) + Sem workaround

| Requirement | Test Level | Risk Link | Test Count | Owner | Notes |
| ----------- | ---------- | --------- | ---------- | ----- | ----- |
| Story 1.1: JWT validation e tenant context | API | R-001, R-002 | 5 | DEV | Token manipulation, cross-tenant access, signature verification |
| Story 1.1: RLS enforcement | Integration | R-002 | 3 | DEV | Isolation tests por tenant, SQL injection attempts |
| Story 1.1: MFA enforcement | E2E | R-003 | 2 | QA | Financial operations require MFA, read-only bypass allowed |
| Story 1.2: Tenant creation uniqueness | API | R-005 | 2 | DEV | Concurrent creation, CNPJ uniqueness |
| Story 1.4: Audit trail immutability | Integration | R-004 | 3 | DEV | Append-only validation, tamper detection |
| Story 1.6: Anomaly detection triggers | API | R-009 | 3 | QA | Rate limiting, IP throttling, behavioral alerts |

**Total P0**: 18 testes, 36 horas

### P1 (High) - Run on PR to main

**Critérios**: Features importantes + Risco médio (3-4) + Workflows comuns

| Requirement | Test Level | Risk Link | Test Count | Owner | Notes |
| ----------- | ---------- | --------- | ---------- | ----- | ----- |
| Story 1.2: Tenant admin invite | API | R-006 | 2 | DEV | Clerk API integration, retry on failure |
| Story 1.3: Configuration engine | API | R-007 | 3 | QA | Schema validation, margin calculation rules |
| Story 1.4: Audit log querying | API | - | 2 | DEV | Filtering, pagination, LGPD anonymization |
| Story 1.6: Security alert notifications | Integration | - | 3 | QA | Email/webhook delivery, alert prioritization |
| Story 1.1: Health check endpoint | E2E | - | 2 | DEV | `/api/me` returns decoded context |

**Total P1**: 12 testes, 12 horas

### P2 (Medium) - Run nightly/weekly

**Critérios**: Features secundárias + Baixo risco (1-2) + Edge cases

| Requirement | Test Level | Risk Link | Test Count | Owner | Notes |
| ----------- | ---------- | --------- | ---------- | ----- | ----- |
| Story 1.3: Configuration versioning | API | R-010 | 2 | DEV | History tracking, rollback capability |
| Story 1.4: Audit log retention | Integration | R-010 | 2 | OPS | Cold storage migration, 5-year retention |
| Story 1.2: Tenant status management | API | - | 2 | DEV | Active/Inactive transitions |
| Story 1.6: False positive handling | API | - | 2 | QA | Anomaly whitelisting, tuning thresholds |

**Total P2**: 8 testes, 4 horas

### P3 (Low) - Run on-demand

**Critérios**: Nice-to-have + Exploratory + Performance benchmarks

Não aplicável para Epic 1 no escopo atual.

**Total P3**: 0 testes, 0 horas

---

## Execution Order

### Smoke Tests (<5 min)

**Propósito**: Feedback rápido, detectar problemas que quebram build

- [ ] Health check retorna 200 com tenant context (30s)
- [ ] JWT válido permite acesso a rota protegida (45s)
- [ ] JWT inválido retorna 401 (30s)
- [ ] RLS bloqueia acesso cross-tenant (1min)

**Total**: 4 cenários

### P0 Tests (<10 min)

**Propósito**: Validação de caminho crítico

- [ ] Token manipulation não bypassa validação (API)
- [ ] Cross-tenant access bloqueado em todos endpoints (API)
- [ ] MFA obrigatório para operações de escrita financeira (E2E)
- [ ] Criação concorrente de tenants não duplica (API)
- [ ] Audit trail não permite edição ou deleção (Integration)
- [ ] Anomaly detection dispara alerta em threshold (API)
- [ ] Signature verification rejeita tokens adulterados (API)
- [ ] RLS enforcement em queries complexas (Integration)
- [ ] Token expiration funciona corretamente (API)
- [ ] Tenant context injection em ALS (Integration)
- [ ] SQL injection não bypassa RLS (Integration)
- [ ] Rate limiting por IP e por CPF (API)
- [ ] Audit trail captura old/new values (Integration)
- [ ] JWT claims obrigatórios presentes (API)
- [ ] MFA bypass detection (E2E)
- [ ] CNPJ uniqueness enforcement (API)
- [ ] Behavioral anomaly patterns (API)
- [ ] Tamper detection em audit logs (Integration)

**Total**: 18 cenários

### P1 Tests (<30 min)

**Propósito**: Cobertura de features importantes

- [ ] Clerk API retry on failure (API)
- [ ] Admin invite email delivery (API)
- [ ] Configuration schema validation (API)
- [ ] Margin calculation with custom rules (API)
- [ ] Configuration update audit trail (API)
- [ ] Audit log filtering by date range (API)
- [ ] LGPD anonymization in reports (API)
- [ ] Security alert webhook delivery (Integration)
- [ ] Alert prioritization logic (Integration)
- [ ] Email notification formatting (Integration)
- [ ] Health check with all context fields (E2E)
- [ ] Tenant context propagation to services (E2E)

**Total**: 12 cenários

### P2/P3 Tests (<60 min)

**Propósito**: Cobertura de regressão completa

- [ ] Configuration version history (API)
- [ ] Configuration rollback (API)
- [ ] Audit log cold storage migration (Integration)
- [ ] 5-year retention policy (Integration)
- [ ] Tenant status transitions (API)
- [ ] Inactive tenant access blocked (API)
- [ ] Anomaly whitelist management (API)
- [ ] Threshold tuning for alerts (API)

**Total**: 8 cenários

---

## Resource Estimates

### Test Development Effort

| Priority | Count | Hours/Test | Total Hours | Notes |
| -------- | ----- | ---------- | ----------- | ----- |
| P0 | 18 | 2.0 | 36 | Setup complexo, testes de segurança |
| P1 | 12 | 1.0 | 12 | Cobertura padrão |
| P2 | 8 | 0.5 | 4 | Cenários simples |
| P3 | 0 | 0.25 | 0 | N/A |
| **Total** | **38** | **-** | **52** | **~6.5 dias** |

### Prerequisites

**Test Data:**

- `TenantFactory` - Criação de tenants com dados faker (auto-cleanup)
- `UserFactory` - Usuários com diferentes roles e permissões
- `JWTFactory` - Tokens válidos/inválidos para testes de auth
- `AuditLogFactory` - Logs para testes de querying e retenção

**Tooling:**

- **Playwright** para testes E2E de MFA flows
- **Vitest** para testes de API e Integration
- **Supertest** para testes de HTTP endpoints
- **PostgreSQL Testcontainers** para testes de RLS isolation
- **Faker.js** para geração de dados de teste

**Environment:**

- PostgreSQL 16+ com RLS habilitado
- Redis para sessões e rate limiting
- Clerk test environment com organizations
- SMTP mock para testes de email
- Webhook mock para testes de notificações

---

## Quality Gate Criteria

### Pass/Fail Thresholds

- **P0 pass rate**: 100% (sem exceções)
- **P1 pass rate**: ≥95% (waivers necessários para falhas)
- **P2/P3 pass rate**: ≥90% (informativo)
- **High-risk mitigations**: 100% completo ou waivers aprovados

### Coverage Targets

- **Caminhos críticos**: ≥80%
- **Cenários de segurança**: 100%
- **Lógica de negócio**: ≥70%
- **Edge cases**: ≥50%

### Non-Negotiable Requirements

- [ ] Todos os testes P0 passam
- [ ] Nenhum risco de alta prioridade (≥6) sem mitigação
- [ ] Testes de segurança (categoria SEC) passam 100%
- [ ] Testes de isolamento multi-tenant passam 100%

---

## Mitigation Plans

### R-001: JWT signature bypass permitindo acesso cross-tenant (Score: 6)

**Mitigation Strategy:**
1. Implementar testes de token manipulation (tokens adulterados, claims modificados)
2. Validar assinatura JWT em middleware antes de processar claims
3. Verificar claims obrigatórios: `tenant_id`, `user_id`, `exp`, `iss`
4. Implementar whitelist de issuers permitidos
5. Adicionar testes de penetration para bypass attempts

**Owner:** DEV
**Timeline:** Sprint 1
**Status:** Planejado
**Verification:** Suite de testes P0 com 5 cenários de token manipulation

### R-002: Vazamento de dados entre tenants via RLS bypass (Score: 6)

**Mitigation Strategy:**
1. Testes de isolamento por tenant em todos os endpoints CRUD
2. Verificar RLS policies em todas as tabelas com `tenant_id`
3. Testes de SQL injection tentando bypass de RLS
4. Validar que queries complexas (joins, subqueries) respeitam RLS
5. Audit de acesso cross-tenant no CI/CD

**Owner:** QA
**Timeline:** Sprint 1
**Status:** Planejado
**Verification:** Suite de Integration tests com 3 cenários de isolation

### R-003: MFA bypass em operações financeiras críticas (Score: 6)

**Mitigation Strategy:**
1. Implementar testes E2E de enforcement de MFA
2. Verificar que rotas de escrita financeira exigem MFA claim no JWT
3. Testar que leitura não bloqueia sem MFA (usabilidade)
4. Validar que tentativas de bypass retornam 403 Forbidden
5. Adicionar logs de auditoria para tentativas de bypass

**Owner:** DEV
**Timeline:** Sprint 1
**Status:** Planejado
**Verification:** 2 testes E2E P0 + audit trail validation

### R-004: Audit trail perdido ou corrompido (Score: 6)

**Mitigation Strategy:**
1. Testes de append-only enforcement no banco
2. Verificar integridade de logs após tentativas de UPDATE/DELETE
3. Implementar checksums ou hashes para detecção de tampering
4. Testes de retenção de 5 anos com cold storage
5. Validar que formato JSON inclui old/new values

**Owner:** DEV
**Timeline:** Sprint 1
**Status:** Planejado
**Verification:** 3 testes Integration P0 de immutability

### R-005: Race condition em criação simultânea de tenants (Score: 6)

**Mitigation Strategy:**
1. Testes de concorrência com criação paralela de tenants
2. Implementar unique constraint em `tenants.cnpj`
3. Validar que segundo request retorna 409 Conflict
4. Testes de idempotency com mesmo payload
5. Verificar comportamento sob load (10 requests simultâneos)

**Owner:** DEV
**Timeline:** Sprint 1
**Status:** Planejado
**Verification:** 2 testes API P0 de concurrent creation

---

## Assumptions and Dependencies

### Assumptions

1. Clerk SDK estará disponível e configurado no ambiente de testes
2. PostgreSQL 16+ com RLS habilitado será usado em todos os ambientes
3. Redis estará disponível para rate limiting e sessões
4. Dados de teste serão limpos automaticamente após cada execução

### Dependencies

1. **Clerk Test Environment** - Necessário para testes de autenticação (Disponível até: Sprint 1)
2. **PostgreSQL Testcontainers** - Necessário para testes de RLS isolation (Disponível até: Sprint 1)
3. **SMTP Mock Server** - Necessário para testes de email notifications (Disponível até: Sprint 1)

### Risks to Plan

- **Risk**: Clerk API rate limits podem bloquear testes em CI/CD
  - **Impact**: Testes de autenticação podem falhar intermitentemente
  - **Contingency**: Implementar caching de tokens válidos + mock server para cenários críticos

- **Risk**: RLS performance pode impactar tempo de execução dos testes
  - **Impact**: Suite de testes pode exceder budget de tempo no CI
  - **Contingency**: Usar database seeding otimizado + parallel test execution

---

## Follow-on Workflows (Manual)

- Executar `*atdd` para gerar testes P0 falhando (workflow separado, não executado automaticamente)
- Executar `*automate` para cobertura mais ampla após implementação existir

---

## Approval

**Test Design Approved By:**

- [ ] Product Manager: ___________ Data: ___________
- [ ] Tech Lead: ___________ Data: ___________
- [ ] QA Lead: ___________ Data: ___________

**Comments:**

---

## Appendix

### Knowledge Base References

- `risk-governance.md` - Framework de classificação de riscos
- `probability-impact.md` - Metodologia de scoring de riscos
- `test-levels-framework.md` - Seleção de níveis de teste
- `test-priorities-matrix.md` - Priorização P0-P3

### Related Documents

- PRD: `_bmad-output/planning-artifacts/prd.md`
- Epic: `_bmad-output/planning-artifacts/epics.md`
- Architecture: `_bmad-output/planning-artifacts/architecture.md`
- Sprint Status: `_bmad-output/implementation-artifacts/sprint-status.yaml`

---

**Gerado por**: BMad TEA Agent - Test Architect Module
**Workflow**: `_bmad/bmm/testarch/test-design`
**Version**: 4.0 (BMad v6)
