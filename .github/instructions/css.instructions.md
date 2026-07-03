---
applyTo: "**/*.css"
---

# CSS instructions

These instructions apply to all CSS files in this repository, including Blazor CSS isolation files such as `ComponentName.razor.css`.

This repository uses **C# / Blazor WebAssembly** and may use **Radzen components**. Prefer maintainable, accessible, component-scoped CSS with design tokens and minimal global overrides.

Detailed accessibility requirements are covered in `.github/instructions/accessibility.instructions.md`. This file focuses on CSS structure, naming, component styling, responsive design, tokens, theming, and Radzen styling strategy.

---

## Core principles

- Prefer component-scoped `.razor.css` files for component-specific styles.
- Use global CSS only for true global concerns.
- Prefer CSS variables/design tokens for colors, spacing, typography, borders, shadows, and radii.
- Follow the existing project CSS style first.
- If no clear convention exists, use simple semantic class names.
- Avoid inline styles.
- Avoid `!important` unless there is a clear, documented reason.
- Keep CSS readable and intentionally scoped.
- Do not create broad selectors that accidentally affect unrelated components.
- Preserve accessibility: focus states, contrast, zoom behavior, responsive layout, and reduced motion.

---

## Blazor CSS isolation

Prefer Blazor CSS isolation for component-specific styles.

Recommended structure:

```text
Components/
  AppButton.razor
  AppButton.razor.cs
  AppButton.razor.css
```

Use `.razor.css` for:

- Component layout.
- Component visual states.
- Component-specific spacing.
- Component-specific responsive behavior.
- Component-specific focus, hover, active, selected, disabled, loading, and error states.

Use global CSS for:

- Design tokens.
- CSS reset or normalization.
- Base typography.
- Global layout shell.
- App-wide utility classes, if the project uses them.
- Shared accessibility helpers such as `.visually-hidden`.
- Theme definitions.
- Vendor or Radzen integration boundaries, when unavoidable.

Avoid putting component-specific styles into global CSS.

---

## Global CSS organization

Recommended global CSS responsibilities:

```text
wwwroot/
  css/
    app.css
    responsive-components.css
    tokens.css
    typography.css
    layout.css
    utilities.css
    radzen-overrides.css
```

Only create these files if the project needs them. Do not introduce unnecessary CSS files.

Suggested purposes:

| File | Purpose |
|---|---|
| `tokens.css` | CSS variables/design tokens |
| `typography.css` | Base font, heading, paragraph styles |
| `layout.css` | App shell and global layout primitives |
| `utilities.css` | Small reusable utility classes |
| `radzen-overrides.css` | Carefully isolated Radzen overrides |
| `app.css` | Main import or app-level styles |
| `responsive-components.css` | Optional: responsive adjustments for components |

Follow the repository’s existing structure if it already has one.

---

## Design tokens and CSS variables

Prefer design tokens through CSS variables for reusable values.

Use tokens for:

- Colors.
- Spacing.
- Font sizes.
- Font weights.
- Line heights.
- Border radii.
- Borders.
- Shadows.
- Z-index values.
- Transition durations.
- Layout widths.

Example:

```css
:root {
    --color-surface: #ffffff;
    --color-surface-muted: #f8fafc;
    --color-text: #111827;
    --color-text-muted: #6b7280;
    --color-border: #d1d5db;
    --color-primary: #2563eb;
    --color-danger: #dc2626;

    --spacing-xs: 0.25rem;
    --spacing-sm: 0.5rem;
    --spacing-md: 1rem;
    --spacing-lg: 1.5rem;

    --radius-sm: 0.25rem;
    --radius-md: 0.5rem;
    --radius-lg: 0.75rem;

    --focus-ring-width: 2px;
    --focus-ring-offset: 2px;
}
```

Prefer:

```css
.customer-card {
    padding: var(--spacing-md);
    border: 1px solid var(--color-border);
    border-radius: var(--radius-md);
    background: var(--color-surface);
}
```

Avoid repeated hard-coded values:

```css
.customer-card {
    padding: 16px;
    border: 1px solid #d1d5db;
    border-radius: 8px;
}
```

Hard-coded values are acceptable for one-off calculations or values that are not part of the design system.

