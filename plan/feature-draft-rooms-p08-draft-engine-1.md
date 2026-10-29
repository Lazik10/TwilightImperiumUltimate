---
goal: Draft Rooms P08 - Draft engine core and Faction Pick settings, start and reset
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-17
owner: Lazik
status: 'In progress'
tags: [feature, draft-rooms, engine, draft-types, faction-pick]
---

# Introduction

![Status: In progress](https://img.shields.io/badge/status-In_progress-yellow)

Adds the pluggable draft engine (interfaces, registry, randomness), the room-level lifecycle (choose draft type, start, reset, lock) and the Faction Pick Draft **settings** with their UI. Nobody can ban or pick yet. Specification: `ENG-001..003`, `ENG-008..010`, `HST-005`, `HST-006`, `FPD-001..003`, `SET-005`, section 7 and section 8.1.

Needs: P07 automated validation passed. On 2026-10-17 the user explicitly deferred manual acceptance until implementation is finished; manual checks remain unchecked, but do not block implementation of subsequent phases.

## 1. Requirements & Constraints

- **REQ-0801**: The room core stays draft-type-agnostic (`ENG-001`). Adding a draft type later must not change room commands.
- **REQ-0802**: The engine classes in `TwilightImperiumUltimate.Draft` are pure: no EF Core, no HTTP. They work on plain state objects. Persistence for Faction Pick lives in `DataAccess` and `Business`.
- **REQ-0803**: Dependency direction stays as in `docs/architecture.md`: Web to Contracts, API to Business and Contracts, Business to DataAccess and Draft, Draft to Core and DataAccess.
- **SEC-0801**: Randomness comes from a CSPRNG behind `IRandomnessSource` and is written to the log (`ENG-007`).
- **SEC-0802**: Settings are validated on the server. Client validation is convenience only.
- **CON-0801**: Seat count, draft type and settings are locked after start (`ENG-009`, `SET-005`). Reset unlocks them.
- **GUD-0801**: The `HostDefined` pick-order option is visible but disabled until P10 (TASK-1003).

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-0801: Engine foundations

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0801 | Re-validate this plan: read specification sections 7 and 8 and the code merged in P01-P07. Adjust task descriptions and file names here where the real code differs. Add a dated bullet to section 7 of this file listing the changes, or stating "no changes". No behaviour change. | The commit shows the edited plan or the "no changes" note. | x | 2026-10-17 |
| TASK-0802 | Add `DraftTypeId` (FactionPick) in `Contracts/Enums/`. Add abstract polymorphic DTO bases `DraftSettingsDto`, `DraftViewDto`, `DraftActionDto` (`[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]`, one `[JsonDerivedType]` per concrete payload) in `Contracts/DTOs/DraftRooms/` and the first derived `FactionPickSettingsDto`. Enums inside them serialize as strings. Register the initial phase view and the four P09 ban-action payloads now so every base has usable concrete types; P09 implements the action behavior and personalized view fields. | Tests serialise and deserialise every derived DTO with the API options and with default `HttpClient` JSON options. An unknown `type` fails with a clear error. | x | 2026-10-17 |
| TASK-0803 | Add the engine contracts in `Draft/DraftRooms/Engine/`: `IDraftType<TSettings, TState>` (`Id`, `MinSeats`, `MaxSeats`, `CreateDefaultSettings`, `ValidateSettings`, `Start`, `Apply`, `DecideFor`, `Undo`, `Project`, `BuildResult`, `FormatResultAsText`), a non-generic `IDraftTypeModule` adapter with checked state/settings dispatch for the registry, and the value types `DraftPhaseKind`, `DraftVisibility`, `DraftViewer` (participant id, seat id, role), `DraftActor`, `DraftTransition` (new state, log events, disclosures), `DraftValidationResult`. Use `IDraftState` for plain state. Validation accepts seat count and a pool size preloaded by Business; Start accepts preloaded typed state and randomness. BuildResult returns explicitly public JSON suitable for the future versioned result snapshot. Pure code only. | A `FakeDraftType` in `tests/TwilightImperiumUltimate.Tests/DraftRooms/Engine/` proves the contract: a seat-private field is absent from every other viewer's projection. | x | 2026-10-17 |
| TASK-0804 | Add `IDraftTypeRegistry` (`Get(DraftTypeId)`, `All`) and register it in `Draft/ServiceCollectionExtensions.cs` (`RegisterDraftServices`). Concrete production modules are registered when implemented in TASK-0811, never test fakes. Reject duplicate and unknown module identifiers. | Unit tests: known type resolves, unknown or unregistered type fails clearly, duplicate registrations fail, DI resolves registry and randomness. | x | 2026-10-17 |
| TASK-0805 | Add `IRandomnessSource` and `CryptoRandomnessSource` (`NextInt`, `Shuffle`) in `Draft/DraftRooms/Engine/`, plus a deterministic fake for tests. | A test shuffles 3 items 6,000 times and every permutation appears within a generous tolerance. | x | 2026-10-17 |

**Checkpoint:** engine contracts and registry exist and are tested with a fake type.

### Implementation Phase 2

- GOAL-0802: Room lifecycle

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0806 | Reuse the existing string column `DraftRooms.DraftType`; migrate empty existing values to `FactionPick` and set the database default in migration `AddDraftType`. Backfill missing Faction Pick settings, preserving customized settings and converting earlier numeric game-version collections to their string names. Set `FactionPick` and stage default settings atomically on room creation. Add `ChangeDraftTypeCommand(code, type, caller)` (Lobby only, host or co-host): remove the old type's persisted settings and runtime rows through a Business-layer persistence adapter using the current room transaction, create the new type's defaults, append a log entry. Selecting the current type is a no-op, not a settings reset. Keep the engine pure; persistence methods do not belong on the engine module. | Nine creation unit tests and 36 focused LocalDB tests passed: allowed in Lobby only, role/secret checks, unknown-type refusal, adapter cleanup, default settings on room creation, migration backfill and rollback/reapply on isolated databases, settings preservation. EF reports no pending model changes. | x | 2026-10-17 |
| TASK-0807 | Add `StartDraftCommand` and `ResetDraftCommand`. Start: Lobby only, every seat claimed by an active participant, seats at least the module `MinSeats`, settings valid, module `Start` (randomisation, the pick order is stored in `FactionPickTurnOrder` and written to the log, the initial phase is stored in `FactionPickState`: Bans when bans mode is not None, otherwise Picks), status InProgress, `Version` plus one, log "draft started". Reset: InProgress only, Business persistence adapter `DeleteStateAsync` (removes state and turn order, keeps settings), status Lobby, log "draft reset". Reuse `DraftRoomStartRules` for shared blocker evaluation. | 85 focused unit tests and 42 focused LocalDB tests passed across lifecycle rules, engine, authorization, creation and type selection. Includes refusal codes, all initial phases, actual order in log, preserved participants/seats/settings/chat on reset, restart after reset, missing room and invalid credentials. Targeted formatting passed. | x | 2026-10-17 |
| TASK-0808 | Extend `DraftRoomViewDto` with `DraftType`, `Settings` (`DraftSettingsDto`), `Draft` (`DraftViewDto`, null in Lobby), `CanStart`, `StartBlockers` (codes such as `SeatsNotFull`, `NotEnoughSeats`, `SettingsInvalid`) and `CanReset`. The draft part comes only from the module `Project(state, viewer)`. Load room/settings/state/order in one query so the projection matches its version. Add navigation metadata migration (no schema changes). | 14 unit tests passed, including host/co-host/member/spectator permissions and lifecycle states. The reflection test from P03 passes. 44 repository/lifecycle/hub integration tests passed. | x | 2026-10-17 |
| TASK-0809 | Lock seat count after start: `AddSeat` and `RemoveSeat` refuse outside Lobby. Implement clarified `SET-005`: persist each participant's draft seat history and reclaim eligibility; allow release/reclaim of that same vacant seat; replacement permanently revokes the former occupant's eligibility, even if the replacement later releases. Allow manager assignment subject to the same eligibility rules. Centralize `IDraftSeatOccupantChangeHandler` in the DataAccess unit-of-work boundary (Business implements it), invoked transactionally on every running occupant change including kick. Hooks must be singleton/stateless because the room-gate unit of work is singleton. P09 adds hidden-selection cleanup. Expose per-seat claim/release/assign permissions and assignment API. | 52 authorization unit tests and 57 focused LocalDB tests passed: structural locks, release/reclaim, replacement revocation, no switching, manager assignment, exactly one winner in a replacement race, hooks, and existing participant/unit-of-work regressions. | x | 2026-10-17 |

**Checkpoint:** a room can change type, start, reset, and refuses structural edits while running.

### Implementation Phase 3

- GOAL-0803: Faction Pick settings

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0810 | Before TASK-0806/0807, add entities `FactionPickSettings` (`RoomId` PK and FK cascade, `GameVersions` persisted as a collection of existing `GameVersion` values, `BansMode`, `BansPerPlayer`, `HostBanCount`, `PickOrderMode`, `PicksPerPlayer`, `Snake`), `FactionPickState` (`RoomId` PK and FK cascade, `Phase` Bans, Picks or Finished, `TurnIndex`, `BansRevealed`) and `FactionPickTurnEntry` (`RoomId`, `Position`, `SeatId`), their configurations (schema `Draft`), DbSets under `// Draft rooms`, migration `AddFactionPickSettingsAndState` and `IFactionPickRepository` in `DataAccess/Repositories/` (load, save and delete of settings, state and turn order using the current room transaction). Do not mark the existing sequential `GameVersion` enum as flags or change its serialized values. Reset removes runtime state and turn order but preserves settings; changing type removes settings too. | Nine SQL Server LocalDB tests passed: defaults, settings/state update, transaction rollback, reset preservation, type-change cleanup, room cascade, cross-room seat rejection, duplicate-seat rejection, migration rollback/reapply on an isolated test database. Targeted formatting passed. | x | 2026-10-17 |
| TASK-0811 | Add and register the pure module `FactionPickDraftType` in `Draft/DraftRooms/FactionPick/` with `ValidateSettings(settings, seatCount, poolSize)` and a Business-layer `FactionPickPoolProvider` (uses `IFactionRepository.GetFactionsByGameVersions` outside the pure engine). Rules from spec 8.1: at least one supported game version, bans per player 1-3, picks per player 1-5, pool after the maximum possible bans must cover seats times picks. Return specific error codes. Initialize the phase and random seat order with an explicit public log event. Until P09/P10 implement phase actions, Apply/DecideFor refuse without effects; result building is unavailable until P11. HostDefined order remains blocked until P10. | 44 focused tests passed across settings/start boundaries, pool provider, registry and JSON contracts. Includes pool one faction too small, bans per player 0 and 4, picks per player 6, no game version, invalid enums, overflow-safe counts, actual random-order log, duplicate factions/seats and repeated start. Targeted formatting passed. | x | 2026-10-17 |
| TASK-0812 | Add `UpdateDraftSettingsCommand` and endpoints `PUT api/draft-rooms/{code}/draft/settings` (polymorphic body), `POST .../draft/start`, `POST .../draft/reset`, and let `PUT .../settings` carry optional `DraftType` atomically. Authorise with `IDraftRoomAuthorizer`. Validate the pool for the proposed expansions, not the previous settings. Validation errors return 400 with the list of codes while retaining the existing singular `code`. | 50 controller tests and 42 lifecycle/settings/type-selection integration tests passed. Includes 400/403/409 mapping, typed payload dispatch, role/status restrictions, persistence, and refusal without overwriting old settings when the proposed pool is too small. | x | 2026-10-17 |
| TASK-0813 | Add `IDraftTypeUiRegistry`, `DynamicComponent` and `FactionPickSettingsEditor` with responsive controls. Add `DraftRoomDraftPanel` in the seats panel so rules remain visible to all roles and on mobile, rather than hiding rules in the manager-only room settings tab. The single supported draft type is shown as a disabled dropdown; HostDefined remains visibly unavailable until P10. Add Start blockers, confirmed Reset, locked rules, typed client APIs, localized engine log messages including randomized seat order, and server-filtered manager assignment choices. Extend `GameVersionPicker` with opt-in accessible multi-selection, preserving existing icon-picker callers. | Full solution: 377 unit passed, 3 skipped; 141 LocalDB integration passed; 203 component passed. Includes member/running read-only rules, blockers, failed Start, confirmed Reset, parameter copying, live edits, eligible assignment, per-seat running permissions, typed settings and credential headers. Web build and targeted formatting passed without warnings. Manual SCN-50/51 deferred by user, not passed. | x | 2026-10-17 |

**Phase gate (tick before starting P09):**

- [ ] SCN-50 settings edit live for others (read-only), invalid settings block Start with a reason, Start needs all seats, after Start type, settings and seat count are locked, Reset unlocks them
- [ ] SCN-51 random pick order is announced in the log (the order is visible, nothing is picked yet)
- [ ] The reflection test (no secret members in DTOs) still passes
- [x] Migrations `AddDraftType` and `AddFactionPickSettingsAndState` applied and rolled back once on isolated LocalDB databases
- [ ] Build, tests, Release build and `dotnet format --verify-no-changes` clean for touched files

## 3. Alternatives

- **ALT-0801**: Store settings as JSON in `DraftRooms`. Rejected: you asked for dedicated tables per draft type.
- **ALT-0802**: One shared "draft state" table with JSON. Rejected for the same reason, and typed tables are easier to test and migrate.

## 4. Dependencies

- **DEP-0801**: `IFactionRepository.GetFactionsByGameVersions`, existing `GameVersion` enum and `GameVersionPicker`.

## 5. Files

- **FILE-0801**: `Contracts/Enums/DraftTypeId.cs`, `Contracts/DTOs/DraftRooms/DraftSettingsDto.cs`, `DraftViewDto.cs`, `DraftActionDto.cs`, `FactionPickSettingsDto.cs`
- **FILE-0802**: `Draft/DraftRooms/Engine/*`, `Draft/DraftRooms/FactionPick/*`, `Draft/ServiceCollectionExtensions.cs`
- **FILE-0803**: `Core/Entities/DraftRooms/FactionPickSettings.cs`, `FactionPickState.cs`, `FactionPickTurnEntry.cs`, `DataAccess/Configurations/DraftRooms/*`, `Migrations/*AddDraftType*`, `Migrations/*AddFactionPickSettingsAndState*`, `Repositories/IFactionPickRepository.cs`
- **FILE-0804**: `Business/Logic/DraftRooms/ChangeDraftType*`, `StartDraft*`, `ResetDraft*`, `UpdateDraftSettings*`
- **FILE-0805**: `API/Controllers/DraftRoomsController.cs`
- **FILE-0806**: `Web/Services/DraftRooms/IDraftTypeUiRegistry.cs`, `Web/Components/DraftRooms/FactionPick/FactionPickSettingsEditor.*`, `DraftRoomHostSettings.*`

## 6. Testing

- **TEST-0801**: Unit tests for the fake draft type contract, registry, randomness, settings validation, start and reset reasons.
- **TEST-0802**: SQL Server LocalDB tests for the new tables and the lifecycle commands.
- **TEST-0803**: bUnit tests for the settings editor and start button states.
- **TEST-0804**: Manual SCN-50 and SCN-51.

## 7. Risks & Assumptions

- Re-validation 2026-10-17 (TASK-0801): `DraftRoom.DraftType` already exists and room creation currently uses an empty string; reuse and backfill it rather than add a duplicate column. `GameVersion` is not a flags enum; represent enabled versions as a collection without changing its existing values. Preserve the user's clarified post-start release/reclaim/replacement behavior from specification SET-005 instead of the stale self-seat lock instruction. Keep persistence in the current Business/DataAccess transaction, outside pure engine interfaces. Build storage and settings validation before lifecycle commands that depend on them. Manual acceptance gates are deferred by the user's explicit instruction, not marked passed.
- Validation 2026-10-17: Release Web publish succeeded after the polymorphic contract changes. The SDK reported publishing without WebAssembly native optimizations because `wasm-tools` is not installed; browser round-trip checks remain deferred, not claimed as passed.
- **RISK-0801**: The polymorphic JSON discriminator can break with trimming in the WASM publish. Check `dotnet publish src/TwilightImperiumUltimate.Web -c Release` and a round trip in the browser after TASK-0802.
- **RISK-0802**: The engine contract may need a small change when bans are added in P09. Keep changes inside `Draft/DraftRooms/Engine/` and update the fake type.
- **ASSUMPTION-0801**: The minimum seats for Faction Pick equal the room minimum of 2.

## 8. Related Specifications / Further Reading

- [Specification: 6.8, 6.9, 7, 8.1, 9.3](../docs/draft-rooms/specification.md)
- [Decisions DEC-06, DEC-07, DEC-13, DEC-27](../docs/draft-rooms/specification.md)
