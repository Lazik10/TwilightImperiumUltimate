---
description: Review the current change or selected code for correctness, maintainability, Blazor quality, accessibility, testing, security, and performance.
---

# Review change

Review the current change, selected code, or git diff.

Follow all repository instructions and relevant docs.

---

## Review areas

Check:

- Correctness
- C# quality
- Blazor component structure
- `.razor` + `.razor.cs` pattern
- Parameter usage
- EventCallback usage
- Service usage
- State management
- Async correctness
- Loading, empty, error, and success states
- Accessibility
- Semantic HTML
- CSS isolation
- Radzen wrapper usage
- Test coverage
- Security
- Performance
- Documentation impact

---

## Review rules

- Focus on meaningful issues.
- Separate blocking issues from suggestions.
- Avoid nitpicks unless they affect maintainability or consistency.
- Prefer existing repository conventions.
- Do not recommend broad rewrites unless necessary.
- Identify missing tests for behavior changes.
- Identify missing docs updates when setup, architecture, or commands changed.

---

## Output format

Return:

```text
Blocking issues:
- [issue or "None"]

Non-blocking suggestions:
- [suggestion or "None"]

Testing recommendations:
- [tests to add/update or "None"]

Accessibility notes:
- [notes or "None"]

Security/performance notes:
- [notes or "None"]

Overall:
- [Approve / Approve with suggestions / Needs changes]
```
