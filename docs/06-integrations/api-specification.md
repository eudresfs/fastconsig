# API Specification - FastConsig

**Versao:** 1.0
**Data:** Janeiro 2026
**OpenAPI:** 3.1.0

---

## 1. Visao Geral

### 1.1 Informacoes Gerais

| Aspecto | Valor |
|---------|-------|
| **Base URL** | `https://api.fastconsig.com.br/api/v1` |
| **Versionamento** | Path-based (`/v1`, `/v2`) |
| **Formato** | JSON (application/json) |
| **Encoding** | UTF-8 |
| **Autenticacao** | Bearer Token (JWT) |

### 1.2 Autenticacao

Todas as requisicoes (exceto login) devem incluir o header:

```
Authorization: Bearer <access_token>
```

### 1.3 Headers Comuns

| Header | Descricao | Obrigatorio |
|--------|-----------|-------------|
| `Authorization` | Bearer token JWT | Sim* |
| `Content-Type` | application/json | Sim (POST/PUT) |
| `Accept` | application/json | Nao |
| `X-Request-ID` | UUID para rastreamento | Nao |

### 1.4 Rate Limiting

| Limite | Valor |
|--------|-------|
| **Por IP** | 100 requests/minuto |
| **Por Tenant** | 1000 requests/minuto |
| **Por Usuario** | 300 requests/minuto |

Headers de resposta:
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1704067200
```

### 1.5 Paginacao

Endpoints de listagem suportam paginacao:

| Parametro | Tipo | Default | Descricao |
|-----------|------|---------|-----------|
| `page` | integer | 1 | Numero da pagina |
| `per_page` | integer | 20 | Itens por pagina (max: 100) |
| `sort` | string | - | Campo para ordenacao |
| `order` | string | asc | Direcao (asc/desc) |

Resposta paginada:
```json
{
  "success": true,
  "data": [...],
  "meta": {
    "page": 1,
    "per_page": 20,
    "total": 150,
    "total_pages": 8,
    "has_next": true,
    "has_prev": false
  }
}
```

### 1.6 Formato de Resposta

**Sucesso:**
```json
{
  "success": true,
  "data": { ... },
  "message": "Operacao realizada com sucesso"
}
```

**Erro:**
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Dados invalidos",
    "details": [
      {
        "field": "cpf",
        "message": "CPF invalido"
      }
    ]
  }
}
```

---

## 2. Endpoints de Autenticacao

### 2.1 POST /auth/login

Autentica usuario e retorna tokens.

**Request:**
```json
{
  "login": "usuario@email.com",
  "password": "senha123"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "access_token": "eyJhbGciOiJIUzI1NiIs...",
    "refresh_token": "eyJhbGciOiJIUzI1NiIs...",
    "token_type": "Bearer",
    "expires_in": 900,
    "user": {
      "id": 1,
      "nome": "Joao Silva",
      "email": "joao@email.com",
      "perfil": "operador",
      "tenant_id": 1,
      "consignataria_id": null,
      "primeiro_acesso": false
    }
  }
}
```

**Response 401:**
```json
{
  "success": false,
  "error": {
    "code": "INVALID_CREDENTIALS",
    "message": "Usuario ou senha invalidos"
  }
}
```

**Response 423:**
```json
{
  "success": false,
  "error": {
    "code": "USER_LOCKED",
    "message": "Usuario bloqueado por excesso de tentativas",
    "details": {
      "unlock_at": "2026-01-15T10:30:00Z"
    }
  }
}
```

---

### 2.2 POST /auth/logout

Invalida o token atual.

**Headers:**
```
Authorization: Bearer <access_token>
```

**Response 200:**
```json
{
  "success": true,
  "message": "Logout realizado com sucesso"
}
```

---

### 2.3 POST /auth/refresh

Renova o access token usando refresh token.

**Request:**
```json
{
  "refresh_token": "eyJhbGciOiJIUzI1NiIs..."
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "access_token": "eyJhbGciOiJIUzI1NiIs...",
    "refresh_token": "eyJhbGciOiJIUzI1NiIs...",
    "expires_in": 900
  }
}
```

