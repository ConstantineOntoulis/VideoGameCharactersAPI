# 01 · Project Overview

## Purpose

`VideoGameCharactersAPI` is an intermediate ASP.NET Core Web API practice project designed to consolidate backend concepts in one coherent codebase.

The project does **not** attempt to model a large business domain. Its domain is intentionally small so that the focus remains on backend implementation choices rather than domain complexity.

## Why this project exists

This repository sits between beginner CRUD practice and a stronger recruiter-facing flagship project.

It was built to strengthen practical familiarity with:

| Area | What the project practices |
|---|---|
| API structure | Controllers, DTOs, services, dependency injection |
| Data access | EF Core with SQL Server and migrations |
| Contract discipline | Input validation and explicit response DTOs |
| Security basics | JWT authentication and policy-based authorization |
| Query behavior | Filtering, sorting, and pagination |
| Operational thinking | Docker setup, health checks, and automated tests |
| Failure handling | Consistent not-found responses and centralized exception handling |

## Scope and intent

| Aspect | Current position |
|---|---|
| Project type | Practice / hardening API |
| Complexity level | Intermediate |
| Domain depth | Deliberately simple |
| Main value | Backend discipline rather than business sophistication |
| Presentation goal | Serious technical practice that reads clearly and honestly |

## What is deliberately included

The project intentionally includes enough features to move beyond introductory CRUD work:

- protected endpoints
- request DTO validation
- role-based access control
- query normalization and pagination
- EF Core migrations
- Docker Compose execution
- health checks
- xUnit tests

## What is deliberately not the goal

This project is **not** positioned as:

- a production-grade identity system
- a large-scale business platform
- a microservices architecture
- an advanced domain-driven design exercise

That boundary is important. The project is strongest when it is presented as a disciplined learning-stage backend API rather than a system pretending to be larger than it is.

## Learning value of the chosen domain

The `Character` domain is small, but still useful for backend practice because it supports:

| Backend concept | Why this domain is enough |
|---|---|
| CRUD operations | characters can be created, read, updated, and deleted |
| Validation | `Name`, `Game`, and `Role` all carry input rules |
| Authorization | read and write operations can be split by role |
| Querying | list results can be filtered, sorted, and paged |
| Testing | endpoint behavior is easy to isolate and verify |

## Current feature profile

| Capability | Present in the current codebase |
|---|---|
| REST-style endpoints | Yes |
| DTO-based contracts | Yes |
| JWT login flow | Yes |
| Policy-based authorization | Yes |
| EF Core + SQL Server | Yes |
| Global exception handler | Yes |
| Health endpoint | Yes |
| Docker workflow | Yes |
| Test project | Yes |

## What this repository demonstrates to a reviewer

A reviewer should read this repository as evidence of the following:

- the ability to structure a small ASP.NET Core API cleanly
- familiarity with standard .NET backend components
- awareness of request validation, protected endpoints, and query design
- willingness to document technical choices and limitations honestly

## Document boundaries

This file focuses only on project purpose, scope, and positioning.

For other topics, use the companion documents:

| Document | Topic |
|---|---|
| `02-architecture.md` | internal structure and request flow |
| `03-api-reference.md` | endpoint-by-endpoint reference |
| `04-authentication-and-authorization.md` | JWT flow and access rules |
| `05-validation-and-error-handling.md` | validation and status-code behavior |
| `06-querying-pagination-and-sorting.md` | query mechanics |
| `07-database-and-entity-framework.md` | persistence layer |
| `08-testing.md` | current test coverage |
| `09-docker-and-local-setup.md` | runtime and container setup |
| `10-known-limitations-and-next-steps.md` | current boundaries and improvement paths |
