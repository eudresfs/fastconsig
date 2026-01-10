#  DOCUMENTAÇÃO COMPLETA DO SISTEMA FASTCONSIG PARA MIGRAÇÃO

  1. VISÃO GERAL DO SISTEMA

  1.1 Informações Básicas

  - Nome: FastConsig - Sistema de Gestão de Consignados
  - Tecnologia: ASP.NET WebForms + .NET Framework 4.0
  - IDE Original: Visual Studio 2010
  - Solução: app .NET/CP.FastConsig.sln
  - Banco de Dados: SQL Server com Entity Framework 4.3.1 (Database First)

  1.2 Propósito do Sistema

  Sistema brasileiro de gestão de empréstimos consignados (payroll loans) que gerencia:
  - Averbações (empréstimos consignados em folha de pagamento)
  - Funcionários de órgãos públicos
  - Consignatárias (instituições financeiras)
  - Consignantes (empregadores/órgãos públicos)
  - Margens consignáveis
  - Parcelas e descontos em folha
  - Fluxos de aprovação

  ---
  2. ARQUITETURA DA SOLUÇÃO

  2.1 Diagrama de Camadas

  ┌─────────────────────────────────────────────────────────────┐
  │                    CAMADA DE APRESENTAÇÃO                   │
  │              CP.FastConsig.WebApplication                   │
  │     (ASP.NET WebForms, Master Pages, User Controls)         │
  └─────────────────────────────────────────────────────────────┘
                                │
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │                      CAMADA FACADE                          │
  │                  CP.FastConsig.Facade                       │
  │         (Simplifica acesso à BLL, 50+ classes)              │
  └─────────────────────────────────────────────────────────────┘
                                │
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │                CAMADA DE LÓGICA DE NEGÓCIO                  │
  │                    CP.FastConsig.BLL                        │
  │        (Classes estáticas com regras de negócio)            │
  └─────────────────────────────────────────────────────────────┘
                                │
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │                 CAMADA DE ACESSO A DADOS                    │
  │                    CP.FastConsig.DAL                        │
  │     (Entity Framework EDMX, Repository Pattern)             │
  └─────────────────────────────────────────────────────────────┘
                                │
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │                      SQL SERVER                             │
  │            (FastConsig + FastConsigCenter)                  │
  └─────────────────────────────────────────────────────────────┘

  2.2 Projetos da Solução
  Projeto: CP.FastConsig.WebApplication
  Tipo: Web Application
  Descrição: Camada de apresentação ASP.NET WebForms
  ────────────────────────────────────────
  Projeto: CP.FastConsig.Facade
  Tipo: Class Library
  Descrição: Padrão Facade para simplificar acesso à BLL
  ────────────────────────────────────────
  Projeto: CP.FastConsig.BLL
  Tipo: Class Library
  Descrição: Lógica de negócio (classes estáticas)
  ────────────────────────────────────────
  Projeto: CP.FastConsig.DAL
  Tipo: Class Library
  Descrição: Acesso a dados com Entity Framework
  ────────────────────────────────────────
  Projeto: CP.FastConsig.Common
  Tipo: Class Library
  Descrição: Enums e constantes compartilhadas
  ────────────────────────────────────────
  Projeto: CP.FastConsig.Util
  Tipo: Class Library
  Descrição: Classes utilitárias
  ────────────────────────────────────────
  Projeto: CP.FastConsig.Services
  Tipo: WCF Service
  Descrição: Serviços WCF para integração
  ────────────────────────────────────────
  Projeto: CP.FastConsig.Test
  Tipo: Test Project
  Descrição: Testes unitários (MSTest)
  ────────────────────────────────────────
  Projeto: CP.FastConsig.Source
  Tipo: Content
  Descrição: Recursos estáticos
  ────────────────────────────────────────
  Projeto: CP.FastConsig.ThirdParty
  Tipo: Class Library
  Descrição: Bibliotecas de terceiros
  ────────────────────────────────────────
  Projeto: CP.FastConsig.Password
  Tipo: Console App
  Descrição: Utilitário de senha
  ────────────────────────────────────────
  Projeto: CP.FastConsig.DB.Main
  Tipo: RedGate SQL
  Descrição: Source control do banco Main
  ────────────────────────────────────────
  Projeto: CP.FastConsig.DB.Center
  Tipo: RedGate SQL
  Descrição: Source control do banco Center
  ---
  3. MODELO DE DOMÍNIO

  3.1 Entidades Principais

  Averbacao (Empréstimo Consignado)

  Entidade central que representa um contrato de empréstimo consignado.

  Campos Principais:
  - IDAverbacao (PK) - Identificador único
  - IDFuncionario (FK) - Funcionário que contratou
  - IDEmpresa (FK) - Consignatária (banco/financeira)
  - IDProduto (FK) - Produto financeiro
  - NumeroContrato - Número do contrato
  - ValorParcela - Valor da parcela mensal
  - ValorTotal - Valor total do empréstimo
  - ValorLiquido - Valor líquido recebido
  - QuantidadeParcelas - Número de parcelas
  - TaxaJuros - Taxa de juros
  - DataContratacao - Data da contratação
  - CompetenciaInicio - Mês/ano do primeiro desconto
  - CompetenciaFim - Mês/ano do último desconto
  - IDAverbacaoSituacao - Status atual (enum AverbacaoSituacao)
  - IDAverbacaoTipo - Tipo (Normal, Compra, Renegociação)

  Status Possíveis (AverbacaoSituacao):
  0 = Cancelado
  1 = Ativo
  2 = Averbado
  3 = AguardandoAprovacao
  4 = Reservado
  5 = Desaprovado
  6 = Suspenso_MargemLivre
  7 = Bloqueado_MargemRetida
  8 = EmProcessodeCompra
  9 = Comprado
  10 = Liquidado
  11 = Concluido
  12 = PreReserva

  Funcionario

  Funcionário público que pode contratar empréstimos.

  Campos Principais:
  - IDFuncionario (PK)
  - IDPessoa (FK) - Dados pessoais
  - IDEmpresa (FK) - Consignante (empregador)
  - Matricula - Número de matrícula
  - DataAdmissao - Data de admissão
  - DataDemissao - Data de demissão (se aplicável)
  - IDFuncionarioSituacao - Status do funcionário
  - SalarioBruto - Salário bruto
  - MargemLivre - Margem disponível para consignação

  Status do Funcionário (FuncionarioSituacao):
  0 = NaoInformado
  1 = AtivoNaFolha
  2 = RetiradoDaFolha
  3 = Exonerado
  4 = Bloqueado
  5 = Aposentado

  Empresa

  Representa tanto Consignantes quanto Consignatárias.

  Campos Principais:
  - IDEmpresa (PK)
  - IDPessoa (FK) - Dados jurídicos
  - IDEmpresaTipo - Tipo da empresa
  - IDEmpresaSituacao - Situação atual
  - RazaoSocial - Nome da empresa
  - NomeFantasia - Nome fantasia
  - CNPJ - Documento

  Tipos de Empresa (EmpresaTipo):
  1 = CasePartners (desenvolvedor)
  2 = Consignante (empregador)
  3 = Agente
  4 = Banco
  5 = Financeira
  6 = Sindicato
  7 = Associacao
  8 = Convenio

  Pessoa

  Dados pessoais/jurídicos compartilhados.

  Campos Principais:
  - IDPessoa (PK)
  - Nome / NomeCompleto
  - CPF / CNPJ
  - DataNascimento
  - Email
  - Telefone
  - Endereco, Cidade, Estado, CEP

  Usuario e UsuarioPerfil

  Sistema de autenticação e autorização.

  Usuario:
  - IDUsuario (PK)
  - IDPessoa (FK)
  - Login - Nome de usuário
  - Senha - Senha (MD5 hash)
  - SenhaProvisoria - Flag de senha temporária
  - Bloqueado - Status de bloqueio
  - UltimoAcesso - Data/hora do último acesso
  - QuantidadeAcessos - Contador de acessos

  UsuarioPerfil:
  - IDUsuarioPerfil (PK)
  - IDUsuario (FK)
  - IDEmpresa (FK)
  - IDModulo - Módulo de acesso
  - IDPerfil - Perfil de permissões

  AverbacaoParcela

  Parcelas individuais de cada averbação.

  Campos Principais:
  - IDAverbacaoParcela (PK)
  - IDAverbacao (FK)
  - NumeroParcela - Número sequencial
  - Competencia - Mês/ano do desconto (formato AAAA/MM)
  - ValorParcela - Valor da parcela
  - IDAverbacaoParcelaSituacao - Status da parcela

  Status da Parcela (AverbacaoParcelaSituacao):
  0 = Cancelada
  1 = Aberta
  2 = LiquidadaFolha
  3 = LiquidadaManual
  4 = RejeitadaFolha

  Produto e ProdutoGrupo

  Produtos financeiros oferecidos.

  ProdutoGrupo:
  - IDProdutoGrupo (PK)
  - Nome - Ex: "Empréstimos", "Mensalidades"

  Produto:
  - IDProduto (PK)
  - IDProdutoGrupo (FK)
  - Nome - Nome do produto
  - PrazoMinimo / PrazoMaximo
  - ValorMinimo / ValorMaximo
  - TaxaJurosMinima / TaxaJurosMaxima

  Solicitacao

  Sistema de workflow para aprovações.

  Campos Principais:
  - IDSolicitacao (PK)
  - IDSolicitacaoTipo - Tipo da solicitação
  - IDSolicitacaoSituacao - Status
  - IDAverbacao (FK) - Averbação relacionada
  - IDEmpresaSolicitante - Quem solicitou
  - IDEmpresaDestino - Para quem
  - DataSolicitacao - Data/hora
  - Observacao - Observações

  Tipos de Solicitação (SolicitacaoTipo):
  1 = InformarSaldoDevedordeContratos
  2 = InformarQuitacao
  3 = ConfirmarRejeitarQuitacao
  4 = ConcluirCompradeDivida
  5 = AprovarAverbacoes
  6 = DesliquidarAverbacoes
  7 = CancelarAverbacoes
  ...etc

  3.2 Relacionamentos Principais

  Pessoa 1 ←──── N Funcionario
  Pessoa 1 ←──── N Empresa
  Pessoa 1 ←──── N Usuario

  Funcionario 1 ←──── N Averbacao
  Empresa (Consignataria) 1 ←──── N Averbacao
  Produto 1 ←──── N Averbacao

  Averbacao 1 ←──── N AverbacaoParcela
  Averbacao 1 ←──── N AverbacaoVinculo (auto-relacionamento)
  Averbacao 1 ←──── N Solicitacao

  Usuario 1 ←──── N UsuarioPerfil
  Empresa 1 ←──── N UsuarioPerfil

  ProdutoGrupo 1 ←──── N Produto

  ---
  4. CAMADA DAL - ACESSO A DADOS

  4.1 Padrão Repository

  O sistema implementa um Repository genérico com as seguintes características:

  Interface IRepositorio:
  public interface IRepositorio<T> where T : class
  {
      int Incluir(T objeto);
      void Alterar(T objeto);
      void Excluir(T objeto);
      void Excluir(int id);
      string DadosString(T objeto);
      T ObterPorId(int id);
      IQueryable<T> Listar();
      void Atachar(T entity);
  }

  Características da Implementação:

  1. Soft Delete: Se a entidade possui campo Ativo, a exclusão marca como inativo em vez de deletar fisicamente.
  2. Auditoria Automática: Todas as operações CRUD são registradas na tabela Auditoria com:
    - Tabela afetada
    - Chave primária
    - Dados do registro (string)
    - Usuário, Módulo, Perfil, Recurso
    - IP e Browser do cliente
    - Tipo de operação (I/U/D)
  3. Campos de Controle:
    - CreatedBy, CreatedOn - Preenchidos na inclusão
    - ModifiedOn - Atualizado na alteração
  4. String de Conexão por Sessão: A conexão é selecionada dinamicamente via HttpContext.Current.Session["NomeStringConexao"].
  5. Metadados Dinâmicos: Usa reflection no Entity Framework para descobrir campos, chaves e nomes de tabelas.

  4.2 Modelo Entity Framework

  - Arquivo EDMX: ModeloFastConsig.edmx
  - Abordagem: Database First
  - Templates T4: Geram classes de entidade e contexto

  4.3 Multi-Tenancy

  O sistema suporta múltiplos bancos de dados (clientes) através de:

  <!-- Conexoes.config -->
  <connectionStrings>
    <add name="LOCALHOST" connectionString="...FastConsigItaqua..." />
    <add name="IPSMI" connectionString="...FastConsigIpsmi..." />
    <add name="ITAQUA" connectionString="...FastConsigItaqua..." />
    <add name="SANTOAMANCIO" connectionString="...FastConsigSantoAmancio..." />
    <add name="SAOMARCOS" connectionString="...FastConsigSaoMarcos..." />
    <add name="TREINAMENTO" connectionString="...FastConsigItaquaTeste..." />
  </connectionStrings>

  ---
  5. CAMADA BLL - REGRAS DE NEGÓCIO

  5.1 Padrão de Implementação

  - Classes Estáticas: Todas as classes BLL são estáticas
  - Instanciação de Repositório: Cada método cria seu próprio repositório
  - Sem Injeção de Dependência: Acoplamento direto

  Exemplo de padrão:
  public static class Averbacoes
  {
      public static Averbacao ObtemAverbacao(int id)
      {
          var rep = new Repositorio<Averbacao>();
          return rep.ObterPorId(id);
      }

      public static IQueryable<Averbacao> ListaAverbacao()
      {
          var rep = new Repositorio<Averbacao>();
          return rep.Listar();
      }
  }

  5.2 Classes BLL Principais
  Classe: Averbacoes.cs
  Responsabilidade: CRUD e regras de empréstimos, aprovações, cancelamentos,
    liquidações
  ────────────────────────────────────────
  Classe: Funcionarios.cs
  Responsabilidade: Gestão de funcionários, margens, bloqueios
  ────────────────────────────────────────
  Classe: Consignatarias.cs
  Responsabilidade: Gestão de bancos/financeiras, relatórios de inadimplência
  ────────────────────────────────────────
  Classe: Usuarios.cs
  Responsabilidade: Autenticação, perfis, senhas
  ────────────────────────────────────────
  Classe: Empresas.cs
  Responsabilidade: Gestão de consignantes/consignatárias
  ────────────────────────────────────────
  Classe: Produtos.cs
  Responsabilidade: Produtos financeiros e coeficientes
  ────────────────────────────────────────
  Classe: Solicitacoes.cs
  Responsabilidade: Workflow de aprovações
  ────────────────────────────────────────
  Classe: Importacao.cs
  Responsabilidade: Importação de dados (funcionários, contratos, retorno folha)
  ────────────────────────────────────────
  Classe: Exportacao.cs
  Responsabilidade: Exportação de arquivos
  5.3 Regras de Negócio Críticas

  Cálculo de Margem

  - Margem = % do salário que pode ser comprometido com consignados
  - Margem Livre = Margem Total - Soma das parcelas ativas
  - Verificação obrigatória antes de nova averbação

  Fluxo de Aprovação

  1. Averbação criada com status AguardandoAprovacao
  2. Solicitação de aprovação criada
  3. Consignante aprova/reprova
  4. Status atualizado para Averbado ou Desaprovado

  Compra de Dívida

  1. Identificar averbações elegíveis para compra
  2. Calcular saldo devedor
  3. Criar nova averbação do tipo Compra
  4. Vincular averbações originais
  5. Processar liquidação das originais

  Conciliação Mensal

  - Comparar valores enviados para desconto vs. valores descontados
  - Identificar parcelas rejeitadas
  - Calcular inadimplência

  ---
  6. CAMADA FACADE

  6.1 Propósito

  Simplifica o acesso à BLL para a camada de apresentação, agrupando operações relacionadas.

  6.2 Lista de Fachadas (50+ classes)

  Principais:
  - FachadaAverbacoes.cs - Operações de empréstimos
  - FachadaFuncionariosConsulta.cs - Consulta de funcionários
  - FachadaGerenciarAverbacao.cs - Gerenciamento de contratos
  - FachadaLogin.cs - Autenticação
  - FachadaDashBoard.cs - Indicadores do dashboard
  - FachadaConciliacao.cs - Conciliação mensal
  - FachadaImportacao.cs - Importação de dados
  - FachadaExportacao.cs - Exportação de dados
  - FachadaFluxoAprovacao.cs - Workflow de aprovações
  - FachadaCentralSimulacao.cs - Simulações de empréstimo
  - FachadaConsignatarias.cs - Gestão de consignatárias
  - FachadaUsuariosPermissoes.cs - Permissões de acesso

  6.3 Padrão de Implementação

  Cada fachada delega para classes BLL:

  public class FachadaAverbacoes
  {
      public Averbacao ObtemAverbacao(int id)
      {
          return Averbacoes.ObtemAverbacao(id);
      }

      public bool Aprovar(int idAverbacao, int idEmpresa, int idResponsavel)
      {
          return Averbacoes.Aprovar(idAverbacao, idEmpresa, idResponsavel);
      }
  }

  ---
  7. CAMADA DE APRESENTAÇÃO

  7.1 Estrutura

  - Master Page: NewMasterPrincipal.Master - Layout principal
  - Página Principal: Default.aspx - Container para User Controls
  - Login: Login.aspx - Autenticação multi-banco
  - User Controls: 80+ controles em WebUserControls/

  7.2 Padrão de Navegação

  1. Usuário faz login selecionando banco e perfil
  2. Master Page carrega menu baseado em permissões
  3. Clique no menu carrega User Control dinamicamente no PlaceHolder
  4. Estado mantido na sessão via GerenciadorSessao

  7.3 Classes Base

  CustomPage.cs:
  public class CustomPage : Page
  {
      protected NewMasterPrincipal MasterPrincipal;
      protected GerenciadorSessao Sessao;
  }

  CustomUserControl.cs:
  public class CustomUserControl : UserControl
  {
      protected NewMasterPrincipal MasterPrincipal;
      protected GerenciadorSessao Sessao;
      // Métodos para carregar scripts, exibir mensagens, etc.
  }

  7.4 Bibliotecas de UI

  - DevExpress v11.1: Grids, editores, gráficos, popups
  - AjaxControlToolkit: Extensores AJAX
  - jQuery 1.6.3: Manipulação DOM
  - jQuery UI 1.8.16: Dialogs, datepickers
  - Highcharts: Gráficos JavaScript
  - iTextSharp: Geração de PDF
  - ClosedXML: Geração de Excel
  - SpreadsheetGear: Manipulação de planilhas

  ---
  8. CAMADA COMMON

  8.1 Enums Principais

  // Módulos do Sistema
  public enum Modulos
  {
      Consignante = 1,
      Funcionario = 2,
      Consignataria = 3,
      Agente = 4,
      Sindicato = 6
  }

  // Status da Averbação
  public enum AverbacaoSituacao
  {
      Cancelado = 0,
      Ativo = 1,
      Averbado = 2,
      AguardandoAprovacao = 3,
      Reservado = 4,
      Desaprovado = 5,
      Suspenso_MargemLivre = 6,
      Bloqueado_MargemRetida = 7,
      EmProcessodeCompra = 8,
      Comprado = 9,
      Liquidado = 10,
      Concluido = 11,
      PreReserva = 12
  }

  // Tipos de Permissão
  public enum Permissao
  {
      Acessar = 1,
      Incluir = 2,
      Alterar = 3,
      Excluir = 4,
      Consultar = 5,
      // ... 48 tipos de permissão
  }

  // Tipos de Empresa
  public enum EmpresaTipo
  {
      CasePartners = 1,
      Consignante = 2,
      Agente = 3,
      Banco = 4,
      Financeira = 5,
      Sindicato = 6,
      Associacao = 7,
      Convenio = 8
  }

  ---
  9. CAMADA UTIL

  9.1 StringHelper

  Métodos de manipulação de string:
  - Base64StringEncode/Decode
  - MD5String, SHA1String
  - MaskString - Aplicação de máscaras
  - StripTags - Remove HTML
  - WordWrap, TitleCase, SentenceCase

  9.2 Seguranca

  public static class Seguranca
  {
      private static string semente = "..."; // Salt para hash

      public static string getMd5Hash(string input)
      {
          // Concatena input + semente e gera MD5
      }

      public static bool verifyMd5Hash(string input, string hash)
      {
          return getMd5Hash(input) == hash;
      }
  }

  9.3 Utilidades

  - ValidaCPF(string cpf) - Validação de CPF brasileiro
  - ValidaCNPJ(string cnpj) - Validação de CNPJ
  - ValidaEmail(string email) - Validação de email
  - MascaraCPF/MascaraCNPJ - Formatação
  - CompetenciaAumenta/Diminui - Cálculos de período (AAAA/MM)
  - SomaDiasUteis - Cálculo de dias úteis

  9.4 DadosSessao

  Acesso simplificado à sessão HTTP:
  public class DadosSessao
  {
      public int IdModulo { get; }
      public int IdBanco { get; }
      public int IdPerfil { get; }
      public int IdUsuario { get; }
      public int IdRecurso { get; }
      public string NomeRecurso { get; }
  }

  ---
  10. SERVIÇOS WCF

  10.1 ServiceUsuario

  Serviço WCF para integração entre sistemas (Center).

  Endpoint:
  <endpoint 
      address="http://localhost:9998/ServiceUsuario.svc" 
      binding="basicHttpBinding" 
      contract="FastConsigCenterService.IServicoUsuario" />

  Modelo de Dados Center:
  - EDMX separado: FastConsigCenter.edmx
  - Banco de dados: FastConsigCenter
  - Gerencia dados compartilhados entre múltiplos clientes

  ---
  11. PADRÕES DE DESIGN IDENTIFICADOS

  11.1 Padrões Utilizados
  ┌─────────────────┬─────────────────────────────────────────┐
  │     Padrão      │                   Uso                   │
  ├─────────────────┼─────────────────────────────────────────┤
  │ Repository      │ DAL - Abstração do acesso a dados       │
  ├─────────────────┼─────────────────────────────────────────┤
  │ Facade          │ Camada Facade - Simplifica acesso à BLL │
  ├─────────────────┼─────────────────────────────────────────┤
  │ Template Method │ CustomPage/CustomUserControl            │
  ├─────────────────┼─────────────────────────────────────────┤
  │ Soft Delete     │ Exclusão lógica via campo Ativo         │
  ├─────────────────┼─────────────────────────────────────────┤
  │ Multi-Tenancy   │ Seleção de banco por sessão             │
  └─────────────────┴─────────────────────────────────────────┘
  11.2 Anti-Patterns Identificados
  Anti-Pattern: Static Classes na BLL
  Descrição: Todas as classes de negócio são estáticas
  Impacto na Migração: Dificulta testes unitários e DI
  ────────────────────────────────────────
  Anti-Pattern: Acoplamento com HttpContext
  Descrição: DAL acessa Session diretamente
  Impacto na Migração: Impede uso fora de contexto web
  ────────────────────────────────────────
  Anti-Pattern: Classes Anêmicas
  Descrição: Entidades sem comportamento
  Impacto na Migração: Toda lógica está na BLL
  ────────────────────────────────────────
  Anti-Pattern: God Classes
  Descrição: Averbacoes.cs com 100+ métodos
  Impacto na Migração: Difícil manutenção
  ────────────────────────────────────────
  Anti-Pattern: Magic Strings
  Descrição: Nomes de campos como strings
  Impacto na Migração: Sujeito a erros
  ────────────────────────────────────────
  Anti-Pattern: Falta de Interfaces
  Descrição: BLL sem contratos
  Impacto na Migração: Impossível mockar
  ---
  12. RECOMENDAÇÕES PARA MIGRAÇÃO

  12.1 Stack Tecnológico Sugerido
  ┌──────────────────────┬─────────────────────────────────┐
  │        Atual         │      Migração Recomendada       │
  ├──────────────────────┼─────────────────────────────────┤
  │ .NET Framework 4.0   │ .NET 8 (LTS)                    │
  ├──────────────────────┼─────────────────────────────────┤
  │ ASP.NET WebForms     │ ASP.NET Core MVC ou Blazor      │
  ├──────────────────────┼─────────────────────────────────┤
  │ Entity Framework 4.3 │ Entity Framework Core 8         │
  ├──────────────────────┼─────────────────────────────────┤
  │ DevExpress WebForms  │ DevExpress Blazor ou MudBlazor  │
  ├──────────────────────┼─────────────────────────────────┤
  │ jQuery               │ Blazor components ou Minimal JS │
  ├──────────────────────┼─────────────────────────────────┤
  │ WCF Services         │ ASP.NET Core Web API / gRPC     │
  └──────────────────────┴─────────────────────────────────┘
  12.2 Arquitetura Sugerida

  ┌─────────────────────────────────────────────────────────────┐
  │                    FRONTEND (Blazor/React)                  │
  └─────────────────────────────────────────────────────────────┘
                                │ HTTP/REST
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │                     API Controllers                         │
  └─────────────────────────────────────────────────────────────┘
                                │
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │                  Application Services                       │
  │               (Use Cases / CQRS Handlers)                   │
  └─────────────────────────────────────────────────────────────┘
                                │
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │                     Domain Layer                            │
  │           (Entities, Value Objects, Domain Services)        │
  └─────────────────────────────────────────────────────────────┘
                                │
                                ▼
  ┌─────────────────────────────────────────────────────────────┐
  │                  Infrastructure Layer                       │
  │        (EF Core DbContext, Repositories, External APIs)     │
  └─────────────────────────────────────────────────────────────┘

  12.3 Estratégia de Migração

  Fase 1 - Preparação
  1. Criar testes de integração para funcionalidades críticas
  2. Documentar todas as regras de negócio da BLL
  3. Mapear todas as consultas SQL e stored procedures

  Fase 2 - Infraestrutura
  1. Criar novo projeto .NET 8
  2. Migrar modelo de dados para EF Core (Code First from Database)
  3. Implementar novo padrão Repository com interfaces
  4. Configurar multi-tenancy moderno

  Fase 3 - Domínio
  1. Refatorar entidades para Rich Domain Model
  2. Extrair regras de negócio para Domain Services
  3. Implementar Application Services com CQRS

  Fase 4 - Apresentação
  1. Criar API REST para cada funcionalidade
  2. Desenvolver frontend Blazor/React
  3. Migrar tela por tela, mantendo paridade funcional

  Fase 5 - Integração
  1. Substituir WCF por Web API/gRPC
  2. Migrar jobs e processos batch
  3. Validar integrações externas

  ---
  13. MÉTRICAS DO PROJETO
  ┌─────────────────────────────┬─────────┐
  │           Métrica           │  Valor  │
  ├─────────────────────────────┼─────────┤
  │ Projetos na Solução         │ 14      │
  ├─────────────────────────────┼─────────┤
  │ Classes BLL                 │ ~20     │
  ├─────────────────────────────┼─────────┤
  │ Classes Facade              │ 50+     │
  ├─────────────────────────────┼─────────┤
  │ User Controls               │ 80+     │
  ├─────────────────────────────┼─────────┤
  │ Entidades no EDMX           │ 60+     │
  ├─────────────────────────────┼─────────┤
  │ Enums                       │ 25+     │
  ├─────────────────────────────┼─────────┤
  │ Linhas de Código (estimado) │ 50.000+ │
  └─────────────────────────────┴─────────┘
  ---