---

### 2.4 POST /auth/forgot-password

Solicita recuperacao de senha.

**Request:**
```json
{
  "email": "usuario@email.com"
}
```

**Response 200:**
```json
{
  "success": true,
  "message": "Se o email existir, um link de recuperacao sera enviado"
}
```

---

### 2.5 POST /auth/reset-password

Redefine a senha usando token de recuperacao.

**Request:**
```json
{
  "token": "abc123...",
  "password": "novaSenha123",
  "password_confirmation": "novaSenha123"
}
```

**Response 200:**
```json
{
  "success": true,
  "message": "Senha alterada com sucesso"
}
```

---

## 3. Endpoints de Funcionarios

### 3.1 GET /funcionarios

Lista funcionarios com filtros e paginacao.

**Query Parameters:**

| Parametro | Tipo | Descricao |
|-----------|------|-----------|
| `cpf` | string | Filtrar por CPF (parcial) |
| `nome` | string | Filtrar por nome (parcial) |
| `matricula` | string | Filtrar por matricula |
| `empresa_id` | integer | Filtrar por empresa |
| `situacao` | string | A/I/F/B/P |
| `page` | integer | Pagina |
| `per_page` | integer | Itens por pagina |

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "cpf": "12345678901",
      "nome": "JOAO DA SILVA",
      "matricula": "001234",
      "empresa": {
        "id": 1,
        "codigo": "EMP001",
        "nome": "PREFEITURA MUNICIPAL"
      },
      "situacao": "A",
      "situacao_descricao": "Ativo",
      "salario_bruto": 5500.00,
      "margem": {
        "total": 1925.00,
        "utilizada": 450.00,
        "disponivel": 1475.00
      },
      "created_at": "2024-01-15T10:30:00Z"
    }
  ],
  "meta": {
    "page": 1,
    "per_page": 20,
    "total": 150,
    "total_pages": 8
  }
}
```

---

### 3.2 GET /funcionarios/{id}

Retorna funcionario por ID.

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "cpf": "12345678901",
    "nome": "JOAO DA SILVA",
    "data_nascimento": "1980-03-15",
    "sexo": "M",
    "email": "joao@email.com",
    "telefone": "11999998888",
    "matricula": "001234",
    "empresa": {
      "id": 1,
      "codigo": "EMP001",
      "nome": "PREFEITURA MUNICIPAL"
    },
    "cargo": "ANALISTA",
    "data_admissao": "2010-02-01",
    "salario_bruto": 5500.00,
    "situacao": "A",
    "situacao_descricao": "Ativo",
    "dados_bancarios": {
      "banco": "001",
      "banco_nome": "BANCO DO BRASIL",
      "agencia": "1234",
      "conta": "56789-0",
      "tipo_conta": "CC"
    },
    "margem": {
      "total": 1925.00,
      "utilizada": 450.00,
      "disponivel": 1475.00,
      "emprestimo": {
        "total": 1650.00,
        "utilizada": 450.00,
        "disponivel": 1200.00
      },
      "cartao": {
        "total": 275.00,
        "utilizada": 0.00,
        "disponivel": 275.00
      }
    },
    "averbacoes_ativas": 1,
    "created_at": "2024-01-15T10:30:00Z",
    "updated_at": "2024-06-20T14:45:00Z"
  }
}
```

**Response 404:**
```json
{
  "success": false,
  "error": {
    "code": "NOT_FOUND",
    "message": "Funcionario nao encontrado"
  }
}
```

---

### 3.3 GET /funcionarios/cpf/{cpf}

Retorna funcionario por CPF.

**Path Parameters:**

| Parametro | Tipo | Descricao |
|-----------|------|-----------|
| `cpf` | string | CPF (apenas numeros) |

**Response:** Mesmo formato de GET /funcionarios/{id}

---

### 3.4 POST /funcionarios

Cria novo funcionario.

