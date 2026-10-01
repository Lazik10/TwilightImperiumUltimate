---
description: Debug and fix .NET / C# / Blazor build errors with minimal, focused changes.
---

# Debug build error

Debug the provided build error, compiler error, test failure, or runtime error.

---

## Workflow

1. Read the exact error message.
2. Identify the project, file, line, and symbol involved.
3. Search related code before changing anything.
4. Explain the likely root cause.
5. Apply the smallest safe fix.
6. Avoid unrelated cleanup.
7. Recommend the validation command.

---

## Rules

- Do not guess blindly.
- Do not rewrite large areas to fix a small error.
- Preserve existing architecture and naming.
- Fix root cause, not just symptoms.
- Update tests if the fix changes behavior.
- Check namespace, using statements, nullable reference issues, generic types, parameters, and dependency injection registration.
- For Blazor errors, check component parameters, event callbacks, namespaces, partial class names, and `.razor` / `.razor.cs` matching.

---

## Common Blazor checks

Verify:

```text
ComponentName.razor matches ComponentName.razor.cs partial class.
Namespace matches project convention.
[Parameter] names match component usage.
EventCallback<T> types match usage.
Required services are registered.
Razor markup uses valid C# expressions.
Generic component type parameters are set correctly.
```

---

## Output summary

Return:

```text
Root cause:
- [explanation]

Fix:
- [files changed]
- [what changed]

Validation:
- [command run or recommended]
```
