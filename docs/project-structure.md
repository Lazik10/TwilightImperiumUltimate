# Project structure

This document explains the folder layout, naming conventions, and organization rules for the TwilightImperiumUltimate C# / Blazor WebAssembly solution.

Keep this file updated whenever folders, naming conventions, project responsibilities, or architectural boundaries change.

---

## Solution layout

```text
TwilightImperiumUltimate/
  TwilightImperiumUltimate.slnx
  Directory.Build.props
  README.md
  LICENSE

  .github/
    agents/
    instructions/
    prompts/
    skills/
    workflows/
    copilot-instructions.md

  docs/
    solution-overview.md
    project-structure.md
    architecture.md
    build-and-test.md

  src/
    .config/

    TwilightImperiumUltimate.Web/
      Components/
        Shared/
          Layouts/
            App.razor
            MainLayout.razor
      Documentation/
      Enums/
      Formatting/
      Helpers/
      Models/
      Options/
      Pages/
      Resources/
      Services/
      Validators/
      wwwroot/
      Program.cs
      _Imports.razor

    TwilightImperiumUltimate.Contracts/
      ApiContracts/
      DTOs/
      Enums/
      Options/

    TwilightImperiumUltimate.API/
      Controllers/
      Discord/
      Email/
      Helpers/
      Jobs/
        AsyncStatisticsSnapshotJob.cs
      Logs/
      Options/
      Services/
      Program.cs

    TwilightImperiumUltimate.Business/
      AutoMapper/
      Helpers/
      Logic/
      Services/

    TwilightImperiumUltimate.Core/
      Constraints/
      Entities/
      Formatting/
      Interfaces/

    TwilightImperiumUltimate.DataAccess/
      Configurations/
        Async/
      DbContexts/
      Migrations/
      Repositories/
        AsyncStatisticsSnapshotRepository.cs
      Schemas/
      Tables/

    TwilightImperiumUltimate.Draft/
      Drafts/
      ValueObjects/

    TwilightImperiumUltimate.Tigl/
      Achievements/
      AsyncRating/
      Discord/
      Extensions/
      Glicko2Rating/
      Helpers/
      RankUp/
      Services/
      TrueSkillRating/

    TwilightImperiumUltimate.Database/
      TwilightImperiumUltimate.Database.sqlproj

  tests/
    TwilightImperiumUltimate.Tests/
```

Notes:

- Generated output folders like `bin` and `obj` exist but are excluded from source structure documentation.
- The solution includes one SQL project (`TwilightImperiumUltimate.Database`) in addition to C# projects.

---

## Project responsibilities

### `TwilightImperiumUltimate.Web`

Blazor WebAssembly frontend project.

Responsibilities:

- Routable pages and reusable UI components.
- Layout/navigation and client-side state.
- Client-side validation.
- API communication through typed client services.
- Static frontend assets and scoped component CSS.

Should not contain:

- Database access.
- Server-only secrets.
- Direct SQL queries.
- Server-only business logic.

### `TwilightImperiumUltimate.Contracts`

Shared contracts and cross-boundary DTOs.

Responsibilities:

- API contracts.
- DTOs and shared enums.
- Shared option models used by client/server boundaries.

Should not contain:

- Blazor UI components.
- Server infrastructure.
- Dependencies on `TwilightImperiumUltimate.Web` or `TwilightImperiumUltimate.API`.

### `TwilightImperiumUltimate.API`

Backend API project.

Responsibilities:

- API controllers and identity endpoints.
- Authentication/authorization.
- Integration orchestration (Discord, SMTP, jobs/hosted services).
- API-side validation, logging, and diagnostics.

Should not contain:

- Blazor UI components.
- Client-only state handling.
- Browser-specific behavior.

### Supporting domain and infrastructure projects

- `TwilightImperiumUltimate.Business`: application/business orchestration.
- `TwilightImperiumUltimate.Core`: domain entities, constraints, shared interfaces.
- `TwilightImperiumUltimate.DataAccess`: EF Core persistence and schema config.
- `TwilightImperiumUltimate.Draft`: draft engines and value objects.
- `TwilightImperiumUltimate.Tigl`: TIGL and rating systems.
- `TwilightImperiumUltimate.Database`: SQL project artifacts.

### Test project

