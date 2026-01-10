# Wireframes e Prototipos - FastConsig

**Versao:** 1.0
**Data:** Janeiro 2026
**Status:** Draft

---

## 1. Principios de Design

### 1.1 Design System

| Aspecto | Especificacao |
|---------|---------------|
| **Metodologia** | Mobile-first, Progressive Enhancement |
| **Framework CSS** | Tailwind CSS 4+ |
| **Componentes** | shadcn/ui (Radix-based) |
| **Icones** | Lucide Icons |
| **Acessibilidade** | WCAG 2.1 AA |

### 1.2 Paleta de Cores

```
Primaria (Brand):
- Primary-500: #2563EB (Azul principal)
- Primary-600: #1D4ED8 (Hover)
- Primary-700: #1E40AF (Active)

Semanticas:
- Success: #16A34A (Verde)
- Warning: #CA8A04 (Amarelo)
- Error: #DC2626 (Vermelho)
- Info: #0284C7 (Azul claro)

Neutras:
- Gray-50: #F9FAFB (Background)
- Gray-100: #F3F4F6 (Cards)
- Gray-200: #E5E7EB (Borders)
- Gray-500: #6B7280 (Text secondary)
- Gray-900: #111827 (Text primary)
```

### 1.3 Tipografia

```
Font Family: Inter (Google Fonts)

Headings:
- H1: 30px / Bold / Gray-900
- H2: 24px / Semibold / Gray-900
- H3: 20px / Semibold / Gray-900
- H4: 16px / Semibold / Gray-900

Body:
- Body: 14px / Regular / Gray-700
- Small: 12px / Regular / Gray-500
- Caption: 11px / Medium / Gray-400
```

### 1.4 Espacamentos

```
Base: 4px

Escala:
- xs: 4px
- sm: 8px
- md: 16px
- lg: 24px
- xl: 32px
- 2xl: 48px
```

### 1.5 Componentes Base

| Componente | Especificacao |
|------------|---------------|
| **Button** | Height 40px, border-radius 6px, font-medium |
| **Input** | Height 40px, border 1px gray-200, focus ring primary |
| **Card** | Background white, border-radius 8px, shadow-sm |
| **Modal** | Max-width 600px, overlay rgba(0,0,0,0.5) |
| **Table** | Header gray-50, hover gray-100, border-b gray-200 |

---

## 2. Layout Master

### 2.1 Estrutura Geral

```
+------------------------------------------------------------------+
|                           HEADER (64px)                          |
|  [Logo]              [Busca Global]         [Notif] [User Menu]  |
+------------------------------------------------------------------+
|        |                                                         |
|        |                                                         |
| SIDE   |                    CONTENT AREA                         |
| BAR    |                                                         |
| (240px)|                    (flex-1)                             |
|        |                                                         |
|        |                                                         |
|        |                                                         |
+--------+---------------------------------------------------------+
|                           FOOTER (48px)                          |
|                    FastConsig v1.0 | Suporte                     |
+------------------------------------------------------------------+
```

### 2.2 Header

```
+--------------------------------------------------------------------+
| [=] [FASTCONSIG]        [____Buscar funcionario____]  [🔔3] [👤▼] |
+--------------------------------------------------------------------+

Elementos:
- Toggle sidebar (mobile)
- Logo/Nome do sistema
- Campo de busca global (funcionarios por CPF/nome)
- Icone de notificacoes com badge
- Menu do usuario (dropdown com: Meus Dados, Alterar Senha, Sair)
```

### 2.3 Sidebar

```
+------------------------+
| [Logo Orgao]           |
| Prefeitura Municipal   |
+------------------------+
| MENU PRINCIPAL         |
|                        |
| [📊] Dashboard         |
| [👥] Funcionarios      |
| [📄] Averbacoes        |
| [💰] Simulacoes        |
| [🔄] Conciliacao       |
| [📥] Import/Export     |
| [🏢] Consignatarias    |
| [📈] Relatorios        |
+------------------------+
| CONFIGURACOES          |
|                        |
| [👤] Usuarios          |
| [🔐] Perfis            |
| [⚙️] Parametros        |
| [📋] Auditoria         |
+------------------------+
| [?] Ajuda              |
+------------------------+

Estados:
- Collapsed: apenas icones (64px)
- Expanded: icones + texto (240px)
- Item ativo: background primary-50, border-left primary-500
```

