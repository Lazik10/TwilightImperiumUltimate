---
goal: Draft Rooms P03 - Seats and room management (backend)
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-15
owner: Lazik
status: 'Planned'
tags: [feature, draft-rooms, backend, api, seats]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

Adds seats and every host or member management command on the server, the per-room transaction pipeline that all later phases reuse, the authorisation rules, and the personalised room view endpoint. No UI yet, you verify with tests and an API script. Specification: `SET-001..004`, `SET-007`, `SET-008`, `HST-001..004`, `HST-009`, `ROM-006`, `ROM-007`, `ROM-009`, `SCR-004`.

Needs: P02 passed (MVP).

## 1. Requirements & Constraints

- **REQ-0301**: Implement the seat and host rules named above. Rules that only matter after the draft has started (`SET-005`, `SET-006`) are prepared as state checks here and finished in P09 and P10.
- **REQ-0302**: Every state-changing command runs through `IDraftRoomUnitOfWork`: per-room gate, one transaction, `Version` plus one, `LastActivityUtc` updated (`ROM-011`).
- **REQ-0303**: The view DTO never contains a secret, a hash or a password member.
- **SEC-0301**: Authorisation decisions live only in `IDraftRoomAuthorizer`. Controllers and handlers never compare roles by hand (`SCR-004`).
- **SEC-0302**: A caller who is not a participant gets 404 for every endpoint except create and join, so rooms cannot be probed.
- **CON-0301**: Same conventions as P01 (main `TwilightImperiumDbContext` through `IDbContextFactory`, schema `Draft`, StyleCop order, `CancellationToken`).
- **PAT-0301**: One command plus handler per use case in `Business/Logic/DraftRooms/`.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-0301: Seat table, transaction pipeline, authorisation

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0301 | Add `DraftRoomSeat` in `Core/Entities/DraftRooms/` (`Id` Guid, `RoomId`, `Index`, `ParticipantId` null, `ClaimedAtUtc` null) and `DraftRoom.Seats`. Add `DraftRoomSeatConfiguration` (schema `Draft`, unique (`RoomId`,`Index`), filtered unique (`RoomId`,`ParticipantId`) where `[ParticipantId] IS NOT NULL`, participant relationship uses client-side nulling, room FK cascade), the DbSet `DraftRoomSeats` under `// Draft rooms` in `TwilightImperiumDbContextDbSets.cs`, and `TableName.DraftRoomSeats`. SQL Server cannot create both the room-to-seat cascade and participant-to-seat `ON DELETE SET NULL` because that creates multiple cascade paths; use `ClientSetNull` and keep the participant and seat tracked when deleting a participant. Create migration `AddDraftRoomSeats` with the normal EF commands from `docs/build-and-test.md`. | Persistence tests on `TestDatabase` verify uniqueness, unclaimed seats, participant deletion unclaiming a tracked seat, and room deletion cascading to occupied seats. Apply the migration to SQL Server and see the table, then roll it back one step and re-apply. | Yes | 2026-10-16 |
| TASK-0302 | Add `IDraftRoomUnitOfWork` and `DraftRoomUnitOfWork` in `DataAccess/`: `Task<TResult> ExecuteAsync<TResult>(string code, Func<DraftRoomScope, Task<TResult>> work, CancellationToken)`. It takes a per-room `SemaphoreSlim` from a `ConcurrentDictionary<Guid, SemaphoreSlim>` (valid for one API instance, `DEC-08`), opens a context from `IDbContextFactory<TwilightImperiumDbContext>` and sets `ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll` (the application default is `NoTracking`), loads the room with participants and seats, runs `work`, and, only when tracked state changed, sets `LastActivityUtc`, increments `Version`, and saves in one transaction. `DraftRoomScope` exposes `Room`, `Context`, `Now`. A `DbUpdateConcurrencyException` becomes `DraftRoomConflictException`. Register the unit of work as a singleton: the keyed gates must be shared across request scopes, and the service keeps no context between calls. | SQL Server LocalDB integration tests: two parallel state-changing calls on one room never overlap and each increments `Version` once; a no-op leaves room version/activity unchanged; a stale write throws `DraftRoomConflictException`. | Yes | 2026-10-16 |
| TASK-0303 | Add `IDraftRoomAuthorizer` and `DraftRoomAuthorizer` in `Business/Services/DraftRooms/`. `Resolve(room, caller)` returns a `DraftRoomActor` (participant, seat, role) or Forbidden. Guests are verified with `IGuestSecretService.Verify`. Kicked and Left participants cannot act. Policy methods mirror spec section 4: `CanManageSeats`, `CanChangeSettings`, `CanKick(target)`, `CanChangeRoles`, `CanClose`, `CanClaimSeat`, `CanUnclaimSeat(target)`. Actor construction is restricted to the authorizer so callers cannot forge active identities. | Unit tests cover account and guest identity resolution, invalid/expired participant states, role hierarchy, lobby-only permissions, target room membership, and seat ownership. | Yes | 2026-10-16 |

