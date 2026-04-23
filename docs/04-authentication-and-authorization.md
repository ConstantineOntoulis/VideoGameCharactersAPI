# 04 · Authentication and Authorization

## Overview

The project uses JWT Bearer authentication and role-based authorization policies.

The implementation is intentionally simple and is designed to demonstrate:

- credential submission through an API endpoint
- token generation
- role claims inside the token
- endpoint protection using named policies

## Authentication model

### Current login source

The current `AuthController` validates credentials against two hardcoded demo accounts.

| Role | Username | Password |
|---|---|---|
| `User` | `user` | `user123` |
| `Admin` | `admin` | `admin123` |

This is a demo-only arrangement used to practice JWT issuance and authorization behavior.

### Login endpoint

| Field | Value |
|---|---|
| Route | `/api/Auth/login` |
| Method | `POST` |
| Access | Anonymous |

### Login request example

```json
{
  "username": "admin",
  "password": "admin123"
}
```

### Successful login response

```json
{
  "token": "<jwt-token>",
  "role": "Admin"
}
```

## Token contents

When login succeeds, the controller generates a JWT that includes:

| Claim | Meaning |
|---|---|
| `ClaimTypes.Name` | username |
| `ClaimTypes.Role` | role assigned to the authenticated user |

The token is signed with a symmetric key loaded from configuration.

## Token validation

JWT validation is configured in `Program.cs`.

### Validation checks

| Check | Enabled |
|---|---|
| issuer validation | Yes |
| audience validation | Yes |
| signature validation | Yes |
| lifetime validation | Yes |

### Configuration values used

| Setting | Purpose |
|---|---|
| `Jwt:Key` | symmetric signing key |
| `Jwt:Issuer` | expected token issuer |
| `Jwt:Audience` | expected token audience |

## Authorization model

Authorization is policy-based.

### Current policies

| Policy | Required roles |
|---|---|
| `UserOrAdmin` | `User`, `Admin` |
| `AdminOnly` | `Admin` |

### Policy usage in the controller

| Endpoint | Policy |
|---|---|
| `GET /api/VideoGameCharacters` | `UserOrAdmin` |
| `GET /api/VideoGameCharacters/{id}` | `UserOrAdmin` |
| `POST /api/VideoGameCharacters` | `AdminOnly` |
| `PUT /api/VideoGameCharacters/{id}` | `AdminOnly` |
| `DELETE /api/VideoGameCharacters/{id}` | `AdminOnly` |

## Request flow

```text
Client sends credentials to /api/Auth/login
   ↓
AuthController verifies demo username/password pair
   ↓
JWT token is created with Name and Role claims
   ↓
Client includes token in Authorization header
   ↓
API authentication middleware validates token
   ↓
Authorization middleware checks endpoint policy
   ↓
Controller action runs only if policy passes
```

## Using the token

After receiving a token, include it in the `Authorization` header:

```http
Authorization: Bearer <jwt-token>
```

### Example protected request

```http
GET /api/VideoGameCharacters
Authorization: Bearer <jwt-token>
```

## Behavior by failure type

| Situation | Result |
|---|---|
| no token provided | `401 Unauthorized` |
| invalid token provided | `401 Unauthorized` |
| valid token, wrong role | `403 Forbidden` |
| valid token, correct role | endpoint continues |

## Authentication vs authorization in this project

| Concept | Meaning in this codebase |
|---|---|
| Authentication | proving who the caller is by presenting a valid JWT |
| Authorization | checking whether the caller's role is allowed to use a given endpoint |

## How to test the auth flow

### In Scalar

1. run the API in development or through Docker Compose
2. call `POST /api/Auth/login`
3. copy the returned token
4. authorize subsequent requests with `Bearer <token>`

### In Postman

1. send a login request to `/api/Auth/login`
2. copy the token from the response body
3. open a protected request
4. add header `Authorization: Bearer <token>`

## Current design boundary

The auth layer is intentionally limited.

| Included | Not included |
|---|---|
| JWT issuance | persisted user accounts |
| role claims | hashed password storage |
| policy-based endpoint protection | registration flow |
| config-driven issuer/audience/key | refresh tokens |

## Related documents

| Document | Topic |
|---|---|
| `03-api-reference.md` | route-by-route auth requirements |
| `05-validation-and-error-handling.md` | 401, 403, and error behavior |
| `10-known-limitations-and-next-steps.md` | current simplifications and future improvements |
