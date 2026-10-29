---
goal: Draft Rooms P05 - Live updates with SignalR and presence
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-16
owner: Lazik
status: 'In Progress'
tags: [feature, draft-rooms, signalr, realtime, backend, frontend, testing]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

Makes the lobby live. A small hub notifies clients, clients refetch their own view, presence shows who is online. The phase starts with a **connectivity spike** because the API has no real-time layer yet and its CORS setup has no credentials. It also introduces the hub integration tests. Specification: `LIV-001..003`, `LIV-006`, `SCR-006`, `SCR-007`, `DEC-08`.

Needs: P04 passed.

## 1. Requirements & Constraints

- **REQ-0501**: The hub only notifies (`LIV-006`): `RoomChanged(version)`, `PresenceChanged`, `RoomClosed`, `RemovedFromRoom`. Commands stay on REST. Each client then fetches its own view, which keeps hidden-information handling in one place.
- **REQ-0502**: Presence in memory, "joined" on the first connection, "left" after 30 seconds without one (`LIV-002`). One API instance (`DEC-08`), no backplane.
- **SEC-0501**: The guest secret travels inside the `Subscribe` message, never in the URL. A bearer token in `access_token` is accepted only for paths under `/hubs` (`SCR-006`).
- **SEC-0502**: Credentialed CORS applies only to the hub policy (`SCR-007`). Existing REST endpoints keep their present CORS behaviour.
- **CON-0501**: `Microsoft.AspNetCore.SignalR.Client` is a first-party Microsoft package (MIT) added to the Web project. Record it in the commit message.
- **CON-0502**: The test host replaces the SQL Server registration of `TwilightImperiumDbContext` and its factory with `TestDatabase` from P00 (temporary SQL Server LocalDB), removes the Discord bot and Quartz hosted services, and skips startup migrations when the environment is `Testing`.
- **GUD-0501**: Reconnect automatically and always refetch after a reconnect (`LIV-003`).

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-0501: Connectivity spike

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0501 | Add the NuGet package `Microsoft.AspNetCore.SignalR.Client` to `src/TwilightImperiumUltimate.Web/TwilightImperiumUltimate.Web.csproj`. Use a patched 10.0.x release; 10.0.9 has a known high-severity transitive SignalR vulnerability. | Web build and Release publish succeed; the dependency vulnerability audit reports no vulnerable packages. | x | 2026-10-16 |
| TASK-0502 | In the API call `AddSignalR()`, add `Hubs/DraftRoomHub.cs` with a `Ping()` method returning the server UTC time, and map it with `app.MapHub<DraftRoomHub>("/hubs/draft-rooms")`. Leave CORS unchanged for now. | API build passes. Startup/negotiate verification is pending a safe test host: normal API startup migrates the configured database and starts Discord/Quartz hosted services. | | |
| TASK-0503 | Add `Pages/Tools/DraftRoomDiagnostics.razor(.cs)` (`@page "/tools/draft-rooms/diagnostics"`, `[Authorize(Roles = "Admin")]`) with a **Ping hub** button that builds a `HubConnection` to the API base URL plus `/hubs/draft-rooms`, calls `Ping` and shows the answer or the error. | Web build, Release publish, and format checks pass. The authenticated browser ping remains pending until the safe API test host is available; do not infer a CORS result yet. | | |
| TASK-0504 | If TASK-0503 shows a CORS error about credentials: in `API/Program.cs` replace `app.UseCors(builder => ...)` by a default policy registered in `AddCors` (same origins, read from `builder.Configuration`) with a parameterless `app.UseCors()`, add a named policy `DraftRoomHub` with the same origins plus `AllowCredentials()`, and map the hub with `.RequireCors("DraftRoomHub")`. If there was no error, change nothing. Then repeat the ping on the deployed Development environment with WebSockets enabled and write the transport that was used into `docs/architecture.md` (new "Real-time layer" section). | Existing pages still load data (CORS unchanged for REST). The ping works locally and on the deployed environment. The architecture document names the transport. | | |

**Checkpoint:** the browser reaches the hub across origins, locally and deployed.

### Implementation Phase 2