**Checkpoint:** the pipeline and the permission matrix are tested.

### Implementation Phase 2

- GOAL-0302: Commands and the view query

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0304 | Update `CreateDraftRoomCommandHandler`: create `DraftRoomLimits.DefaultSeatCount` (6) seats and seat the host on Seat 1 in the same transaction (`SET-001`, `SET-002`). Update the P01 tests. | Unit tests updated and green. SQL Server LocalDB test: a new room has 6 seats and Seat 1 holds the host. | Yes | 2026-10-16 |
| TASK-0305 | Add `AddSeatCommand` and `RemoveSeatCommand(code, seatId)` with handlers. Only host or co-host, only in Lobby, seat count stays 2-8, a seat can be removed only when it is free, the remaining seats are renumbered 1..N in their order. | SQL Server LocalDB tests: adding at 8 refused and below 8 appends, removing at 2 refused, occupied seat refused, caller without right refused, and renumbering preserves the original order. | Yes | 2026-10-16 |
| TASK-0306 | Add `ClaimSeatCommand(code, seatId)` and `UnclaimSeatCommand(code, seatId)`. Self-service only in Lobby, claiming moves a participant who already holds a seat, host or co-host can unclaim any seat in any state. Claims run inside the unit of work (`SET-008`). | SQL Server LocalDB tests: self-claim/move and unclaim permissions are enforced, managers can unclaim in progress, and eight participants claiming one seat in parallel produce exactly one success and seven conflicts. | Yes | 2026-10-16 |
| TASK-0307 | Add `LeaveRoomCommand`, `KickParticipantCommand(code, participantId)` and `SetParticipantRoleCommand(code, participantId, role)`. Leave in Lobby frees the seat and sets Status Left (joining again sets Active). Kick sets Kicked, frees the seat, and the identity cannot rejoin (`HST-002`). A co-host cannot kick the host or other co-hosts. Only the host changes roles. | SQL Server LocalDB tests verify leave/kick state and seat release, co-host kick restrictions, host-only role changes, and invalid role rejection. Existing join tests cover Left re-entry and kicked identity denial. | Yes | 2026-10-16 |
| TASK-0308 | Add `UpdateRoomSettingsCommand` (room name 3-60, `IsLocked`), `ChangeRoomPasswordCommand` (validates and rehashes, members unaffected, `ROM-006`) and `CloseRoomCommand` (hard delete with cascade). Host and co-host only. | SQL Server LocalDB tests cover settings validation/authorization, password replacement without changing existing participants, and room closure cascading to participants and seats. | Yes | 2026-10-16 |
| TASK-0309 | Add `GetDraftRoomViewQuery(code, caller)` and `DraftRoomViewDto` in `Contracts/DTOs/DraftRooms/`: `RoomId`, `Code`, `Name`, `Status`, `IsLocked`, `Version`, `Participants` (Active only: `Id`, `DisplayName`, `Role`, `IsGuest`, `SeatId`), `Seats` (`Id`, `Index`, `ParticipantId`), `Me` (`ParticipantId`, `Role`, `SeatId`, flags `CanManageSeats`, `CanChangeSettings`, `CanKick`, `CanChangeRoles`, `CanClose`, `CanClaimSeat`). A stranger gets "not found". | Unit tests cover host, guest member, and stranger views; a repository integration test verifies participants and seats are loaded. Reflection verifies that the view DTOs do not expose properties named like `*Secret*`, `*Hash*` or `Password*`. `DraftRoomEntryDto.GuestSecret` remains intentionally limited to the create/join credential response. | Yes | 2026-10-16 |

