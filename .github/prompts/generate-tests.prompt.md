---
description: Generate or update tests for C#, Blazor components, services, pages, and wrapper components.
---

# Generate tests

Generate tests for the selected or requested code.

Follow `.github/instructions/testing.instructions.md` for the test stack (xUnit, bUnit, FluentAssertions, NSubstitute/Moq, Bogus, Playwright), naming convention (`MethodName_WhenCondition_ShouldExpectedResult`), Arrange/Act/Assert style, and what to test vs. not test — do not restate those rules here.

---

## Before writing tests

1. Inspect the code under test.
2. Identify observable behavior.
3. Identify dependencies.
4. Check existing test style and test project structure.
5. Prefer existing testing libraries already used in the repository.
6. Decide whether unit, component, integration, or E2E tests are appropriate.
7. Avoid testing private implementation details.

---

## Output summary

Return:

```text
Summary:
- [tests added/updated]
- [behaviors covered]

Validation:
- [test command run or recommended]

Notes:
- [coverage gaps]
- [assumptions]
```