- GOAL-0502: Presence, subscription, notifications

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0505 | Add `IPresenceTracker` and singleton `PresenceTracker` in `Business/Services/DraftRooms/`, tracking connection ids by room and participant. Raise `ParticipantOnline` and `ParticipantOffline`; delay offline transitions by the 30-second `DraftRoomLimits.PresenceGracePeriod` using injectable `TimeProvider` timers. | Fake-time tests cover multiple connections, room isolation, quick reconnect cancellation, and offline after the grace period. | x | 2026-10-16 |
| TASK-0506 | Implement `DraftRoomHub.Subscribe(string code, DraftRoomCredentials credentials)`: validate through `IDraftRoomAuthorizer` (account from the authenticated user, guest from id plus secret in the message), add the connection to group `room:{roomId}`, register presence, return the current `Version`. `OnDisconnectedAsync` updates presence. Configure `BearerTokenOptions.Events.OnMessageReceived` for `IdentityConstants.BearerScheme` to read `access_token` only when the path starts with `/hubs`. | Covered by the hub tests in TASK-0510 and TASK-0511. Manual: a wrong secret makes `Subscribe` fail. | | |
| TASK-0507 | Define the `IRoomChangeNotifier` port in Core because DataAccess may depend on Core but must not depend on Business. Provide a `NullRoomChangeNotifier` default in Business and implement `SignalRRoomChangeNotifier` in the API with `IHubContext<DraftRoomHub>`. Update `IDraftRoomUnitOfWork` (P03) to call the notifier **after** the transaction commits with `RoomChanged(version)`. Presence events send `PresenceChanged`. Close sends `RoomClosed`, kick sends `RemovedFromRoom` to that participant. | Unit tests verify that room changes notify only after commit, failures publish nothing, close emits `RoomClosed`, and kick emits `RemovedFromRoom`. | x | 2026-10-16 |
| TASK-0508 | Add `Services/DraftRooms/IDraftRoomConnection` and `DraftRoomConnection` (wraps `HubConnection`: `ConnectAsync(code, credentials)`, automatic reconnect with backoff, resubscribe after reconnect, events `RoomChanged`, `PresenceChanged`, `Closed`, a `State` property). Connect `DraftRoomSession` to it: refresh when `RoomChanged(version)` is newer than the current view and after every reconnect. | Fake-connection tests verify newer/older version handling, reconnect refresh, and close/removal/presence forwarding. Web Release build and publish, component tests, and formatting pass. | x | 2026-10-16 |

**Checkpoint:** server and client are wired, unit tests pass.

### Implementation Phase 3

- GOAL-0503: UI, hub integration tests, acceptance

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0509 | Add `DraftRoomConnectionStatus` (connected, reconnecting, offline) to the header with a visible reconnect banner. Add presence dots to `DraftParticipantList` and `DraftSeatCard` (the view DTO gets `IsOnline` from the tracker). The **Refresh** button stays only while disconnected. | bUnit: status text per state, dot per `IsOnline`, Refresh visible only when disconnected. | x | 2026-10-16 |
| TASK-0510 | Expose the API entry point with a public partial `Program` test hook and guard `app.CreateOrUpdateDbContextAsync()` with `if (!app.Environment.IsEnvironment("Testing"))`. In the integration project add the packages `Microsoft.AspNetCore.Mvc.Testing` and `Microsoft.AspNetCore.SignalR.Client`, a project reference to the API, and `Hosting/TestApiFactory.cs` (`WebApplicationFactory<Program>`, environment `Testing`, `TestDatabase` for `TwilightImperiumDbContext` and `IDbContextFactory<TwilightImperiumDbContext>`, suppresses hosted services, and registers a test authentication scheme for account callers). First test: two SignalR clients subscribe to one room, one claims a seat over REST, both receive `RoomChanged`. | `dotnet test tests/TwilightImperiumUltimate.IntegrationTests --filter FullyQualifiedName~Hub` passes. | x | 2026-10-16 |
| TASK-0511 | Add hub tests: after a disconnect and the grace period (configured to about 1 second in tests) the room view marks the participant offline, a client in room B never receives room A events, `Subscribe` with wrong credentials fails, and a kicked participant receives `RemovedFromRoom`. | All five hub integration tests and all 75 integration tests pass. TestServer verifies presence state; the offline timer-thread broadcast remains part of manual SCN-21 because it was not reliably observable through TestServer Long Polling. | x | 2026-10-16 |
| TASK-0512 | Update `docs/architecture.md` (real-time layer, hub, presence, single-instance note) and `docs/build-and-test.md` (hub tests). Run or record SCN-20 to SCN-22. | Documentation is updated; real-browser/deployed scenarios must be completed before the P05 phase gate is checked. | | |

