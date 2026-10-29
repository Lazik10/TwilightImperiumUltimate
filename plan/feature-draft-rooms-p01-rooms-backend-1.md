---
goal: Draft Rooms P01 - Rooms backend, create and join
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-16
owner: Lazik
status: 'In progress'
tags: [feature, draft-rooms, backend, api, database, security]
---

# Introduction

![Status: In progress](https://img.shields.io/badge/status-In%20progress-yellow)

Builds the server side of "create a room" and "join a room": the database tables (added to the main `TwilightImperiumDbContext`), password and guest-secret handling, create and join commands, and two API endpoints. You verify it with the tests and with Swagger or PowerShell, no UI yet. Specification: `ROM-001..006`, `ROM-011`, `IDN-001..005`, `SCR-001..002`, `SCR-005`, `SCR-010`, decisions `DEC-28` and `ASM-01`.

Needs: P00 finished.

## 1. Requirements & Constraints

- **REQ-0101**: Implement create and join exactly as `ROM-001..006` and `IDN-001..005` describe, including limits from `DraftRoomLimits`.
- **REQ-0102**: Failure answers use `ApiResponse<T>` with `Success = false` plus an HTTP status. The server sends no message text, the Web localises it.
- **SEC-0101**: Passwords and guest secrets follow `SCR-001` and `SCR-002` and are never logged.
- **SEC-0102**: Wrong code and wrong password are indistinguishable, also by timing class (run a dummy hash verification when the room does not exist).
- **SEC-0103**: Create and join responses send `Cache-Control: no-store`.
- **CON-0101**: Draft room tables are added to the main `TwilightImperiumDbContext` (`DEC-28`) in the new schema `Draft`, in the existing configuration style, and the migration goes into the existing `Migrations/` folder. Existing tables, configurations and migrations are not modified. Avoid unnecessary provider-specific mapping constructs.
- **CON-0102**: Repositories take `IDbContextFactory<TwilightImperiumDbContext>` and create one context per method, like `SlicesArchiveRepository`. The application registers contexts with `NoTracking` as the default, so write paths must opt in to tracking.
- **CON-0103**: StyleCop member ordering (SA1201/SA1202/SA1204/SA1214), `ArgumentNullException.ThrowIfNull` on public methods, `CancellationToken` on async APIs.
- **GUD-0101**: Commands and handlers follow `Business/Logic/SlicesArchive/AddNewSliceDraftCommand*.cs`. Results use `FluentResults.Result<T>` with a `DraftRoomError` carrying a `DraftRoomErrorCode`.
- **PAT-0101**: Keep the controller thin and mediator-based like `API/Controllers/DraftsController.cs`.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-0101: Persistence model exercised against SQL Server LocalDB

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0101 | Add enums `DraftRoomStatus` (Lobby, InProgress, Finished), `DraftRoomRole` (Member, CoHost, Host), `DraftParticipantStatus` (Active, Left, Kicked) in `src/TwilightImperiumUltimate.Contracts/Enums/`. Add `DraftRoomLimits` in `src/TwilightImperiumUltimate.Contracts/DraftRooms/DraftRoomLimits.cs` with the constants from `ROM-001`, `ROM-005`, `ROM-008`, `SET-001`, `CHT-001`, `CHT-003`, `LIV-002` (room name 3-60, password 6-64, display name 2-24, `CodeLength` 6, `CodeAlphabet` `ABCDEFGHJKMNPQRSTUVWXYZ23456789`, seats 2-8 default 6, 32 participants, chat 300, 5 open rooms, 5 failures per 15 minutes, 10 creates per hour, 30 s presence grace). | `dotnet build src/TwilightImperiumUltimate.Contracts` succeeds. New test `tests/TwilightImperiumUltimate.Tests/DraftRooms/DraftRoomLimitsTests.cs` asserts the alphabet has 31 distinct characters and none of `0 O 1 I L`. | ✅ | 2026-10-15 |
| TASK-0102 | Add entities `DraftRoom` and `DraftRoomParticipant` in `src/TwilightImperiumUltimate.Core/Entities/DraftRooms/` (Guid `Id`, properties exactly as spec 9.2, `DraftRoom.Version` int, `DraftRoom.Participants` navigation). They do not implement `IEntity` (it requires an int `Id`). | `dotnet build src/TwilightImperiumUltimate.Core` succeeds. | ✅ | 2026-10-15 |
| TASK-0103 | Add `Draft = "Draft"` to `DataAccess/Schemas/Schema.cs` and the table names `DraftRooms` and `DraftRoomParticipants` to `Tables/TableName.cs`. Add `DraftRoomConfiguration` and `DraftRoomParticipantConfiguration` in `DataAccess/Configurations/DraftRooms/` (the main context applies every configuration of the assembly, so nothing else is registered; copy the style of `Configurations/Galaxy/SliceDraftConfiguration.cs`). Add the DbSets `DraftRooms` and `DraftRoomParticipants` under a `// Draft rooms` comment in `DbContexts/TwilightImperium/TwilightImperiumDbContextDbSets.cs`. Mappings: schema `Draft`, unique index on `Code`, `Version` as `IsConcurrencyToken()`, enums `HasConversion<string>()`, lengths from spec 9.2, `UserId` as foreign key to `AspNetUsers` with `OnDelete(DeleteBehavior.SetNull)` and the same `nvarchar(450)` type as `AspNetUsers.Id`, filtered unique indexes written as `[UserId] IS NOT NULL` and `[Status] = 'Active'`, no `rowversion`, no SQL Server-only functions. | `dotnet build src/TwilightImperiumUltimate.DataAccess` succeeds. Model tests in the next task pass. | ✅ | 2026-10-15 |
| TASK-0104 | Add `tests/TwilightImperiumUltimate.IntegrationTests/DraftRooms/DraftRoomsModelTests.cs` on top of the `TestDatabase` helper from P00 (TASK-0005 and TASK-0006). Cases: unique `Code`, unique (`RoomId`, `DisplayNameKey`) for Active, two guests with `UserId` null allowed, unique (`RoomId`, `UserId`), cascade delete of participants, deleting the user sets `UserId` to null, concurrency conflict on `Version` throws `DbUpdateConcurrencyException`. | `dotnet test tests/TwilightImperiumUltimate.IntegrationTests --filter FullyQualifiedName~DraftRooms` shows every test passed. | ✅ | 2026-10-15 |
| TASK-0105 | Create the migration with the normal commands from `docs/build-and-test.md`: `dotnet ef migrations add AddDraftRooms --project src/TwilightImperiumUltimate.DataAccess/TwilightImperiumUltimate.DataAccess.csproj --startup-project src/TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj --output-dir Migrations`. Read the generated `Up()` and `Down()`: they must contain only the two new tables, their indexes and the foreign key. If other tables appear, the model snapshot has drifted: stop and fix that first. The API already migrates at startup (`AppExtensions.CreateOrUpdateDbContextAsync`), so nothing else is registered. | Apply, inspect, roll back and reapply the migration on a disposable SQL Server LocalDB database; start the API against a separate temporary migrated database and confirm Swagger responds. | ✅ | 2026-10-15 |
| TASK-0106 | Add `IDraftRoomRepository` and `DraftRoomRepository` in `DataAccess/Repositories/` (factory `IDbContextFactory<TwilightImperiumDbContext>`, one context per method, tracking enabled where a method writes) with `CodeExistsAsync`, `CreateRoomWithHostAsync`, `GetRoomByCodeAsync`, `FindParticipantByUserAsync`, `FindParticipantByIdAsync`, `AddParticipantAsync`, `CountOpenRoomsForUserAsync`, `CountActiveParticipantsAsync`, `IsDisplayNameTakenAsync`. Register in `RegisterRepositories`. Add `DraftRoomRepositoryTests` in the integration project on top of `TestDatabase`. | `dotnet test tests/TwilightImperiumUltimate.IntegrationTests` is green, each repository method has at least one test. | ✅ | 2026-10-15 |

**Checkpoint:** the tables exist on SQL Server, the model and repository tests pass on the test database chosen in P00.

### Implementation Phase 2

- GOAL-0102: Business rules for create and join

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0107 | In `src/TwilightImperiumUltimate.Business/Services/DraftRooms/` add `IRoomCodeGenerator`/`RoomCodeGenerator` (`RandomNumberGenerator.GetItems` over `DraftRoomLimits.CodeAlphabet`), `IGuestSecretService`/`GuestSecretService` (`Generate` = 32 random bytes as base64url, `Hash` = SHA-256, `Verify` with `CryptographicOperations.FixedTimeEquals`), `IRoomPasswordHasher`/`RoomPasswordHasher` (wraps `PasswordHasher<DraftRoom>`: `Hash`, `Verify`). Register them in `Business/ServiceCollectionExtensions.cs` through a new `RegisterDraftRoomServices()`. | Unit tests in `tests/TwilightImperiumUltimate.Tests/DraftRooms/`: 10,000 codes are 6 characters from the alphabet, secrets are unique and 43 characters, `Verify` is true for the right secret and false for a changed one, two hashes of one password differ, a wrong password fails. | ✅ | 2026-10-15 |
| TASK-0108 | Add `IDraftRoomDisplayNameValidator` with `Normalize` (trim, collapse spaces), `MakeKey` (upper invariant) and `ValidateGuestName`: 2-24 characters, only letters, digits, space and `_ - . '`, no control characters, not equal (case-insensitive) to a registered username (`IUserRepository.GetUserByUserName`). | Unit tests with Moq cover each rule, including a registered username and a name of 25 characters. | ✅ | 2026-10-16 |
| TASK-0109 | Add `IJoinAttemptThrottle` (singleton) with `IsBlocked(ip, code)`, `RegisterFailure(ip, code)`, `RegisterSuccess(ip, code)`: 5 failures per 15 minutes per (IP, code), uses `TimeProvider`, removes expired entries. | Unit tests with a small fake `TimeProvider`: blocked after 5 failures, unblocked after 15 minutes, success clears, other codes unaffected. | ✅ | 2026-10-16 |
| TASK-0110 | Add `CreateDraftRoomCommand(CreateDraftRoomRequest, DraftRoomCaller)` and its handler in `Business/Logic/DraftRooms/`. `DraftRoomCaller` carries `UserId`, `UserName`, `GuestParticipantId`, `GuestSecret`. Account: name = `UserName`, at most 5 open rooms. Guest: validate name, generate secret. Hash password, generate a unique code (retry up to 5 times), create room (Lobby, `Version` 1) and host participant. Return `DraftRoomEntryDto`, `GuestSecret` only for guests. Preserve full account usernames by sizing participant display-name columns for Identity's 256-character limit; guest validation remains 2-24 characters. | Unit tests (Moq repository): guest and account happy paths, room limit, code collision retries, invalid guest name. Integration test persists a 256-character account display name; handler and full solution tests pass. | ✅ | 2026-10-16 |
| TASK-0111 | Add `JoinDraftRoomRequest` and `JoinDraftRoomCommand(JoinDraftRoomRequest, DraftRoomCaller, string ClientIp)` with its handler. Order: throttle check, find room by code, known identity re-entry without password (account by `UserId`, guest by `ParticipantId` plus verified secret), otherwise verify password (dummy hash when the room is missing), kicked identity refused, room cap, name rules, create participant, success clears the throttle, failure registers it. Unknown code and wrong password return the same `DraftRoomErrorCode.NotFoundOrWrongPassword`. Account name collisions get a numeric suffix; participant display-name columns accommodate the full Identity username plus a two-digit suffix. Add repository operations to create/reactivate participants while incrementing room version and activity atomically. | Unit tests: wrong code equals wrong password, blocked attempt, account and guest re-entry without password, kicked refusal, duplicate guest name, full room, numeric account-name suffix, guest creation and secret. SQL Server LocalDB repository tests verify new joins and re-entry update status, `LastSeenUtc`, room version, and activity. | ✅ | 2026-10-16 |

**Checkpoint:** all business rules are unit tested without a controller.

### Implementation Phase 3

- GOAL-0103: API endpoints and operational safety

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0112 | Apply data annotations built from `DraftRoomLimits` to both request contracts. Create requires a room name and password; join requires an exact-length room code but keeps password and display name optional for account re-entry; provided passwords and display names must respect their limits. | Unit tests with `Validator.TryValidateObject` verify valid requests, required values, optional re-entry fields, and too-short/too-long values for both request types. | ✅ | 2026-10-16 |
| TASK-0113 | Add `API/Helpers/DraftRoomCallerResolver.cs` (account from `ClaimTypes.NameIdentifier` and name claim, guest from headers `X-Draft-Participant-Id` and `X-Draft-Participant-Secret`) and `API/Controllers/DraftRoomsController.cs` (`[Route("api/draft-rooms")]`, `[ApiController]`, anonymous) with `POST api/draft-rooms` and `POST api/draft-rooms/join`. Map failures to RFC 9457 Problem Details: not found or wrong password 404, throttled 429, validation 400, conflict 409, kicked participant 403; room-code generation failure is 503. Success returns `Ok`. Add `Cache-Control: no-store`. | API Release build succeeds. Unit tests cover caller claims/headers/anonymous guest, invalid identity data, success responses, no-store metadata, and status mappings. | ✅ | 2026-10-16 |
| TASK-0114 | Create limit: `AddRateLimiter` in `API/Services/ServiceCollectionExtensions.cs` with a partitioned fixed-window policy `draft-room-create` (10 per hour per remote IP, status 429), `app.UseRateLimiter()` after `UseRouting`, `[EnableRateLimiting("draft-room-create")]` on create. Then log `Connection.RemoteIpAddress` and `X-Forwarded-For` once on the deployed Development environment. If the logged IP is a proxy address, add `UseForwardedHeaders` (config `ForwardedHeaders:KnownProxies`) before `UseRateLimiter` (risk `RSK-02`). | API Release build succeeds. Still required before release: run the 11th-create request check and verify deployed proxy/client-IP behavior; do not trust forwarded headers until trusted proxies are configured. | | |
| TASK-0115 | Add structured Serilog events in the handlers (`DraftRoomCreated`, `DraftRoomJoined`, `DraftRoomJoinFailed`, `DraftRoomJoinThrottled`) with room id and code only. | Draft Rooms unit tests pass. Still required against an isolated local API: create, join, force five wrong-password attempts, then verify log files contain no password or secret. | | |
| TASK-0116 | Run the manual API script from section 6 against the local API and SQL Server. Record the outcome of each step in the commit message. | Every expected result in the script matches (create, join, re-entry, wrong password, throttling, name rules). | | |

**Phase gate (tick before starting P02):**

- [ ] `dotnet build TwilightImperiumUltimate.slnx --configuration Release` shows no new warnings in touched projects
- [ ] `dotnet test TwilightImperiumUltimate.slnx` is green
- [ ] `dotnet format TwilightImperiumUltimate.slnx --verify-no-changes` has no new findings in touched files
- [ ] Migration applied, rollback to 0 and re-apply works
- [ ] Manual API script passes (SCN-04, SCN-07 at API level)
- [ ] No password or secret in the log file

## 3. Alternatives

- **ALT-0101**: A dedicated `DraftRoomsDbContext` with its own migrations (the option I recommended). You chose the main context (`DEC-28`), so P01 uses the shared context and SQL Server LocalDB fixture from P00.
- **ALT-0102**: Use JWT or cookies for guests. Rejected: a per-room secret in a header is simpler and cannot be confused with account authentication.

## 4. Dependencies

- **DEP-0101**: P00 test projects and the `TestDatabase` helper.
- **DEP-0102**: Existing `IUserRepository.GetUserByUserName`, `FluentResults`, MediatR, Serilog, Swashbuckle.

## 5. Files

- **FILE-0101**: `Contracts/Enums/DraftRoomStatus.cs`, `DraftRoomRole.cs`, `DraftParticipantStatus.cs`, `Contracts/DraftRooms/DraftRoomLimits.cs`
- **FILE-0102**: `Core/Entities/DraftRooms/DraftRoom.cs`, `DraftRoomParticipant.cs`
- **FILE-0103**: `DataAccess/Schemas/Schema.cs`, `Tables/TableName.cs`, `Configurations/DraftRooms/*`, `DbContexts/TwilightImperium/TwilightImperiumDbContextDbSets.cs`, `Migrations/*AddDraftRooms*`, `Repositories/IDraftRoomRepository.cs`, `DraftRoomRepository.cs`, `ServiceCollectionExtensions.cs`
- **FILE-0104**: `Business/Services/DraftRooms/*`, `Business/Logic/DraftRooms/*`, `Business/ServiceCollectionExtensions.cs`
- **FILE-0105**: `Contracts/ApiContracts/DraftRooms/*`, `Contracts/DTOs/DraftRooms/DraftRoomEntryDto.cs`
- **FILE-0106**: `API/Controllers/DraftRoomsController.cs`, `API/Helpers/DraftRoomCallerResolver.cs`, `API/Services/ServiceCollectionExtensions.cs`, `API/Program.cs` (rate limiter, forwarded headers)
- **FILE-0107**: `docs/build-and-test.md` and `DataAccess/MigrationCommands.txt` stay unchanged here, the normal EF commands already apply to the main context.

## 6. Testing

- **TEST-0101**: Unit tests in `tests/TwilightImperiumUltimate.Tests/DraftRooms/` for limits, security helpers, name validator, throttle, create and join handlers, resolver, DTO validation.
- **TEST-0102**: Persistence tests in `tests/TwilightImperiumUltimate.IntegrationTests/DraftRooms/` for the model and repository using the temporary SQL Server LocalDB fixture.
- **TEST-0103**: Manual API script (replace the port if yours differs):

```powershell
$api = "https://localhost:7292"
$body = @{ name = "Friday TI"; password = "friday1"; displayName = "Alice" } | ConvertTo-Json
$created = Invoke-RestMethod -Method Post -Uri "$api/api/draft-rooms" -ContentType "application/json" -Body $body
$created         # roomId, code, participantId, guestSecret (shown only now), role = Host

$join = @{ code = $created.code; password = "friday1"; displayName = "Bob" } | ConvertTo-Json
Invoke-RestMethod -Method Post -Uri "$api/api/draft-rooms/join" -ContentType "application/json" -Body $join   # Bob, role Member

$headers = @{ "X-Draft-Participant-Id" = $created.participantId; "X-Draft-Participant-Secret" = $created.guestSecret }
Invoke-RestMethod -Method Post -Uri "$api/api/draft-rooms/join" -ContentType "application/json" -Headers $headers -Body (@{ code = $created.code } | ConvertTo-Json)   # Alice re-enters without a password

$bad = @{ code = $created.code; password = "wrong"; displayName = "Eve" } | ConvertTo-Json
try { Invoke-RestMethod -Method Post -Uri "$api/api/draft-rooms/join" -ContentType "application/json" -Body $bad } catch { $_.Exception.Response.StatusCode }   # 404, same for an unknown code
```

Expected: five wrong attempts in a row make the next attempt answer 429. A second guest named `alice` is refused.

## 7. Risks & Assumptions

- **RISK-0101**: LocalDB verifies the SQL Server provider, but does not replace a deployment-environment migration check. Apply, inspect, and roll back the migration on the target SQL Server instance in TASK-0105.
- **RISK-0102**: `EnableSensitiveDataLogging()` is on in the existing context registration, so EF error messages can contain parameter values such as hashes or chat text. Handlers must not log whole entities, and the log check in TASK-0115 includes one forced failure.
- **RISK-0103**: IP throttling is useless behind a proxy that hides client IPs. TASK-0114 checks it on the deployed environment.
- **ASSUMPTION-0101**: Create returns HTTP 200 (like other POST endpoints in this repository), not 201.

## 8. Related Specifications / Further Reading

- [Specification: sections 6.2, 6.3, 9, 10, 11](../docs/draft-rooms/specification.md)
- [Decisions DEC-28, ASM-01 and risks RSK-01, RSK-02](../docs/draft-rooms/specification.md)