**Request:**
```json
{
  "cpf": "12345678901",
  "nome": "JOAO DA SILVA",
  "data_nascimento": "1980-03-15",
  "sexo": "M",
  "email": "joao@email.com",
  "telefone": "11999998888",
  "matricula": "001234",
  "empresa_id": 1,
  "cargo": "ANALISTA",
  "data_admissao": "2010-02-01",
  "salario_bruto": 5500.00,
  "situacao": "A",
  "dados_bancarios": {
    "banco": "001",
    "agencia": "1234",
    "conta": "56789-0",
    "tipo_conta": "CC"
  }
}
```

**Response 201:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "cpf": "12345678901",
    "nome": "JOAO DA SILVA",
    "...": "..."
  },
  "message": "Funcionario criado com sucesso"
}
```

**Response 400:**
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Dados invalidos",
    "details": [
      {"field": "cpf", "message": "CPF invalido"},
      {"field": "matricula", "message": "Matricula ja existe nesta empresa"}
    ]
  }
}
```

---

### 3.5 PUT /funcionarios/{id}

Atualiza funcionario.

**Request:**
```json
{
  "nome": "JOAO DA SILVA SANTOS",
  "email": "joao.santos@email.com",
  "salario_bruto": 6000.00
}
```

**Response 200:**
```json
{
  "success": true,
  "data": { "...": "..." },
  "message": "Funcionario atualizado com sucesso"
}
```

---

### 3.6 DELETE /funcionarios/{id}

Inativa funcionario (soft delete).

**Response 200:**
```json
{
  "success": true,
  "message": "Funcionario inativado com sucesso"
}
```

---

### 3.7 GET /funcionarios/{id}/margem

Retorna detalhes da margem do funcionario.

**Response 200:**
```json
{
  "success": true,
  "data": {
    "funcionario_id": 1,
    "salario_bruto": 5500.00,
    "percentual_margem": 35.00,
    "margem": {
      "total": 1925.00,
      "utilizada": 450.00,
      "disponivel": 1475.00,
      "reservada": 0.00
    },
    "por_produto": [
      {
        "produto": "EMPRESTIMO",
        "percentual": 30.00,
        "total": 1650.00,
        "utilizada": 450.00,
        "disponivel": 1200.00
      },
      {
        "produto": "CARTAO",
        "percentual": 5.00,
        "total": 275.00,
        "utilizada": 0.00,
        "disponivel": 275.00
      }
    ],
    "composicao": [
      {
        "averbacao_id": 123,
        "numero_contrato": "CTR2024001234",
        "consignataria": "BANCO DO BRASIL",
        "valor_parcela": 450.00,
        "parcelas_restantes": 36,
        "data_fim": "2027-01-01"
      }
    ]
  }
}
```

---

### 3.8 GET /funcionarios/{id}/averbacoes

Lista averbacoes do funcionario.

**Query Parameters:**

| Parametro | Tipo | Descricao |
|-----------|------|-----------|
| `situacao` | string | Filtrar por situacao |
| `ativas` | boolean | Apenas averbacoes ativas |

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": 123,
      "numero_contrato": "CTR2024001234",
      "consignataria": {
        "id": 1,
        "nome": "BANCO DO BRASIL"
      },
      "produto": "EMPRESTIMO",
      "tipo_operacao": "NOVO",
      "situacao": "DE",
      "situacao_descricao": "Descontada",
      "valor_total": 15000.00,
      "valor_parcela": 450.00,
      "parcelas_total": 48,
      "parcelas_pagas": 12,
      "parcelas_restantes": 36,
      "taxa_mensal": 1.99,
      "data_inicio_desconto": "2024-02-01",
      "data_fim_desconto": "2028-01-01"
    }
  ]
}
```

---

### 3.9 GET /funcionarios/{id}/historico

Retorna historico de alteracoes do funcionario.

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "data": "2024-06-20T14:45:00Z",
      "usuario": "admin@orgao.gov.br",
      "acao": "UPDATE",
      "alteracoes": [
        {
          "campo": "salario_bruto",
          "valor_anterior": "5500.00",
          "valor_novo": "6000.00"
        }
      ]
    }
  ]
}
```

