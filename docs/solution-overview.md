# Solution overview

## Purpose

This solution is a C# / Blazor WebAssembly application.

TwilightImperiumUltimate is a .NET 8 solution that provides a full companion platform for Twilight Imperium 4th Edition players.

The application allows users to:

- Browse game references, community pages, and tools through a Blazor WebAssembly frontend
- Use an ASP.NET Core API for data, identity, and integrations
- Run drafting and map-generation workflows (faction, color, map, slices, milty)
- Track TIGL ranking and statistics logic (including multiple rating models)

Keep this document short and practical. Its purpose is to help developers and AI agents quickly understand the repository before making changes.

---

## Technology stack

- Language: C#
- UI framework: Blazor WebAssembly
- Markup: Razor, HTML
- Styling: CSS
- Backend/API: ASP.NET Core Web API + Identity API endpoints
- Authentication: ASP.NET Core Identity + bearer token auth
- Data storage: Entity Framework Core + SQL Server
- Testing: xUnit + FluentAssertions + Moq + Bogus + coverlet
- Package management: NuGet
- IDEs/editors: Visual Studio Code, Visual Studio 2026 Insiders
- AI assistance: GitHub Copilot
- Runtime: .NET 8
- Client auth state: custom `AuthenticationStateProvider` + local storage
- Scheduling/background jobs: Quartz.NET
- Logging: Serilog (API file/console, web browser console)
- External integrations: Discord.Net, SMTP (MailKit/MimeKit), AsyncTI4 data pull
- UI component library: Radzen Blazor
- Validation: FluentValidation + Blazilla
- Mapping and messaging: AutoMapper, MediatR

---

## Solution structure

```text
src/
  TwilightImperiumUltimate.API/
  TwilightImperiumUltimate.Business/
  TwilightImperiumUltimate.Contracts/
  TwilightImperiumUltimate.Core/
  TwilightImperiumUltimate.DataAccess/
  TwilightImperiumUltimate.Database/
  TwilightImperiumUltimate.Draft/
  TwilightImperiumUltimate.Tigl/
  TwilightImperiumUltimate.Web/
tests/
  TwilightImperiumUltimate.Tests/
```

Typical responsibilities:

| Project/folder | Purpose |
|---|---|
| `TwilightImperiumUltimate.Web` | Blazor WebAssembly frontend |
| `TwilightImperiumUltimate.Contracts` | Shared DTOs, contracts, enums, and option models |
| `TwilightImperiumUltimate.API` | Backend/API project |
| `tests` | Unit tests (currently concentrated in `TwilightImperiumUltimate.Tests`) |

---

## Main projects

### `TwilightImperiumUltimate.Web`

Blazor WebAssembly frontend project.

Responsibilities:

- Pages
- Reusable UI components
- Layout and navigation
- Client-side state
- Client-side validation
- API calls through typed services
- Component-specific CSS

Should not contain:

- Server-only business logic
- Database access
- Secrets
- Direct SQL queries
- Long-running background jobs

### `TwilightImperiumUltimate.Contracts`

Shared project for contracts and models used by both client and server.

Responsibilities:

- DTOs
- Request and response models
- Shared enums
- Shared validation attributes
- Constants and options that are safe to share with the client

Should not contain:

- UI components
- CSS
- Server infrastructure
- Database-specific entities unless intentionally shared
- Secrets or private configuration

### `TwilightImperiumUltimate.API`

Backend/API project.

Responsibilities:

- API endpoints
- Authentication and authorization
- Business services
- Data access
- External integrations
- Server-side validation
- Logging

Should not contain:

- Blazor UI components
- Client-only state
- Browser-specific logic

---

## Main application areas

