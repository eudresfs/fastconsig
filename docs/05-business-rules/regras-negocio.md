# Regras de Negocio - Sistema FastConsig

## Documento de Especificacao de Regras de Negocio

**Versao:** 1.0
**Data:** Janeiro/2026
**Sistema:** FastConsig - Sistema de Gestao de Consignados
**Baseado em:** Analise do sistema legado .NET Framework 4.0

---

> **Nota**: Este documento especifica as regras de negócio do NOVO sistema Node.js/TypeScript.
> Para referência do comportamento do sistema legado .NET, consulte
> [docs/archive/dotnet/business-rules.md](../../archive/dotnet/business-rules.md).

---

## 1. Introducao

### 1.1 Objetivo

Este documento descreve as regras de negocio do sistema FastConsig, um sistema brasileiro de gestao de emprestimos consignados (consignacoes em folha de pagamento). O documento foi elaborado com base na analise do codigo-fonte do sistema legado.

### 1.2 Escopo

O sistema gerencia o ciclo completo de emprestimos consignados, incluindo:
- Cadastro e gestao de funcionarios (servidores)
- Registro e acompanhamento de averbacoes (contratos de emprestimo)
- Calculo e controle de margem consignavel
- Simulacoes de emprestimo e compra de divida
- Conciliacao mensal com folha de pagamento
- Portabilidade e compra de divida entre consignatarias

### 1.3 Glossario

| Termo | Definicao |
|-------|-----------|
| **Averbacao** | Registro de um emprestimo consignado no sistema |
| **Consignante** | Orgao empregador que processa os descontos em folha |
| **Consignataria** | Instituicao financeira que concede o emprestimo |
| **Funcionario/Servidor** | Pessoa fisica que contrata o emprestimo |
| **Margem** | Percentual do salario disponivel para consignacao |
| **Parcela** | Prestacao mensal do emprestimo |
| **Competencia** | Mes/ano de referencia (formato AAAA/MM) |
| **Refinanciamento** | Liquidacao de contrato existente para novo contrato |
| **Portabilidade** | Transferencia de divida entre consignatarias |

---

## 2. Modulo de Funcionarios (Servidores)

### 2.1 Cadastro de Funcionarios

#### RN-FUN-001: Identificacao Unica
- Cada funcionario e identificado por matricula unica no consignante
- Um funcionario pode ter multiplas matriculas (vinculos diferentes)
- CPF e utilizado como identificador secundario para busca

#### RN-FUN-002: Dados Obrigatorios
- Matricula
- Nome completo (via entidade Pessoa)
- CPF (via entidade Pessoa)
- Situacao funcional
- Categoria funcional
- Regime de trabalho

#### RN-FUN-003: Relacionamento Pessoa-Funcionario
- Uma Pessoa pode ter varios Funcionarios (multiplos vinculos)
- Dados pessoais (endereco, telefone, email) sao armazenados na entidade Pessoa
- Dados funcionais sao armazenados na entidade Funcionario

### 2.2 Situacoes do Funcionario

#### RN-FUN-004: Estados Possiveis
O funcionario pode estar em uma das seguintes situacoes:

| ID | Situacao | Descricao |
|----|----------|-----------|
| 0 | Nao Informado | Situacao nao definida |
| 1 | Ativo na Folha | Funcionario em atividade normal |
| 2 | Retirado da Folha | Funcionario removido da folha |
| 3 | Exonerado | Funcionario desligado |
| 4 | Bloqueado | Funcionario com restricoes |
| 5 | Aposentado | Funcionario aposentado |

#### RN-FUN-005: Restricoes por Situacao
- **Ativo na Folha**: Pode realizar novas averbacoes normalmente
- **Retirado da Folha**: Nao pode realizar novas averbacoes
- **Exonerado**: Nao pode realizar novas averbacoes
- **Bloqueado**: Nao pode realizar novas averbacoes (exceto com autorizacao especial)
- **Aposentado**: Tratamento especial para averbacoes existentes

### 2.3 Bloqueios de Funcionario

#### RN-FUN-006: Tipos de Bloqueio
```
BloqueioTipo:
  0 = Completo (bloqueia todas as operacoes)
  1 = Por Tipo de Produto (bloqueia produtos especificos)
  2 = Por Tipo de Empresa (bloqueia consignatarias especificas)
```

#### RN-FUN-007: Estrutura do Bloqueio
- IDFuncionario: Funcionario bloqueado
- TipoBloqueio: Tipo do bloqueio (0, 1 ou 2)
- Chaves: Identificador do produto ou empresa bloqueada
- Motivo: Justificativa do bloqueio
- DataBloqueio: Data de inicio do bloqueio
- DataDesbloqueio: Data de fim do bloqueio (quando aplicavel)
- Ativo: Flag indicando se bloqueio esta ativo (1) ou inativo (0)

#### RN-FUN-008: Validacao de Bloqueio
Antes de permitir nova averbacao, o sistema verifica:
1. Se existe bloqueio completo (TipoBloqueio = "0")
2. Se existe bloqueio para o produto selecionado (TipoBloqueio = "1")
3. Se existe bloqueio para a consignataria (TipoBloqueio = "2")

### 2.4 Autorizacoes Especiais