---

## Theme readiness

Do not force dark mode unless the project already supports it, but write CSS so theming is possible later.

Rules:

- Prefer semantic tokens such as `--color-surface`, `--color-text`, and `--color-border`.
- Avoid naming tokens only by color value such as `--blue-500` unless the project uses a color scale.
- Avoid hard-coding colors inside many components.
- Keep theme-specific values centralized.
- Ensure components use tokens instead of direct color values where practical.

Example:

```css
:root {
    --color-surface: #ffffff;
    --color-text: #111827;
}

[data-theme="dark"] {
    --color-surface: #111827;
    --color-text: #f9fafb;
}
```

Only add theme selectors if the project has or is adding theme support.

---

## Naming conventions

Follow existing project conventions first.

If no convention exists, use simple semantic class names:

```css
.customer-card { }
.customer-card-header { }
.customer-card-title { }
.form-field { }
.validation-message { }
```

Prefer names that describe purpose:

```css
.invoice-summary
.primary-navigation
.form-actions
.empty-state
```

Avoid names that describe only appearance:

```css
.blue-box
.big-red-text
.left-div
```

Avoid overcomplicated naming

BEM is acceptable if the project already uses BEM:

```css
.customer-card__title--highlighted { }
```

Do not mix multiple naming conventions in the same area.

---

## Selector scope

Keep selectors as narrow as practical.

Prefer:

```css
.customer-card {
    padding: var(--spacing-md);
}
```

Avoid overly broad selectors:

```css
div {
    padding: 1rem;
}

button {
    margin-left: 1rem;
}
```

Avoid deeply nested selectors:

```css
.page .panel .content .item .title span {
    color: var(--color-text);
}
```

Prefer clear component classes instead.

---

## Radzen styling strategy

This project may use Radzen components.

Preferred strategy:

1. Use project-owned wrapper components for repeated or styled Radzen usage.
2. Style the project-owned wrapper where possible.
3. Use Radzen parameters, templates, and supported styling APIs before CSS overrides.
4. Avoid deep overrides of Radzen internal classes unless necessary.
5. Keep unavoidable Radzen overrides isolated and documented.

Recommended wrapper example:

```text
Components/
  AppButton.razor
  AppButton.razor.cs
  AppButton.razor.css
```

Prefer styling:

```css
.app-button {
    min-height: 2.5rem;
}
```

Avoid scattered overrides such as:

```css
.rz-button {
    ...
}
```

If a Radzen override is unavoidable:

- Put it in a clearly named file or scoped context.
- Add a comment explaining why it is needed.
- Keep the selector as narrow as possible.
- Avoid `!important` unless there is no reasonable alternative.
- Verify the override does not affect unrelated Radzen components.

Example:

```css
/* Required because Radzen does not expose this spacing through component parameters. */
.app-data-grid :global(.rz-datatable-header) {
    padding-block: var(--spacing-sm);
}
```

Use `:global` only when required by Blazor CSS isolation and supported by the current project setup.

---

## Inline styles

Avoid inline styles in `.razor`, `.html`, and generated markup.

Avoid:

```razor
<div style="margin-top: 16px; color: red;">
```

Prefer:

```razor
<div class="validation-message">
```

```css
.validation-message {
    margin-top: var(--spacing-sm);
    color: var(--color-danger);
}
```

Inline styles are acceptable only when:

- The value is truly dynamic.
- A CSS class or CSS variable would be less clear.
- The reason is obvious or documented.

For dynamic values, prefer CSS variables:

```razor
<div class="progress-bar" style="--progress-value: @ProgressPercent%;">
```

```css
.progress-bar {
    width: var(--progress-value);
}
```

---

## Avoid `!important`

Do not use `!important` by default.

Avoid:

```css
.button {
    color: white !important;
}
```

Before using `!important`, try:

- Better selector scope.
- Component-scoped CSS.
- Project-owned wrapper component.
- Design token adjustment.
- Vendor-supported styling parameter.
- Moving the rule to the correct stylesheet.
- Removing the conflicting rule.

If `!important` is unavoidable:

- Add a comment explaining why.
- Keep the selector narrow.
- Do not use it as a general fix for cascade problems.

