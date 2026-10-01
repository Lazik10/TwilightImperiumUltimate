---
name: prepare-production-pr
description: Use this skill before opening or merging a pull request in a C# / Blazor WebAssembly repository. It reviews the diff, tests, docs, accessibility, security, performance, validation, risks, and creates a production-ready PR summary.
---

# Prepare production PR

Use this skill before opening, reviewing, or merging a pull request.

The goal is to make sure the change is ready for a production-quality repository.

---

## Repository context

Follow `.github/copilot-instructions.md` and the relevant `.github/instructions/*.instructions.md` files (they auto-apply by file type). Use `docs/build-and-test.md` as the source of truth for validation commands, and `docs/*.md` for architecture/structure context.

---

## PR preparation workflow

### 1. Review the scope

Identify:

- What changed
- Why it changed
- User-facing behavior
- Technical behavior
- Affected projects
- Affected pages/components/services/models/tests/docs
- New dependencies
- Configuration changes
- Security-sensitive changes
- Accessibility-sensitive changes
- Performance-sensitive changes

---

### 2. Review changed files

Group changes by area:

```text
Pages
Components
Services
Models/DTOs
Tests
CSS
Docs
Configuration
Build/CI
```

Check whether files are in the correct location according to:

```text
docs/project-structure.md
```

---

## Blazor review

Check:

```text
[ ] Components use `.razor` and `.razor.cs`.
[ ] Component-specific CSS uses `.razor.css`.
[ ] Markup is readable.
[ ] Logic is in code-behind.
[ ] Parameters are strongly typed.
[ ] EventCallback/EventCallback<T> is used for callbacks.
[ ] Services are injected.
[ ] Components use typed services instead of raw HttpClient.
[ ] Loading, empty, error, validation, and success states are handled where relevant.
[ ] Repeated/styled Radzen usage is wrapped or intentionally direct.
[ ] Lists use @key where identity matters.
[ ] JavaScript interop is avoided or justified.
```

---

## Testing review

Check:

```text
[ ] Behavior changes include tests where practical.
[ ] Tests use xUnit or existing framework.
[ ] bUnit is used for Blazor component tests.
[ ] Test names use MethodName_WhenCondition_ShouldExpectedResult.
[ ] Tests use Arrange / Act / Assert.
[ ] Tests focus on behavior.
[ ] Reusable wrapper components are tested.
[ ] Radzen internals are not tested directly.
[ ] Bug fixes include regression tests where practical.
[ ] Tests are deterministic.
```

If tests are missing, identify the most valuable tests to add.

---

## Accessibility review

Check:

```text
[ ] Semantic HTML is used.
[ ] Buttons and links are used correctly.
[ ] Forms have labels.
[ ] Validation messages are clear.
[ ] Keyboard navigation is considered.
[ ] Focus visibility is preserved.
[ ] Dialogs manage focus if applicable.
[ ] Icon-only controls have accessible names.
[ ] Color is not the only state indicator.
[ ] Loading/empty/error states are accessible.
[ ] CSS does not remove focus outlines or break zoom/text wrapping.
```

Recommend manual checks for non-trivial UI changes:

```text
Keyboard-only navigation
Lighthouse
axe DevTools
Accessibility Insights
Screen reader smoke test where practical
```

---

## Security review

Check:

```text
[ ] No secrets are committed.
[ ] No secrets are placed in the Blazor WebAssembly client.
[ ] Client/server trust boundary is respected.
[ ] Authorization is not only client-side.
[ ] User input is validated at the correct boundary.
[ ] Raw HTML rendering is avoided or justified.
[ ] MarkupString usage is safe.
[ ] Technical exceptions are not shown to users.
[ ] Browser storage does not contain sensitive data.
[ ] New dependencies are justified.
```

---

## Performance review

Check:

```text
[ ] Large lists use paging, filtering, or virtualization where appropriate.
[ ] @key is used where identity matters.
[ ] Expensive expressions are not repeated in markup.
[ ] Unnecessary StateHasChanged calls are avoided.
[ ] API calls are not chatty.
[ ] Data is not over-fetched unnecessarily.
[ ] CSS selectors are not overly broad.
[ ] Images/assets do not cause obvious layout issues.
[ ] JavaScript interop is avoided or justified.
```

---

## Documentation review

Check whether these need updates:

```text
README.md
docs/solution-overview.md
docs/project-structure.md
docs/architecture.md
docs/build-and-test.md
.github/copilot-instructions.md
```

Update docs when changes affect:

- Solution purpose
- Project structure
- Architecture
- Build commands
- Test commands
- Run commands
- Deployment/publish commands
- API communication
- State management
- Authentication/authorization
- Dependencies
- Developer workflow

---

## Validation

Use commands from `docs/build-and-test.md`.

Typical validation:

```bash
dotnet restore
dotnet build
dotnet test
dotnet format --verify-no-changes
```

For UI changes, recommend manual browser validation:

```text
Page loads without console errors.
Loading state appears.
Empty state appears.
Error state appears.
Form validation works.
Keyboard navigation works.
Focus is visible.
Responsive layout works.
Expected API calls are made.
```

Do not claim validation was run unless it was actually run.

---

## Risk review

Identify:

- Breaking API/model changes
- Public component parameter changes
- Route changes
- Dependency changes
- Configuration changes
- Security-sensitive changes
- Accessibility-sensitive changes
- Performance-sensitive changes
- Missing tests
- Incomplete docs
- Migration/deployment concerns

---

## PR summary format

Use this format:

```markdown
## Summary

- 
- 

## What changed

- 

## Tests

- 

## Validation

- [ ] `dotnet restore`
- [ ] `dotnet build`
- [ ] `dotnet test`
- [ ] `dotnet format --verify-no-changes`
- [ ] Manual UI check, if applicable

## Accessibility

- 

## Security

- 

## Documentation

- 

## Risks

- 

## Follow-up

- 
```

If a section has no meaningful content, write `None`.

---

## Review outcome format

When asked to review readiness, return:

```text
Blocking issues:
- [issue or "None"]

Non-blocking suggestions:
- [suggestion or "None"]

Missing tests:
- [test or "None"]

Documentation updates:
- [doc or "None"]

Validation needed:
- [command/check]

Overall:
- [Ready / Ready after minor updates / Not ready]
```

---

## Checklist

```text
[ ] Scope is clear.
[ ] Changed files are grouped by area.
[ ] Blazor conventions are followed.
[ ] Tests are present or missing tests are identified.
[ ] Accessibility is reviewed.
[ ] Security is reviewed.
[ ] Performance is reviewed.
[ ] Documentation impact is reviewed.
[ ] Validation commands are listed accurately.
[ ] Risks and follow-up work are identified.
[ ] PR summary is clear and honest.
```
