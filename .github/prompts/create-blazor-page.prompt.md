---
description: Create a routable Blazor page following this repository's C# / Blazor WebAssembly conventions.
---

# Create Blazor page

Create a routable Blazor page for the requested feature or screen.

Follow these repository instructions:

```text
.github/copilot-instructions.md
.github/instructions/blazor.instructions.md
.github/instructions/html.instructions.md
.github/instructions/css.instructions.md
.github/instructions/accessibility.instructions.md
.github/instructions/testing.instructions.md
```

Review relevant docs before non-trivial changes:

```text
docs/solution-overview.md
docs/project-structure.md
docs/architecture.md
docs/build-and-test.md
```

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

Include:

- Loading state
- Empty state
- Error state
- Retry behavior when appropriate
- User-friendly error messages

---

## UI rules

- Prefer project-owned wrapper components.
- Wrap repeated or styled Radzen usage.
- Use direct Radzen only for simple one-off UI.
- Use semantic HTML.
- Use buttons for actions.
- Use links for navigation.
- Use labels for form fields.
- Include accessible names for icon-only actions.
- Do not rely only on color to show state.
- Use `@key` when rendering lists where identity matters.

---

## Tests

Add or update tests when the page has meaningful behavior.

Use:

- xUnit
- bUnit
- FluentAssertions
- NSubstitute or Moq when useful

Test naming:

```text
MethodName_WhenCondition_ShouldExpectedResult
```

Test:

- Loading state
- Successful data load
- Empty state
- Error state
- Main actions
- Event callbacks
- Navigation behavior, if relevant
- Validation behavior, if relevant

---

## Validation

Use `docs/build-and-test.md`.

Typical commands:

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
