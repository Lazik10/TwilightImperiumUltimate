---
description: Find maintainability issues and technical debt in C# / Blazor WebAssembly code.
---

# Find technical debt

Review the requested area for technical debt and maintainability risks.

---

## Check

Look for:

- Large components
- Too much logic in markup
- Missing `.razor.cs`
- Repeated Radzen usage that should be wrapped
- Duplicated CSS
- Broad global CSS
- Raw `HttpClient` calls in components
- Missing typed services
- Missing loading/error states
- Missing tests
- Brittle tests
- Inconsistent naming
- Overly complex abstractions
- Missing documentation
- Security or accessibility shortcuts

---

## Output format

Return:

```text
High-priority debt:
- 

Medium-priority debt:
- 

Low-priority cleanup:
- 

Suggested first fixes:
1. 
2. 
3. 
```

Focus on actionable items. Do not list style nitpicks unless they affect maintainability.
