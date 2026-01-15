# Business Rules & Logic Flows

## Overview

This document details the core business rules and logic flows of the FastConsig system, primarily implemented in the **Business Logic Layer (BLL)**. The system orchestrates the lifecycle of payroll-deducted loans (*averbações*), managing interactions between Employees (*Funcionarios*), Employers (*Consignantes*), and Financial Institutions (*Consignatarias*).

## 1. Loan Lifecycle (Averbacao)

The central entity is the `Averbacao`. Its lifecycle is managed by the `Averbacoes` class.

### 1.1. Creation Flow (`SalvarAverbacao`)

When a new loan is saved:
1.  **Data Update**: Employee personal data (`Pessoa`) is updated with the latest information provided.
2.  **Number Generation**: A unique protocol number is generated using `ProxNumero`.
3.  **Refinancing Calculation**: If the loan involves refinancing or buying debt, the system calculates the `ValorRefinanciado` based on selected existing contracts.
4.  **Linking**: Existing contracts involved in the operation are linked via `AverbacaoVinculo`.
5.  **Solicitation**: If employee approval is required and obtained immediately, a "Processed" solicitation is created.
6.  **Workflow Trigger**: The system calls `ProcessarTramitacoes` to determine the initial state.

### 1.2. Status Transition (`Tramitar`)

The system uses a rigid state machine enforced by the `Tramitar` method.
*   **Action**: Records a new entry in `AverbacaoTramitacao`.
*   **Audit**: Logs the `IDAverbacaoSituacao` (Status ID), `IDEmpresa` (Actor), and `OBS` (Reason).
*   **Key States**:
    *   `PreReserva` (12): Temporary reservation.
    *   `AguardandoAprovacao` (3): Pending external action.
    *   `Averbado` (2): Finalized active contract.
    *   `Liquidado` (10): Paid off (manually or via refinancing).
    *   `Cancelado` (0): Voided.
    *   `EmProcessodeCompra` (8): Under negotiation for debt purchase.

### 1.3. Installment Generation (`GerarParcelas`)

Installments (`AverbacaoParcela`) are generated **only** when the loan transitions to a finalized state (`Averbado`).
*   **Logic**: Iterates from 1 to `Prazo` (Term).
*   **Competence**: Starts from `CompetenciaInicial` and increments monthly using `Utilidades.CompetenciaAumenta`.
*   **Status**: Created as `Aberta` (1).

## 2. Approval Workflows (`FluxoAprovacoes`)

The system supports configurable approval chains defined in `FluxoAprovacao`.

### 2.1. Configuration
Rules are defined per **Product Group** (`ProdutoGrupo`) and can be specific to an **Company** (`Empresa`).
*   `RequerAprovacaoConsignante`: Employer must approve.
*   `RequerAprovacaoConsignataria`: Bank must approve.
*   `RequerAprovacaoFuncionario`: Employee must approve.

### 2.2. Processing Logic (`ProcessarAprovacao`)
Executed whenever a loan is saved or modified.
1.  **Check Configuration**: Determines which approvals are required.
2.  **Check Existing**: Verifies if a valid approval (Solicitation) already exists.
3.  **Create Solicitation**: If pending, creates a task (`EmpresaSolicitacao`) for the relevant party.
4.  **Auto-Approval**: Checks user permissions. If the user has specific rights (Permission 43), the system may auto-approve requests on behalf of the consignee.

## 3. Debt Purchase & Refinancing

Complex operations involving the payoff of existing loans to create a new one.

### 3.1. Debt Purchase Flow (`ProcessarFluxoCompra`)
Used when Bank B buys a loan from Bank A.
1.  **Initiation**: New loan saved with type `Compra` (2) or `CompraERenegociacao` (4). Target loans are linked.
2.  **Solicitation Chain**:
    *   `InformarSaldoDevedor` (1): Request sent to original bank.
    *   `InformarQuitacao` (2): Original bank confirms payoff amount.
    *   `ConfirmarRejeitarQuitacao` (3): Buying bank validates payment.
    *   `ConcluirCompradeDivida` (4): Finalization.
