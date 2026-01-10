# Layouts de Arquivos - FastConsig

**Versao:** 1.0
**Data:** Janeiro 2026
**Status:** Draft

---

## 1. Visao Geral

Este documento especifica os layouts de arquivos para importacao e exportacao de dados no sistema FastConsig. Os layouts foram projetados para compatibilidade com sistemas de folha de pagamento e instituicoes financeiras.

### 1.1 Formatos Suportados

| Formato | Extensao | Uso Principal |
|---------|----------|---------------|
| Excel | .xlsx | Importacao/Exportacao geral |
| CSV | .csv | Integracao com sistemas |
| TXT Posicional | .txt | Folha de pagamento legada |

### 1.2 Convencoes Gerais

| Aspecto | Padrao |
|---------|--------|
| Encoding | UTF-8 (Excel/CSV) ou ISO-8859-1 (TXT) |
| Separador CSV | Ponto e virgula (;) |
| Decimal | Virgula (,) |
| Data | DD/MM/YYYY ou YYYYMMDD (posicional) |
| Booleano | S/N ou 1/0 |
| Texto | Sem aspas, trim automatico |
| Numerico | Sem separador de milhar |

---

## 2. Layouts de Importacao

### 2.1 Importacao de Funcionarios

#### 2.1.1 Formato Excel/CSV

**Nome do Layout:** `IMP_FUNCIONARIOS_V1`

**Descricao:** Importacao de cadastro de funcionarios (servidores).

| # | Campo | Tipo | Tamanho | Obrigatorio | Formato | Descricao |
|---|-------|------|---------|-------------|---------|-----------|
| 1 | CPF | Texto | 11 | Sim | 99999999999 | CPF sem pontuacao |
| 2 | NOME | Texto | 100 | Sim | | Nome completo |
| 3 | DATA_NASCIMENTO | Data | 10 | Sim | DD/MM/YYYY | Data de nascimento |
| 4 | SEXO | Texto | 1 | Nao | M/F | Masculino/Feminino |
| 5 | MATRICULA | Texto | 20 | Sim | | Matricula funcional |
| 6 | CODIGO_EMPRESA | Texto | 10 | Sim | | Codigo da empresa/orgao |
| 7 | CARGO | Texto | 100 | Nao | | Descricao do cargo |
| 8 | DATA_ADMISSAO | Data | 10 | Sim | DD/MM/YYYY | Data de admissao |
| 9 | SALARIO_BRUTO | Numerico | 15,2 | Sim | 9999999999999,99 | Salario bruto |
| 10 | SITUACAO | Texto | 1 | Sim | A/I/F/B/P | Ativo/Inativo/Afastado/Bloqueado/Aposentado |
| 11 | EMAIL | Texto | 100 | Nao | | Email do funcionario |
| 12 | TELEFONE | Texto | 20 | Nao | | Telefone com DDD |
| 13 | BANCO | Texto | 3 | Nao | 999 | Codigo do banco |
| 14 | AGENCIA | Texto | 10 | Nao | | Numero da agencia |
| 15 | CONTA | Texto | 20 | Nao | | Numero da conta |
| 16 | TIPO_CONTA | Texto | 2 | Nao | CC/CP | Corrente/Poupanca |

**Exemplo CSV:**
```
CPF;NOME;DATA_NASCIMENTO;SEXO;MATRICULA;CODIGO_EMPRESA;CARGO;DATA_ADMISSAO;SALARIO_BRUTO;SITUACAO;EMAIL;TELEFONE;BANCO;AGENCIA;CONTA;TIPO_CONTA
12345678901;JOAO DA SILVA;15/03/1980;M;001234;EMP001;ANALISTA;01/02/2010;5500,00;A;joao@email.com;11999998888;001;1234;56789-0;CC
98765432100;MARIA SANTOS;22/07/1985;F;001235;EMP001;TECNICO;15/06/2015;3800,50;A;;11988887777;104;5678;12345-6;CP
```