### 2.4 Content Area

```
+----------------------------------------------------------+
| BREADCRUMB                                                |
| Dashboard > Funcionarios > Joao da Silva                  |
+----------------------------------------------------------+
| PAGE HEADER                                               |
| [Titulo da Pagina]                    [Acoes Principais]  |
| Descricao opcional                    [+ Novo] [Exportar] |
+----------------------------------------------------------+
|                                                           |
|                      PAGE CONTENT                         |
|                                                           |
|   Cards, Tabelas, Formularios, etc.                      |
|                                                           |
+----------------------------------------------------------+
```

---

## 3. Telas de Autenticacao

### 3.1 Login

```
+------------------------------------------+
|                                          |
|           [LOGO FASTCONSIG]              |
|                                          |
|        Sistema de Consignados            |
|                                          |
|  +------------------------------------+  |
|  |                                    |  |
|  |  Email ou Usuario                  |  |
|  |  [_____________________________]   |  |
|  |                                    |  |
|  |  Senha                             |  |
|  |  [_____________________________]   |  |
|  |                                    |  |
|  |  [ ] Lembrar-me                    |  |
|  |                                    |  |
|  |  [        ENTRAR        ]          |  |
|  |                                    |  |
|  |  Esqueceu a senha?                 |  |
|  |                                    |  |
|  +------------------------------------+  |
|                                          |
|        © 2026 FastConsig                 |
+------------------------------------------+

Background: Gradiente azul ou imagem institucional
Card: Branco, centralizado, max-width 400px
```

### 3.2 Recuperacao de Senha

```
+------------------------------------------+
|           [LOGO FASTCONSIG]              |
|                                          |
|        Recuperar Senha                   |
|                                          |
|  +------------------------------------+  |
|  |                                    |  |
|  |  Informe seu email cadastrado:     |  |
|  |                                    |  |
|  |  Email                             |  |
|  |  [_____________________________]   |  |
|  |                                    |  |
|  |  [     ENVIAR LINK     ]           |  |
|  |                                    |  |
|  |  <- Voltar para login              |  |
|  |                                    |  |
|  +------------------------------------+  |
+------------------------------------------+
```

### 3.3 Primeiro Acesso (Troca de Senha)

```
+------------------------------------------+
|           [LOGO FASTCONSIG]              |
|                                          |
|      Defina sua nova senha               |
|                                          |
|  +------------------------------------+  |
|  |                                    |  |
|  |  ⚠️ Por seguranca, voce precisa    |  |
|  |  definir uma nova senha.           |  |
|  |                                    |  |
|  |  Nova Senha                        |  |
|  |  [_____________________________]   |  |
|  |  Min. 8 caracteres, letras e nums  |  |
|  |                                    |  |
|  |  Confirmar Senha                   |  |
|  |  [_____________________________]   |  |
|  |                                    |  |
|  |  [      CONFIRMAR      ]           |  |
|  |                                    |  |
|  +------------------------------------+  |
+------------------------------------------+
```

---

## 4. Dashboard

### 4.1 Dashboard Consignante

```
+------------------------------------------------------------------+
| Dashboard                                           [Jan/2026 ▼]  |
+------------------------------------------------------------------+
|                                                                   |
| +------------------+ +------------------+ +--------------------+  |
| | 👥 5.234         | | 💰 R$ 17.5M      | | 📊 40%             |  |
| | Funcionarios     | | Margem Total     | | Utilizacao         |  |
| | Ativos           | |                  | |                    |  |
| | ↑ 12 este mes    | | R$ 7M utilizada  | | ████████░░░░       |  |
| +------------------+ +------------------+ +--------------------+  |
|                                                                   |
| +------------------+ +------------------+ +--------------------+  |
| | ⏳ 23            | | ✅ 156           | | ❌ 8               |  |
| | Pendentes        | | Aprovadas (mes)  | | Rejeitadas (mes)   |  |
| | Aprovacao        | |                  | |                    |  |
| | [Ver todas ->]   | | R$ 2.3M          | | R$ 120K            |  |
| +------------------+ +------------------+ +--------------------+  |
|                                                                   |
| +--------------------------------+ +-----------------------------+|
| | Volume por Consignataria       | | Evolucao Mensal             ||
| |                                | |                             ||
| | BB        ████████████ 45%     | |     ____                    ||
| | CEF       ████████░░░░ 32%     | |    /    \    /\             ||
| | Bradesco  █████░░░░░░░ 15%     | |   /      \  /  \___         ||
| | Outros    ███░░░░░░░░░  8%     | |  /        \/                ||
| |                                | | Jan Fev Mar Abr Mai Jun     ||
| +--------------------------------+ +-----------------------------+|
|                                                                   |
+------------------------------------------------------------------+
```