---

## 4. Endpoints de Averbacoes

### 4.1 GET /averbacoes

Lista averbacoes com filtros.

**Query Parameters:**

| Parametro | Tipo | Descricao |
|-----------|------|-----------|
| `funcionario_id` | integer | Filtrar por funcionario |
| `consignataria_id` | integer | Filtrar por consignataria |
| `situacao` | string | AG/AP/RJ/EN/DE/SU/BL/LI/CA |
| `tipo_operacao` | string | N/R/C |
| `data_inicio` | date | Data de criacao (de) |
| `data_fim` | date | Data de criacao (ate) |

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": 123,
      "numero_contrato": "CTR2024001234",
      "funcionario": {
        "id": 1,
        "cpf": "12345678901",
        "nome": "JOAO DA SILVA",
        "matricula": "001234"
      },
      "consignataria": {
        "id": 1,
        "nome": "BANCO DO BRASIL"
      },
      "produto": {
        "id": 1,
        "nome": "EMPRESTIMO CONSIGNADO"
      },
      "tipo_operacao": "N",
      "tipo_operacao_descricao": "Novo",
      "situacao": "DE",
      "situacao_descricao": "Descontada",
      "valor_total": 15000.00,
      "valor_parcela": 450.00,
      "parcelas_total": 48,
      "taxa_mensal": 1.99,
      "cet_anual": 28.93,
      "data_contrato": "2024-01-10",
      "data_inicio_desconto": "2024-02-01",
      "data_fim_desconto": "2028-01-01",
      "created_at": "2024-01-10T15:30:00Z"
    }
  ],
  "meta": { "...": "..." }
}
```

---

### 4.2 GET /averbacoes/{id}

Retorna averbacao por ID.

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": 123,
    "numero_contrato": "CTR2024001234",
    "funcionario": {
      "id": 1,
      "cpf": "12345678901",
      "nome": "JOAO DA SILVA",
      "matricula": "001234",
      "empresa": "PREFEITURA MUNICIPAL"
    },
    "consignataria": {
      "id": 1,
      "cnpj": "00000000000191",
      "nome": "BANCO DO BRASIL"
    },
    "produto": {
      "id": 1,
      "codigo": "EMP01",
      "nome": "EMPRESTIMO CONSIGNADO"
    },
    "tipo_operacao": "N",
    "tipo_operacao_descricao": "Novo",
    "situacao": "DE",
    "situacao_descricao": "Descontada",
    "valor_total": 15000.00,
    "valor_liquido": 14670.00,
    "valor_parcela": 450.00,
    "parcelas_total": 48,
    "parcelas_pagas": 12,
    "parcelas_restantes": 36,
    "saldo_devedor": 12500.00,
    "taxa_mensal": 1.99,
    "taxa_anual": 26.68,
    "cet_mensal": 2.15,
    "cet_anual": 28.93,
    "iof": 180.00,
    "tac": 150.00,
    "data_contrato": "2024-01-10",
    "data_inicio_desconto": "2024-02-01",
    "data_fim_desconto": "2028-01-01",
    "averbacao_vinculada": null,
    "historico": [
      {
        "data": "2024-01-10T15:30:00Z",
        "situacao": "AG",
        "usuario": "agente@banco.com"
      },
      {
        "data": "2024-01-11T09:00:00Z",
        "situacao": "AP",
        "usuario": "aprovador@orgao.gov.br"
      },
      {
        "data": "2024-01-15T08:00:00Z",
        "situacao": "EN",
        "usuario": "sistema"
      },
      {
        "data": "2024-02-05T10:00:00Z",
        "situacao": "DE",
        "usuario": "sistema"
      }
    ],
    "created_at": "2024-01-10T15:30:00Z",
    "updated_at": "2024-02-05T10:00:00Z"
  }
}
```

---