Example:

```css
/* Required to override third-party inline style produced by [component name]. */
.app-dialog-title {
    margin-bottom: var(--spacing-md) !important;
}
```

---

## Layout

Prefer modern, flexible layout techniques.

Use:

- Flexbox for one-dimensional layout.
- CSS Grid for two-dimensional layout.
- Logical properties where useful.
- Responsive units and spacing tokens.
- Container or media queries when appropriate.

Prefer:

```css
.form-actions {
    display: flex;
    gap: var(--spacing-sm);
    justify-content: flex-end;
    flex-wrap: wrap;
}
```

Avoid layout based on magic numbers:

```css
.form-actions {
    margin-left: 437px;
}
```

Avoid using tables for layout.

---

## Responsive design

CSS should support common viewport sizes and browser zoom.

Rules:

- Use responsive layouts by default.
- Avoid fixed widths where content should adapt.
- Avoid fixed heights that clip text.
- Allow text to wrap.
- Use `max-width` for readable content areas.
- Test components at narrow widths.
- Do not rely only on hover interactions.
- Keep touch targets comfortable.
- Ensure sticky elements do not cover focused content.
- Ensure dialogs and panels fit small screens.

Good:

```css
.content-panel {
    width: min(100%, 64rem);
    margin-inline: auto;
    padding: var(--spacing-md);
}
```

Use media queries when needed:

```css
@media (max-width: 48rem) {
    .form-actions {
        flex-direction: column;
        align-items: stretch;
    }
}
```

---

## Typography

Use consistent typography.

Rules:

- Prefer relative units such as `rem`.
- Keep line height readable.
- Avoid very small text.
- Do not use heading elements only for visual size.
- Do not use CSS to make non-heading text behave like document headings without a semantic reason.
- Keep text spacing friendly to readability.

Example:

```css
.page-title {
    font-size: var(--font-size-xl);
    line-height: 1.2;
    font-weight: 600;
}
```

If typography tokens do not exist, consider adding them centrally before repeating hard-coded font values.

---

## Spacing

Use spacing tokens for consistency.

Prefer:

```css
.card {
    padding: var(--spacing-md);
    margin-block-end: var(--spacing-lg);
}
```

Avoid arbitrary repeated spacing:

```css
.card {
    padding: 17px;
    margin-bottom: 23px;
}
```

Use logical properties where they improve readability and localization readiness:

```css
margin-inline-start
margin-inline-end
padding-block
padding-inline
```

---

## Focus states

Every interactive component must have a visible focus state.

Rules:

- Do not remove focus outlines globally.
- Use `:focus-visible` where appropriate.
- Ensure focus indicators have sufficient contrast.
- Ensure focus indicators are not clipped by `overflow: hidden`.
- Keep focus states consistent across project-owned components.
- Include focus styling for custom controls.

Avoid:

```css
*:focus {
    outline: none;
}
```

Prefer:

```css
.app-button:focus-visible {
    outline: var(--focus-ring-width) solid currentColor;
    outline-offset: var(--focus-ring-offset);
}
```

---

## Interactive states

Style important component states intentionally.

Consider:

- Default
- Hover
- Focus
- Focus-visible
- Active
- Selected
- Disabled
- Loading
- Error
- Success
- Empty

Rules:

- Disabled styles should still be readable.
- Error styles must not rely on color alone.
- Loading states should not cause layout shift if avoidable.
- Hover styles should not be the only way to reveal critical content.
- Touch users should not lose access to hover-only content.

---

## Accessibility-safe CSS

CSS must not make the UI inaccessible.

Avoid:

```css
.visually-hidden-but-focusable {
    display: none;
}
```

Avoid hiding important content with:

```css
display: none;
visibility: hidden;
opacity: 0;
```

unless the content should truly be unavailable.

Use a visually hidden utility for screen-reader-only text when appropriate:

```css
.visually-hidden {
    position: absolute;
    width: 1px;
    height: 1px;
    padding: 0;
    margin: -1px;
    overflow: hidden;
    white-space: nowrap;
    border: 0;
    clip-path: inset(50%);
}
```

Do not apply visually hidden styles to focusable interactive elements unless they become visible on focus.

---

## Motion and animation

