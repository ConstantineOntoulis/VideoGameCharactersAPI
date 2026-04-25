<div align="center">

# VideoGameCharactersAPI

**A structured ASP.NET Core Web API practice project for consolidating intermediate backend concepts**

![.NET 10](https://img.shields.io/badge/.NET-10-2E5E5B?style=for-the-badge)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-B68A3A?style=for-the-badge)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-6E3942?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-1E1A18?style=for-the-badge)
![JWT](https://img.shields.io/badge/JWT-Auth-2E5E5B?style=for-the-badge)
![Docker](https://img.shields.io/badge/Docker-Containerized-B68A3A?style=for-the-badge)
![xUnit](https://img.shields.io/badge/xUnit-Tests-6E3942?style=for-the-badge)
![Scalar](https://img.shields.io/badge/Scalar-API%20UI-1E1A18?style=for-the-badge)
![CI](https://img.shields.io/github/actions/workflow/status/ConstantineOntoulis/VideoGameCharactersAPI/CI.yml?branch=master&style=for-the-badge&label=CI)

</div>

---

## Overview

`VideoGameCharactersAPI` is a backend practice project built to strengthen the transition from foundational C# and introductory CRUD work into more deliberate ASP.NET Core API design.

Its purpose is not to simulate a large production platform, but to demonstrate disciplined practice in areas that matter for junior backend preparation:

* layered API structure
* DTO-based request and response contracts
* input validation
* JWT authentication and role-based authorization
* filtering, sorting, and pagination
* centralized exception handling with `ProblemDetails`
* EF Core with SQL Server and migrations
* Docker-based execution
* automated tests

---

## Project Positioning

| Category          | Description                                                                                    |
| ----------------- | ---------------------------------------------------------------------------------------------- |
| Project type      | Intermediate backend practice / hardening project                                              |
| Primary goal      | Consolidate backend concepts in one coherent ASP.NET Core API                                  |
| Domain choice     | Intentionally simple, so the learning focus remains on architecture and API behavior           |
| Intended audience | Recruiters, interviewers, and reviewers evaluating learning progression and backend discipline |
| Scope boundary    | Not presented as a full production platform; presented as structured technical practice        |

---

## What This Project Demonstrates

| Area              | Included in this project                                                                            |
| ----------------- | --------------------------------------------------------------------------------------------------- |
| API design        | REST-style controller endpoints for character resources                                             |
| Validation        | Data-annotation validation on request DTOs                                                          |
| Authentication    | JWT-based login flow                                                                                |
| Authorization     | Policy-based endpoint protection for `User` and `Admin` roles                                       |
| Error handling    | Centralized unhandled-exception handling with `ProblemDetails`                                      |
| Data access       | EF Core + SQL Server                                                                                |
| Querying          | Filtering, sorting, and pagination for list retrieval                                               |
| Delivery          | Dockerfile and Docker Compose setup                                                                 |
| Health monitoring | `/health` endpoint                                                                                  |
| API exploration   | OpenAPI + Scalar UI in development                                                                  |
| Testing           | xUnit-based tests for authentication, authorization, validation, query rules, and endpoint behavior |
| CI validation     | GitHub Actions workflow for restore, build, and test execution                                      |

---

## Architecture at a Glance

| Layer          | Responsibility                                                       |
| -------------- | -------------------------------------------------------------------- |
| Controllers    | Handle HTTP requests and return HTTP responses                       |
| DTOs           | Define request and response contracts exposed by the API             |
| Services       | Contain application logic and coordinate data operations             |
| DbContext      | Connect EF Core to SQL Server and manage persistence                 |
| Infrastructure | Centralized cross-cutting behavior such as global exception handling |

### Request flow

```text
HTTP Request
   ↓
Controller
   ↓
Service
   ↓
Entity Framework Core / DbContext
   ↓
SQL Server
```

---

## Technology Stack

| Category             | Tools / Technologies           |
| -------------------- | ------------------------------ |
| Backend framework    | ASP.NET Core Web API (.NET 10) |
| Language             | C#                             |
| ORM                  | Entity Framework Core          |
| Database             | SQL Server                     |
| Authentication       | JWT Bearer authentication      |
| Authorization        | Role-based policies            |
| API documentation UI | Scalar                         |
| API description      | OpenAPI                        |
| Containerization     | Docker, Docker Compose         |
| Testing              | xUnit                          |

---

## API Surface Summary

| Endpoint group            | Route pattern                          | Access            |
| ------------------------- | -------------------------------------- | ----------------- |
| Authentication            | `/api/Auth/login`                      | Anonymous         |
| Character listing         | `GET /api/VideoGameCharacters`         | `User` or `Admin` |
| Character retrieval by id | `GET /api/VideoGameCharacters/{id}`    | `User` or `Admin` |
| Character creation        | `POST /api/VideoGameCharacters`        | `Admin` only      |
| Character update          | `PUT /api/VideoGameCharacters/{id}`    | `Admin` only      |
| Character deletion        | `DELETE /api/VideoGameCharacters/{id}` | `Admin` only      |
| Health check              | `/health`                              | Public            |

---

## Authentication Model

This API uses a simplified JWT-based authentication flow suitable for a practice project.

### Demo credentials

| Role  | Username | Password   |
| ----- | -------- | ---------- |
| User  | `user`   | `user123`  |
| Admin | `admin`  | `admin123` |

### Authorization policies

| Policy        | Allowed roles   |
| ------------- | --------------- |
| `UserOrAdmin` | `User`, `Admin` |
| `AdminOnly`   | `Admin`         |

> The authentication model is intentionally simplified for educational purposes. It is designed to demonstrate JWT issuance and policy-based authorization rather than full account management.

---

## Key Functional Features

### Character retrieval

* retrieve all characters through a protected endpoint
* retrieve a single character by id
* return paged results for collection queries

### Query support

* filter by `Game`
* filter by `Role`
* sort by `Name`, `Game`, or `Role`
* choose ascending or descending order
* normalize page and page-size values before query execution

### Write operations

* create new characters
* update existing characters
* delete characters
* restrict write operations to the `Admin` role

### Error behavior

* automatic `400 Bad Request` responses for invalid DTO input
* `401 Unauthorized` when authentication is missing or invalid
* `403 Forbidden` when the authenticated role lacks permission
* `404 Not Found` when a requested character does not exist
* centralized `500 Internal Server Error` responses for unhandled exceptions

---

## Local Setup

### Prerequisites

| Requirement    | Notes                                               |
| -------------- | --------------------------------------------------- |
| .NET SDK       | .NET 10                                             |
| SQL Server     | Local SQL Server instance or compatible environment |
| Optional tools | Visual Studio / VS Code, Postman, Docker Desktop    |

### 1. Configure the database connection

Update the connection string in `VideoGameCharactersAPI/appsettings.json` if needed.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLExpress;Database=VideoCharactersDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 2. Run the API

```bash
dotnet restore
dotnet run --project VideoGameCharactersAPI
```

### 3. Open the API UI

The address depends on how the application is being run.

| Run mode                         | Scalar URL                        |
| -------------------------------- | --------------------------------- |
| Local development (`dotnet run`) | `https://localhost:<port>/scalar` |
| Docker Compose                   | `http://localhost:8080/scalar`    |

### Notes

* the application applies pending migrations on startup
* when running through Docker Compose, the API is exposed on port `8080`
* the root route redirects to the Scalar API UI in the current project setup
* the health endpoint is available at `/health`

---

## Docker Setup

This repository also includes a Docker-based setup with:

* a SQL Server container
* a restore step for the included database backup
* an API container

### Environment file

Create a `.env` file based on `.env.example`.

```bash
cp .env.example .env
```

### Start the containers

```bash
docker compose up --build
```

### Default container access

| Service    | Address                 |
| ---------- | ----------------------- |
| API        | `http://localhost:8080` |
| SQL Server | `localhost:14333`       |

---

## Testing

The solution includes automated tests covering several important behaviors.

| Test area              | Purpose                                                 |
| ---------------------- | ------------------------------------------------------- |
| Authentication tests   | verify login success and token issuance                 |
| Authorization tests    | verify protected endpoints reject missing tokens        |
| Forbidden tests        | verify insufficient roles cannot access admin endpoints |
| Validation tests       | verify invalid request bodies return `400 Bad Request`  |
| Success endpoint tests | verify valid authenticated requests succeed             |
| Query rules tests      | verify pagination normalization logic                   |
| Smoke tests            | verify the application can boot and respond             |

### Run tests

```bash
dotnet test
```
```md
### Continuous integration

The repository includes a GitHub Actions workflow at:

```text
.github/workflows/CI.yml

---

## Project Structure

```text
VideoGameCharactersAPI-master/
├── Database/
│   ├── VideoGameCharactersDb.bak
│   └── restore-db.sh
├── VideoGameCharactersAPI/
│   ├── Controllers/
│   ├── Data/
│   ├── Dtos/
│   ├── Infrastructure/
│   ├── Migrations/
│   ├── Models/
│   ├── Services/
│   ├── Dockerfile
│   ├── Program.cs
│   └── appsettings.json
├── VideoGameCharactersAPI.Tests/
│   ├── AuthenticationTests.cs
│   ├── AuthorizationTests.cs
│   ├── ForbiddenTests.cs
│   ├── QueryRulesTests.cs
│   ├── SmokeTests.cs
│   ├── SuccessEndpointTests.cs
│   └── ValidationTests.cs
└── docker-compose.yml
```

---

## Documentation Map

This README is intentionally limited to the project overview, setup, scope, and navigation layer.

The detailed technical documentation is intended to be separated into focused companion documents so that the documentation suite remains organized and avoids repetition.

| Planned document                              | Focus                                               |
| --------------------------------------------- | --------------------------------------------------- |
| `docs/01-project-overview.md`                 | purpose, scope, and learning goals                  |
| `docs/02-architecture.md`                     | internal structure and request flow                 |
| `docs/03-api-reference.md`                    | endpoint-by-endpoint reference                      |
| `docs/04-authentication-and-authorization.md` | login flow, token use, and policies                 |
| `docs/05-validation-and-error-handling.md`    | DTO validation, status codes, and `ProblemDetails`  |
| `docs/06-querying-pagination-and-sorting.md`  | filters, ordering, and paging behavior              |
| `docs/07-database-and-entity-framework.md`    | entity model, `DbContext`, migrations, and indexing |
| `docs/08-testing.md`                          | test coverage and execution                         |
| `docs/09-docker-and-local-setup.md`           | environment setup and container workflow            |
