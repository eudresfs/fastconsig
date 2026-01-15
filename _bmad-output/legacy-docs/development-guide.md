# Development Guide

## Prerequisites

*   **OS**: Windows (Required for .NET Framework 4.0 and Visual Studio)
*   **IDE**: Visual Studio 2010 or newer (2019/2022 recommended with .NET Framework 4.x support)
*   **Framework**: .NET Framework 4.0
*   **Database**: SQL Server (Express or Full)
*   **Third-Party Libraries**:
    *   DevExpress v11.1.8.0 (Required for UI components)
    *   AjaxControlToolkit
    *   ClosedXML
    *   DoddleReport

## Setup Instructions

1.  **Clone the Repository**:
    ```bash
    git clone <repository-url>
    ```

2.  **Database Configuration**:
    *   Ensure SQL Server is running.
    *   Update connection strings in `CP.FastConsig.WebApplication/Conexoes.config`.
    *   Default connection string name: `FastConsigEntities`.

3.  **Third-Party DLLs**:
    *   Ensure the `CP.FastConsig.ThirdParty` folder contains all referenced DLLs, especially DevExpress libraries. If these are missing, the project will not build.

4.  **Open Solution**:
    *   Open `CP.FastConsig.sln` in Visual Studio.

## Build

*   **Command Line**:
    ```bash
    msbuild "CP.FastConsig.sln" /p:Configuration=Debug
    ```
*   **Visual Studio**:
    *   Right-click Solution -> Build Solution.

## Running the Application

1.  Set `CP.FastConsig.WebApplication` as the StartUp Project.
2.  Press **F5** to run in Debug mode.
3.  The application will launch `Login.aspx` by default.

## Testing

*   **Unit Tests**: Located in `CP.FastConsig.Test` and `CP.FastConsig.TestWS`.
*   **Run Tests**:
    *   Visual Studio Test Explorer.
    *   MSTest command line.

## Development Workflow

*   **UI Changes**: Edit `.aspx` or `.ascx` files in `CP.FastConsig.WebApplication`.
*   **Business Logic**: Update `CP.FastConsig.BLL` static classes.
*   **Database Changes**:
    1.  Update SQL Server schema.
    2.  Update `ModeloFastConsig.edmx` in `CP.FastConsig.DAL`.
    3.  Rebuild solution.

## Common Issues

*   **Missing References**: Usually points to missing DevExpress DLLs in the `ThirdParty` folder.
*   **Connection Error**: Verify `Conexoes.config` points to your local SQL instance.
