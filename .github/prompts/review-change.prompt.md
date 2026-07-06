---
description: Review the current change or selected code for correctness, maintainability, Blazor quality, accessibility, testing, security, and performance.
---

# Review change

Review the current change, selected code, or git diff using the `Reviewer` agent's review focus and checklist (`.github/agents/reviewer.agent.md`) and the baseline in `.github/instructions/code-review.instructions.md`. Do not restate that checklist here — apply it directly.

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