**Checkpoint:** all commands and the view are unit and integration tested.

### Implementation Phase 3

- GOAL-0303: API surface

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0310 | Add annotated body request contracts in `Contracts/ApiContracts/DraftRooms/` for P03 commands that accept a body (`SetParticipantRoleRequest`, `UpdateRoomSettingsRequest`, `ChangeRoomPasswordRequest`). Existing `DraftRoomEntryDto` and `DraftRoomViewDto` serve as the create/join and room-view response contracts; the other commands have no response payload. | `Validator.TryValidateObject` unit tests cover valid values, missing required values, invalid enum values, and minimum/maximum limits. | Yes | 2026-10-16 |
| TASK-0311 | Add the endpoints of spec section 10 that belong to P03 to `DraftRoomsController`: `GET {code}`, seats add and remove, claim, unclaim, leave, kick, role, settings, password, close. Each action resolves the caller, sends one mediator request and maps typed failures to Problem Details. Successful reads return the personalized view; successful mutations return no content. | Controller tests enumerate all route metadata, verify request/caller forwarding and success responses, and cover 400/403/404/409 mappings. Release API build confirms endpoint metadata compiles. | Yes | 2026-10-16 |
| TASK-0312 | Ensure `JoinDraftRoomCommandHandler` refuses new participants when `IsLocked` (`ROM-009`), keeps refusing kicked identities, enforces 32 active participants, and reactivates a participant joining again as `Left`. Existing handler logic already implements these rules; add explicit regression coverage that a known Left guest can re-enter a locked room without a password. | Unit tests cover locked-room re-entry, locked-room rejection for new participants, Left reactivation, kicked identity denial, and refusal at the 33-participant limit. | Yes | 2026-10-16 |
| TASK-0313 | Run the API script from section 6 (two guests, optionally one account). Record the result of each step in the commit message. | Every expected status in the script matches. | Yes | 2026-10-16 |

**Phase gate (tick before starting P04):**

- [x] `dotnet build TwilightImperiumUltimate.slnx --configuration Release` shows no new warnings in touched projects
- [x] `dotnet test TwilightImperiumUltimate.slnx` is green
- [x] `dotnet format TwilightImperiumUltimate.slnx --verify-no-changes` has no new findings in touched files
- [x] Migration `AddDraftRoomSeats` applied and rolled back once
- [x] Parallel claim test passes (exactly one winner)
- [x] API script passes for SCN-10 to SCN-17 at API level

The Release build reports eight existing analyzer warnings in `ITwilightImperiumApiHttpClient.cs` and `TwilightImperiumApiHttpClientHeaderTests.cs`; it reports none in the Draft Rooms implementation files.

## 3. Alternatives

- **ALT-0301**: Optimistic concurrency only, no per-room gate. Rejected: under simultaneous clicks it produces many avoidable conflicts. The gate is cheap with one API instance.
- **ALT-0302**: Let each endpoint check roles itself. Rejected: one authoriser keeps the rules testable in one place.

## 4. Dependencies

- **DEP-0301**: P01 repository, security helpers, `DraftRoomCaller`. P02 is not required by the code, only by your verification order.

## 5. Files

