# 09 · Docker and Local Setup

## Overview

The project can be run in two main ways:

| Mode | Typical use |
|---|---|
| local .NET execution | direct development and debugging |
| Docker Compose | containerized execution with SQL Server and restore workflow |

## Prerequisites

| Requirement | Notes |
|---|---|
| .NET SDK 10 | required for local execution |
| SQL Server | required for local non-Docker execution |
| Docker Desktop or compatible runtime | required for container workflow |

## Local setup

### 1. Check the connection string

The current local configuration lives in:

```text
VideoGameCharactersAPI/appsettings.json
```

Current example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLExpress;Database=VideoCharactersDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

Adjust this if your local SQL Server instance differs.

### 2. Run the application

```bash
dotnet restore
dotnet run --project VideoGameCharactersAPI
```

### 3. Development URLs

The current `launchSettings.json` includes:

| Profile | URLs |
|---|---|
| `http` | `http://localhost:5021` |
| `https` | `https://localhost:7158` and `http://localhost:5021` |

### 4. Open the API UI

When the application is running in development:

| UI | URL pattern |
|---|---|
| Scalar | `https://localhost:<port>/scalar` |

## Docker Compose setup

### Compose services

The current `docker-compose.yml` defines three services.

| Service | Purpose |
|---|---|
| `db` | SQL Server container |
| `db-restore` | restore workflow for the included `.bak` file |
| `api` | ASP.NET Core API container |

### Compose flow

```text
db starts
   ↓
db becomes healthy
   ↓
db-restore runs restore-db.sh
   ↓
api container starts
```

## Environment file

Create a `.env` file from `.env.example`.

Example values expected by the project:

| Variable | Purpose |
|---|---|
| `MSSQL_SA_PASSWORD` | SQL Server SA password |
| `JWT_KEY` | JWT signing key |
| `JWT_ISSUER` | JWT issuer |
| `JWT_AUDIENCE` | JWT audience |
| `ASPNETCORE_ENVIRONMENT` | runtime environment |

### Example preparation

```bash
cp .env.example .env
```

## Start the containerized stack

```bash
docker compose up --build
```

## Container access points

| Service | Address |
|---|---|
| API | `http://localhost:8080` |
| Scalar in Docker | `http://localhost:8080/scalar` |
| SQL Server | `localhost:14333` |

## Docker-related configuration details

### API container

The API container receives:

| Setting | Current value pattern |
|---|---|
| port binding | `8080:8080` |
| connection string override | `Server=db;Database=VideoGameCharactersDb;...` |
| JWT config | passed through environment variables |

### Database container

The SQL Server container:

- uses image `mcr.microsoft.com/mssql/server:2025-latest`
- mounts `./Database` as a read-only backup directory
- stores database data in the `sql_data` volume
- exposes host port `14333`

### Restore container

The restore step runs:

```text
/var/opt/mssql/backup/restore-db.sh
```

Its purpose is to restore the included database backup before the API starts.

## Health endpoint

The project maps:

```text
/health
```

The Dockerfile also defines a container health check using:

```text
http://localhost:8080/health
```

## Accuracy note about URLs

There are two different Scalar access patterns depending on run mode:

| Run mode | Scalar URL |
|---|---|
| local development | `https://localhost:<port>/scalar` |
| Docker Compose | `http://localhost:8080/scalar` |

## Related documents

| Document | Topic |
|---|---|
| `03-api-reference.md` | endpoint surface |
| `07-database-and-entity-framework.md` | connection and migration behavior |
| `10-known-limitations-and-next-steps.md` | setup-related caveats |
