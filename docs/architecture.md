# Architecture

This document describes the current architecture of TwilightImperiumUltimate and aligns to the repository architecture template structure.

For folder-by-folder structure, see `docs/project-structure.md`.
For setup and build commands, see `docs/build-and-test.md`.

---

## 1. Solution summary

TwilightImperiumUltimate is a .NET 10 multi-project solution.

### Purpose

The application provides rules/reference browsing and game support capabilities for Twilight Imperium communities, including async statistics, draft/map/slice generation workflows, and TIGL ranking/leaderboard features.

### Primary users

- Players and community members browsing rules and statistics.
- League/organized-play participants using TIGL ranking and leaderboard views.
- Administrators/maintainers managing integrations and operational workflows.

### Main capabilities

- Rules/reference browsing and game support pages.
- Community and AsyncTI4 statistics features.
- Draft/map/slice generation workflows.
- TIGL ranking, leaderboard, and related admin flows.

---

## 2. Technology stack

| Area | Current implementation |
|---|---|
| Runtime | .NET 10 |
| Language | C# |
| Frontend | Blazor WebAssembly |
| UI markup | Razor, HTML |
| Styling | CSS, scoped `.razor.css` |
| Backend/API | ASP.NET Core Web API |
| Shared contracts | `TwilightImperiumUltimate.Contracts` project |
| Authentication | ASP.NET Core Identity API endpoints + bearer tokens |
| Authorization | ASP.NET Core authorization middleware/policies |
| Data storage | SQL Server (EF Core) |
| Background processing | Quartz.NET scheduled jobs + hosted services |
| Logging | Serilog (API request logging + browser console sink in Web) |
| UI libraries | Radzen Blazor components |
| Validation | FluentValidation + Blazilla |
| Mapping/mediator | AutoMapper + MediatR |
| Testing | xUnit + FluentAssertions + Moq + Bogus + coverlet |
| Package management | NuGet |
| IDEs | Visual Studio Code, Visual Studio |

---

## 3. Architectural style

The solution follows a layered architecture across Web client, API, application modules, and persistence.

Current runtime topology:

```text
Browser (Blazor WASM)
  -> TwilightImperiumUltimate.Web
    -> ITwilightImperiumApiHttpClient
      -> HTTPS API calls (Bearer token and optional x-api-key)
        -> TwilightImperiumUltimate.API controllers / identity endpoints
          -> Business layer
            -> DataAccess / Draft / Tigl services
              -> SQL Server and external integrations (Discord, SMTP, Async data sources)
```

  Async community statistics are published through a persisted aggregate snapshot. The hourly `AsyncGameDataJob` imports source data first and explicitly triggers `AsyncStatisticsSnapshotJob` only after successful batch commits; the snapshot job also has a configurable post-sync cron trigger and a startup trigger that only builds when no snapshot exists. It builds all categories from one scoped source projection/profile load, serializes only already-masked public DTOs, and atomically publishes one JSON row while retaining only the newest two rows. Summary endpoints read the published row through a bounded, single-flight `IMemoryCache`, use versioned `IDistributedCache` keys, return the existing DTO shapes, and expose the snapshot version through `ETag` and `Last-Modified`. A failed or cancelled refresh never unpublishes the previous successful row.

  `AddDistributedMemoryCache` is process-local and is suitable only for development. A horizontally scaled deployment must register a shared Redis or SQL Server `IDistributedCache` provider in the API host and keep the versioned key namespace shared between instances. This repository intentionally does not select a provider package or connection string because that is deployment-specific; the snapshot reader consumes the standard abstraction and remains compatible with either provider.

Design principles:

- Keep components focused on rendering and interaction.
- Move non-trivial behavior into services.
- Keep contracts strongly typed and implementation-agnostic.
- Keep persistence concerns in `TwilightImperiumUltimate.DataAccess`.
- Keep composition centralized in executable project startup and DI extensions.

---

## 4. Project roles

### `TwilightImperiumUltimate.Web`

Blazor WebAssembly frontend.

Responsibilities:

- Pages, routing, reusable UI components, and component-scoped styling.
- Client-side auth state handling and local storage integration.
- Typed API calls through `ITwilightImperiumApiHttpClient`.

Should not contain:

- Server persistence implementation details.
- Database or infrastructure code that belongs to API/server libraries.

### `TwilightImperiumUltimate.Contracts`

Shared transport contracts.

Responsibilities:

- DTOs, enums, options, and API contracts shared between client/server boundaries.

Should not contain:

