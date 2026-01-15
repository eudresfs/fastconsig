# Technology Stack & Architecture

## Technology Stack

| Category | Technology | Version | Justification |
|----------|------------|---------|---------------|
| Language | C# | 4.0 | Core backend language |
| Framework | .NET Framework | 4.0 | Legacy target framework |
| Web Framework | ASP.NET WebForms | 4.0 | Presentation layer |
| ORM | Entity Framework | 4.3.1 | Data access (Database First) |
| Database | SQL Server | - | Relational database (inferred) |
| UI Components | DevExpress | 11.1.8.0 | Rich UI controls |
| JavaScript | jQuery | 1.6.3 | Client-side scripting |
| Services | WCF | 4.0 | Service layer communication |
| Reporting | DoddleReport | - | Reporting engine |

## Architecture Pattern

**Layered Architecture (N-Tier) with Facade Pattern**

The solution follows a strict layered approach:

1.  **Presentation Layer (`CP.FastConsig.WebApplication`)**: ASP.NET WebForms application handling UI and user interaction.
2.  **Service Layer (`CP.FastConsig.Services`)**: WCF Services exposing business logic to external consumers or internal components.
3.  **Facade Layer (`CP.FastConsig.Facade`)**: Simplifies access to the Business Logic Layer, acting as an intermediary for the Presentation layer.
4.  **Business Logic Layer (`CP.FastConsig.BLL`)**: Contains the core business rules and domain logic.
5.  **Data Access Layer (`CP.FastConsig.DAL`)**: Handles database interactions using Entity Framework.
6.  **Cross-Cutting Concerns**:
    *   `CP.FastConsig.Common`: Shared enums and constants.
    *   `CP.FastConsig.Util`: Utility classes and helpers.
