# 05 · Validation and Error Handling

## Overview

This document describes how the API currently handles invalid input, missing resources, authorization failures, and unexpected server errors.

The behavior comes from three main sources:

| Source | Responsibility |
|---|---|
| `[ApiController]` | automatic model-validation responses |
| controller actions | explicit `404 Not Found` responses for missing characters |
| `GlobalExceptionHandler` | centralized fallback for unhandled exceptions |

## Request validation

### Character creation and update DTOs

`CreateCharacterRequest` and `UpdateCharacterRequest` currently enforce the same rules.

| Field | Rules |
|---|---|
| `Name` | required, maximum 100 characters |
| `Game` | required, maximum 100 characters |
| `Role` | required, must match `Protagonist`, `Hero`, `Heroine`, `Antagonist`, or `Villain` |

### Example invalid body

```json
{
  "name": "",
  "game": "",
  "role": ""
}
```

### Result

Because the controller is marked with `[ApiController]`, invalid model state produces an automatic:

| Status | Meaning |
|---|---|
| `400 Bad Request` | request body failed DTO validation |

## Login request validation

`LoginRequest` currently contains:

| Field | Data annotations present? |
|---|---|
| `Username` | No |
| `Password` | No |

That means the login endpoint currently relies on credential matching logic rather than attribute-based request validation.

## Not-found behavior

For character retrieval, update, and delete operations, the controller explicitly returns `404 Not Found` when the target entity does not exist.

### Response shape

The controller uses `Problem(...)`, producing a response in `ProblemDetails` format.

Representative example:

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
  "title": "Character not found.",
  "status": 404,
  "detail": "No character with id 999 was found.",
  "instance": "/api/VideoGameCharacters/999"
}
```

## Global exception handling

Unexpected exceptions are handled by `GlobalExceptionHandler`.

### Current behavior

When an unhandled exception occurs, the handler:

1. logs the exception
2. records request method, path, and trace identifier
3. builds a `ProblemDetails` object
4. returns `500 Internal Server Error`
5. adds a `traceId` extension field

### Response shape

Representative response structure:

```json
{
  "type": "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
  "title": "An unexpected server error occurred.",
  "status": 500,
  "detail": "The server encountered an unexpected error while processing the request.",
  "instance": "/api/VideoGameCharacters",
  "traceId": "<trace-id>"
}
```

## Authorization-related failures

Authentication and authorization failures are handled by ASP.NET Core middleware rather than custom controller code.

| Situation | Status |
|---|---|
| missing token | `401 Unauthorized` |
| invalid token | `401 Unauthorized` |
| valid token but insufficient role | `403 Forbidden` |

## Error-behavior matrix

| Status code | Source | Typical trigger |
|---|---|---|
| `200 OK` | controller success path | successful GET or login |
| `201 Created` | controller success path | successful POST |
| `204 No Content` | controller success path | successful PUT or DELETE |
| `400 Bad Request` | `[ApiController]` validation | invalid create/update body |
| `401 Unauthorized` | authentication middleware | missing or invalid JWT |
| `403 Forbidden` | authorization middleware | user lacks required role |
| `404 Not Found` | explicit controller response | target character id not found |
| `500 Internal Server Error` | `GlobalExceptionHandler` | unhandled exception |

## Where each response type is produced

| Behavior | Produced by |
|---|---|
| validation failure | MVC model-validation pipeline |
| not-found for characters | `VideoGameCharactersController` |
| invalid login credentials | `AuthController` |
| unexpected failure | `GlobalExceptionHandler` |

## Accuracy notes about the current implementation

| Observation | Meaning |
|---|---|
| create/update DTOs are validated | body contract is explicit for write operations |
| login request is not data-annotation validated | login behaves differently from character write endpoints |
| not-found responses use `Problem(...)` | missing-resource errors are structured |
| unhandled exceptions are centralized | the API avoids raw exception leakage |

## Related documents

| Document | Topic |
|---|---|
| `03-api-reference.md` | endpoint-specific response behavior |
| `04-authentication-and-authorization.md` | `401` and `403` behavior |
