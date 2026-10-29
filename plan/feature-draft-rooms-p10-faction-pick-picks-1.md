---
goal: Draft Rooms P10 - Faction Pick picks phase, undo, seat assignment and turn alert
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-17
owner: Lazik
status: 'Implemented - manual acceptance deferred'
tags: [feature, draft-rooms, engine, faction-pick, frontend, accessibility]
---

# Introduction

![Status: Implemented](https://img.shields.io/badge/status-Implemented-green)

Builds the second phase of the Faction Pick Draft: turn-based **picks** with lock-in, the host-defined pick order, **decide for**, **undo last pick**, post-start seat release/reclaim/replacement, host assignment to a vacant seat, and the **blinking tab title** that tells the active player it is their turn. After this phase a complete draft can be played to the end. Specification: `FPD-004`, `FPD-007`, `FPD-008`, `FPD-010`, `HST-007`, `HST-008`, `SET-005`, `LIV-004`, `NFR-002`, decisions `DEC-10..12`, `DEC-29`, `ASM-04`, `ASM-11`.

Needs: P09 passed. Planned at task level and re-checked against the real code first (TASK-1001).

## 1. Requirements & Constraints

- **REQ-1001**: Implement `FPD-007`: order computed at start (random or host-defined), snake reverses each round when on, the active seat selects privately and locks in publicly, picked and banned factions cannot be selected, each seat makes `PicksPerPlayer` picks.
- **REQ-1002**: Undo removes only the last locked pick, only while Picks is running, never after Finished (`ASM-04`).
- **REQ-1003**: After start, a seated participant may release their own seat and reclaim only that same seat while vacant. Any active unseated participant may claim a seat released after start, which replaces its former occupant and ends that occupant's reclaim eligibility. Host or co-host may unclaim any seat or assign an active unseated participant to a vacant seat. Every occupant change invokes the P09 handler and enforces `SET-006`.
- **REQ-1004**: Turn alert: when my action is needed and the tab is hidden, the title alternates between the normal title and "Your turn" once per second. It stops when the tab is visible or the turn ends. A per-user switch (local storage, default on) turns it off. A visible banner is always shown (`LIV-004`, `NFR-002`).
- **SEC-1001**: The redaction matrix from P09 is extended to picks: another seat's pending pick is never visible to anyone, the host included.
- **SEC-1002**: A seat can only act on its own turn. Two simultaneous lock-ins: exactly one wins, the other gets a conflict.
- **CON-1001**: Page visibility cannot be observed by Blazor, so one tiny JS module reports a boolean (`ASM-11`). The title blink itself stays in Blazor (`PageTitle` with `PeriodicTimer`). If hidden-tab behaviour proves unreliable, document it and extend the module.
- **GUD-1001**: Order editing uses up and down buttons, not drag and drop, so it works with a keyboard.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-1001: Picks rules, order, decide-for, undo, seat assignment

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-1001 | Re-validate specification sections 6.9/7 against P09. Reuse its typed action pipeline, immutable choices, decide-for API, board and occupant hook. Post-start seat authorization/assignment already landed in P08/P09. Persist host-defined lobby order in settings (separate from the runtime turn-order table) so Reset preserves configuration; Start validates actual seat membership. | Plan reconciled with committed code; no duplicate seat backend or action endpoints are needed. | x | 2026-10-17 |
| TASK-1002 | Implement pure picks, active-seat cursor, snake ordering, availability checks and completion. | All 70 combinations of 2-8 seats, 1-5 picks and both snake settings pass; invalid picks and out-of-turn actions are refused. | x | 2026-10-17 |
| TASK-1003 | Enable HostDefined order with keyboard up/down controls. Persist `FactionPickSettings.SeatOrder`, validate exact current-seat membership and copy to runtime order at Start. Reset preserves configuration. | Pure validation, LocalDB save/start/reset and migration rollback/reapply tests, and bUnit reorder tests pass. Configured order is logged without claiming it was randomized. | x | 2026-10-17 |
| TASK-1004 | Implement pick decide-for with ordered public disclosure and attribution, plus manager-only transactional undo and its API. | Pure rules and real HTTP full-flow tests cover disclosure, undo, stale duplicate lock-in and refusal after Finished. | x | 2026-10-17 |
| TASK-1005 | Project public picks/cursor and only the viewer's own pending choice. | Six-viewer matrix and six live REST/SignalR viewers verify pending-pick redaction. | x | 2026-10-17 |
| TASK-1006 | Reuse P08/P09 running release/reclaim/replacement/assignment authorization and occupant hook. Add explicit Release/Reclaim labels using server-projected permissions. | Existing race/eligibility tests remain green; additional LocalDB picks tests prove pending cleanup without disclosure and public locked-pick preservation. Manual SCN-61/62 deferred. | x | 2026-10-17 |

**Checkpoint:** a draft can be played through the API and the rules are covered by tests.

### Implementation Phase 2

- GOAL-1002: Picks UI and turn alert

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-1007 | Extend the board with private selection, cursor-bound lock-in, per-seat public picks, current-turn marker, independently chosen manager replacement, confirmed undo and final-pick warning. | bUnit verifies role/turn restrictions, payloads, confirmation, stale manager-choice cancellation and read-only finished lists. | x | 2026-10-17 |
| TASK-1008 | Add the visibility-only JS observer, disposable Blazor title timer, default-on persisted preference and always-visible action prompt above mobile panels. | Fake-clock/visibility bUnit tests verify alternation, immediate restoration, persistence, disposal and explicit interop failure. Five-minute browser rhythm remains unmeasured and documented in architecture. | x | 2026-10-17 |

**Checkpoint:** a human can play a full draft in three browsers.

### Implementation Phase 3

- GOAL-1003: Full-flow tests and acceptance

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-1009 | Extend the six-viewer hub test through a complete three-seat snake draft, undo, manager decision and completion; add out-of-turn lock race and occupant cleanup regressions. | Live full flow and LocalDB concurrency tests pass. | x | 2026-10-17 |
| TASK-1010 | Run the accessibility skill's code audit and automated accessibility-sensitive checks; record deferred manual browser scenarios. | Semantic controls, labels, current-step state, textual choice states, confirmation dialogs and focus handling checked. SCN-70 to SCN-73 remain manual gates below. | x | 2026-10-17 |

**Phase gate (tick before starting P11):**

- [ ] SCN-70 three seats, two picks each, snake order 1-2-3-3-2-1, out-of-turn selection refused, banned and picked factions unavailable, lock-in final
- [ ] SCN-71 undo last pick works, decide-for shows the reveal in the log
- [ ] SCN-62 post-start self-release, same-seat-only reclaim, replacement claim, and reclaim denial after replacement work
- [ ] SCN-72 hidden tab on my turn: title blinks, focusing the tab stops it, the switch turns it off
- [ ] SCN-73 the last pick moves the room to Finished
- [ ] Extended redaction matrix and full-flow tests green
- [ ] Build, tests, Release build and `dotnet format --verify-no-changes` clean for touched files

## 3. Alternatives

- **ALT-1001**: Blink with a JS timer only. Rejected for now: Blazor can do the timer, JS is limited to reporting visibility (`ASM-11`).
- **ALT-1002**: Restrict every post-start replacement to the host. Rejected by the user's clarified rule: eligible former occupants can reclaim their own vacancy, and a new unseated participant can replace them; manager assignment uses the same eligibility rules.

## 4. Dependencies

- **DEP-1001**: P09 engine pieces and the redaction tests. P05 test host.

## 5. Files

- **FILE-1001**: `Draft/DraftRooms/FactionPick/*` (picks rules, decide-for, undo, projection)
- **FILE-1002**: `Business/Logic/DraftRooms/UndoLastPick*`, `AssignSeat*`
- **FILE-1003**: `API/Controllers/DraftRoomsController.cs`
- **FILE-1004**: `Web/Components/DraftRooms/FactionPick/FactionPickBoard.*`, `FactionPickSettingsEditor.*`, `DraftParticipantList.*`, `DraftRoomHeader.*`, `TurnAlertTitle.*`
- **FILE-1005**: `Web/Services/DraftRooms/IPageVisibility.cs`, `PageVisibility.cs`, `Web/wwwroot/js/draft-room-visibility.js`
- **FILE-1006**: `tests/TwilightImperiumUltimate.IntegrationTests/Hubs/DraftRoomHubTests.cs`, `DraftRooms/DraftLifecycleCommandTests.cs`, `docs/architecture.md`

## 6. Testing

- **TEST-1001**: Unit tests for order, snake, validations, decide-for, undo, assign.
- **TEST-1002**: Extended redaction matrix and full-flow hub tests with concurrency cases.
- **TEST-1003**: bUnit tests for the board and the turn alert.
- **TEST-1004**: Manual SCN-70 to SCN-73.

## 7. Risks & Assumptions

- **2026-10-17 implementation**: Pure engine, application, board and title controls implemented. Full unit suite: 500 passed, 3 pre-existing skips; full component suite: 227 passed; focused LocalDB lifecycle/migration/live-flow suite: 40 passed. Configured-order migration `20261017074043_AddFactionPickConfiguredOrder` uses an empty collection default for existing rooms and was rolled back/reapplied in an isolated test database. Manual browser/keyboard/screen-reader and long-hidden-tab checks are deferred at the user's request, not marked passed.

- **2026-10-17 revalidation**: P09 supplies private-choice persistence, a stateless singleton occupant-change hook, transactional typed actions and manager disclosure. P08/P09 already enforce release/reclaim/replacement and assignment with race tests. Host-defined order will be a settings collection, then copied to the runtime order at Start, preserving configured rules across Reset without retaining draft progress.

- **RISK-1001**: Browsers slow timers in hidden tabs (`RSK-04`). The banner and the log are the fallbacks, the observed behaviour is documented.
- **RISK-1002**: The last pick ends the draft immediately, so a mistake on it cannot be undone (`ASM-04`). The lock-in confirmation text for the final pick should say so.
- **ASSUMPTION-1001**: The pick order is final once the draft starts, so the order list is not editable while running.

## 8. Related Specifications / Further Reading

- [Specification: 6.5 HST-007, HST-008, 6.6 LIV-004, 6.9, 7.4, 8](../docs/draft-rooms/specification.md)
- [Assumptions ASM-03, ASM-04, ASM-11; scenarios SCN-70 to SCN-73](../docs/draft-rooms/specification.md)
