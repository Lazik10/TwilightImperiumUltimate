---
goal: Draft Rooms P00 - Foundations and test infrastructure
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-15
owner: Lazik
status: 'Completed'
tags: [feature, draft-rooms, testing, infrastructure]
---

# Introduction

![Status: Completed](https://img.shields.io/badge/status-Completed-brightgreen)

Creates the working branch and the two test projects that later phases need (bUnit component tests, integration tests), establishes temporary SQL Server LocalDB as the integration-test provider, and records a build baseline. Nothing user visible changes. Specification: sections 14 and 15 of [`docs/draft-rooms/specification.md`](../docs/draft-rooms/specification.md), decisions `DEC-24`, `DEC-28`, assumption `ASM-01`.

## 1. Requirements & Constraints

- **REQ-0001**: Test projects follow the folder convention already recommended in `docs/build-and-test.md` (`tests/TwilightImperiumUltimate.ComponentTests/`) and are listed in `TwilightImperiumUltimate.slnx`.
- **REQ-0002**: The existing project `tests/TwilightImperiumUltimate.Tests` and its tests stay untouched.
- **REQ-0003**: Every later persistence and hub test gets its database from one helper, `TestDatabase`, which exposes an `IDbContextFactory<TwilightImperiumDbContext>` and creates a temporary SQL Server LocalDB database.
- **CON-0001**: Reuse the package versions already used in the repository: `xunit` 2.9.3, `xunit.runner.visualstudio` 3.1.5, `Microsoft.NET.Test.Sdk` 18.7.0, `FluentAssertions` 8.10.0, `Moq` 4.20.72, `coverlet.collector` 10.0.1, EF Core 10.0.9.
- **CON-0002**: Every task ends with the standard checks from `plan/README.md` section 3 and exactly one commit.
- **CON-0003**: Persistence and hub integration tests run against SQL Server LocalDB. No alternate EF Core provider or provider-compatibility spike is introduced.
- **GUD-0001**: New packages are test-only. Record each new package and its licence in the commit message (bUnit: MIT, `Microsoft.EntityFrameworkCore.SqlServer`: MIT).

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-0001: Branch, test projects and solution wiring

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0001 | Create the branch `feature/draft-rooms` from the current branch (`git switch -c feature/draft-rooms`). Commit `docs/draft-rooms/specification.md` and the whole `plan/` folder as the first commit. | Branch is `feature/draft-rooms`; first commit `07d6b6f` contains the specification and all plan files only. | ✅ | 2026-10-15 |
| TASK-0002 | Create `tests/TwilightImperiumUltimate.ComponentTests/TwilightImperiumUltimate.ComponentTests.csproj` (`net10.0`, nullable and implicit usings on, packages from CON-0001 plus the newest stable `bunit` that supports `net10.0`, `ProjectReference` to `src/TwilightImperiumUltimate.Web/TwilightImperiumUltimate.Web.csproj`). Add `Usings.cs` and `Components/SmokeTests.cs` that renders `ResponsiveText` (`Components/Shared/Text/ResponsiveText.razor`) with `Text="Hello"` and asserts the markup contains `Hello`. Register in the test context any service the component needs. | `dotnet test tests/TwilightImperiumUltimate.ComponentTests/TwilightImperiumUltimate.ComponentTests.csproj` reports 1 passed, 0 failed. | ✅ | 2026-10-15 |
| TASK-0003 | Create `tests/TwilightImperiumUltimate.IntegrationTests/TwilightImperiumUltimate.IntegrationTests.csproj` (`net10.0`, same test packages plus `Microsoft.EntityFrameworkCore.SqlServer` 10.0.9, `ProjectReference` to `Core`, `Contracts`, `DataAccess`, `Business`). Add `Usings.cs` and an insert/read test that obtains a unique temporary LocalDB database from `TestDatabase`. | `dotnet test tests/TwilightImperiumUltimate.IntegrationTests/TwilightImperiumUltimate.IntegrationTests.csproj` reports the LocalDB insert/read test passed and cleans up its temporary database. | ✅ | 2026-10-15 |
| TASK-0004 | Add both projects to `TwilightImperiumUltimate.slnx` as `<Project Path="tests/..." />` entries next to the existing test project. | `dotnet build TwilightImperiumUltimate.slnx` succeeds. `dotnet test TwilightImperiumUltimate.slnx` is green and the number of tests in `TwilightImperiumUltimate.Tests` is unchanged. | ✅ | 2026-10-15 |

**Checkpoint:** three test projects run from the solution; the old tests are unchanged.

### Implementation Phase 2

- GOAL-0002: Temporary SQL Server LocalDB test database setup

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0005 | Add `tests/TwilightImperiumUltimate.IntegrationTests/Infrastructure/TestDatabase.cs`. It creates a uniquely named SQL Server LocalDB database, builds `TwilightImperiumDbContext` with `UseSqlServer`, runs `Database.EnsureCreated`, and exposes an `IDbContextFactory<TwilightImperiumDbContext>`. Dispose the helper by dropping its database with active connections forced closed. | The helper creates and disposes a temporary LocalDB database successfully. | ✅ | 2026-10-15 |
| TASK-0006 | Add an integration test that obtains `TestDatabase`, inserts and reads one simple existing entity (for example `Website`) and verifies the persisted value. Record the provider and database lifecycle in `plan/README.md` section 5a and assumption `ASM-01`. | `dotnet test tests/TwilightImperiumUltimate.IntegrationTests --filter FullyQualifiedName~MainContextTestDatabase` passes and the database is dropped after disposal. | ✅ | 2026-10-15 |

**Checkpoint:** you know which database the persistence and hub tests will use, and why.

### Implementation Phase 3

- GOAL-0003: Documentation and baseline

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-0007 | Update `docs/build-and-test.md`: section 11 lists the three test projects, section 12 documents the component tests (path, command, bUnit), section 13 documents the integration tests (the database provider decided in TASK-0006, its prerequisites, the command), section 14 says E2E tests arrive in phase P12. | All documented commands for the existing unit, new component, and new integration test projects were run successfully. | ✅ | 2026-10-15 |
| TASK-0008 | Run `dotnet build TwilightImperiumUltimate.slnx --configuration Release` and `dotnet format TwilightImperiumUltimate.slnx --verify-no-changes`. Record the aggregate warning count, format result, and any pre-existing diagnostics in `plan/README.md`, section 5 "Baseline"; do not fix unrelated baseline findings. | The Release build succeeds; format diagnostics are recorded and none are in P00-added test files. | ✅ | 2026-10-15 |

**Phase gate (tick before starting P01):**

- [x] `dotnet build TwilightImperiumUltimate.slnx` succeeds
- [x] `dotnet test TwilightImperiumUltimate.slnx --configuration Release` is green (81 existing tests passed, 3 skipped; 1 component test and 1 LocalDB integration test passed)
- [x] Temporary SQL Server LocalDB provider and lifecycle are recorded in `plan/README.md` section 5a and `ASM-01`
- [x] `docs/build-and-test.md` commands verified
- [x] Baseline numbers are in `plan/README.md`

## 3. Alternatives

- **ALT-0001**: Put the bUnit tests into `TwilightImperiumUltimate.Tests`. Rejected: that project would need a reference to the WASM Web project, and `docs/build-and-test.md` already recommends a separate component test project.
- **ALT-0002**: Testcontainers with SQL Server instead of LocalDB. Rejected: needs Docker on every machine.
- **ALT-0003**: A dedicated `DraftRoomsDbContext` for draft rooms (the option I recommended). You chose the main context (`DEC-28`), which is used for these SQL Server LocalDB tests.

## 4. Dependencies

- **DEP-0001**: NuGet `bunit` (MIT). Check on nuget.org that the version supports `net10.0` before pinning it.
- **DEP-0002**: NuGet `Microsoft.EntityFrameworkCore.SqlServer` 10.0.9 (MIT), test project only.
- **DEP-0003**: SQL Server LocalDB (`(localdb)\MSSQLLocalDB`) installed and available to the test process.

## 5. Files

- **FILE-0001**: `tests/TwilightImperiumUltimate.ComponentTests/*` (new)
- **FILE-0002**: `tests/TwilightImperiumUltimate.IntegrationTests/*` (new, including `Infrastructure/TestDatabase.cs` and persistence tests)
- **FILE-0003**: `TwilightImperiumUltimate.slnx` (two project entries)
- **FILE-0004**: `docs/build-and-test.md`
- **FILE-0005**: `plan/README.md` (baseline section 5 and decision section 5a), `docs/draft-rooms/specification.md` (`ASM-01`, `RSK-01`)

## 6. Testing

- **TEST-0001**: `SmokeTests.RendersResponsiveText` (bUnit).
- **TEST-0002**: `MainContextTestDatabaseTests.MainContextTestDatabase_WhenUsed_ShouldInsertAndReadWebsite` (temporary SQL Server LocalDB).

## 7. Risks & Assumptions

- **RISK-0001**: bUnit may lag behind .NET 10. Fallback: use the newest prerelease that supports `net10.0` and note it in the commit message.
- **RISK-0002**: Referencing the Blazor WebAssembly project from a test project can fail on missing services or static assets. The smoke test proves it works before any real test depends on it.
- **ASSUMPTION-0001**: No CI workflow exists in the repository (`.github/workflows` is empty), so nothing besides the solution file needs updating.
- **RISK-0003**: LocalDB may be unavailable or the test process may lack permission to create/drop databases. P00 verifies the LocalDB prerequisite; setup failures surface from `TestDatabase` rather than silently switching providers.

## 8. Related Specifications / Further Reading

- [Specification, section 14 Testing strategy](../docs/draft-rooms/specification.md)
- [Specification, section 15 Delivery approach](../docs/draft-rooms/specification.md)
