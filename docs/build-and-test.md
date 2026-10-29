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

- .NET 10 SDK (or a newer SDK capable of targeting .NET 10)
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

The `AddLinkedPlayerProfileIds` migration adds nullable Async and TIGL profile ID columns to website users. Apply pending migrations before deploying the account-linking UI.

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
- `tests/TwilightImperiumUltimate.ComponentTests`
- `tests/TwilightImperiumUltimate.IntegrationTests`
- `tests/TwilightImperiumUltimate.E2ETests` (requires the publish/browser prerequisites in section 14)

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

The unit-test project is:

- `tests/TwilightImperiumUltimate.Tests`
- `tests/TwilightImperiumUltimate.ComponentTests`
- `tests/TwilightImperiumUltimate.IntegrationTests`

The latter projects contain Blazor component tests and database integration tests respectively. The separate E2E project exercises published Web assets and the API in a real browser; see section 14 before running the complete solution's tests.

Run unit-style tests in the current test project:

```bash
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj
```

Current known focus areas include TIGL-related rating/async logic and supporting data-centric behavior.

## 12. Blazor component tests

Component tests use bUnit:

```bash
dotnet test tests/TwilightImperiumUltimate.ComponentTests/TwilightImperiumUltimate.ComponentTests.csproj
```

Project:

- `tests/TwilightImperiumUltimate.ComponentTests`

The component-test project builds the Web project, so Web compilation must succeed before this test command can run.

Use component tests for high-value rendering, interaction, and state transitions (loading/empty/error/success).

## 13. Integration tests

Integration tests are in:

- `tests/TwilightImperiumUltimate.IntegrationTests`

Database integration tests use a uniquely named temporary SQL Server LocalDB database per test fixture (`(localdb)\MSSQLLocalDB`). The test process needs permission to create and drop databases on LocalDB.

The fixture disables `AUTO_CLOSE` only on its newly created disposable database. LocalDB otherwise enables it by default; with test connection pooling disabled for reliable teardown, closing the last connection can repeatedly close/reopen the database between operations. A regression test verifies this database option. No ordinary/main database, server-wide setting, production connection option or assertion retry is changed.

Integration collections run at most two at a time. Run solution test projects sequentially (`-m:1`, below) so browser and integration fixtures do not compete for LocalDB database creation. Earlier unrestricted runs hit SQL login/creation timeouts; this is a local test-resource boundary, not a production performance fix. Explicit transaction races and the 160-client load case retain their concurrent clients.

A crashed run or failed teardown can leave a disposable test database behind. Verify its ownership before removing it; never drop databases solely because their names share the `DraftRoomsTests_` prefix on a shared instance.

Run the integration tests:

```bash
dotnet test tests/TwilightImperiumUltimate.IntegrationTests/TwilightImperiumUltimate.IntegrationTests.csproj
```

Run the Draft Rooms hub integration tests:

```bash
dotnet test tests/TwilightImperiumUltimate.IntegrationTests/TwilightImperiumUltimate.IntegrationTests.csproj --filter FullyQualifiedName~Hub
```

These tests use SQL Server LocalDB, a `WebApplicationFactory` in the `Testing` environment, and SignalR Long Polling through `TestServer`. They cover hub subscription authorization, room isolation, notifications, participant removal, and presence state after a disconnect. They do not replace manual browser checks for deployed WebSocket/CORS connectivity, tab-close presence, or API-restart reconnection.

### Draft result and retention checks

The integration suite also verifies result snapshot creation and rollback in the finishing transaction, migration rollback/reapply, case-sensitive share tokens, source-room deletion independence, account deletion, manager/owner deletion rules, guest-account linking and activity rechecks during retention. Component tests cover public/finished-room results, account history paging, deletion confirmation, focus restoration, stale responses and noindex metadata.

`DraftRooms:InactiveRoomRetentionDays` defaults to `30`; `DraftRooms:CleanupCronExpression` defaults to `0 15 3 * * ?` (03:15 UTC daily). Negative retention, values over 36500 days and zero outside Development fail startup validation. Cleanup never expires result snapshots.

For manual development testing, **use a separate database for this worktree**. The normal API applies migrations at startup, and worktrees otherwise share the ordinary connection configuration. Override `ConnectionStrings__TwilightImperium` with a dedicated LocalDB database such as `TwilightImperiumUltimate_DraftRooms_Dev` before starting the API; do not point this branch at the database used by the older main branch.

On that isolated Development instance only, retention can be tested with `DraftRooms__InactiveRoomRetentionDays=0` and a temporary `DraftRooms__CleanupCronExpression=0/15 * * * * ?`. Create and finish a draft, record its public result link, allow cleanup to run, and verify the room disappears while the result remains readable. Restore the normal values afterwards. This accelerated cleanup is destructive to rooms in the configured database and must never target the ordinary/main or production database. The manual procedure and real-browser clipboard paste check remain deferred acceptance items; the automated cleanup tests use only uniquely named disposable test databases.