#### RN-FUN-009: Tipos de Autorizacao
```
FuncionarioAutorizacaoTipo:
  0 = Independente de qualquer restricao
  1 = Independente de Margem
  2 = Independente de Situacao
  3 = Independente de Bloqueio
```

#### RN-FUN-010: Validade da Autorizacao
- Cada autorizacao possui data de registro e validade em dias
- Formula: DataValida = AutorizacaoData + AutorizacaoValidade
- Autorizacao e valida se: DataValida >= DataAtual

#### RN-FUN-011: Efeito das Autorizacoes
- **Tipo 0**: Ignora todas as validacoes (margem, situacao, bloqueio)
- **Tipo 1**: Permite averbacao mesmo sem margem disponivel
- **Tipo 2**: Permite averbacao mesmo com situacao invalida
- **Tipo 3**: Permite averbacao mesmo com funcionario bloqueado

---

## 3. Modulo de Margem Consignavel

### 3.1 Calculo da Margem

#### RN-MAR-001: Margem Bruta
- A margem bruta e o valor base calculado sobre o salario do funcionario
- Armazenada no campo `MargemBruta` da entidade Funcionario
- Valor em moeda (R$), nao percentual

#### RN-MAR-002: Percentual por Grupo de Produto
- Cada grupo de produto possui um `PercentualMargemBruta`
- A margem por grupo = (MargemBruta * PercentualMargemBruta) / 100
- Exemplo: Se MargemBruta = R$ 1.000 e PercentualMargemBruta = 30%, MargemGrupo = R$ 300

#### RN-MAR-003: Formula de Geracao de Margem
```csharp
MargemFolha = (Funcionario.MargemBruta * ProdutoGrupo.PercentualMargemBruta) / 100
```

#### RN-MAR-004: Margem por Grupo de Produto
- Cada funcionario possui margem separada por grupo de produto
- Entidade FuncionarioMargem armazena:
  - IDFuncionario
  - IDProdutoGrupo
  - MargemFolha (valor disponivel)

### 3.2 Calculo de Margem Disponivel

#### RN-MAR-005: Formula de Margem Disponivel Real
```csharp
MargemDisponivelReal(idProdutoGrupo) =
    SUM(FuncionarioMargem.MargemFolha onde ProdutoGrupo.IDProdutoGrupoCompartilha = pg.IDProdutoGrupoCompartilha)
    - SUM(Averbacao.ValorDeducaoMargem onde Ativo=1 AND ProdutoGrupo.IDProdutoGrupoCompartilha = pg.IDProdutoGrupoCompartilha AND AverbacaoSituacao.DeduzMargem = true)
```

#### RN-MAR-006: Compartilhamento de Margem
- Grupos de produtos podem compartilhar margem atraves do campo `IDProdutoGrupoCompartilha`
- Averbacoes de grupos compartilhados consomem da mesma margem

#### RN-MAR-007: Situacoes que Deduzem Margem
A flag `DeduzMargem` na AverbacaoSituacao indica se a situacao consome margem:
- **Deduz**: Ativo, Averbado, Aguardando Aprovacao, Reservado, Em Processo de Compra
- **Nao Deduz**: Cancelado, Suspenso (Margem Livre), Liquidado, Concluido

### 3.3 Reserva e Liberacao de Margem

#### RN-MAR-008: Reserva de Margem
- Ao criar uma averbacao, o sistema reserva a margem atraves do campo `ValorDeducaoMargem`
- Para averbacao normal: ValorDeducaoMargem = ValorParcela
- Para refinanciamento/compra: ValorDeducaoMargem = ValorParcela - (MargemRefinanciada + MargemComprada)

#### RN-MAR-009: Calculo de Deducao em Refinanciamento
```csharp
ValorDeducaoMargem = ValorParcelaNovo - SUM(ValorParcela dos contratos liquidados que DeduzMargem)
```

#### RN-MAR-010: Liberacao de Margem
A margem e liberada automaticamente quando a averbacao muda para:
- Cancelado
- Suspenso (Margem Livre)
- Liquidado
- Concluido

---

## 4. Modulo de Averbacoes

### 4.1 Tipos de Averbacao

#### RN-AVE-001: Classificacao de Averbacoes
```
AverbacaoTipo:
  1 = Normal (novo emprestimo)
  2 = Compra (portabilidade de outra consignataria)
  3 = Renegociacao (refinanciamento na mesma consignataria)
  4 = Compra e Renegociacao (ambos)
```

#### RN-AVE-002: Determinacao do Tipo
O tipo e determinado automaticamente com base nas selecoes:
- Se nenhum contrato selecionado: Tipo = Normal
- Se apenas contratos proprios selecionados: Tipo = Renegociacao
- Se apenas contratos de terceiros selecionados: Tipo = Compra
- Se ambos selecionados: Tipo = Compra e Renegociacao

### 4.2 Estados da Averbacao

#### RN-AVE-003: Situacoes Possiveis
```
AverbacaoSituacao:
  0  = Cancelado
  1  = Ativo
  2  = Averbado
  3  = Aguardando Aprovacao
  4  = Reservado
  5  = Desaprovado
  6  = Suspenso (Margem Livre)
  7  = Bloqueado (Margem Retida)
  8  = Em Processo de Compra
  9  = Comprado
  10 = Liquidado
  11 = Concluido
  12 = Pre-Reserva
```