- **FILE-0301**: `Core/Entities/DraftRooms/DraftRoomSeat.cs`
- **FILE-0302**: `DataAccess/Configurations/DraftRooms/DraftRoomSeatConfiguration.cs`, `DataAccess/Migrations/*AddDraftRoomSeats*`, `DataAccess/DraftRoomUnitOfWork.cs`
- **FILE-0303**: `Business/Services/DraftRooms/DraftRoomAuthorizer.cs` and interface, `Business/Logic/DraftRooms/*Seat*`, `*Leave*`, `*Kick*`, `*Role*`, `*Settings*`, `*Password*`, `*Close*`, `GetDraftRoomViewQuery*`
- **FILE-0304**: `Contracts/DTOs/DraftRooms/DraftRoomViewDto.cs`, `Contracts/ApiContracts/DraftRooms/*`
- **FILE-0305**: `API/Controllers/DraftRoomsController.cs`

## 6. Testing

- **TEST-0301**: Unit tests for the authoriser matrix, every command, the view query, the DTO reflection test.
- **TEST-0302**: SQL Server LocalDB integration tests for seat constraints, parallel claims, the unit of work, cascade on close.
- **TEST-0303**: API script. First run the P01 create/join steps using `$roomPassword`; `$created` and `$joined` are their direct JSON responses:

```powershell
$alice = @{
    "X-Draft-Participant-Id" = $created.participantId
    "X-Draft-Participant-Secret" = $created.guestSecret
}
$bob = @{
    "X-Draft-Participant-Id" = $joined.participantId
    "X-Draft-Participant-Secret" = $joined.guestSecret
}

function Invoke-ExpectedStatus {
    param (
        [string]$Method,
        [string]$Uri,
        [hashtable]$Headers,
        [int]$ExpectedStatus,
        [string]$Body
    )

    $request = @{
        Method = $Method
        Uri = $Uri
        Headers = $Headers
        UseBasicParsing = $true
    }
    if ($null -ne $Body) {
        $request.ContentType = "application/json"
        $request.Body = $Body
    }

    try {
        $response = Invoke-WebRequest @request
        $status = [int]$response.StatusCode
        $content = $response.Content
    }
    catch {
        if ($null -eq $_.Exception.Response) {
            throw
        }
        $status = [int]$_.Exception.Response.StatusCode
        $content = $_.ErrorDetails.Message
    }

    if ($status -ne $ExpectedStatus) {
        throw "$Method $Uri returned $status; expected $ExpectedStatus. $content"
    }

    return $content
}

$code = $created.code
$bobId = $joined.participantId
$viewJson = Invoke-ExpectedStatus GET "$api/api/draft-rooms/$code" $alice 200
$view = $viewJson | ConvertFrom-Json
$seat2 = $view.seats[1].id
Invoke-ExpectedStatus POST "$api/api/draft-rooms/$code/seats/$seat2/claim" $bob 204
Invoke-ExpectedStatus POST "$api/api/draft-rooms/$code/seats/$seat2/claim" $alice 409
Invoke-ExpectedStatus POST "$api/api/draft-rooms/$code/seats/$seat2/unclaim" $alice 204
Invoke-ExpectedStatus POST "$api/api/draft-rooms/$code/participants/$bobId/kick" $alice 204
$rejoinBody = @{ code = $code; password = $roomPassword } | ConvertTo-Json
Invoke-ExpectedStatus POST "$api/api/draft-rooms/join" $bob 403 $rejoinBody
Invoke-ExpectedStatus DELETE "$api/api/draft-rooms/$code" $alice 204
```

Executed on 2026-10-16 against a fresh isolated LocalDB: create 200, join 200, view 200, claim 204, occupied-seat claim 409, host unclaim 204, kick 204, kicked re-entry 403, and close 204.

## 7. Risks & Assumptions

- **RISK-0301**: A forgotten code path that bypasses the unit of work loses the version increment. The controller tests assert that every mutating endpoint ends in a command that uses it.
- **RISK-0302**: Per-room semaphores accumulate. Remove a gate when its room is closed or deleted (covered by a unit test in TASK-0302).
- **ASSUMPTION-0301**: Assigning a participant to a vacant seat after the draft starts (`ASM-03`) is built in P10, not here.

## 8. Related Specifications / Further Reading

- [Specification: 4 Roles and permissions, 6.4, 6.5, 9.2](../docs/draft-rooms/specification.md)
- [Scenarios SCN-10 to SCN-17](../docs/draft-rooms/specification.md)
