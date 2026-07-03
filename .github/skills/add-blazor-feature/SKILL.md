---
name: add-blazor-feature
description: Use this skill when adding a complete Blazor WebAssembly feature that may involve pages, components, services, models, tests, styling, accessibility, Radzen wrappers, and documentation updates.
---

# Add Blazor feature

Use this skill when implementing a complete feature in this C# / Blazor WebAssembly repository.

A feature may include:

- A new page
- One or more components
- Component code-behind files
- Component-scoped CSS
- Typed client services
- Models, DTOs, or request/response objects
- Loading, empty, error, validation, and success states
- Tests
- Documentation updates

For small single-file changes, use the relevant prompt instead. For example:

```text
/create-blazor-component
/create-blazor-page
/create-service
/generate-tests
```

---

## Repository conventions

Follow these repository files:

```text
.github/copilot-instructions.md
.github/instructions/blazor.instructions.md
.github/instructions/html.instructions.md
.github/instructions/css.instructions.md
.github/instructions/accessibility.instructions.md
.github/instructions/testing.instructions.md
```

Use these docs as project context:

```text
docs/solution-overview.md
docs/project-structure.md
docs/architecture.md
docs/build-and-test.md
```

Use `docs/build-and-test.md` as the source of truth for build, test, format, run, and publish commands.

---

## Feature workflow

Follow this workflow.

### 1. Understand the feature

Identify:

- User goal
- Main screen or workflow
- Inputs
- Outputs
- Data source
- Required user actions
- Validation rules
- Error cases
- Empty states
- Loading states
- Success states
- Permission or authorization requirements, if any

If some details are missing, make reasonable assumptions and state them in the final summary. Do not stop unless the missing information makes the implementation unsafe or clearly wrong.

---

### 2. Inspect existing code

Before creating new patterns:

1. Search for similar pages.
2. Search for similar components.
3. Search for existing project-owned wrapper components.
4. Search for existing services.
5. Search for existing models or DTOs.
6. Search for existing tests.
7. Search for existing Radzen usage.
8. Search for existing loading, empty, error, and validation patterns.

Follow the repository’s existing patterns unless they conflict with the explicit instructions.

---

### 3. Plan affected files

Identify whether the feature needs:

```text
Pages/
  FeaturePage.razor
  FeaturePage.razor.cs
  FeaturePage.razor.css

Components/
  FeatureComponent.razor
  FeatureComponent.razor.cs
  FeatureComponent.razor.css

Services/
  IFeatureService.cs
  FeatureService.cs

Models or DTOs/
  FeatureDto.cs
  FeatureRequest.cs
  FeatureResponse.cs

Tests/
  FeaturePageTests.cs
  FeatureComponentTests.cs
  FeatureServiceTests.cs

Docs/
  docs/solution-overview.md
  docs/project-structure.md
  docs/architecture.md
  docs/build-and-test.md
```

Create only the files that are needed.

---

## Blazor component rules

Every component or page must use:

```text
ComponentName.razor
ComponentName.razor.cs
```

Add this when component-specific styling is needed:

```text
ComponentName.razor.css
```

Rules:

- Keep markup and component composition in `.razor`.
- Keep parameters, injected services, state, lifecycle methods, event handlers, and helpers in `.razor.cs`.
- Avoid large `@code` blocks.
- Use partial classes for code-behind files.
- Follow the existing namespace and folder conventions.
- Do not put reusable business logic directly in components.
- Use services for reusable logic and API communication.

---

## Page rules

For new pages:

- Use the existing route style.
- Add a clear route directive.
- Use semantic page structure.
- Include page title behavior if the project uses it.
- Use project-owned layout/header components if available.
- Load data through typed services.
- Handle loading, empty, error, and success states.
- Validate route parameters.
- Use user-friendly error messages.
- Avoid exposing technical exceptions to users.

Recommended page state pattern:

