# 07 · Database and Entity Framework

## Overview

The project uses Entity Framework Core with SQL Server.

The persistence model is intentionally small and currently centers on a single entity: `Character`.

## Entity model

### `Character`

| Property | Type | Purpose |
|---|---|---|
| `Id` | `int` | primary key |
| `Name` | `string` | character name |
| `Game` | `string` | game title |
| `Role` | `string` | character role |

## Database context

`CharacterDbContext` is the EF Core context used by the application.

### Current responsibilities

| Responsibility | Present |
|---|---|
| expose `DbSet<Character>` | Yes |
| connect to SQL Server | Yes |
| configure model rules | Yes |
| configure index on `Game` | Yes |

### Current DbSet

| Member | Type |
|---|---|
| `Characters` | `DbSet<Character>` |

## Current schema evolution

The codebase currently includes two migrations.

| Migration | Purpose |
|---|---|
| `Initial` | creates the `Characters` table |
| `AddGameIndex` | creates an index on `Game` and adjusts that column for indexing |

## Current table structure

Based on the existing migrations, the `Characters` table currently has:

| Column | SQL type | Notes |
|---|---|---|
| `Id` | `int` | identity primary key |
| `Name` | `nvarchar(max)` | required |
| `Game` | `nvarchar(450)` | required; changed for indexing |
| `Role` | `nvarchar(max)` | required |

## Indexing

The current model configures an index on:

| Table | Column | Reason |
|---|---|---|
| `Characters` | `Game` | supports filtering and ordering scenarios involving game title |

The index is defined in `OnModelCreating(...)` and represented in the `AddGameIndex` migration.

## Connection configuration

The application reads its database connection from:

| Setting | Source |
|---|---|
| `ConnectionStrings:DefaultConnection` | `appsettings.json` or environment variables |

### Current local-development example

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLExpress;Database=VideoCharactersDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### Current Docker Compose override

In Docker, the API container receives:

```text
ConnectionStrings__DefaultConnection=Server=db;Database=VideoGameCharactersDb;User Id=sa;Password=<...>;TrustServerCertificate=True
```

## Runtime migration behavior

`Program.cs` currently applies pending migrations at startup:

```text
dbContext.Database.Migrate()
```

This means the application attempts to bring the database schema up to date automatically when it launches.

## Data flow through EF Core

```text
Request DTO
   ↓
service method
   ↓
Character entity
   ↓
CharacterDbContext
   ↓
SQL Server
```

For read operations, the service typically projects directly to `CharacterResponseDto` instead of returning entities.

## Current persistence characteristics

| Characteristic | Current status |
|---|---|
| single table | Yes |
| foreign keys | No |
| navigation properties | No |
| seeding code | No explicit EF Core seeding in the codebase |
| startup migration execution | Yes |
| SQL Server provider | Yes |

## Relationship to validation

There is an important distinction between:

| Level | Current rule source |
|---|---|
| API request validation | DTO data annotations |
| database schema shape | EF migrations / model configuration |

At present, the DTOs apply maximum lengths for `Name` and `Game`, but the existing migration history does not mirror all of those limits in the database schema.

## Related documents

| Document | Topic |
|---|---|
| `02-architecture.md` | where `CharacterDbContext` sits in the overall structure |
| `06-querying-pagination-and-sorting.md` | how EF queries are shaped in the service |