Use motion carefully.

Rules:

- Avoid unnecessary animation.
- Keep transitions short.
- Do not animate essential content in a distracting way.
- Do not use flashing content.
- Respect `prefers-reduced-motion`.
- Avoid motion that prevents users from completing tasks.

Example:

```css
@media (prefers-reduced-motion: reduce) {
    *,
    *::before,
    *::after {
        animation-duration: 0.01ms;
        animation-iteration-count: 1;
        scroll-behavior: auto;
        transition-duration: 0.01ms;
    }
}
```

Adapt this to the project’s CSS strategy before applying globally.

---

## Z-index

Avoid arbitrary z-index escalation.

Prefer defining tokens or a scale:

```css
:root {
    --z-index-dropdown: 1000;
    --z-index-sticky: 1020;
    --z-index-overlay: 1040;
    --z-index-dialog: 1050;
    --z-index-toast: 1060;
}
```

Avoid:

```css
.modal {
    z-index: 999999;
}
```

Use higher z-index only when the layering model requires it.

---

## CSS comments

Use comments to explain non-obvious decisions.

Good comments explain:

- Vendor overrides.
- Browser workarounds.
- Accessibility-related choices.
- Theme or token decisions.
- Temporary compatibility constraints.

Avoid comments that simply repeat the selector.

Good:

```css
/* Required because Radzen grid header padding is not exposed through parameters. */
.app-grid :global(.rz-datatable-header) {
    padding-block: var(--spacing-sm);
}
```

---

## File size and maintainability

Keep CSS maintainable.

Rules:

- Split large global CSS files when responsibilities become unclear.
- Keep component CSS close to the component.
- Remove unused CSS when removing components.
- Avoid duplicate rules.
- Avoid copy-pasting large style blocks.
- Prefer reusable components or tokens over repeated CSS.
- Do not introduce a CSS framework or utility system without explaining why.

---

## Common anti-patterns

Avoid:

```css
div {
    margin: 10px;
}

button {
    outline: none;
}

.error {
    color: red;
}

.rz-button {
    width: 100% !important;
}

.blue-card {
    background: blue;
}

.fixed-panel {
    height: 300px;
    overflow: hidden;
}
```

Prefer:

```css
.customer-card {
    padding: var(--spacing-md);
    background: var(--color-surface);
}

.app-button:focus-visible {
    outline: var(--focus-ring-width) solid currentColor;
    outline-offset: var(--focus-ring-offset);
}

.validation-message {
    color: var(--color-danger);
}

.validation-message::before {
    content: "Error: ";
}
```

---

## Copilot behavior

When generating or editing CSS:

1. Prefer component-scoped `.razor.css` for component-specific styles.
2. Use global CSS only for app-wide concerns.
3. Follow existing CSS conventions first.
4. If no convention exists, use simple semantic class names.
5. Prefer design tokens and CSS variables.
6. Keep CSS theme-ready without forcing dark mode.
7. Avoid inline styles.
8. Avoid `!important` unless justified.
9. Prefer project-owned wrappers for styling repeated Radzen components.
10. Avoid deep Radzen overrides unless necessary.
11. Preserve visible focus states.
12. Preserve color contrast and accessibility.
13. Support responsive layouts and browser zoom.
14. Respect reduced-motion preferences.
15. Summarize any introduced global CSS, tokens, or vendor overrides.

---

## Review checklist

Use this checklist for non-trivial CSS changes:

```text
[ ] Component-specific styles are in `.razor.css` where appropriate.
[ ] Global CSS is used only for global concerns.
[ ] Existing CSS conventions are followed.
[ ] Class names are semantic.
[ ] Design tokens or CSS variables are used for repeated values.
[ ] Inline styles were avoided.
[ ] `!important` was avoided or justified.
[ ] Radzen overrides are isolated and documented.
[ ] Focus states remain visible.
[ ] Color is not the only state indicator.
[ ] Contrast is preserved.
[ ] Layout works at narrow widths.
[ ] Browser zoom and text wrapping are considered.
[ ] Motion respects reduced-motion preferences.
[ ] No broad selectors accidentally affect unrelated components.
[ ] Unused or duplicate CSS was not introduced.
```
