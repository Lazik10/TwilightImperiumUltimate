# Build and test

This document is the practical runbook for building, running, testing, and validating TwilightImperiumUltimate locally.

Use this together with:

- `docs/solution-overview.md`
- `docs/project-structure.md`
- `docs/architecture.md`

---

## 1. Solution type

This repository contains a C# / Blazor WebAssembly solution.

### Hosting model

Blazor WebAssembly frontend with a separate API backend:

- Frontend: `src/TwilightImperiumUltimate.Web`
- Backend API: `src/TwilightImperiumUltimate.API`
- Shared contracts: `src/TwilightImperiumUltimate.Contracts`

### Main projects

| Project | Purpose |
|---|---|
| `TwilightImperiumUltimate.Web` | Blazor WebAssembly frontend |
| `TwilightImperiumUltimate.Contracts` | Shared DTOs, models, enums, and API contracts |
| `TwilightImperiumUltimate.API` | ASP.NET Core backend/API |
| `TwilightImperiumUltimate.Tests` | Current automated tests |

Main solution file: `TwilightImperiumUltimate.slnx`

---

## 2. Prerequisites

Required tools:

- .NET 8 SDK
- Git
- SQL Server instance reachable from your local machine (for API database access)
- Visual Studio Code or Visual Studio
- Modern browser (Edge, Chrome, Firefox)

Optional but recommended:

- C# Dev Kit in VS Code
- .NET extension pack in VS Code
- GitHub Copilot

Check SDK availability:

```bash
dotnet --list-sdks
dotnet --version
```

Trust dev HTTPS certificate (first-time machine setup):

```bash
dotnet dev-certs https --trust
```

---

## 3. First-time setup

From repository root:

```bash
git clone <repository-url>
cd TwilightImperiumUltimate
dotnet restore TwilightImperiumUltimate.slnx
```

Restore local tools (EF Core CLI manifest is under `src/.config`):

```bash
dotnet tool restore --tool-manifest src/.config/dotnet-tools.json
```

Trust dev HTTPS certificate (first-time machine setup):

```bash
dotnet dev-certs https --trust
```

---

## 4. Configuration

### API configuration

Main config files:

- `src/TwilightImperiumUltimate.API/appsettings.json`
- `src/TwilightImperiumUltimate.API/appsettings.Development.json`

Critical settings for local run:

- `ConnectionStrings:TwilightImperium`
- `Frontend:LocalUrl`
- optional integration values (SMTP, Discord, API keys)

### Web configuration

Main config files:

- `src/TwilightImperiumUltimate.Web/wwwroot/appsettings.json`
- `src/TwilightImperiumUltimate.Web/wwwroot/appsettings.Development.json`

Critical setting for local run:

- `TwilightImperiumApiOptions:BaseUrl`

Current development convention:

- Web runs at `https://localhost:7299`
- API runs at `https://localhost:7292`
- Web development appsettings points API base URL to `https://localhost:7292/`

Security note:

- Do not commit real secrets, tokens, or production credentials in appsettings files.
- Move sensitive values to user-secrets or environment variables for local/prod-safe workflows.

### Database and EF Core commands

EF Core tool is managed via local tool manifest:

- `src/.config/dotnet-tools.json` (`dotnet-ef`)

Run from repository root.

Add migration:

```bash
dotnet ef migrations add <MigrationName> \
  --project src/TwilightImperiumUltimate.DataAccess/TwilightImperiumUltimate.DataAccess.csproj \
  --startup-project src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj \
  --output-dir Migrations
```

Update database:

```bash
dotnet ef database update \
  --project src/TwilightImperiumUltimate.DataAccess/TwilightImperiumUltimate.DataAccess.csproj \
  --startup-project src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj
```

The `AsyncStatisticsSnapshot` migration creates the persisted aggregate JSON row and a filtered unique index so only one row can be published. Deploy it with the same `dotnet ef database update` command before enabling the jobs. Source synchronization uses `AsyncStats:CroneExpression`; snapshot refresh uses `AsyncStats:SnapshotCronExpression` and defaults to ten minutes after each hour. Refresh failures preserve the last published row, so recovery is to fix the source/database issue and trigger the snapshot job again.

The server uses `IMemoryCache` for deserialized snapshots. `AddDistributedMemoryCache` remains process-local and is development-only; it is not a multi-instance cache. A horizontally scaled deployment must replace that registration with a shared `IDistributedCache` implementation such as Redis or SQL Server and coordinate its versioned keys before relying on cross-instance cache coherence. No provider-specific package is added because production cache and connection details are deployment-specific.

