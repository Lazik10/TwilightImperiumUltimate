---
applyTo: "**/*.html, **/*.razor"
---

# HTML and Razor markup instructions

These instructions apply to HTML-like markup in this repository, including standalone `.html` files and Blazor `.razor` components.

This repository uses **C# / Blazor WebAssembly** and may use **Radzen components**. Prefer semantic, accessible, maintainable markup and project-owned wrapper components where appropriate.

Detailed accessibility rules belong in `.github/instructions/accessibility.instructions.md`. This file focuses on HTML structure, semantic markup, form structure, safe rendering, and component wrapping conventions.

---

## Core principles

- Prefer semantic HTML over generic containers.
- Keep markup readable and intentionally structured.
- Use native HTML elements correctly before adding ARIA.
- Avoid clickable `<div>` or `<span>` elements.
- Use project-owned components for styled or reusable UI patterns.
- Wrap third-party UI library components, such as Radzen components, behind repository-owned components when practical.
- Keep styling concerns out of raw markup where possible.
- Avoid inline styles unless there is a specific, documented reason.
- Do not render user-provided HTML unless it has been sanitized and explicitly approved.

---

## Blazor and Razor conventions

In `.razor` files:

- Keep markup clear and easy to scan.
- Avoid large inline expressions in markup.
- Move repeated markup into reusable components.
- Move non-trivial event handling or state logic into the component code section or `.razor.cs` file according to the existing project pattern.
- Do not put business logic directly in Razor markup.
- Prefer strongly typed parameters and event callbacks.
- Use `@key` when rendering lists where element or component identity matters.
- Keep conditional rendering simple and readable.
- Prefer meaningful component names that describe the UI purpose.

Good:

```razor
@if (IsLoading)
{
    <LoadingPanel />
}
else if (Items.Count == 0)
{
    <EmptyState Message="No items found." />
}
else
{
    <ItemList Items="Items" />
}
```

Avoid:

```razor
<div onclick="...">Click me</div>
```

Use:

```razor
<button type="button" @onclick="HandleClick">Click me</button>
```

---

## Project-owned wrapper components

This project prefers wrapping styled HTML elements and third-party UI components in repository-owned Blazor components when the element is reused, styled, behavior-rich, or part of the design system.

Recommended pattern:

```text
Components/
  AppButton.razor
  AppButton.razor.cs
  AppButton.razor.css
```

Use wrapper components for:

- Styled buttons.
- Styled links.
- Reusable form fields.
- Inputs with labels and validation.
- Cards, panels, dialogs, tabs, tables, and grids.
- Repeated Radzen components.
- Repeated layout patterns.
- Components that may need to be replaced later.

Reason:

- Third-party components can be replaced in one place.
- Styling stays consistent.
- Markup stays cleaner.
- Accessibility behavior can be centralized.
- Repeated UI patterns are easier to test.

Example:

```razor
<AppButton ButtonStyle="ButtonStyle.Primary"
           OnClick="SaveAsync">
    Save
</AppButton>
```

Instead of repeating Radzen usage everywhere:

```razor
<RadzenButton Text="Save"
              ButtonStyle="ButtonStyle.Primary"
              Click="SaveAsync" />
```

### Wrapper component rules

- Keep wrappers thin unless shared behavior is needed.
- Do not create unnecessary wrappers for one-off static layout.
- Preserve access to important parameters through wrapper parameters.
- Use clear names such as `AppButton`, `AppTextInput`, `AppDialog`, `AppCard`, or project-specific names.
- Document any wrapper that intentionally hides third-party component functionality.
- Avoid leaking third-party component details into many unrelated pages.

---

## Semantic HTML

Use semantic elements where they describe the meaning of the content.

Prefer:

```html
<header>
<nav>
<main>
<section>
<article>
<aside>
<footer>
<button>
<a>
<form>
<label>
<table>
<ul>
<ol>
```