- Blazor UI concerns.
- Server infrastructure implementation.

### `TwilightImperiumUltimate.API`

ASP.NET Core API backend and server composition root.

Responsibilities:

- Controllers and Identity endpoints.
- AuthN/AuthZ middleware and policies.
- Integration orchestration (Discord, SMTP, jobs/hosted services).

### Domain/application libraries

- `TwilightImperiumUltimate.Business`: application/business orchestration and DI composition for domain services.
- `TwilightImperiumUltimate.DataAccess`: EF Core DbContext, repositories, persistence schemas/configurations.
- `TwilightImperiumUltimate.Core`: core entities, interfaces, constraints, formatting primitives.
- `TwilightImperiumUltimate.Draft`, `TwilightImperiumUltimate.Tigl`: feature/domain modules.

### Tests

- `TwilightImperiumUltimate.Tests`: unit/integration-style automated tests focused currently on TIGL and data-centric behavior.

---

## 5. Main application areas

| Area | Typical location | Responsibility |
|---|---|---|
| Pages and routing | `src/TwilightImperiumUltimate.Web/Pages` | Routable UI pages |
| Reusable components | `src/TwilightImperiumUltimate.Web/Components` | Shared visual/interaction units |
| Client services | `src/TwilightImperiumUltimate.Web/Services` | API access and client state behavior |
| API endpoints | `src/TwilightImperiumUltimate.API/Controllers` | HTTP endpoint surface |
| Server services/workflows | `src/TwilightImperiumUltimate.API/Services` + module libraries | Domain workflows and integrations |
| Persistence | `src/TwilightImperiumUltimate.DataAccess` | EF Core and SQL persistence |
| Contracts | `src/TwilightImperiumUltimate.Contracts` | Shared DTOs/contracts |
| Jobs/background | `src/TwilightImperiumUltimate.API/Jobs` | Scheduled/background processing |

---

## 6. Dependency rules

### Allowed dependencies

Current project references:

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

### Avoided dependencies

Avoided dependencies:

```text
Contracts -> implementation projects
Web -> server internals (non-contract libraries)
UI -> direct persistence/infrastructure concerns
```

### Package rules

- Do not add new packages without documenting why the package is needed.
- Prefer built-in .NET and Blazor features where practical.
- Keep package/version updates consistent with solution-level dependency management.

---

## 7. Blazor component architecture

### Component responsibilities

Current component model:

- Components/pages focus on rendering and user interaction.
- Non-trivial behavior is delegated to client services.
- Shared UI is implemented through reusable components and scoped CSS.
- Code-behind files are preferred for complex pages/components.

Components should not contain server-only persistence or infrastructure behavior.

### Recommended component data flow

Recommended flow:

```text
Page/parent component
  -> passes parameters
Child component
  -> emits callback or triggers action
Client service
  -> calls typed API client
State update and UI re-render
```

### Component conventions

- Use reusable components instead of repeated markup blocks.
- Prefer code-behind for non-trivial page/component logic.
- Prefer scoped CSS (`.razor.css`) for component-specific styling.
- Keep UI state handling explicit (loading, empty, error, success).

---

## 8. State management

### Current approach

Current state approach:

- Local component state for page/component-level interactions.
- Scoped services for shared client state where needed.
- Browser local storage for persisted auth payload/state.

### State management rules

Rules:

- Keep state local unless shared behavior requires a service.
- Persist only what must survive browser refresh.
- Do not store sensitive credentials in insecure client storage.

### Example state flow

```text
User action
  -> Component event handler
    -> Client state/API service
      -> State update
        -> Component re-render
```

---

## 9. API communication

### Recommended pattern

Current communication pattern:

```text
Blazor component/page
  -> ITwilightImperiumApiHttpClient
    -> HTTP(S) API call (Bearer token, optional x-api-key)
      -> API controller/identity endpoint
        -> business/domain services
          -> persistence/external integrations
```

### API rules

Rules:

- Keep route/path constants centralized where practical.
- Keep request/response contracts strongly typed.
- Handle loading, empty, success, and error states explicitly in the UI.

### Real-time layer

Draft Rooms use ASP.NET Core SignalR at `/hubs/draft-rooms`. The Web client negotiates the available transport and automatically reconnects; after reconnecting it re-subscribes to the room and refreshes its current view. The API hub validates the account or guest credentials before adding a connection to room- and participant-scoped groups.

