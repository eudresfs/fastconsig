# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

FastConsig is a Brazilian payroll loan management system (sistema de consignados) built with ASP.NET WebForms and .NET Framework 4.0. The solution is located in `app .NET/CP.FastConsig.sln` and was originally developed for Visual Studio 2010.

## Build Commands

```bash
# Build the entire solution
msbuild "app .NET/CP.FastConsig.sln" /p:Configuration=Debug

# Build specific project
msbuild "app .NET/CP.FastConsig.WebApplication/CP.FastConsig.WebApplication.csproj"

# Restore NuGet packages (if needed)
nuget restore "app .NET/CP.FastConsig.sln"
```

## Architecture

The solution follows a layered architecture with Facade pattern:

### Layer Structure (dependency flow: top to bottom)

1. **CP.FastConsig.WebApplication** - ASP.NET WebForms presentation layer
   - ASPX pages with code-behind
   - Master pages (NewMasterPrincipal.Master)
   - WCF service endpoints (.svc files)

2. **CP.FastConsig.Facade** - Facade layer that simplifies BLL access
   - Each `Fachada*.cs` file corresponds to a specific feature/page
   - Coordinates between presentation and business logic

3. **CP.FastConsig.BLL** - Business Logic Layer
   - Static classes with business rules (Averbacoes.cs, Funcionarios.cs, etc.)
   - Domain operations for loans, employees, companies

4. **CP.FastConsig.DAL** - Data Access Layer
   - Generic Repository pattern (`Repositorio<T>`)
   - Entity Framework with EDMX model (ModeloFastConsig.edmx)
   - Entity classes for all domain objects

5. **CP.FastConsig.Common** - Shared enumerations and constants
   - `Enums.cs` defines all system enums (AverbacaoSituacao, Modulos, Permissao, etc.)

6. **CP.FastConsig.Util** - Utility classes
   - StringHelper, Seguranca, DadosSessao

7. **CP.FastConsig.Services** - WCF Services project with Center database model

### Supporting Projects

- **CP.FastConsig.Source** - Static resources, scripts, webcam components
- **CP.FastConsig.ThirdParty** - Third-party libraries
- **CP.FastConsig.Test** - Unit tests (MSTest)
- **CP.FastConsig.DB.Main** / **CP.FastConsig.DB.Center** - RedGate SQL Source Control projects

## Key Domain Concepts

- **Averbacao** (Loan endorsement) - Core entity representing payroll-deducted loans
- **Funcionario** (Employee) - Employees who can take loans
- **Consignante** (Consignor) - Employer entities that process payroll deductions
- **Consignataria** (Consignee) - Financial institutions that provide loans
- **Margem** (Margin) - Available payroll deduction capacity
- **Parcela** (Installment) - Loan payment installments

## Database

- Uses SQL Server with Entity Framework (Database First approach)
- Two databases: Main (FastConsig) and Center
- Connection strings configured in `Conexoes.config`
- Session-based connection string selection (`NomeStringConexao`)

## Coding Conventions

- Portuguese naming for domain entities and business methods
- Static BLL classes with repository instantiation per method
- Facade methods delegate directly to BLL methods
- No emojis or signatures in commits
