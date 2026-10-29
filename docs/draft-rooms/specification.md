# Draft Rooms specification

| | |
|---|---|
| Status | Draft v1, decisions collected from interview rounds 1-6 |
| Owner | Lazik |
| Created | 2026-10-15 |
| Implementation plans | [`plan/README.md`](../../plan/README.md) |

This document says **what** Draft Rooms does and **which rules** it follows. The plan files in `plan/` say **in which small steps** it gets built. Every requirement has an ID (for example `SET-005`) so plan tasks and tests can point at it.

Words like MUST and SHOULD are used on purpose: MUST = required for v1, SHOULD = wanted, may slip.

---

## 1. Purpose and scope

Draft Rooms lets a group run a Twilight Imperium 4 draft together, live, from separate devices.

1. A person opens **Tools > Draft Rooms** and chooses **Create Draft** or **Join Draft**.
2. The creator (the **host**) picks a room name and a simple password. People join with the room code (or a link) and the password, either with their website account or as a guest.
3. The room opens in its **own browser tab with a bare layout** (like Game Tracker).
4. In the **lobby** people claim **seats**, the host configures the draft type and its settings, and everyone can chat.
5. The host **starts** the draft. The draft runs live according to the rules of the chosen **draft type**. The first draft type is the **Faction Pick Draft**.
6. When the draft finishes, the **results** can be copied, shared by a public link, and kept in account history.

### In scope for v1

- Rooms, passwords, room codes, links, guest and account identities, per-room guest identity stored in the browser.
- Seats, host and co-host powers, kick (with block), spectators, room lock.
- Live updates (SignalR), presence, chat, activity log, tab-title turn alert.
- A pluggable draft engine with hidden information, lock-in, host-on-behalf decisions with public disclosure, undo and reset.
- Draft type #1: Faction Pick Draft (bans: none / host / simultaneous secret; picks: turn-based).
- Results: in-room view, copy as text, public share link, account history, 30-day inactivity cleanup.
- Tests at every layer (unit, component, SQL Server LocalDB persistence, hub integration, Playwright end-to-end).

### Out of scope for v1 (explicitly)

Milty draft, galaxy-building draft, random-galaxy draft (each is a later draft type); pick timers; pause; skip turn; sound or desktop notifications; host deletion or muting of chat messages; seat reservation for named people; rematch inside a finished room; per-seat recovery codes for guests; Discord posting; admin tooling for rooms; a privacy notice (your decision, see risk `RSK-06`); multi-instance API (no backplane); languages other than English.

---

## 2. Glossary

| Term | Meaning |
|---|---|
| Room | One draft session. Has an `Id` (Guid), a short `Code`, a name, a password, a state. |
| Code | 6 characters, alphabet `ABCDEFGHJKMNPQRSTUVWXYZ23456789` (no 0/O/1/I/L). Used to join and in the room URL. Not secret on its own. |
| Participant | A person inside a room. Either **account-backed** (website user) or **guest-backed** (random per-room identity). |
| Guest identity | `ParticipantId` (public Guid) plus `Secret` (private, 256-bit random). Kept in the browser, only a hash is stored on the server. |
| Host | The creator. Full powers. One per room. |
| Co-host | A participant promoted by the host. Nearly the same powers (see 4). |
| Seat | A numbered slot (1..N). A participant holds at most one seat. Draft state (picks) belongs to the **seat**, not to the person. |
| Spectator | A participant without a seat. Can watch and chat. |
| Draft type | A pluggable module: settings, phases, actions, visibility rules, results, its own tables. |
| Phase | A step of a draft type (for example Bans, Picks). |
| Pending selection | A choice a player has highlighted but not locked in. Private to that seat. |
| Lock-in | The player's explicit confirmation. A locked-in choice is public and final (undo is host-only). |
| Hidden information | Anything the draft rules keep private (v1: pending selections). Never sent to anyone who may not see it. |
| Decide for | Host/co-host completes an action on behalf of a seat. Always public, and it reveals that seat's hidden information to everyone. |
| Result snapshot | Immutable record of a finished draft. Outlives the room. |

---

## 3. Decision log

`You` = decided by you in the interview. `Assumed` = my default, listed again in section 16 so you can overrule it.

| ID | Decision | Source |
|---|---|---|
| DEC-01 | Landing page `/tools/draft-rooms` (normal layout) with **Create Draft** and **Join Draft**. Tools menu link goes live in phase P02 (sitemap entry stays). | You |
| DEC-02 | Create form: room name + password. Password: min 6 characters, any characters allowed, no complexity rules. | You |
| DEC-03 | People join with a short room code or shareable link, then the password. Room name is only a label (not unique). | You |
| DEC-04 | Account user is shown by account username. Guest enters a display name and gets a random per-room identity stored in the browser. | You |
| DEC-05 | After create or join, the room opens in a new tab with a bare layout (like `GameTrackerLayout`). | You |
| DEC-06 | Host edits settings, including draft type, in the lobby. Type, settings and seat count lock when the draft starts. | You |
| DEC-07 | Draft types are pluggable and each has its own settings, flow and tables. One type at a time. Order: Faction Pick, Milty, galaxy-building, random-galaxy. | You |
| DEC-08 | Live updates use SignalR. One API instance, WebSockets enabled. No backplane. | You |
| DEC-09 | Chat and activity log are persisted. Join/leave show in chat. Spectators can chat. | You |
| DEC-10 | Active player's tab title blinks. Per-user switch to turn it off. No sound, no desktop notification. | You |
| DEC-11 | No turn timer. The room waits for lock-in, or the host decides for the player. A host decision reveals the player's hidden information publicly. | You |
| DEC-12 | A highlighted-but-not-locked selection is hidden from everyone else (including the host) until lock-in. | You |
| DEC-13 | Draft settings include pick order mode (host-defined or random) and bans mode (none, host bans, simultaneous secret bans revealed together). Bans run before the picks. | You |
| DEC-14 | Seats: host adds/removes before start (2-8), people claim their own seat, host can unclaim or kick anyone, and host starts seated on Seat 1. Post-start participant release, reclaim, and replacement rules are in `DEC-29`. | You |
| DEC-15 | Kick removes the person and blocks that identity from rejoining (best effort for guests). | You |
| DEC-16 | Host can promote any participant to co-host. | You |
| DEC-17 | Unseated participants are spectators. After start new people may still join unless the host locks the room. | You |
| DEC-18 | Guest to account: in-room prompt when a logged-in user opens a room where this browser holds a guest identity, plus a manual button. Allowed in any room state. | You |
| DEC-19 | Host controls after start: undo last pick, reset to lobby, close room. | You |
| DEC-20 | At the end: results stay visible read-only, copy as text, public share link, save to account history. No rematch. Results kept until the host deletes them. | You |
| DEC-21 | Rooms with no activity for 30 days are deleted. | You |
| DEC-22 | Landing page shows "My rooms" for accounts (database) and guests (this browser), one-click re-enter. | You |
| DEC-23 | No privacy notice in v1. | You |
| DEC-24 | Test tooling: xUnit, bUnit, EF Core with temporary SQL Server LocalDB databases for persistence and hub integration tests, Playwright. | You |
| DEC-25 | Delivery: one long-lived branch, one commit per task. | You |
| DEC-26 | All UI is built with the project's `Responsive*` components. | You |
| DEC-27 | First draft type is the **Faction Pick Draft**: a pool of factions, optional bans, turn-based picks. It proves every engine mechanic with far less UI than Milty or galaxy-building. | You (confirmed my proposal) |
| DEC-28 | Draft room tables live in the **main `TwilightImperiumDbContext`** (schema `Draft`) with migrations in the existing `Migrations/` folder, following the repository convention. | You (final check) |
| DEC-29 | A seated participant may release their own seat after the draft starts and may reclaim only that same seat while it is still vacant. Another active unseated participant may claim the released seat and replace them; once claimed, the former occupant cannot reclaim it. Host/co-host may also unclaim or assign seats. | You (latest clarification) |
| DEC-30 | Persist a room participant when they successfully join, whether seated or spectating. Claiming a seat associates that existing participant with the seat; it does not create a second participant row. Keep kicked participant records so the room can refuse their re-entry. | You |