**Validacoes:**
- CPF deve ser valido (digitos verificadores)
- Matricula unica por empresa
- Data de admissao <= data atual
- Salario bruto > 0
- Codigo empresa deve existir no sistema

---

#### 2.1.2 Formato TXT Posicional

**Nome do Layout:** `IMP_FUNCIONARIOS_POS_V1`

| Posicao | Tamanho | Campo | Tipo | Formato |
|---------|---------|-------|------|---------|
| 1-11 | 11 | CPF | N | Zeros a esquerda |
| 12-111 | 100 | NOME | A | Espacos a direita |
| 112-119 | 8 | DATA_NASCIMENTO | N | YYYYMMDD |
| 120-120 | 1 | SEXO | A | M/F |
| 121-140 | 20 | MATRICULA | A | Espacos a direita |
| 141-150 | 10 | CODIGO_EMPRESA | A | Espacos a direita |
| 151-250 | 100 | CARGO | A | Espacos a direita |
| 251-258 | 8 | DATA_ADMISSAO | N | YYYYMMDD |
| 259-273 | 15 | SALARIO_BRUTO | N | Zeros, 2 decimais implicitos |
| 274-274 | 1 | SITUACAO | A | A/I/F/B/P |

**Exemplo:**
```
12345678901JOAO DA SILVA                                                                                       19800315M001234              EMP001    ANALISTA                                                                                    20100201000000550000A
```

---

### 2.2 Importacao de Contratos/Averbacoes

#### 2.2.1 Formato Excel/CSV

**Nome do Layout:** `IMP_CONTRATOS_V1`

| # | Campo | Tipo | Tamanho | Obrigatorio | Formato | Descricao |
|---|-------|------|---------|-------------|---------|-----------|
| 1 | CPF | Texto | 11 | Sim | 99999999999 | CPF do funcionario |
| 2 | MATRICULA | Texto | 20 | Sim | | Matricula funcional |
| 3 | NUMERO_CONTRATO | Texto | 30 | Sim | | Numero do contrato na consignataria |
| 4 | CODIGO_CONSIGNATARIA | Texto | 10 | Sim | | Codigo da consignataria |
| 5 | CODIGO_PRODUTO | Texto | 10 | Sim | | Codigo do produto |
| 6 | TIPO_OPERACAO | Texto | 1 | Sim | N/R/C | Novo/Refinanciamento/Compra |
| 7 | VALOR_TOTAL | Numerico | 15,2 | Sim | | Valor total financiado |
| 8 | QTD_PARCELAS | Numerico | 3 | Sim | | Quantidade de parcelas |
| 9 | VALOR_PARCELA | Numerico | 15,2 | Sim | | Valor da parcela |
| 10 | TAXA_MENSAL | Numerico | 6,4 | Sim | | Taxa de juros mensal (%) |
| 11 | TAXA_ANUAL | Numerico | 6,4 | Nao | | Taxa de juros anual (%) |
| 12 | CET_MENSAL | Numerico | 6,4 | Nao | | CET mensal (%) |
| 13 | CET_ANUAL | Numerico | 6,4 | Nao | | CET anual (%) |
| 14 | DATA_CONTRATO | Data | 10 | Sim | DD/MM/YYYY | Data de assinatura |
| 15 | DATA_INICIO_DESCONTO | Data | 10 | Sim | DD/MM/YYYY | Competencia inicial |
| 16 | DATA_FIM_DESCONTO | Data | 10 | Sim | DD/MM/YYYY | Competencia final |
| 17 | CONTRATO_VINCULADO | Texto | 30 | Nao | | Contrato liquidado (refin/compra) |
| 18 | IOF | Numerico | 15,2 | Nao | | Valor do IOF |
| 19 | TAC | Numerico | 15,2 | Nao | | Tarifa de abertura |
| 20 | VALOR_LIQUIDO | Numerico | 15,2 | Nao | | Valor creditado ao cliente |

