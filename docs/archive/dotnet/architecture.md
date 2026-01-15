# System Architecture

## Executive Summary

FastConsig is a **Monolithic Web Application** designed for managing payroll loans (consignado). It is built on the Microsoft .NET 4.0 stack, utilizing ASP.NET WebForms for the user interface, WCF for service orientation, and Entity Framework for data access. The architecture strictly follows an N-Tier layered approach to separate concerns.

## Architecture Pattern

**Pattern:** Layered Architecture (N-Tier) + Facade
**Style:** Monolith (Single Solution)

### Layers

1.  **Presentation Layer (`CP.FastConsig.WebApplication`)**
    *   **Responsibility**: User interaction, rendering HTML, session management.
    *   **Technologies**: ASP.NET WebForms (.aspx), DevExpress Controls, jQuery.
    *   **Communication**: Calls the Facade layer.

2.  **Facade Layer (`CP.FastConsig.Facade`)**
    *   **Responsibility**: Simplifies complexity of the BLL, provides a clean API for the UI.
    *   **Pattern**: Facade Design Pattern.
    *   **Communication**: Orchestrates calls to BLL.

3.  **Business Logic Layer (`CP.FastConsig.BLL`)**
    *   **Responsibility**: Core domain rules, validations, calculations (loan margins, interest rates).
    *   **Structure**: Static helper classes and domain services.
    *   **Communication**: Calls DAL.

4.  **Service Layer (`CP.FastConsig.Services`)**
    *   **Responsibility**: Exposes business functionality via WCF (SOAP) for external integrations or alternative clients.
    *   **Technologies**: WCF (`.svc`).

5.  **Data Access Layer (`CP.FastConsig.DAL`)**
    *   **Responsibility**: CRUD operations, database transactions.
    *   **Technologies**: Entity Framework 4.3.1 (Database First), Repository Pattern.

## Data Architecture

*   **Database**: SQL Server.
*   **Modeling**: EDMX (Entity Data Model XML) in DAL.
*   **Key Entities**: `Averbacao` (Loan), `Funcionario` (Employee), `Empresa` (Consignor/Consignee).
*   **State Management**: Database-driven with heavy reliance on `ASP.NET Session` for user context (`DadosSessao.cs`).

## Integration Architecture

*   **Internal**: Direct assembly references (DLLs).
*   **External**: WCF Services available for third-party integration (e.g., banks sending status updates).

## Security

*   **Authentication**: Custom Forms Authentication.
*   **Authorization**: Role-based access control (`Permissao`, `Perfil`) enforced in BLL and UI.
*   **Audit**: Extensive auditing (`Auditoria`, `UsuarioHistorico`) tracking changes to sensitive entities.
