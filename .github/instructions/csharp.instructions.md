---
description: 'Guidelines for building C# applications'
applyTo: '**/*.cs'
---

# C# Development

## C# Instructions
- Always use the latest version C#, currently C# 14 features.
- Write clear and concise comments only for public methods and properties that are not self-explanatory.

## General Instructions
- Make only high confidence suggestions when reviewing code changes.
- Write code with good maintainability practices, including comments on why certain design decisions were made.
- Handle edge cases and write clear exception handling.
- For libraries or external dependencies, mention their usage and purpose in comments.

## Naming Conventions

- Follow PascalCase for component names, method names, and public members.
- Use camelCase for private fields and local variables.
- Prefix interface names with "I" (e.g., IUserService).

## Formatting

- Apply code-formatting style defined in `.editorconfig`.
- Prefer file-scoped namespace declarations and single-line using directives.
- Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Use `nameof` instead of string literals when referring to member names.
- Ensure that XML doc comments are created for any public APIs. When applicable, include `<example>` and `<code>` documentation in the comments.

## Nullable Reference Types

- Declare variables non-nullable, and check for `null` at entry points.
- Always use `is null` or `is not null` instead of `== null` or `!= null`.
- Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.

## Data Access Patterns

- Use Entity Framework Core following the repository's existing data-access pattern; don't introduce a new one.
- Keep migrations and seed data consistent with `TwilightImperiumUltimate.DataAccess` conventions.
- Write efficient, async query patterns and avoid N+1 queries.

## Authentication and Authorization

- Enforce authentication and authorization consistently across controllers and Minimal APIs.
- Use role/policy-based authorization matching existing policies; never rely on client-side checks alone.

## Validation and Error Handling

- Validate models using data annotations or FluentValidation, matching the existing project convention.
- Use a centralized exception-handling middleware and return RFC 9457 Problem Details for API errors.
- Never leak stack traces or internal exception details to callers.

## Logging and Monitoring

- Use the project's existing structured logging provider (see `Program.cs`/`GlobalUsings.cs` for current setup).
- Do not log secrets, tokens, connection strings, or PII.

## Testing

- Always include test cases for critical paths of the application.
- Do not emit "Act", "Arrange" or "Assert" comments.
- Copy existing style in nearby files for test method names and capitalization.
- Mock dependencies rather than hitting real databases/network calls in unit tests.

## Performance Optimization

- Use asynchronous programming for I/O-bound work.
- Paginate, filter, and sort large data sets instead of returning full result sets.
- Use caching (in-memory or distributed) where it clearly helps and is already part of the project's pattern.

## Deployment and DevOps

- Follow `docs/build-and-test.md` for build, test, run, and publish commands.
- See `.github/skills/containerize-aspnetcore/SKILL.md` before adding or changing container support.