- `TwilightImperiumUltimate.Tests`: automated tests currently focused on TIGL and related data-centric behavior.

---

## Folder rules

### Client project (`TwilightImperiumUltimate.Web`)

| Folder | Purpose | Notes |
|---|---|---|
| `Pages` | Routable Blazor pages | Feature-organized page folders are used. |
| `Components` | Reusable UI components | `Components/Shared` contains core reusable primitives. |
| `Services` | Client-side services | Use DI and typed HTTP abstractions. |
| `Models` | Client UI models | Do not duplicate shared contracts unless UI-only. |
| `Resources` | Localized/front-end resource files | Keep localization assets organized by feature/domain. |
| `Validators` | Client validators | Works with FluentValidation/Blazilla. |
| `wwwroot` | Static assets | Includes `resources/images` with language-aware folders. |

### Shared contracts project (`TwilightImperiumUltimate.Contracts`)

| Folder | Purpose | Notes |
|---|---|---|
| `ApiContracts` | API request/response contracts | Includes TIGL contract areas. |
| `DTOs` | Data transfer models | Includes nested async DTO trees. |
| `Enums` | Shared enums | Keep values stable for API/client compatibility. |
| `Options` | Shared options models | Keep implementation-agnostic. |

### Server/API project (`TwilightImperiumUltimate.API`)

| Folder | Purpose | Notes |
|---|---|---|
| `Controllers` | API controllers | Main HTTP endpoint surface. |
| `Services` | API services/workflows | Keep server behavior testable and focused. |
| `Jobs` | Scheduled/background jobs | Quartz-based jobs and operational tasks. |
| `Options` | Options/configuration classes | Use strongly typed options binding. |
| `Discord`, `Email` | External integrations | Keep boundaries explicit and configurable. |

---

## Blazor component organization

Current project uses feature-oriented organization with reusable shared component primitives.

Patterns used in this repository:

```text
Components/
  Shared/
    ...reusable primitives...

Pages/
  About/
  Account/
  Community/
  Game/
  News/
  Rules/
  Tigl/
  Tools/
```

Component file pattern:

```text
MyComponent.razor
MyComponent.razor.css
MyComponent.razor.cs   (when complexity warrants code-behind)
```

---

## Naming conventions

### General C# naming

- Use `PascalCase` for public types/members and Blazor components.
- Use `camelCase` for locals/parameters.
- Use `_camelCase` for private fields where existing project style uses it.
- Prefer descriptive names over abbreviations.

### Blazor files

| Type | Example |
|---|---|
| Page | `RulesPage.razor` |
| Component | `FactionSummaryCard.razor` |
| Layout | `Components/Shared/Layouts/MainLayout.razor` |
| Scoped CSS | `FactionSummaryCard.razor.css` |
| Code-behind | `FactionSummaryCard.razor.cs` |

### Services

Prefer interface + implementation for non-trivial services:

```text
Services/
  IExampleService.cs
  ExampleService.cs
```

---

## Page conventions

Pages should:

- Include an `@page` directive.
- Keep routing explicit and readable.
- Delegate reusable UI to components.
- Delegate API/business behavior to services.
- Handle loading, empty, success, and error states where relevant.
- Keep heavy logic out of Razor markup.

---

## Component conventions

Components should:

- Have a single, clear purpose.
- Accept input through `[Parameter]`.
- Notify parents via `EventCallback<T>` where needed.
- Prefer services for non-trivial data/business operations.
- Use `@key` in lists when item identity matters.
- Keep markup readable and style via scoped CSS.

---

## Service conventions

Client-side and API-side services should:

- Be registered through DI (commonly via `ServiceCollectionExtensions.cs`).
- Hide raw transport details from UI components.
- Use strongly typed models/contracts.
- Avoid hardcoded environment-specific endpoints.
- Keep responsibilities focused for testability.

---

## CSS conventions

Prefer scoped CSS for component-specific styling:

```text
MyComponent.razor
MyComponent.razor.css
```

Use global CSS for app-wide primitives and shared utility patterns.

Rules:

- Use semantic class names.
- Avoid broad selectors that affect unrelated components.
- Avoid `!important` unless explicitly justified.
- Keep responsive behavior explicit.

---

## Static assets

