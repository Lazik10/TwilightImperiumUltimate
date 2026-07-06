---
name: run-quality-checks
description: Use this skill to quickly run and interpret the repository's build, test, and format validation commands. For a full pre-PR review (diff, tests, docs, accessibility, security), use prepare-production-pr instead.
---

# Run quality checks

Run the repository's standard validation commands and report the results plainly.

## Commands

Use `docs/build-and-test.md` as the source of truth. Typical commands:

```bash
dotnet restore TwilightImperiumUltimate.sln
dotnet build TwilightImperiumUltimate.sln
dotnet test tests/TwilightImperiumUltimate.Tests/TwilightImperiumUltimate.Tests.csproj
dotnet format --verify-no-changes
```

## Workflow

1. Run restore, then build, then test, in order. Stop and report the exact error at the first failing step.
2. Run `dotnet format --verify-no-changes` last and report any files that need formatting.
3. Do not claim a command was run unless it was actually executed.

## Output summary

Return:

```text
Build:
- [pass/fail + summary]

Tests:
- [pass/fail + failing test names, if any]

Formatting:
- [pass/fail + files needing formatting, if any]

Notes:
- [anything blocking validation from running]
```
