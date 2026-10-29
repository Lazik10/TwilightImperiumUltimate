# Draft Rooms implementation plans

Specification: [`docs/draft-rooms/specification.md`](../docs/draft-rooms/specification.md)

The specification says what Draft Rooms does. These files say in which small steps it gets built. Work on **one phase at a time**: finish its tasks, tick its **phase gate**, and only then open the next file.

**Current status:** implementation is available through P12. See [P12](feature-draft-rooms-p12-e2e-hardening-1.md) for final automated evidence and the outstanding manual/deployment acceptance gates. Checkmarks record implementation and automated checks, not manual scenarios that the user deferred.

## 1. Roadmap

| Phase | Plan file | What you can verify when it is done | Needs | Tasks |
|---|---|---|---|---|
| P00 | [Foundations](feature-draft-rooms-p00-foundations-1.md) | Three test projects run from the solution, persistence tests use temporary SQL Server LocalDB databases, baseline recorded. Nothing user visible. | none | TASK-0001 to TASK-0008 |
| P01 | [Rooms backend](feature-draft-rooms-p01-rooms-backend-1.md) | Create and join a room through the API (PowerShell script): account or guest, password, throttling. Tables exist. | P00 | TASK-0101 to TASK-0116 |
| P02 | [Landing page and entry](feature-draft-rooms-p02-landing-and-entry-1.md) | **MVP.** Landing page, Create and Join forms, room opens in a new bare tab, Tools menu link. | P01 | TASK-0201 to TASK-0212 |
| P03 | [Seats backend](feature-draft-rooms-p03-seats-backend-1.md) | Seats, claim, kick, co-host, lock, password, close on the API (script). Per-room transaction pipeline and permission matrix. | P02 | TASK-0301 to TASK-0313 |
| P04 | [Lobby UI](feature-draft-rooms-p04-lobby-ui-1.md) | Seat list, people list, host settings in the room page. Manual **Refresh**. | P03 | TASK-0401 to TASK-0408 |
| P05 | [Live with SignalR](feature-draft-rooms-p05-live-signalr-1.md) | Changes appear in other browsers within about 1 second. Presence. Hub integration tests. | P04 | TASK-0501 to TASK-0512 |
| P06 | [Chat and log](feature-draft-rooms-p06-chat-and-log-1.md) | Live chat, join and leave messages, activity log. | P05 | TASK-0601 to TASK-0608 |
| P07 | [Account link and My rooms](feature-draft-rooms-p07-account-link-my-rooms-1.md) | Guest seat linked to an account, safe login return, "My rooms" list. | P06 | TASK-0701 to TASK-0706 |
| P08 | [Draft engine and settings](feature-draft-rooms-p08-draft-engine-1.md) | Pluggable engine, draft type choice, Faction Pick settings, Start and Reset, locking. | P07 | TASK-0801 to TASK-0813 |
| P09 | [Faction Pick: bans](feature-draft-rooms-p09-faction-pick-bans-1.md) | Bans (none, host, simultaneous secret), hidden information proved by tests, decide-for disclosure. | P08 | TASK-0901 to TASK-0910 |
| P10 | [Faction Pick: picks](feature-draft-rooms-p10-faction-pick-picks-1.md) | Snake picks, undo, assign seat after start, blinking tab. A full draft can be played. | P09 | TASK-1001 to TASK-1010 |
| P11 | [Results and retention](feature-draft-rooms-p11-results-retention-1.md) | Result snapshot, copy as text, public link, account history, 30-day cleanup. | P10 | TASK-1101 to TASK-1110 |
| P12 | [E2E, hardening, release](feature-draft-rooms-p12-e2e-hardening-1.md) | Playwright journeys, security and accessibility reviews, load check, docs, release checklist. | P11 | TASK-1201 to TASK-1210 |

P08 to P12 were re-checked against the implementation at the start of each phase. Faction Pick is the first supported room engine; other draft types remain future work rather than unfinished tasks in these plans.

## 2. How identifiers work

- Every plan file uses four-digit identifiers: the first two digits are the phase, the last two the running number. `TASK-0207` is task 7 of phase P02. `GOAL-0302`, `REQ-0901`, `RISK-1001` follow the same scheme, so every identifier is unique across all files.
- Specification identifiers use area codes instead: `LND`, `ROM`, `IDN`, `SET`, `HST`, `LIV`, `CHT`, `ENG`, `FPD`, `RES`, `SCR`, `NFR` (requirements), `DEC` (decisions), `ASM` (assumptions), `RSK` (risks), `SCN` (acceptance scenarios).
- Each task row has a **Verify** column: the exact command or click path that proves the task is done. Tick **Completed** and write the date when it passes.