Static files belong under `src/TwilightImperiumUltimate.Web/wwwroot`.

Current notable asset structure:

```text
wwwroot/
  resources/
    images/
      cs-CZ/
      en-US/
```

Rules:

- Use meaningful file names.
- Keep large assets optimized.
- Document and isolate JS interop files when required.

---

## Shared model conventions

Shared models in `TwilightImperiumUltimate.Contracts` must be safe for client consumption.

Guidelines:

- Keep transport models implementation-agnostic.
- Avoid server-only sensitive fields in client-visible contracts.
- Use clear request/response naming in API contract areas.
- Keep enum and option names stable to reduce API/client drift.

---

## Testing structure

Current test layout:

```text
tests/
  TwilightImperiumUltimate.Tests/
    Tigl/
```

Testing rules:

- Follow existing test patterns first.
- Add regression tests when fixing defects.
- Expand coverage beyond TIGL/data-centric behavior as other areas evolve.

---

## Dependency direction

Current project-reference flow:

```text
API -> Business, Contracts
Business -> DataAccess, Draft, Tigl
Core -> Contracts
DataAccess -> Core
Draft -> Core, DataAccess
Tigl -> Core, DataAccess, Contracts
Web -> Contracts
Tests -> DataAccess, Tigl
```

Avoid:

```text
Contracts -> implementation projects
Web -> server internals
Shared contracts -> UI or infrastructure dependencies
```

Client-server communication should happen through HTTP and shared API contracts, not direct server implementation references.

---

## Feature organization options

This solution uses a hybrid with strong feature grouping in pages and shared primitives in components.

Current project decision:

```text
Hybrid (feature-oriented pages + shared component primitives)
```

Rules:

- New files should follow existing project patterns.
- Avoid broad folder reorganization during unrelated feature work.
- Propose structural refactors as focused, separate changes where possible.

---

## Where to put new files

| New file type | Preferred location |
|---|---|
| Routable page | `src/TwilightImperiumUltimate.Web/Pages` |
| Reusable UI component | `src/TwilightImperiumUltimate.Web/Components` |
| Shared UI primitive | `src/TwilightImperiumUltimate.Web/Components/Shared` |
| Client API service | `src/TwilightImperiumUltimate.Web/Services` |
| Client-only UI model | `src/TwilightImperiumUltimate.Web/Models` |
| Shared DTO/API contract | `src/TwilightImperiumUltimate.Contracts/DTOs` or `src/TwilightImperiumUltimate.Contracts/ApiContracts` |
| API endpoint/controller | `src/TwilightImperiumUltimate.API/Controllers` |
| API service/workflow | `src/TwilightImperiumUltimate.API/Services` |
| Persistence config/repository | `src/TwilightImperiumUltimate.DataAccess` |
| Component CSS | Next to `.razor` file as `.razor.css` |
| Unit/integration test | `tests/TwilightImperiumUltimate.Tests` |

---

## What agents and Copilot should do first

Before making non-trivial changes:

1. Read `docs/solution-overview.md`.
2. Read this `docs/project-structure.md` file.
3. Read `docs/architecture.md` and `docs/build-and-test.md`.
4. Inspect similar existing files before introducing new patterns.
5. Keep changes focused and update docs when structure/rules change.

---

## Pull request checklist

When a PR changes structure or conventions, check:

- [ ] New files are in the correct folders.
- [ ] Names follow existing conventions.
- [ ] Project references still follow intended dependency direction.
- [ ] Shared contracts remain free of implementation dependencies.
- [ ] Services are registered correctly.
- [ ] Tests were added or updated where appropriate.
- [ ] `docs/project-structure.md` was updated if structure changed.
- [ ] Related docs (`docs/solution-overview.md`, `docs/architecture.md`) were updated when needed.

---

## Notes and project-specific decisions

### Decision: project registration and dependency flow

Date: 2026-07-03

Decision:

- Keep executable composition roots in `TwilightImperiumUltimate.API` and `TwilightImperiumUltimate.Web` with registration delegated to extension classes in appropriate projects.
- Keep `TwilightImperiumUltimate.Contracts` implementation-agnostic.

Reason:

- Preserves layering boundaries and keeps startup wiring discoverable.

Impact:

- New cross-project dependencies should be reviewed against documented dependency direction before merge.