## 14. End-to-end tests

The xUnit/Playwright project is `tests/TwilightImperiumUltimate.E2ETests`. On Windows, from the repository root:

```powershell
dotnet publish src\TwilightImperiumUltimate.Web\TwilightImperiumUltimate.Web.csproj -c Release
dotnet build tests\TwilightImperiumUltimate.E2ETests\TwilightImperiumUltimate.E2ETests.csproj -c Release
powershell -NoProfile -ExecutionPolicy Bypass -File tests\TwilightImperiumUltimate.E2ETests\bin\Release\net10.0\playwright.ps1 install chromium
dotnet test tests\TwilightImperiumUltimate.E2ETests\TwilightImperiumUltimate.E2ETests.csproj -c Release --no-build
```

Publish again after changing Web code: these tests intentionally exercise published Release assets, not the development server or stale Debug files. Browser installation is a one-time prerequisite per Playwright version. `Microsoft.Playwright.Xunit` is Apache-2.0 licensed and supplies the real-browser runner; it is test-only.

The fixture starts two ephemeral loopback hosts (static Web with SPA fallback, and `WebApplicationFactory.UseKestrel(0)` API). It overrides only `TwilightImperiumApiOptions:BaseUrl` in the served public configuration and the API's allowed test frontend origin. Each fixture creates/drops only `DraftRoomsTests_<guid>` on LocalDB. It never starts the normal API against the ordinary/main database. Browser profiles are fresh, guest storage is isolated per context, non-test network origins are blocked, and every owned browser/host/database is disposed.

Chromium is the default. To use an already installed Edge instead, set `$env:DRAFT_ROOMS_E2E_BROWSER_CHANNEL = 'msedge'` in the same PowerShell process as `dotnet test`; clear that variable to return to Chromium. No existing user browser/profile is attached.

The room and full draft journeys each run three independent times. They cover create/join in new tabs, claims, live chat, reload persistence, the bare room layout, simultaneous private bans, unrevealed choice-bearing fields, snake turns, undo, a clean public result context, noindex and actual system clipboard text. Test interactions bring the active player's tab forward, including newly opened tabs and reloads, to model real users rather than relying on background WASM scheduling. The third room run also exercises the mobile section controls with Enter and checks that their buttons fit the viewport. Public pools legitimately contain every eligible faction; privacy assertions inspect selected state and private response fields, not absence of faction names.

Reload response inspection accepts only requests initiated after the new main-frame navigation commits; an old document's live refresh can otherwise lose its response body during navigation. Ordinary assertions allow 30 seconds; cold room startup and reload inspection allow 60 seconds to accommodate observed local fixture/database latency. There are no automatic retries or forced clicks. This startup allowance is not a production response-time guarantee.

The suite asserts that no JavaScript page exceptions occur. It does not certify a clean resource console or complete visual assets: the local published output can lack existing shared images/fonts, and external assets such as the html2canvas CDN are deliberately blocked. Verify complete deployed assets and visual appearance during manual acceptance.

After the prerequisites above, run the complete solution:

```powershell
dotnet test TwilightImperiumUltimate.slnx -c Release --no-build -m:1
```

### Opt-in room load check

Ordinary integration runs exclude `Category=Load` through `integration.runsettings`. An explicit test filter replaces that default, so run the one-minute load case alone:

```powershell
dotnet test tests\TwilightImperiumUltimate.IntegrationTests\TwilightImperiumUltimate.IntegrationTests.csproj -c Release --filter Category=Load --logger "console;verbosity=detailed"
```

The fixture prepares 20 rooms through the real create business command, rather than consuming/bypassing the HTTP ten-creations-per-IP/hour limit during measured traffic. It connects eight isolated guests per room over TestServer Long Polling and exercises actual authorized seat APIs and two-message-per-client chat bursts within production throttles for at least one minute. No production limits are disabled.

A test-only EF transaction interceptor timestamps actual committed room versions, which are matched to all clients' `RoomChanged` receipts. The test reports p50/p95/max delivery latency, CPU seconds, and sampled process-private-memory baseline/peak, verifies final delivery to all 160 clients and persisted room limits, and fails rather than substituting a notifier timestamp. API and clients share the measured test process; SQL Server is separate. This is a local integration sanity check, not a deployed WebSocket or capacity benchmark; acceptance of the measured numbers remains an operator decision.

### Draft Rooms release checks (operator-owned)

- Back up the target database before deploying the API/migrations; retain a tested rollback/recovery plan.
- Verify one API instance, WebSockets, HTTPS, the configured exact frontend CORS origins, and retention/cleanup settings.
- Redact `access_token` in reverse-proxy/web-server query logs; SignalR browser transports can send bearer tokens in that parameter.
- Verify real-device/mobile, keyboard/screen-reader, deployed reconnection and retention scenarios from the specification, then monitor errors for one day.
- These deployment/manual acceptance gates are not fulfilled by a local browser or load-test pass. Do not target the ordinary/main database to perform them.

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
