# Data Models & Database Schema

## Overview

The application uses **Entity Framework 4.3.1 (Database First)**. The data context is managed by `FastConsigEntities` located in `CP.FastConsig.DAL`.

## Core Entities

The database schema is extensive, covering user management, loan processing (averbação), auditing, and financial entity configuration.

### Main Domains

#### 1. Authentication & Users
*   **Usuario**: System users.
*   **Perfil**: User profiles/roles.
*   **Permissao**: System permissions.
*   **UsuarioPerfil**: Link between users and profiles.
*   **UsuarioResponsabilidade**: User responsibilities.
*   **UsuarioVinculo** (Implied): Links users to specific contexts.

#### 2. Consignment Entities
*   **Empresa**: Base entity for organizations.
*   **Consignante**: Employer organizations (deduct payments).
*   **Consignataria**: Financial institutions (banks).
*   **EmpresaVinculo**: Relationships between companies.
*   **EmpresaUsuario**: Users associated with companies.

#### 3. Loans (Averbacao)
*   **Averbacao**: Core loan record.
*   **AverbacaoParcela**: Installments of the loan.
*   **AverbacaoHistorico**: Audit trail for loan changes.
*   **AverbacaoSituacao**: Status of the loan (e.g., Pending, Active, Cancelled).
*   **AverbacaoTipo**: Type of loan product.
*   **Produto**: Financial products available.

#### 4. Audit & Logs
*   **Auditoria**: General audit logs.
*   **AuditoriaAcesso**: Login/access logs.
*   **AuditoriaOperacao**: Operation logs.
*   **Mensagem**: System messages/notifications.

#### 5. Financial & Reconciliation
*   **Conciliacao**: Reconciliation records.
*   **ConciliacaoMovimento**: Financial movements.
*   **Feriado**: Holidays (for business day calculations).

## Key Tables List

| Entity Name | Description |
|-------------|-------------|
| `Averbacao` | Central table for loan contracts. |
| `Funcionario` | Employee data (borrowers). |
| `Usuario` | System operators and administrators. |
| `Empresa` | Stores both Consignantes and Consignatarias. |
| `Parametro` | System-wide configuration parameters. |
| `Produto` | Loan products definition. |
| `Fluxo` | Workflow definitions for approvals. |

## Database Context

**Context Class:** `FastConsigEntities`
**Connection String Name:** `FastConsigEntities`

## Extensions & Utilities

The DAL also includes utility classes in `ModeloCustom` for complex reporting or unmapped queries:
- `AnaliseProducao`
- `VolumeAverbacoes`
- `IndiceNegocio`
