---
name: accessibility-audit
description: Use this skill when reviewing or improving accessibility for Blazor WebAssembly UI, Razor markup, HTML, CSS, forms, dialogs, Radzen components, wrapper components, keyboard navigation, focus behavior, and WCAG 2.2 AA alignment.
---

# Accessibility audit

Use this skill when reviewing or improving accessibility for UI in this C# / Blazor WebAssembly repository.

Accessibility should be treated as a normal quality requirement. Use **WCAG 2.2 AA** as the baseline target.

---

## Repository context

Follow `.github/copilot-instructions.md` and `.github/instructions/accessibility.instructions.md` (plus the `html`, `css`, `blazor`, and `testing` instructions — they auto-apply by file type). Reference `docs/*.md` for architecture and build/test commands.

---

## Audit workflow

### 1. Identify the UI scope

Determine what is being reviewed:

```text
Single component
Page
Form
Dialog/modal
Data grid/table
Navigation
Reusable wrapper component
Radzen-based UI
Current git diff
Whole feature
```

---

### 2. Inspect existing patterns

Before changing UI:

1. Search for project-owned wrapper components.
2. Search for existing accessible patterns.
3. Search for similar forms/dialogs/tables.
4. Check Radzen wrapper usage.
5. Check component-scoped CSS.
6. Check existing tests.

Prefer existing project patterns if they are accessible.

---

## Core accessibility checks

Check:

```text
[ ] Semantic HTML is used.
[ ] Buttons are used for actions.
[ ] Links are used for navigation.
[ ] Clickable div/span patterns are avoided.
[ ] Headings are logical.
[ ] Main content and landmarks are clear.
[ ] Form controls have labels.
[ ] Validation messages are clear and close to fields.
[ ] Keyboard navigation works.
[ ] Focus is visible.
[ ] Focus order is logical.
[ ] Dialogs manage focus correctly.
[ ] Icon-only controls have accessible names.
[ ] Images have appropriate alt text.
[ ] Color is not the only state indicator.
[ ] Contrast is preserved.
[ ] Loading, empty, error, and success states are accessible.
[ ] ARIA is minimal, correct, and synchronized with state.
[ ] UI works at narrow widths and browser zoom.
[ ] Motion respects reduced-motion preferences.
```

---

## Semantic HTML first

Prefer native semantic elements.

Use:

```html
<button>
<a>
<form>
<label>
fieldset
legend
main
nav
section
article
table
ul
ol
```

Avoid:

```html
<div @onclick="...">
<span @onclick="...">
<a href="#">
```

Do not replace native HTML with ARIA unless there is a clear need.

Bad:

```html
<div role="button" tabindex="0">Save</div>
```

Good:

```html
<button type="button">Save</button>
```

---

## Blazor and wrapper rules

Prefer project-owned accessible wrapper components for repeated UI — see the full list in `.github/instructions/blazor.instructions.md` (`## Project-owned component wrappers`). If a repeated UI pattern lacks a wrapper, consider creating one.

For Radzen: direct usage is fine for simple one-off UI; wrap repeated, styled, behavior-rich, or accessibility-sensitive usage. Do not test Radzen internals.

---

## Keyboard review

Check:

- `Tab` order is logical.
- `Shift + Tab` works.
- `Enter` activates links/buttons where expected.
- `Space` activates buttons and checkboxes.
- `Esc` closes dialogs/menus/popovers where expected.
- Focus does not disappear after actions.
- Focus is moved intentionally after opening/closing dialogs.
- No keyboard traps exist.
- Hover-only interactions have keyboard/touch alternatives.

Avoid positive `tabindex`.

---

## Focus review

Check CSS:

- No global `outline: none`.
- Focus states are visible.
- Focus indicators are not clipped.
- Focus contrast is sufficient.
- `:focus-visible` is used appropriately.
- Disabled controls are understandable.

Good:

```css
.app-button:focus-visible {
    outline: 2px solid currentColor;
    outline-offset: 2px;
}
```

---

## Forms review

Check:

- Every field has a visible label or justified accessible name.
- Labels are associated with inputs.
- Required fields are clear.
- Validation messages are close to fields.
- Validation messages are understandable.
- Validation messages are available to assistive technology where appropriate.
- Placeholder text is not the only label.
- Fieldsets and legends are used for grouped choices.
- Submit behavior is keyboard accessible.
- Duplicate submissions are prevented where needed.

---

## Dialog and overlay review

For dialogs/modals/popovers:

- Dialog has an accessible title.
- Focus moves into dialog on open.
- Focus returns to trigger on close.
- `Esc` closes where expected.
- Background content is not reachable for modal dialogs.
- Close action is clear.
- Dialog works with keyboard only.
- Avoid nested dialogs unless necessary.

Prefer project-owned dialog wrappers.

---

## Data grid and table review

For tables/grids:

- Tables are used for tabular data, not layout.
- Headers are clear.
- Captions or labels are used where helpful.
- Sorting/filtering/pagination controls are accessible.
- Row action buttons have accessible names.
- Empty/loading/error states are shown.
- Large tables remain usable on small screens.

For Radzen grids, prefer `AppDataGrid` or a project-owned wrapper when repeated.

---

## Color and contrast review

Check:

- Text contrast.
- Button contrast.
- Link contrast.
- Focus indicator contrast.
- Error/success/warning states.
- Disabled states.
- Placeholder text.
- Charts/icons/status badges.

Do not rely only on color.

Bad:

```html
<span class="red">Invalid</span>
```

Better:

```html
<span class="validation-message">Error: Email is required.</span>
```

---

## ARIA review

Use ARIA only when needed.

Check:

- ARIA references point to existing IDs.
- IDs are unique.
- `aria-expanded` matches actual state.
- `aria-controls` points to controlled content when used.
- `aria-describedby` points to hints/errors.
- `aria-labelledby` points to visible label text.
- `aria-hidden` is not used on focusable elements or focusable parents.
- Live regions are not overused.

Do not use ARIA to fake native controls when native controls work.

---

## CSS review

Check:

- Component-specific styles are in `.razor.css`.
- Global CSS is not overbroad.
- Focus states are not removed.
- Layout works with text wrapping and zoom.
- Fixed heights do not clip content.
- Motion respects `prefers-reduced-motion`.
- Color contrast is preserved.
- Visual order does not conflict with DOM order.
- Hidden content is intentionally hidden.

---

## Testing recommendations

For non-trivial UI changes, recommend:

```text
Keyboard-only test
Focus visibility check
Lighthouse
axe DevTools
Accessibility Insights
Browser accessibility tree inspection
Screen reader smoke test when practical
```

For automated tests, consider bUnit checks for:

- Accessible labels.
- Error messages.
- Icon button labels.
- Important ARIA attributes.
- Loading/empty/error states.
- Wrapper component behavior.

---

## Fixing strategy

Prefer fixes in this order:

1. Use semantic HTML.
2. Use existing project-owned components.
3. Improve wrapper components so all usages benefit.
4. Add missing labels/names/states.
5. Fix keyboard and focus behavior.
6. Fix CSS contrast/focus/responsive issues.
7. Add ARIA only when needed.
8. Add or update tests for important behavior.

Avoid one-off accessibility patches scattered across pages if a wrapper would solve the issue centrally.

---

## Final response format

Return:

```text
Accessibility issues:
- [issue]

Changes made:
- [change]

Testing recommended:
- [manual/tool checks]

Notes:
- [assumptions]
- [remaining risks]
```

For review-only tasks:

```text
Blocking issues:
- [issue or "None"]

Recommendations:
- [recommendation or "None"]

Manual checks:
- [checks]

Overall:
- [Approve / Needs changes]
```

---

## Checklist

```text
[ ] Semantic HTML checked.
[ ] Buttons/links are used correctly.
[ ] Forms have labels and validation messages.
[ ] Keyboard access checked.
[ ] Focus visibility checked.
[ ] Dialog behavior checked if applicable.
[ ] Icon-only controls have accessible names.
[ ] Color is not the only indicator.
[ ] Contrast considered.
[ ] Loading/empty/error states are accessible.
[ ] ARIA is minimal and correct.
[ ] Radzen usage is wrapped where appropriate.
[ ] Component CSS does not break accessibility.
[ ] Manual/tool testing is recommended.
```
