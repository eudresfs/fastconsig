# UI Component Inventory

## Technology
*   **Framework**: ASP.NET WebForms 4.0
*   **Component Library**: DevExpress ASP.NET Controls (v11.1)
*   **Client-Side**: jQuery 1.6.3, jQuery UI 1.8.16

## Web User Controls (`.ascx`)

The application makes heavy use of User Controls to modularize the interface.

### Functional Categories

#### Averbacao (Loan) Management
*   `WebUserControlAverbacao.ascx`: Main control for loan data entry/editing.
*   `WebUserControlAverbacaoCancelar.ascx`: Loan cancellation interface.
*   `WebUserControlAverbacaoLiquidar.ascx`: Loan liquidation/payoff.
*   `WebUserControlAverbacaoSuspenderBloquear.ascx`: Suspension/Blocking logic.
*   `WebUserControlGerenciarAverbacao.ascx`: Management dashboard for loans.
*   `WebUserControlMinhasAverbacoes.ascx`: Employee's view of their loans.
*   `WebUserControlTermoAverbacao.ascx`: Generates the loan agreement document.

#### Employee & User Management
*   `WebUserControlFuncionarioAposentar.ascx`
*   `WebUserControlFuncionarios.ascx`: Employee CRUD.
*   `WebUserControlUsuario.ascx`: User CRUD.
*   `WebUserControlDadosUsuario.ascx`: User profile data.
*   `WebUserControlUsuariosPermissoes.ascx`: Permission management.
*   `WebUserControlPerfis.ascx`: Profile management.

#### Dashboard & Charts
*   `WebUserControlDashBoard.ascx`: Main dashboard container.
*   `WebUserControlChartArea.ascx`
*   `WebUserControlChartBarra.ascx`
*   `WebUserControlChartPizza.ascx`
*   `WebUserControlChartLinha.ascx`
*   `WebUserControlChartVolumeAverbacoes.ascx`

#### Configuration & Admin
*   `WebUserControlConfigurar.ascx`
*   `WebUserControlConsignatarias.ascx`
*   `WebUserControlProdutos.ascx`
*   `WebUserControlParametros.ascx` (Inferred from context)
*   `WebUserControlAuditoria.ascx`

#### Import/Export
*   `WebUserControlImportacao.ascx`
*   `WebUserControlExportacao.ascx`
*   `WebUserControlImportacaoFiltros.ascx`

## Pages (`.aspx`)

*   **Main**: `Default.aspx`, `Login.aspx`
*   **Specific Features**: `ListaAverbacoesVinc.aspx`, `Ocorrencia.aspx`, `VideoPlay.aspx`.
*   **Master Page**: `NewMasterPrincipal.Master` - Defines the common layout, menu structure, and theme.

## Design System

*   **Themes**: Uses App_Themes (`Aqua`, `Componentes`).
*   **CSS**: `StyleSheet.css`, `EstilosGerais.css`, `EstilosFormularios.css`.
*   **Layout**: `ASPxRoundPanel` is frequently used to group content sections.
*   **Grids**: `ASPxGridView` is the standard for data listing.