#### RN-AVE-004: Atributos das Situacoes

| Situacao | DeduzMargem | ParaDescontoFolha | Liquidavel | Cancelavel |
|----------|-------------|-------------------|------------|------------|
| Cancelado | Nao | Nao | Nao | Nao |
| Ativo | Sim | Sim | Sim | Sim |
| Averbado | Sim | Sim | Sim | Sim |
| Aguardando Aprovacao | Sim | Nao | Nao | Sim |
| Reservado | Sim | Nao | Nao | Sim |
| Desaprovado | Nao | Nao | Nao | Nao |
| Suspenso (Margem Livre) | Nao | Nao | Sim | Sim |
| Bloqueado (Margem Retida) | Sim | Nao | Sim | Sim |
| Em Processo de Compra | Sim | Nao | Nao | Condicional |
| Comprado | Sim | Nao | Nao | Nao |
| Liquidado | Nao | Nao | Nao | Nao |
| Concluido | Nao | Nao | Nao | Nao |
| Pre-Reserva | Sim | Nao | Nao | Sim |

### 4.3 Transicoes de Estado

#### RN-AVE-005: Fluxo Normal de Averbacao
```
Reservado -> Aguardando Aprovacao (se requer aprovacao)
          -> Averbado (se nao requer aprovacao)

Aguardando Aprovacao -> Averbado (aprovado)
                     -> Desaprovado (rejeitado)

Averbado -> Ativo (apos primeiro desconto)
         -> Suspenso (suspensao manual)
         -> Bloqueado (bloqueio manual)
         -> Liquidado (quitacao antecipada)
         -> Cancelado (cancelamento)

Ativo -> Suspenso (suspensao manual)
      -> Bloqueado (bloqueio manual)
      -> Liquidado (quitacao ou fim do prazo)
      -> Cancelado (cancelamento)
      -> Concluido (todas parcelas pagas)
```

#### RN-AVE-006: Fluxo de Compra de Divida
```
Reservado -> Em Processo de Compra

Em Processo de Compra (aguarda):
  - Informar Saldo Devedor
  - Informar Quitacao
  - Confirmar/Rejeitar Quitacao
  - Concluir Compra

Em Processo de Compra -> Averbado (compra concluida)
                      -> Cancelado (compra cancelada)

Contrato Original -> Comprado (quitacao confirmada)
                  -> Liquidado (apos conclusao da compra)
```

#### RN-AVE-007: Transicoes Permitidas

| De | Para | Condicao |
|----|------|----------|
| Reservado | Aguardando Aprovacao | Requer aprovacao |
| Reservado | Averbado | Nao requer aprovacao, tipo Normal |
| Reservado | Em Processo de Compra | Tipo Compra ou Compra+Renegociacao |
| Aguardando Aprovacao | Averbado | Aprovado |
| Aguardando Aprovacao | Desaprovado | Rejeitado |
| Averbado | Ativo | Primeiro desconto realizado |
| Ativo | Suspenso | Solicitacao de suspensao |
| Ativo | Bloqueado | Solicitacao de bloqueio |
| Ativo | Liquidado | Quitacao manual ou por compra |
| Ativo | Cancelado | Cancelamento autorizado |
| Suspenso | Ativo | Reativacao |
| Bloqueado | Ativo | Desbloqueio |
| Em Processo de Compra | Averbado | Compra concluida |
| Em Processo de Compra | Cancelado | Compra cancelada (com restricoes) |

### 4.4 Tramitacao

#### RN-AVE-008: Registro de Tramitacao
Toda mudanca de estado gera um registro em AverbacaoTramitacao:
- IDAverbacao
- IDAverbacaoSituacao (nova situacao)
- IDEmpresa (responsavel)
- OBS (observacao)
- CreatedOn (data/hora)

#### RN-AVE-009: Historico de Tramitacoes
- A situacao atual da averbacao e a ultima tramitacao registrada
- O historico completo e mantido para auditoria

### 4.5 Validacoes de Averbacao

#### RN-AVE-010: Validacoes Obrigatorias
Antes de salvar uma averbacao, o sistema valida:
1. Funcionario existe e esta ativo
2. Funcionario nao esta bloqueado
3. Margem disponivel suficiente
4. Prazo dentro do limite maximo
5. Valor da parcela informado (> 0)
6. Mes inicio valido (>= mes de corte atual)
7. Valor contratado <= Valor consignado
8. Usuario possui permissao para o tipo de averbacao

#### RN-AVE-011: Validacao de Margem
```csharp
bool ValidaMargem() {
    if (TemAutorizacaoEspecial(IndependenteDeMargem)) return true;
    decimal margemDisp = CalcularMargemDisponivel();
    return ValorParcela <= margemDisp;
}
```

#### RN-AVE-012: Validacao de Situacao Funcional
```csharp
bool ValidaFuncSituacao() {
    if (TemAutorizacaoEspecial(IndependenteDeSituacao)) return true;
    return Funcionario.IDFuncionarioSituacao == AtivoNaFolha;
}
```

