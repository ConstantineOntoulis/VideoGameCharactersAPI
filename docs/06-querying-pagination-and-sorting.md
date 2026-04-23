# 06 · Querying, Pagination, and Sorting

## Overview

The collection endpoint `GET /api/VideoGameCharacters` supports optional filtering, sorting, and pagination.

The logic is implemented in `VideoGameService.GetAllCharactersAsync(...)`.

## Supported query parameters

| Parameter | Type | Default | Current behavior |
|---|---|---|---|
| `Page` | integer | `1` | normalized through `QueryRules.NormalizePage` |
| `PageSize` | integer | `10` | normalized through `QueryRules.NormalizePageSize` |
| `Game` | string | none | optional equality filter |
| `Role` | string | none | optional equality filter |
| `SortBy` | string | none | supports `name`, `game`, or `role` |
| `SortDirection` | string | `asc` | `desc` triggers descending order |

## Filtering

### Current filters

| Field | How it is applied |
|---|---|
| `Game` | `c.Game == query.Game` |
| `Role` | `c.Role == query.Role` |

The service only applies a filter when the incoming value is not null, empty, or whitespace.

### Example filter request

```http
GET /api/VideoGameCharacters?Game=Final%20Fantasy%20VII&Role=Hero
Authorization: Bearer <jwt-token>
```

## Sorting

The service uses a switch expression based on:

- `SortBy?.ToLower()`
- `SortDirection?.ToLower()`

### Supported fields

| `SortBy` value | Supported |
|---|---|
| `name` | Yes |
| `game` | Yes |
| `role` | Yes |

### Direction behavior

| `SortDirection` value | Result |
|---|---|
| `desc` | descending order |
| any other value | ascending order |

### Fallback behavior

If `SortBy` is missing or unsupported, the service falls back to:

```text
Order by Id ascending
```

## Pagination

Pagination is applied after filtering and sorting.

### Current helper rules

`QueryRules` contains two normalization methods.

| Method | Behavior |
|---|---|
| `NormalizePage(int page)` | values below `1` become `1` |
| `NormalizePageSize(int pageSize)` | values below `1` become `10`; values above `50` become `50` |

### Effective page-size rules

| Requested value | Effective result |
|---|---|
| `0` | `10` |
| `-1` | `10` |
| `5` | `5` |
| `50` | `50` |
| `999` | `50` |

### EF Core paging mechanics

After normalization, the query uses:

- `Skip((page - 1) * pageSize)`
- `Take(pageSize)`

## Response shape

The collection endpoint returns `PagedResponseDto<CharacterResponseDto>`.

### Current response contract

| Property | Meaning |
|---|---|
| `Page` | effective page number used |
| `PageSize` | effective page size used |
| `TotalCount` | total number of rows matching filters before paging |
| `Items` | items for the current page |

### Example response

```json
{
  "page": 2,
  "pageSize": 5,
  "totalCount": 17,
  "items": [
    {
      "id": 6,
      "name": "Tifa",
      "game": "Final Fantasy VII",
      "role": "Heroine"
    }
  ]
}
```

## Query execution pattern

The current service builds the query in stages:

```text
Characters DbSet
   ↓
AsNoTracking()
   ↓
optional filters
   ↓
sorting
   ↓
count query
   ↓
Skip / Take
   ↓
DTO projection
   ↓
materialization with ToListAsync()
```

## Performance-aware choices currently present

| Choice | Purpose |
|---|---|
| `AsNoTracking()` | avoids change tracking for read-only list queries |
| projection to DTOs | avoids returning entity objects directly |
| count before paging | gives total match count to the client |
| index on `Game` | supports one of the available filters |

## Current scope boundary

The query layer currently does **not** include:

- partial-text search
- multi-field sorting
- validation attributes on query parameters
- custom query operators
- complex specification patterns

That is appropriate for the present scope of the project.

## Related documents

| Document | Topic |
|---|---|
| `03-api-reference.md` | endpoint usage and route details |
| `07-database-and-entity-framework.md` | EF Core and indexing |
| `08-testing.md` | current query rule tests |
