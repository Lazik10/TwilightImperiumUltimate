# Copilot repository instructions

This repository contains a **hosted C# / Blazor WebAssembly solution with an ASP.NET Core backend**.

These instructions apply globally to the repository. More specific rules may exist in `.github/instructions/*.instructions.md` for C#, Blazor/Razor, CSS, tests, and other file types.

---

## Project context

This solution uses a Blazor WebAssembly frontend (`TwilightImperiumUltimate.Web`) and an ASP.NET Core API backend (`TwilightImperiumUltimate.API`) with shared transport contracts in `TwilightImperiumUltimate.Contracts`.

Before making non-trivial changes, review these documents:

- `docs/solution-overview.md` — high-level solution purpose and project map
- `docs/project-structure.md` — folder layout, naming, and file placement rules
- `docs/architecture.md` — architecture, dependencies, state, API, and security rules
- `docs/build-and-test.md` — restore, build, run, test, format, and publish commands

Follow the documented architecture and conventions unless the existing code clearly shows a newer pattern.

---

## General behavior

When working in this repository:

- Inspect existing code before creating new patterns.
- Prefer small, focused changes over broad rewrites.
- Preserve the current folder structure unless a change is clearly justified.
- Reuse existing services, components, models, and helpers where practical.
- Do not introduce new abstractions unless they reduce duplication or clarify responsibilities.
- Do not rename public APIs, routes, DTOs, files, or folders without explaining the impact.
- Do not add new NuGet packages without explaining why the dependency is needed.
- Do not remove tests, validation, error handling, or accessibility behavior unless replacing it with something better.
- Keep generated code readable and maintainable for a human C# developer.

---

## Architecture boundaries

Respect the intended dependency direction of the solution.

Current dependency direction in this solution:

```text
TwilightImperiumUltimate.Web -> TwilightImperiumUltimate.Contracts
TwilightImperiumUltimate.API -> TwilightImperiumUltimate.Business, TwilightImperiumUltimate.Contracts
TwilightImperiumUltimate.Business -> TwilightImperiumUltimate.DataAccess, TwilightImperiumUltimate.Draft, TwilightImperiumUltimate.Tigl
TwilightImperiumUltimate.DataAccess -> TwilightImperiumUltimate.Core
TwilightImperiumUltimate.Core -> TwilightImperiumUltimate.Contracts
TwilightImperiumUltimate.Draft -> TwilightImperiumUltimate.Core, TwilightImperiumUltimate.DataAccess
TwilightImperiumUltimate.Tigl -> TwilightImperiumUltimate.Core, TwilightImperiumUltimate.DataAccess, TwilightImperiumUltimate.Contracts
```

Global rules:

- Keep UI behavior in Blazor components.
- Move reusable business logic into services.
- Keep shared contracts independent from UI and infrastructure concerns.
- Keep server-only logic out of the Blazor WebAssembly client.
- Do not place secrets, private keys, connection strings, or privileged configuration in the WebAssembly client.
- Treat all client-side code and data as visible to users.
- Validate important business rules on the server when a server/API exists.
- Avoid JavaScript interop unless Blazor or .NET cannot reasonably solve the problem.

---

## Change workflow

For implementation tasks:

1. Read the relevant documentation in `docs/`.
2. Inspect similar existing files before editing.
3. Identify the smallest safe change.
4. Follow existing naming, layout, and dependency patterns.
5. Update related tests when behavior changes.
6. Update documentation when setup, architecture, commands, or conventions change.
7. Summarize changed files and important assumptions.

For larger tasks, first produce a short plan before editing.

---

## Validation expectations

Use `docs/build-and-test.md` as the source of truth for commands.

Primary validation commands for this repository:

```bash
dotnet restore TwilightImperiumUltimate.sln
dotnet build TwilightImperiumUltimate.sln
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj
```

Recommended formatting validation (optional when `dotnet format` is not available in the current environment):

```bash
dotnet format --verify-no-changes
```

When the change affects UI behavior, also consider:

- Loading states
- Empty states
- Error states
- Validation messages
- Keyboard navigation
- Focus behavior
- Browser console errors
- Network/API failures
- Responsive layout

If validation cannot be run, clearly state which commands should be run and why they matter.

---

## Documentation rules

Update documentation when changes affect:

- Solution structure
- Project names or paths
- Build, run, test, or publish commands
- Architecture or dependency direction
- Authentication or authorization
- API communication
- State management
- New required configuration
- New dependencies
- Important developer workflow changes

Do not duplicate large documentation sections inside this file. Link to the appropriate document instead.

---

## Security rules

- Never commit secrets.
- Never put secrets in the Blazor WebAssembly client.
- Do not expose stack traces or internal exception details to users.
- Do not bypass authorization checks.
- Do not rely only on client-side authorization for protected server resources.
- Avoid unsafe HTML rendering.
- Sanitize or encode user-provided content where appropriate.
- Review new dependencies for maintenance, security, and license risk.

---

## Quality expectations

Generated code should be:

- Correct
- Simple
- Readable
- Strongly typed
- Testable where practical
- Consistent with the existing repository
- Conservative with dependencies
- Clear about assumptions

Prefer boring, maintainable code over clever code.

---

## Pull request summary expectations

When summarizing work, include:

- What changed
- Why it changed
- Files or areas affected
- Tests or validation performed
- Any assumptions or follow-up work

Use this format when helpful:

```text
Summary:
- [change 1]
- [change 2]

Validation:
- [command/result]
- [manual check/result]

Notes:
- [assumption or follow-up]
```

---

## Instruction file ownership

This file is intentionally global.

Put language-specific, framework-specific, or file-specific rules in targeted instruction files, for example:

```text
.github/instructions/csharp.instructions.md
.github/instructions/blazor.instructions.md
.github/instructions/css.instructions.md
.github/instructions/html.instructions.md
.github/instructions/testing.instructions.md
```

Do not duplicate detailed rules across global and targeted instruction files.
