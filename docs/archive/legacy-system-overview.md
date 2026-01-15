# Project Overview: FastConsig

## Executive Summary

FastConsig is a comprehensive payroll loan management system (Sistema de Consignados) tailored for the Brazilian market. It acts as an intermediary between **Consignantes** (Public/Private Employers) and **Consignatarias** (Banks/Financial Institutions), managing the entire lifecycle of payroll-deducted loans from simulation to contract liquidation.

The system allows employees to view their margins, simulate loans, and request payroll deductions, while providing administrators with tools for reconciliation, reporting, and user management.

## Tech Stack Summary

| Layer | Technology |
|-------|------------|
| **Frontend** | ASP.NET WebForms, DevExpress 11.1, jQuery |
| **Backend** | C# .NET 4.0, WCF |
| **Data** | Entity Framework 4.3.1, SQL Server |
| **Architecture** | N-Tier Monolith with Facade Pattern |

## Key Features

1.  **Loan Lifecycle Management**: Simulation, Request, Approval, Active, Suspended, Liquidated.
2.  **Margin Management**: Calculation of available payroll margin for employees.
3.  **Reconciliation**: Tools to reconcile payroll deductions with bank expectations.
4.  **Multi-Tenant Capabilities**: Supports multiple Consignantes and Consignatarias within the same database structure (`Empresa` entity).
5.  **Audit Trails**: Detailed tracking of user actions and data changes.

## Repository Structure

The project is a **Monolith** contained in a single Visual Studio Solution (`CP.FastConsig.sln`).

*   **Web Application**: `CP.FastConsig.WebApplication`
*   **Services**: `CP.FastConsig.Services`
*   **Business Logic**: `CP.FastConsig.BLL`, `CP.FastConsig.Facade`
*   **Data Access**: `CP.FastConsig.DAL`

## Documentation Map

*   [Architecture](./architecture.md) - High-level system design.
*   [Development Guide](./development-guide.md) - Setup and build instructions.
*   [API Contracts](./api-contracts-root.md) - WCF Service definitions.
*   [Data Models](./data-models-root.md) - Database schema details.
*   [Component Inventory](./component-inventory-root.md) - UI controls list.
*   [Deployment Guide](./deployment-guide.md) - IIS and production setup.
