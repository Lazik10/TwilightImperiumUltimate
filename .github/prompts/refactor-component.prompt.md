---
description: Refactor a Blazor component while preserving behavior and following repository conventions.
---

# Refactor Blazor component

Refactor the requested Blazor component or page without changing behavior unless explicitly requested.

Follow:

```text
.github/instructions/blazor.instructions.md
.github/instructions/html.instructions.md
.github/instructions/css.instructions.md
.github/instructions/accessibility.instructions.md
.github/instructions/testing.instructions.md
```

---

## Task

Improve maintainability, readability, structure, and testability while preserving behavior.

---

## Refactoring workflow

1. Inspect the current component.
2. Identify existing behavior.
3. Identify current parameters, callbacks, services, and state.
4. Preserve public component API unless a change is requested.
5. Move logic to `.razor.cs` if needed.
6. Extract child components only when it improves clarity.
7. Move reusable logic into services or helpers.
8. Keep component-specific styling in `.razor.css`.
9. Preserve accessibility behavior.
10. Update or add tests for behavior that could regress.

---

## Refactoring rules

- Keep `.razor` focused on markup.
- Keep `.razor.cs` focused on logic.
- Do not introduce unnecessary abstractions.
- Do not change behavior silently.
- Do not rename public parameters without explaining impact.
- Use project-owned wrappers for repeated/styled UI.
- Wrap repeated Radzen usage where appropriate.
- Preserve loading, empty, error, validation, and success states.
- Preserve keyboard and focus behavior.
- Avoid raw `HttpClient` in components.

---

## Output summary

Return:

```text
Summary:
- [what was refactored]
- [behavior preserved]
- [files changed]

Validation:
- [commands run or recommended]

Notes:
- [assumptions]
- [follow-up work]
```