```razor
@if (isLoading)
{
    <LoadingPanel Message="Loading..." />
}
else if (errorMessage is not null)
{
    <ErrorPanel Message="@errorMessage" OnRetry="LoadAsync" />
}
else if (items.Count == 0)
{
    <EmptyState Message="No items found." />
}
else
{
    <FeatureList Items="items" />
}
```

---

## Service rules

Components and pages should call typed services, not raw `HttpClient`.

Preferred flow:

```text
Page or component
    ↓
Typed client service
    ↓
HttpClient
    ↓
API endpoint or external service
```

Service rules:

- Use interfaces if the project pattern uses them.
- Keep service methods focused.
- Use strongly typed request and response models.
- Use cancellation tokens where appropriate.
- Keep API route strings centralized where practical.
- Handle expected errors intentionally.
- Do not swallow exceptions silently.
- Do not store secrets in the Blazor WebAssembly client.
- Register services in dependency injection if needed.

Example shape:

```csharp
public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDto>> GetCustomersAsync(
        CancellationToken cancellationToken = default);
}
```

---

## State management rules

Use local component state by default.

Use local state for:

- Loading flags
- Error messages
- Current selection
- Form editing state
- Dialog open/closed state
- Expanded/collapsed state
- Component-specific UI state

Use scoped services for:

- State shared across multiple components
- Cross-page state
- Cached lookup data
- Current client-side session/UI state

Avoid introducing global or centralized state management unless there is a clear reason.

---

## Project-owned wrapper components

Prefer project-owned components for repeated, styled, behavior-rich, or accessibility-sensitive UI.

Examples:

```text
AppButton
AppTextInput
AppSelect
AppCheckbox
AppDialog
AppConfirmDialog
AppAlert
AppCard
AppTabs
AppDataGrid
LoadingPanel
EmptyState
ErrorPanel
```

Use wrappers because they centralize:

- Styling
- Accessibility
- Radzen usage
- Replaceability
- Testing
- Common behavior

If a wrapper exists, use it.

If a repeated or important UI pattern does not have a wrapper, consider creating one.

Do not create unnecessary wrappers for one-off static markup.

---

## Radzen rules

This repository may use Radzen components.

Direct Radzen usage is acceptable for one-off, simple UI.

Prefer project-owned wrappers when Radzen usage is:

- Repeated
- Styled
- Behavior-rich
- Accessibility-sensitive
- Used in multiple pages
- Part of the design system
- Likely to change later

Do not scatter repeated Radzen implementation details across feature pages.

Do not test Radzen internals. Test the project-owned wrapper contract.

---

## HTML and accessibility rules

Use semantic and accessible markup.

Rules:

- Use buttons for actions.
- Use links for navigation.
- Do not use clickable `div` or `span` elements.
- Use labels for form fields.
- Use headings in logical order.
- Use lists for list content.
- Use tables only for tabular data.
- Ensure icon-only controls have accessible names.
- Ensure keyboard access.
- Ensure visible focus.
- Do not rely only on color to communicate state.
- Keep ARIA minimal and correct.
- Ensure dialogs follow project dialog/focus patterns.
- Do not render raw HTML unless trusted or sanitized.

For non-trivial UI work, consider:

- Keyboard-only testing
- Focus visibility
- Lighthouse
- axe DevTools
- Accessibility Insights
- Screen reader smoke testing where practical

---

## CSS rules

Use component-scoped `.razor.css` for feature-specific styles.

Rules:

- Prefer semantic class names.
- Use existing design tokens / CSS variables.
- Avoid inline styles.
- Avoid `!important` unless justified.
- Preserve accessible contrast.
- Preserve visible focus states.
- Support responsive layouts.
- Support text wrapping and browser zoom.
- Avoid broad global selectors.
- Avoid deep Radzen overrides unless necessary.

If Radzen overrides are unavoidable:

- Keep them narrow.
- Put them near the wrapper or in an approved override location.
- Add a comment explaining why.
- Avoid `!important` unless there is no reasonable alternative.