● A análise completa da codebase FastConsig foi concluída. Aqui está um resumo executivo do que foi documentado:

  Resumo da Análise Completa

  Sistema Analisado

  FastConsig - Sistema brasileiro de gestão de empréstimos consignados (crédito descontado em folha de pagamento), desenvolvido em ASP.NET WebForms com .NET Framework 4.0.

  Arquitetura Documentada

  WebApplication (Apresentação)
         ↓
      Facade (50+ classes)
         ↓
      BLL (Lógica de Negócio - classes estáticas)
         ↓
      DAL (Entity Framework 4.3 + Repository Pattern)
         ↓
     SQL Server (Multi-tenant)

  Principais Componentes Documentados
  Camada: DAL
  Itens Documentados: Repository genérico, soft delete, auditoria automática,
    multi-tenancy por sessão
  ────────────────────────────────────────
  Camada: BLL
  Itens Documentados: 100+ métodos em Averbacoes.cs, regras de margem, fluxos de
    aprovação, compra de dívida
  ────────────────────────────────────────
  Camada: Facade
  Itens Documentados: 50+ fachadas mapeando apresentação → BLL
  ────────────────────────────────────────
  Camada: Common
  Itens Documentados: 25+ enums (AverbacaoSituacao, Permissao, EmpresaTipo, etc.)
  ────────────────────────────────────────
  Camada: Util
  Itens Documentados: StringHelper, Seguranca (MD5), Utilidades (CPF/CNPJ),
    DadosSessao
  ────────────────────────────────────────
  Camada: WebApplication
  Itens Documentados: Master Page, Login, 80+ User Controls, padrão de navegação
  ────────────────────────────────────────
  Camada: Services
  Itens Documentados: WCF ServiceUsuario, modelo Center separado
  Entidades Principais Mapeadas

  - Averbacao - Empréstimo consignado (12 status possíveis)
  - Funcionario - Servidor público (5 status)
  - Empresa - Consignante/Consignatária (8 tipos)
  - AverbacaoParcela - Parcelas mensais
  - Usuario/UsuarioPerfil - Autenticação e permissões
  - Solicitacao - Workflow de aprovações (18 tipos)
  - Produto/ProdutoGrupo - Produtos financeiros

  Anti-Patterns Identificados para Correção na Migração

  1. Classes BLL estáticas (sem DI/testabilidade)
  2. Acoplamento DAL → HttpContext.Session
  3. God Classes (Averbacoes.cs com 100+ métodos)
  4. Entidades anêmicas (sem comportamento)
  5. Magic strings para nomes de campos

  Recomendações de Migração

  - .NET 8 com Entity Framework Core 8
  - Blazor ou ASP.NET Core MVC para frontend
  - Arquitetura Clean Architecture ou CQRS
  - Web API/gRPC substituindo WCF
  - Implementar Domain-Driven Design para regras de negócio