### 4.2 Dashboard Consignataria

```
+------------------------------------------------------------------+
| Dashboard - Banco do Brasil                         [Jan/2026 ▼]  |
+------------------------------------------------------------------+
|                                                                   |
| +------------------+ +------------------+ +--------------------+  |
| | 📄 156           | | 💵 R$ 2.3M       | | 📈 1.95%           |  |
| | Contratos (mes)  | | Producao (mes)   | | Taxa Media         |  |
| |                  | |                  | |                    |  |
| | ↑ 23% vs anterior| | ↑ R$ 400K        | | CET: 2.1%          |  |
| +------------------+ +------------------+ +--------------------+  |
|                                                                   |
| +------------------+ +------------------+ +--------------------+  |
| | 💼 R$ 8.5M       | | ⏳ 12            | | 🔴 2.5%            |  |
| | Carteira Total   | | Pipeline         | | Inadimplencia      |  |
| |                  | | (em aprovacao)   | |                    |  |
| | 523 contratos    | | R$ 180K          | | 13 contratos       |  |
| +------------------+ +------------------+ +--------------------+  |
|                                                                   |
| +--------------------------------+ +-----------------------------+|
| | Ranking de Agentes             | | Producao Semanal            ||
| |                                | |                             ||
| | 1. Maria Silva    R$ 450K  ⭐  | |         ___                 ||
| | 2. Joao Santos    R$ 380K      | |    ____/   \                ||
| | 3. Ana Costa      R$ 290K      | |   /         \___            ||
| | 4. Pedro Lima     R$ 210K      | |  /                          ||
| | [Ver todos ->]                 | | Seg Ter Qua Qui Sex         ||
| +--------------------------------+ +-----------------------------+|
|                                                                   |
+------------------------------------------------------------------+
```

---

## 5. Funcionarios

### 5.1 Lista de Funcionarios

```
+------------------------------------------------------------------+
| Funcionarios                                    [+ Novo] [Export] |
| Gerencie os servidores do orgao                                   |
+------------------------------------------------------------------+
| Filtros:                                                          |
| [CPF________] [Nome____________] [Empresa ▼] [Situacao ▼] [Buscar]|
+------------------------------------------------------------------+
| □ | CPF          | Nome              | Matricula | Empresa  | ... |
+------------------------------------------------------------------+
| □ | 123.***.***-01| JOAO DA SILVA    | 001234    | Prefeit. | ●A |
| □ | 987.***.***-00| MARIA SANTOS     | 001235    | Camara   | ●A |
| □ | 111.***.***-22| PEDRO COSTA      | 001236    | Prefeit. | ○I |
| □ | 222.***.***-33| ANA OLIVEIRA     | 001237    | IPREV    | ●A |
+------------------------------------------------------------------+
| Mostrando 1-20 de 5.234        [<] [1] [2] [3] ... [262] [>]     |
+------------------------------------------------------------------+

Legenda situacao:
●A = Ativo (verde)
○I = Inativo (cinza)
●F = Afastado (amarelo)
●B = Bloqueado (vermelho)
●P = Aposentado (azul)

Acoes por linha (hover): [👁 Ver] [✏️ Editar] [📄 Averbacoes]
```

### 5.2 Ficha do Funcionario

