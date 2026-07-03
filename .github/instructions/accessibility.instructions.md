---
applyTo: "**/*.html, **/*.razor, **/*.css"
---

# Accessibility instructions

These instructions apply to HTML, Razor markup, Blazor components, and CSS in this repository.

This repository uses **C# / Blazor WebAssembly** and may use **Radzen components**. Accessibility should be treated as a normal quality requirement, not an optional final check.

Use **WCAG 2.2 AA** as the baseline accessibility target. For complex interactive components such as dialogs, menus, tabs, grids, comboboxes, and popovers, follow established WAI-ARIA Authoring Practices patterns.

These instructions are intentionally practical and project-focused. They should guide Copilot toward accessible UI implementation without turning every task into a full accessibility audit.

---

## Core principles

- Prefer semantic HTML before using ARIA.
- Use native browser behavior wherever possible.
- Ensure every interactive element is keyboard accessible.
- Ensure focus is visible and predictable.
- Ensure forms have labels, validation messages, and clear error states.
- Ensure color is not the only way to communicate meaning.
- Ensure text and interactive elements have sufficient contrast.
- Ensure loading, empty, error, and success states are accessible.
- Do not hide content from assistive technology unless it is intentionally decorative or redundant.
- Do not create custom interactive components when a native element or existing accessible component works.

---

## Project-owned accessible wrapper components

This project prefers wrapping repeated styled HTML elements and repeated third-party UI components in repository-owned Blazor components.

Use project-owned wrappers for accessibility-sensitive UI such as:

- Buttons
- Links styled as buttons
- Text inputs
- Selects and dropdowns
- Checkboxes and radio groups
- Dialogs and modals
- Toasts and alerts
- Tabs
- Menus
- Tooltips
- Data grids and tables
- Cards and panels with actions
- Loading indicators
- Empty states
- Error states

Recommended examples:

```text
Components/
  AppButton.razor
  AppButton.razor.cs
  AppButton.razor.css

  AppTextInput.razor
  AppTextInput.razor.cs
  AppTextInput.razor.css

  AppDialog.razor
  AppDialog.razor.cs
  AppDialog.razor.css

  AppDataGrid.razor
  AppDataGrid.razor.cs
  AppDataGrid.razor.css
```

Why this matters:

- Accessibility behavior can be fixed in one place.
- Radzen or another UI library can be replaced more easily.
- Labels, validation, focus, ARIA, and keyboard behavior can be standardized.
- Repeated patterns become easier to test.
- Pages stay simpler and more consistent.

When using Radzen components repeatedly, prefer a project-owned wrapper unless the usage is clearly one-off and simple.

---

## Semantic HTML first

Use semantic HTML elements according to their meaning.

Prefer:

```html
<header>
<nav>
<main>
<section>
<article>
<aside>
<footer>
button
a
form
label
fieldset
legend
table
ul
ol
```

Avoid using generic elements for interactive behavior:

```html
<div @onclick="...">
<span @onclick="...">
```

Use:

```razor
<button type="button" @onclick="HandleClick">Open</button>
```

Only use ARIA when native HTML does not provide the needed semantics.

Bad:

```html
<div role="button" tabindex="0">Save</div>
```

Good:

```html
<button type="button">Save</button>
```

---

## Page structure and landmarks

Each page should have a clear structure.

Rules:

- Use one main content area.
- Use headings in a logical order.
- Do not skip heading levels for visual styling.
- Use CSS to change heading appearance instead of choosing the wrong heading level.
- Use navigation landmarks for navigation areas.
- Use sections with headings where content is complex.
- Avoid multiple competing `<main>` elements on one page.

Recommended shape:

```razor
<main>
    <PageHeader Title="Orders" />

    <section aria-labelledby="orders-heading">
        <h2 id="orders-heading">Orders</h2>

        <OrdersList Orders="Orders" />
    </section>
</main>
```

---

## Keyboard accessibility

All interactive functionality must be usable with a keyboard.

Check:

- `Tab` moves to interactive elements in a logical order.
- `Shift + Tab` moves backward logically.
- `Enter` activates links and buttons where appropriate.
- `Space` activates buttons, checkboxes, and similar controls.
- `Esc` closes dialogs, menus, popovers, or overlays where expected.
- Arrow keys work for complex widgets when the pattern requires them.
- Focus does not get lost after an action.
- Focus is moved intentionally after opening or closing dialogs.
- Disabled controls are not focusable unless the design intentionally requires explanation.

Avoid:

- Positive `tabindex` values.
- Removing focus outlines without replacing them.
- Keyboard traps.
- Click-only interactions.
- Hover-only interactions.

Use `tabindex="0"` only when a non-focusable element truly must become focusable. Prefer native interactive elements instead.

Avoid:

```html
<div tabindex="3">...</div>
```

Prefer natural DOM order.

---

## Focus visibility

Focus must be visible for keyboard users.

CSS rules:

- Do not remove outlines globally.
- Do not use `outline: none` unless a clear replacement focus style is provided.
- Focus indicators must be visible against the surrounding background.
- Focus states should be consistent across project-owned components.
- Custom components should expose a visible focus state.

Avoid:

```css
*:focus {
    outline: none;
}
```

Better:

```css
.app-button:focus-visible {
    outline: 2px solid currentColor;
    outline-offset: 2px;
}
```

Use `:focus-visible` when appropriate, but make sure keyboard focus remains visible.

---

## Buttons and links

Use the correct element based on intent.

Use a button for actions:

```razor
<button type="button" @onclick="OpenDialog">Open details</button>
```

Use a link for navigation:

```html
<a href="/orders">View orders</a>
```

Rules:

- Do not use fake links such as `<a href="#">`.
- Do not use clickable `<div>` or `<span>`.
- Always specify `type="button"` for non-submit buttons near or inside forms.
- Link text should describe the destination or action.
- Avoid vague link text such as “click here” or “read more” when used without context.

Good:

```html
<a href="/invoices/123">View invoice 123</a>
```

Avoid:

```html
<a href="/invoices/123">Click here</a>
```

---

## Forms and validation

Forms must be understandable, operable, and error-tolerant.

Rules:

- Every input must have a visible label or a clearly justified accessible name.
- Do not rely only on placeholder text as a label.
- Associate labels with inputs.
- Use `fieldset` and `legend` for related groups such as radio buttons and checkboxes.
- Mark required fields clearly.
- Use appropriate input types.
- Use `autocomplete` where helpful and safe.
- Show validation messages close to the field.
- Make validation messages available to assistive technology.
- Keep error messages clear and actionable.
- Preserve user input after validation errors when possible.
- Do not disable submit buttons without explaining what is missing where appropriate.

Good:

```razor
<EditForm Model="Model" OnValidSubmit="SaveAsync">
    <DataAnnotationsValidator />

    <div class="form-field">
        <label for="email">Email address</label>
        <InputText id="email"
                   type="email"
                   autocomplete="email"
                   @bind-Value="Model.Email" />
        <ValidationMessage For="() => Model.Email" />
    </div>

    <AppButton Type="submit">Save</AppButton>
</EditForm>
```

For reusable fields, prefer a project-owned wrapper such as:

```razor
<AppTextInput Label="Email address"
              InputType="email"
              AutoComplete="email"
              @bind-Value="Model.Email"
              ValidationFor="() => Model.Email" />
```

---

## Error messages and status messages

Users must be able to perceive and understand state changes.

Rules:

- Show clear error messages.
- Show success messages when an action completes and the result is not obvious.
- Show loading states for async operations.
- Show empty states for empty lists or search results.
- Do not rely only on color to indicate errors or success.
- Use accessible live regions carefully for important dynamic updates.

Examples of state components:

```razor
<LoadingPanel Message="Loading orders..." />
<EmptyState Message="No orders found." />
<ErrorPanel Message="Orders could not be loaded. Try again." />
```