#### RN-AVE-013: Validacao de Bloqueio
```csharp
bool ValidaFuncBloqueio() {
    if (TemAutorizacaoEspecial(IndependenteDeBloqueio)) return true;

    // Verifica bloqueio completo
    if (FuncionarioBloqueio.Any(TipoBloqueio == "0" AND Ativo == 1)) return false;

    // Verifica bloqueio por produto
    if (FuncionarioBloqueio.Any(TipoBloqueio == "1" AND Chaves == IDProduto AND Ativo == 1)) return false;

    // Verifica bloqueio por empresa
    if (FuncionarioBloqueio.Any(TipoBloqueio == "2" AND Chaves == IDConsignataria AND Ativo == 1)) return false;

    return true;
}
```

#### RN-AVE-014: Validacao de Prazo Maximo
- Prazo maximo e definido por parametro global ou por produto
- Se produto tem PrazoMaximo definido, este prevalece
- Validacao: Prazo <= PrazoMaximo

#### RN-AVE-015: Prevencao de Duplicacao
```csharp
bool VerificaExistePrevenirDuplicacao(Averbacao averb) {
    return Averbacoes.Any(
        IDFuncionario == averb.IDFuncionario &&
        Data == DateTime.Today &&
        AverbacaoSituacao.DeduzMargem == true &&
        IDConsignataria == averb.IDConsignataria &&
        IDProduto == averb.IDProduto &&
        ValorParcela == averb.ValorParcela
    );
}
```

### 4.6 Suspensao e Bloqueio

#### RN-AVE-016: Diferenca entre Suspensao e Bloqueio
- **Suspensao (Margem Livre)**: Averbacao inativa, margem liberada para outras operacoes
- **Bloqueio (Margem Retida)**: Averbacao inativa, margem permanece reservada

#### RN-AVE-017: Regras de Suspensao
- Pode ser aplicada a averbacoes com situacao Ativo ou Averbado
- Requer motivo justificado
- Pode ser revertida (volta para Ativo)
- Nao envia para desconto em folha

#### RN-AVE-018: Regras de Bloqueio
- Pode ser aplicada a averbacoes com situacao Ativo ou Averbado
- Requer motivo justificado
- Pode ser revertido (volta para Ativo)
- Margem continua reservada
- Nao envia para desconto em folha

### 4.7 Liquidacao

#### RN-AVE-019: Tipos de Liquidacao
- **Manual**: Operador registra quitacao antecipada
- **Por Folha**: Ultima parcela descontada em folha
- **Por Compra**: Contrato liquidado por portabilidade

#### RN-AVE-020: Efeitos da Liquidacao
1. Situacao muda para Liquidado
2. Margem e liberada (DeduzMargem = false)
3. Parcelas restantes marcadas como LiquidadaManual
4. Registro de tramitacao gerado

#### RN-AVE-021: Restricao de Liquidacao em Compra
Se a averbacao participa de processo de compra ativo:
- Nao pode ser liquidada diretamente
- Deve seguir fluxo de confirmacao de quitacao

### 4.8 Cancelamento

#### RN-AVE-022: Validacoes de Cancelamento
```csharp
bool PodeCancelar(Averbacao averb) {
    // Nao pode cancelar se participa de compra em andamento
    if (averb.AverbacaoVinculo.Any(v =>
        v.Averbacao1.IDAverbacaoSituacao == EmProcessodeCompra)) {
        return false; // CancelamentoIndevido.ParticipandoCompra
    }

    // Nao pode cancelar compra com solicitacoes processadas
    if (averb.IDAverbacaoSituacao == EmProcessodeCompra &&
        (averb.IDAverbacaoTipo == Compra || averb.IDAverbacaoTipo == CompraERenegociacao)) {
        if (ExisteSolicitacoesProcessadas) {
            return false; // CancelamentoIndevido.ExisteSolicitacoesProcessadas
        }
    }

    return true;
}
```

#### RN-AVE-023: Efeitos do Cancelamento
1. Situacao muda para Cancelado
2. Margem e liberada
3. Parcelas marcadas como Cancelada
4. Se for refinanciamento, contratos originais voltam para Ativo

---

## 5. Modulo de Parcelas

### 5.1 Geracao de Parcelas

#### RN-PAR-001: Geracao Automatica
Parcelas sao geradas automaticamente ao averbar:
- Para cada mes do prazo, uma parcela e criada
- Competencia inicial = CompetenciaInicial da averbacao
- Numero sequencial de 1 ate Prazo

#### RN-PAR-002: Estrutura da Parcela
```csharp
AverbacaoParcela {
    IDAverbacao,
    Numero,           // 1, 2, 3... ate Prazo
    Competencia,      // AAAA/MM
    Valor,            // Valor nominal da parcela
    ValorDescontado,  // Valor efetivamente descontado (pode diferir)
    IDAverbacaoParcelaSituacao
}
```

### 5.2 Situacoes de Parcela

#### RN-PAR-003: Estados Possiveis
```
AverbacaoParcelaSituacao:
  0 = Cancelada
  1 = Aberta
  2 = Liquidada Folha (desconto em folha)
  3 = Liquidada Manual (quitacao manual)
  4 = Rejeitada Folha (nao descontada)
```

### 5.3 Calculo de Saldo

#### RN-PAR-004: Saldo Restante
```csharp
decimal SaldoRestante {
    string anoMes = ObtemAnoMesCorte();
    int prazoRestante = AverbacaoParcela.Count(p => p.Competencia >= anoMes);
    return prazoRestante * ValorParcela;
}
```

