# Deployment Guide

## Architecture

*   **Web Server**: IIS (Internet Information Services) 7.0+
*   **Database Server**: SQL Server 2008 R2 or newer
*   **Runtime**: .NET Framework 4.0 Full

## Publishing

1.  **Build Release**:
    ```bash
    msbuild "CP.FastConsig.sln" /p:Configuration=Release
    ```

2.  **Web Deploy (Visual Studio)**:
    *   Right-click `CP.FastConsig.WebApplication`.
    *   Select **Publish**.
    *   Target: File System or IIS Web Deploy.

## Configuration

### Web.config / Conexoes.config
Ensure the connection string in the production environment points to the production database server.

```xml
<connectionStrings configSource="Conexoes.config" />
```

### IIS Settings
*   **App Pool**: .NET 4.0 Integrated Mode.
*   **Authentication**: Anonymous Authentication enabled (Forms Auth handled by app).
*   **Permissions**: Ensure the IIS User has Write permissions to any folders used for file uploads or temporary storage (e.g., `Temp/`, `Arquivos/`).

## WCF Services
If `CP.FastConsig.Services` is deployed separately:
*   Ensure it runs in an App Pool compatible with .NET 4.0.
*   Update `CP.FastConsig.WebApplication` to point to the correct Service URL.

## Database Deployment
*   Use RedGate SQL Source Control projects (`CP.FastConsig.DB.Main`, `CP.FastConsig.DB.Center`) to apply schema changes to the production database.
