# 10 · Known Limitations and Next Steps

## Overview

This document records the current boundaries of the project as it exists today.

The purpose is not to diminish the project, but to describe it accurately and professionally.

## Current limitations

### 1. Demo authentication model

Authentication is intentionally simplified.

| Current state | Limitation |
|---|---|
| credentials are hardcoded in `AuthController` | no persisted user store |
| JWT tokens are issued successfully | no account management workflow |
| role claims are present | no registration, password reset, or refresh tokens |

### 2. Login request validation is minimal

`LoginRequest` currently has no data-annotation validation attributes.

| Effect | Meaning |
|---|---|
| login endpoint works | Yes |
| attribute-based request validation for login | No |

### 3. Query validation is lightweight

The query layer normalizes page values and handles sort fallbacks, but it does not formally validate all query parameters.

| Current behavior | Limitation |
|---|---|
| invalid page/page-size values are normalized | supported |
| unsupported `SortBy` falls back to `Id` ordering | no explicit 400 for invalid sort field |
| optional filters are available | no advanced search operators |

### 4. Test-host customization is not yet expanded

`CustomWebApplicationFactory` exists but does not currently override services or set up a specialized test database.

| Current state | Limitation |
|---|---|
| integration-style tests exist | Yes |
| custom host setup in factory | No |
| advanced test-environment configuration | No |

### 5. Persistence model is intentionally small

The data model currently contains one main entity.

| Current state | Limitation |
|---|---|
| single `Character` entity | Yes |
| relationships / navigation properties | No |
| richer domain model | No |

### 6. DTO rules and database schema are not perfectly mirrored

The write DTOs impose `StringLength` limits on `Name` and `Game`, but the current migration history does not fully enforce all of those same limits in the schema.

### 7. Root redirect behavior depends on environment assumptions

`Program.cs` always maps `/` to redirect to `/scalar`, while Scalar itself is only mapped in development.

In the current Docker setup this works because the environment is configured as development, but it remains a design caveat worth noting.

## What is already strong enough for the current stage

Even with the limitations above, the project already demonstrates:

- layered API organization
- DTO-based contracts
- JWT-based protected endpoints
- role-based access control
- filtering, sorting, and pagination
- centralized exception handling
- Docker-based execution
- automated tests
- supporting documentation

## Recommended next steps

The next steps should remain proportionate to the project's scope.

### High-value next steps

| Step | Why it matters |
|---|---|
| add CI workflow | completes the practice-project delivery story |
| validate `LoginRequest` | aligns login with the rest of the contract approach |
| clean package boundaries | removes test-only packages from the main API project |
| reduce tutorial-style comments | improves code presentation |
| improve `CustomWebApplicationFactory` | strengthens testing credibility |

### Optional refinement steps

| Step | Why it may help |
|---|---|
| align DB schema more closely with DTO limits | improves persistence consistency |
| standardize naming more tightly | improves polish and readability |
| add richer response assertions in tests | increases verification depth |

## What should not become the main focus here

This project does not need to become a full production platform.

So the next steps should **not** primarily be:

- building a full identity subsystem
- introducing unnecessary architectural layers
- expanding into a large multi-entity product
- overcomplicating the current educational scope

## Strategic role of this project

The best way to present this repository is:

| Positioning | Why it is appropriate |
|---|---|
| serious backend practice project | honest and technically grounded |
| hardening-phase API | matches the feature set |
| bridge toward a stronger flagship project | reflects its role in a learning progression |

## Final note

The project is strongest when it is documented and presented honestly:

- substantial enough to show real backend progression
- scoped enough to remain coherent
- transparent about what is simplified
- clear about what comes next