---

## Testing rules

Behavior changes should normally include tests.

Use:

- xUnit
- bUnit for Blazor component tests
- FluentAssertions
- NSubstitute or Moq
- Bogus when generated test data improves readability
- Playwright for critical end-to-end workflows

Test naming must use:

```text
MethodName_WhenCondition_ShouldExpectedResult
```

Test:

- Service behavior
- Component rendering
- Parameters
- Event callbacks
- Loading state
- Empty state
- Error state
- Validation behavior
- Form submission behavior
- Wrapper component behavior
- Important accessibility attributes
- Regression cases for bug fixes

Do not test:

- Radzen internals
- Private implementation details
- Framework behavior
- Exact markup unless it is part of the component contract
- CSS classes unless they are part of public behavior

---

## Performance rules

For feature UI:

- Use `@key` when rendering lists where identity matters.
- Avoid expensive expressions directly in Razor markup.
- Avoid repeated allocations inside markup loops.
- Use paging, filtering, or `Virtualize<TItem>` for large lists.
- Avoid unnecessary `StateHasChanged`.
- Avoid chatty API calls.
- Avoid over-fetching data.
- Avoid unnecessary JavaScript interop.
- Keep component parameters stable and intentional.

---

## Security rules

Blazor WebAssembly client code is visible to users.

Rules:

- Do not store secrets in the client.
- Do not put connection strings, private keys, or privileged configuration in WebAssembly.
- Do not rely only on client-side authorization.
- Validate protected operations on the server when a server/API exists.
- Do not expose technical exception details to users.
- Avoid unsafe raw HTML rendering.
- Use `MarkupString` only for trusted or sanitized content with a documented reason.
- Do not introduce new dependencies without explaining why.

---

## Documentation rules

Update documentation when the feature changes:

- Solution purpose
- Project structure
- Architecture
- Dependency direction
- Build/run/test commands
- API communication
- State management
- Authentication or authorization
- New dependencies
- Developer workflow

Relevant docs:

```text
docs/solution-overview.md
docs/project-structure.md
docs/architecture.md
docs/build-and-test.md
```

Do not duplicate large sections across docs. Link to the appropriate file instead.

---

## Validation

Use `docs/build-and-test.md` as the source of truth.

Typical validation commands may include:

```bash
dotnet restore
dotnet build
dotnet test
dotnet format --verify-no-changes
```

For UI changes, also recommend manual browser checks:

- Page loads without console errors.
- Loading state appears.
- Empty state appears.
- Error state appears.
- Form validation works.
- Keyboard navigation works.
- Focus is visible.
- Responsive layout works.
- API calls use expected URLs.

Do not invent validation results. If a command was not run, say so.

---

## Implementation checklist

Before finishing, verify:

```text
[ ] Existing patterns were inspected.
[ ] New components use `.razor` and `.razor.cs`.
[ ] `.razor.css` is used for component-specific styling where needed.
[ ] Typed services are used instead of raw `HttpClient` in components.
[ ] Local state is used for local UI state.
[ ] Shared state uses scoped services where appropriate.
[ ] Project-owned wrappers are used where appropriate.
[ ] Repeated/styled Radzen usage is wrapped or intentionally left direct.
[ ] Loading, empty, error, validation, and success states are handled where relevant.
[ ] Semantic HTML is used.
[ ] Accessibility is considered.
[ ] Lists use `@key` where identity matters.
[ ] Tests are added or updated for behavior changes.
[ ] Documentation is updated if structure, architecture, setup, or commands changed.
[ ] Validation commands are run or recommended.
```

---

## Final response format

After implementation, summarize with:

```text
Summary:
- [feature implemented]
- [files created/updated]
- [services/components/models/tests added or updated]

Validation:
- [commands run]
- [commands recommended if not run]
- [manual UI checks recommended]

Notes:
- [assumptions]
- [risks]
- [follow-up work]
```