| Area | Typical location | Notes |
|---|---|---|
| Pages | `src/TwilightImperiumUltimate.Web/Pages` | Routable Blazor pages by domain (`Game`, `Community`, `Tools`, `Rules`, `Tigl`, etc.) |
| Components | `src/TwilightImperiumUltimate.Web/Components` | Reusable UI components and shared controls/layout |
| Layout | `src/TwilightImperiumUltimate.Web/MainLayout.razor` | App shell, navigation, and shared layout |
| Services | `src/TwilightImperiumUltimate.Web/Services` | Client-side services and API clients |
| Shared models | `src/TwilightImperiumUltimate.Contracts` | DTOs, requests/responses, enums |
| Static assets | `src/TwilightImperiumUltimate.Web/wwwroot` | Images, CSS, fonts, static files |
| Component styles | `*.razor.css` | Prefer scoped CSS for component-specific styles |
| Tests | `tests/TwilightImperiumUltimate.Tests` | Unit tests currently focused on TIGL rating logic |

---

## Architecture rules

- Keep Blazor components focused on UI and interaction logic.
- Move reusable business logic into services.
- Prefer dependency injection over static service access.
- Prefer strongly typed models over dynamic data.
- Keep API contracts in shared models when appropriate.
- Do not duplicate DTOs across client and server unless there is a clear reason.
- Avoid JavaScript interop unless Blazor cannot solve the problem cleanly.
- Do not store secrets in the Blazor WebAssembly client.
- Do not introduce new NuGet packages without explaining why.

---

## Dependency direction

Current project reference direction:

```text
Web -> Contracts

API -> Business, Contracts

Business -> DataAccess, Draft, Tigl

DataAccess -> Core

Core -> Contracts

Draft -> Core, DataAccess

Tigl -> Core, DataAccess, Contracts

Tests -> DataAccess, Tigl
```

General rules:

- The contracts project should not depend on implementation projects.
- UI components should depend on abstractions/services, not low-level infrastructure.
- Server projects may depend on shared contracts.
- Client and API communication should remain HTTP + contract based.

---

## Blazor conventions

- Pages go in `Pages` (feature-organized in this repository).
- Reusable components go in `Components` and shared primitives in `Components/Shared`.
- Component-specific CSS should usually go in `.razor.css` files.
- Use `[Parameter]` for component inputs.
- Use `EventCallback<T>` for child-to-parent events.
- Use `[CascadingParameter]` only for app-wide or layout-level state.
- Use `@key` when rendering lists where item identity matters.
- Avoid large inline expressions in Razor markup.
- Avoid heavy business logic directly in `.razor` files.
- Use lifecycle methods intentionally.

---

## CSS and HTML conventions

- Prefer semantic HTML.
- Prefer component-scoped CSS for component-specific styling.
- Use global CSS only for application-wide styles, layout primitives, variables, or resets.
- Keep CSS class names descriptive.
- Avoid overly broad selectors.
- Avoid `!important` unless there is a clear reason.
- Do not duplicate existing utility classes or shared styles.
- Consider accessibility when adding markup, forms, buttons, modals, menus, or custom controls.

---

## State management

Current approach:

- Local component/page state for view-specific interactions
- Scoped services for shared user/session behavior
- Singleton caches for selected high-read datasets (rankings/TIGL)
- Local storage for authentication persistence across reloads

Common patterns:

- Use local component state for simple UI behavior.
- Use scoped services for shared client-side state.
- Use browser storage only when state must survive reloads.
- Avoid global state unless multiple independent components need the same data.

Example flow:

```text
User action
  -> Blazor event handler
  -> service call or state update
  -> component state update
  -> UI re-render
```

---

## API communication

Preferred pattern:

```text
Page/Component
  -> domain service/provider
  -> ITwilightImperiumApiHttpClient
  -> typed DTO request/response
  -> API endpoint path constants
```

Rules:

- Components should not scatter raw API URLs throughout the UI.
- Prefer typed request and response models from `TwilightImperiumUltimate.Contracts`.
- Keep endpoint paths centralized in path resources/constants.
- Handle loading, empty, success, validation, and error states.
- Do not expose technical exception details to users.
- Keep authentication and authorization behavior consistent with the rest of the project.