Avoid overusing:

```html
<div>
<span>
```

A `<div>` is acceptable for layout when no semantic element fits.

---

## Page structure

Each page should have a clear structure:

- One main content area.
- A logical heading hierarchy.
- Sections for distinct content areas.
- Navigation elements only for navigation.
- Buttons for actions.
- Links for navigation.

Recommended shape:

```razor
<main>
    <PageHeader Title="Customers" />

    <section aria-labelledby="customer-list-heading">
        <h2 id="customer-list-heading">Customer list</h2>

        <CustomerList Customers="Customers" />
    </section>
</main>
```

Avoid skipping heading levels for visual reasons. Use CSS for visual size, not heading level.

---

## Buttons and links

Use the correct element based on intent.

Use `<button>` for:

- Submitting forms.
- Opening dialogs.
- Saving data.
- Deleting data.
- Toggling UI state.
- Running commands.

Use `<a>` for:

- Navigation to another page.
- Navigation to another route.
- Opening a real URL.
- Download links.

Rules:

- Always specify `type="button"` for non-submit buttons inside or near forms.
- Use `type="submit"` for form submission buttons.
- Do not use `<a href="#">` as a fake button.
- Do not attach click handlers to non-interactive elements unless there is a strong reason and accessibility is fully handled.

Good:

```razor
<button type="button" @onclick="OpenDialog">Open details</button>
<a href="/customers">View customers</a>
```

Avoid:

```razor
<a href="#" @onclick="OpenDialog">Open details</a>
<div @onclick="OpenDialog">Open details</div>
```

---

## Forms

Use clear, semantic form structure.

Rules:

- Use `<form>` or Blazor `EditForm` for form submission.
- Use labels for all user-editable fields.
- Associate labels with inputs.
- Use fieldsets and legends for related groups of fields.
- Show validation messages close to the relevant field.
- Use appropriate input types such as `email`, `tel`, `number`, `date`, and `password`.
- Use `autocomplete` where helpful and safe.
- Mark required fields clearly.
- Do not rely only on placeholder text as a label.
- Do not disable submit buttons without also making the reason clear where appropriate.

Good HTML:

```html
<label for="email">Email address</label>
<input id="email" name="email" type="email" autocomplete="email" required>
```

Good Blazor:

```razor
<EditForm Model="Model" OnValidSubmit="SaveAsync">
    <DataAnnotationsValidator />

    <div class="form-field">
        <label for="customer-name">Customer name</label>
        <InputText id="customer-name" @bind-Value="Model.Name" />
        <ValidationMessage For="() => Model.Name" />
    </div>

    <AppButton Type="submit">Save</AppButton>
</EditForm>
```

---

## Images and media

Rules:

- Use meaningful `alt` text for meaningful images.
- Use empty `alt=""` for decorative images.
- Do not put important text only inside images.
- Specify image dimensions when practical to reduce layout shift.
- Use captions when images need explanation.
- Ensure icons have accessible names when they act as buttons or links.

Good:

```html
<img src="/images/profile.jpg" alt="Profile photo of the current user">
<img src="/images/decorative-border.svg" alt="">
```

---

## Lists

Use real lists for list content.

Use:

```html
<ul>
    <li>First item</li>
    <li>Second item</li>
</ul>
```

or:

```html
<ol>
    <li>Step one</li>
    <li>Step two</li>
</ol>
```

Avoid using repeated `<div>` elements for content that is actually a list.

---

## Tables

Use tables for tabular data, not layout.

Rules:

- Use `<table>` only for tabular data.
- Use `<th>` for headers.
- Use `<caption>` when helpful.
- Use `scope="col"` or `scope="row"` where appropriate.
- Do not use tables for page layout.

Good:

```html
<table>
    <caption>Monthly invoice totals</caption>
    <thead>
        <tr>
            <th scope="col">Month</th>
            <th scope="col">Total</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row">January</th>
            <td>$120.00</td>
        </tr>
    </tbody>
</table>
```