---

## 3a. What already exists in the repository

| Fact | Where | Consequence |
|---|---|---|
| Empty stub page `@page "/tools/draft-rooms"` | `src/TwilightImperiumUltimate.Web/Pages/Tools/DraftRoom.razor` | Renamed to `DraftRooms.razor` (landing); the real room page gets the name `DraftRoom.razor`. |
| Route constant, page title strings, SEO text, sitemap entry already exist | `Pages/Pages.cs`, `Strings.resx` (`Page_DraftRooms`, `Page_DraftRooms_PageTitle`), `SeoMetadata.razor.cs`, `Contracts/Seo/SitemapPaths.cs` | No new SEO for the landing page. Room and result pages must be `noindex`. |
| Bare layout precedent | `Pages/Layouts/GameTrackerLayout.razor` (just `<main>@Body</main>`), menu link with `Target: "_blank"` | `DraftRoomLayout` is a copy of that idea. |
| Tools menu list | `Components/Shared/Navigation/MainNavigation.razor.cs`, field `_toolsLinks` | One `NavLinkItem` is added. |
| Account auth | ASP.NET Core Identity API endpoints, bearer tokens, `ICurrentUserState` in Web | Account participants use the existing login. Guests cannot, so they get their own secret. |
| No real-time layer, CORS without credentials | `API/Program.cs`, `API/Services/ServiceCollectionExtensions.cs` | SignalR and a CORS change are new (phase P05). |
| One `TwilightImperiumDbContext`, SQL Server, `IDbContextFactory` repositories, schemas per area | `DataAccess/` | Draft room tables join it in a new schema `Draft` (`DEC-28`). Contexts are registered with `NoTracking` by default, so write paths opt in to tracking. |
| Faction pool query | `IFactionRepository.GetFactionsByGameVersions` | Faction Pick Draft reuses it. |
| Quartz jobs | `API/Jobs/`, `RegisterQuartzJobs` | Cleanup job follows the same pattern. |
| Tests: xUnit, Moq, FluentAssertions, Bogus only | `tests/TwilightImperiumUltimate.Tests` | bUnit, SQL Server LocalDB integration, hub and Playwright projects are new. `docs/build-and-test.md` already recommends `tests/TwilightImperiumUltimate.ComponentTests/`. |
| `Strings.resx` uses `ResXFileCodeGenerator` | `Web/Resources/Strings.resx` + `Strings.Designer.cs` | Every new string needs the `.resx` entry **and** the `.Designer.cs` property (or save in Visual Studio to regenerate). |

---

## 4. Roles and permissions

| Action | Host | Co-host | Seated member | Unseated member |
|---|---|---|---|---|
| View room, chat, log | yes | yes | yes | yes |
| Send chat | yes | yes | yes | yes |
| Claim or release own seat (see `SET-003` and `SET-005`) | yes | yes | yes | claim |
| Add or remove seats (lobby only) | yes | yes | no | no |
| Change draft type and settings (lobby only) | yes | yes | no | no |
| Rename room, change password, lock room | yes | yes | no | no |
| Unclaim another seat, assign a participant to a vacant seat (after start) | yes | yes | no | no |
| Kick a member | yes | yes (not host, not co-hosts) | no | no |
| Promote or demote co-hosts | yes | no | no | no |
| Start, reset to lobby, undo last pick, decide for a seat | yes | yes | no | no |
| Delete results | yes | yes | no | no |
| Close room | yes | yes | no | no |
| Take draft actions | only through the seat they hold | same | yes, own seat | no |
| See another seat's hidden information | **never** | **never** | never | never |

Nobody can kick or demote the host. Host and co-host have no special visibility of hidden information (`HST-010`).
After start, an unseated participant may claim a seat only when it has been released. A participant who voluntarily released their seat may reclaim that same seat only while it remains vacant; claiming it for someone else ends that reclaim eligibility.

---

## 5. User flows

### 5.1 Landing
`/tools/draft-rooms` shows a heading and two buttons. **Create Draft** swaps the content to the create form. **Join Draft** swaps it to the join form. A "My rooms" list appears from phase P07.

### 5.2 Create
Form fields: room name, password, and (only when not logged in) display name. On success the guest identity is saved in the browser (guests only) and the room opens in a new tab. If the browser blocks the tab, an **Open room** link is shown.

### 5.3 Join
Form fields: room code, password, and (only when not logged in) display name. A person who opens the room link `/tools/draft-rooms/{code}` directly and is not yet a participant sees the same join form inside the bare room page and stays in that tab.

### 5.4 Re-enter
A known identity (account participant, or guest whose browser holds the secret) enters again without a password, from the link, from "My rooms", or by joining again.

### 5.5 Lobby
Participants and seats are listed. People claim seats. The host edits seat count, draft type and settings, room lock and password. Chat and log are visible. The host presses **Start** when every seat is claimed and the settings are valid.

### 5.6 Draft
The room shows the draft type's board. The active player (or every player who still has to act in a simultaneous phase) gets a banner and a blinking tab title. Players select, then lock in. The host can decide for a seat, undo the last pick or reset to the lobby.

### 5.7 Finish
The draft ends, the room becomes Finished and read-only. A result snapshot appears with **Copy as text**, a public link and (for accounts) a history entry.

### 5.8 Guest to account
A guest sees "Playing as guest, log in to keep this seat". After logging in and returning to the room, one click links the guest seat to the account.

---

## 6. Functional requirements

### 6.1 Landing and navigation (`LND`)

| ID | Requirement | Scenarios |
|---|---|---|
| LND-001 | `/tools/draft-rooms` MUST show **Create Draft** and **Join Draft**. Choosing one swaps the page content to that form without leaving the page. | SCN-01, SCN-03 |
| LND-002 | A Tools-menu link MUST exist from phase P02 (desktop and mobile), text from `Strings.Page_DraftRooms`. | SCN-08 |
| LND-003 | After a successful create or join the room MUST open in a new tab. If it is blocked, the form MUST show an **Open room** link (`target="_blank"`, `rel="noopener"`). | SCN-01, SCN-03 |
| LND-004 | `/tools/draft-rooms/{code}` MUST use a bare layout: no site header, menu or footer. | SCN-08 |
| LND-005 | The landing page SHOULD list "My rooms" (accounts from the database, guests from this browser) with one-click re-enter. | SCN-42 |
| LND-006 | Room pages and result pages MUST be `noindex`. The landing page stays indexable and stays in the sitemap. | SCN-08 |

