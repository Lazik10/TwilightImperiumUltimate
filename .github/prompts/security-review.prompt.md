---
description: Review C# / Blazor WebAssembly changes for security issues.
---

# Security review

Review the requested code, diff, feature, component, service, or page for security risks using the security checklist in `.github/skills/prepare-production-pr/SKILL.md` (`## Security review`) and the Blazor WebAssembly security rules in `.github/copilot-instructions.md`. Do not restate that checklist here — apply it directly.

---

## Output format

Return:

```text
Blocking security issues:
- [issue or "None"]

Security recommendations:
- [recommendation or "None"]

Tests / validation:
- [security tests or manual checks]

Overall:
- [Approve / Needs changes]
```