### 4.3 POST /averbacoes

Cria nova averbacao.

**Request:**
```json
{
  "funcionario_id": 1,
  "produto_id": 1,
  "tipo_operacao": "N",
  "numero_contrato": "CTR2024001234",
  "valor_total": 15000.00,
  "parcelas": 48,
  "valor_parcela": 450.00,
  "taxa_mensal": 1.99,
  "taxa_anual": 26.68,
  "cet_mensal": 2.15,
  "cet_anual": 28.93,
  "iof": 180.00,
  "tac": 150.00,
  "valor_liquido": 14670.00,
  "data_contrato": "2024-01-10",
  "data_inicio_desconto": "2024-02-01"
}
```

**Response 201:**
```json
{
  "success": true,
  "data": {
    "id": 123,
    "numero_contrato": "CTR2024001234",
    "situacao": "AG",
    "situacao_descricao": "Aguardando Aprovacao",
    "...": "..."
  },
  "message": "Averbacao criada com sucesso"
}
```

**Response 400:**
```json
{
  "success": false,
  "error": {
    "code": "MARGEM_INSUFICIENTE",
    "message": "Margem disponivel insuficiente",
    "details": {
      "margem_disponivel": 400.00,
      "valor_parcela": 450.00
    }
  }
}
```

---

### 4.4 PUT /averbacoes/{id}

Atualiza averbacao (apenas campos permitidos pelo status).

**Request:**
```json
{
  "numero_contrato": "CTR2024001234-A"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": { "...": "..." },
  "message": "Averbacao atualizada com sucesso"
}
```

---

### 4.5 POST /averbacoes/{id}/aprovar

Aprova averbacao pendente.

**Request:**
```json
{
  "observacao": "Documentacao verificada"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": 123,
    "situacao": "AP",
    "situacao_descricao": "Aprovada",
    "data_aprovacao": "2024-01-11T09:00:00Z",
    "aprovador": "aprovador@orgao.gov.br"
  },
  "message": "Averbacao aprovada com sucesso"
}
```

---

### 4.6 POST /averbacoes/{id}/rejeitar

Rejeita averbacao pendente.

**Request:**
```json
{
  "motivo": "DOCUMENTACAO_INVALIDA",
  "observacao": "Contrato sem assinatura do funcionario"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": 123,
    "situacao": "RJ",
    "situacao_descricao": "Rejeitada",
    "motivo_rejeicao": "Contrato sem assinatura do funcionario"
  },
  "message": "Averbacao rejeitada"
}
```

---

### 4.7 POST /averbacoes/{id}/suspender

Suspende averbacao temporariamente.

**Request:**
```json
{
  "motivo": "Funcionario afastado por licenca medica",
  "previsao_retorno": "2024-06-01"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": 123,
    "situacao": "SU",
    "situacao_descricao": "Suspensa"
  },
  "message": "Averbacao suspensa"
}
```

---

### 4.8 POST /averbacoes/{id}/cancelar

Cancela averbacao.

**Request:**
```json
{
  "motivo": "Solicitacao do cliente"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": 123,
    "situacao": "CA",
    "situacao_descricao": "Cancelada",
    "margem_liberada": 450.00
  },
  "message": "Averbacao cancelada"
}
```

---

### 4.9 POST /averbacoes/{id}/liquidar

Registra liquidacao/quitacao da averbacao.

**Request:**
```json
{
  "data_liquidacao": "2024-06-15",
  "valor_quitado": 12500.00,
  "observacao": "Quitacao antecipada"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": 123,
    "situacao": "LI",
    "situacao_descricao": "Liquidada",
    "data_liquidacao": "2024-06-15",
    "margem_liberada": 450.00
  },
  "message": "Averbacao liquidada"
}
```

---

## 5. Endpoints de Simulacoes

### 5.1 POST /simulacoes/emprestimo

Simula emprestimo consignado.

