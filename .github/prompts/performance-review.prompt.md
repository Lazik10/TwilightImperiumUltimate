---
description: Review C# / Blazor WebAssembly code for rendering, data loading, and UI performance.
---

# Performance review

Review the requested component, page, service, or diff for performance issues.

---

## Check

Review:

- Unnecessary re-rendering
- Expensive expressions in Razor markup
- Large lists without virtualization or paging
- Missing `@key` in identity-sensitive lists
- Repeated allocations in markup
- Excessive child components in large loops
- Chatty API calls
- Over-fetching data
- Blocking async calls
- Unnecessary JavaScript interop
- Large CSS or broad selectors
- Images/assets that may cause layout shift
- State changes that trigger broad re-renders

---

## Blazor performance rules

- Use `@key` when list identity matters.
- Avoid complex LINQ expressions directly in markup.
- Use `Virtualize<TItem>` for large lists when appropriate.
- Prefer paging/filtering for large data sets.
- Avoid unnecessary `StateHasChanged`.
- Avoid `async void`.
- Avoid `.Result` and `.Wait()`.
- Keep component parameters stable and intentional.
- Avoid unnecessary cascading values.

---

## Output format

Return:

```text
Performance issues:
- [issue or "None"]

Recommended changes:
- [change or "None"]

Tests / validation:
- [manual or automated validation]

Overall:
- [summary]
```