For complex tables or grids, prefer a project-owned wrapper component around the chosen table/grid implementation.

---

## Dialogs and overlays

For dialogs, modals, popovers, and overlays:

- Prefer existing project-owned dialog components.
- If using Radzen dialogs, prefer wrapping them behind project-owned dialog services or components where practical.
- Ensure dialogs have a clear title.
- Ensure keyboard focus behavior is handled.
- Ensure closing behavior is clear.
- Avoid creating custom modal markup unless required.

Detailed keyboard and focus requirements belong in the accessibility instruction file.

---

## Safe HTML rendering

Do not render raw HTML from users, APIs, CMS content, markdown, or external sources unless it has been sanitized and explicitly approved.

Avoid:

```razor
@((MarkupString)UserProvidedHtml)
```

Only use `MarkupString` when:

- The content source is trusted, or the content has been sanitized.
- The reason is documented.
- Safer alternatives were considered.
- The output does not allow script injection or unsafe attributes.

Rules:

- Prefer plain text rendering by default.
- Encode user-provided content.
- Avoid inline event attributes such as `onclick`.
- Do not inject unsanitized HTML into the DOM.
- Do not use `iframe` unless the source and sandboxing are reviewed.

---

## IDs, classes, and data attributes

Rules:

- Use stable IDs only when needed for labels, headings, anchors, tests, or accessibility relationships.
- Do not generate duplicate IDs in loops.
- Prefer semantic class names.
- Avoid classes that describe only visual appearance when a reusable component or design token would be better.
- Use `data-testid` or the project’s existing test attribute convention only when needed for automated tests.
- Avoid using test-specific attributes as styling hooks.

---

## Inline styles and scripts

Avoid inline styles:

```html
<div style="color: red;">
```

Prefer CSS classes or component-scoped CSS:

```html
<div class="validation-message">
```

Avoid inline scripts in HTML files or Razor markup.

If JavaScript is required:

- Use Blazor JavaScript interop intentionally.
- Keep JavaScript isolated and documented.
- Do not add JavaScript for behavior that Blazor can handle cleanly.

---

## Responsive markup

Rules:

- Do not hard-code layout in a way that breaks small screens.
- Prefer flexible containers and reusable layout components.
- Keep content order logical in the DOM.
- Do not rely on visual order alone.
- Test important screens at desktop and mobile widths.

---

## Comments

Use comments sparingly.

Good comments explain:

- Non-obvious structure.
- Workarounds.
- Accessibility relationships.
- Security-sensitive rendering choices.
- Temporary compatibility constraints.

Avoid comments that repeat the markup.

---

## Anti-patterns

Avoid:

```html
<div onclick="...">
<span onclick="...">
<a href="#">
<button>Submit</button> <!-- inside a form without clear type -->
<input placeholder="Email"> <!-- no label -->
<img src="chart.png"> <!-- missing alt text -->
<table> <!-- used only for layout -->
<div class="red-text"> <!-- style-specific naming -->
```

Prefer:

```html
<button type="button">Open</button>
<a href="/customers">Customers</a>
<label for="email">Email</label>
<input id="email" name="email" type="email">
<img src="chart.png" alt="Sales increased from January to March">
```

---

## Copilot behavior

When generating or editing HTML/Razor markup:

1. Prefer existing project-owned components.
2. Prefer semantic HTML.
3. Prefer accessible structure.
4. Keep Radzen usage behind wrapper components when the UI pattern is reused or styled.
5. Do not scatter third-party UI library details across many pages.
6. Do not introduce raw HTML rendering unless explicitly required.
7. Include loading, empty, error, and success states where relevant.
8. Keep markup readable and maintainable.
9. Follow existing project conventions before introducing a new pattern.
10. If unsure whether a wrapper component exists, search the repository before creating raw markup or direct Radzen usage.