**Request:**
```json
{
  "funcionario_id": 1,
  "produto_id": 1,
  "tipo_simulacao": "VALOR",
  "valor": 15000.00,
  "prazos": [24, 36, 48, 60]
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "funcionario": {
      "id": 1,
      "nome": "JOAO DA SILVA",
      "margem_disponivel": 1475.00
    },
    "simulacoes": [
      {
        "prazo": 24,
        "valor_total": 15000.00,
        "valor_parcela": 750.00,
        "valor_liquido": 14520.00,
        "taxa_mensal": 1.89,
        "taxa_anual": 25.12,
        "cet_mensal": 2.05,
        "cet_anual": 27.45,
        "iof": 180.00,
        "tac": 300.00,
        "total_juros": 3000.00,
        "dentro_margem": true
      },
      {
        "prazo": 36,
        "valor_total": 15000.00,
        "valor_parcela": 550.00,
        "valor_liquido": 14520.00,
        "taxa_mensal": 1.95,
        "taxa_anual": 25.95,
        "cet_mensal": 2.10,
        "cet_anual": 28.10,
        "iof": 180.00,
        "tac": 300.00,
        "total_juros": 4800.00,
        "dentro_margem": true
      },
      {
        "prazo": 48,
        "valor_total": 15000.00,
        "valor_parcela": 450.00,
        "valor_liquido": 14520.00,
        "taxa_mensal": 1.99,
        "taxa_anual": 26.68,
        "cet_mensal": 2.15,
        "cet_anual": 28.93,
        "iof": 180.00,
        "tac": 300.00,
        "total_juros": 6600.00,
        "dentro_margem": true
      },
      {
        "prazo": 60,
        "valor_total": 15000.00,
        "valor_parcela": 395.00,
        "valor_liquido": 14520.00,
        "taxa_mensal": 2.05,
        "taxa_anual": 27.55,
        "cet_mensal": 2.20,
        "cet_anual": 29.75,
        "iof": 180.00,
        "tac": 300.00,
        "total_juros": 8700.00,
        "dentro_margem": true
      }
    ],
    "validade": "2024-01-17T23:59:59Z"
  }
}
```

---

### 5.2 POST /simulacoes/compra-divida

Simula compra de divida/portabilidade.

**Request:**
```json
{
  "funcionario_id": 1,
  "produto_id": 1,
  "dividas": [
    {
      "averbacao_id": 100,
      "saldo_devedor": 8000.00
    },
    {
      "averbacao_id": 101,
      "saldo_devedor": 5000.00
    }
  ],
  "prazo": 48
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "funcionario": {
      "id": 1,
      "nome": "JOAO DA SILVA"
    },
    "dividas_consolidadas": {
      "quantidade": 2,
      "saldo_total": 13000.00,
      "parcela_atual_total": 650.00
    },
    "proposta": {
      "prazo": 48,
      "valor_total": 15000.00,
      "valor_parcela": 450.00,
      "troco_cliente": 1700.00,
      "taxa_mensal": 1.99,
      "economia_mensal": 200.00
    },
    "comparativo": {
      "situacao_atual": {
        "parcela_total": 650.00,
        "prazo_medio_restante": 24
      },
      "nova_situacao": {
        "parcela_total": 450.00,
        "prazo": 48
      }
    }
  }
}
```

---

## 6. Endpoints de Consignatarias

### 6.1 GET /consignatarias

