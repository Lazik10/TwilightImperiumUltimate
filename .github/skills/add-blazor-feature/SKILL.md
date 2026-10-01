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

Follow `.github/copilot-instructions.md` and the relevant `.github/instructions/*.instructions.md` files (they auto-apply to Blazor/HTML/CSS/accessibility/testing files by file type). Reference `docs/*.md` for architecture and use `docs/build-and-test.md` for build/test/format/run/publish commands.

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

## Cross-cutting rules

Apply the rules already defined in the repository instructions instead of restating them here:

- Typed services over raw `HttpClient`, local vs. scoped state, project-owned wrapper components, Radzen usage — `.github/instructions/blazor.instructions.md`
- Semantic HTML, forms, buttons vs. links — `.github/instructions/html.instructions.md`
- Accessibility (keyboard, focus, ARIA, labels, contrast) — `.github/instructions/accessibility.instructions.md`
- Component-scoped CSS, design tokens, Radzen overrides — `.github/instructions/css.instructions.md`
- Test stack, naming (`MethodName_WhenCondition_ShouldExpectedResult`), and what to test — `.github/instructions/testing.instructions.md`
- Performance (`@key`, virtualization, avoiding chatty API calls) — `.github/instructions/blazor.instructions.md`
- Security (no secrets in the WASM client, server-side authorization) — `.github/copilot-instructions.md`
- Documentation updates — `.github/copilot-instructions.md`; relevant files: `docs/solution-overview.md`, `docs/project-structure.md`, `docs/architecture.md`, `docs/build-and-test.md`

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
