---
goal: Draft Rooms P04 - Lobby UI (seats, people, host settings)
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-15
owner: Lazik
status: 'Planned'
tags: [feature, draft-rooms, frontend, blazor, lobby]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

Builds the lobby inside the bare room page on top of the P03 endpoints: header, seat list, participant list, host settings. Until P05 you update the page with a **Refresh** button. Specification: `SET-001..004`, `SET-007`, `HST-002..004`, `HST-009`, `ROM-006`, `ROM-009`, `NFR-001`, `NFR-003`.

Needs: P03 passed.

## 1. Requirements & Constraints

- **REQ-0401**: Show exactly what the section 4 permission table allows. Buttons are driven by the `Me` flags from `DraftRoomViewDto`.
- **REQ-0402**: One `IDraftRoomSession` per open room page holds the latest view and raises `Changed`, so P05 can add push updates without touching the components.
- **REQ-0403**: Build the UI from `Responsive*` components. New reusable parts are named `DraftRoom*` or `DraftSeat*`.
- **SEC-0401**: Hiding a button is only convenience. The server enforces every rule (`SCR-004`).
- **CON-0401**: Each component has `.razor` and `.razor.cs`, and `.razor.css` when needed. Strings go into `Strings.resx` and `Strings.Designer.cs`.
- **GUD-0401**: Use `@key` on seat and participant lists. Confirm destructive actions (kick, close, leave with a seat) with `ResponsiveConfirmDialog`.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-0401: Session state and page states

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0401 | Extend `IDraftRoomApiService` with `GetViewAsync`, `AddSeatAsync`, `RemoveSeatAsync`, `ClaimSeatAsync`, `UnclaimSeatAsync`, `LeaveAsync`, `KickAsync`, `SetRoleAsync`, `UpdateSettingsAsync`, `ChangePasswordAsync`, `CloseAsync` (paths in `Paths.resx` and `Paths.Designer.cs`, guest headers as before). Add `IDraftRoomSession` and `DraftRoomSession` in `Services/DraftRooms/`: `View`, `IsLoading`, `Error`, `RefreshAsync()`, `ExecuteAsync(Func<Task<DraftRoomApiCommandResult>>)` which refreshes after each command (including 409 conflicts), and a `Changed` event. Register transient. | Tests verify refresh updates `View`, command conflicts refresh the view, `Changed` fires once per refresh, and forbidden responses are exposed. | x | 2026-10-15 |
| TASK-0402 | In `Pages/Tools/DraftRoom.razor(.cs)` initialize the room session after the join gate, call `RefreshAsync`, pass it down with `CascadingValue`, and unsubscribe from session changes when the page is disposed. Add loading, generic error, removed/forbidden, and room-closed/unavailable states. The API currently uses 404 both for a closed room and an inactive participant after entry, so the unavailable message states both possibilities instead of claiming one cause. | bUnit tests cover loading, generic error, forbidden access, 404 after entry, live session updates, and existing join behavior. | x | 2026-10-15 |

### Implementation Phase 2

