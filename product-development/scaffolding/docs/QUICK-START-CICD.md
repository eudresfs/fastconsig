# Quick Start - CI/CD FastConsig

Guia rápido para configurar o CI/CD do zero.

---

## ⚡ Setup Rápido (15 minutos)

### 1️⃣ Gerar Secrets (2 minutos)

```bash
# Gerar JWT Secrets (executar localmente)
echo "JWT_ACCESS_SECRET=$(openssl rand -base64 64)"
echo "JWT_REFRESH_SECRET=$(openssl rand -base64 64)"

# Gerar senha do banco (executar localmente)
echo "DB_PASSWORD=$(openssl rand -base64 32)"
```

**Copie os valores gerados e guarde em local seguro!**

---

### 2️⃣ Configurar Secrets no GitHub (5 minutos)

Vá para: `Settings` → `Secrets and variables` → `Actions` → `New repository secret`

Adicione os seguintes secrets:

```yaml
OCI_USERNAME: "grnvzpym0ltz/seu-usuario"
OCI_TOKEN: "seu-auth-token-do-oci"
DB_PASSWORD: "valor-gerado-no-passo-1"
JWT_ACCESS_SECRET: "valor-gerado-no-passo-1"
JWT_REFRESH_SECRET: "valor-gerado-no-passo-1"
```

**Opcional (Email):**
```yaml
SMTP_HOST: "smtp.gmail.com"
SMTP_PORT: "587"
SMTP_USER: "seu-email@gmail.com"
SMTP_PASSWORD: "sua-senha-de-app"
```

---

### 3️⃣ Preparar Docker Swarm (3 minutos)

```bash
# Criar rede do proxy (se não existir)
docker network create --driver overlay proxy_network

# Adicionar label aos nodes
docker node update --label-add workload=worker $(docker node ls -q)

# Verificar
docker node ls -f label=workload=worker
docker network ls | grep proxy_network
```

---

### 4️⃣ Configurar Runners (5 minutos)

**No servidor de BUILD:**
```bash
# Baixar e instalar GitHub Actions Runner
mkdir actions-runner-build && cd actions-runner-build
curl -o actions-runner-linux-x64-2.313.0.tar.gz -L https://github.com/actions/runner/releases/download/v2.313.0/actions-runner-linux-x64-2.313.0.tar.gz
tar xzf ./actions-runner-linux-x64-2.313.0.tar.gz

# Configurar (usar token do GitHub)
./config.sh --url https://github.com/seu-org/fastconsig --token SEU_TOKEN --labels self-hosted,build

# Iniciar
./run.sh
```

**No servidor de DEPLOY (Swarm Manager):**
```bash
# Baixar e instalar GitHub Actions Runner
mkdir actions-runner-deploy && cd actions-runner-deploy
curl -o actions-runner-linux-x64-2.313.0.tar.gz -L https://github.com/actions/runner/releases/download/v2.313.0/actions-runner-linux-x64-2.313.0.tar.gz
tar xzf ./actions-runner-linux-x64-2.313.0.tar.gz

# Configurar (usar token do GitHub)
./config.sh --url https://github.com/seu-org/fastconsig --token SEU_TOKEN --labels self-hosted,deploy

# Iniciar
./run.sh
```

---

### 5️⃣ Primeiro Deploy (Manual)

```bash
# Clonar repositório no servidor de deploy
git clone https://github.com/seu-org/fastconsig.git
cd fastconsig/product-development/scaffolding

# Definir variáveis
export IMAGE_TAG=$(git rev-parse --short HEAD)
export OCI_REGISTRY="gru.ocir.io/grnvzpym0ltz"
export STACK_NAME="fastconsig-dev"
export NODE_ENV="development"
export APP_URL="dev-app.fastconsig.com.br"
export API_URL="dev-api.fastconsig.com.br"
export DB_NAME="fastconsig_dev"
export DB_USER="fastconsig_prod"
export DB_PASSWORD="sua-senha-do-secret"
export JWT_ACCESS_SECRET="seu-secret-do-github"
export JWT_REFRESH_SECRET="seu-secret-do-github"
export SMTP_HOST="smtp.gmail.com"
export SMTP_PORT="587"
export SMTP_USER=""
export SMTP_PASSWORD=""

# Login no registry
echo "seu-oci-token" | docker login gru.ocir.io -u grnvzpym0ltz/seu-usuario --password-stdin

# Deploy
docker stack deploy -c ops/fastconsig.yml fastconsig-dev --with-registry-auth --detach=true

# Verificar
docker stack services fastconsig-dev
docker stack ps fastconsig-dev
```