```
+------------------------------------------------------------------+
| <- Voltar                                                         |
+------------------------------------------------------------------+
| +--------+                                                        |
| | [FOTO] |  JOAO DA SILVA                              [Editar]  |
| |        |  CPF: 123.456.789-01 | Matricula: 001234              |
| +--------+  Prefeitura Municipal - Analista                       |
|             Situacao: ● Ativo                                     |
+------------------------------------------------------------------+
|                                                                   |
| [Dados Pessoais] [Margem] [Averbacoes] [Historico]               |
|                                                                   |
+------------------------------------------------------------------+
|                                                                   |
| ABA: MARGEM                                                       |
|                                                                   |
| +------------------------+  +--------------------------------+    |
| | MARGEM DISPONIVEL      |  | COMPOSICAO                     |    |
| |                        |  |                                |    |
| | R$ 1.475,00            |  | Emprestimo  ████████░░ R$ 450 |    |
| |                        |  | Cartao      ░░░░░░░░░░ R$ 0   |    |
| | de R$ 1.925,00 (35%)   |  |                                |    |
| | Salario: R$ 5.500,00   |  | Total: R$ 450,00/mes          |    |
| +------------------------+  +--------------------------------+    |
|                                                                   |
| AVERBACOES ATIVAS                                    [+ Nova]     |
| +----------------------------------------------------------------+|
| | Contrato      | Consignataria | Parcela | Restam | Status     ||
| +----------------------------------------------------------------+|
| | CTR2024001234 | Banco Brasil  | R$ 450  | 36/48  | ● Descont. ||
| +----------------------------------------------------------------+|
|                                                                   |
+------------------------------------------------------------------+
```

### 5.3 Formulario de Funcionario

```
+------------------------------------------------------------------+
| Novo Funcionario                                                  |
+------------------------------------------------------------------+
|                                                                   |
| DADOS PESSOAIS                                                    |
| +-------------------------------+ +-----------------------------+ |
| | CPF *                         | | Data Nascimento *           | |
| | [___.___.___-__]              | | [__/__/____]                | |
| +-------------------------------+ +-----------------------------+ |
| | Nome Completo *                                               | |
| | [__________________________________________________]          | |
| +-------------------------------+ +-----------------------------+ |
| | Email                         | | Telefone                    | |
| | [_____________________]       | | [(__) _____-____]           | |
| +-------------------------------+ +-----------------------------+ |
|                                                                   |
| DADOS FUNCIONAIS                                                  |
| +-------------------------------+ +-----------------------------+ |
| | Matricula *                   | | Empresa *                   | |
| | [______________]              | | [Selecione...          ▼]  | |
| +-------------------------------+ +-----------------------------+ |
| | Cargo                         | | Data Admissao *             | |
| | [______________]              | | [__/__/____]                | |
| +-------------------------------+ +-----------------------------+ |
| | Salario Bruto *               | | Situacao *                  | |
| | [R$ ___________]              | | [Ativo               ▼]    | |
| +-------------------------------+ +-----------------------------+ |
|                                                                   |
| DADOS BANCARIOS                                                   |
| +------------------+ +------------------+ +--------------------+  |
| | Banco            | | Agencia          | | Conta              |  |
| | [001 - BB   ▼]   | | [____]           | | [_________-_]      |  |
| +------------------+ +------------------+ +--------------------+  |
|                                                                   |
|                                    [Cancelar]  [Salvar]           |
+------------------------------------------------------------------+
```

---

## 6. Averbacoes

### 6.1 Lista de Averbacoes

```
+------------------------------------------------------------------+
| Averbacoes                                          [+ Nova]      |
+------------------------------------------------------------------+
| Filtros Rapidos: [Todas] [Pendentes] [Ativas] [Encerradas]       |
+------------------------------------------------------------------+
| Filtros Avancados:                                    [Limpar]    |
| [Funcionario___] [Consignataria ▼] [Status ▼] [Periodo: __ a __] |
+------------------------------------------------------------------+
|                                                                   |
| □ | Contrato      | Funcionario      | Consig. | Parcela | Status|
+------------------------------------------------------------------+
| □ | CTR2024001234 | JOAO DA SILVA    | BB      | R$ 450  | ⏳ AG |
| □ | CTR2024001235 | MARIA SANTOS     | CEF     | R$ 380  | ✅ DE |
| □ | CTR2024001236 | PEDRO COSTA      | Bradesco| R$ 520  | ❌ RJ |
+------------------------------------------------------------------+
| [Aprovar Selecionados] [Rejeitar Selecionados]                   |
+------------------------------------------------------------------+

Status:
⏳ AG = Aguardando Aprovacao (amarelo)
✅ AP = Aprovada (verde claro)
✅ DE = Descontada (verde)
❌ RJ = Rejeitada (vermelho)
⏸️ SU = Suspensa (cinza)
🔒 BL = Bloqueada (vermelho escuro)
✔️ LI = Liquidada (azul)
```