- GOAL-0402: Lobby components

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0403 | Add `Components/DraftRooms/DraftRoomHeader` (`.razor`, `.razor.cs`, `.razor.css`): room name, role badge, code and join link with `CopyButton`, **Refresh**, **Leave** for members, **Close room** when `CanClose`, both through `ResponsiveConfirmDialog`. Confirmed leave returns to Draft Rooms; close leaves the page on its unavailable state. | bUnit verifies role-based actions, copied-link markup, confirmation/cancellation, commands through the session, refresh, and leave navigation. | x | 2026-10-15 |
| TASK-0404 | Add `DraftSeatList` and `DraftSeatCard`: seat number, occupant name or "Free", **Claim**, **Leave seat**, **Unclaim** for host and co-host, **Add seat** and **Remove seat** when `CanManageSeats` (disabled at the limits with a visible reason). | bUnit: claim and unclaim call the session, free seats show Claim, limits disable the buttons. | x | 2026-10-15 |
| TASK-0405 | Add `DraftParticipantList`: name, role badge, seat, guest marker. Host actions per row: **Kick** (confirm), **Make co-host** or **Remove co-host** (`CanChangeRoles`), **Unclaim seat**. | bUnit: a co-host sees no promote buttons, nobody sees Kick on the host. | x | 2026-10-15 |
| TASK-0406 | Add `DraftRoomHostSettings` (visible when `CanChangeSettings`): rename room, **Lock room** with `ResponsiveSwitch`, change-password dialog (`ResponsivePassword`, repeat field, same validator rules as the create form). | bUnit: permission visibility, name validation/save, accessible lock switch and update call, password mismatch, success and failure states. | x | 2026-10-15 |
| TASK-0407 | Responsive layout in the room page: two columns on desktop (main area and side panel), tabs on mobile with `ResponsiveTab` (Seats, People, Settings). CSS isolation files, no horizontal scroll at 360 px width. | bUnit verifies tab selection, panel associations, settings permission visibility and permission-loss fallback. CSS uses a 768 px breakpoint with min-width-safe grid tracks. Manual 1400 px / 360 px viewport check remains part of the TASK-0408 acceptance gate. | x | 2026-10-15 |

### Implementation Phase 3

- GOAL-0403: Quality and manual acceptance

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0408 | Component tests for every new component (visibility by permission, actions call the session). Accessibility pass with the `accessibility-audit` skill: roles and labels on lists and dialogs, focus returns to the trigger after a dialog closes, visible focus, no colour-only state. Then walk SCN-10 to SCN-17 in two browsers using **Refresh**. | Automated tests and source audit are complete; custom modal dialogs now use native modal behavior for focus containment and restoration. The two-browser SCN-10..17 walkthrough and 1400 px / 360 px viewport check remain unverified; keep the P04 phase gate open until those manual checks are completed. | | |

**Phase gate (tick before starting P05):**

- [ ] SCN-10 host on Seat 1, claim a seat, a taken seat is refused
- [ ] SCN-11 add and remove seats within 2-8, occupied seat cannot be removed
- [ ] SCN-12 unclaim, move, leave
- [ ] SCN-13 host unclaims, kick blocks the identity
- [ ] SCN-14 promote to co-host, co-host limits
- [ ] SCN-15 locked room refuses a new person
- [ ] SCN-16 changed password applies to new joins only
- [ ] SCN-17 close room shows "room closed" in every tab
- [ ] Build, tests, Release build and `dotnet format --verify-no-changes` clean for touched files

## 3. Alternatives

- **ALT-0401**: Poll the view every few seconds as a stop-gap. Rejected: P05 replaces it, and manual Refresh keeps this phase easy to verify.

## 4. Dependencies

- **DEP-0401**: P03 endpoints and `DraftRoomViewDto`.

## 5. Files

- **FILE-0401**: `Web/Services/DraftRooms/IDraftRoomSession.cs`, `DraftRoomSession.cs`, `IDraftRoomApiService.cs`, `DraftRoomApiService.cs`
- **FILE-0402**: `Web/Components/DraftRooms/DraftRoomHeader.*`, `DraftSeatList.*`, `DraftSeatCard.*`, `DraftParticipantList.*`, `DraftRoomHostSettings.*`
- **FILE-0403**: `Web/Pages/Tools/DraftRoom.razor(.cs)`
- **FILE-0404**: `Web/Resources/Strings.resx`, `Strings.Designer.cs`, `Paths.resx`, `Paths.Designer.cs`

## 6. Testing

- **TEST-0401**: bUnit tests for the session, page states and each component.
- **TEST-0402**: Manual scenarios SCN-10 to SCN-17 with two browser profiles.

## 7. Risks & Assumptions

- **RISK-0401**: Two people press the same button at once. The server answers 409 and the session refreshes, the user sees the current state.
- **ASSUMPTION-0401**: Chat and log tabs do not exist yet. They are added in P06.

## 8. Related Specifications / Further Reading

- [Specification: 4, 5.5, 6.4, 6.5, 12](../docs/draft-rooms/specification.md)
