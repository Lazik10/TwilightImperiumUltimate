---
description: Prepare a pull request summary with validation, risks, tests, documentation impact, and follow-up work.
---

# Prepare pull request

Prepare a pull request summary for the current changes.

---

## Workflow

1. Review the current diff.
2. Identify user-facing changes.
3. Identify technical changes.
4. Identify tests added or changed.
5. Identify validation performed or still needed.
6. Identify documentation impact.
7. Identify risks and follow-up work.

---

## Output format

Return:

```markdown
## Summary

- 
- 

## Validation

- [ ] `dotnet build`
- [ ] `dotnet test`
- [ ] `dotnet format --verify-no-changes`
- [ ] Manual UI check, if applicable

## Tests

- 

## Documentation

- 

## Risks

- 

## Follow-up

- 
```

If a section has nothing meaningful, write `None`.

Do not invent validation results. If a command was not run, say it was not run.