#### RN-PAR-005: Prazo Restante
```csharp
int PrazoRestante {
    string anoMes = ObtemAnoMesCorte();
    return AverbacaoParcela.Count(p => p.Competencia >= anoMes);
}
```

---

## 6. Modulo de Simulacoes

### 6.1 Simulacao de Emprestimo

#### RN-SIM-001: Parametros de Entrada
- Valor desejado (emprestimo ou parcela)
- Prazo
- Produto/Consignataria

#### RN-SIM-002: Coeficientes
- Cada consignataria define coeficientes por prazo
- Coeficiente = ValorParcela / ValorContratado
- Armazenado em EmpresaCoeficienteDetalhe

#### RN-SIM-003: Calculos
```
ValorConsignado = Prazo * ValorParcela
ValorContratado = ValorParcela / Coeficiente
```

### 6.2 Simulacao de Compra de Divida

#### RN-SIM-004: Objetivos de Simulacao
```
TipoSimulacaoDivida:
  0 = Obter Mais Dinheiro
  1 = Regularizar Margem
  2 = Reduzir Valor Pago
  3 = Diminuir Quantidade de Parcelas
```

#### RN-SIM-005: Parametros de Compra
- Contratos a serem comprados (de outras consignatarias)
- Contratos a serem refinanciados (da mesma consignataria)
- Novo prazo desejado
- Margem disponivel

#### RN-SIM-006: Calculo de Vantagem
O sistema calcula e compara:
- Situacao atual (soma de parcelas de todos os contratos)
- Situacao proposta (novo contrato unificado)
- Diferenca de valor total pago
- Nova margem disponivel

---

## 7. Modulo de Conciliacao

### 7.1 Processo de Conciliacao

#### RN-CON-001: Competencia
- Conciliacao e feita mensalmente por competencia (AAAA/MM)
- Compara valores enviados vs. valores descontados

#### RN-CON-002: Etapas da Conciliacao
1. **Geracao de Movimento**: Parcelas a descontar na folha
2. **Envio para Folha**: Arquivo de corte gerado
3. **Retorno da Folha**: Importacao dos valores descontados
4. **Analise de Divergencias**: Comparacao enviado vs. descontado
5. **Repasse**: Valores a repassar para consignatarias

### 7.2 Resumo de Conciliacao

#### RN-CON-003: Valores Calculados
```csharp
ListarConciliacaoResumoFolha(competencia, idempresa) {
    Previsao = TotalEnviadoParaDescontar(competencia);
    Retorno = TotalRetornadoPelaFolha(competencia);
    Diferenca = Retorno - Previsao;
    PrevisaoRepasse = TotalPrevistoRepasse(competencia);
    Conciliado = TotalConciliado(competencia);
}
```

### 7.3 Tipos de Conciliacao

#### RN-CON-004: Classificacao
Cada parcela conciliada recebe um tipo:
- Conforme (valor enviado = valor descontado)
- Divergente (valores diferentes)
- Nao Descontado (rejeitado pela folha)
- Descontado a Maior
- Descontado a Menor

### 7.4 Data de Corte

#### RN-CON-005: Calculo da Competencia de Corte
```csharp
string ObtemAnoMesCorte(idmodulo, idempresa) {
    int diaCorte = Parametro["DiaCorte"];

    // Verifica historico de corte especifico
    CorteHistorico ch = CorteHistorico.FirstOrDefault(c => c.Competencia == anoMesAtual);
    if (ch != null) diaCorte = ch.DiaCorte;

    // Consignataria pode ter dia de corte proprio
    if (idmodulo == Consignataria && idempresa > 0) {
        Empresa emp = ObtemConsignataria(idempresa);
        if (emp.DiaCorte > 0) {
            if (diaCorte < emp.DiaCorte) somaMes++;
            diaCorte = emp.DiaCorte;
        }
    }

    if (diaAtual > diaCorte)
        return CompetenciaAumenta(anoMesAtual, somaMes + 1);
    else
        return CompetenciaAumenta(anoMesAtual, somaMes);
}
```

---

## 8. Modulo de Portabilidade/Compra de Divida

### 8.1 Fluxo de Compra de Divida

#### RN-POR-001: Etapas do Processo
```
1. Consignataria Compradora registra averbacao tipo Compra
2. Sistema envia solicitacao de Saldo Devedor para Consignataria Vendedora
3. Vendedora informa saldo devedor e dados para pagamento
4. Compradora informa quitacao (paga o saldo)
5. Vendedora confirma ou rejeita quitacao
6. Se confirmado, Compradora conclui compra
7. Contrato original e liquidado, novo contrato e averbado
```

#### RN-POR-002: Solicitacoes de Compra
```
SolicitacaoTipo:
  1 = Informar Saldo Devedor de Contratos
  2 = Informar Quitacao
  3 = Confirmar/Rejeitar Quitacao
  4 = Concluir Compra de Divida
```

### 8.2 Saldo Devedor

#### RN-POR-003: Calculo de Saldo Devedor
```csharp
decimal SaldoDevedorValor {
    // Busca ultimo saldo devedor informado
    var solicitacao = UltimaSolicitacao(InformarSaldoDevedor);
    if (solicitacao != null) {
        var saldoDevedor = solicitacao.SaldoDevedor.Ultimo();
        // Validade de 30 dias
        if (DateTime.Today <= saldoDevedor.Data.AddDays(30))
            return saldoDevedor.Valor;
    }
    // Se nao ha saldo informado, usa calculo de saldo restante
    return SaldoRestante;
}
```