Committed room changes are published as `RoomChanged` after the persistence transaction completes. The hub also publishes `PresenceChanged`, `RoomClosed`, and participant-scoped `RemovedFromRoom` notifications. A singleton presence tracker records active connections and delays the offline transition for 30 seconds to allow short reconnects. The hosted `PresenceChatPublisher` persists and broadcasts `Join` and `Leave` chat messages for first-online and final-offline transitions without incrementing the room state version. Explicit participant leave and kick operations persist and broadcast their `Leave` messages in the same transaction as the room mutation.

Presence is currently process-local: the deployment must run one API instance. Horizontal scaling requires a shared SignalR backplane and distributed presence tracking before multiple API instances can safely serve Draft Rooms.

Successful room mutations append public-only activity log entries in the same persistence transaction. Password values and hashes and guest secrets are excluded; deleting a room cascades to its log entries.

The API accepts bearer tokens from the `access_token` query parameter only on `/hubs` paths, as required by browser WebSocket transports; other API routes continue to use the normal bearer-token handling. Integration tests use SignalR Long Polling over `TestServer`; they do not verify deployed WebSocket negotiation, cross-origin credentialed CORS, or browser tab-close/restart behavior. Those remain manual acceptance checks.

Faction Pick rules execute in the pure Draft module. The Business persistence adapter supplies settings, seats, the faction pool and immutable choices; the personalized `Project` method is the only engine-to-client projection. Pending picks remain seat-private, including from managers. Lock-in carries the observed absolute turn index so duplicate requests cannot accidentally lock the same seat's next turn at a snake boundary. Manager decisions disclose the replaced pending choice before the public pick; undo restores the last turn and removes pending choices. A configured lobby order is stored separately from runtime order, validated against the current room seats and copied on Start. Reset keeps configuration but deletes runtime state.

The room's `TurnAlertTitle` keeps a visible action-required prompt independent of the selected mobile panel. Its isolated visibility observer receives only a boolean from `document.visibilityState`; JavaScript receives no room state or credentials. A Blazor `PeriodicTimer` alternates the hidden tab title at a nominal one-second interval, and the local-storage preference `draftRooms.turnAlert.v1` defaults to enabled. Visible tabs, completed actions and disabling the preference restore the normal title immediately. Component disposal cancels the timer and removes the browser listener and .NET reference. Deterministic component tests verify the title and preference behavior. **RSK-04 remains a manual acceptance item:** no five-minute hidden-tab browser rhythm has been measured yet; browser background throttling can lengthen the nominal interval. The visible prompt remains available even if browser interop or preference storage fails.

### Finished draft results and retention

The final locked pick creates `Draft.DraftResults` and `Draft.DraftResultPlayers` in the same room transaction as the Finished transition. A filtered unique source-room index prevents a second snapshot. The public, versioned engine payload contains numbered seats, display-name snapshots, revealed bans, locked picks, settings and manager attribution, not runtime seat GUIDs, account/participant identifiers, private choices, chat or activity logs. The copy formatter uses stable faction enum identifiers so the pure Draft module does not depend on Web localization resources; the result UI uses the existing human-readable faction resources.

Results have no source-room foreign key and survive room deletion. Share tokens contain 128 random bits, are base64url encoded and matched with a case-sensitive SQL collation. `GET api/draft-rooms/results/{token}` is anonymous; invalid, unknown and deleted tokens return the same result-not-found problem. Verified room participants can retrieve the snapshot through `GET api/draft-rooms/{code}/result`. Public result pages use the main layout and the existing `SeoMetadata` noindex rule for nested Draft Rooms routes.

Account history includes snapshots an account owns or occupies, twenty per page. It projects the effective deletion permission in SQL rather than making a query per result. While the source room exists, only an active, verified host or co-host can delete its snapshot; ownership alone is insufficient. After room deletion, only the owning account may delete it, using a database predicate that also checks the source room is absent. Live-room deletion clears `ResultId`, appends a public log entry and broadcasts the version change after commit. The finished-room view reloads on version changes to remove another manager's deleted snapshot and share link.

Linking a guest account after finishing changes only ownership/player account associations. Public display names and the stored payload remain immutable. A guest host should link before closing the room or before retention removes it if they want account history and continued ownership; a guest-only orphan has no account owner.

`DraftRoomCleanupJob` runs daily at 03:15 UTC by default and disallows concurrent execution. It reads inactive rooms in batches of at most 100 and rechecks `LastActivityUtc` inside each room's unit of work before deleting, preserving results and notifying connected clients after commit. `DraftRooms:InactiveRoomRetentionDays` defaults to 30; zero is permitted only in Development, and negative or excessive retention values fail startup validation. `DraftRooms:CleanupCronExpression` controls the UTC schedule. Cleanup failures propagate to Quartz rather than being reported as successful runs.

