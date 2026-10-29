---
goal: Draft Rooms P09 - Faction Pick bans phase and hidden information
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-15
owner: Lazik
status: 'Implemented - manual acceptance deferred'
tags: [feature, draft-rooms, engine, faction-pick, security, hidden-information]
---

# Introduction

![Status: Implemented](https://img.shields.io/badge/status-Implemented-green)

Builds the first phase of the Faction Pick Draft: **bans** with the three modes (none, host bans, simultaneous secret bans revealed together). This is the first phase with **hidden information**, so it also proves the redaction design, the "host decides for a seat" disclosure rule and the seat-change safety rule. Specification: `FPD-004..006`, `FPD-009`, `FPD-010`, `ENG-004..006`, `HST-008`, `HST-010`, `SET-006`, `SCR-003`, section 7.

Needs: P08 passed. Planned at task level and re-checked against the real code first (TASK-0901).

## 1. Requirements & Constraints

- **REQ-0901**: Implement the three bans modes exactly as `FPD-005` and `FPD-006` state. Bans run before the picks, and a phase without bans (mode None) goes straight to Picks (built in P10).
- **REQ-0902**: Hidden information leaves the server only through `Project(state, viewer)` (`ENG-006`). No endpoint, hub event or log line may carry another seat's unrevealed selection.
- **REQ-0903**: **Decide for** a seat reveals that seat's unlocked selection to everyone in the log before the host's choice is applied (`HST-008`). The host never sees it earlier.
- **REQ-0904**: When the occupant of a seat changes after start, every **unrevealed** selection of that seat (pending, or locked but not yet revealed) is discarded without revealing it, and the seat's lock flag is cleared (`SET-006`).
- **SEC-0901**: There is a test for every viewer class (own seat, other seat, host, co-host, spectator, unseated guest) in every bans state (before any lock, partly locked, after reveal).
- **SEC-0902**: Log and notification payloads for a hidden action contain no faction names until the reveal.
- **CON-0901**: Pending selections are rows with `State = Pending` in `FactionPickSelections`. Locked rows stay hidden until `BansRevealed` is true.
- **GUD-0901**: Reuse the existing faction display components (`Components/Drafts/Faction/*`, `ResponsiveFactionPicker`) for the faction tiles.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-0901: Storage and engine rules for bans

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0901 | Re-validate specification sections 7 and 8 against P08. Use the DataAccess unit-of-work occupant hook, with a singleton/stateless Business implementation, not a Draft-layer I/O service. Runtime state stores the locked seat identifiers independently of choice rows so zero bans can be locked. Decide-for accepts an explicit replacement ban list; it never fetches another seat's choices for the manager UI. | P08 full solution passed: 377 unit, 141 integration, 203 component; manual gates deferred by user. | x | 2026-10-17 |
| TASK-0902 | Add `FactionPickSelection`, typed enums, room/seat constraints, null-inclusive unique index for host and seat choices, transactional repository synchronization, personalized-view loading, and runtime `LockedBanSeatIds` for empty lock-in. Generate `AddFactionPickSelections`. | 14 LocalDB tests passed, including duplicate host/seat rejection, synchronized lock timestamps, reset deletion, room cascade, and migration rollback/reapply preserving rules. Targeted formatting passed. | x | 2026-10-17 |
| TASK-0903 | Implement pure select/deselect/lock/host-confirm rules, pool/count/actor/phase guards, empty lock-in, overlap deduplication, and one last-seat reveal. Add immutable server-only choices and pure decide-for disclosure with explicit manager replacement bans; application wiring follows TASK-0905/0907. | 64 focused unit/contract tests passed: all modes, count/pool/ownership/lock guards, exactly one reveal, overlap, empty lock-in and explicit disclosure-before-lock. Targeted formatting passed. | x | 2026-10-17 |
| TASK-0904 | Project public pool, public host or revealed bans, own-seat bans and independent public lock flags exclusively through the module. Host and co-host receive no extra private data. | All 18 matrix cases passed (six viewer classes times three lock/reveal states), asserting exact public/own faction sets and lock counts. | x | 2026-10-17 |

**Checkpoint:** bans rules and redaction pass their tests with no UI.

### Implementation Phase 2

- GOAL-0902: Decide-for, seat-change safety, API

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0905 | Add `DecideForCommand` and a shared typed action handler. Load and persist choices/locks through the existing room transaction. Persist ordered public effects with actor-name and seat-index snapshots; managers never load private choices into their UI. Pure decide-for emits the disclosure before replacing and locking the choice. | 71 focused unit/controller/serialization tests and 42 lifecycle/hub LocalDB tests passed. Includes member refusal without changes and persisted disclosure-before-lock ordering. Targeted formatting passed. | x | 2026-10-17 |
| TASK-0906 | Register singleton/stateless Business `FactionPickOccupantChangeHandler` and singleton stateless `DraftRoomLog`. Use only the passed transaction scope. Delete unrevealed bans and pending picks, clear the unrevealed lock flag, append a generic seat-only log even when no choices existed. | LocalDB tests passed for pending/locked-unrevealed deletion, lock clearing, revealed-ban preservation and exact content-free log fields. Existing running replacement and hub tests passed with the real hook. | x | 2026-10-17 |
| TASK-0907 | Add authenticated-room `POST .../draft/actions` and manager `POST .../draft/decide-for`, with polymorphic actions including explicit replacement `DecideBansAction`. Invalid actions return 400, permission failures 403, wrong phase/already locked 409. Successful actions increment Version once and notify only after commit. | Controller mapping and exact routes passed; real handler tests cover hidden persistence, own/other projections, simultaneous lock race with one reveal and single version increments. Existing SignalR integration tests passed. | x | 2026-10-17 |

**Checkpoint:** the server side is complete and tested.

### Implementation Phase 3

- GOAL-0903: Board UI, integration tests, acceptance

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0908 | Add registry-driven `FactionPickBoard`, faction-icon toggle grid, public lock progress, host bans, own private selection, confirmed decide-for with an independent replacement selection, typed client APIs and localized public activity text. Keep accessible toggle names stable, announce state separately and restore focus after actions/modal closure. | All 214 component tests passed, including role/lock/reveal states, stable accessible names, explicit disclosure dialog and typed credential headers. Release Web publish and targeted formatting passed without warnings. | x | 2026-10-17 |
| TASK-0909 | Exercise six real SignalR connections: three seated guests, unseated host/co-host and spectator. Assert exact private/public selection fields for each `GET view` and scan every chat/log/version payload for choice data before disclosure. The public pool legitimately contains every faction, so absence of a faction name from the entire view is not a valid secrecy assertion. Test locked-secret discard on host unclaim, reclaim, member decide-for refusal, co-host disclosure received by everyone and final reveal. | Live redaction test passed; full standalone LocalDB suite passed all 153 tests. Full unit suite: 405 passed, 3 skipped. Component suite: 214 passed. The initial overlapping full validation run had 29 database failures; rerunning the complete database suite without concurrent publish/format passed. | x | 2026-10-17 |
| TASK-0910 | Audit the board with `accessibility-audit`: semantic lists/buttons, stable toggle labels with state descriptions, visible non-colour states, explicit focus-visible styling, native modal wrapper and post-action focus restoration. | Code audit and bUnit assertions complete. Browser/keyboard/screen-reader SCN-60/61 remain explicitly deferred by the user's instruction, not passed. | x | 2026-10-17 |

**Phase gate (tick before starting P10):**

- [ ] SCN-60 Alice selects ban X; Bob's, the host's and a spectator's data does not contain X (network tab and the redaction tests agree); lock flags are visible; the reveal happens for everyone at once; decide-for writes the revealed selection into the log
- [ ] SCN-61 host unclaims Alice's seat after start: her unrevealed selection is discarded and not revealed
- [x] Redaction test matrix green (6 viewers times 3 states)
- [x] Migration `AddFactionPickSelections` applied and rolled back once
- [x] Build, tests, Release Web publish and targeted `dotnet format --verify-no-changes` clean for touched files

Manual multi-profile/browser gates remain deferred by the user's instruction; automated completion permits continuing to P10 without claiming those manual checks passed.

## 3. Alternatives

- **ALT-0901**: Send each client a full state and hide fields in the UI. Rejected: anyone can read the network response. The server must never send hidden data.
- **ALT-0902**: Reveal bans one by one as players lock in. Rejected: you asked for simultaneous reveal.

## 4. Dependencies

- **DEP-0901**: P08 engine contracts, `IDraftSeatOccupantChangeHandler`, settings, state and turn-order tables. P05 hub test host.

## 5. Files

- **FILE-0901**: `Core/Entities/DraftRooms/FactionPickSelection.cs`, `DataAccess/Configurations/DraftRooms/*`, `Migrations/*AddFactionPickSelections*`, `Repositories/IFactionPickRepository.cs`
- **FILE-0902**: `Draft/DraftRooms/FactionPick/*` (bans rules, projection, decide-for, occupant handler)
- **FILE-0903**: `Business/Logic/DraftRooms/DecideFor*`, `ApplyDraftAction*`
- **FILE-0904**: `Contracts/DTOs/DraftRooms/FactionPickActions.cs`, `FactionPickViewDto.cs`
- **FILE-0905**: `API/Controllers/DraftRoomsController.cs`
- **FILE-0906**: `Web/Components/DraftRooms/FactionPick/FactionPickBoard.*`
- **FILE-0907**: `tests/TwilightImperiumUltimate.IntegrationTests/Hubs/RedactionTests.cs`

## 6. Testing

- **TEST-0901**: Unit tests for every bans rule, the disclosure and the occupant handler.
- **TEST-0902**: The 6 by 3 redaction matrix.
- **TEST-0903**: Hub-level redaction tests (payload scan).
- **TEST-0904**: bUnit tests for the board. Manual SCN-60 and SCN-61.

## 7. Risks & Assumptions

- **2026-10-17 revalidation**: P08 now provides lifecycle APIs, personalized views, server-filtered replacement choices and the transactional occupant hook. Corrected the hook's registration layer and lifetime. A separate locked-seat collection is required for legal zero-choice lock-in. Existing action DTOs are preserved; a dedicated decide-for ban payload will carry the manager's explicit replacement choices.

- **RISK-0901**: The most dangerous bug in the feature is a leak of unrevealed data. Never add a second code path that builds a view. If one seems necessary, extend `Project` instead and add its test first.
- **RISK-0902**: A seat's next occupant could read the previous occupant's secret choice (`RSK-07`). TASK-0906 and the integration test cover it.
- **ASSUMPTION-0901**: In host-bans mode the host's toggles are public immediately, because nothing is secret there.

## 8. Related Specifications / Further Reading

- [Specification: 6.8 ENG-005, ENG-006, 6.9, 7.2, 7.3, 7.5, 11 SCR-003](../docs/draft-rooms/specification.md)
- [Scenarios SCN-60, SCN-61, risk RSK-07](../docs/draft-rooms/specification.md)
