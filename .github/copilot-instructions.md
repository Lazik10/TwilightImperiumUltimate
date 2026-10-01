# Copilot repository instructions

Hosted C# / Blazor WebAssembly solution: `TwilightImperiumUltimate.Web` (Blazor WASM) + `TwilightImperiumUltimate.API` (ASP.NET Core) + shared `TwilightImperiumUltimate.Contracts`. File-type rules live in `.github/instructions/*.instructions.md` and auto-apply — don't duplicate them here.

Before non-trivial changes, check `docs/solution-overview.md`, `docs/project-structure.md`, `docs/architecture.md`, `docs/build-and-test.md`.

## Behavior

- Inspect existing code/patterns before creating new ones; prefer the smallest safe change.
- Reuse existing services/components/models. Don't add abstractions, packages, or rename public APIs/routes/DTOs/files without explaining impact.
- Never remove tests, validation, error handling, or accessibility without a better replacement.
- Update tests/docs when behavior, architecture, commands, or conventions change. Summarize changed files and assumptions.

## Architecture (dependency direction)

`Web → Contracts`. `API → Business, Contracts`. `Business → DataAccess, Draft, Tigl`. `DataAccess → Core`. `Core → Contracts`. `Draft → Core, DataAccess`. `Tigl → Core, DataAccess, Contracts`.

- UI behavior stays in Blazor components; reusable business logic goes in services.
- Server-only logic, secrets, and connection strings never go in the WASM client — treat all client-side code/data as visible to users.
- Validate important business rules server-side when an API exists. Avoid JS interop unless Blazor/.NET can't reasonably solve it.

## Security

No secrets or exception internals exposed to users/client. Don't bypass or rely only on client-side authorization. Sanitize/encode user content; avoid unsafe raw HTML. Review new dependencies for security/license risk.

## Validation

Source of truth: `docs/build-and-test.md` (`dotnet restore`/`build`/`test`, `dotnet format --verify-no-changes`). For UI changes also check loading/empty/error states, keyboard/focus, console errors, responsive layout. State plainly if a command wasn't run.

## Quality

Correct, simple, readable, strongly typed, testable, conservative with dependencies, consistent with the repo. Prefer boring, maintainable code over clever code.
