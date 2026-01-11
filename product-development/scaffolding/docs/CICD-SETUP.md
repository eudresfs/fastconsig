# CI/CD Setup - FastConsig

Documentação completa para configuração do pipeline de CI/CD no GitHub Actions com deploy no Docker Swarm.

---

## 📋 Índice

1. [Visão Geral](#visão-geral)
2. [Pré-requisitos](#pré-requisitos)
3. [Configuração de Secrets](#configuração-de-secrets)
4. [Estrutura de Arquivos](#estrutura-de-arquivos)
5. [Workflows](#workflows)
6. [Deploy](#deploy)
7. [Troubleshooting](#troubleshooting)

---

## 🎯 Visão Geral

O projeto FastConsig utiliza GitHub Actions para:

- **CI (Continuous Integration)**: Validação automática de código em PRs e pushes
- **CD (Continuous Deployment)**: Build de imagens Docker e deploy automático no Swarm

### Ambientes

| Ambiente | Branch | URLs |
|----------|--------|------|
| **Produção** | `main` | `app.fastconsig.com.br` / `api.fastconsig.com.br` |
| **Development** | `development` | `dev-app.fastconsig.com.br` / `dev-api.fastconsig.com.br` |

---

## ✅ Pré-requisitos

### 1. Runners Self-Hosted

O projeto requer 2 runners self-hosted configurados no GitHub:

- **Runner `build`**: Para build de imagens Docker
- **Runner `deploy`**: Para deploy no Docker Swarm

**Configuração de labels nos runners:**

```bash
# Runner de Build
Labels: self-hosted, build

# Runner de Deploy
Labels: self-hosted, deploy
```

### 2. Docker Swarm

- Swarm inicializado e funcionando
- Rede `proxy_network` criada (para Traefik)
- Nodes com label `workload=worker`

```bash
# Criar rede do proxy (se não existir)
docker network create --driver overlay proxy_network

# Adicionar label aos nodes
docker node update --label-add workload=worker <node-name>
```

### 3. OCI Registry (Oracle Cloud)

- Registry configurado: `gru.ocir.io/grnvzpym0ltz`
- Credenciais de acesso (Username e Auth Token)

---

## 🔐 Configuração de Secrets

Configure os seguintes secrets no GitHub:

**Caminho:** `Settings` → `Secrets and variables` → `Actions` → `New repository secret`

### Secrets Obrigatórios

| Secret Name | Descrição | Exemplo / Como Gerar |
|-------------|-----------|---------------------|
| `OCI_USERNAME` | Username do OCI Registry | `grnvzpym0ltz/seu-usuario` |
| `OCI_TOKEN` | Auth Token do OCI | Gerado no OCI Console |
| `DB_PASSWORD` | Senha do PostgreSQL | Senha forte (min 16 chars) |
| `JWT_ACCESS_SECRET` | Secret para JWT Access Token | `openssl rand -base64 64` |
| `JWT_REFRESH_SECRET` | Secret para JWT Refresh Token | `openssl rand -base64 64` |

### Secrets Opcionais (Email)

| Secret Name | Descrição | Valor Padrão |
|-------------|-----------|--------------|
| `SMTP_HOST` | Servidor SMTP | `smtp.gmail.com` |
| `SMTP_PORT` | Porta SMTP | `587` |
| `SMTP_USER` | Usuário SMTP | - |
| `SMTP_PASSWORD` | Senha SMTP | - |

### Secrets Opcionais (Codecov)

| Secret Name | Descrição |
|-------------|-----------|
| `CODECOV_TOKEN` | Token para upload de coverage (opcional) |

---

## 🗂️ Estrutura de Arquivos

```
.github/
└── workflows/
    ├── ci.yml              # Pipeline de validação
    └── build-deploy.yml    # Pipeline de build e deploy

ops/
└── fastconsig.yml          # Stack do Docker Swarm

docker/
├── Dockerfile.api          # Dockerfile da API
├── Dockerfile.web          # Dockerfile do Frontend
├── Dockerfile.jobs         # Dockerfile do Worker
└── nginx/
    ├── nginx.conf          # Config Nginx
    └── default.conf        # Virtual host
```

---

## ⚙️ Workflows

### 1. CI Workflow (`ci.yml`)

**Trigger:**
- Push em `main` ou `development`
- Pull Requests para `main` ou `development`
- Manual (`workflow_dispatch`)

**Jobs:**

| Job | Descrição | Duração Aprox. |
|-----|-----------|----------------|
| **lint** | ESLint + Prettier | ~2 min |
| **typecheck** | TypeScript type check | ~2 min |
| **test** | Testes unitários + coverage | ~5 min |
| **build** | Build de todos os apps | ~5 min |
| **security** | Audit de segurança | ~2 min |

**Threshold de Coverage:**
- Mínimo: 90% (configurado no `vitest.config.ts`)
- Atual: **96%** ✅

### 2. Build & Deploy Workflow (`build-deploy.yml`)

**Trigger:**
- Push em `main` ou `development`
- Manual (`workflow_dispatch`)

**Jobs:**

#### Job 1: `build-and-push`

**Matriz de builds:**
```yaml
strategy:
  matrix:
    app: [api, web, jobs]
```

Para cada app:
1. Gera tag de imagem (git short hash)
2. Faz build do Dockerfile específico
3. Faz push para OCI Registry
4. Tag `latest` se for `main` branch

**Imagens geradas:**
```
gru.ocir.io/grnvzpym0ltz/fastconsig-api:<git-hash>
gru.ocir.io/grnvzpym0ltz/fastconsig-web:<git-hash>
gru.ocir.io/grnvzpym0ltz/fastconsig-jobs:<git-hash>
```

#### Job 2: `deploy`

1. Define ambiente baseado no branch:
   - `main` → `production`
   - `development` → `dev`

2. Configura variáveis de ambiente:
   ```bash
   # Produção
   STACK_NAME=fastconsig-production
   APP_URL=app.fastconsig.com.br
   API_URL=api.fastconsig.com.br
   DB_NAME=fastconsig_prod

   # Development
   STACK_NAME=fastconsig-dev
   APP_URL=dev-app.fastconsig.com.br
   API_URL=dev-api.fastconsig.com.br
   DB_NAME=fastconsig_dev
   ```

3. Executa migrations do Prisma

4. Faz deploy da stack no Swarm

5. Verifica health dos serviços

---

## 🚀 Deploy

### Estrutura da Stack

A stack completa inclui 5 serviços:

| Serviço | Réplicas | Porta | Descrição |
|---------|----------|-------|-----------|
| **fastconsig-api** | 2 | 3001 | Backend Fastify + tRPC |
| **fastconsig-web** | 2 | 80 | Frontend React (Nginx) |
| **fastconsig-jobs** | 1 | - | Worker BullMQ |
| **postgres** | 1 | 5432 | PostgreSQL 18 |
| **redis** | 1 | 6379 | Redis 7 |

### Volumes Persistentes

```yaml
volumes:
  postgres-data:   # Dados do PostgreSQL
  redis-data:      # Dados do Redis
```

### Networks

- **proxy_network** (external): Comunicação com Traefik (pública)
- **internal** (overlay): Comunicação entre serviços (privada)

### Health Checks

Todos os serviços possuem health checks configurados:

- **API**: `GET /health` (30s interval)
- **Web**: `GET /` (30s interval)
- **PostgreSQL**: `pg_isready` (10s interval)
- **Redis**: `redis-cli ping` (10s interval)
- **Jobs**: `pgrep` process check (60s interval)

### Recursos (CPU/Memory)

| Serviço | CPU Limit | Memory Limit | CPU Reserva | Memory Reserva |
|---------|-----------|--------------|-------------|----------------|
| API | - | - | - | - |
| Web | - | - | - | - |
| Jobs | 1 | 512M | 0.5 | 256M |
| PostgreSQL | 2 | 2G | 1 | 1G |
| Redis | 1 | 512M | 0.5 | 256M |

---

## 🔧 Comandos Úteis

### Verificar Status da Stack

```bash
# Listar stacks
docker stack ls

# Listar serviços da stack
docker stack services fastconsig-production

# Ver logs de um serviço
docker service logs fastconsig-production_fastconsig-api -f

# Ver réplicas de um serviço
docker service ps fastconsig-production_fastconsig-api
```

### Forçar Update de Serviço

```bash
# Forçar pull de nova imagem
docker service update --image <nova-imagem> fastconsig-production_fastconsig-api

# Forçar restart
docker service update --force fastconsig-production_fastconsig-api
```

### Remover Stack

```bash
# CUIDADO: Remove todos os serviços (volumes persistem)
docker stack rm fastconsig-production
```

### Backup do Banco de Dados

```bash
# Conectar no container do PostgreSQL
docker exec -it $(docker ps -q -f name=fastconsig-production_postgres) bash

# Dentro do container
pg_dump -U fastconsig_prod fastconsig_prod > /tmp/backup.sql

# Copiar backup para host
docker cp $(docker ps -q -f name=fastconsig-production_postgres):/tmp/backup.sql ./backup.sql
```

---

## 🐛 Troubleshooting

### 1. Build Falha no Pipeline

**Problema:** Build de imagem Docker falha

**Soluções:**
```bash
# Verificar logs do workflow no GitHub Actions

# Testar build localmente
docker build -f docker/Dockerfile.api -t test .

# Verificar espaço em disco no runner
df -h

# Limpar cache do Docker no runner
docker system prune -a -f
```

### 2. Deploy Falha

**Problema:** Deploy não atualiza serviços

**Soluções:**
```bash
# Verificar se imagem foi pushed
docker pull gru.ocir.io/grnvzpym0ltz/fastconsig-api:<hash>

# Verificar logs do deploy no GitHub Actions

# Verificar autenticação do registry
docker login gru.ocir.io -u <user>

# Forçar update manual
docker service update --image <nova-imagem> --force <service>
```

### 3. Serviços Não Ficam Healthy

**Problema:** Serviços não passam no health check

**Soluções:**
```bash
# Ver logs do serviço
docker service logs fastconsig-production_fastconsig-api -f

# Ver detalhes do serviço
docker service inspect fastconsig-production_fastconsig-api

# Verificar conectividade com DB
docker exec -it <container-id> sh
ping postgres

# Verificar variáveis de ambiente
docker service inspect fastconsig-production_fastconsig-api --format='{{json .Spec.TaskTemplate.ContainerSpec.Env}}' | jq
```

### 4. Migrations Falham

**Problema:** Prisma migrations não aplicam

**Soluções:**
```bash
# Executar migrations manualmente
docker run --rm \
  --network fastconsig-production_internal \
  -e DATABASE_URL="postgresql://user:pass@postgres:5432/fastconsig_prod" \
  gru.ocir.io/grnvzpym0ltz/fastconsig-api:<hash> \
  npx prisma migrate deploy

# Verificar status das migrations
npx prisma migrate status

# Resetar database (CUIDADO!)
npx prisma migrate reset
```

### 5. Problemas de Certificado SSL

**Problema:** Traefik não gera certificado

**Soluções:**
```bash
# Verificar configuração do Traefik
docker service logs <traefik-service> -f

# Verificar DNS
nslookup app.fastconsig.com.br

# Verificar regras do Traefik
docker service inspect fastconsig-production_fastconsig-web --format='{{json .Spec.Labels}}' | jq
```

### 6. Alto Uso de Recursos

**Problema:** Serviços consumindo muita CPU/memória

**Soluções:**
```bash
# Verificar uso de recursos
docker stats

# Ajustar limites de recursos
docker service update \
  --limit-cpu 1 \
  --limit-memory 512M \
  fastconsig-production_fastconsig-api

# Verificar logs para problemas
docker service logs fastconsig-production_fastconsig-api -f
```

---

## 📊 Monitoramento

### Métricas Importantes

1. **Taxa de Sucesso do CI**: >= 95%
2. **Tempo de Build**: < 10 minutos
3. **Tempo de Deploy**: < 5 minutos
4. **Uptime dos Serviços**: >= 99.5%
5. **Health Check Success Rate**: >= 99%

### Alertas Recomendados

- Deploy falhou
- Coverage abaixo de 90%
- Serviço ficou unhealthy por > 5 minutos
- Uso de disco > 80%
- Uso de memória > 90%

---

## 🔄 Atualizações Futuras

### Melhorias Planejadas

- [ ] Implementar blue/green deployment
- [ ] Adicionar smoke tests pós-deploy
- [ ] Configurar auto-scaling horizontal
- [ ] Implementar rollback automático em caso de falha
- [ ] Adicionar notificações Slack/Discord
- [ ] Configurar Datadog/Grafana para monitoramento

---

## 📝 Checklist de Setup Inicial

- [ ] Configurar runners self-hosted (build e deploy)
- [ ] Criar secrets no GitHub
- [ ] Criar rede `proxy_network` no Swarm
- [ ] Adicionar labels nos nodes do Swarm
- [ ] Configurar DNS (apontar domínios para Swarm)
- [ ] Configurar Traefik (se ainda não estiver)
- [ ] Fazer primeiro deploy manual para testar
- [ ] Configurar backup automático do banco
- [ ] Documentar procedimentos de rollback
- [ ] Treinar equipe nos comandos básicos

---

## 🆘 Suporte

**Em caso de problemas:**

1. Verificar logs do GitHub Actions
2. Verificar logs dos serviços no Swarm
3. Consultar seção de Troubleshooting
4. Abrir issue no repositório
5. Contactar Tech Lead

---

**Última Atualização:** 11 de Janeiro de 2026
**Versão:** 1.0