---

### 6️⃣ Verificar Deploy

```bash
# Aguardar serviços ficarem healthy (~2 minutos)
watch -n 2 'docker stack services fastconsig-dev'

# Verificar logs
docker service logs fastconsig-dev_fastconsig-api -f --tail 50
docker service logs fastconsig-dev_fastconsig-web -f --tail 50
docker service logs fastconsig-dev_postgres -f --tail 50

# Testar endpoints
curl https://dev-api.fastconsig.com.br/health
curl https://dev-app.fastconsig.com.br
```

---

## 🚀 Automatizar Deployments

Após o setup manual funcionar:

### Push para Development

```bash
git checkout development
git add .
git commit -m "feat: nova funcionalidade"
git push origin development

# GitHub Actions irá:
# 1. Rodar CI (lint, test, typecheck, build)
# 2. Build das imagens Docker
# 3. Push para OCI Registry
# 4. Deploy automático em dev-*.fastconsig.com.br
```

### Push para Production

```bash
git checkout main
git merge development
git push origin main

# GitHub Actions irá:
# 1. Rodar CI (lint, test, typecheck, build)
# 2. Build das imagens Docker
# 3. Push para OCI Registry (+ tag :latest)
# 4. Deploy automático em *.fastconsig.com.br
```

---

## ✅ Checklist de Validação

Marque conforme completa:

- [ ] Secrets configurados no GitHub
- [ ] Rede `proxy_network` criada
- [ ] Nodes com label `workload=worker`
- [ ] Runner `build` configurado e online
- [ ] Runner `deploy` configurado e online
- [ ] DNS apontando para servidor Swarm
- [ ] Primeiro deploy manual funcionou
- [ ] API respondendo em `/health`
- [ ] Web carregando corretamente
- [ ] PostgreSQL healthy
- [ ] Redis healthy
- [ ] Jobs worker rodando
- [ ] CI passou em pull request de teste
- [ ] Deploy automático funcionou

---

## 🎯 Próximos Passos

Após o CI/CD funcionando:

1. **Configurar Monitoramento**
   - Integrar Datadog/Grafana
   - Configurar alertas
   - Criar dashboards

2. **Implementar Sprint 2**
   - US-003: Recuperação de senha
   - US-060: Reset senha por admin
   - Testes (manter 96% coverage)

3. **Documentação**
   - Runbook operacional
   - Procedimentos de rollback
   - Guia de troubleshooting

4. **Otimizações**
   - Configurar CDN para assets estáticos
   - Implementar rate limiting
   - Configurar backup automático

---

## 📞 Ajuda Rápida

**Problema:** CI falhando
```bash
# Verificar workflow
https://github.com/seu-org/fastconsig/actions

# Ver logs detalhados
Clique no workflow → Job → Step
```

**Problema:** Deploy não atualizando
```bash
# Forçar pull da imagem
docker service update --image <imagem>:latest --force <service>

# Verificar registry
docker pull gru.ocir.io/grnvzpym0ltz/fastconsig-api:latest
```

**Problema:** Serviço unhealthy
```bash
# Ver logs
docker service logs <service> -f --tail 100

# Ver detalhes
docker service ps <service> --no-trunc
```

---

## 🔗 Links Úteis

- [Documentação Completa](./CICD-SETUP.md)
- [GitHub Actions Docs](https://docs.github.com/en/actions)
- [Docker Swarm Docs](https://docs.docker.com/engine/swarm/)
- [Traefik Docs](https://doc.traefik.io/traefik/)

---

**Tempo total estimado:** 15-20 minutos
**Dificuldade:** Intermediário
**Pré-requisitos:** Docker Swarm + Traefik funcionando
