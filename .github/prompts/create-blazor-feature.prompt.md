---
description: Create a complete Blazor WebAssembly feature with pages, components, services, models, tests, and documentation updates where needed.
---

# Create Blazor feature

Create a complete feature based on the user's request.

This prompt is for multi-file work. For a single component, use `create-blazor-component.prompt.md`. For a single page, use `create-blazor-page.prompt.md`.

Follow:

```text
.github/copilot-instructions.md
.github/instructions/blazor.instructions.md
.github/instructions/html.instructions.md
.github/instructions/css.instructions.md
.github/instructions/accessibility.instructions.md
.github/instructions/testing.instructions.md
```

Review:

```text
docs/solution-overview.md
docs/project-structure.md
docs/architecture.md
docs/build-and-test.md
```

---

## Task

Implement the requested Blazor feature using the repository architecture and conventions.

If the request is large, produce a short implementation plan first.

---

## Feature workflow

1. Understand the requested user workflow.
2. Search for similar existing features.
3. Identify affected projects and folders.
4. Identify pages, components, services, DTOs/models, state, tests, and docs.
5. Prefer existing project-owned wrapper components.
6. Create or update typed services instead of using raw `HttpClient` in components.
7. Add loading, empty, error, validation, and success states.
8. Add component-scoped CSS where needed.
9. Add or update tests.
10. Update docs when architecture, commands, or structure changes.
11. Summarize assumptions and validation.

---

## Expected file types

Depending on the feature, create or update:

```text
Pages/
  FeaturePage.razor
  FeaturePage.razor.cs
  FeaturePage.razor.css

Components/
  FeatureWidget.razor
  FeatureWidget.razor.cs
  FeatureWidget.razor.css

Services/
  IFeatureService.cs
  FeatureService.cs

Models or DTOs/
  FeatureDto.cs
  FeatureRequest.cs
  FeatureResponse.cs

Tests/
  FeatureServiceTests.cs
  FeaturePageTests.cs
  FeatureWidgetTests.cs
```

Do not create files that are not needed.

---

## Component and page rules

- Every component/page uses `.razor` and `.razor.cs`.
- Add `.razor.css` when styling is needed.
- Keep markup in `.razor`.
- Keep logic in `.razor.cs`.
- Keep reusable business logic in services.
- Use typed services for API communication.
- Prefer local component state.
- Use scoped services for shared state.
- Use project-owned wrappers for repeated/styled UI and Radzen usage.
- Use direct Radzen only for simple one-off UI.

---

## UI states

For data-driven UI, include relevant states:

```text
Loading
Empty
Error
Success
Validation
Permission denied, if applicable
```

Use project-owned components where available:

```text
LoadingPanel
EmptyState
ErrorPanel
AppAlert
AppButton
AppDialog
AppDataGrid
```

---

## Testing

Behavior changes should normally include tests.

Use:

- xUnit
- bUnit
- FluentAssertions
- NSubstitute or Moq
- Bogus when helpful
- Playwright only for critical end-to-end flows

Test naming:

```text
MethodName_WhenCondition_ShouldExpectedResult
```

Test behavior, not implementation details.

For wrapper components, test the project-owned wrapper contract, not Radzen internals.

---

## Accessibility

Ensure:

- Semantic HTML
- Keyboard-accessible interactions
- Visible focus states
- Labels for fields
- Accessible names for icon-only controls
- Dialog focus behavior, if dialogs are used
- Loading/error messages accessible to users
- Color is not the only state indicator

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
- [feature added]
- [files created/updated]
- [services/components/models/tests added]

Validation:
- [commands run or recommended]

Notes:
- [assumptions]
- [risks]
- [follow-up work]
```