---

## Error handling

Expected approach:

- Show user-friendly validation messages.
- Show loading states for async operations.
- Show empty states when there is no data.
- Show safe error messages when an operation fails.
- Log technical details where appropriate.
- Do not leak secrets, tokens, stack traces, or internal exception details to users.

Current logging behavior:

- API uses Serilog request logging plus configured sinks (console and rolling files).
- Web logs to browser console via Serilog sink.

---

## Testing approach

Use the existing test style in the repository.

Current test setup:

- Framework: xUnit
- Assertions/mocking: FluentAssertions, Moq
- Test data helpers: Bogus
- Coverage collector: coverlet.collector
- Current coverage focus: TIGL rating implementations (`Async`, `Glicko`, `TrueSkill`)

When adding or changing features:

- Add unit tests for reusable business logic.
- Add component tests for important UI behavior when bUnit tests are introduced.
- Add integration tests for API/service behavior where practical.
- Add end-to-end tests for critical user flows if/when an E2E framework is added.
- Update existing tests when behavior intentionally changes.

---

## Build and validation

Before considering a change complete, run or consider:

```bash
dotnet restore TwilightImperiumUltimate.slnx
dotnet build TwilightImperiumUltimate.slnx
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj
dotnet format TwilightImperiumUltimate.slnx --verify-no-changes
```

Common focused builds:

```bash
dotnet build src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj
dotnet build src/TwilightImperiumUltimate.Web/TwilightImperiumUltimate.Web.csproj
```

---

## Local development

Restore packages:

```bash
dotnet restore TwilightImperiumUltimate.slnx
```

Build solution:

```bash
dotnet build TwilightImperiumUltimate.slnx
```

Run tests:

```bash
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj
```

Run the API:

```bash
dotnet run --project src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj
```

Run the Blazor WebAssembly client:

```bash
dotnet run --project src/TwilightImperiumUltimate.Web/TwilightImperiumUltimate.Web.csproj
```

Notes:

- Web development config points API base URL to localhost (`wwwroot/appsettings.Development.json`).
- API uses SQL Server connection string key `TwilightImperium`.

---

## Important constraints

- Do not add NuGet packages without explaining the reason.
- Do not change public DTOs or API contracts without checking client/server impact.
- Keep component implementation logic in code-behind files when following existing pattern.
- Prefer shared/reusable components and services over duplicated behavior.
- Do not introduce JavaScript unless necessary.
- Do not store secrets in the Blazor WebAssembly client.
- Do not silently change authentication, authorization, routing, or data persistence behavior.
- Avoid broad formatting-only changes mixed with behavioral changes.

---

## Common change workflow for agents

Before implementing non-trivial changes:

1. Read this file.
2. Inspect similar existing files.
3. Identify the correct project and folder.
4. Follow the existing naming and coding conventions.
5. Make the smallest useful change.
6. Update related tests and documentation when relevant.
7. Run or recommend the relevant validation commands.
8. Summarize changed files, assumptions, and follow-up risks.

---

## Pull request checklist

Use this checklist when reviewing changes:

- [ ] The solution builds.
- [ ] Tests pass or test impact is explained.
- [ ] New behavior follows existing architecture.
- [ ] Components remain focused and readable.
- [ ] Reusable logic is placed in services where appropriate.
- [ ] CSS is scoped or intentionally global.
- [ ] Loading, empty, validation, and error states are handled where relevant.
- [ ] No secrets or sensitive values were added.
- [ ] Public contracts were not changed accidentally.
- [ ] Documentation was updated if architecture, folder structure, build commands, or conventions changed.

---

## Notes for GitHub Copilot and AI agents

This file is a project map. It does not replace reading the actual code.

When this document conflicts with the code:

1. Prefer the existing code pattern for the immediate change.
2. Mention the conflict in the response or pull request notes.
3. Suggest updating this document if it is stale.