**Exemplo CSV:**
```
CPF;MATRICULA;NUMERO_CONTRATO;CODIGO_CONSIGNATARIA;CODIGO_PRODUTO;TIPO_OPERACAO;VALOR_TOTAL;QTD_PARCELAS;VALOR_PARCELA;TAXA_MENSAL;TAXA_ANUAL;CET_MENSAL;CET_ANUAL;DATA_CONTRATO;DATA_INICIO_DESCONTO;DATA_FIM_DESCONTO;CONTRATO_VINCULADO;IOF;TAC;VALOR_LIQUIDO
12345678901;001234;CTR2024001234;BANCO01;EMP01;N;15000,00;48;450,00;1,99;26,68;2,15;28,93;10/01/2024;01/02/2024;01/01/2028;;180,00;150,00;14670,00
```

**Validacoes:**
- CPF + Matricula deve existir no sistema
- Consignataria deve estar ativa e conveniada
- Produto deve estar ativo
- Valor parcela <= margem disponivel
- Data inicio desconto >= proximo mes
- Para Refinanciamento/Compra: contrato vinculado obrigatorio

---

### 2.3 Importacao de Retorno da Folha

#### 2.3.1 Formato Excel/CSV

**Nome do Layout:** `IMP_RETORNO_FOLHA_V1`

| # | Campo | Tipo | Tamanho | Obrigatorio | Formato | Descricao |
|---|-------|------|---------|-------------|---------|-----------|
| 1 | COMPETENCIA | Texto | 7 | Sim | MM/YYYY | Mes/Ano de referencia |
| 2 | CPF | Texto | 11 | Sim | 99999999999 | CPF do funcionario |
| 3 | MATRICULA | Texto | 20 | Sim | | Matricula funcional |
| 4 | CODIGO_CONSIGNATARIA | Texto | 10 | Sim | | Codigo da consignataria |
| 5 | NUMERO_CONTRATO | Texto | 30 | Sim | | Numero do contrato |
| 6 | VALOR_ENVIADO | Numerico | 15,2 | Sim | | Valor que deveria descontar |
| 7 | VALOR_DESCONTADO | Numerico | 15,2 | Sim | | Valor efetivamente descontado |
| 8 | STATUS | Texto | 1 | Sim | D/N/P | Descontado/Nao descontado/Parcial |
| 9 | MOTIVO | Texto | 100 | Nao | | Motivo do nao desconto |

**Codigos de Motivo (quando STATUS = N ou P):**
| Codigo | Descricao |
|--------|-----------|
| 01 | Funcionario exonerado |
| 02 | Funcionario afastado |
| 03 | Margem insuficiente |
| 04 | Contrato nao localizado |
| 05 | Funcionario nao localizado na folha |
| 06 | Valor divergente |
| 07 | Contrato suspenso |
| 08 | Outros |

**Exemplo CSV:**
```
COMPETENCIA;CPF;MATRICULA;CODIGO_CONSIGNATARIA;NUMERO_CONTRATO;VALOR_ENVIADO;VALOR_DESCONTADO;STATUS;MOTIVO
01/2024;12345678901;001234;BANCO01;CTR2024001234;450,00;450,00;D;
01/2024;98765432100;001235;BANCO01;CTR2024001235;380,00;0,00;N;Funcionario afastado
01/2024;11122233344;001236;BANCO02;CTR2024005678;520,00;260,00;P;Margem insuficiente
```

---

#### 2.3.2 Formato TXT Posicional

**Nome do Layout:** `IMP_RETORNO_FOLHA_POS_V1`

**Header (Tipo = 0):**
| Posicao | Tamanho | Campo | Descricao |
|---------|---------|-------|-----------|
| 1-1 | 1 | TIPO_REGISTRO | 0 = Header |
| 2-8 | 7 | COMPETENCIA | MMYYYY |
| 9-18 | 10 | CODIGO_CONSIGNANTE | Codigo do orgao |
| 19-26 | 8 | DATA_GERACAO | YYYYMMDD |
| 27-32 | 6 | HORA_GERACAO | HHMMSS |
| 33-42 | 10 | QTD_REGISTROS | Zeros a esquerda |