### 6.2 Room lifecycle and access (`ROM`)

| ID | Requirement | Scenarios |
|---|---|---|
| ROM-001 | Create needs room name (3-60 chars) and password (6-64 chars, any characters, no complexity rules, case-sensitive). Guests also give a display name. | SCN-01 |
| ROM-002 | A room has a unique `Id` (Guid) and a unique `Code`. Room names are labels and may repeat. | SCN-01 |
| ROM-003 | A new identity joins with code + password. A known identity re-enters without a password. | SCN-03, SCN-06 |
| ROM-004 | Wrong code and wrong password MUST give the same status code and the same message. | SCN-04 |
| ROM-005 | Failed joins are throttled to 5 per 15 minutes per (IP, code). Creation is limited to 10 per hour per IP. An account has at most 5 open rooms. | SCN-04 |
| ROM-006 | The password is stored only as a hash. It is never returned and never logged. The host can change it; this affects new joins only. | SCN-16 |
| ROM-007 | States: Lobby, InProgress, Finished. Closing deletes the room in any state. | SCN-17 |
| ROM-008 | At most 32 active participants per room (8 seats plus spectators). | - |
| ROM-009 | The host can lock the room: no new participants, known participants may re-enter. | SCN-15 |
| ROM-010 | Rooms with no activity for 30 days are deleted automatically. Finished results are kept until deleted. | SCN-80 |
| ROM-011 | Every state-changing command increments the room `Version` and sets `LastActivityUtc`. | - |

### 6.3 Identity (`IDN`)

| ID | Requirement | Scenarios |
|---|---|---|
| IDN-001 | A participant is account-backed (Identity user id) or guest-backed. | SCN-01, SCN-02 |
| IDN-002 | An account-backed participant is shown by the account username captured at join. | SCN-02 |
| IDN-003 | A guest chooses a display name (2-24 chars) and receives a random public `ParticipantId` and a private `Secret`. The secret is returned once, stored only in the browser (local storage, keyed by room code), and only its hash is stored on the server. | SCN-01 |
| IDN-004 | The secret is never broadcast, never in a URL, never logged, compared in constant time. | SCN-01 |
| IDN-005 | Display names are unique per room (case-insensitive). A guest cannot use a registered username. If an account username collides, a numeric suffix is added. | SCN-07 |
| IDN-006 | A logged-in user whose browser holds a guest identity for the room can link it. The participant keeps seat, role and history, the guest secret is destroyed. If the account already is a participant in that room the link is refused with a clear message. | SCN-40, SCN-41 |
| IDN-007 | Linking is allowed in any room state. | SCN-40 |
| IDN-008 | A participant row is created or reactivated on successful room join, regardless of whether the person claims a seat. Claiming or releasing a seat updates that participant's seat association; it does not create or delete the participant row. Kicked participant records are retained and denied re-entry by the server and UI. | SCN-01, SCN-13 |

### 6.4 Seats (`SET`)

| ID | Requirement | Scenarios |
|---|---|---|
| SET-001 | A room has 2-8 seats (default 6) until the draft starts. Host or co-host add or remove seats in the lobby. Only unoccupied seats can be removed. Seats are renumbered 1..N. | SCN-11 |
| SET-002 | The host starts seated on Seat 1 and may unclaim it. | SCN-10 |
| SET-003 | In the lobby a participant claims an unoccupied seat, moves, or unclaims. A participant holds at most one seat. | SCN-10, SCN-12 |
| SET-004 | Host and co-host can unclaim any seat. Kick follows `HST-002`. | SCN-13 |
| SET-005 | After start the seat count is locked. A participant may release their own seat and may reclaim only that same seat while it is vacant. Any active unseated participant may claim a seat released after start; doing so replaces the former occupant and ends their reclaim eligibility. Host or co-host may unclaim any seat or assign an active unseated participant to a vacant seat. | SCN-61, SCN-62 |
| SET-006 | When the occupant of a seat changes after start, every **unrevealed** selection of that seat (pending, or locked but not yet revealed) is discarded and NOT revealed, and the seat's lock-in flag is cleared. Locked picks are public and stay. | SCN-61 |
| SET-007 | An unseated participant is a spectator: may watch and chat. | SCN-30 |
| SET-008 | Claiming is atomic: two simultaneous claims of one seat, exactly one succeeds, the other gets `409`. | SCN-10 |

### 6.5 Host and co-host powers (`HST`)

| ID | Requirement | Scenarios |
|---|---|---|
| HST-001 | Powers follow the table in section 4. | all |
| HST-002 | **Kick** removes the participant, frees the seat (lobby) or vacates it (after start), and blocks that identity from rejoining the room. There is no unblock in v1. | SCN-13 |
| HST-003 | Only the host promotes or demotes co-hosts. | SCN-14 |
| HST-004 | **Leave**: in the lobby it frees the seat. After start, leaving the room does not release the seat; the participant must explicitly release it. Post-start release and replacement follow `SET-005`. | SCN-12, SCN-62 |
| HST-005 | **Start** (lobby only) requires every seat claimed, at least the draft type's minimum seats, valid settings. Randomisation happens on the server and is written to the log. | SCN-50, SCN-51 |
| HST-006 | **Reset to lobby** (in progress only) discards the draft state. Participants, seats, chat and log stay. Type and settings become editable again. | SCN-50 |
| HST-007 | **Undo last pick** (picks phase only). Revealed bans and Finished drafts cannot be undone. | SCN-71 |
| HST-008 | **Decide for** a seat: host or co-host completes the pending action for the active seat (turn phases) or for any seat that has not locked in (simultaneous phases). The log names the host and the seat and lists every hidden fact that the action reveals. The UI warns before the host confirms. | SCN-60, SCN-71 |
| HST-009 | **Close room** deletes the room, its seats, participants, chat, log and draft state. Connected clients see a "room closed" screen. | SCN-17 |
| HST-010 | Host and co-host have no extra read access to hidden information. | SCN-60 |

### 6.6 Live behaviour (`LIV`)

| ID | Requirement | Scenarios |
|---|---|---|
| LIV-001 | After a command commits, connected participants learn about it within about 1 second via SignalR and refresh their own view. | SCN-20 |
| LIV-002 | Presence: online dot per participant. "joined" when the first connection opens. "left" after 30 seconds without a connection. Explicit Leave or Kick is immediate. | SCN-21 |
| LIV-003 | The client reconnects automatically, resubscribes and refetches. A banner is visible while disconnected. | SCN-22 |
| LIV-004 | When it is my turn (turn phase) or my action is needed (simultaneous phase), a banner is shown and, while the tab is hidden, the tab title alternates between the normal title and "Your turn" once per second. It stops when the tab becomes visible or the turn ends. A per-user switch (stored in the browser, default on) disables blinking. | SCN-72 |
| LIV-005 | No sound and no desktop notifications in v1. | - |
| LIV-006 | The hub only pushes: a version number, public chat messages, public log entries, presence changes, room closed, removed from room. Commands use REST. | SCN-20 |