For dynamic messages, consider appropriate live region behavior:

```html
<div role="status" aria-live="polite">
    Changes saved.
</div>
```

Use assertive announcements only for urgent information:

```html
<div role="alert">
    Payment failed. Check your card details.
</div>
```

Do not overuse live regions, because excessive announcements can make the UI difficult for screen reader users.

---

## Images, icons, and media

Rules:

- Meaningful images need meaningful `alt` text.
- Decorative images should use `alt=""`.
- Icon-only buttons need an accessible name.
- Do not put important text only inside an image.
- Captions should be used when images need explanation.
- Avoid auto-playing audio or video.
- Provide captions or transcripts for important audio/video content where applicable.

Good:

```html
<img src="/images/profile.jpg" alt="Profile photo of the current user">
<img src="/images/divider.svg" alt="">
```

Icon button:

```razor
<button type="button" aria-label="Delete order">
    <TrashIcon aria-hidden="true" />
</button>
```

---

## Color and contrast

Color must not be the only way information is communicated.

Rules:

- Use text, icons, labels, or patterns in addition to color.
- Ensure text has sufficient contrast against its background.
- Ensure buttons, form controls, and focus indicators are visible.
- Ensure disabled states remain understandable.
- Check hover, active, selected, error, and focus states.
- Do not use low-contrast placeholder text as the only label.

Bad:

```html
<span class="red">Invalid</span>
```

Better:

```html
<span class="validation-message">
    Error: Email address is required.
</span>
```

CSS should preserve contrast in:

- Normal state
- Hover state
- Focus state
- Disabled state
- Selected state
- Error state

---

## Motion and animation

Respect users who prefer reduced motion.

Rules:

- Avoid unnecessary animation.
- Do not use flashing content.
- Do not use animation that blocks task completion.
- Keep transitions short and subtle.
- Provide reduced-motion behavior for animations that are not essential.

CSS example:

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

Use this carefully and adapt it to the project’s CSS strategy.

---

## Hidden content

Be intentional about hidden content.

Rules:

- `display: none` and `visibility: hidden` hide content visually and from assistive technology.
- Use visually hidden utility classes only when content should be available to screen readers.
- Do not hide focusable interactive elements from sighted users.
- Do not create invisible focus traps.

Example visually hidden utility:

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

Use visually hidden text for icon-only controls when `aria-label` is not preferred.

---

## ARIA usage

Use ARIA only when needed.

Rules:

- Prefer native elements over ARIA roles.
- Do not change the meaning of native elements incorrectly.
- Do not use ARIA to hide focusable content.
- Ensure ARIA references point to existing IDs.
- Ensure generated IDs are unique.
- Use `aria-expanded` for expandable controls.
- Use `aria-controls` when it helps connect a control to the controlled region.
- Use `aria-describedby` for descriptions, hints, and error messages.
- Use `aria-labelledby` when visible text labels a region or widget.
- Keep ARIA states in sync with UI state.

Good expandable example:

```razor
<button type="button"
        aria-expanded="@IsOpen"
        aria-controls="filter-panel"
        @onclick="ToggleFilters">
    Filters
</button>

@if (IsOpen)
{
    <section id="filter-panel" aria-label="Filter options">
        ...
    </section>
}
```

Avoid:

```html
<button role="heading">Save</button>
```

---

## Dialogs and modals

Dialogs are accessibility-sensitive. Prefer project-owned dialog wrappers.

Rules:

- Dialogs must have a clear accessible name.
- Focus should move into the dialog when it opens.
- Focus should return to the triggering element when it closes.
- `Esc` should close the dialog unless there is a good reason not to.
- Background content should not be reachable by keyboard while a modal dialog is open.
- Dialogs should have a clear close action.
- Avoid opening dialogs automatically unless necessary.
- Avoid nested dialogs unless there is no better design.

For repeated Radzen dialogs, prefer a project-owned wrapper or dialog service abstraction such as:

```text
AppDialog
AppConfirmDialog
AppDialogService
```