**Detalhe (Tipo = 1):**
| Posicao | Tamanho | Campo | Descricao |
|---------|---------|-------|-----------|
| 1-1 | 1 | TIPO_REGISTRO | 1 = Detalhe |
| 2-12 | 11 | CPF | Zeros a esquerda |
| 13-32 | 20 | MATRICULA | Espacos a direita |
| 33-42 | 10 | CODIGO_CONSIGNATARIA | Espacos a direita |
| 43-72 | 30 | NUMERO_CONTRATO | Espacos a direita |
| 73-87 | 15 | VALOR_ENVIADO | Zeros, 2 decimais implicitos |
| 88-102 | 15 | VALOR_DESCONTADO | Zeros, 2 decimais implicitos |
| 103-103 | 1 | STATUS | D/N/P |
| 104-105 | 2 | CODIGO_MOTIVO | 01-08 ou espacos |

**Trailer (Tipo = 9):**
| Posicao | Tamanho | Campo | Descricao |
|---------|---------|-------|-----------|
| 1-1 | 1 | TIPO_REGISTRO | 9 = Trailer |
| 2-11 | 10 | TOTAL_REGISTROS | Zeros a esquerda |
| 12-26 | 15 | TOTAL_ENVIADO | Soma valores enviados |
| 27-41 | 15 | TOTAL_DESCONTADO | Soma valores descontados |

---

## 3. Layouts de Exportacao

### 3.1 Exportacao de Margem

#### 3.1.1 Formato Excel/CSV

**Nome do Layout:** `EXP_MARGEM_V1`

| # | Campo | Tipo | Tamanho | Descricao |
|---|-------|------|---------|-----------|
| 1 | CPF | Texto | 11 | CPF do funcionario |
| 2 | NOME | Texto | 100 | Nome completo |
| 3 | MATRICULA | Texto | 20 | Matricula funcional |
| 4 | CODIGO_EMPRESA | Texto | 10 | Codigo da empresa |
| 5 | NOME_EMPRESA | Texto | 100 | Nome da empresa |
| 6 | SITUACAO | Texto | 20 | Situacao do funcionario |
| 7 | SALARIO_BRUTO | Numerico | 15,2 | Salario bruto |
| 8 | MARGEM_TOTAL | Numerico | 15,2 | Margem total calculada |
| 9 | MARGEM_UTILIZADA | Numerico | 15,2 | Margem comprometida |
| 10 | MARGEM_DISPONIVEL | Numerico | 15,2 | Margem livre |
| 11 | MARGEM_EMPRESTIMO | Numerico | 15,2 | Margem para emprestimo |
| 12 | MARGEM_CARTAO | Numerico | 15,2 | Margem para cartao |
| 13 | QTD_CONTRATOS_ATIVOS | Numerico | 5 | Quantidade de contratos |
| 14 | DATA_NASCIMENTO | Data | 10 | Data de nascimento |
| 15 | IDADE | Numerico | 3 | Idade em anos |

**Exemplo CSV:**
```
CPF;NOME;MATRICULA;CODIGO_EMPRESA;NOME_EMPRESA;SITUACAO;SALARIO_BRUTO;MARGEM_TOTAL;MARGEM_UTILIZADA;MARGEM_DISPONIVEL;MARGEM_EMPRESTIMO;MARGEM_CARTAO;QTD_CONTRATOS_ATIVOS;DATA_NASCIMENTO;IDADE
12345678901;JOAO DA SILVA;001234;EMP001;PREFEITURA MUNICIPAL;ATIVO;5500,00;1925,00;450,00;1475,00;1237,50;237,50;1;15/03/1980;45
```

---

### 3.2 Exportacao de Remessa para Folha

#### 3.2.1 Formato Excel/CSV

**Nome do Layout:** `EXP_REMESSA_V1`