#### RN-POR-004: Estrutura do Saldo Devedor
```csharp
EmpresaSolicitacaoSaldoDevedor {
    IDAverbacao,
    Data,
    Validade,
    Valor,
    IDTipoPagamento,  // 1=Boleto, 2=TED
    Identificador,    // Codigo do boleto ou identificador
    Banco,
    Agencia,
    ContaCredito,
    NomeFavorecido,
    Observacao
}
```

### 8.3 Processamento de Compra

#### RN-POR-005: Fluxo de Processamento
```csharp
ProcessarFluxoCompra(Averbacao dado) {
    // Pai = averbacao de compra
    Averbacao pai = dado.AverbacaoVinculo.Ultimo().Averbacao1;

    // Marca pai como Em Processo de Compra
    if (pai.Situacao != EmProcessodeCompra)
        Tramitar(pai, EmProcessodeCompra);

    // Verifica tipo de solicitacao processada
    switch (ultimaSolicitacao.Tipo) {
        case InformarSaldoDevedor:
            // Proximo passo: Informar Quitacao
            AdicionaSolicitacao(InformarQuitacao, para: vendedora);
            break;

        case InformarQuitacao:
            // Proximo passo: Confirmar Quitacao
            AdicionaSolicitacao(ConfirmarRejeitarQuitacao, para: compradora);
            break;

        case ConfirmarRejeitarQuitacao:
            if (Processada) {
                // Marca como Comprado
                Tramitar(contrato, Comprado);
                if (!ExistemOutrosParaQuitar)
                    AdicionaSolicitacao(ConcluirCompra, para: compradora);
            } else {
                // Quitacao rejeitada, precisa regularizar
                AdicionaSolicitacao(RegularizarQuitacaoRejeitada, para: vendedora);
            }
            break;

        case ConcluirCompradeDivida:
            // Liquida todos os contratos vinculados
            foreach (contrato in contratosVinculados) {
                Tramitar(contrato, Liquidado);
                ParcelasAlterarSituacao(contrato, LiquidadaManual);
            }
            // Atualiza deducao de margem
            dado.ValorDeducaoMargem = dado.ValorParcela;
            // Averba o novo contrato
            Tramitar(dado, Averbado);
            GerarParcelas(dado);
            break;
    }
}
```

### 8.4 Refinanciamento

#### RN-POR-006: Regras de Refinanciamento
- Refinanciamento e feito na mesma consignataria
- Contratos selecionados sao liquidados imediatamente
- Novo contrato e averbado com margem recalculada
- Nao ha fluxo de solicitacoes (processo interno)

#### RN-POR-007: Processamento de Renegociacao
```csharp
ProcessaRenegociacao(Averbacao dado) {
    // Atualiza deducao de margem
    dado.ValorDeducaoMargem = dado.ValorParcela;
    Alterar(dado);

    // Averba novo contrato
    Tramitar(dado, Averbado);

    // Liquida contratos originais
    foreach (vinculo in dado.AverbacaoVinculo1.Where(propria)) {
        Tramitar(vinculo, Liquidado);
        ParcelasAlterarSituacao(vinculo, LiquidadaManual);
    }
}
```

### 8.5 Contratos Elegiveis

#### RN-POR-008: Contratos para Comprar
```csharp
AverbacoesParaComprar(funcionario, idempresa) {
    return funcionario.Averbacao.Where(
        IDConsignataria != idempresa &&              // De outra consignataria
        ProdutoGrupo == Emprestimos &&               // Apenas emprestimos
        Situacao == Ativo &&                         // Apenas ativos
        !ParticipaDeCompraEmAndamento               // Nao participa de outra compra
    );
}
```

#### RN-POR-009: Contratos para Refinanciar
```csharp
AverbacoesParaRefinanciar(funcionario, idempresa) {
    return funcionario.Averbacao.Where(
        IDConsignataria == idempresa &&              // Da mesma consignataria
        ProdutoGrupo == Emprestimos &&               // Apenas emprestimos
        Situacao == Ativo &&                         // Apenas ativos
        !ParticipaDeCompraEmAndamento               // Nao participa de compra
    );
}
```

---

## 9. Modulo de Aprovacoes

### 9.1 Fluxo de Aprovacao

#### RN-APR-001: Niveis de Aprovacao
O sistema suporta aprovacao em multiplos niveis:
1. Funcionario (servidor)
2. Consignataria (banco/financeira)
3. Consignante (orgao empregador)

#### RN-APR-002: Configuracao de Fluxo
Configurado por grupo de produto em FluxoAprovacao:
- RequerAprovacaoFuncionario
- RequerAprovacaoConsignataria
- RequerAprovacaoConsignante

#### RN-APR-003: Configuracao por Empresa
FluxoAprovacaoEmpresa permite configuracao especifica:
- IDProdutoGrupo
- IDEmpresa
- RequerAprovacao

### 9.2 Processamento de Aprovacao

