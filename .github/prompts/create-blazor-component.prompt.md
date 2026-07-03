---
description: Create a Blazor component following this repository's C# / Blazor WebAssembly conventions.
---

# Create Blazor component

Create a Blazor component for the requested UI or behavior.

This repository uses:

- C#
- Blazor WebAssembly
- Razor
- CSS / Blazor CSS isolation
- Project-owned wrapper components
- Some Radzen components
- `.razor` + `.razor.cs` for all components
- `.razor.css` for component-specific styles when needed
- xUnit and bUnit for tests where appropriate

Follow the repository instructions:

```text
.github/copilot-instructions.md
.github/instructions/blazor.instructions.md
.github/instructions/html.instructions.md
.github/instructions/css.instructions.md
.github/instructions/accessibility.instructions.md
.github/instructions/testing.instructions.md
```

Also review relevant documentation before making non-trivial changes:

```text
docs/solution-overview.md
docs/project-structure.md
docs/architecture.md
docs/build-and-test.md
```

---

## Task

Create a Blazor component based on the user's request.

If important information is missing, make a reasonable assumption and state it in the summary. Do not stop unless the missing information would make the implementation unsafe or clearly wrong.

---

## Before creating files

1. Identify the requested component name and purpose.
2. Search for similar existing components.
3. Follow the existing folder structure.
4. Check whether a project-owned wrapper component already exists.
5. Check whether direct Radzen usage is appropriate or whether a project-owned wrapper should be used.
6. Identify required parameters, event callbacks, services, state, and child content.
7. Decide whether the component needs component-scoped CSS.
8. Decide whether tests are needed.

---

## File creation rules

Create these files for every component:

```text
ComponentName.razor
ComponentName.razor.cs
```

Create this file when component-specific styling is needed:

```text
ComponentName.razor.css
```

Create or update tests when the component has behavior:

```text
ComponentNameTests.cs
```

Follow the repository's existing test project structure. If no test project exists, recommend the test instead of inventing a full test setup unless asked.

---

## Component structure

The `.razor` file should contain markup and component composition.

The `.razor.cs` file should contain:

- Partial class
- Parameters
- Injected services
- Event callbacks
- Component state
- Lifecycle methods
- Event handlers
- Helper methods

Avoid large `@code` blocks in `.razor` files.

Preferred shape:

```razor
<section class="component-name">
    <!-- Markup here -->
</section>
```

```csharp
using Microsoft.AspNetCore.Components;

namespace ProjectName.Client.Components;

public partial class ComponentName
{
    [Parameter]
    public string Title { get; set; } = string.Empty;
}
```

Follow the existing namespace, folder, and component patterns in the repository.

---

## Parameters

Use strongly typed parameters.

Rules:

- Use `[Parameter]` for component inputs.
- Use `[EditorRequired]` for required parameters where appropriate.
- Use `EventCallback` or `EventCallback<T>` for callbacks.
- Avoid `Action` or `Func<Task>` callback parameters unless the existing project requires them.
- Do not mutate parameters directly.
- Avoid overly generic `object` parameters.
- Prefer `IReadOnlyList<T>` or `IEnumerable<T>` for read-only collections.

Example:

```csharp
[Parameter, EditorRequired]
public IReadOnlyList<CustomerDto> Customers { get; set; } = [];

[Parameter]
public EventCallback<CustomerDto> CustomerSelected { get; set; }
```

---

## UI and markup rules

Use semantic, accessible markup.

Rules:

- Use buttons for actions.
- Use links for navigation.
- Avoid clickable `div` or `span` elements.
- Use labels for form fields.
- Use headings in logical order.
- Use lists for list content.
- Use tables only for tabular data.
- Include loading, empty, error, and success states where relevant.
- Do not render raw HTML unless it is trusted or sanitized.
- Use `@key` when rendering lists where identity matters.

---

## Project-owned wrapper components

Prefer existing project-owned components for repeated or styled UI.

Examples:

```text
AppButton
AppTextInput
AppSelect
AppCheckbox
AppDialog
AppCard
AppDataGrid
LoadingPanel
EmptyState
ErrorPanel
```

If a suitable wrapper exists, use it.

If a repeated or styled UI pattern does not have a wrapper yet, consider creating one when appropriate.

Do not scatter direct third-party component usage across pages and components when a project-owned wrapper would centralize styling, accessibility, and replaceability.

---

## Radzen rules

Direct Radzen usage is acceptable for one-off, simple UI.

Prefer a project-owned wrapper when Radzen usage is:

- Repeated
- Styled
- Accessibility-sensitive
- Behavior-rich
- Used across multiple pages
- Part of the design system
- Likely to change later

Do not test Radzen internals. Test the project-owned wrapper behavior.

---

## Styling rules

Use `.razor.css` for component-specific styles.

Rules:

- Prefer semantic class names.
- Use existing design tokens / CSS variables when available.
- Avoid inline styles.
- Avoid `!important` unless justified.
- Preserve visible focus states.
- Preserve accessible contrast.
- Support responsive layout and text wrapping.
- Avoid global CSS for component-specific styling.

---

## Services and data loading

Components should call typed client services, not raw `HttpClient`.

Preferred flow:

```text
Component
    ↓
Typed client service
    ↓
HttpClient
    ↓
API endpoint or external service
```

If data loading is needed:

- Show a loading state.
- Show an error state.
- Show an empty state when appropriate.
- Keep error messages user-friendly.
- Avoid exposing technical exception details to users.
- Use cancellation tokens where the existing project pattern supports them.

---

## Accessibility expectations

For interactive components:

- Ensure keyboard access.
- Ensure focus is visible.
- Use accessible labels and names.
- Ensure icon-only buttons have accessible names.
- Do not rely only on color.
- Ensure validation messages are clear and close to fields.
- Ensure dialogs and overlays follow project accessibility patterns.
- Prefer semantic HTML before ARIA.
- Keep ARIA minimal and correct.

---

## Testing expectations

Add or update tests when the component has behavior.

Use:

- xUnit for test framework
- bUnit for Blazor component tests
- FluentAssertions for readable assertions when available
- NSubstitute or Moq when mocking is useful
- Bogus when realistic generated test data improves readability

Test naming must use:

```text
MethodName_WhenCondition_ShouldExpectedResult
```

Test behavior such as:

- Rendered content
- Parameters
- Event callbacks
- Conditional rendering
- Loading state
- Empty state
- Error state
- Validation behavior
- Wrapper component behavior
- Important accessibility attributes

Do not test:

- Radzen internals
- Private implementation details
- Exact markup unless it is part of the component contract
- CSS classes unless they are part of expected behavior or public component contract

---

## Validation

Use `docs/build-and-test.md` as the source of truth.

Typical commands may include:

```bash
dotnet build
dotnet test
dotnet format --verify-no-changes
```

If commands cannot be run, list the commands the developer should run.

---

## Output summary

After implementation, summarize:

```text
Summary:
- Created/updated [files]
- Added [behavior]
- Reused [existing components/services]
- Added/updated [tests], if applicable

Validation:
- [commands run or recommended]

Notes:
- [assumptions]
- [follow-up work]
```

---

## Checklist

Before finishing, verify:

```text
[ ] Component has `.razor` and `.razor.cs`.
[ ] Component has `.razor.css` if styling is needed.
[ ] Markup is semantic and accessible.
[ ] Reused project-owned components where appropriate.
[ ] Radzen usage is wrapped or intentionally direct.
[ ] Parameters are strongly typed.
[ ] EventCallback or EventCallback<T> is used for callbacks.
[ ] Typed services are used instead of raw HttpClient.
[ ] Loading, empty, error, and success states are handled where relevant.
[ ] Focus and keyboard behavior are considered.
[ ] Tests are added or recommended when behavior exists.
[ ] Validation commands are run or recommended.
```