| # | Campo | Tipo | Tamanho | Descricao |
|---|-------|------|---------|-----------|
| 1 | COMPETENCIA | Texto | 7 | Competencia do desconto |
| 2 | CPF | Texto | 11 | CPF do funcionario |
| 3 | NOME | Texto | 100 | Nome do funcionario |
| 4 | MATRICULA | Texto | 20 | Matricula funcional |
| 5 | CODIGO_EMPRESA | Texto | 10 | Codigo da empresa |
| 6 | CODIGO_CONSIGNATARIA | Texto | 10 | Codigo da consignataria |
| 7 | NOME_CONSIGNATARIA | Texto | 100 | Nome da consignataria |
| 8 | NUMERO_CONTRATO | Texto | 30 | Numero do contrato |
| 9 | CODIGO_PRODUTO | Texto | 10 | Codigo do produto |
| 10 | VALOR_PARCELA | Numerico | 15,2 | Valor a descontar |
| 11 | PARCELA_ATUAL | Numerico | 3 | Numero da parcela |
| 12 | TOTAL_PARCELAS | Numerico | 3 | Total de parcelas |
| 13 | CODIGO_VERBA | Texto | 10 | Codigo da verba/rubrica |

**Exemplo CSV:**
```
COMPETENCIA;CPF;NOME;MATRICULA;CODIGO_EMPRESA;CODIGO_CONSIGNATARIA;NOME_CONSIGNATARIA;NUMERO_CONTRATO;CODIGO_PRODUTO;VALOR_PARCELA;PARCELA_ATUAL;TOTAL_PARCELAS;CODIGO_VERBA
02/2024;12345678901;JOAO DA SILVA;001234;EMP001;BANCO01;BANCO DO BRASIL;CTR2024001234;EMP01;450,00;2;48;9001
```

---

#### 3.2.2 Formato TXT Posicional

**Nome do Layout:** `EXP_REMESSA_POS_V1`

**Header (Tipo = 0):**
| Posicao | Tamanho | Campo | Descricao |
|---------|---------|-------|-----------|
| 1-1 | 1 | TIPO_REGISTRO | 0 = Header |
| 2-8 | 7 | COMPETENCIA | MMYYYY |
| 9-18 | 10 | CODIGO_CONSIGNANTE | Codigo do orgao |
| 19-118 | 100 | NOME_CONSIGNANTE | Nome do orgao |
| 119-126 | 8 | DATA_GERACAO | YYYYMMDD |
| 127-132 | 6 | HORA_GERACAO | HHMMSS |

**Detalhe (Tipo = 1):**
| Posicao | Tamanho | Campo | Descricao |
|---------|---------|-------|-----------|
| 1-1 | 1 | TIPO_REGISTRO | 1 = Detalhe |
| 2-12 | 11 | CPF | Zeros a esquerda |
| 13-32 | 20 | MATRICULA | Espacos a direita |
| 33-42 | 10 | CODIGO_EMPRESA | Espacos a direita |
| 43-52 | 10 | CODIGO_CONSIGNATARIA | Espacos a direita |
| 53-82 | 30 | NUMERO_CONTRATO | Espacos a direita |
| 83-92 | 10 | CODIGO_PRODUTO | Espacos a direita |
| 93-107 | 15 | VALOR_PARCELA | Zeros, 2 decimais |
| 108-110 | 3 | PARCELA_ATUAL | Zeros a esquerda |
| 111-113 | 3 | TOTAL_PARCELAS | Zeros a esquerda |
| 114-123 | 10 | CODIGO_VERBA | Espacos a direita |

**Trailer (Tipo = 9):**
| Posicao | Tamanho | Campo | Descricao |
|---------|---------|-------|-----------|
| 1-1 | 1 | TIPO_REGISTRO | 9 = Trailer |
| 2-11 | 10 | TOTAL_REGISTROS | Quantidade de detalhes |
| 12-26 | 15 | TOTAL_VALOR | Soma dos valores |

---

### 3.3 Exportacao de Averbacoes

#### 3.3.1 Formato Excel/CSV

**Nome do Layout:** `EXP_AVERBACOES_V1`

