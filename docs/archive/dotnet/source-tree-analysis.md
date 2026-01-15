# Source Tree Analysis

## Directory Structure

```
app .NET/
├── CP.FastConsig.sln              # Visual Studio Solution File
├── CP.FastConsig.WebApplication/  # [Presentation] ASP.NET WebForms Application
│   ├── Default.aspx               # Main entry page (after login)
│   ├── Login.aspx                 # Authentication entry point
│   ├── Web.config                 # Application configuration
│   ├── WebUserControls/           # Reusable UI Components (.ascx)
│   ├── App_Themes/                # Skin and CSS themes
│   ├── Imagens/                   # Application assets
│   ├── Scripts/                   # JavaScript files (jQuery, custom)
│   └── Service References/        # WCF Client Proxies
├── CP.FastConsig.Services/        # [Service] WCF Service Layer
│   ├── ServiceUsuario.svc         # Core User Service
│   └── IServicoUsuario.cs         # Service Contract
├── CP.FastConsig.BLL/             # [Business] Business Logic Layer
│   ├── Averbacoes.cs              # Loan logic
│   ├── Funcionarios.cs            # Employee logic
│   └── ... (Domain Logic)
├── CP.FastConsig.DAL/             # [Data] Data Access Layer
│   ├── ModeloFastConsig.edmx      # Entity Framework Model
│   ├── Repositorio.cs             # Generic Repository Pattern
│   └── ... (Entity Classes)
├── CP.FastConsig.Facade/          # [Facade] Facade Layer
│   ├── FachadaAverbacao.cs        # Simplified interface for UI
│   └── ...
├── CP.FastConsig.Common/          # [Shared] Constants and Enums
├── CP.FastConsig.Util/            # [Shared] Utilities (Session, String, Security)
├── CP.FastConsig.ThirdParty/      # [Deps] External DLLs (DevExpress, etc.)
└── packages/                      # NuGet packages
```

## Critical Folders

### WebApplication (Presentation)
*   **WebUserControls/**: Contains the bulk of the application's UI logic. Complex forms like Loan Entry (`WebUserControlAverbacao.ascx`) reside here.
*   **Scripts/**: Client-side logic including Highcharts and custom validation.

### Services (API)
*   **ServiceUsuario.svc**: The main gateway for external or decoupled internal calls.

### BLL (Business Logic)
*   Contains the core business rules. `Averbacoes.cs` is likely the most critical class given the project name.

### DAL (Data)
*   **EFExtensions/**: Custom extensions for Entity Framework.
*   **ModeloCustom/**: DTOs or custom query result classes.

## Entry Points

1.  **Web UI**: `Login.aspx` -> `Default.aspx`.
2.  **API**: `ServiceUsuario.svc` (WCF).