### 6.2 Nova Averbacao (Wizard 3 Passos)

**Passo 1: Selecionar Funcionario**
```
+------------------------------------------------------------------+
| Nova Averbacao                                        Passo 1/3   |
+------------------------------------------------------------------+
|  (●)────────( )────────( )                                        |
|  Funcionario  Dados      Confirmacao                              |
+------------------------------------------------------------------+
|                                                                   |
| Buscar Funcionario:                                               |
| [CPF ou Nome do funcionario____________________] [Buscar]         |
|                                                                   |
| +--------------------------------------------------------------+ |
| | JOAO DA SILVA                                                 | |
| | CPF: 123.456.789-01 | Matricula: 001234                       | |
| | Prefeitura Municipal - Analista                               | |
| |                                                               | |
| | Margem Disponivel: R$ 1.475,00                                | |
| | (Emprestimo: R$ 1.200,00 | Cartao: R$ 275,00)                 | |
| |                                                  [Selecionar] | |
| +--------------------------------------------------------------+ |
|                                                                   |
|                                              [Cancelar] [Proximo]|
+------------------------------------------------------------------+
```

**Passo 2: Dados da Averbacao**
```
+------------------------------------------------------------------+
| Nova Averbacao                                        Passo 2/3   |
+------------------------------------------------------------------+
|  (✓)────────(●)────────( )                                        |
|  Funcionario  Dados      Confirmacao                              |
+------------------------------------------------------------------+
| Funcionario: JOAO DA SILVA (001234) | Margem: R$ 1.200,00        |
+------------------------------------------------------------------+
|                                                                   |
| +-------------------------------+ +-----------------------------+ |
| | Produto *                     | | Tipo Operacao *             | |
| | [Emprestimo Consignado ▼]     | | [Novo              ▼]       | |
| +-------------------------------+ +-----------------------------+ |
|                                                                   |
| +-------------------------------+ +-----------------------------+ |
| | Numero do Contrato *          | | Data do Contrato *          | |
| | [CTR2024__________]           | | [__/__/____]                | |
| +-------------------------------+ +-----------------------------+ |
|                                                                   |
| +-------------------------------+ +-----------------------------+ |
| | Valor Total *                 | | Quantidade de Parcelas *    | |
| | [R$ __________]               | | [48 meses          ▼]       | |
| +-------------------------------+ +-----------------------------+ |
|                                                                   |
| +-------------------------------+ +-----------------------------+ |
| | Taxa Mensal (%) *             | | Valor da Parcela            | |
| | [1,99]                        | | R$ 450,00 (calculado)       | |
| +-------------------------------+ +-----------------------------+ |
|                                                                   |
| ⚠️ Valor da parcela dentro da margem disponivel                  |
|                                                                   |
|                                    [Voltar] [Cancelar] [Proximo] |
+------------------------------------------------------------------+
```

