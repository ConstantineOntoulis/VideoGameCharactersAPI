# 08 · Testing

## Overview

The solution includes a separate xUnit test project:

```text
VideoGameCharactersAPI.Tests
```

The current tests focus on endpoint behavior and small query-rule helpers.

## Test stack

| Tool | Role |
|---|---|
| xUnit | test framework |
| `WebApplicationFactory<Program>` | boots the API for integration-style tests |
| `HttpClient` | sends requests to the test host |

## Current test files

| File | Category | What it checks |
|---|---|---|
| `AuthenticationTests.cs` | integration-style | successful admin login returns `200 OK` and a token |
| `AuthorizationTests.cs` | integration-style | protected create endpoint rejects anonymous requests with `401` |
| `ForbiddenTests.cs` | integration-style | non-admin user receives `403` on admin-only create |
| `ValidationTests.cs` | integration-style | invalid create body returns `400 Bad Request` |
| `SuccessEndpointTests.cs` | integration-style | valid admin create request returns `201 Created` |
| `SmokeTests.cs` | integration-style | application boots and responds |
| `QueryRulesTests.cs` | unit | page and page-size normalization logic |

## Integration test model

Most tests follow this pattern:

```text
Create test client
   ↓
call login endpoint if a token is needed
   ↓
attach Bearer token when required
   ↓
send request to protected endpoint
   ↓
assert expected status code
```

## Auth-related test coverage

| Behavior | Covered? |
|---|---|
| valid admin login returns token | Yes |
| anonymous create is rejected | Yes |
| user role blocked from admin create | Yes |
| admin can create character | Yes |

## Validation-related coverage

| Behavior | Covered? |
|---|---|
| invalid create body returns `400` | Yes |
| update-body validation | No dedicated test currently present |
| login request DTO validation | not applicable in the current code because `LoginRequest` has no data annotations |

## Unit-test coverage

`QueryRulesTests` currently verifies:

| Rule | Covered? |
|---|---|
| page below `1` becomes `1` | Yes |
| page size below `1` becomes `10` | Yes |
| page size above `50` becomes `50` | Yes |

## Current test-host setup

The project includes `CustomWebApplicationFactory`, but its current implementation is empty:

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
}
```

### What that means

| Observation | Meaning |
|---|---|
| custom factory exists | Yes |
| custom host overrides are present | No |
| custom in-memory database setup | No |
| explicit test seeding in factory | No |

So the current tests use the default behavior of `WebApplicationFactory<Program>` rather than a specialized test environment.

## How to run the tests

### Run all tests

```bash
dotnet test
```

### Run the test project specifically

```bash
dotnet test VideoGameCharactersAPI.Tests
```

## What the current tests demonstrate

The current suite demonstrates that the project has moved beyond manual-only verification and now checks:

- login behavior
- authorization boundaries
- validation failures
- success paths
- basic query normalization logic

## Current test boundaries

The test suite currently does **not** claim to provide:

- exhaustive endpoint coverage
- performance testing
- load testing
- full custom test-host orchestration
- detailed response-body verification for every scenario

That is acceptable for the present scope of the project.

## Related documents

| Document | Topic |
|---|---|
| `03-api-reference.md` | routes exercised by the tests |
| `04-authentication-and-authorization.md` | auth and role behavior behind the tests |
| `10-known-limitations-and-next-steps.md` | current testing limitations |