Lista consignatarias do convenio.

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "cnpj": "00000000000191",
      "razao_social": "BANCO DO BRASIL S.A.",
      "nome_fantasia": "BANCO DO BRASIL",
      "situacao": "A",
      "situacao_descricao": "Ativa",
      "convenio": {
        "data_inicio": "2020-01-01",
        "data_fim": "2025-12-31"
      },
      "produtos_ativos": 3,
      "averbacoes_ativas": 250
    }
  ]
}
```

---

### 6.2 GET /consignatarias/{id}

Retorna consignataria por ID.

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "cnpj": "00000000000191",
    "razao_social": "BANCO DO BRASIL S.A.",
    "nome_fantasia": "BANCO DO BRASIL",
    "endereco": {
      "logradouro": "Av. Paulista, 1000",
      "cidade": "Sao Paulo",
      "uf": "SP",
      "cep": "01310-100"
    },
    "contato": {
      "nome": "Joao Gerente",
      "email": "consignado@bb.com.br",
      "telefone": "1140041234"
    },
    "dados_bancarios": {
      "banco": "001",
      "agencia": "1234",
      "conta": "12345-6"
    },
    "situacao": "A",
    "convenio": {
      "data_inicio": "2020-01-01",
      "data_fim": "2025-12-31",
      "limite_valor": 50000.00,
      "limite_prazo": 96
    },
    "produtos": [
      {
        "id": 1,
        "nome": "EMPRESTIMO CONSIGNADO",
        "ativo": true
      }
    ],
    "estatisticas": {
      "averbacoes_ativas": 250,
      "valor_carteira": 3500000.00,
      "producao_mes": 150000.00
    }
  }
}
```

---

## 7. Endpoints de Relatorios

### 7.1 GET /relatorios/producao

Relatorio de producao de averbacoes.

**Query Parameters:**

| Parametro | Tipo | Descricao |
|-----------|------|-----------|
| `data_inicio` | date | Data inicial |
| `data_fim` | date | Data final |
| `consignataria_id` | integer | Filtrar por consignataria |
| `produto_id` | integer | Filtrar por produto |
| `agrupamento` | string | dia/semana/mes/consignataria |
| `formato` | string | json/xlsx/pdf |

**Response 200 (JSON):**
```json
{
  "success": true,
  "data": {
    "periodo": {
      "inicio": "2024-01-01",
      "fim": "2024-01-31"
    },
    "resumo": {
      "total_averbacoes": 150,
      "valor_total": 2250000.00,
      "ticket_medio": 15000.00
    },
    "por_consignataria": [
      {
        "consignataria": "BANCO DO BRASIL",
        "quantidade": 80,
        "valor": 1200000.00,
        "percentual": 53.3
      },
      {
        "consignataria": "CAIXA ECONOMICA",
        "quantidade": 70,
        "valor": 1050000.00,
        "percentual": 46.7
      }
    ],
    "evolucao": [
      {"data": "2024-01-01", "quantidade": 5, "valor": 75000.00},
      {"data": "2024-01-02", "quantidade": 8, "valor": 120000.00}
    ]
  }
}
```

---

### 7.2 GET /relatorios/margem

Relatorio de posicao de margem.

**Response 200:**
```json
{
  "success": true,
  "data": {
    "resumo": {
      "total_funcionarios": 5000,
      "margem_total": 17500000.00,
      "margem_utilizada": 7000000.00,
      "margem_disponivel": 10500000.00,
      "percentual_utilizacao": 40.0
    },
    "por_empresa": [
      {
        "empresa": "PREFEITURA",
        "funcionarios": 3000,
        "margem_total": 10500000.00,
        "utilizacao": 42.0
      }
    ]
  }
}
```

---

### 7.3 GET /relatorios/inadimplencia

Relatorio de averbacoes nao descontadas.

**Response 200:**
```json
{
  "success": true,
  "data": {
    "competencia": "01/2024",
    "resumo": {
      "total_enviado": 500,
      "valor_enviado": 225000.00,
      "total_descontado": 480,
      "valor_descontado": 216000.00,
      "total_nao_descontado": 20,
      "valor_nao_descontado": 9000.00,
      "taxa_inadimplencia": 4.0
    },
    "por_motivo": [
      {"motivo": "Funcionario afastado", "quantidade": 10, "valor": 4500.00},
      {"motivo": "Margem insuficiente", "quantidade": 5, "valor": 2250.00},
      {"motivo": "Funcionario exonerado", "quantidade": 5, "valor": 2250.00}
    ],
    "detalhes": [
      {
        "funcionario": "JOAO DA SILVA",
        "cpf": "123.***.***-01",
        "contrato": "CTR001",
        "consignataria": "BANCO DO BRASIL",
        "valor": 450.00,
        "motivo": "Funcionario afastado"
      }
    ]
  }
}
```

