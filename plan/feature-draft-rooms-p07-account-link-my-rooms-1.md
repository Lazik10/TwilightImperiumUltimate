---
goal: Draft Rooms P07 - Guest to account link and My rooms
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-16
owner: Lazik
status: 'In progress'
tags: [feature, draft-rooms, identity, security, frontend, backend]
---

# Introduction

![Status: In progress](https://img.shields.io/badge/status-In%20progress-yellow)

Lets a guest swap their seat's guest identity for a website account, makes the login page return to the room safely, and adds the "My rooms" list to the landing page. Specification: `IDN-006`, `IDN-007`, `LND-005`, `SCR-011`, decisions `DEC-18`, `DEC-22`.

Needs: P06 passed.

## 1. Requirements & Constraints

- **REQ-0701**: Implement `IDN-006` and `IDN-007`. The linked participant keeps seat, role and history, and the guest secret stops working.
- **REQ-0702**: Implement `LND-005`: accounts see their rooms from the database, guests see the rooms stored in this browser, both enter again without a password.
- **SEC-0701**: Linking needs both proofs in one request: a valid account bearer token **and** the valid guest secret.
- **SEC-0702**: After a successful link `SecretHash` is cleared. The old secret no longer authenticates.
- **SEC-0703**: The login `returnUrl` accepts only local paths: it must start with a single `/`, must not start with `//` or `/\`, and must contain no scheme (`SCR-011`).
- **CON-0701**: `Pages/Account/Login.razor.cs` currently always navigates to `Pages.Index`. Keep that as the fallback.
- **GUD-0701**: When guest headers are present they identify the caller. The link endpoint is the only one that reads both identities.
- **GUD-0702**: SignalR guest credentials identify the caller whenever supplied, even when the connection also carries an authenticated account.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-0701: Server and client link flow, safe login return

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0701 | Add `LinkAccountCommand(code, caller)` and handler in `Business/Logic/DraftRooms/`. The caller must carry the account (bearer) and a verified guest identity. The participant becomes account-backed: `UserId` set, `SecretHash` cleared, `DisplayName` set to the username (numeric suffix if another active participant already uses it). If the account already has a participant record in this room return a conflict (`AccountAlreadyInRoom`), since the unique (`RoomId`,`UserId`) index preserves one identity row even after leaving. Allowed in every room status for an active guest participant. Appends a public account-link log entry and increments `Version`. | Unit tests refuse malformed callers. SQL Server LocalDB handler tests cover successful linking, seat/role/history preservation, guest secret removal, conflict, wrong secret, suffix naming, room status independence, and unique (`RoomId`,`UserId`) constraint. | x | 2026-10-16 |
| TASK-0702 | Add `POST api/draft-rooms/{code}/link-account` (bearer required, guest headers required) to `DraftRoomsController`. Update `DraftRoomCallerResolver`: when guest headers are present they identify the caller, only the link endpoint also reads the account. Add `LinkAccountAsync` to `IDraftRoomApiService`, which removes the stored guest identity through `IGuestIdentityStore.RemoveAsync(code)` only after a success. | Resolver tests for both-present, guest-only, account-only. Service test: identity removed on success only. | x | 2026-10-16 |
| TASK-0703 | In `Pages/Account/Login.razor.cs` accept `[SupplyParameterFromQuery] returnUrl` and, after a successful login, navigate to it only when `IsLocalPath(returnUrl)` is true, otherwise to `Pages.Index`. Put `IsLocalPath` in a small tested helper in `Web/Helpers/`. | `IsLocalPath` unit tests with at least 12 cases: `/tools/draft-rooms/ABC234` true; `https://evil.example`, `//evil.example`, `/\evil.example`, `javascript:alert(1)`, empty, whitespace and encoded variants false (SCN-43). | x | 2026-10-16 |

**Checkpoint:** link flow and the safe return are tested without UI.

### Implementation Phase 2

- GOAL-0702: Room UI for guests and accounts

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0704 | In the room page add a guest banner "Playing as guest, log in to keep this seat" with links to `Pages.Login?returnUrl=/tools/draft-rooms/{code}` and to register. When `ICurrentUserState.User` is set **and** the guest store holds an identity for this code, show a dialog "Link this seat to your account?" before anything else (Yes calls `LinkAccountAsync`, No dismisses it for this session and the page continues as the guest). Add a manual **Use my account** button to the header for the same action. Show a clear message for the conflict result. Keep SignalR guest credentials authoritative when an authenticated account is also present. | bUnit: banner for guests, dialog only with both conditions, Yes calls the service and refreshes, No keeps the guest identity. Manual SCN-40 and SCN-41. Hub integration test: authenticated connection with guest credentials subscribes as that guest. | x | 2026-10-16 |

### Implementation Phase 3

- GOAL-0703: My rooms

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0705 | Add `GetMyRoomsQuery` and `GET api/draft-rooms/mine` (account only): rooms where the account is an Active participant (`Code`, `Name`, `Status`, `Role`, `SeatIndex`, `LastActivityUtc`), newest first, at most 50. | Unit and SQL Server LocalDB tests: only own rooms, kicked and left excluded, ordering, limit. | ✅ | 2026-10-16 |
| TASK-0706 | Add `Components/DraftRooms/MyRoomsList` to the landing page. Accounts load from the API, guests from `IGuestIdentityStore.ListAsync()`. Add an optional `RoomName` to the stored `GuestIdentity` (same storage key, missing value allowed) and save it at create and join. Each entry has **Enter** (opens a new tab through `IRoomTabOpener`) and, for guests, **Forget**. A 404 from the room page removes the stale guest entry. Run the accessibility audit on the list. | bUnit: both sources render, Enter calls the opener, Forget removes the entry. Manual SCN-42. | ✅ | 2026-10-16 |

**Phase gate (tick before starting P08):**

Manual acceptance is deferred until implementation is finished by the user's instruction on 2026-10-17. Continue implementation without marking unexecuted manual scenarios as passed.

- [ ] SCN-40 guest holding a seat logs in, returns to the room, links, keeps seat and role, guest secret removed from local storage
- [ ] SCN-41 the conflict case shows a clear message
- [ ] SCN-42 "My rooms" works for an account and, separately, for a guest browser
- [ ] SCN-43 an external `returnUrl` is ignored
- [ ] Build, tests, Release build and `dotnet format --verify-no-changes` clean for touched files

## 3. Alternatives

- **ALT-0701**: Link automatically at login without asking. Rejected: you decided on a prompt plus a manual button, and an automatic link could pull the wrong seat into an account on a shared computer.

## 4. Dependencies

- **DEP-0701**: P02 `IGuestIdentityStore` and `IRoomTabOpener`, P03 authoriser, P06 log.

## 5. Files

- **FILE-0701**: `Business/Logic/DraftRooms/LinkAccountCommand*`, `GetMyRoomsQuery*`
- **FILE-0702**: `API/Controllers/DraftRoomsController.cs`, `API/Helpers/DraftRoomCallerResolver.cs`
- **FILE-0703**: `Web/Pages/Account/Login.razor.cs`, `Web/Helpers/LocalUrlHelper.cs`
- **FILE-0704**: `Web/Services/DraftRooms/GuestIdentityStore.cs`, `DraftRoomApiService.cs`
- **FILE-0705**: `Web/Components/DraftRooms/MyRoomsList.*`, `DraftRoomHeader.*`, `Web/Pages/Tools/DraftRoom.razor(.cs)`, `DraftRooms.razor(.cs)`

## 6. Testing

- **TEST-0701**: Unit and SQL Server LocalDB integration tests for linking and My rooms.
- **TEST-0702**: `IsLocalPath` tests.
- **TEST-0703**: bUnit tests for banner, dialog and list. Manual SCN-40 to SCN-43.

## 7. Risks & Assumptions

- **RISK-0701**: Account registration needs email confirmation, which leaves the page. Guest storage survives it (local storage), so the prompt still appears after the person comes back.
- **ASSUMPTION-0701**: A guest who declines the link keeps playing as guest and the bearer token is ignored for that room because guest headers win.
- **ASSUMPTION-0702**: Linking is refused if the account has any participant row in the room (including a participant who has left), because the database enforces one participant row per room/account and this task does not merge participant history.

## 8. Related Specifications / Further Reading

- [Specification: 5.8, 6.1 LND-005, 6.3, 11 SCR-011](../docs/draft-rooms/specification.md)
