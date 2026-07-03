---
description: Review and improve accessibility for Blazor, Razor, HTML, CSS, and Radzen-based UI.
---

# Improve accessibility

Review the requested UI, component, page, or diff for accessibility issues and improve it where appropriate.

Follow:

```text
.github/instructions/accessibility.instructions.md
.github/instructions/html.instructions.md
.github/instructions/css.instructions.md
.github/instructions/blazor.instructions.md
```

---

## Check

Review:

- Semantic HTML
- Headings and landmarks
- Buttons vs links
- Labels and form validation
- Keyboard navigation
- Focus visibility
- Dialog and overlay behavior
- Accessible names
- Icon-only controls
- Color contrast
- Color-only state indicators
- Loading, empty, error, and success states
- ARIA correctness
- Radzen usage and wrapper opportunities
- Responsive and zoom behavior

---

## Rules

- Prefer semantic HTML before ARIA.
- Prefer native elements where possible.
- Prefer project-owned wrapper components for repeated accessibility-sensitive UI.
- Do not overuse ARIA.
- Do not introduce inaccessible custom controls.
- Keep changes focused and practical.

---

## Output summary

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