## 3. Rules for every task

1. Work on branch `feature/draft-rooms`. **One commit per task**, message `Draft Rooms TASK-0207: <task title>`.
2. Every commit builds and passes the tests that exist at that point.
3. Standard checks before each commit, from the repository root:

```powershell
dotnet build TwilightImperiumUltimate.slnx
dotnet test TwilightImperiumUltimate.slnx -c Release -m:1
dotnet build TwilightImperiumUltimate.slnx --configuration Release
dotnet format TwilightImperiumUltimate.slnx --verify-no-changes
```

   StyleCop and Sonar analyzers run only in the Release build (`Directory.Build.props`). The rule is **no new findings in files you touched** (compare with the baseline in section 5).
   The complete solution now includes real browser tests. Publish Web and install Chromium first using [the E2E prerequisites](../docs/build-and-test.md#14-end-to-end-tests). The one-minute load check is explicitly opt-in.
4. A new user-visible string goes into `Strings.resx` **and** `Strings.Designer.cs`, a new API path into `Paths.resx` **and** `Paths.Designer.cs`. The designer files are generated by Visual Studio on save, command-line builds do not regenerate them.
5. Follow the repository instructions: `.github/instructions/csharp.instructions.md` (StyleCop member order, nullability, `CancellationToken`), `blazor.instructions.md` (`.razor` plus `.razor.cs` for every component), `html.instructions.md`, `css.instructions.md`, `accessibility.instructions.md`, `rest-api.instructions.md`, `testing.instructions.md`.
6. Never log or return a password, a guest secret or hidden draft information.

## 4. How to run things locally

Open terminals in this worktree, not the ordinary/main checkout. In the API terminal, set the isolated database before starting:

```powershell
$env:ConnectionStrings__TwilightImperium = 'Server=(localdb)\MSSQLLocalDB;Database=TwilightImperiumUltimate_DraftRooms_Dev;Trusted_Connection=True;TrustServerCertificate=True'
dotnet run --project src\TwilightImperiumUltimate.API\TwilightImperiumUltimate.API.csproj
```

In a second terminal:

```powershell
dotnet run --project src\TwilightImperiumUltimate.Web\TwilightImperiumUltimate.Web.csproj
```

The normal API applies migrations at startup. Never point this worktree at the ordinary/main or production database for development testing. These manual server commands are documented for the user, not executed by the automated test fixtures.

Use **two browser profiles** for manual scenarios (for example a normal window and a private window), because one profile shares one local storage and therefore one guest identity. The scenario texts are in section 15.1 of the specification.

Draft room tables are part of the main `TwilightImperiumDbContext`, so the normal EF Core commands in `docs/build-and-test.md` apply (add migration, update database, roll back one step).

## 5. Baseline

Filled in by `TASK-0008` (build warnings per project in Release, `dotnet format --verify-no-changes` result, date). Later phases compare against it.

| Project | Release build warnings | Date |
|---|---|---|
| Solution baseline | Release build succeeded with 0 warnings and 0 errors after resolving the pre-existing analyzer and vulnerable transitive dependency warnings. | 2026-10-15 |

Formatting: `dotnet format TwilightImperiumUltimate.slnx --verify-no-changes` passed with no diagnostics on 2026-10-15. The baseline commands ran with P00 implementation changes present, so this was not a clean-tree baseline.

### 5a. Test database decision

Filled in by `TASK-0006`: the provider used by persistence and hub integration tests and the temporary database lifecycle.

| Result | Provider | Date |
|---|---|---|
| PASS: temporary database creation, `EnsureCreated`, insert/read, and cleanup verified. | SQL Server LocalDB (`(localdb)\MSSQLLocalDB`); one uniquely named database per fixture. | 2026-10-15 |

Persistence integration tests use `TestDatabase`, which creates a unique temporary SQL Server LocalDB database for each fixture and drops it when disposed.

## 6. Tracking progress

- Tick the **Completed** column and write the date in the plan file when a task's Verify step passes.
- Change `status` in the plan's front matter and the badge text from `Planned` to `In progress` when you start a phase and to `Completed` when its gate passes.
- Changes to the specification or to a plan are normal. Edit the file and say why in the commit message.
