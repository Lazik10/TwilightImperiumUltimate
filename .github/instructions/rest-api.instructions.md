---
description: 'Guidelines for building REST APIs with ASP.NET (API project only)'
applyTo: 'src/TwilightImperiumUltimate.API/**/*.cs'
---

# ASP.NET REST API Development

Guidelines specific to the `TwilightImperiumUltimate.API` project: controllers, Minimal API endpoints, routing, and API documentation.

For data access, authentication/authorization, validation, logging, testing, performance, and deployment guidance shared across the whole solution, see `.github/instructions/csharp.instructions.md` — that guidance already applies to this project too. Do not duplicate it here.

## API Design Fundamentals

- Design resource-oriented URLs and use appropriate HTTP verbs.
- Follow the existing project convention (controllers vs. Minimal APIs) for a given feature rather than mixing styles.
- Return appropriate status codes and use the shared response contracts in `TwilightImperiumUltimate.Contracts`.
- Use RFC 9457 Problem Details for error responses.

## Controller-Based APIs

- Use attribute routing and the `[ApiController]` attribute.
- Return `ActionResult<T>` or another specific typed result; avoid returning bare `object`.
- Keep controllers thin — delegate business logic to `TwilightImperiumUltimate.Business` services via dependency injection.

## Minimal API Endpoints

- Group related endpoints with route groups to keep large endpoint files readable.
- Keep endpoint handlers thin; delegate to injected services.
- Follow the existing project convention rather than introducing a new API style without discussion.

## API Versioning and Documentation

- Document new endpoints, parameters, responses, and authentication requirements in Swagger/OpenAPI.
- Call out breaking changes and versioning impact when routes or contracts change.