#### RN-APR-004: Fluxo de Aprovacao
```csharp
ProcessarFluxoAprovacao(Averbacao dado, idProdutoGrupo) {
    if (dado.Situacao == PreReserva) return true;

    bool terminouFluxo = false;

    // 1. Verificar aprovacao do funcionario
    if (RequerAprovacaoFuncionario) {
        if (!JaAprovadoPeloFuncionario) {
            AdicionaSolicitacao(AprovarAverbacoes, para: funcionario);
            terminouFluxo = true;
        }
    }

    // 2. Verificar aprovacao da consignataria
    if (!terminouFluxo && RequerAprovacaoConsignataria) {
        if (!JaAprovadoPelaConsignataria) {
            if (!TemPermissaoAutoAprovar) {
                AdicionaSolicitacao(AprovarAverbacoes, para: consignataria);
                terminouFluxo = true;
            } else {
                // Auto-aprovacao
                AdicionaSolicitacao(AprovarAverbacoes, Processada);
            }
        }
    }

    // 3. Verificar aprovacao do consignante
    if (!terminouFluxo && RequerAprovacaoConsignante) {
        AdicionaSolicitacao(AprovarAverbacoes, para: consignante);
        terminouFluxo = true;
    }

    return terminouFluxo;
}
```

### 9.3 Situacao apos Aprovacao

#### RN-APR-005: Fluxo de Situacao
```csharp
ProcessarFluxoSituacao(Averbacao dado, existeFluxoPendente) {
    if (existeFluxoPendente) {
        if (dado.Situacao != AguardandoAprovacao)
            Tramitar(dado, AguardandoAprovacao);
        return;
    }

    // Sem fluxo pendente, processa conforme tipo
    switch (dado.Tipo) {
        case Normal:
            Tramitar(dado, Averbado);
            if (PrazoIndeterminado) GerarParcelas = false;
            else GerarParcelas = true;
            break;

        case Renegociacao:
            ProcessaRenegociacao(dado);
            GerarParcelas = true;
            break;

        case Compra:
        case CompraERenegociacao:
            ProcessaCompraInicio(dado);
            GerarParcelas = false; // Parcelas geradas apos conclusao
            break;
    }

    if (GerarParcelas) GerarParcelas(dado);
}
```

---

## 10. Validacoes Gerais

### 10.1 Validacao de CPF

#### RN-VAL-001: Formato de CPF
- CPF deve ter 11 digitos
- Mascara de exibicao: XXX.XXX.XXX-XX
- Armazenado sem mascara

### 10.2 Validacao de Matricula

#### RN-VAL-002: Unicidade de Matricula
- Matricula e unica por consignante
- Formato definido pelo consignante

### 10.3 Validacao de Valores

#### RN-VAL-003: Valores Monetarios
- Precisao de 2 casas decimais
- Separador decimal: virgula (,)
- Separador de milhar: ponto (.)

#### RN-VAL-004: Valor Parcela
- Deve ser maior que zero
- Nao pode exceder margem disponivel (exceto com autorizacao)

#### RN-VAL-005: Valor Contratado vs Consignado
- ValorContratado <= ValorConsignado
- ValorConsignado = Prazo * ValorParcela

### 10.4 Validacao de Datas

#### RN-VAL-006: Competencia
- Formato: AAAA/MM (armazenamento) ou MM/AAAA (exibicao)
- Mes deve ser entre 01 e 12
- Ano deve ser >= 2010 e <= 2200

#### RN-VAL-007: Mes Inicio
- Deve ser >= mes de corte atual
- Alteracao do primeiro mes requer permissao especifica

---

## 11. Permissoes

### 11.1 Tipos de Permissao

#### RN-PER-001: Permissoes do Sistema
```
Permissao:
  1  = Acessar
  2  = Incluir
  3  = Alterar
  4  = Excluir
  5  = Consultar
  17 = Ajustar Margem
  18 = Remanejar Margem
  19 = Rescindir
  20 = Aposentar
  21 = Bloquear
  30 = Suspender/Ativar
  31 = Liquidar
  32 = Aprovar/Desaprovar
  33 = Desliquidar
  36 = Informar Saldo Devedor
  37 = Informar Quitacao
  38 = Exportar
  39 = Cancelar
  44 = Averbacao Simples
  45 = Refinanciamentos
  46 = Compra
  47 = Compra com Renegociacao
  48 = Alterar Primeiro Mes Desconto
```

### 11.2 Validacao de Permissao

#### RN-PER-002: Verificacao de Permissao
```csharp
bool CheckPermissao(idRecurso, idEmpresa, idPerfil, idPermissao) {
    return Permissoes.Any(
        IDRecurso == idRecurso &&
        IDEmpresa == idEmpresa &&
        IDPerfil == idPerfil &&
        IDPermissao == idPermissao
    );
}
```

---

## 12. Formulas e Calculos

### 12.1 Calculo de Margem

#### RN-FOR-001: Margem por Grupo
```
MargemGrupo = (MargemBruta * PercentualMargemBruta) / 100
```

#### RN-FOR-002: Margem Disponivel
```
MargemDisponivel = MargemFolha - SomaDeducoes
Onde:
  SomaDeducoes = SUM(ValorDeducaoMargem) para averbacoes onde DeduzMargem = true
```

#### RN-FOR-003: Margem com Refinanciamento
```
MargemDisponivel = MargemBase + ValorParcela(contratoRefinanciado)
```

### 12.2 Calculo de Emprestimo