### 6.7 Chat and activity log (`CHT`)

| ID | Requirement | Scenarios |
|---|---|---|
| CHT-001 | Chat is plain text, 1-300 characters, persisted. The latest 200 messages load with the room. | SCN-30, SCN-31 |
| CHT-002 | System messages "X joined" and "X left" appear in the chat. | SCN-31 |
| CHT-003 | Spectators can chat. There is no host delete or mute in v1. A hidden anti-flood guard (5 messages per 5 seconds per participant) answers politely. | SCN-30, SCN-33 |
| CHT-004 | The activity log is append-only and visible to all participants. It lists room, seat, settings, identity-link and draft actions. It never contains secrets and never contains hidden information except as disclosed by `HST-008`. | SCN-32 |
| CHT-005 | Chat and log never cross rooms. | SCN-30 |

### 6.8 Draft engine (`ENG`)

| ID | Requirement |
|---|---|
| ENG-001 | The room core knows seats, participants, lifecycle, chat and log. It knows nothing about factions, slices or tiles. Draft types plug in through `IDraftType`. |
| ENG-002 | A draft type declares: settings and validation, ordered phases, accepted actions, visibility of every field, result builder, text formatter. |
| ENG-003 | The server is authoritative. Clients send intents. The server checks identity, role, turn, state version and rules. |
| ENG-004 | Phase kinds: **Turn** (one active seat), **Simultaneous** (all listed seats act, hidden until all locked), **HostOnly**, **Automatic**. |
| ENG-005 | Choosing is two steps: a pending selection (private to the seat, editable) and a lock-in (public, final). |
| ENG-006 | Every field has a visibility: Public, SeatPrivate or ServerOnly. The only code that produces data for a client is `Project(state, viewer)`. No endpoint or event returns raw state. |
| ENG-007 | Randomness comes from a CSPRNG on the server. Results (for example pick order) are written to the log. |
| ENG-008 | Each draft type persists in its own tables. The room stores `DraftType`. Changing the type in the lobby deletes the old type's rows. |
| ENG-009 | Type, settings and seat count lock at start. Reset unlocks them. |
| ENG-010 | Adding a draft type means adding a module (engine, tables, DTOs, Web components). The room core does not change. |

### 6.9 Faction Pick Draft (`FPD`)

| ID | Requirement | Scenarios |
|---|---|---|
| FPD-001 | The pool is the factions of the selected game versions (`IFactionRepository.GetFactionsByGameVersions`). | SCN-50 |
| FPD-002 | Settings are in section 8. | SCN-50 |
| FPD-003 | Start is refused when the pool cannot cover `seats x picksPerPlayer` after bans. | SCN-50 |
| FPD-004 | Phases: Bans (skipped for mode None), Picks, Finished. | SCN-60, SCN-70 |
| FPD-005 | Host bans: the host (or co-host) toggles factions as banned (public immediately), then confirms. | SCN-60 |
| FPD-006 | Simultaneous bans: every seat privately selects up to `BansPerPlayer` factions and locks in. Lock state is public, content is not. When all seats are locked, all bans are revealed at the same moment. The same faction banned by several seats counts once. | SCN-60 |
| FPD-007 | Picks: the order is computed at start (random or host-defined). With Snake on, the direction reverses every round. The active seat selects a faction (private) and locks in (public). Banned and picked factions cannot be selected. Each seat makes `PicksPerPlayer` picks. | SCN-70 |
| FPD-008 | When the last pick is locked in the room becomes Finished and a result snapshot is created. | SCN-73 |
| FPD-009 | Hidden information in v1 is limited to pending selections. | SCN-60 |
| FPD-010 | Decide-for works in simultaneous bans and in picks. Undo works in picks. | SCN-60, SCN-71 |

### 6.10 Results and history (`RES`)

| ID | Requirement | Scenarios |
|---|---|---|
| RES-001 | On finish an immutable snapshot is stored: type, settings, order, bans, picks per seat, who decided for whom. It does not depend on the room. | SCN-80 |
| RES-002 | The finished room shows the result read-only with **Copy as text**. | SCN-80 |
| RES-003 | A public page `/tools/draft-rooms/results/{token}` (128-bit token, `noindex`) shows display names, seats, bans, picks. It shows no chat, no log and no hidden information. | SCN-80 |
| RES-004 | An account sees its history: finished drafts it owns or in which its account held a seat (also seats linked after the draft), newest first. Including ownership keeps an unseated account host's results manageable after room cleanup. | SCN-80 |
| RES-005 | Host or co-host (while the room exists) or the owning account (afterwards) can delete a result. The link then answers `404` and history entries disappear. | SCN-80 |
| RES-006 | Closing a room never deletes results. | SCN-80 |

---

## 7. Draft engine model

### 7.1 How the pieces fit

```text
Web (Blazor)                       API                          Business / Draft
-----------                       ---                          ----------------
Room page  --REST commands-->  DraftRoomsController  -->  Command pipeline
   ^                                                          |  1. resolve identity -> participant
   |  SignalR: "RoomChanged(version)"                         |  2. authorise (role, seat, state)
   |  chat/log/presence events                                |  3. take per-room gate + transaction
   |                                                          |  4. IDraftType.Apply(...)  (pure rules)
   +--REST: GET view (personalised)  <-- Project(state, viewer)  5. save state, log, Version++
                                                              |  6. notify hub after commit
```

Commands go through REST. The hub only notifies. Every client then calls `GET api/draft-rooms/{code}` and receives **its own** view, produced by `Project(state, viewer)`. This gives one place where hidden information can leak, and one place to test it (`ENG-006`, `SCR-003`).

### 7.2 Phase kinds

| Kind | Who acts | Visible while running | Done when |
|---|---|---|---|
| Turn | One seat at a time | Pending: that seat only. Locked: everyone. | The seat locks in. |
| Simultaneous | Every listed seat | Pending: own seat only. "Locked in" flag: everyone. | All locked, then everything is revealed together. |
| HostOnly | Host or co-host | Public as it happens. | Host confirms. |
| Automatic | Server | Public result. | Immediately. |

### 7.3 Decide for (public disclosure)