| # | Campo | Tipo | Tamanho | Descricao |
|---|-------|------|---------|-----------|
| 1 | ID_AVERBACAO | Numerico | 10 | ID interno do sistema |
| 2 | NUMERO_CONTRATO | Texto | 30 | Numero do contrato |
| 3 | CPF | Texto | 11 | CPF do funcionario |
| 4 | NOME_FUNCIONARIO | Texto | 100 | Nome do funcionario |
| 5 | MATRICULA | Texto | 20 | Matricula |
| 6 | CODIGO_EMPRESA | Texto | 10 | Codigo da empresa |
| 7 | NOME_EMPRESA | Texto | 100 | Nome da empresa |
| 8 | CODIGO_CONSIGNATARIA | Texto | 10 | Codigo da consignataria |
| 9 | NOME_CONSIGNATARIA | Texto | 100 | Nome da consignataria |
| 10 | CODIGO_PRODUTO | Texto | 10 | Codigo do produto |
| 11 | NOME_PRODUTO | Texto | 50 | Nome do produto |
| 12 | TIPO_OPERACAO | Texto | 20 | Novo/Refinanciamento/Compra |
| 13 | SITUACAO | Texto | 20 | Status atual |
| 14 | VALOR_TOTAL | Numerico | 15,2 | Valor financiado |
| 15 | QTD_PARCELAS | Numerico | 3 | Total de parcelas |
| 16 | VALOR_PARCELA | Numerico | 15,2 | Valor da parcela |
| 17 | TAXA_MENSAL | Numerico | 6,4 | Taxa mensal (%) |
| 18 | CET_ANUAL | Numerico | 6,4 | CET anual (%) |
| 19 | DATA_CONTRATO | Data | 10 | Data do contrato |
| 20 | DATA_INICIO_DESCONTO | Data | 10 | Primeira competencia |
| 21 | DATA_FIM_DESCONTO | Data | 10 | Ultima competencia |
| 22 | PARCELAS_PAGAS | Numerico | 3 | Parcelas ja descontadas |
| 23 | PARCELAS_RESTANTES | Numerico | 3 | Parcelas a vencer |
| 24 | SALDO_DEVEDOR | Numerico | 15,2 | Saldo devedor atual |
| 25 | DATA_APROVACAO | Data | 10 | Data de aprovacao |
| 26 | USUARIO_APROVACAO | Texto | 50 | Usuario que aprovou |
| 27 | DATA_CADASTRO | Data | 10 | Data de criacao |
| 28 | USUARIO_CADASTRO | Texto | 50 | Usuario que criou |

---

## 4. Codigos e Tabelas de Dominio

### 4.1 Situacao do Funcionario

| Codigo | Descricao | Permite Averbacao |
|--------|-----------|-------------------|
| A | Ativo | Sim |
| I | Inativo | Nao |
| F | Afastado | Nao |
| B | Bloqueado | Nao |
| P | Aposentado | Sim (se ativo) |

### 4.2 Tipo de Operacao

| Codigo | Descricao |
|--------|-----------|
| N | Novo (emprestimo novo) |
| R | Refinanciamento |
| C | Compra de Divida |

### 4.3 Situacao da Averbacao

| Codigo | Descricao |
|--------|-----------|
| AG | Aguardando Aprovacao |
| AP | Aprovada |
| RJ | Rejeitada |
| EN | Enviada para Folha |
| DE | Descontada (em andamento) |
| SU | Suspensa |
| BL | Bloqueada |
| LI | Liquidada |
| CA | Cancelada |

### 4.4 Tipo de Conta Bancaria

| Codigo | Descricao |
|--------|-----------|
| CC | Conta Corrente |
| CP | Conta Poupanca |
| CS | Conta Salario |

### 4.5 Tipo de Empresa

| Codigo | Descricao |
|--------|-----------|
| PRE | Prefeitura |
| AUT | Autarquia |
| CAM | Camara Municipal |
| IPR | Instituto de Previdencia |
| FUN | Fundacao |
| CON | Consorcio |
| OUT | Outros |

---

## 5. Tratamento de Erros