#### RN-FOR-004: Coeficiente
```
Coeficiente = ValorParcela / ValorContratado
```

#### RN-FOR-005: Valor Consignado
```
ValorConsignado = Prazo * ValorParcela
```

#### RN-FOR-006: Competencia Final
```
CompetenciaFinal = CompetenciaInicial + (Prazo - 1) meses
```

### 12.3 Calculo de Saldo

#### RN-FOR-007: Saldo Restante
```
SaldoRestante = ParcelasRestantes * ValorParcela
Onde:
  ParcelasRestantes = COUNT(parcelas onde Competencia >= CompetenciaCorte)
```

#### RN-FOR-008: Prazo Restante
```
PrazoRestante = COUNT(parcelas onde Competencia >= CompetenciaCorte)
```

### 12.4 Calculo de Competencia

#### RN-FOR-009: Aumentar Competencia
```
CompetenciaAumenta(anoMes, meses):
  mes = anoMes.mes + meses
  ano = anoMes.ano + (mes / 12)
  mes = mes % 12
  if (mes == 0) { mes = 12; ano-- }
  return ano/mes
```

#### RN-FOR-010: Diminuir Competencia
```
CompetenciaDiminui(anoMes, meses):
  return CompetenciaAumenta(anoMes, -meses)
```

---

## 13. Excecoes e Casos Especiais

### 13.1 Autorizacoes Especiais

#### RN-EXC-001: Bypass de Validacoes
Funcionarios com autorizacao especial tipo 0 (Independente de qualquer restricao) ignoram todas as validacoes:
- Margem
- Situacao funcional
- Bloqueios
- Limites de prazo

### 13.2 Aposentadoria

#### RN-EXC-002: Impacto da Aposentadoria
- Funcionario muda para situacao Aposentado
- Averbacoes ativas permanecem ativas
- Novas averbacoes requerem autorizacao especial
- Margem pode ser recalculada com novo salario

### 13.3 Exoneracao

#### RN-EXC-003: Impacto da Exoneracao
- Funcionario muda para situacao Exonerado ou Retirado da Folha
- Averbacoes ativas sao tratadas caso a caso
- Novas averbacoes nao sao permitidas

### 13.4 Suspensao de Empresa

#### RN-EXC-004: Situacoes de Empresa
```
EmpresaSituacao:
  1 = Normal
  2 = Suspenso para Averbacoes
  3 = Suspenso para Compra
  4 = Bloqueado
  5 = Bloqueio Personalizado
```

#### RN-EXC-005: Restricoes por Situacao de Empresa
- **Suspenso para Averbacoes**: Nao permite averbacoes normais
- **Suspenso para Compra**: Nao permite compra de divida
- **Bloqueado**: Nao permite nenhuma operacao

### 13.5 Compra Cancelada

#### RN-EXC-006: Cancelamento de Compra
Nao e permitido cancelar compra quando:
- Existem solicitacoes de saldo devedor ja processadas
- Existem quitacoes ja informadas
- Contratos originais ja foram marcados como Comprado

### 13.6 Quitacao Rejeitada

#### RN-EXC-007: Tratamento de Rejeicao
Quando a consignataria vendedora rejeita a quitacao:
1. Sistema cria solicitacao de "Regularizar Quitacao Rejeitada"
2. Compradora deve resolver pendencia
3. Pode informar nova quitacao ou cancelar compra

### 13.7 Liquidacao Automatica

#### RN-EXC-008: Expiração de Prazo
O sistema pode aplicar liquidacao automatica quando:
- Prazo de confirmacao de quitacao expira
- Parametro de liquidacao automatica esta ativo

---

## 14. Anexos

### 14.1 Diagrama de Estados - Averbacao

```
[Novo] --> [Reservado] --> [Aguardando Aprovacao] --> [Averbado] --> [Ativo]
                |                    |                    |             |
                |                    v                    |             v
                |              [Desaprovado]              |        [Suspenso]
                |                                         |             |
                |                                         v             v
                +--> [Em Processo Compra] --> [Averbado] --> [Bloqueado]
                                                              |
                                                              v
                                                         [Liquidado]
                                                              |
                                                              v
                                                         [Concluido]

                         [Cancelado] <-- (qualquer estado cancelavel)
```

### 14.2 Diagrama de Fluxo - Compra de Divida

```
Compradora                     Vendedora
    |                              |
    |-- Registra Compra ---------> |
    |                              |
    | <--- Informa Saldo Devedor --|
    |                              |
    |-- Informa Quitacao --------> |
    |                              |
    | <-- Confirma/Rejeita --------|
    |                              |
    |-- Conclui Compra ----------> |
    |                              |
    [Novo Contrato Averbado]   [Contrato Original Liquidado]
```

### 14.3 Grupos de Produto Padrao

| ID | Nome | Tipo | % Margem |
|----|------|------|----------|
| 1 | Emprestimos | Prazo Determinado | 30-35% |
| 7 | Mensalidades | Prazo Indeterminado | 5-10% |

---

## 15. Historico de Revisoes

| Versao | Data | Autor | Descricao |
|--------|------|-------|-----------|
| 1.0 | Janeiro/2026 | Analise de Sistema | Documento inicial baseado no sistema legado |

---

*Documento gerado a partir da analise do codigo-fonte do sistema FastConsig (.NET Framework 4.0)*
