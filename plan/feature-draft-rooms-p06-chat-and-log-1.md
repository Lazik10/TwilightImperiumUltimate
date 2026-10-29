---
goal: Draft Rooms P06 - Chat and activity log
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-16
owner: Lazik
status: 'Completed'
tags: [feature, draft-rooms, chat, log, backend, frontend]
---

# Introduction

![Status: Completed](https://img.shields.io/badge/status-Completed-brightgreen)

Adds the persisted chat with join and leave messages and the append-only activity log, both live through the hub. Specification: `CHT-001..005`, `LIV-002`, `SCR-008`, `SCR-010`, `NFR-001`, decisions `DEC-09`, `ASM-06`.

Needs: P05 passed.

## 1. Requirements & Constraints

- **REQ-0601**: Implement `CHT-001..005`. Chat is plain text, 1-300 characters, the latest 200 messages load with the room.
- **REQ-0602**: Every existing state-changing command appends a public log entry in the same transaction through `IDraftRoomLog`. Log data is public data only. "Password changed" is logged without any password content.
- **REQ-0603**: Join and leave messages come from the server (presence events and explicit leave or kick), never from clients.
- **SEC-0601**: Chat and room text is trimmed, length-limited, stored and rendered as plain text. `MarkupString` is never used and links are not clickable (`SCR-008`).
- **SEC-0602**: A hidden anti-flood guard allows 5 messages per 5 seconds per participant (`ASM-06`). It answers with a polite error code, not a stack trace.
- **CON-0601**: Chat does not change room state, so it must not increment `Version` and must not trigger `RoomChanged`. It still updates `LastActivityUtc`.
- **GUD-0601**: Read endpoints page with `afterId` and never return more than 200 items.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-0601: Persistence and logging service

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0601 | Add enums `ChatMessageKind` (User, Join, Leave) and `DraftLogEntryType` (RoomCreated, ParticipantJoined, ParticipantLeft, ParticipantKicked, SeatAdded, SeatRemoved, SeatClaimed, SeatUnclaimed, RoleChanged, RoomRenamed, RoomLocked, RoomUnlocked, PasswordChanged) in `Contracts/Enums/`. Add entities `DraftRoomChatMessage` and `DraftRoomLogEntry` (`long` identity `Id`, columns per spec 9.2) with configurations (index (`RoomId`,`Id`), cascade on room delete, `Text` max 300), DbSets under `// Draft rooms`, `TableName` constants, schema `Draft` and migration `AddDraftRoomChatAndLog`. | Persistence tests on `TestDatabase`: ordering by `Id`, cascade on room delete, a 301-character text is rejected by the mapping. Migration applied and rolled back once on SQL Server. | x | 2026-10-16 |
| TASK-0602 | Add `IDraftRoomLog` and `DraftRoomLog` in `Business/Services/DraftRooms/` with `Append(DraftRoomScope scope, DraftLogEntryType type, Guid? actorParticipantId, object data)` which serialises public data with `System.Text.Json` into `DataJson` inside the current transaction. Instrument every P01 and P03 command (create, join, leave, kick, role, seat add, remove, claim, unclaim, settings, lock, password changed). Closing a room needs no entry because the room is deleted. | LocalDB command integration tests assert expected log types, valid JSON for every emitted payload, and case-insensitive exclusion of `secret`/`password`; create/join/password tests also exclude actual guest secrets, password values, and hashes. A repository failure test proves the mutation and log roll back together. | x | 2026-10-16 |

**Checkpoint:** every action already produces a log row.

### Implementation Phase 2

- GOAL-0602: Chat commands, presence messages, read endpoints, live events

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0603 | Add parameter `changesState` (default true) to `IDraftRoomUnitOfWork.ExecuteAsync`. When false it skips the `Version` increment and the `RoomChanged` notification but still sets `LastActivityUtc`. Add `SendChatCommand(code, text)` and handler: trim, 1-300 characters, active participants only (spectators allowed), anti-flood guard (5 per 5 seconds per participant, `TimeProvider`), persist, return the DTO, then notify `ChatMessageReceived` (extend `IRoomChangeNotifier`). | Unit tests refuse empty, whitespace, and 301-character messages and verify the throttle window, participant isolation, and concurrency. LocalDB integration tests verify spectator success, kicked participant refusal, sixth-message refusal, post-commit room notification, activity timestamp update, and unchanged room version. | x | 2026-10-16 |
| TASK-0604 | Add a hosted service `PresenceChatPublisher` in the API that listens to `IPresenceTracker`: first connection writes a `Join` chat message "X joined", offline after the grace period writes a `Leave` message "X left" (use an `IServiceScopeFactory` scope and the unit of work with `changesState: false`). Explicit Leave and Kick write the Leave message in their state-change transaction and broadcast it after commit. A refresh inside the grace period writes nothing. | Hub integration tests with a 1-second test grace period verify Join/Leave persistence and room broadcast, no Leave on reconnect, and one Leave after final disconnect. Command integration tests verify explicit Leave and Kick persist and broadcast Leave messages. | x | 2026-10-16 |
| TASK-0605 | Add `GET api/draft-rooms/{code}/chat?afterId=&limit=`, `POST api/draft-rooms/{code}/chat`, `GET api/draft-rooms/{code}/log?afterId=&limit=` (limit max 200, members only) with `ChatMessageDto` and `DraftLogEntryDto`. After commit send hub events `ChatMessageReceived(dto)` and `LogEntryAdded(dto)` to the room group only. | Controller tests. Hub test: a client in room B never receives chat or log events of room A. | x | 2026-10-16 |

**Checkpoint:** server side complete and covered by hub tests.

### Implementation Phase 3

- GOAL-0603: Panels and acceptance

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0606 | Add `Components/DraftRooms/DraftRoomChatPanel` (`.razor`, `.razor.cs`, `.razor.css`): message list with `role="log"` and `aria-live="polite"`, `@key` per message id, `ResponsiveTextBox` with `MaxLength` 300, send on Enter and with a **Send** button, auto-scroll only when already at the bottom, never steals focus, system messages carry a text label (not colour alone), loads the latest 200 on start, shows a polite text when the flood guard refuses. | bUnit: renders history, appends a new message, hides send for an empty text, shows the refusal text. | x | 2026-10-16 |
| TASK-0607 | Add `DraftRoomLogPanel` (timestamps in local time, one line per entry, live append). Add **Chat** and **Log** to the desktop side panel and as mobile tabs, with an unread counter on the inactive tab. | bUnit: live append works, unread counter increments and clears. | x | 2026-10-16 |
| TASK-0608 | Add component tests for both panels, accessibility audit with the `accessibility-audit` skill (live region stays off during history load, keyboard reachable), then verify SCN-30 to SCN-33. | Full solution tests, keyboard/live-region bUnit checks, and Release build pass. Automated integration coverage verifies persistence, presence, room isolation, logging safety, throttling, and the 300-character boundary. | x | 2026-10-16 |

**Phase gate:**

- [x] SCN-30 room-scoped broadcast, spectator chat, 300-character acceptance, and cross-room isolation are covered by integration/component tests; sub-second latency was not benchmarked
- [x] SCN-31 join and leave chat events and bounded history are covered by hub/API integration tests
- [x] SCN-32 seat, kick, promotion, and settings log entries are covered by integration tests; payload assertions exclude secrets/passwords
- [x] SCN-33 the sixth message in the throttle window is refused by integration tests and announced politely by the chat component
- [x] Full tests, Release build and format verification pass for changed files

**Manual follow-up:** No browser-based multi-client walkthrough or screen-reader smoke test was run. The automated checks verify the live region remains off while history is pending and becomes polite afterwards, and that arrow-key navigation moves tab focus. Verify actual announcement behavior with Narrator/NVDA before production launch.

## 3. Alternatives

- **ALT-0601**: One combined table for chat and log. Rejected: different payloads, different paging and a different UI. Two small tables stay simple.
- **ALT-0602**: Let clients write join and leave messages. Rejected: they could forge them.

## 4. Dependencies

- **DEP-0601**: P05 hub, presence tracker, notifier and hub test host.

## 5. Files

- **FILE-0601**: `Contracts/Enums/ChatMessageKind.cs`, `DraftLogEntryType.cs`, `Contracts/DTOs/DraftRooms/ChatMessageDto.cs`, `DraftLogEntryDto.cs`
- **FILE-0602**: `Core/Entities/DraftRooms/DraftRoomChatMessage.cs`, `DraftRoomLogEntry.cs`; `DataAccess/Configurations/DraftRooms/*`, `Migrations/*AddDraftRoomChatAndLog*`
- **FILE-0603**: `Business/Services/DraftRooms/DraftRoomLog.cs`, `Business/Logic/DraftRooms/SendChatCommand*`, `GetChat*`, `GetLog*`
- **FILE-0604**: `API/Services/PresenceChatPublisher.cs`, `API/Hubs/DraftRoomHub.cs`, `API/Controllers/DraftRoomsController.cs`
- **FILE-0605**: `Web/Components/DraftRooms/DraftRoomChatPanel.*`, `DraftRoomLogPanel.*`, `Web/Pages/Tools/DraftRoom.razor(.cs)`

## 6. Testing

- **TEST-0601**: Unit tests for the log (one per command), chat rules, flood guard.
- **TEST-0602**: SQL Server LocalDB integration tests for the two tables.
- **TEST-0603**: Hub tests: broadcast, isolation between rooms, presence messages.
- **TEST-0604**: bUnit tests for both panels and automated integration coverage for SCN-30 to SCN-33; manual screen-reader/browser smoke tests remain recommended.

## 7. Risks & Assumptions

- **RISK-0601**: A noisy chat could keep the room "active" forever and defeat the 30-day cleanup. That is intended: an active room is not deleted.
- **RISK-0602**: Screen readers may repeat the whole history when the list re-renders. The audit in TASK-0608 checks it, `@key` per message avoids re-creating items.
- **ASSUMPTION-0601**: Without host delete or mute, kick is the only moderation tool in v1.

## 8. Related Specifications / Further Reading

- [Specification: 6.7, 11 SCR-008, 13 NFR-001](../docs/draft-rooms/specification.md)