3.  **Liquidation**: Upon conclusion, the old loans are moved to `Liquidado` status ("Liquidado por Conclusão de Compra de Dívida").
4.  **Activation**: The new loan moves to `Averbado`.

### 3.2. Refinancing (`ProcessaRenegociacao`)
Used when renegotiating a loan within the same bank (`AverbacaoTipo.Renegociacao`).
1.  **Liquidation**: Old loans are immediately `Liquidado`.
2.  **Activation**: New loan becomes `Averbado`.
3.  **Margin**: The system calculates the net margin impact (`ValorDeducaoMargem`) to ensure the employee doesn't exceed limits during the transition.

## 4. Solicitation System (`Solicitacoes`)

The system relies heavily on an asynchronous task queue (`EmpresaSolicitacao`) to manage requests between different entities (e.g., Bank requesting balance from another Bank).

### 4.1. Core Entities
*   **Solicitante**: The entity (Company/User) initiating the request.
*   **Responsavel**: The entity (Company/User) expected to act.
*   **Status**: `Pendente` (1), `Processada` (2), `Rejeitada` (5).

### 4.2. Common Request Types
*   **Portability/Debt Purchase**: `InformarSaldoDevedordeContratos`, `InformarQuitacao`, `ConcluirCompradeDivida`.
*   **Approvals**: `AprovarAverbacoes`, `AprovarReservaporSimulacao`.
*   **Maintenance**: `CancelarAverbacoes`, `DesliquidarAverbacoes`.

### 4.3. Work Queue
The `ObtemSolicitacoesPendentes` method filters tasks based on the current user's context (e.g., if logged in as a Bank, shows only requests targeting that Bank).

## 5. Employee Management (`Funcionarios`)

### 5.1. Blocking Rules
*   Employees can be blocked from taking loans via `FuncionarioBloqueio`.
*   **Exception**: Users with `FuncionarioAutorizacaoTipo.IndependentedeBloqueio` can bypass these blocks.

### 5.2. Margin Calculation
*   Margins are defined per `ProdutoGrupo`.
*   Logic: `MargemFolha` (Total Allowed) - `MargemUtilizada` (Sum of `ValorParcela` of active loans).
*   **Shared Margins**: Some products may share margin limits (`IDProdutoGrupoCompartilha`).

## 6. Financial Reconciliation (`Conciliacao`)

*   **Objective**: Match system expectations (`ConciliacaoMovimento`) with payroll return files (`ConciliacaoRetorno`).
*   **Process**:
    1.  `ArquivoParaDescontoFolha`: Generates the file to be sent to the payroll processor.
    2.  **Import**: Return file is imported.
    3.  **Matching**: System compares `ValorDescontado` vs `Valor` (Expected).
    4.  **Reporting**: Generates discrepancies list.

## 7. Company & Product Configuration

### 7.1. Companies (`Empresas`)
*   **Types**: `Consignante` (2), `Agente` (3), `Banco` (4), `Sindicato` (6).
*   **Suspension**: Companies can be suspended (`EmpresaSuspensao`), preventing new operations for a period or indefinitely.
*   **Relationships**: `EmpresaVinculo` defines which Agents work for which Banks.

### 7.2. Products (`Produtos`)
*   **Groups**: `Emprestimos` (1), `Mensalidades` (7).
*   **Constraints**: Products define `PrazoMinimo`, `PrazoMaximo`, `ValorMinimo`, `ValorMaximo`.

## 8. Security & Permissions (`Permissoes`)

*   **RBAC**: Role-Based Access Control.
*   **Granularity**: Permissions are assigned to a (Company, Profile, Resource, PermissionType) tuple.
*   **Check Logic**: `CheckPermissao` verifies if a specific tuple exists in `PermissaoUsuario`.