### 5.1 Codigos de Erro de Importacao

| Codigo | Descricao | Acao Sugerida |
|--------|-----------|---------------|
| ERR-001 | CPF invalido | Corrigir digitos verificadores |
| ERR-002 | CPF nao encontrado | Cadastrar funcionario primeiro |
| ERR-003 | Matricula duplicada | Verificar matricula unica |
| ERR-004 | Empresa nao encontrada | Verificar codigo da empresa |
| ERR-005 | Consignataria nao encontrada | Verificar codigo da consignataria |
| ERR-006 | Consignataria inativa | Contatar administrador |
| ERR-007 | Produto nao encontrado | Verificar codigo do produto |
| ERR-008 | Margem insuficiente | Reduzir valor da parcela |
| ERR-009 | Data invalida | Corrigir formato da data |
| ERR-010 | Valor invalido | Corrigir formato numerico |
| ERR-011 | Campo obrigatorio vazio | Preencher campo |
| ERR-012 | Contrato duplicado | Contrato ja existe no sistema |
| ERR-013 | Funcionario bloqueado | Desbloquear funcionario |
| ERR-014 | Funcionario inativo | Ativar funcionario |
| ERR-015 | Prazo excede limite | Reduzir quantidade de parcelas |
| ERR-016 | Valor excede limite | Reduzir valor total |
| ERR-017 | Contrato vinculado nao encontrado | Verificar numero do contrato original |
| ERR-018 | Competencia invalida | Corrigir formato MM/YYYY |
| ERR-019 | Arquivo corrompido | Gerar arquivo novamente |
| ERR-020 | Encoding invalido | Converter para UTF-8 |

### 5.2 Relatorio de Erros

Apos processamento, o sistema gera relatorio contendo:

| Campo | Descricao |
|-------|-----------|
| Linha | Numero da linha com erro |
| Campo | Nome do campo com problema |
| Valor | Valor informado |
| Erro | Codigo e descricao do erro |
| Sugestao | Acao corretiva |

**Exemplo de Relatorio:**
```
RELATORIO DE ERROS - IMPORTACAO DE FUNCIONARIOS
Data: 15/01/2024 14:30:45
Arquivo: funcionarios_janeiro.xlsx
Total de Linhas: 150
Processadas com Sucesso: 147
Erros: 3

Linha | Campo           | Valor         | Erro                    | Sugestao
------|-----------------|---------------|-------------------------|---------------------------
45    | CPF             | 12345678900   | ERR-001: CPF invalido   | Verificar digitos
78    | DATA_NASCIMENTO | 30/02/1990    | ERR-009: Data invalida  | Data nao existe
112   | SALARIO_BRUTO   | -500,00       | ERR-010: Valor invalido | Valor deve ser positivo
```

---

## 6. Layouts Personalizados

### 6.1 Configuracao de Layout Customizado

O sistema permite configurar layouts personalizados para atender necessidades especificas de integracao.

**Parametros Configuraveis:**

| Parametro | Descricao | Opcoes |
|-----------|-----------|--------|
| Formato | Tipo de arquivo | Excel, CSV, TXT |
| Separador | Delimitador (CSV) | ; , \t \| |
| Encoding | Codificacao | UTF-8, ISO-8859-1, Windows-1252 |
| Header | Primeira linha e cabecalho | Sim/Nao |
| Campos | Lista de campos a incluir | Selecao livre |
| Ordem | Ordem dos campos | Configuravel |
| Mascara Data | Formato de datas | DD/MM/YYYY, YYYY-MM-DD, YYYYMMDD |
| Decimal | Separador decimal | Virgula, Ponto |
| Milhar | Separador de milhar | Ponto, Virgula, Nenhum |

---

## 7. Historico de Revisoes

| Versao | Data | Autor | Descricao |
|--------|------|-------|-----------|
| 1.0 | Janeiro 2026 | Product Team | Versao inicial |

---

*Este documento especifica os layouts de arquivos do sistema FastConsig e deve ser mantido atualizado conforme evolucao das integracoes.*