**Passo 3: Confirmacao**
```
+------------------------------------------------------------------+
| Nova Averbacao                                        Passo 3/3   |
+------------------------------------------------------------------+
|  (✓)────────(✓)────────(●)                                        |
|  Funcionario  Dados      Confirmacao                              |
+------------------------------------------------------------------+
|                                                                   |
| RESUMO DA AVERBACAO                                               |
|                                                                   |
| +--------------------------------------------------------------+ |
| | Funcionario: JOAO DA SILVA                                    | |
| | CPF: 123.456.789-01 | Matricula: 001234                       | |
| +--------------------------------------------------------------+ |
| | Contrato: CTR2024001234                                       | |
| | Produto: Emprestimo Consignado                                | |
| | Tipo: Novo                                                    | |
| +--------------------------------------------------------------+ |
| | Valor Total:       R$ 15.000,00                               | |
| | Parcelas:          48x de R$ 450,00                           | |
| | Taxa Mensal:       1,99% a.m.                                 | |
| | CET Anual:         28,93% a.a.                                | |
| | Inicio Desconto:   02/2026                                    | |
| +--------------------------------------------------------------+ |
|                                                                   |
| [ ] Confirmo que os dados estao corretos                         |
|                                                                   |
|                          [Voltar] [Cancelar] [Enviar p/ Aprovacao]|
+------------------------------------------------------------------+
```

### 6.3 Tela de Aprovacao

```
+------------------------------------------------------------------+
| Aprovar Averbacao                                                 |
+------------------------------------------------------------------+
|                                                                   |
| DADOS DO FUNCIONARIO                                              |
| +--------------------------------------------------------------+ |
| | JOAO DA SILVA                                                 | |
| | CPF: 123.456.789-01 | Matricula: 001234                       | |
| | Prefeitura Municipal - Analista | Situacao: Ativo             | |
| | Salario: R$ 5.500,00 | Margem Disponivel: R$ 1.475,00         | |
| +--------------------------------------------------------------+ |
|                                                                   |
| DADOS DA AVERBACAO                                                |
| +--------------------------------------------------------------+ |
| | Contrato: CTR2024001234 | Consignataria: Banco do Brasil      | |
| | Produto: Emprestimo Consignado | Tipo: Novo                   | |
| |                                                               | |
| | Valor: R$ 15.000,00 | Parcelas: 48x R$ 450,00                 | |
| | Taxa: 1,99% a.m. | CET: 28,93% a.a.                           | |
| | Inicio: 02/2026 | Fim: 01/2030                                | |
| +--------------------------------------------------------------+ |
|                                                                   |
| VALIDACOES                                                        |
| ✅ Margem disponivel suficiente                                   |
| ✅ Funcionario ativo                                              |
| ✅ Consignataria conveniada                                       |
| ✅ Prazo dentro do limite                                         |
|                                                                   |
| Observacao (opcional):                                            |
| [__________________________________________________________]     |
|                                                                   |
|               [Rejeitar]  [Solicitar Info]  [✓ Aprovar]          |
+------------------------------------------------------------------+
```

---

## 7. Simulador

### 7.1 Simulacao de Emprestimo

```
+------------------------------------------------------------------+
| Simulador de Emprestimo                                           |
+------------------------------------------------------------------+
|                                                                   |
| Funcionario: [Buscar por CPF ou Nome________________] [Buscar]    |
|                                                                   |
| +--------------------------------------------------------------+ |
| | JOAO DA SILVA                                                 | |
| | Margem Disponivel: R$ 1.200,00                                | |
| +--------------------------------------------------------------+ |
|                                                                   |
| +---------------------------+ +-------------------------------+   |
| | Simular por:              | | Produto:                      |   |
| | (●) Valor desejado        | | [Emprestimo Consignado ▼]     |   |
| | ( ) Valor da parcela      | |                               |   |
| +---------------------------+ +-------------------------------+   |
|                                                                   |
| Valor desejado: [R$ 15.000,00_______]  [Simular]                 |
|                                                                   |
+------------------------------------------------------------------+
| RESULTADO DA SIMULACAO                                            |
+------------------------------------------------------------------+
|                                                                   |
| | Prazo  | Parcela   | Valor Liq. | Taxa    | CET     | Status  | |
| |--------|-----------|------------|---------|---------|---------|  |
| | 24x    | R$ 750,00 | R$ 14.520  | 1,89%   | 27,45%  | ⚠️      | |
| | 36x    | R$ 550,00 | R$ 14.520  | 1,95%   | 28,10%  | ✅      | |
| | 48x    | R$ 450,00 | R$ 14.520  | 1,99%   | 28,93%  | ✅ Best | |
| | 60x    | R$ 395,00 | R$ 14.520  | 2,05%   | 29,75%  | ✅      | |
|                                                                   |
| ✅ = Dentro da margem  ⚠️ = Excede margem                        |
|                                                                   |
|                            [Imprimir]  [Criar Averbacao 48x ->]  |
+------------------------------------------------------------------+
```