The snapshot reader uses `async-statistics-snapshot:v{SnapshotVersion}` as its distributed-cache key and keeps a bounded in-process copy. After a successful publication, the job invalidates the local memory entry; the next read resolves the new version and leaves older distributed entries to expire naturally. Profile visibility updates enqueue a refresh. If no snapshot exists, summary endpoints return `503 Service Unavailable` until the startup or scheduled job successfully publishes one.

Refresh logs include snapshot version, category count, build/persistence/total durations, generated time, and UTF-8 payload size. Summary responses include `X-Async-Snapshot-Generated-At` and `X-Async-Snapshot-Age-Seconds` alongside `ETag` and `Last-Modified`. The Web client uses category/filter/limit plus the observed snapshot version in its in-memory cache keys and starts optional category prefetch after the selected category's first render with a concurrency limit of three. Payload measurements are intentionally log-based and do not log serialized content; the current category endpoints remain the measured deployment choice until production traffic provides evidence for an aggregate endpoint.

Rollback to migration:

```bash
dotnet ef database update <MigrationName> \
  --project src/TwilightImperiumUltimate.DataAccess/TwilightImperiumUltimate.DataAccess.csproj \
  --startup-project src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj
```

Remove latest migration:

```bash
dotnet ef migrations remove \
  --project src/TwilightImperiumUltimate.DataAccess/TwilightImperiumUltimate.DataAccess.csproj \
  --startup-project src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj
```

---

## 5. Restore

Restore NuGet packages for the whole solution:

```bash
dotnet restore TwilightImperiumUltimate.slnx
```

Restore local tools (if needed):

```bash
dotnet tool restore --tool-manifest src/.config/dotnet-tools.json
```

---

## 6. Build

### Build whole solution

```bash
dotnet build TwilightImperiumUltimate.slnx
dotnet build TwilightImperiumUltimate.slnx --configuration Release
```

### Build key projects only

```bash
dotnet build src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj
dotnet build src/TwilightImperiumUltimate.Web/TwilightImperiumUltimate.Web.csproj
```

### VS Code task equivalents

If using workspace tasks:

- `build-api`
- `build-web`

---

## 7. Run the application

Run API:

```bash
dotnet run --project src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj
```

Run Web app (second terminal):

```bash
dotnet run --project src/TwilightImperiumUltimate.Web/TwilightImperiumUltimate.Web.csproj
```

Expected development URLs:

- Web: `https://localhost:7299`
- API: `https://localhost:7292`
- Swagger: `https://localhost:7292/swagger`

---

## 8. Hot reload

Web hot reload:

```bash
dotnet watch --project src/TwilightImperiumUltimate.Web/TwilightImperiumUltimate.Web.csproj
```

API hot reload:

```bash
dotnet watch --project src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj
```

---

## 9. Clean build

Use this when builds behave strangely or generated files appear stale:

```bash
dotnet clean TwilightImperiumUltimate.slnx
dotnet restore TwilightImperiumUltimate.slnx
dotnet build TwilightImperiumUltimate.slnx
```

---

## 10. Run tests

Current automated tests are in:

- `tests/TwilightImperiumUltimate.Tests`

Run all tests in test project:

```bash
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj
```

Run with Release config:

```bash
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj --configuration Release
```

Collect coverage:

```bash
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj --collect:"XPlat Code Coverage"
```

Current known focus areas include TIGL-related tests (for example async/glicko/trueskill flows).

---

## 11. Unit tests

Current unit-test coverage lives in:

- `tests/TwilightImperiumUltimate.Tests`

Run unit-style tests in the current test project:

```bash
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj
```

Current known focus areas include TIGL-related rating/async logic and supporting data-centric behavior.

## 12. Blazor component tests

Current status:

- No dedicated bUnit/component test project is documented yet.

Recommended future convention:

```text
tests/TwilightImperiumUltimate.ComponentTests/
```

Use component tests for high-value rendering, interaction, and state transitions (loading/empty/error/success).

## 13. Integration tests

Current status:

- No separate integration test project is documented yet.
- Existing tests in `TwilightImperiumUltimate.Tests` provide some integration-like verification for selected logic paths.

If a dedicated integration test project is introduced, add exact commands and required local dependencies here.

## 14. End-to-end tests

Current status:

- No dedicated E2E test project/command is documented yet.

If E2E tests are added (Playwright/Selenium), document:

- project path
- test run command
- browser/runtime prerequisites
- local environment dependencies

## 15. Test filtering

Filter by test class/method name:

```bash
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj --filter FullyQualifiedName~<NameFragment>
```

Filter by trait/category (if traits are present in tests):

```bash
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj --filter Category=<CategoryName>
```

## 16. Code formatting

Format entire solution:

```bash
dotnet format TwilightImperiumUltimate.slnx
```

Verify formatting without changing files:

```bash
dotnet format TwilightImperiumUltimate.slnx --verify-no-changes
```

## 17. Static analysis and warnings

Primary analyzer/compile validation:

```bash
dotnet build TwilightImperiumUltimate.slnx
dotnet build TwilightImperiumUltimate.slnx --configuration Release
```

Guidance:

- Fix new nullable/analyzer warnings introduced by a change.
- Avoid suppressing warnings unless there is a documented rationale.

## 18. CSS and frontend validation

Current frontend model:

- Blazor WebAssembly with standard CSS and scoped `.razor.css` files.
- No Node.js build/lint pipeline is currently documented for frontend assets.

Validate frontend changes by:

- building Web project
- running Web + API locally
- checking browser console/network for errors

## 19. Browser testing checklist

When changing UI, verify at minimum:

- page loads without console errors
- loading/empty/error/success states are handled
- forms validate and submit correctly
- expected API URL/host is used in network calls
- layout behaves correctly on desktop and smaller screens

Recommended browsers:

- Edge
- Chrome
- Firefox

## 20. Accessibility checks

For UI-impacting changes, validate:

- semantic HTML usage
- keyboard navigation support
- visible focus states
- input labels and helpful validation/error messages
- non-color-only status communication
- acceptable text contrast

## 21. Common validation workflow

Use this full baseline before merging significant changes:

```bash
dotnet restore TwilightImperiumUltimate.slnx
dotnet build TwilightImperiumUltimate.slnx
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj
dotnet format TwilightImperiumUltimate.slnx --verify-no-changes
```

For UI changes, also run API and Web locally and validate affected flows in the browser.

## 22. Pull request checklist

Before opening/merging a PR:

```md
- [ ] Solution restores successfully.
- [ ] API and Web projects build successfully.
- [ ] Relevant tests pass.
- [ ] Formatting check passes.
- [ ] No secrets were added to committed config.
- [ ] New dependencies are justified.
- [ ] UI changes were manually validated in browser (if applicable).
- [ ] Documentation was updated if setup/build/run/test behavior changed.
```

## 23. CI validation

This file documents local validation commands; keep CI commands aligned with the same baseline:

- restore
- build (typically Release)
- test
- formatting check

When CI workflow files are updated, reflect command changes here in the same change set.

## 24. Publish

Publish API in Release mode:

```bash
dotnet publish src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj --configuration Release --output ./artifacts/publish/api
```

Publish Web in Release mode:

```bash
dotnet publish src/TwilightImperiumUltimate.Web/TwilightImperiumUltimate.Web.csproj --configuration Release --output ./artifacts/publish/web
```

Deployment target(s) are environment-specific; keep this section synchronized with actual pipeline/release topology.

## 25. Troubleshooting

### Port mismatch between Web and API

Symptoms:

- Browser CORS/network failures
- 404/401 from wrong host

Checks:

- API launch settings: `src/TwilightImperiumUltimate.API/Properties/launchSettings.json`
- Web launch settings: `src/TwilightImperiumUltimate.Web/Properties/launchSettings.json`
- Web development API base URL: `src/TwilightImperiumUltimate.Web/wwwroot/appsettings.Development.json`

### SQL connectivity issues

Checks:

- `ConnectionStrings:TwilightImperium` in API appsettings
- SQL Server instance name and database existence
- local permissions and trust settings

### HTTPS certificate issues

Fix:

```bash
dotnet dev-certs https --trust
```

### Stale build artifacts

```bash
dotnet clean TwilightImperiumUltimate.slnx
dotnet restore TwilightImperiumUltimate.slnx
dotnet build TwilightImperiumUltimate.slnx
```

## 26. Guidance for AI agents and Copilot

When working in this repository:

1. Read `docs/solution-overview.md`, `docs/project-structure.md`, and `docs/architecture.md` first.
2. Use this file as the source of truth for build/run/test commands.
3. Prefer smallest relevant validation command first, then run full baseline before finalization.
4. Do not invent alternate commands when this file already documents them.
5. Update this file in the same PR whenever command paths/workflows change.

## 27. Maintenance rules

Update this document whenever:

- local ports change
- startup projects change
- project paths or names change
- tool manifest location/version changes
- test project layout changes
- required infrastructure (DB, services) changes

Suggested update process:

1. Validate commands directly in terminal.
2. Update this file in the same PR as structural/runtime changes.
3. Keep examples copy-paste ready and path-accurate.

## 28. Related documents

- `README.md`
- `docs/solution-overview.md`
- `docs/project-structure.md`
- `docs/architecture.md`
- `.github/copilot-instructions.md`
