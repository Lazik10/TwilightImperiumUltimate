---
description: Review and improve accessibility for Blazor, Razor, HTML, CSS, and Radzen-based UI.
---

# Improve accessibility

Review the requested UI, component, page, or diff for accessibility issues and improve it, using the `Accessibility Reviewer` agent's checklist (`.github/agents/accessibility-reviewer.agent.md`) and `.github/skills/accessibility-audit/SKILL.md`. Do not restate that checklist here — apply it directly.

---

## Output format

Return:

```text
Issues found:
- [issue]

Changes made:
- [change]

Manual checks recommended:
- Keyboard-only navigation
- Focus visibility
- Lighthouse / axe DevTools / Accessibility Insights
- Screen reader smoke test, if practical
```

