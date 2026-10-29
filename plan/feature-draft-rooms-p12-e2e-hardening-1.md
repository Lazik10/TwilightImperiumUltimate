---
goal: Draft Rooms P12 - End-to-end tests, hardening, documentation and release
version: 1.0
date_created: 2026-10-15
last_updated: 2026-10-17
owner: Lazik
status: 'Implemented - manual acceptance deferred'
tags: [feature, draft-rooms, e2e, playwright, security, accessibility, documentation, release]
---

# Introduction

![Status: Implemented - manual acceptance deferred](https://img.shields.io/badge/status-Implemented--manual_acceptance_deferred-yellow)

Closes the feature: Playwright end-to-end tests, a security review, an accessibility audit, a performance sanity check, documentation updates, removal of temporary code and a release checklist. Specification: section 14, `NFR-005`, `NFR-006`, `NFR-007`, scenario `SCN-90`.

Needs: P11 passed. Planned at task level and re-checked against the real code first (TASK-1201).

## 1. Requirements & Constraints

- **REQ-1201**: End-to-end tests cover the create, join, claim and chat flow and a complete Faction Pick draft with several isolated browser contexts.
- **REQ-1202**: The end-to-end draft test also proves hidden information in choice-bearing DOM state and response fields: another context cannot identify which faction is pending. Public faction pools legitimately contain all eligible faction names, so raw absence of a name from the entire DOM is not a valid privacy assertion.
- **REQ-1203**: Documentation matches the final code: architecture, project structure, solution overview, build and test, SEO.
- **SEC-1201**: The security review covers password and secret handling, CORS, hub authentication, the redaction choke point, throttles, XSS, enumeration, the login `returnUrl`, and public result links. Findings are fixed and each gets a regression test.
- **CON-1201**: Playwright needs browsers installed locally. Document the install command and the test command (`docs/build-and-test.md` section 14).
- **CON-1202**: The temporary diagnostics page from P05 is removed.
- **GUD-1201**: Keep the E2E suite small and stable: it tests journeys, the unit and integration tests test rules.

## 2. Implementation Steps

### Implementation Phase 1

- GOAL-1201: Playwright project and journeys

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-1201 | Re-validate this plan against specification section 14 and the P11 implementation. Correct privacy assertions to inspect selection-bearing state, not raw faction-name presence in the public pool. | Revalidation note below records actual P11 tests and deferred manual gates. | x | 2026-10-17 |
| TASK-1202 | Create `tests/TwilightImperiumUltimate.E2ETests/` (`Microsoft.Playwright.Xunit`, added to `TwilightImperiumUltimate.slnx`) with real Kestrel/API and published Release Web hosts, SPA fallback, test-only API URL/CORS configuration and a disposable LocalDB database. Document the Release build, Chromium installation and test commands in `docs/build-and-test.md` section 14. | Real-browser landing smoke test sees Create Draft and Join Draft. Included in the clean seven-case solution run. | x | 2026-10-17 |
| TASK-1203 | Add the room journey test with two isolated browser contexts: a guest creates a room, a second guest joins with code and password and claims a seat, a chat message sent by one appears in the other, a reload keeps the identity, and the room page has no site header or menu. | All three repetitions passed in one solution run, including mobile Enter activation and viewport bounds. No retries. | x | 2026-10-17 |
| TASK-1204 | Add the draft journey test with three contexts plus a clean context: simultaneous bans, another seat's pending choice is absent from choice-bearing response/DOM selection state (not from the public pool), reveal, snake picks, undo, finish, **Copy as text**, and a public result page opened by the clean context. | All three repetitions passed in one solution run, including actual clipboard contents and clean-context room denial/public-result access. No retries. | x | 2026-10-17 |

**Checkpoint:** the main journeys are guarded by automated browser tests.

### Implementation Phase 2

- GOAL-1202: Hardening

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-1205 | Run the `security-review` agent on the whole feature (API, hub, Web, tests). Fix every finding with severity medium or higher, add a regression test for each, and put the findings table into the commit message. | Whole-feature review found no vulnerabilities. No fixes/re-run required; reverse-proxy `access_token` log redaction remains an operator checklist item. | x | 2026-10-17 |
| TASK-1206 | Run the `Accessibility Reviewer` agent on the landing page, the room (lobby, bans, picks, results) and the public result page. Fix findings and add regressions. | Reported blocking issues addressed: actual faction names, labelled controls, mobile disclosure/desktop region semantics and terminal focus. Component/browser regressions pass. Full keyboard SCN-01 to SCN-73, screen-reader and device acceptance remain deferred; no second agent review is claimed. | x | 2026-10-17 |
| TASK-1207 | Add a tagged load test (`[Trait("Category","Load")]`) in the integration project: 20 rooms with 8 connected clients each, chat bursts and seat changes for one minute. Record memory, CPU and the time from commit to `RoomChanged` in the commit message and confirm the limits in `DraftRoomLimits` still hold. | Final standalone Release rerun passed: 20 rooms/160 clients, 61.22 s, 620 changes, all 4960 expected notifications. Recorded measurements below; operator acceptance is deferred. Default runs exclude load; an explicit filter replaces the default. | x | 2026-10-17 |

**Checkpoint:** security, accessibility and load have been checked by tools, not only by eye.

### Implementation Phase 3

- GOAL-1203: Documentation, cleanup, release

| Task | Description | Verify | Completed | Date |
|------|-------------|--------|-----------|------|
| TASK-1208 | Update architecture, project structure, solution overview, build/test, SEO and roadmap documentation to match the final implementation, isolated database requirements and manual release gates. | Changed local publish/build/browser-install/test commands exercised. Manual API startup and destructive deployment/retention commands are documented but intentionally not executed against ordinary/main or production databases. | x | 2026-10-17 |
| TASK-1209 | Remove `Pages/Tools/DraftRoomDiagnostics.razor(.cs)` and temporary feature code. Run the Release solution build, full tests using the documented LocalDB resource budget, and full-solution formatting verification. Use `prepare-production-pr` for final direct review. | Release build: zero warnings/errors. Complete solution: 972 passed, three existing simulation skips. Full-solution formatting passed. No feature TODO/diagnostics remains in the checked source. Manual/operator checklist items are explicitly deferred, not passed. | x | 2026-10-17 |
| TASK-1210 | Release checklist: take a database backup, deploy the API (it applies the draft room migrations at startup like all other migrations), confirm WebSockets are enabled on the host, confirm the CORS origins and the hub policy in the environment configuration, confirm `DraftRooms:*` settings, then smoke test SCN-01 to SCN-80 on the deployed environment with real devices (one phone). Watch the log for errors for one day. | Smoke test passes. The log shows no unexpected errors after one day. | | |

**Phase gate (feature done):**

- [x] SCN-90: seven Playwright cases pass; security review has no confirmed findings and reported blocking accessibility issues have automated regressions
- [ ] Full manual keyboard/screen-reader/device acceptance
- [ ] The load test numbers are recorded and accepted
- [x] Changed local documentation commands verified; manual server/deployment commands remain operator-owned
- [x] Diagnostics page removed, no leftover `TODO` from this feature in checked source
- [ ] Release smoke test passed on the deployed environment

### Automated hardening evidence (2026-10-17)

- Security agent: no confirmed vulnerabilities. Operational requirement: redact SignalR `access_token` query parameters in proxy/server logs.
- Release solution build: zero warnings and zero errors. The complete documented solution command (`dotnet test TwilightImperiumUltimate.slnx -c Release --no-build --no-restore -m:1`) passed in one run: 521 unit tests, 253 component tests, 191 integration tests and seven browser cases. The three skipped unit cases are existing rating simulations. Load is opt-in, not included in this count.
- Release Web publish and full-solution `dotnet format TwilightImperiumUltimate.slnx --no-restore --verify-no-changes` passed. Final direct review used the production-PR checklist; deployment and manual acceptance remain explicitly open below.
- Browser smoke plus three room and three full-draft repetitions passed together. A proven reload-capture race was fixed by accepting only room requests initiated after navigation commit, rather than reading an old document's discarded live-refresh response. No assertion retries or forced clicks are used.
- Unrestricted local runs hit SQL login/database-creation timeouts. The documented solution command runs projects sequentially; integration collections are bounded to two while deliberate concurrency inside each test is retained. An owned disposable probe confirmed LocalDB defaults `AUTO_CLOSE` to on; the fixture now disables it only for its own newly created database, with a passing regression. No ordinary/main database or server-wide/production connection setting was changed. This is a test-resource boundary, not a production performance guarantee.
- Accessibility regressions cover localized faction names, actual numeric input labels/IDs, bans mode selection, mobile disclosure controls and named desktop regions, terminal-state focus, and mobile/faction button bounds. Browser page-exception assertions pass; missing existing shared image/font resources and intentionally blocked external assets mean full visual/resource-console acceptance remains manual.
- Final standalone load rerun: one test executed/passed (TRX counters checked, not merely a zero exit code). Measured 61.22 seconds, 620 seat changes and 4960 client delivery samples across all 20 rooms/eight clients each. The initial measurements remain recorded in the TASK-1207 commit.
- Commit-to-`RoomChanged`: p50 4.21 ms, p95 60.73 ms, maximum 230.10 ms. A test-only EF interceptor timestamps actual SQL transaction commits; missing timestamps or final deliveries fail the test.
- Process private memory: baseline 87.8 MiB, sampled peak 131.2 MiB. CPU: 52.92 seconds, 86.4% of one core during the measured window. API/TestServer and long-polling clients share the process; SQL Server is excluded. This is not a deployed WebSocket capacity measurement.
- Setup uses the actual create business command instead of exhausting the HTTP ten-creations/IP/hour policy. Measured traffic uses real seat/chat authorization and production throttles. Persisted seat/participant bounds are asserted.
- These measured values are recorded, not automatically accepted on the user's behalf. Deployment, real-device/screen-reader checks and one-day monitoring remain manual gates.

## 3. Alternatives

- **ALT-1201**: Skip E2E and rely on bUnit and hub tests. Rejected: you asked for Playwright, and the draft journey spans three browsers, which only an E2E test exercises.
- **ALT-1202**: Run E2E against the real SQL Server. Rejected: the test database from P00 keeps it self-contained and repeatable.

## 4. Dependencies

- **DEP-1201**: NuGet `Microsoft.Playwright.Xunit` (Apache-2.0), downloaded browsers (Chromium at least).
- **DEP-1202**: P05 `TestApiFactory`. A published build of the Web project.

## 5. Files

- **FILE-1201**: `tests/TwilightImperiumUltimate.E2ETests/*` (new), `TwilightImperiumUltimate.slnx`
- **FILE-1202**: `tests/TwilightImperiumUltimate.IntegrationTests/Load/*`
- **FILE-1203**: `docs/architecture.md`, `docs/project-structure.md`, `docs/solution-overview.md`, `docs/build-and-test.md`, `docs/seo.md`
- **FILE-1204**: `Web/Pages/Tools/DraftRoomDiagnostics.razor(.cs)` (deleted)

## 6. Testing

- **TEST-1201**: Playwright room journey and draft journey.
- **TEST-1202**: Regression tests for every security finding.
- **TEST-1203**: Load test (tagged, not part of the default run).
- **TEST-1204**: Release smoke test on the deployed environment.

## 7. Risks & Assumptions

- **2026-10-17 revalidation**: P11 adds public snapshots, finished/public result views, account history, immutable-name account linking and UTC cleanup. Its automated baseline is 521 passing unit tests (three existing skips), 249 component tests and 190 integration tests. User-requested manual acceptance remains deferred rather than passed. E2E hosting must use real Kestrel/browser traffic and a uniquely named disposable LocalDB database, never the ordinary worktree connection. Public pools contain every eligible faction, so privacy assertions now target choice-bearing response and DOM selection state. The shared clipboard helper now awaits actual copying and exposes failure; the browser journey must check the actual clipboard contents, not only a success label. Production backup, deployment, real-device verification and one-day monitoring remain manual release tasks.
- **RISK-1201**: Serving a published Blazor WebAssembly app from a test fixture is fiddly (base path, fingerprinted assets). Keep the fixture small and documented. If it proves unstable, run Web through its dev server instead and note it in the build document.
- **RISK-1202**: E2E tests can be slow or flaky. Keep to two journeys, use auto-waiting locators, retry nothing silently.
- **ASSUMPTION-1201**: Chromium alone is enough for the automated suite. Firefox and Safari are checked manually in the release smoke test.

## 8. Related Specifications / Further Reading

- [Specification: 13, 14, 15, 16](../docs/draft-rooms/specification.md)
- [Scenario SCN-90](../docs/draft-rooms/specification.md)