When the host or a co-host decides for a seat, the server first builds the **disclosure set** (everything hidden that belongs to that seat and is touched by the action, in v1 the seat's pending selection), makes it public, writes it to the log together with the host's name, and then applies the host's choice. The host never sees the hidden data before it is public for everyone.

### 7.4 Undo and reset

- Undo removes the last locked-in pick, restores the pool and the turn pointer, clears pending selections, and logs the undo. It is unavailable for revealed bans and after Finished.
- Reset deletes type-specific runtime rows (phase, turn cursor, runtime order and choices), preserves configured rules, participants, seats and chat, sets the state back to Lobby, and logs the reset.

### 7.5 Seat occupant changes (hidden-information safety)

Picks belong to seats. If the person on a seat changes after start (host unclaim, assign, kick), the seat's **unrevealed** selections (pending, or locked but not yet revealed, for example secret bans before the reveal) are deleted without revealing them, and the seat's lock-in flag is cleared (`SET-006`). Otherwise the next occupant would inherit the previous person's secret choice.

### 7.6 How the planned draft types fit (design on paper, only #1 is built)

| | #1 Faction Pick | #2 Milty | #3 Galaxy-building | #4 Random galaxy |
|---|---|---|---|---|
| Settings | Bans mode, pick order mode, picks per player, snake, game versions | Same as the existing Milty Draft settings | Tiles per player (for example 2 red + 3 blue + home systems; depends on player count), order | Generation options, seat and strategy-card order rules |
| Phases | Bans (host / simultaneous) then Picks (turn) | Picks (turn, snake): slice, faction, seat position | Deal (automatic, **private hands**) then Place (turn, snake, placement action) | Generate (automatic), assign seats, strategy-card order (automatic or turn) |
| Hidden information | Pending selections | Pending selections | Dealt hands (undecided: you answered "unsure", the engine supports both) | None |
| Needs from engine | Turn, Simultaneous, lock-in, decide-for, undo | Turn, multi-kind picks | Automatic deal, private state, placement action | Automatic phases |
| Existing code to reuse | `IFactionRepository` | `IDraftSlicesService`, slice balancers | `IGalaxyBuilder`, system tile services | `IGalaxyBuilder` |

---

## 8. Faction Pick Draft (draft type #1)

### 8.1 Settings

| Setting | Values | Default | Notes |
|---|---|---|---|
| GameVersions | BaseGame, ProphecyOfKings, ThundersEdge (multi-selection) | all three | Uses a collection of the existing sequential `GameVersion` enum, not enum flags. |
| BansMode | None, HostBans, SimultaneousPlayerBans | None | Runs before the picks. |
| BansPerPlayer | 1-3 | 1 | Only for SimultaneousPlayerBans. |
| HostBanCount | 0 up to the largest value that still leaves enough factions | 0 | Only for HostBans. |
| PickOrderMode | Random, HostDefined | Random | HostDefined: host orders the seats in the lobby with up/down buttons (no drag and drop, for keyboard users). |
| PicksPerPlayer | 1-5 | 1 | |
| Snake | on, off | on | Reverses direction every round when picks per player is above 1. |

Validation (`FPD-003`): `poolAfterBans >= seats x picksPerPlayer`, where `poolAfterBans` is the pool size minus the **maximum** possible bans (`BansPerPlayer x seats` for simultaneous, `HostBanCount` for host bans).

### 8.2 Phases and rules

1. **Bans** (skipped for None).
   - HostBans: host toggles factions (public at once), confirms, phase moves on.
   - SimultaneousPlayerBans: each seat selects, locks in; when all are locked every ban is revealed at once.
2. **Picks.** Order from `PickOrderMode`. The active seat selects a faction (private), locks in (public). The faction leaves the pool.
3. **Finished.** Result snapshot created.

### 8.3 Result content

Per seat: ordered picks, who decided (seat or host). Overall: bans, pick order, settings, finish time. Text format (one line per seat): `Seat 1 - Alice: Faction A, Faction B`.

---

## 9. Data model

### 9.1 Principles

- **Table per draft type.** Generic tables hold the room. Each draft type has its own tables, created by its own migration.
- The tables live in the **main `TwilightImperiumDbContext`**, schema `Draft`, with configurations in `DataAccess/Configurations/DraftRooms/` and migrations in the existing `Migrations/` folder (`DEC-28`). Mappings should avoid unnecessary provider-specific constructs; persistence and hub integration tests use the same SQL Server provider as production.
- Identity foreign-key columns use `nvarchar(450)` to match the existing `AspNetUsers.Id` column type.
- Concurrency: `DraftRooms.Version` is an int concurrency token that the code increments. A per-room in-memory gate (single instance, `DEC-08`) reduces conflicts, the token guarantees correctness.
- Guid keys for rooms, participants, seats. `long` identity keys for chat and log (cheap ordering and `afterId` paging).
- Enums are stored as strings.

### 9.2 Generic tables

| Table | Key columns | Constraints |
|---|---|---|
| `DraftRooms` | `Id` Guid PK, `Code` varchar(8), `Name` nvarchar(60), `PasswordHash` varchar(256), `Status`, `DraftType`, `IsLocked`, `Version` int (concurrency token), `CreatedAtUtc`, `LastActivityUtc`, `ResultId` Guid null (no FK) | Unique `Code`. |
| `DraftRoomParticipants` | `Id` Guid PK, `RoomId` FK (cascade), `UserId` nvarchar(450) null (FK to `AspNetUsers`, set null on user delete), `SecretHash` varbinary(32) null, `DisplayName` nvarchar(258), `DisplayNameKey` nvarchar(258), `Role`, `Status` (Active, Left, Kicked), `JoinedAtUtc`, `LastSeenUtc` | Account usernames are captured in full (Identity permits 256 characters); a numeric collision suffix can add up to two characters. Guest names remain limited to 24 characters. Unique (`RoomId`, `UserId`) where `UserId` is not null. Unique (`RoomId`, `DisplayNameKey`) for Active. |
| `DraftRoomSeats` (P03) | `Id` Guid PK, `RoomId` FK (cascade), `Index` int, `ParticipantId` Guid null (set null), `ClaimedAtUtc` null | Unique (`RoomId`, `Index`). Unique (`RoomId`, `ParticipantId`) where not null. |
| `DraftRoomChatMessages` (P06) | `Id` bigint identity, `RoomId` FK (cascade), `ParticipantId` null, `Kind` (User, Join, Leave), `Text` nvarchar(300), `CreatedAtUtc` | Index (`RoomId`, `Id`). |
| `DraftRoomLogEntries` (P06) | `Id` bigint identity, `RoomId` FK (cascade), `ActorParticipantId` null, `Type` varchar(48), `DataJson` nvarchar(max), `CreatedAtUtc` | Index (`RoomId`, `Id`). `DataJson` is public data only. |
| `DraftResults` (P11) | `Id` Guid PK, `ShareToken` varchar(22) unique with case-sensitive binary collation (128 random bits, base64url), `DraftType`, `RoomName`, `SourceRoomId` Guid null (no FK, unique when non-null), `OwnerUserId` nvarchar(450) null (host's account at finish or later linking, FK to `AspNetUsers`, set null), `FinishedAtUtc`, `PayloadVersion` int, `PayloadJson` nvarchar(max), `CreatedAtUtc` | Survives room deletion. |
| `DraftResultPlayers` (P11) | (`ResultId` FK cascade, `SeatIndex`) PK, `DisplayName`, `UserId` nvarchar(450) null (FK to `AspNetUsers`, set null) | Index on `UserId` for history. |

### 9.3 Faction Pick tables (phases P08-P10)

| Table | Content |
|---|---|
| `FactionPickSettings` (P08) | `RoomId` PK/FK, `GameVersions` flags, `BansMode`, `BansPerPlayer`, `HostBanCount`, `PickOrderMode`, `PicksPerPlayer`, `Snake`. |
| `FactionPickStates` (P08) | `RoomId` PK/FK, `Phase`, `TurnIndex`, `BansRevealed`. |
| `FactionPickTurnOrder` (P08) | `RoomId`, `Position`, `SeatId`. |
| `FactionPickSelections` (P09) | `Id`, `RoomId`, `SeatId` null (host bans), `Kind` (Ban, Pick), `Faction`, `State` (Pending, Locked), `Round`, `DecidedByParticipantId` null, `CreatedAtUtc`, `LockedAtUtc`. Pending rows, and locked bans before the reveal, are SeatPrivate. |

### 9.4 Retention

| Data | Rule |
|---|---|
| Room and everything under it | Deleted by close, or by the daily job after 30 days without activity (`DraftRooms:InactiveRoomRetentionDays`, default 30). |
| Result snapshots | Kept until host or co-host deletes them. |

---

## 10. API and real-time contract (initial, refined per phase)

Base route `api/draft-rooms`. Responses use the existing `ApiResponse<T>` wrapper. JSON enums are strings. Status codes: 200/201, 400 validation, 401 bad identity, 403 not allowed, 404 not found (also used for wrong code or password), 409 stale version or conflict, 429 throttled.

| Method and route | Who | Phase |
|---|---|---|
| `POST api/draft-rooms` create | account or guest | P01 |
| `POST api/draft-rooms/join` join or re-enter | account or guest | P01 |
| `GET api/draft-rooms/{code}` my personalised view | member | P03 |
| `POST api/draft-rooms/{code}/seats`, `DELETE .../seats/{seatId}` | host, co-host | P03 |
| `POST .../seats/{seatId}/claim`, `.../unclaim` | member, host, co-host | P03 |
| `POST .../seats/{seatId}/assign` (after start) | host, co-host | P10 |
| `POST .../leave` | member | P03 |
| `POST .../participants/{id}/kick`, `.../role` | host, co-host (role: host only) | P03 |
| `PUT .../settings` (name, lock, draft type), `PUT .../password` | host, co-host | P03, P08 |
| `DELETE api/draft-rooms/{code}` close | host, co-host | P03 |
| `GET .../chat?afterId=`, `POST .../chat`, `GET .../log?afterId=` | member | P06 |
| `POST .../link-account` | account + guest headers | P07 |
| `GET api/draft-rooms/mine` | account | P07 |
| `PUT .../draft/settings`, `POST .../draft/start`, `POST .../draft/reset` | host, co-host | P08 |
| `POST .../draft/actions`, `POST .../draft/decide-for` | seat holder / host, co-host | P09 |
| `POST .../draft/undo` | host, co-host | P10 |
| `GET api/draft-rooms/results/{token}` | anyone | P11 |
| `GET api/draft-rooms/results/mine`, `DELETE api/draft-rooms/results/{id}` | account / host, co-host | P11 |

Guest identity on REST: headers `X-Draft-Participant-Id` and `X-Draft-Participant-Secret`. Account identity: the normal bearer token. Responses that contain a secret send `Cache-Control: no-store`.

**Hub** `/hubs/draft-rooms`
- Client to server: `Subscribe(code, credentials)` (guest secret travels inside the message, never in the URL).
- Server to client: `RoomChanged(version)`, `ChatMessageReceived(dto)`, `LogEntryAdded(dto)`, `PresenceChanged(participantId, isOnline)`, `RoomClosed()`, `RemovedFromRoom()`.

**Polymorphic DTOs.** `DraftSettingsDto`, `DraftViewDto` and `DraftActionDto` are abstract bases with `System.Text.Json` polymorphism (`type` discriminator). A new draft type adds derived DTOs only.

---

## 11. Security and privacy (`SCR`)

| ID | Requirement |
|---|---|
| SCR-001 | Room passwords are hashed with the ASP.NET Core `PasswordHasher` (PBKDF2). Never logged, never returned. |
| SCR-002 | The guest secret is 256-bit CSPRNG output, base64url. Only a SHA-256 hash is stored. Compared with `CryptographicOperations.FixedTimeEquals`. Sent only in a header (REST) or inside the `Subscribe` message (hub), never in a URL. |
| SCR-003 | Hidden information leaves the server only through `Project(state, viewer)`. There are tests for every viewer class: own seat, other seat, host, co-host, spectator. |
| SCR-004 | Every command re-checks identity, participant status, role, seat, turn and state version on the server. The client is never trusted. |
| SCR-005 | Join and create are throttled (`ROM-005`). Wrong code and wrong password answer identically (`ROM-004`). |
| SCR-006 | A hub connection is not trusted until `Subscribe` succeeds. One group per room. Hub events carry no hidden data. A bearer token in the `access_token` query string is accepted only on `/hubs/*`. |
| SCR-007 | CORS keeps explicit origins. Credentials are allowed only for the hub policy. |
| SCR-008 | All text input is trimmed, length-limited and rendered as text. `MarkupString` is never used for user content. |
| SCR-009 | Result tokens are 128-bit random. The public result page returns only the public snapshot. |
| SCR-010 | Logs contain no passwords, secrets or tokens. Room codes may be logged. |
| SCR-011 | The login `returnUrl` accepts only local paths (no open redirect). |
| SCR-012 | Abuse caps: 5 open rooms per account, 32 participants per room, chat anti-flood guard, creation rate limit. |

---

## 12. UI structure

### 12.1 Pages and layout

| Route | File | Layout |
|---|---|---|
| `/tools/draft-rooms` | `Pages/Tools/DraftRooms.razor` (renamed stub) | Main layout, `Page` component |
| `/tools/draft-rooms/{Code}` | `Pages/Tools/DraftRoom.razor` | `Pages/Layouts/DraftRoomLayout.razor` (bare) |
| `/tools/draft-rooms/results/{Token}` | `Pages/Tools/DraftRoomResult.razor` | Main layout, `noindex` |

Route constants are added to `Pages/Pages.cs`.

### 12.2 Room screen

- **Top bar**: room name, code and link with `CopyButton`, role badge, connection status, Leave (or Close for host and co-host).
- **Main area**: lobby (seats, host settings) or the draft type's board.
- **Side panel (desktop)**: participants, chat, log. **Mobile**: tabs Draft, Seats, Chat, Log (`ResponsiveTab`).
- **Turn banner**: shown whenever my action is needed.

### 12.3 Components

All under `Components/DraftRooms/` with a `.razor` and `.razor.cs` each (`.razor.css` when needed): `CreateDraftRoomForm`, `JoinDraftRoomForm`, `DraftRoomHeader`, `DraftSeatList`, `DraftSeatCard`, `DraftParticipantList`, `DraftRoomHostSettings`, `DraftRoomChatPanel`, `DraftRoomLogPanel`, `DraftRoomConnectionStatus`, `TurnAlertTitle`, `MyRoomsList`, `DraftRoomResultView`. Faction Pick: `Components/DraftRooms/FactionPick/` (`FactionPickSettingsEditor`, `FactionPickBoard`, `FactionPickResultView`). A UI registry maps `DraftTypeId` to these components (`DynamicComponent`).

Reuse before creating: `ResponsiveForm`, `ResponsiveFormField`, `ResponsiveTextBox`, `ResponsivePassword`, `ResponsiveButton`, `ResponsiveCard`, `ResponsiveContainer`, `ResponsiveTab`, `ResponsiveDropDown`, `ResponsiveNumericInput`, `ResponsiveSwitch`, `ResponsiveConfirmDialog`, `ResponsiveHeading`, `ResponsiveText`, `CopyButton`, `StatusMessages/*`.

---

## 13. Non-functional requirements (`NFR`)

| ID | Requirement |
|---|---|
| NFR-001 | WCAG 2.2 AA. Labels and errors for every field. Chat and log are `role="log"` with `aria-live="polite"`. The seat list works with the keyboard. Moving focus after panel swaps. |
| NFR-002 | The tab-title blink toggles at most once per second, can be switched off, and a visible banner always accompanies it. |
| NFR-003 | Layout works from 360 px width. Chat and log become tabs on small screens. |
| NFR-004 | English only. Every user-visible string is in `Strings.resx` (and `Strings.Designer.cs`). API paths are in `Paths.resx`. |
| NFR-005 | Limits: 8 seats, 32 participants, 300-character chat. A view response stays small (no history beyond the latest 200 chat messages). |
| NFR-006 | Serilog events for room created, joined, started, finished, closed, throttled, and auth failures. No secrets (`SCR-010`). |
| NFR-007 | Every phase leaves `dotnet build` and `dotnet test` green. New warnings on touched files are fixed in the same task. |

---

## 14. Testing strategy

| Layer | Tool | Project | Introduced | Used for |
|---|---|---|---|---|
| Unit | xUnit, Moq, FluentAssertions, Bogus | `tests/TwilightImperiumUltimate.Tests` (exists) | P01 | Hashing, code generation, validators, command handlers, authoriser matrix, engine rules, projection and redaction. |
| Component | bUnit | `tests/TwilightImperiumUltimate.ComponentTests` (new) | P00 | Forms, panels, lobby, boards, validators of the Web project. |
| Persistence | EF Core with a uniquely named temporary SQL Server LocalDB database per test fixture | `tests/TwilightImperiumUltimate.IntegrationTests` (new) | P00 (project and fixture), P01 (first use) | Mappings, constraints, atomic claims, concurrency token, cascade delete. |
| Hub and API | `WebApplicationFactory`, SignalR client | same `IntegrationTests` | P05 | Subscribe, notifications, presence, redaction through real endpoints. |
| End to end | Playwright | `tests/TwilightImperiumUltimate.E2ETests` (new) | P12 | Create, join, claim, chat, a full Faction Pick draft with several browser contexts. |

Rule: **hidden information has a test for every viewer class** in P09 and P10. Each task states how you verify it. `docs/build-and-test.md` is updated when a test project is added.

---

## 15. Delivery approach

- One long-lived branch `feature/draft-rooms`, **one commit per task**, message `Draft Rooms TASK-xxxx: <title>`.
- Every commit builds and passes the tests that exist at that point.
- A phase ends with a **gate**: a checklist you tick yourself (commands plus acceptance scenarios). Do not start the next phase before the gate passes.
- Standard checks for every task: `dotnet build TwilightImperiumUltimate.slnx`, `dotnet test` for the affected test projects, `dotnet build TwilightImperiumUltimate.slnx --configuration Release` (StyleCop and Sonar analyzers run only in Release), `dotnet format TwilightImperiumUltimate.slnx --verify-no-changes`. No **new** findings in touched files.
- Manual scenarios use two browsers: a normal window and a private window (or two profiles), because both share one local storage per profile.

### 15.1 Acceptance scenarios

| ID | Phase | Scenario |
|---|---|---|
| SCN-01 | P02 | Logged out, open `/tools/draft-rooms`, Create Draft, name "Friday TI", password "friday1", display name "Alice". A new tab opens at `/tools/draft-rooms/{CODE}` with a bare layout showing the room name, the code and "Alice (Host)". |
| SCN-02 | P02 | Logged in as an account: no display-name field, the room shows the account username as host. |
| SCN-03 | P02 | In a private window, Join Draft with the code, password and name "Bob". A new tab opens, Bob is a member. |
| SCN-04 | P01, P02 | Wrong code and wrong password show the same message. Five wrong attempts for one code answer `429` for 15 minutes. |
| SCN-05 | P02 | Open the room link in a third profile: the join form appears inside the room page and joining keeps the same tab. |
| SCN-06 | P02 | Close the room tab, open the link again in the same browser: re-enter without a password. |
| SCN-07 | P01, P02 | A duplicate display name is rejected. A guest cannot use a registered username. |
| SCN-08 | P02 | The room tab has no site header, menu or footer. The Tools menu shows Draft Rooms. The room page source contains `noindex`. |
| SCN-10 | P03, P04 | Host sits on Seat 1. Bob claims Seat 2. A second claim of an occupied seat is refused. |
| SCN-11 | P03, P04 | Host adds and removes seats. Seats stay within 2-8. An occupied seat cannot be removed. |
| SCN-12 | P03, P04 | A participant unclaims, moves seats, leaves (lobby frees the seat). |
| SCN-13 | P03, P04 | Host unclaims Bob. Host kicks Bob. Bob cannot rejoin with the right password. |
| SCN-14 | P03, P04 | Host promotes Bob to co-host. Co-host edits seats and settings but cannot kick the host or promote anyone. |
| SCN-15 | P03, P04 | Host locks the room: a new person cannot join, Bob can re-enter. |
| SCN-16 | P03, P04 | Host changes the password: new joins need the new one, Bob is unaffected. |
| SCN-17 | P03, P04 | Host closes the room: every open tab shows "room closed". |
| SCN-20 | P05 | Two browsers: a seat claim shows up in the other within about 1 second, without refresh. |
| SCN-21 | P05 | Closing a tab turns the online dot grey. After about 30 seconds the chat shows "left". |
| SCN-22 | P05 | Restart the API: both tabs show the reconnect banner, then recover on their own. |
| SCN-30 | P06 | A chat message reaches everyone in about 1 second. A spectator can chat. 300-character limit holds. Another room never receives it. |
| SCN-31 | P06 | Join and leave show in chat. A page refresh keeps the last 200 messages. |
| SCN-32 | P06 | The log lists seat, kick, promote and settings events, with no secrets. |
| SCN-33 | P06 | Sending more than 5 messages in 5 seconds shows a polite refusal. |
| SCN-40 | P07 | A guest holding a seat logs in and returns to the room, is offered "Link to your account", accepts. Seat and role are kept. The guest secret is gone from local storage. |
| SCN-41 | P07 | If the account already is a participant of that room the link is refused with a clear message. |
| SCN-42 | P07 | "My rooms" lists rooms for an account and (separately) for a guest browser. Clicking re-enters without a password. |
| SCN-43 | P07 | A login `returnUrl` pointing to another site is ignored. |
| SCN-50 | P08 | Host edits Faction Pick settings, others see the changes live and read-only. Invalid settings block Start with a reason. Start needs all seats claimed. After Start type, settings and seat count are locked. Reset unlocks them. |
| SCN-51 | P08, P10 | Random order is announced in the log (P08). Host-defined order is used as given (P10). |
| SCN-60 | P09 | Simultaneous bans: Alice selects ban X. Bob's, the host's and a spectator's choice-bearing response fields and DOM selection state do not disclose Alice's choice (check the network tab and the redaction tests). The public faction pool may contain X; its presence alone is not a privacy leak. Lock flags are visible. When all are locked all bans appear together. Decide-for reveals the pending selection in the log. |
| SCN-61 | P09, P10 | After start the host unclaims Alice's seat: her unrevealed selection is discarded, not revealed (P09). The host can then assign a spectator to the vacant seat (P10). |
| SCN-62 | P10 | After start Alice releases her own seat and can reclaim it while vacant, but cannot claim a different seat. Bob, an active unseated participant, can claim Alice's released seat; Alice then cannot reclaim it. The seat's unrevealed selections are discarded when its occupant changes, while public locked picks remain. |
| SCN-70 | P10 | Three seats, two picks each, snake order: the order is 1-2-3-3-2-1. A non-active seat cannot select. Banned and picked factions are unavailable. Lock-in is final. |
| SCN-71 | P10 | Host undoes the last pick. Host decides for the active seat: the log shows the reveal. |
| SCN-72 | P10 | When it is my turn and the tab is hidden the title blinks. Focusing the tab stops it. The switch turns it off. |
| SCN-73 | P10 | The last pick moves the room to Finished. |
| SCN-80 | P11 | Result visible in the room, Copy as text works, the public link opens in a private window, history lists it for the account, deleting the result makes the link answer 404, the cleanup job removes a room inactive for 30 days but keeps results. |
| SCN-90 | P12 | The Playwright suite passes. Security and accessibility checklists are done. |

---

## 16. Assumptions to confirm, risks, open items

### 16.1 Assumptions (change any of them and I update the plans)

| ID | Assumption | Why |
|---|---|---|
| ASM-01 | Database integration tests use a uniquely named, temporary SQL Server LocalDB database for each test fixture. The fixture creates it, applies the full EF Core model with `EnsureCreated`, and drops it on disposal. | You chose the main context (`DEC-28`) and confirmed SQL Server LocalDB as the only test database provider (`DEC-24`). Using SQL Server avoids maintaining a second provider and tests the mappings against the production database engine. |
| ASM-02 | The guest identity lives in **browser local storage** per room, not in an HTTP cookie. | The API is a different origin, tokens already live in local storage, local storage is shared across tabs, so the new tab sees it. A cookie would need credentialed CORS for every call. |
| ASM-04 | A **Finished** draft is final: no undo, no reset. | Results are public and immutable. Mistakes on the last pick cannot be fixed, so the host should check before locking the final pick. |
| ASM-05 | Co-host powers: everything except kicking or demoting the host and promoting or demoting co-hosts. Co-hosts can close the room. | Prevents a stuck room if the host disappears (`DEC-16`). |
| ASM-06 | A hidden anti-flood guard stays on chat even though "rate limit" was not selected. | Security safeguard for a public input, not a feature. |
| ASM-07 | Presence grace is 30 seconds before "left". | Avoids join/leave spam on refresh. |
| ASM-08 | A new room has 6 seats, the host on Seat 1. Seats range 2-8. | TI4 default. |
| ASM-09 | Kicked identities cannot be unblocked in v1. Changing the room password does not unblock them. | Keeps v1 small. |
| ASM-10 | The public result page uses the main layout. The room page uses the bare layout. | The result page is a shareable website page. |
| ASM-11 | The blink itself is plain Blazor (`PageTitle` plus `PeriodicTimer`). Blazor cannot observe whether the tab is hidden, so one tiny JS module reports that single boolean (`visibilitychange`). | Repository rule: avoid JS interop when .NET can do it. Visibility is the part .NET cannot do. |
| ASM-12 | Opening the new tab needs a small JS call (`window.open`). A fallback **Open room** link is always shown. | Blazor has no API for opening a new tab after an async call. |
| ASM-13 | The account display name is the username at the time of joining. | Simple and stable for logs and results. |
| ASM-14 | Faction Pick defaults: all three game versions, no bans, random order, one pick, snake on. | Section 8. |
| ASM-15 | A guest cannot use a registered username as display name. | Prevents impersonation. |
| ASM-16 | Closing a room leaves finished results untouched. Only "Delete results" removes them. | `RES-006`. |

### 16.2 Risks

| ID | Risk | Mitigation |
|---|---|---|
| RSK-01 | LocalDB tests use the SQL Server engine but do not fully substitute for checking migrations and configuration against the deployment SQL Server instance. | Use LocalDB for automated persistence tests, and manually apply, inspect, and roll back each migration against the deployment SQL Server environment before release. |
| RSK-02 | Behind a reverse proxy every request may look like it comes from one IP, which would make IP throttling punish everyone. | Task in P01 verifies `RemoteIpAddress` on the deployed environment and configures forwarded headers if needed. |
| RSK-03 | SignalR across origins may need credentialed CORS and bearer-from-query handling. | P05 starts with a connectivity spike and applies the smallest working change. |
| RSK-04 | Browsers throttle timers in hidden tabs, so blinking may slow down after a few minutes. | In-page banner always visible, JS fallback task, findings documented. |
| RSK-05 | Popup blockers (especially Safari) block `window.open` after an `await`. | Fallback link (`LND-003`). |
| RSK-06 | Guest identifiers, display names, chat text and public result links are personal data and the site has no privacy notice. | Your decision `DEC-23`. Revisit before wide release. |
| RSK-07 | A seat's next occupant could inherit hidden choices. | `SET-006` plus a test. |
| RSK-08 | One API instance: a restart drops live connections. | State is in the database, clients reconnect and refetch (`LIV-003`). |
| RSK-09 | `Strings.Designer.cs` is huge and generated, easy to conflict. | One small commit per task and a build after each edit. |
| RSK-10 | Existing issue, **not part of this work**: `DiscordAuthController.cs` line 88 logs the raw Discord token response. | Reported to you earlier, left untouched. |
| RSK-11 | Existing issue, **not part of this work**: the SMTP password is stored in plain text in `src/TwilightImperiumUltimate.API/appsettings.json` (around line 40), so it is in source control. | Rotate it and move it to user secrets or an environment variable. Left untouched. |
| RSK-12 | `EnableSensitiveDataLogging()` is on in the existing context registration, so EF error messages can contain parameter values (hashes, chat text). Serilog filters the `Microsoft` namespace to Warning, but errors still log. | Handlers never log whole entities. The log check in P01 includes a forced failure. |

### 16.3 Open items

None blocking. Phases P08-P12 are planned at task level but will be re-checked against the code at the start of each phase (the first task of those plans is a short re-validation).
