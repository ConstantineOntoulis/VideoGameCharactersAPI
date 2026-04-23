# 02 · Architecture

## Overview

The solution is organized as a layered ASP.NET Core Web API with a small and explicit structure.

The main execution path is:

```text
HTTP Request
   ↓
Controller
   ↓
Service
   ↓
CharacterDbContext
   ↓
SQL Server
```

## Solution layout

| Path | Responsibility |
|---|---|
| `VideoGameCharactersAPI/Controllers` | HTTP endpoints and HTTP-level response behavior |
| `VideoGameCharactersAPI/Dtos` | request and response contracts |
| `VideoGameCharactersAPI/Services` | application logic and query shaping |
| `VideoGameCharactersAPI/Data` | EF Core database context |
| `VideoGameCharactersAPI/Models` | persistence entity classes |
| `VideoGameCharactersAPI/Infrastructure` | cross-cutting concerns such as exception handling |
| `VideoGameCharactersAPI/Migrations` | EF Core migration history |
| `VideoGameCharactersAPI.Tests` | automated test project |
| `Database` | backup and restore assets used by the Docker workflow |

## Layer responsibilities

### Controllers

Controllers expose the API surface and translate HTTP requests into service calls.

| Controller | Responsibility |
|---|---|
| `AuthController` | authenticates demo users and issues JWT tokens |
| `VideoGameCharactersController` | exposes protected CRUD-style endpoints for characters |

Key controller characteristics in the current codebase:

- `[ApiController]` is used
- route definitions are explicit
- authorization policies are applied at action level
- 404 responses are returned through `Problem(...)` when an entity is missing

### DTOs

DTOs define the shape of the external API contract.

| DTO | Role |
|---|---|
| `CreateCharacterRequest` | request body for character creation |
| `UpdateCharacterRequest` | request body for character update |
| `CharacterResponseDto` | outgoing shape for a single character |
| `PagedResponseDto<T>` | outgoing shape for paged collection results |
| `GetCharactersQuery` | query parameters for list retrieval |
| `LoginRequest` | request body for login |
| `LoginResponse` | response body returned after successful login |

### Services

The service layer contains the application logic behind character endpoints.

| Service component | Responsibility |
|---|---|
| `IVideoGameCharacterService` | service contract used by the controller |
| `VideoGameService` | current implementation for CRUD and query behavior |
| `QueryRules` | helper rules for normalizing page and page-size values |

The service layer currently handles:

- entity creation and update
- query composition
- filtering by `Game` and `Role`
- sorting by `Name`, `Game`, or `Role`
- pagination logic
- entity-to-DTO projection
- logging of create, update, delete, and failed lookup operations

### Data layer

`CharacterDbContext` is the EF Core database context used to access the `Characters` table.

It exposes:

| Member | Purpose |
|---|---|
| `DbSet<Character> Characters` | root query/update surface for character entities |

In `OnModelCreating`, the current code configures an index on `Game`.

### Infrastructure

The `Infrastructure` folder currently contains `GlobalExceptionHandler`.

Its role is to:

- intercept unhandled exceptions
- log the exception details
- return a standardized `ProblemDetails`-style `500` response
- attach a `traceId` so the failure can be correlated with logs

## Startup composition

The application startup is defined in `Program.cs`.

### Registered services

| Registration | Purpose |
|---|---|
| `AddControllers()` | enables MVC controller support |
| `AddProblemDetails()` | enables standard problem details services |
| `AddExceptionHandler<GlobalExceptionHandler>()` | registers centralized exception handling |
| `AddOpenApi()` | exposes OpenAPI metadata |
| `AddHealthChecks()` | enables `/health` |
| `AddDbContext<CharacterDbContext>()` | configures EF Core with SQL Server |
| `AddScoped<IVideoGameCharacterService, VideoGameService>()` | wires controller to service implementation |
| `AddAuthentication().AddJwtBearer(...)` | enables JWT validation |
| `AddAuthorization(...)` | registers `UserOrAdmin` and `AdminOnly` policies |

### Policies

| Policy | Rule |
|---|---|
| `UserOrAdmin` | requires role `User` or `Admin` |
| `AdminOnly` | requires role `Admin` |

## Request pipeline

The current request pipeline in `Program.cs` is ordered as follows:

1. development-only OpenAPI and Scalar mapping
2. HTTPS redirection outside development
3. root redirect to `/scalar`
4. health-check endpoint mapping
5. exception handling
6. authentication
7. authorization
8. controller mapping

## Request flow by example

### Example: `POST /api/VideoGameCharacters`

```text
Client sends authenticated POST request
   ↓
`VideoGameCharactersController.AddCharacter`
   ↓
authorization policy `AdminOnly` is enforced
   ↓
request DTO is validated by `[ApiController]`
   ↓
`IVideoGameCharacterService.AddCharacterAsync(...)`
   ↓
new `Character` entity is added through `CharacterDbContext`
   ↓
changes are saved to SQL Server
   ↓
controller returns `201 Created`
```

### Example: `GET /api/VideoGameCharacters`

```text
Client sends authenticated GET request with optional query parameters
   ↓
`VideoGameCharactersController.GetCharacters`
   ↓
service composes EF Core query
   ↓
optional filtering, sorting, and paging are applied
   ↓
results are projected to `CharacterResponseDto`
   ↓
controller returns `200 OK` with `PagedResponseDto<CharacterResponseDto>`
```

## Design characteristics of the current code

| Characteristic | Current status |
|---|---|
| Layered separation | present |
| Service abstraction | present |
| Repository pattern | not used |
| Single-entity persistence model | present |
| Query projection | present |
| Global exception handling | present |
| Demo authentication approach | present |
| Complex domain model | intentionally absent |

## Document boundary

This file explains **how the project is structured**.

It does not repeat:

- endpoint details from `03-api-reference.md`
- auth usage guidance from `04-authentication-and-authorization.md`
- validation/status-code specifics from `05-validation-and-error-handling.md`