Do not create custom modal markup unless the focus management and keyboard behavior are handled correctly.

---

## Dropdowns, menus, popovers, and tooltips

Rules:

- Prefer native controls or accessible project-owned wrappers.
- Use menus only for actual command menus, not ordinary navigation or layout.
- Ensure keyboard interaction works.
- Ensure the component can be dismissed.
- Ensure focus is managed predictably.
- Do not put important content only in a tooltip.
- Tooltips must not be required to complete a task.
- Hover-only content must also be accessible by keyboard and touch users.

For Radzen dropdowns, menus, and popovers, prefer repository-owned wrappers when the pattern is repeated.

---

## Tabs

Rules:

- Use tabs only for switching between related panels of content.
- The active tab must be identifiable visually and programmatically.
- Keyboard navigation should follow the expected tab pattern.
- Each tab should control a related tab panel.
- Do not use tabs as general page navigation unless the pattern is intentionally designed and accessible.

Prefer a project-owned wrapper around tab components when tabs are reused.

---

## Tables, grids, and data grids

Use tables for tabular data.

Rules:

- Use table headers.
- Use captions or accessible labels where helpful.
- Do not use tables for layout.
- For complex grids, prefer an accessible grid component or project-owned wrapper.
- Ensure sorting, filtering, paging, and selection are keyboard accessible.
- Ensure row actions have clear labels.
- Avoid icon-only actions without accessible names.
- Make empty, loading, and error states accessible.

For Radzen data grids, prefer a project-owned wrapper such as:

```text
AppDataGrid
```

The wrapper should standardize:

- Loading state
- Empty state
- Error state
- Row action labels
- Keyboard behavior expectations
- Accessible names
- Pagination labels
- Column header behavior

---

## Blazor-specific accessibility rules

When generating Blazor components:

- Use `EventCallback<T>` for child-to-parent events where appropriate.
- Use stable IDs for label/input relationships.
- Avoid duplicate IDs in loops.
- Use `@key` when rendering lists where identity matters.
- Keep async UI state accessible.
- Show loading states during async operations.
- Keep validation messages associated with their fields.
- Ensure conditional rendering does not unexpectedly remove focused elements.
- After deleting, saving, or closing UI, ensure focus lands somewhere logical.
- Avoid calling JavaScript for accessibility behavior unless necessary.

Example stable ID pattern:

```razor
<label for="@InputId">@Label</label>
<InputText id="@InputId" @bind-Value="Value" />
```

In the code-behind:

```csharp
private string InputId { get; } = $"input-{Guid.NewGuid():N}";
```

Use this carefully in components that need unique IDs.

---

## CSS accessibility rules

CSS must not break accessibility.

Rules:

- Do not remove focus outlines globally.
- Do not hide content that should remain available to screen readers.
- Do not use visual order that conflicts with logical DOM order.
- Do not make text too small to read.
- Do not rely on hover-only behavior.
- Do not create low-contrast text.
- Do not use fixed heights that clip text when users zoom.
- Support text resizing and browser zoom.
- Keep responsive layouts usable at small widths.
- Ensure custom scroll areas are keyboard accessible.
- Preserve visible states for hover, focus, active, selected, disabled, error, and loading.

Avoid:

```css
button:focus {
    outline: none;
}
```

Prefer:

```css
button:focus-visible {
    outline: 2px solid currentColor;
    outline-offset: 2px;
}
```

---

## Responsive and zoom behavior

The UI should remain usable when:

- Browser zoom is increased.
- Text size is increased.
- The viewport is narrow.
- Content wraps onto multiple lines.
- Labels and buttons contain longer translated text.

Rules:

- Avoid fixed pixel heights for text-heavy components.
- Allow content to wrap.
- Avoid clipping validation messages.
- Ensure sticky headers, panels, or footers do not cover focused content.
- Keep touch targets large enough for comfortable use.

---

## Screen reader guidance

Screen reader users should be able to understand structure, controls, and state.

Rules:

- Use meaningful headings.
- Use meaningful labels.
- Use meaningful button and link text.
- Use accessible names for icon-only controls.
- Use `aria-describedby` for hints and error messages.
- Use `role="status"` or `aria-live="polite"` for important non-urgent updates.
- Use `role="alert"` only for urgent errors.
- Avoid excessive live region announcements.
- Avoid repeating the same text visually and through ARIA in confusing ways.
- Do not use `aria-hidden="true"` on focusable elements or their parents.

Good:

```html
<button type="button" aria-label="Close dialog">
    <span aria-hidden="true">×</span>
</button>
```

---

## Testing expectations

When UI changes are made, recommend or perform accessibility checks appropriate to the change.

Manual checks:

- Navigate using only the keyboard.
- Confirm focus is visible.
- Confirm focus order is logical.
- Confirm dialogs trap and restore focus appropriately.
- Confirm forms have labels and validation messages.
- Confirm error messages are clear.
- Confirm icon-only controls have accessible names.
- Confirm content is usable at narrow widths.
- Confirm browser zoom does not break the layout.
- Confirm color is not the only indicator.

Tool checks:

- Lighthouse
- axe DevTools
- Accessibility Insights
- Browser developer tools accessibility tree
- Screen reader smoke testing where practical

Screen reader smoke tests may use:

- Narrator on Windows
- NVDA on Windows
- VoiceOver on macOS/iOS
- TalkBack on Android

Automated tools do not catch everything. Passing an automated accessibility scan does not guarantee the UI is accessible.

---

## Common anti-patterns

Avoid:

```html
<div onclick="...">Save</div>
<span onclick="...">Delete</span>
<a href="#">Open modal</a>
<input placeholder="Email">
<img src="chart.png">
<button>Submit</button>
```

Avoid CSS like:

```css
*:focus {
    outline: none;
}

.error {
    color: red;
}
```

Avoid Razor patterns like:

```razor
<div @onclick="SaveAsync">Save</div>
@((MarkupString)UserProvidedHtml)
```

Prefer:

```html
<button type="button">Save</button>
<a href="/orders">Orders</a>
<label for="email">Email address</label>
<input id="email" name="email" type="email" autocomplete="email">
<img src="chart.png" alt="Sales increased from January to March">
```

Prefer CSS like:

```css
.validation-message {
    color: var(--color-danger-text);
}

.app-button:focus-visible {
    outline: 2px solid currentColor;
    outline-offset: 2px;
}
```

---

## Copilot behavior

When generating or editing UI:

1. Start with semantic HTML.
2. Prefer native interactive elements.
3. Prefer existing project-owned accessible components.
4. Wrap repeated Radzen components behind project-owned components.
5. Include labels, accessible names, and validation behavior.
6. Ensure keyboard access and visible focus.
7. Include loading, empty, error, and success states where relevant.
8. Do not rely only on color to communicate meaning.
9. Avoid unsafe raw HTML rendering.
10. Keep ARIA minimal, correct, and synchronized with state.
11. Mention accessibility assumptions in summaries for UI changes.
12. Recommend manual keyboard testing for interactive changes.
13. Recommend accessibility tooling checks for non-trivial UI changes.

---

## Review checklist for non-trivial UI changes

Use this checklist when reviewing generated UI:

```text
[ ] Semantic elements are used correctly.
[ ] Buttons and links are used according to intent.
[ ] All form fields have labels.
[ ] Validation messages are clear and close to the field.
[ ] Keyboard navigation works.
[ ] Focus is visible.
[ ] Focus order is logical.
[ ] Dialogs manage focus correctly.
[ ] Icon-only controls have accessible names.
[ ] Images have appropriate alt text.
[ ] Color is not the only indicator.
[ ] Text and controls have sufficient contrast.
[ ] Loading, empty, error, and success states are handled.
[ ] Responsive layout works at narrow widths.
[ ] No unsafe raw HTML rendering was introduced.
[ ] Repeated Radzen usage is wrapped or intentionally left direct.
```