**Phase gate (tick before starting P06):**

- [ ] SCN-20 a seat claim appears in the other browser within about 1 second without refresh
- [ ] SCN-21 closing a tab turns the dot grey, "left" follows after about 30 seconds (chat message comes in P06, check the dot and the log line in the API output)
- [ ] SCN-22 restarting the API makes both tabs recover on their own
- [ ] Spike result and transport are written in `docs/architecture.md`
- [ ] Hub tests green, existing REST CORS behaviour unchanged
- [ ] Build, tests, Release build and `dotnet format --verify-no-changes` clean for touched files

## 3. Alternatives

- **ALT-0501**: Polling. Rejected: slower, wasteful and no honest presence. SignalR was your choice.
- **ALT-0502**: Commands over hub methods. Rejected: REST is testable with Swagger and PowerShell, and keeps the hub thin and safe.
- **ALT-0503**: Redis backplane. Out of scope: one API instance (`DEC-08`).

## 4. Dependencies

- **DEP-0501**: NuGet `Microsoft.AspNetCore.SignalR.Client` (Web), `Microsoft.AspNetCore.Mvc.Testing` (tests).
- **DEP-0502**: WebSockets enabled on the hosting environment (you confirmed this is possible).

## 5. Files

- **FILE-0501**: `Web/TwilightImperiumUltimate.Web.csproj`, `Web/Services/DraftRooms/IDraftRoomConnection.cs`, `DraftRoomConnection.cs`, `DraftRoomSession.cs`, `Web/Components/DraftRooms/DraftRoomConnectionStatus.*`, `Web/Pages/Tools/DraftRoomDiagnostics.razor(.cs)`
- **FILE-0502**: `API/Hubs/DraftRoomHub.cs`, `API/Services/SignalRRoomChangeNotifier.cs`, `API/Program.cs`, `API/Services/ServiceCollectionExtensions.cs`
- **FILE-0503**: `Business/Services/DraftRooms/PresenceTracker.cs`, `IPresenceTracker.cs`, `IRoomChangeNotifier.cs`
- **FILE-0504**: `tests/TwilightImperiumUltimate.IntegrationTests/Hosting/TestApiFactory.cs`, `Hubs/*`
- **FILE-0505**: `docs/architecture.md`, `docs/build-and-test.md`

## 6. Testing

- **TEST-0501**: Unit tests for the presence tracker and the notifier ordering.
- **TEST-0502**: Hub integration tests: subscribe, notify, presence, isolation between rooms, wrong credentials, kick.
- **TEST-0503**: bUnit tests for the connection status and presence dots.
- **TEST-0504**: Manual SCN-20 to SCN-22.

## 7. Risks & Assumptions

- **RISK-0501**: Credentialed CORS and bearer-from-query behave differently across hosts (`RSK-03`). TASK-0503 and TASK-0504 test the real environments before more code depends on them.
- **RISK-0502**: A reverse proxy may close idle WebSockets. SignalR keep-alives and automatic reconnect cover it. Note any observed timeout in the architecture document.
- **RISK-0503**: The test host may start more services than expected. TASK-0510 removes the Discord bot and Quartz, and any other hosted service found must be removed too.
- **ASSUMPTION-0501**: The diagnostics page is temporary and is removed in P12.

## 8. Related Specifications / Further Reading

- [Specification: 6.6, 7.1, 10 (hub), 11 SCR-006, SCR-007](../docs/draft-rooms/specification.md)
- [Risk RSK-03, RSK-08](../docs/draft-rooms/specification.md)
