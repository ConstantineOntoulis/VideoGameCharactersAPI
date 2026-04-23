# 03 · API Reference

## Base notes

This document describes the current HTTP API surface exposed by the project.

### Common characteristics

| Item | Current behavior |
|---|---|
| API style | controller-based ASP.NET Core Web API |
| Authentication mechanism | JWT Bearer token |
| Response format | JSON |
| Public endpoint | `/health` |
| Interactive UI in development | `/scalar` |

## Endpoint index

| Group | Method | Route | Access |
|---|---|---|---|
| Authentication | `POST` | `/api/Auth/login` | Anonymous |
| Characters | `GET` | `/api/VideoGameCharacters` | `User` or `Admin` |
| Characters | `GET` | `/api/VideoGameCharacters/{id}` | `User` or `Admin` |
| Characters | `POST` | `/api/VideoGameCharacters` | `Admin` only |
| Characters | `PUT` | `/api/VideoGameCharacters/{id}` | `Admin` only |
| Characters | `DELETE` | `/api/VideoGameCharacters/{id}` | `Admin` only |
| Health | `GET` | `/health` | Public |

---

## 1. Login

| Field | Value |
|---|---|
| Method | `POST` |
| Route | `/api/Auth/login` |
| Access | Anonymous |
| Purpose | validates demo credentials and returns a JWT token |

### Request body

```json
{
  "username": "admin",
  "password": "admin123"
}
```

### Successful response

| Status | Meaning |
|---|---|
| `200 OK` | credentials matched one of the demo accounts |

```json
{
  "token": "<jwt-token>",
  "role": "Admin"
}
```

### Failure response

| Status | Meaning |
|---|---|
| `401 Unauthorized` | credentials did not match the configured demo accounts |

The current controller returns:

```json
{
  "message": "Invalid username or password."
}
```

---

## 2. Get all characters

| Field | Value |
|---|---|
| Method | `GET` |
| Route | `/api/VideoGameCharacters` |
| Access | `UserOrAdmin` policy |
| Purpose | returns a paged collection of characters with optional filtering and sorting |

### Query parameters

| Parameter | Type | Required | Description |
|---|---|---|---|
| `Page` | integer | No | requested page number |
| `PageSize` | integer | No | requested page size |
| `Game` | string | No | filter by game |
| `Role` | string | No | filter by role |
| `SortBy` | string | No | sort field: `name`, `game`, or `role` |
| `SortDirection` | string | No | sort direction; `desc` activates descending order |

### Example request

```http
GET /api/VideoGameCharacters?Game=Final%20Fantasy%20VII&SortBy=Name&SortDirection=asc&Page=1&PageSize=5
Authorization: Bearer <jwt-token>
```

### Successful response

| Status | Meaning |
|---|---|
| `200 OK` | request succeeded |

```json
{
  "page": 1,
  "pageSize": 5,
  "totalCount": 2,
  "items": [
    {
      "id": 1,
      "name": "Aerith",
      "game": "Final Fantasy VII",
      "role": "Heroine"
    },
    {
      "id": 2,
      "name": "Cloud",
      "game": "Final Fantasy VII",
      "role": "Hero"
    }
  ]
}
```

### Possible error responses

| Status | Meaning |
|---|---|
| `401 Unauthorized` | token missing or invalid |
| `403 Forbidden` | authenticated user does not satisfy policy |
| `500 Internal Server Error` | unhandled server failure |

---

## 3. Get a character by id

| Field | Value |
|---|---|
| Method | `GET` |
| Route | `/api/VideoGameCharacters/{id}` |
| Access | `UserOrAdmin` policy |
| Purpose | returns one character by primary key |

### Example request

```http
GET /api/VideoGameCharacters/1
Authorization: Bearer <jwt-token>
```

### Successful response

| Status | Meaning |
|---|---|
| `200 OK` | matching character was found |

```json
{
  "id": 1,
  "name": "Cloud",
  "game": "Final Fantasy VII",
  "role": "Hero"
}
```

### Not found response

| Status | Meaning |
|---|---|
| `404 Not Found` | no character with the given id exists |

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
  "title": "Character not found.",
  "status": 404,
  "detail": "No character with id 999 was found.",
  "instance": "/api/VideoGameCharacters/999"
}
```

---

## 4. Create a character

| Field | Value |
|---|---|
| Method | `POST` |
| Route | `/api/VideoGameCharacters` |
| Access | `AdminOnly` policy |
| Purpose | creates a new character and returns the created resource |

### Request body

```json
{
  "name": "Barret",
  "game": "Final Fantasy VII",
  "role": "Hero"
}
```

### Successful response

| Status | Meaning |
|---|---|
| `201 Created` | character was stored successfully |

Representative response shape:

```json
{
  "id": 3,
  "name": "Barret",
  "game": "Final Fantasy VII",
  "role": "Hero"
}
```

### Possible error responses

| Status | Meaning |
|---|---|
| `400 Bad Request` | body failed DTO validation |
| `401 Unauthorized` | token missing or invalid |
| `403 Forbidden` | authenticated user is not an admin |
| `500 Internal Server Error` | unhandled server failure |

---

## 5. Update a character

| Field | Value |
|---|---|
| Method | `PUT` |
| Route | `/api/VideoGameCharacters/{id}` |
| Access | `AdminOnly` policy |
| Purpose | updates an existing character |

### Request body

```json
{
  "name": "Tifa",
  "game": "Final Fantasy VII",
  "role": "Heroine"
}
```

### Successful response

| Status | Meaning |
|---|---|
| `204 No Content` | character existed and was updated |

### Possible error responses

| Status | Meaning |
|---|---|
| `400 Bad Request` | body failed DTO validation |
| `401 Unauthorized` | token missing or invalid |
| `403 Forbidden` | authenticated user is not an admin |
| `404 Not Found` | no character with the given id exists |
| `500 Internal Server Error` | unhandled server failure |

---

## 6. Delete a character

| Field | Value |
|---|---|
| Method | `DELETE` |
| Route | `/api/VideoGameCharacters/{id}` |
| Access | `AdminOnly` policy |
| Purpose | deletes an existing character |

### Successful response

| Status | Meaning |
|---|---|
| `204 No Content` | character existed and was deleted |

### Possible error responses

| Status | Meaning |
|---|---|
| `401 Unauthorized` | token missing or invalid |
| `403 Forbidden` | authenticated user is not an admin |
| `404 Not Found` | no character with the given id exists |
| `500 Internal Server Error` | unhandled server failure |

---

## 7. Health check

| Field | Value |
|---|---|
| Method | `GET` |
| Route | `/health` |
| Access | Public |
| Purpose | exposes a basic health endpoint |

### Successful response

| Status | Meaning |
|---|---|
| `200 OK` | health-check pipeline responded successfully |

## Authorization summary

| Route | Policy |
|---|---|
| `/api/Auth/login` | none |
| `/api/VideoGameCharacters` (`GET`) | `UserOrAdmin` |
| `/api/VideoGameCharacters/{id}` (`GET`) | `UserOrAdmin` |
| `/api/VideoGameCharacters` (`POST`) | `AdminOnly` |
| `/api/VideoGameCharacters/{id}` (`PUT`) | `AdminOnly` |
| `/api/VideoGameCharacters/{id}` (`DELETE`) | `AdminOnly` |

## Related documents

| Document | Why use it |
|---|---|
| `04-authentication-and-authorization.md` | how to obtain and apply a token |
| `05-validation-and-error-handling.md` | request validation and status-code rules |
| `06-querying-pagination-and-sorting.md` | detailed query behavior for list retrieval |