---

## 8. Conciliacao

### 8.1 Tela de Conciliacao

```
+------------------------------------------------------------------+
| Conciliacao Mensal                                                |
+------------------------------------------------------------------+
| Competencia: [Janeiro/2026 ▼]                    [Importar Retorno]|
+------------------------------------------------------------------+
|                                                                   |
| RESUMO                                                            |
| +----------------+ +----------------+ +----------------+          |
| | 📤 500         | | ✅ 480         | | ❌ 20          |          |
| | Enviados       | | Descontados    | | Divergentes    |          |
| | R$ 225.000     | | R$ 216.000     | | R$ 9.000       |          |
| +----------------+ +----------------+ +----------------+          |
|                                                                   |
| DIVERGENCIAS                                       [Tratar Todas] |
| +----------------------------------------------------------------+|
| | Funcionario      | Contrato      | Enviado | Desc. | Motivo   ||
| +----------------------------------------------------------------+|
| | JOAO DA SILVA    | CTR001234     | R$ 450  | R$ 0  | Afastado ||
| | MARIA SANTOS     | CTR001235     | R$ 380  | R$ 190| Parcial  ||
| | [Tratar]         |               |         |       |          ||
| +----------------------------------------------------------------+|
|                                                                   |
| Status: ⏳ Em andamento          [Fechar Competencia]             |
+------------------------------------------------------------------+
```

---

## 9. Relatorios

### 9.1 Tela de Relatorio

```
+------------------------------------------------------------------+
| Relatorio de Producao                                             |
+------------------------------------------------------------------+
|                                                                   |
| FILTROS                                                           |
| +-------------------------------+ +-----------------------------+ |
| | Periodo                       | | Consignataria               | |
| | [01/01/2026] a [31/01/2026]   | | [Todas               ▼]    | |
| +-------------------------------+ +-----------------------------+ |
| +-------------------------------+ +-----------------------------+ |
| | Produto                       | | Agrupamento                 | |
| | [Todos                 ▼]     | | [Por Consignataria   ▼]    | |
| +-------------------------------+ +-----------------------------+ |
|                                                                   |
|                    [Limpar]  [Gerar Relatorio]                   |
|                                                                   |
+------------------------------------------------------------------+
| RESULTADO                                  [Excel] [PDF] [Imprimir]|
+------------------------------------------------------------------+
|                                                                   |
| Periodo: 01/01/2026 a 31/01/2026                                 |
| Total: 156 averbacoes | R$ 2.340.000,00                          |
|                                                                   |
| +----------------------------------------------------------------+|
| | Consignataria      | Qtd | Valor Total    | % Part. | Ticket  ||
| +----------------------------------------------------------------+|
| | Banco do Brasil    | 80  | R$ 1.200.000   | 51,3%   | R$ 15K  ||
| | Caixa Economica    | 50  | R$ 750.000     | 32,1%   | R$ 15K  ||
| | Bradesco           | 26  | R$ 390.000     | 16,6%   | R$ 15K  ||
| +----------------------------------------------------------------+|
| | TOTAL              | 156 | R$ 2.340.000   | 100%    | R$ 15K  ||
| +----------------------------------------------------------------+|
|                                                                   |
+------------------------------------------------------------------+
```

---

## 10. Componentes Reutilizaveis

### 10.1 DataTable

```
+------------------------------------------------------------------+
| [Buscar...________]                    [Colunas ▼] [Filtros ▼]   |
+------------------------------------------------------------------+
| □ | Coluna 1 ▲  | Coluna 2    | Coluna 3    | Acoes              |
+------------------------------------------------------------------+
| □ | Valor 1     | Valor 2     | Valor 3     | [👁] [✏️] [🗑]     |
| □ | Valor 1     | Valor 2     | Valor 3     | [👁] [✏️] [🗑]     |
| □ | Valor 1     | Valor 2     | Valor 3     | [👁] [✏️] [🗑]     |
+------------------------------------------------------------------+
| Selecionados: 2                 [<] 1 2 3 ... 10 [>] | 20/pag ▼ |
+------------------------------------------------------------------+

Features:
- Ordenacao por coluna (clique no header)
- Selecao multipla (checkbox)
- Acoes por linha (icons hover)
- Paginacao
- Configuracao de colunas visiveis
- Filtros avancados
- Busca global
```

