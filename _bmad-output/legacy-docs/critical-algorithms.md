# Critical Algorithms

## Overview

This document analyzes the specific implementations of critical mathematical and logical operations within the FastConsig system. These algorithms dictate financial accuracy, data integrity, and business rule enforcement.

## 1. Margin Calculation (`Calculo de Margem`)

**Location:** `CP.FastConsig.BLL.Funcionarios.ListarMargens(int idFuncionario)`

The system does not store a "Current Margin" value directly on the employee record. Instead, it calculates it on-the-fly to ensure consistency.

### 1.1. The Algorithm

1.  **Grouping**: Employee margins are grouped by **Product Group** (`ProdutoGrupo`).
2.  **Total Margin (`MargemFolha`)**: Sum of all `MargemFolha` entries assigned to the employee for that group.
3.  **Used Margin (`MargemUtilizada`)**:
    *   Iterates through all **Active Loans** (`Averbacao`) for the employee.
    *   **Filter**: Loans must be in a status that "Deducts Margin" (`AverbacaoSituacao.DeduzMargem == true`).
    *   **Scope**: Loans must belong to a product group that **shares** the margin limit with the current group (`IDProdutoGrupoCompartilha`).
    *   **Summation**: Sums the `ValorParcela` (Installment Value) of these qualifying loans.

### 1.2. Implications
*   **Real-time**: Margin is always current based on active loans.
*   **Shared Margins**: The `IDProdutoGrupoCompartilha` property allows different products (e.g., "Personal Loan" and "Refinancing") to consume the same margin pot (e.g., "30% Legal Limit").

## 2. Outstanding Balance Calculation (`Saldo Devedor`)

**Location:** `CP.FastConsig.BLL.Averbacoes.CalculaSaldoRestante(int idaverbacao)`

Used to estimate the payoff amount for Refinancing or Debt Purchase.

### 2.1. Logic
The system uses a linear calculation based on remaining installments, rather than a Present Value (PV) calculation with interest discounting in this specific method (though PV might happen elsewhere for specific bank integrations).

### 2.2. Steps
1.  **Determine Cutoff (`ObtemAnoMesCorte`)**: Calculates the current billing cycle (competence) for the Consignee.
2.  **Count Remaining Installments**:
    *   Counts parcels where `Competencia` >= Current Billing Cycle.
    *   If no parcels generated yet, uses the full `Prazo` (Term).
3.  **Calculate Balance**: `Remaining Count * ValorParcela`.

## 3. Margin Deduction for Refinancing

**Location:** `CP.FastConsig.BLL.Averbacoes.CalculaValorDeducaoMargem(int IdAverbacao)`

When a loan is refinanced, the system must calculate the *net impact* on the employee's margin.

### 3.1. Formula
$$
\text{NetImpact} = \text{NewInstallment} - \sum(\text{OldInstallments})
$$

*   **NewInstallment**: `ValorParcela` of the new loan.
*   **OldInstallments**: Sum of `ValorParcela` of all loans being refinanced (linked via `AverbacaoVinculo`) where the status implies margin deduction.

### 3.2. Usage
This value (`ValorDeducaoMargem`) is stored on the `Averbacao` entity. If negative, it implies the refinancing actually *freed up* margin.

## 4. Billing Cycle Calculation (`Competencia`)

**Location:** `CP.FastConsig.BLL.Averbacoes.ObtemAnoMesCorte`

Determines the "Active" month for billing/payroll purposes. This is critical for preventing operations in a closed month.

### 4.1. Variables
*   **Day**: Today's day.
*   **DiaCorte**: The configured cutoff day (Global Parameter or Company-specific).
*   **Current Month**: Today's Month/Year.

### 4.2. Logic
1.  **Company Override**: Checks if the specific Consignataria (Bank) has a specific `DiaCorte`.
2.  **Comparison**:
    *   If `Today.Day > DiaCorte`: The current operational month is **Next Month**.
    *   If `Today.Day <= DiaCorte`: The current operational month is **This Month**.
3.  **Result**: Returns string in format "YYYY/MM".

## 5. Installment Generation (`GerarParcelas`)

**Location:** `CP.FastConsig.BLL.Averbacoes.GerarParcelas`

Generates the schedule of payments (`AverbacaoParcela`) once a loan is finalized.

### 5.1. Process
1.  **Check**: If parcels exist, abort.
2.  **Loop**: 1 to `Prazo`.
3.  **Competence**:
    *   Parcel 1: `CompetenciaInicial` (Input on loan).
    *   Parcel N: `PreviousMonth + 1 Month`.
4.  **Value**: `ValorParcela` (Fixed value).
5.  **Status**: Initialized as `Aberta` (Open).

## 6. Debt Purchase Settlement (`ProcessarFluxoCompra`)

**Location:** `CP.FastConsig.BLL.Averbacoes.ProcessarFluxoCompra`

Orchestrates the complex state changes when a debt is bought.

### 6.1. Algorithm
1.  **Verify State**: Checks if the new loan is in `EmProcessodeCompra`.
2.  **Check Solicitations**: Reviews the chain of `EmpresaSolicitacao` tasks.
3.  **Finalization Trigger**: When `ConcluirCompradeDivida` is processed:
    *   **Liquidate Old**: Finds all linked loans (from other banks).
    *   **Update Status**: Sets them to `Liquidado`.
    *   **Update Parcel Status**: Sets their parcels to `LiquidadaManual` (to prevent payroll discount of the old loan).
    *   **Activate New**: Sets the new loan to `Averbado`.
    *   **Adjust Margin**: Updates `ValorDeducaoMargem` to reflect the final reality.
    *   **Generate Parcels**: Calls `GerarParcelas` for the new loan.
