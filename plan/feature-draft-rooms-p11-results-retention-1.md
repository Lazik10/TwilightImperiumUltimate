---
goal: Draft Rooms P11 - Results, share link, account history and retention
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-17
owner: Lazik
status: 'Implemented - manual acceptance deferred'
tags: [feature, draft-rooms, results, retention, quartz, privacy]
---

# Introduction

![Status: Implemented - manual acceptance deferred](https://img.shields.io/badge/status-Implemented_manual_acceptance_deferred-yellow)

Turns a finished draft into an immutable **result snapshot** that outlives the room: shown read-only in the room, **copied as text**, opened through a **public link**, listed in the **account history**, deletable, and a daily job that deletes rooms inactive for 30 days without touching results. Specification: `RES-001..006`, `ROM-010`, `SCR-009`, decisions `DEC-20`, `DEC-21`, `ASM-16`.

Needs: P10 passed. Planned at task level and re-checked against the real code first (TASK-1101).

Implementation checkmarks below record implemented code and automated verification, not completion of the manual verification steps. SCN-80, actual browser clipboard/paste checks and the accelerated Development cleanup walkthrough remain deferred at the user's request.

## 1. Requirements & Constraints

- **REQ-1101**: Implement `RES-001..006`. A snapshot is created in the same transaction that finishes the draft.
- **REQ-1102**: The snapshot does not depend on the room: no foreign key to the room, the room can be deleted while the snapshot stays.
- **REQ-1103**: A finished room is read-only: every draft command refuses (`ASM-04`).
- **SEC-1101**: The public result contains only display names, seat numbers, bans, picks and "decided by host" markers. It contains no user ids, no participant ids, no chat, no log and nothing hidden (`RES-003`).
- **SEC-1102**: The share token is 128-bit random (16 bytes, base64url). Unknown and deleted tokens answer 404 with the same body.
- **CON-1101**: Closing a room and the cleanup job never delete results (`RES-006`, `ROM-010`).
- **CON-1102**: The result page is `noindex`.
- **GUD-1101**: Configuration keys `DraftRooms:InactiveRoomRetentionDays` (default 30) and `DraftRooms:CleanupCronExpression` (default daily 03:15 UTC).

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-1101: Snapshot storage and creation on finish

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-1101 | Reconcile results with the P10 transition pipeline, room unit of work, personalized projection and account linking. | Revalidation notes below define the atomic snapshot boundary and safe public payload. | x | 2026-10-17 |
| TASK-1102 | Add entities `DraftResult` (`Id`, `ShareToken` unique, `DraftType`, `RoomName`, `SourceRoomId` with no foreign key, `OwnerUserId` null with a foreign key to `AspNetUsers` (set null), `FinishedAtUtc`, `PayloadVersion`, `PayloadJson`, `CreatedAtUtc`) and `DraftResultPlayer` (`ResultId` FK cascade, `SeatIndex`, `DisplayName`, `UserId` null with a foreign key to `AspNetUsers` (set null); key (`ResultId`,`SeatIndex`), index on `UserId`), configurations (schema `Draft`), DbSets under `// Draft rooms`, migration `AddDraftResults` and `IDraftResultRepository`. | SQL Server LocalDB tests pass for uniqueness, case sensitivity, cascade, account deletion, room-independent lifetime and migration rollback/reapply. | x | 2026-10-17 |
| TASK-1103 | Build and stage the public snapshot in the final pick's existing room transaction, generate a random token, set `ResultId`, transition to Finished and append the existing `DraftFinished` event. Finished draft commands remain refused. | Pure payload/privacy/golden-string tests and LocalDB exactly-once/exception rollback tests pass. The unit of work commits on normal return and rolls back on exceptions; it does not inspect FluentResults. | x | 2026-10-17 |

**Checkpoint:** a finished draft always leaves exactly one snapshot.

### Implementation Phase 2

- GOAL-1102: Read, copy, history, delete

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-1104 | Add anonymous token reads, account-only history pages of twenty, verified in-room result reads and manager/owner deletion. Public envelopes exclude private identity and source-room fields. | Controller authentication/problem tests, public contract reflection tests, SQL permission tests and paging tests pass. Invalid/unknown/deleted results have the same not-found code and response shape. | x | 2026-10-17 |
| TASK-1105 | Add registered read-only generic/Faction Pick result views, a finished-room panel and the public result page using the main layout. | bUnit verifies public/room rendering, stale-response rejection, version-triggered refresh and noindex metadata. Manual DevTools check deferred. | x | 2026-10-17 |
| TASK-1106 | Implement the pure text formatter and use `CopyButton` for result text and the absolute share URL in both views. Await real clipboard completion, expose failures and preserve fallback focus. | Golden-string unit test, bUnit pending/failure tests and a Node check of JS asynchronous completion/fallback/failure/focus pass. Actual browser paste check deferred. | x | 2026-10-17 |
| TASK-1107 | Add account-only "My drafts" with bounded paging and public links. Link guest-account history/ownership without rewriting immutable names or payload. | bUnit covers guest exclusion, paging, errors/retry and account-change stale-response rejection. LocalDB proves linking after finish associates history without changing public data. | x | 2026-10-17 |
| TASK-1108 | Add confirmed deletion in finished rooms and history using current server-derived permissions, reload live results on room version changes and restore focus after deletion. | bUnit verifies confirmation, permission visibility and focus; LocalDB verifies deletion invalidates token/history while retaining the room. Private-window walkthrough deferred. | x | 2026-10-17 |

**Checkpoint:** a result can be seen, copied, shared, listed and deleted.

### Implementation Phase 3

- GOAL-1103: Retention and acceptance

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-1109 | Schedule the non-overlapping UTC cleanup job; batch candidates, recheck activity inside the room gate, preserve snapshots, notify after commit and log deleted count. Default retention is thirty days; startup validation permits zero only in Development. | Fake-clock LocalDB tests cover the exact cutoff, newly active candidates, already deleted rooms, retained snapshots/players and close notifications. Accelerated isolated Development walkthrough deferred. | x | 2026-10-17 |
| TASK-1110 | Audit result/history semantics, headings, wrapper controls, live/error states, dialogs, focus restoration and responsive layout. Walk SCN-80 end to end. | Code audit and automated label/focus/state checks complete; real-browser, keyboard/screen-reader, clipboard and SCN-80 checks remain deferred. | partial | 2026-10-17 |

**Phase gate (tick before starting P12):**

- [ ] SCN-80 the result shows in the finished room, copy as text works, the public link opens in a private window, history lists it, deleting makes the link answer 404, the cleanup job removes an old room but keeps results
- [x] Reflection tests (no secret members, no user ids in the public DTO) green
- [x] Migration `AddDraftResults` applied and rolled back once
- [x] Build, tests, Release build and `dotnet format --verify-no-changes` clean for touched files

## 3. Alternatives

- **ALT-1101**: Keep results inside the room tables. Rejected: they must outlive the room and be readable without membership.
- **ALT-1102**: Auto-expire results. Rejected: you chose to keep them until deleted.

## 4. Dependencies

- **DEP-1101**: P10 draft completion, P07 link command and "My rooms" patterns, existing Quartz setup.

## 5. Files

- **FILE-1101**: `Core/Entities/DraftRooms/DraftResult.cs`, `DraftResultPlayer.cs`, `DataAccess/Configurations/DraftRooms/*`, `Migrations/*AddDraftResults*`, `Repositories/IDraftResultRepository.cs`
- **FILE-1102**: `Business/Logic/DraftRooms/GetResultByToken*`, `GetMyResults*`, `DeleteResult*`, `LinkAccountCommand*`
- **FILE-1103**: `Contracts/DTOs/DraftRooms/DraftResultDto.cs`, `FactionPickResultDto.cs`
- **FILE-1104**: `API/Controllers/DraftRoomsController.cs`, `API/Jobs/DraftRoomCleanupJob.cs`, `API/Services/ServiceCollectionExtensions.cs`, `API/appsettings.json`
- **FILE-1105**: `Web/Components/DraftRooms/DraftRoomResultView.*`, `FactionPick/FactionPickResultView.*`, `MyDraftsList.*`, `Web/Pages/Tools/DraftRoomResult.razor(.cs)`, `Web/Components/Shared/Seo/SeoMetadata.razor.cs`

## 6. Testing

- **TEST-1101**: Unit tests for snapshot building, queries, delete rules, text formatting.
- **TEST-1102**: SQL Server LocalDB tests for the tables, the retention job and the link-after-finish case.
- **TEST-1103**: bUnit tests for the views and the list. Manual SCN-80.

## 7. Risks & Assumptions

- **2026-10-17 revalidation**: P10 already returns `DraftTransition.IsFinished` and sets room status inside the shared action transaction. Snapshot creation must be inserted into that same transaction, not a post-commit handler. `DraftRoom.ResultId` already exists without a result relationship. Public engine result payloads must use seat numbers rather than runtime seat/participant identifiers and must not reuse `FactionPickSettingsDto` because its configured order contains runtime GUIDs. Token matching must be case-sensitive. Account history should include owned snapshots as well as seated participation so an unseated account host can still manage their results after room cleanup. Linking updates ownership/history metadata only; it must never rewrite immutable public display-name snapshots.
- **2026-10-17 implementation**: Completed public envelope/query/deletion APIs, finished/public UI, copy/share controls, account history/link metadata and UTC cleanup. Full unit suite: 521 passed / 3 existing skips; component suite: 249 passed; integration suite: 190 passed. The copy formatter deliberately retains stable faction identifiers in the pure Draft assembly; UI labels use existing Web faction resources. Changed-source formatting verification, Release Web publication and Release API build pass without warnings. All deferred manual/deployment checks remain unchecked.

- **RISK-1101**: A guest-hosted room that is deleted by the cleanup job leaves a result nobody can delete (no owner account). Mitigation: results from guest hosts can be deleted by a co-host or host only while the room exists. Revisit if it matters.
- **RISK-1102**: Public links show display names (`RSK-06`, you decided against a privacy notice). Revisit before a wide release.
- **ASSUMPTION-1101**: The cleanup job runs on the single API instance, so no distributed lock is needed.

## 8. Related Specifications / Further Reading

- [Specification: 6.10, 9.2, 9.4, 11 SCR-009](../docs/draft-rooms/specification.md)
- [Decisions DEC-20, DEC-21, DEC-23](../docs/draft-rooms/specification.md)