### 10.2 Modal

```
+----------------------------------------+
| Titulo do Modal                    [X] |
+----------------------------------------+
|                                        |
|  Conteudo do modal aqui.              |
|                                        |
|  Pode conter formularios, textos,     |
|  confirmacoes, etc.                   |
|                                        |
+----------------------------------------+
|              [Cancelar]  [Confirmar]  |
+----------------------------------------+

Tamanhos: sm (400px), md (600px), lg (800px), xl (1000px)
Overlay: rgba(0,0,0,0.5) com click para fechar
```

### 10.3 KPI Card

```
+---------------------------+
| 📊 Titulo do KPI          |
|                           |
| R$ 1.234.567,00          |
|                           |
| ↑ 12% vs mes anterior    |
+---------------------------+

Variacoes:
- Com icone
- Com grafico sparkline
- Com progress bar
- Com comparativo
```

### 10.4 Alert/Toast

```
Tipos:

[✓] Sucesso: Operacao realizada com sucesso          [X]

[!] Alerta: Atencao, verifique os dados              [X]

[X] Erro: Ocorreu um erro ao processar               [X]

[i] Info: Voce tem 5 aprovacoes pendentes            [X]

Posicao: top-right
Duracao: 5 segundos (auto-dismiss)
```

---

## 11. Fluxos de Navegacao

### 11.1 Mapa do Site

```
Login
  |
  +-- Dashboard
        |
        +-- Funcionarios
        |     +-- Lista
        |     +-- Novo
        |     +-- Ficha
        |           +-- Dados Pessoais
        |           +-- Margem
        |           +-- Averbacoes
        |           +-- Historico
        |
        +-- Averbacoes
        |     +-- Lista
        |     +-- Nova (Wizard)
        |     +-- Detalhe
        |     +-- Aprovar
        |
        +-- Simulacoes
        |     +-- Emprestimo
        |     +-- Compra Divida
        |
        +-- Conciliacao
        |     +-- Mensal
        |     +-- Historico
        |
        +-- Import/Export
        |     +-- Importar
        |     +-- Exportar
        |     +-- Historico
        |
        +-- Consignatarias
        |     +-- Lista
        |     +-- Detalhe
        |
        +-- Relatorios
        |     +-- Producao
        |     +-- Margem
        |     +-- Inadimplencia
        |     +-- Auditoria
        |
        +-- Configuracoes
              +-- Usuarios
              +-- Perfis
              +-- Parametros
              +-- Meus Dados
```

---

## 12. Responsividade

### 12.1 Breakpoints

| Nome | Largura | Uso |
|------|---------|-----|
| `sm` | 640px | Mobile landscape |
| `md` | 768px | Tablet |
| `lg` | 1024px | Desktop pequeno |
| `xl` | 1280px | Desktop |
| `2xl` | 1536px | Desktop grande |

### 12.2 Adaptacoes Mobile

```
DESKTOP (>= 1024px)          MOBILE (< 768px)
+--------+-----------+       +------------------+
| Sidebar| Content   |       | [=] Header       |
|        |           |       +------------------+
|        |           |       |                  |
|        |           |  -->  |     Content      |
|        |           |       |                  |
|        |           |       +------------------+
+--------+-----------+       | [Nav] [Nav] [Nav]|
                             +------------------+

Mudancas:
- Sidebar vira drawer (toggle)
- Tabelas viram cards
- Filtros em modal/drawer
- Navegacao bottom tabs
- Botoes full-width
```

---

## 13. Historico de Revisoes

| Versao | Data | Autor | Descricao |
|--------|------|-------|-----------|
| 1.0 | Janeiro 2026 | Design Team | Versao inicial |

---

*Este documento define os wireframes e padroes visuais do sistema FastConsig.*
