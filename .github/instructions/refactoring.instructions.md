---
applyTo: "**"
description: "Repository-wide refactoring workflow for C# and Blazor WebAssembly"
---

# Refactoring instructions

Use this file to coordinate refactoring work. Do not repeat detailed language, UI, API, accessibility, testing, or review rules here; apply the specialized instruction files listed below.

## Referenced instructions

Apply the relevant files based on the code being changed:

- [C# instructions](./csharp.instructions.md) for C# code, naming, nullability, async code, security, logging, and data access.
- [Blazor instructions](./blazor.instructions.md) for components, code-behind, parameters, lifecycle, state, services, rendering, Radzen, and JavaScript interop.
- [HTML instructions](./html.instructions.md) for Razor markup, semantic HTML, forms, safe rendering, and wrapper components.
- [CSS instructions](./css.instructions.md) for CSS isolation, design tokens, responsive styling, and Radzen overrides.
- [Accessibility instructions](./accessibility.instructions.md) for WCAG 2.2 AA, keyboard behavior, focus, forms, dialogs, and accessible UI states.
- [REST API instructions](./rest-api.instructions.md) when changing the API project, endpoints, routes, contracts, or OpenAPI documentation.
- [Testing instructions](./testing.instructions.md) for unit, component, integration, and end-to-end tests.
- [Code review instructions](./code-review.instructions.md) for final review priorities and reporting findings.

When instructions overlap, use the most specific file for the code being changed.

## Refactoring goals

Refactor to improve one or more of the following:

- Readability and maintainability
- Separation of concerns
- Testability
- Correctness and null safety
- Async and cancellation behavior
- Rendering or runtime performance
- Accessibility
- Removal of confirmed duplication or dead code

Do not refactor solely to introduce a preferred pattern when the existing code is already clear and correct.

## Required workflow

1. Read the affected code and nearby files before editing.
2. Identify the behavior and contracts that must remain unchanged.
3. Check the relevant specialized instruction files.
4. Choose the smallest coherent refactoring scope.
5. Add or confirm tests around risky behavior when practical.
6. Make one logical change at a time.
7. Build and test the affected projects.
8. Review the final diff for behavior changes and unrelated edits.
9. Report what changed and what validation actually ran.

Do not claim that a build, test, format, or publish command passed unless it was executed successfully.

## Preserve behavior and contracts

Unless the task explicitly requests a behavior change, preserve:

- Public APIs and method signatures
- Component parameters and callback contracts
- Routes, query-string names, and navigation behavior
- API endpoints, status codes, and serialized contract names
- Authentication and authorization behavior
- Storage keys and persisted data formats
- DOM IDs, CSS classes, and selectors used by tests or JavaScript
- Loading, empty, error, validation, and success states
- Keyboard, focus, and accessibility behavior

Stop and report the risk before making a breaking change, destructive migration, authentication change, or broad architectural rewrite outside the requested scope.

## Refactoring rules

- Prefer small, reviewable changes over rewrites.
- Follow existing repository architecture and nearby conventions.
- Keep business logic out of Blazor components.
- Keep controllers and API endpoints thin.
- Prefer typed services at external boundaries.
- Remove duplication only when the shared abstraction has a clear responsibility.
- Remove dead code only after confirming it is unused.
- Remove unused usings that are no longer used
- If possible move usings that are repeatedly used into the global usings file. (GlobalUsings.cs)
- Avoid speculative abstractions and generic `Helper`, `Manager`, or `Utils` classes.
- Do not introduce a new dependency when existing platform or repository features are sufficient.
- Do not weaken nullable reference type safety.
- Do not block asynchronous code with `.Result`, `.Wait()`, or equivalent patterns.
- Dispose subscriptions, timers, cancellation sources, streams, and JavaScript references.
- Do not edit generated output.
- Do not mix unrelated formatting, renaming, package updates, or cleanup into the refactor.
- Replace `FlexRowContainer` and `FlexColumnCenteredContainer` usages with `ResponsiveContainer` directly when able (both are thin pass-through wrappers around it). If the resulting `ResponsiveContainer` adds no layout behavior beyond what its single child or that child's own CSS already provides (for example a block-level element that is already `width: 100%`), remove the wrapper entirely instead of keeping a no-op container -- see the div-simplification rule below.
- `VerticalSpace`, `HorizontalSpace`, `ResponsiveVerticalSpacer`, and `ResponsiveHorizontalSpacer` are deprecated. Do not use them in new or refactored code. If any are found during a refactor, remove them and instead apply the equivalent spacing (`.mt-*`/`.mb-*`/`.ml-*`/`.mr-*` margin utility classes from `wwwroot/css/app.css`) via the `CssClass` parameter on the adjacent/relevant component or element, so spacing does not require an extra empty spacer element in the DOM.

## Blazor-specific checks

When changing a component, confirm that:

- Markup remains in `.razor` and component logic remains in `.razor.cs` according to project conventions.
- Parameter values are not mutated unexpectedly.
- `EventCallback` or `EventCallback<T>` is used for component events.
- Lifecycle methods do not trigger duplicate loading or render loops.
- Loading, empty, error, and success states remain explicit.
- Expensive work is not performed repeatedly in markup or render paths.
- `@key` is preserved or added where list identity matters.
- Focus and keyboard behavior remain correct after conditional rendering.
- Repeated Radzen behavior is routed through project-owned wrappers when appropriate.
- JavaScript interop remains isolated and disposable.
- Unnecessary wrapper `<div>`/container elements are merged or removed when they add no semantic meaning and no layout behavior beyond what a sibling/child already provides (confirm this from the actual CSS -- for example a flex wrapper around a single already-`width:100%` block child is usually a no-op and safe to remove).
- After merging or removing markup, every selector in the component's `.razor.css` still matches an existing element or class. Pay special attention to classes passed via a `CssClass` parameter into a child component (including `ResponsiveContainer`, `FlexRowContainer`, etc.) -- those render with the CHILD's own scope, so the selector needs `::deep` even though it looks like a plain scoped class in the parent's stylesheet.

Use the Blazor, HTML, CSS, accessibility, and testing instruction files for the detailed rules.

## Validation

Use repository-provided scripts and documentation first. Otherwise, run the relevant commands:

```bash
dotnet restore
dotnet build
dotnet test
dotnet format --verify-no-changes
```

Run release publish validation when the refactor affects trimming, serialization, reflection, static assets, deployment, or Blazor WebAssembly startup:

```bash
dotnet publish <client-project> -c Release
```

Run only the commands relevant to the affected scope, but state clearly when a validation step was not run.

## Final review

Review the diff using the priorities in `code-review.instructions.md`.

At minimum, check for:

- Accidental behavior or contract changes
- Security or authorization regressions
- Missing error handling
- Missing disposal or cancellation
- New duplication or unnecessary abstraction
- Rendering or API performance regressions
- Missing tests for changed behavior
- Accessibility regressions
- Unrelated file changes

## Required completion report

Use this format:

```markdown
## Summary

Briefly describe the refactoring and its purpose.

## Files changed

- `path/to/file`: What changed

## Behavior preserved

List important contracts or behavior intentionally kept unchanged.

## Validation

- Build: Passed / Failed / Not run
- Tests: Passed / Failed / Not run
- Formatting: Passed / Failed / Not run
- Publish: Passed / Failed / Not run

## Risks or follow-up

List remaining assumptions, risks, or recommended follow-up work.
```
