---
description: Create a Blazor component following this repository's C# / Blazor WebAssembly conventions.
---

# Create Blazor component

Create a Blazor component for the requested UI or behavior.

Follow `.github/copilot-instructions.md` and the auto-applying `.github/instructions/*.instructions.md` files for `.razor`/`.razor.cs`/`.css` (`blazor`, `html`, `css`, `accessibility`, `testing`) — do not restate their rules here. Review `docs/*.md` before non-trivial changes.

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

## Cross-cutting rules

Apply the rules already defined in the repository instructions instead of restating them here:

- Strongly typed parameters, `EventCallback<T>` — `.github/instructions/blazor.instructions.md` (`## Parameters`, `## Event callbacks`)
- Semantic HTML, project-owned wrapper components, and Radzen usage — `.github/instructions/blazor.instructions.md` and `.github/instructions/html.instructions.md`
- Component-scoped CSS and design tokens — `.github/instructions/css.instructions.md`
- Accessibility (keyboard, focus, ARIA, labels) — `.github/instructions/accessibility.instructions.md`
- Typed services for data loading, loading/empty/error states — `.github/instructions/blazor.instructions.md`
- Test stack and naming (`MethodName_WhenCondition_ShouldExpectedResult`) — `.github/instructions/testing.instructions.md`

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
