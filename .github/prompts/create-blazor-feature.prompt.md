---
description: Create a complete Blazor WebAssembly feature with pages, components, services, models, tests, and documentation updates where needed.
---

# Create Blazor feature

Create a complete feature based on the user's request.

This prompt is for multi-file work. For a single component, use `create-blazor-component.prompt.md`. For a single page, use `create-blazor-page.prompt.md`.

Follow `.github/copilot-instructions.md` and the auto-applying `.github/instructions/*.instructions.md` files for `.razor`/`.razor.cs`/`.css` (`blazor`, `html`, `css`, `accessibility`, `testing`) — do not restate their rules here. Review `docs/*.md` before non-trivial changes.

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

## Cross-cutting rules

Apply the rules already defined in the repository instructions instead of restating them here:

- Component/page structure, typed services, state, project-owned wrappers, Radzen usage — `.github/instructions/blazor.instructions.md`
- Semantic HTML and forms — `.github/instructions/html.instructions.md`
- Accessibility (keyboard, focus, ARIA, labels) — `.github/instructions/accessibility.instructions.md`
- Component-scoped CSS and design tokens — `.github/instructions/css.instructions.md`
- Test stack and naming (`MethodName_WhenCondition_ShouldExpectedResult`) — `.github/instructions/testing.instructions.md`

Include relevant UI states (loading/empty/error/success/validation) using project-owned components (`LoadingPanel`, `EmptyState`, `ErrorPanel`, etc.) where available.

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