---

## 8. Schemas/Models

### 8.1 Funcionario

```json
{
  "type": "object",
  "properties": {
    "id": {"type": "integer"},
    "cpf": {"type": "string", "pattern": "^[0-9]{11}$"},
    "nome": {"type": "string", "maxLength": 100},
    "data_nascimento": {"type": "string", "format": "date"},
    "sexo": {"type": "string", "enum": ["M", "F"]},
    "email": {"type": "string", "format": "email"},
    "telefone": {"type": "string"},
    "matricula": {"type": "string", "maxLength": 20},
    "empresa_id": {"type": "integer"},
    "cargo": {"type": "string"},
    "data_admissao": {"type": "string", "format": "date"},
    "salario_bruto": {"type": "number"},
    "situacao": {"type": "string", "enum": ["A", "I", "F", "B", "P"]}
  },
  "required": ["cpf", "nome", "matricula", "empresa_id", "data_admissao", "salario_bruto", "situacao"]
}
```

### 8.2 Averbacao

```json
{
  "type": "object",
  "properties": {
    "id": {"type": "integer"},
    "numero_contrato": {"type": "string", "maxLength": 30},
    "funcionario_id": {"type": "integer"},
    "consignataria_id": {"type": "integer"},
    "produto_id": {"type": "integer"},
    "tipo_operacao": {"type": "string", "enum": ["N", "R", "C"]},
    "situacao": {"type": "string", "enum": ["AG", "AP", "RJ", "EN", "DE", "SU", "BL", "LI", "CA"]},
    "valor_total": {"type": "number"},
    "valor_parcela": {"type": "number"},
    "parcelas_total": {"type": "integer"},
    "taxa_mensal": {"type": "number"},
    "data_contrato": {"type": "string", "format": "date"},
    "data_inicio_desconto": {"type": "string", "format": "date"},
    "data_fim_desconto": {"type": "string", "format": "date"}
  }
}
```

### 8.3 Error

```json
{
  "type": "object",
  "properties": {
    "code": {"type": "string"},
    "message": {"type": "string"},
    "details": {
      "type": "array",
      "items": {
        "type": "object",
        "properties": {
          "field": {"type": "string"},
          "message": {"type": "string"}
        }
      }
    }
  }
}
```

---

## 9. Codigos de Erro

| Codigo | HTTP | Descricao |
|--------|------|-----------|
| `VALIDATION_ERROR` | 400 | Dados de entrada invalidos |
| `INVALID_CREDENTIALS` | 401 | Usuario ou senha invalidos |
| `TOKEN_EXPIRED` | 401 | Token JWT expirado |
| `TOKEN_INVALID` | 401 | Token JWT invalido |
| `FORBIDDEN` | 403 | Sem permissao para a acao |
| `NOT_FOUND` | 404 | Recurso nao encontrado |
| `CONFLICT` | 409 | Conflito (ex: duplicidade) |
| `USER_LOCKED` | 423 | Usuario bloqueado |
| `RATE_LIMIT_EXCEEDED` | 429 | Limite de requisicoes excedido |
| `INTERNAL_ERROR` | 500 | Erro interno do servidor |
| `MARGEM_INSUFICIENTE` | 400 | Margem disponivel insuficiente |
| `FUNCIONARIO_BLOQUEADO` | 400 | Funcionario bloqueado para averbacoes |
| `CONSIGNATARIA_INATIVA` | 400 | Consignataria suspensa ou inativa |
| `OPERACAO_NAO_PERMITIDA` | 400 | Operacao nao permitida para o status atual |

---

## 10. Historico de Revisoes

| Versao | Data | Autor | Descricao |
|--------|------|-------|-----------|
| 1.0 | Janeiro 2026 | Tech Team | Versao inicial |

---

*Esta especificacao define os endpoints REST do sistema FastConsig e deve ser mantida sincronizada com a implementacao.*