The shared clipboard button awaits the actual browser clipboard operation, explicitly reports failure and preserves focus during its fallback. Its temporary confirmation and JS module are cancelled/disposed with the component. Automated tests cover clipboard completion, rejection, fallback and focus restoration; a real-browser paste check remains part of deferred manual acceptance.

---

## 10. Validation and error handling

### Validation rules

Current approach:

- Client-side validation via FluentValidation + Blazilla.
- Server-side validation at API/service trust boundaries.

### Error handling rules

Rules:

- Do not expose stack traces/internal exception details to end users.
- Keep user-facing messages clear and actionable.
- Log technical details on the server with sufficient request context.

### UI states to consider

- Loading state.
- Empty state.
- Validation error state.
- Permission denied state.
- Network/API error state.
- Success state.

---

## 11. Authentication and authorization

### Authentication

Summary:

- API uses Identity API endpoints with bearer token authentication.
- Web uses a custom `AuthenticationStateProvider` and local storage-backed auth state.

### Authorization

- Authorization is enforced server-side with middleware/policies/endpoints.

Rules:

- Protect API endpoints on the server.
- Keep client-side checks as UX guidance, not as sole enforcement.
- Keep authorization concerns centralized in middleware/policies/endpoints.

Authenticated website users can link their Discord account from Account Info. The API requires
the current bearer-authenticated user to start the flow, stores a random one-time state value with
a ten-minute expiry, and resolves the Discord callback to the initiating website user. It looks up
existing Async and TIGL profiles by the verified Discord ID and stores the nullable profile IDs on
that website user. Every completed authorization also saves Discord's account `username` (not
the guild nickname or global display name) in the existing `DiscordId` form field. All three
values are refreshed together, including null IDs for profiles that no longer match. The link
button stays disabled during authorization navigation and the return page waits for user loading.
This account-linking flow does not create a TIGL player profile. The API uses
`IDistributedCache` for OAuth state; deployments with multiple API instances must configure a
shared cache so the callback can consume the state created by the initiating instance.

---

## 12. Styling architecture

### Styling approach

Current styling approach:

- Component-level styling through `.razor.css` scoped styles.
- Shared/global styles and static assets under Web `wwwroot`.
- UI composition built with reusable components and Radzen controls.

### CSS rules

Rules:

- Prefer scoped styles for component-specific behavior.
- Keep class naming semantic and avoid broad global selectors.
- Preserve responsive behavior in reusable layout components.

---

## 13. Accessibility

Accessibility expectations:

- Prefer semantic HTML in Blazor components.
- Ensure keyboard navigation for interactive controls.
- Provide labels for inputs and meaningful alt text for images.
- Preserve visible focus states and avoid color-only status indicators.

Note: an accessibility-specific test baseline is recommended for high-value user flows.

---

## 14. Testing architecture

### Test strategy

Current state:

- `TwilightImperiumUltimate.Tests`: pure engine, business/service, and existing TIGL/data unit tests.
- `TwilightImperiumUltimate.ComponentTests`: bUnit rendering, interactions, accessibility contracts, and client-service regressions.
- `TwilightImperiumUltimate.IntegrationTests`: isolated SQL Server LocalDB, migrations, API authorization, transactions and SignalR. The explicitly opted-in load category measures SQL-commit-to-client delivery; ordinary runs exclude it.
- `TwilightImperiumUltimate.E2ETests`: published WASM and real Kestrel/API ports with Playwright contexts for guest identity, live room interactions, private bans/picks, snake order, undo, clipboard contents and public results. The hosts override only the public Web API address and test CORS origin; real authorization and production business rules remain enabled.

Recommended expansion path:

- Unit tests for additional business logic modules.
- API integration tests for endpoint and persistence contracts.
- Blazor component tests (bUnit) for high-value UI behavior.

### Recommended test ownership

| Code type | Recommended test type |
|---|---|
| Domain/business logic | Unit tests |
| API endpoint/persistence contracts | Integration tests |
| High-value Blazor UI behavior | Component tests (bUnit) |
| Critical end-user flows | End-to-end tests |

### Testing rules

- Follow existing repository test style and naming conventions.
- Add or update tests when behavior changes.
- Add regression tests for production bug fixes.
- Keep tests deterministic.

---

## 15. Logging, telemetry, and diagnostics

