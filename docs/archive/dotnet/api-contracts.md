# API Contracts & Service Definition

## Overview

The application utilizes **WCF (Windows Communication Foundation)** for its service layer, specifically defined in the `CP.FastConsig.Services` project. The primary service contract is `IServicoUsuario`.

## Service Contracts

### IServicoUsuario

**Namespace:** `CP.FastConsig.Services`
**Implementation:** `ServiceUsuario.svc`

This service handles user authentication, management, and retrieval of consignor/consignee data.

| Operation | Input Parameters | Return Type | Description |
|-----------|------------------|-------------|-------------|
| `GetData` | `int value` | `string` | Test operation. |
| `GetDataUsingDataContract` | `CompositeType composite` | `CompositeType` | Test operation using data contract. |
| `ListarConsignatarias` | None | `List<Consignataria>` | Lists all available consignees (financial institutions). |
| `ListarConsignantes` | None | `List<Consignante>` | Lists all available consignors (employers). |
| `ExisteUsuario` | `string CPF` | `bool` | Checks if a user exists by CPF. |
| `ExcluirUsuarioPorCpf` | `string cpf`, `int? idCriador`, `string tipoCriador` | `bool` | Deletes a user by CPF. |
| `ObtemUsuarioPorLogin` | `string login` | `Usuario` | Retrieves user details by login username. |
| `ObtemUsuarioPorCpf` | `string cpf` | `Usuario` | Retrieves user details by CPF. |
| `AtualizaUsuario` | `string cpf`, `string nome`, `string email`, `string telefone`, `string login`, `string senha`, `int idConsignataria`, `string novoCpf`, `int idConsignante`, `int? idCriador`, `string tipoCriador` | `void` | Updates user information including associated entities. |
| `ObtemConsignante` | `int idConsignante` | `Consignante` | Retrieves specific consignor details. |
| `ObtemConsignataria` | `int idConsignataria` | `Consignataria` | Retrieves specific consignee details. |
| `IncluirUsuario` | `int idConsignataria`, `int idConsignante`, `string cpf`, `string nome`, `string email`, `string fone`, `string login`, `string senha`, `int? idCriador`, `string tipoCriador` | `bool` | Creates a new user. |
| `AlterarUsuario` | `string cpf`, `string nome`, `string email`, `string fone`, `string login`, `string senha`, `string novoCpf`, `int? idCriador`, `string tipoCriador` | `bool` | Modifies existing user basic data. |
| `ConsignantesDoUsuario` | `string cpf` | `List<Consignante>` | Lists consignors linked to a user. |
| `ConsignantesDaConsignataria` | `int idConsignataria` | `List<Consignante>` | Lists consignors associated with a specific consignee. |
| `ConsignatariaDoUsuario` | `string CPF` | `Consignataria` | Retrieves the consignee linked to a user. |
| `AlterarSenhaUsuario` | `string cpf`, `string novaSenha`, `int? idCriador`, `string tipoCriador` | `bool` | Changes user password. |

## Data Types

### CompositeType
Used for testing data contract serialization.
- `bool BoolValue`
- `string StringValue`

### Domain Entities (Shared)
The service returns domain entities defined in `CP.FastConsig.DAL`:
- `Usuario`
- `Consignante`
- `Consignataria`

## Integration Patterns

- **WCF SOAP/HTTP**: The services are exposed via `BasicHttpBinding`, likely for SOAP interoperability.
- **Endpoint**: `http://localhost:9998/ServiceUsuario.svc` (Default dev configuration)
