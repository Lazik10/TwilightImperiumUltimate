---
description: Create a routable Blazor page following this repository's C# / Blazor WebAssembly conventions.
---

# Create Blazor page

Create a routable Blazor page for the requested feature or screen.

Follow `.github/copilot-instructions.md` and the auto-applying `.github/instructions/*.instructions.md` files for `.razor`/`.razor.cs`/`.css` (`blazor`, `html`, `css`, `accessibility`, `testing`) — do not restate their rules here. Review `docs/*.md` before non-trivial changes.

---

## Task

Create a Blazor page based on the user's request.

If important information is missing, make a reasonable assumption and state it in the summary.

---

## Before creating files

1. Identify the page name, route, purpose, and expected user actions.
2. Search for similar existing pages.
3. Follow the existing page folder and namespace conventions.
4. Identify required services, models, child components, and wrappers.
5. Decide whether the page needs new reusable child components.
6. Decide whether tests are needed.
7. Decide whether docs need updates.

---

## File creation rules

Create:

```text
PageName.razor
PageName.razor.cs
```

Create when page-specific styling is needed:

```text
PageName.razor.css
```

Create or update tests when behavior is added:

```text
PageNameTests.cs
```

Follow existing repository structure. Do not invent new folders if similar pages already establish a pattern.

---

## Page structure

The `.razor` file should contain:

- Route directive
- Page title, if the project uses it
- Semantic page markup
- Project-owned components
- Loading, empty, error, and success states
- Child components for complex sections

The `.razor.cs` file should contain:

- Partial class
- Injected typed services
- Local state
- Lifecycle methods
- Event handlers
- Data loading
- Navigation logic
- Helper methods

Avoid large `@code` blocks.

---

## Routing

Use the project’s existing route style.

Example:

```razor
@page "/customers"
```

Rules:

- Use clear, predictable route names.
- Do not introduce route parameters unless needed.
- Validate route parameters before using them.
- Show a clear not-found or error state for invalid parameters.
- Use navigation services according to the existing project pattern.

---

## Data loading

Pages should use typed services rather than raw `HttpClient`.

Preferred flow:

```text
Page
    ↓
Typed client service
    ↓
HttpClient
    ↓
API endpoint or external service
```

Include a loading state, empty state, error state (with retry when appropriate), and user-friendly error messages.

---

## Cross-cutting rules

Apply the rules already defined in the repository instructions instead of restating them here:

- Project-owned wrapper components and Radzen usage — `.github/instructions/blazor.instructions.md`
- Semantic HTML, forms, buttons vs. links — `.github/instructions/html.instructions.md`
- Accessibility (keyboard, focus, ARIA, labels) — `.github/instructions/accessibility.instructions.md`
- Test stack and naming (`MethodName_WhenCondition_ShouldExpectedResult`) — `.github/instructions/testing.instructions.md`; for pages, test loading/empty/error states, main actions, event callbacks, and navigation behavior.

---

## Validation

Use `docs/build-and-test.md`.

```bash
dotnet build
dotnet test
dotnet format --verify-no-changes
```

---

## Output summary

Return:

```text
Summary:
- [files created/updated]
- [behavior added]
- [services/components reused]

Validation:
- [commands run or recommended]

Notes:
- [assumptions]
- [follow-up work]
```