Current model:

- Serilog on API for request/application logging.
- Serilog browser console sink on Web.

Rules:

- Do not log secrets/tokens/sensitive personal data.
- Keep logs structured and sufficiently contextual for diagnosis.

---

## 16. Configuration

### Configuration sources

Current configuration sources include:

- API `appsettings.json` and environment-specific variants.
- Web `wwwroot/appsettings*.json` loaded during client startup.
- DI options binding in composition roots.

### Configuration rules

Rules:

- Keep secrets out of source control.
- Use environment variables, user secrets, or secret stores for sensitive values.

---

## 17. Security rules

- Treat browser-hosted client state as user-visible and user-modifiable.
- Enforce authorization and trust-boundary validation on server endpoints.
- Keep sensitive configuration out of committed client appsettings.
- Review dependency and integration changes for security impact.

---

## 18. Performance rules

Blazor/Web:

- Minimize unnecessary re-renders.
- Avoid expensive calculations directly in markup.
- Keep initial data loads focused to avoid large startup payloads.

API/Server:

- Avoid over-fetching and chatty endpoint patterns.
- Keep scheduled/background jobs idempotent and resilient.
- Keep schedules configurable and comments synchronized with actual intervals.

---

## 19. Build, deployment, and environments

### Environments

| Environment | Purpose |
|---|---|
| Local | Developer machine |
| Development | Shared development/staging environment |
| Test/QA | Validation environment |
| Production | Live environment |

### Deployment flow

```text
Developer branch
  -> Pull request
    -> CI build and tests
      -> Merge to main
        -> Deployment pipeline
          -> Target environment
```

### Build validation

Build validation:

```bash
dotnet restore
dotnet build
dotnet test
```

Repository currently documents local setup/build commands in `docs/build-and-test.md`.
Environment-specific deployment details should be kept here as they are formalized.

---

## 20. Architecture decision records

Recommended location:

```text
docs/decisions/
```

Current status:

- No formal ADR index is currently maintained in this file.
- Add ADR links when major architecture decisions are accepted.

### Decision log

| Date | Decision | Reason | Link |
|---|---|---|---|
| TBD | Start ADR log for major architecture choices | Improve traceability of architectural changes | `docs/decisions/` |

---

## 21. Known constraints

- Must preserve current layer boundaries and contract independence.
- Must support .NET 10 Blazor WASM client + ASP.NET Core API split architecture.
- Must preserve SQL Server + EF Core persistence model.
- Must support current integrations (Discord, SMTP, Async-related jobs/services).

---

## 22. Known technical debt

| Area | Issue | Impact | Desired fix |
|---|---|---|---|
| Configuration security | Sensitive values are present in tracked Web appsettings files | Risk of secret leakage and environment drift | Move secrets to secure configuration sources |
| Test coverage breadth | Coverage appears concentrated on TIGL/data-centric logic | Higher regression risk in broader API/Web behavior | Expand API/UI-focused test coverage |
| Operational clarity | Job schedule comments can drift from configured intervals | Operational confusion and incorrect run expectations | Keep schedule comments synchronized with configuration |

---

## 23. Guidance for AI agents and Copilot

When making architecture-relevant changes:

1. Read `docs/solution-overview.md`, this architecture file, and `docs/project-structure.md` first.
2. Preserve dependency direction and contract isolation rules.
3. Check composition roots (`Program.cs`, DI extension classes) when behavior changes.
4. Update tests and documentation when architecture, flows, or boundaries change.
5. Do not introduce new dependencies without rationale.
6. Keep secrets out of client-side committed configuration.

---

## 24. Maintenance rules

This document should be updated when:

- Projects are added/removed/renamed.
- Dependency direction changes.
- Authentication/authorization flow changes.
- API communication/state management patterns change.
- New integrations/jobs/workers are added.

Architecture change confirmation checklist:

- [ ] Dependency direction still enforces layer boundaries.
- [ ] Contracts remain implementation-agnostic.
- [ ] Auth and authorization flow is documented and consistent with runtime wiring.
- [ ] Background jobs and schedules are documented.
- [ ] Sensitive configuration handling follows security policy.
- [ ] Test strategy reflects the real critical paths.

If this document conflicts with code behavior, prefer code behavior and update this file in the same change set.

---

## 25. Related documents

- `README.md`
- `docs/solution-overview.md`
- `docs/project-structure.md`
- `docs/build-and-test.md`
- `docs/coding-standards.md`
- `.github/copilot-instructions.md`
