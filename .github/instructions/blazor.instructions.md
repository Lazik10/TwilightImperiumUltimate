---
applyTo: "**/*.razor, **/*.razor.cs"
---

# Blazor instructions

These instructions apply to Blazor component markup files and Blazor component code-behind files in this repository.

This repository uses **C# / Blazor WebAssembly** and may use **Radzen components**. Prefer maintainable Blazor components with clear markup, code-behind files, typed services, accessible UI behavior, and project-owned wrapper components for repeated or styled UI patterns.

Detailed HTML, CSS, and accessibility rules are covered separately:

```text
.github/instructions/html.instructions.md
.github/instructions/css.instructions.md
.github/instructions/accessibility.instructions.md
```

This file focuses on Blazor component structure, lifecycle, parameters, events, state, service usage, Radzen usage, rendering performance, JavaScript interop, and maintainability.

---

## Core principles

- Every Blazor component should use a `.razor` file and a matching `.razor.cs` code-behind file.
- Keep `.razor` files focused on markup and component composition.
- Keep component state, parameters, injected services, event handlers, lifecycle methods, and helper methods in `.razor.cs`.
- Keep reusable business logic out of components.
- Prefer typed client services for API calls.
- Prefer local component state by default.
- Use scoped services for shared client-side state.
- Prefer project-owned components for repeated or styled UI.
- Wrap repeated or important Radzen usage behind project-owned components.
- Avoid JavaScript interop unless Blazor or .NET cannot reasonably solve the problem.
- Keep components readable, testable, accessible, and easy to replace.

---

## Required component file pattern

All components should use this pattern:

```text
Components/
  ExampleComponent.razor
  ExampleComponent.razor.cs
  ExampleComponent.razor.css
```

Rules:

- The `.razor` file contains markup only, plus directives needed by the markup.
- The `.razor.cs` file contains the partial class.
- The `.razor.css` file contains component-scoped styles when styles are needed.
- Do not use large `@code` blocks in `.razor` files.
- Do not place business logic in `.razor` markup.
- Do not place component-specific styles in global CSS unless there is a clear reason.

Minimal example:

```razor
@inherits ExampleComponentBase

<section class="example-component">
    <h2>@Title</h2>

    <AppButton OnClick="HandleClickAsync">
        Save
    </AppButton>
</section>
```

Preferred code-behind shape:

```csharp
using Microsoft.AspNetCore.Components;

namespace MyProject.Client.Components;

public partial class ExampleComponent
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public EventCallback OnSaved { get; set; }

    private async Task HandleClickAsync()
    {
        await OnSaved.InvokeAsync();
    }
}
```

If the existing project uses a base class naming pattern such as `ExampleComponentBase`, follow the existing project pattern. Otherwise, use a simple `public partial class ExampleComponent`.

---

## Component responsibilities

A Blazor component may handle:

- Rendering UI.
- Receiving input through `[Parameter]`.
- Raising events through `EventCallback` or `EventCallback<T>`.
- Calling injected client services.
- Managing local UI state.
- Showing loading, empty, error, validation, and success states.
- Coordinating child components.
- Formatting UI-specific display values.

A Blazor component should not handle:

- Complex business rules.
- Direct database access.
- Server-only logic.
- Raw SQL.
- Secret handling.
- Large object mapping logic.
- Scattered raw `HttpClient` calls.
- Reusable domain logic.
- Complex global state management.
- Direct Radzen implementation details across many unrelated pages.

Move reusable logic into services, helpers, or shared models as appropriate.

---

## Project-owned component wrappers

This project prefers project-owned components for repeated, styled, or behavior-rich UI.

Use wrappers for:

- Buttons.
- Links styled as buttons.
- Text inputs.
- Form fields.
- Selects and dropdowns.
- Checkboxes and radio groups.
- Dialogs and modals.
- Alerts and toasts.
- Cards and panels.
- Tabs.
- Menus.
- Data grids and tables.
- Loading states.
- Empty states.
- Error states.
- Repeated Radzen components.

Recommended naming examples:

```text
AppButton
AppLinkButton
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

Reason:

- Styling is centralized.
- Accessibility behavior is centralized.
- Third-party UI components can be replaced more easily.
- Pages stay cleaner.
- Repeated patterns are easier to test.

Do not create unnecessary wrappers for one-off static markup. Prefer wrappers when the UI is repeated, styled, behavior-rich, accessibility-sensitive, or depends on Radzen.

---

## Radzen usage

Direct Radzen usage is acceptable for one-off, simple UI.

Prefer project-owned wrappers when Radzen usage is:

- Repeated.
- Styled.
- Accessibility-sensitive.
- Used in multiple pages.
- Part of the design system.
- Likely to change later.
- Complex, such as grids, dialogs, dropdowns, menus, and forms.

Good:

```razor
<AppButton ButtonStyle="AppButtonStyle.Primary"
           OnClick="SaveAsync">
    Save
</AppButton>
```

Acceptable for simple one-off UI:

```razor
<RadzenIcon Icon="info" />
```

Avoid scattering repeated Radzen implementation details:

```razor
<RadzenButton Text="Save"
              ButtonStyle="ButtonStyle.Primary"
              Click="SaveAsync" />
```

If repeated, wrap it:

```razor
<AppButton OnClick="SaveAsync">Save</AppButton>
```

Wrapper rules:

- Keep wrappers thin by default.
- Expose important parameters through project-owned names.
- Avoid leaking Radzen-specific naming into pages unless necessary.
- Document intentionally unsupported Radzen features.
- Do not hide accessibility-relevant behavior.
- Keep wrapper APIs stable and project-oriented.

---

## Parameters

Use `[Parameter]` for component inputs.

Rules:

- Use clear parameter names.
- Prefer strongly typed parameters.
- Avoid overly generic `object` parameters.
- Prefer immutable or read-only data where practical.
- Do not mutate parameters directly.
- Copy parameter values into local state only when local editing or derived state is required.
- Validate required parameters where appropriate.
- Use `[EditorRequired]` for required component parameters when useful.
- Avoid parameter sets that allow invalid combinations.

Good:

```csharp
[Parameter, EditorRequired]
public IReadOnlyList<CustomerDto> Customers { get; set; } = [];

[Parameter]
public bool IsLoading { get; set; }

[Parameter]
public EventCallback<CustomerDto> CustomerSelected { get; set; }
```

Avoid:

```csharp
[Parameter]
public object? Data { get; set; }
```

---

## Event callbacks

Use `EventCallback` and `EventCallback<T>` for child-to-parent communication.

Rules:

- Prefer `EventCallback<T>` when passing data.
- Prefer `EventCallback` for simple notifications.
- Do not use `Action` or `Func<Task>` for component callback parameters unless the existing project has a specific pattern requiring it.
- Await callback invocation.
- Name callbacks clearly.

Good:

```csharp
[Parameter]
public EventCallback<OrderDto> OrderSelected { get; set; }

private async Task SelectOrderAsync(OrderDto order)
{
    await OrderSelected.InvokeAsync(order);
}
```

Avoid:

```csharp
[Parameter]
public Action<OrderDto>? OnOrderSelected { get; set; }
```

---

## Dependency injection

Use dependency injection for services.

Rules:

- Inject typed client services into components.
- Keep service interfaces focused.
- Prefer constructor injection in code-behind if the project uses it.
- Use `[Inject]` properties if that is the established component pattern.
- Do not manually instantiate services inside components.
- Do not use static service locators.
- Do not inject many unrelated services into one component.

Good:

```csharp
[Inject]
private ICustomerService CustomerService { get; set; } = default!;
```

If a component needs too many services, consider whether it is doing too much.

---

## API calls

Components should not scatter raw `HttpClient` calls.

Preferred flow:

```text
Blazor component
    ↓
Typed client service
    ↓
HttpClient
    ↓
API endpoint or external service
```

Good:

```csharp
private IReadOnlyList<CustomerDto> customers = [];
private bool isLoading;
private string? errorMessage;

protected override async Task OnInitializedAsync()
{
    isLoading = true;

    try
    {
        customers = await CustomerService.GetCustomersAsync();
    }
    catch
    {
        errorMessage = "Customers could not be loaded.";
    }
    finally
    {
        isLoading = false;
    }
}
```

Avoid:

```csharp
var customers = await HttpClient.GetFromJsonAsync<List<CustomerDto>>("api/customers");
```

inside many different components.

Raw `HttpClient` may be acceptable inside a typed service implementation.

---

## State management

Use local component state by default.

Use local component state for:

- Loading flags.
- Current selection.
- Form editing state.
- Expanded/collapsed state.
- Dialog open/closed state.
- Validation display state.
- Component-specific UI state.

Use scoped services for:

- State shared across multiple components.
- Current user/session UI state.
- Cached lookup data.
- Cross-page client-side state.
- State that must survive component disposal during navigation.

Avoid global state for simple local UI behavior.

Rules:

- Keep state as close as possible to where it is used.
- Do not introduce centralized state management without a clear reason.
- Keep state changes explicit.
- Avoid hidden side effects in property getters.
- Avoid async work in property getters.
- Avoid unnecessary cascading values for local state.
- Document shared state service behavior.

---

## Lifecycle methods

Use Blazor lifecycle methods intentionally.

Common guidance:

- Use `OnInitialized` for synchronous one-time initialization.
- Use `OnInitializedAsync` for asynchronous one-time initialization.
- Use `OnParametersSet` when component state depends on parameters.
- Use `OnParametersSetAsync` when asynchronous work depends on parameters.
- Use `OnAfterRenderAsync` only when DOM-dependent work or JS interop is required.
- Avoid doing expensive work on every render.
- Avoid starting duplicate async operations.
- Dispose subscriptions and timers.

Rules:

- Do not call async methods from constructors.
- Do not call async methods from property getters.
- Do not use `async void` except for true event-handler cases where no alternative exists.
- Prefer `async Task` methods.
- Handle cancellation where long-running or repeatable operations are possible.
- Avoid infinite render loops in `OnAfterRenderAsync`.

Good:

```csharp
protected override async Task OnInitializedAsync()
{
    await LoadCustomersAsync();
}
```

Use `OnAfterRenderAsync` carefully:

```csharp
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (!firstRender)
    {
        return;
    }

    await JsInteropService.InitializeAsync();
}
```

Only use JS interop when necessary.

---

## Async patterns

Rules:

- Prefer `async Task` over `async void`.
- Await asynchronous operations.
- Use `try`/`catch` around user-visible operations.
- Show loading state during long-running operations.
- Disable or guard duplicate submissions when appropriate.
- Use cancellation tokens when operations can be canceled or superseded.
- Do not block async code with `.Result` or `.Wait()`.
- Do not ignore returned tasks.
- Keep error messages user-friendly.

Good:

```csharp
private bool isSaving;
private string? saveErrorMessage;

private async Task SaveAsync()
{
    if (isSaving)
    {
        return;
    }

    isSaving = true;
    saveErrorMessage = null;

    try
    {
        await CustomerService.SaveAsync(Model);
    }
    catch
    {
        saveErrorMessage = "The customer could not be saved. Try again.";
    }
    finally
    {
        isSaving = false;
    }
}
```

---

## Loading, empty, error, and success states

Data-driven components should consider:

- Loading state.
- Empty state.
- Error state.
- Success state.
- Validation state.
- Permission denied state, if applicable.

Good markup pattern:

```razor
@if (isLoading)
{
    <LoadingPanel Message="Loading customers..." />
}
else if (errorMessage is not null)
{
    <ErrorPanel Message="@errorMessage" OnRetry="LoadCustomersAsync" />
}
else if (customers.Count == 0)
{
    <EmptyState Message="No customers found." />
}
else
{
    <CustomerList Customers="customers" />
}
```

Avoid blank screens during async operations.

---

## Forms

Use Blazor form components and validation patterns.

Rules:

- Prefer `EditForm` for forms bound to models.
- Use validation components consistently.
- Keep validation messages close to fields.
- Use project-owned form field wrappers for repeated field patterns.
- Do not rely only on placeholders as labels.
- Do not put large validation logic in markup.
- Keep submit logic in `.razor.cs`.
- Prevent duplicate submission when appropriate.
- Preserve input state when validation fails.

Good:

```razor
<EditForm Model="Model" OnValidSubmit="SaveAsync">
    <DataAnnotationsValidator />

    <AppTextInput Label="Customer name"
                  @bind-Value="Model.Name"
                  ValidationFor="() => Model.Name" />

    <AppButton Type="submit" IsLoading="isSaving">
        Save
    </AppButton>
</EditForm>
```

---

## Rendering performance

Avoid unnecessary rendering and expensive markup.

Rules:

- Use `@key` when rendering lists where identity matters.
- Avoid expensive calculations directly in markup.
- Avoid allocating new objects repeatedly in markup.
- Avoid complex LINQ expressions inside markup loops.
- Avoid unnecessary cascading values.
- Avoid unnecessary attribute splatting in components rendered many times.
- Use `Virtualize<TItem>` for large lists when appropriate.
- Use pagination or server-side filtering for large data sets.
- Keep component parameters simple and stable.
- Avoid excessive child component nesting for very large repeated lists.
- Do not call `StateHasChanged` unnecessarily.
- Consider `ShouldRender` only when there is a clear performance need.

Good:

```razor
@foreach (var customer in customers)
{
    <CustomerRow @key="customer.Id"
                 Customer="customer"
                 OnSelected="SelectCustomerAsync" />
}
```

Avoid:

```razor
@foreach (var customer in customers.OrderBy(x => x.Name).Where(x => x.IsActive))
{
    ...
}
```

Prefer preparing data in code-behind:

```csharp
private IReadOnlyList<CustomerDto> VisibleCustomers =>
    customers
        .Where(customer => customer.IsActive)
        .OrderBy(customer => customer.Name)
        .ToList();
```

For large collections, avoid recreating lists repeatedly. Cache or compute intentionally.

---

## Conditional rendering

Keep conditional rendering readable.

Good:

```razor
@if (CanEdit)
{
    <AppButton OnClick="EditAsync">Edit</AppButton>
}
```

Avoid deeply nested conditions in markup.

If rendering becomes complex:

- Extract a child component.
- Move computed properties to `.razor.cs`.
- Use clear helper methods.
- Simplify the UI state model.

---

## Cascading values

Use cascading values carefully.

Appropriate uses:

- Authentication/user context.
- Theme context.
- Layout-level state.
- Form or validation context.
- Shared context for a component subtree.

Avoid cascading values for:

- Simple parent-child data.
- Local component state.
- Values that change frequently and cause broad re-rendering.
- Hidden dependencies that make components hard to reuse.

Prefer explicit parameters where practical.

---

## JavaScript interop

Avoid JavaScript interop unless necessary.

Use JS interop for:

- Browser APIs not exposed by Blazor.
- DOM measurements that cannot be done otherwise.
- Third-party JavaScript libraries that are intentionally adopted.
- Focus management only when Blazor/native behavior is insufficient.
- File, clipboard, storage, or media APIs when needed.

Rules:

- Keep JS interop isolated in services.
- Do not scatter `IJSRuntime` calls across many components.
- Prefer typed wrapper services.
- Dispose JavaScript object references when required.
- Use `OnAfterRenderAsync` for DOM-dependent JS interop.
- Do not use JS to do what Blazor can do cleanly.
- Do not introduce JS dependencies without explaining why.

Preferred flow:

```text
Component
    ↓
Typed JS interop service
    ↓
IJSRuntime
    ↓
JavaScript module
```

---

## Security boundaries

Blazor WebAssembly client code is visible to users.

Rules:

- Do not store secrets in the client.
- Do not put private keys, connection strings, or privileged configuration in WebAssembly.
- Do not trust client-side validation alone for protected operations.
- Do not rely only on client-side authorization for server resources.
- Avoid rendering raw HTML.
- Use `MarkupString` only for trusted or sanitized content.
- Treat all API responses as untrusted until validated for display.
- Keep authorization enforced on the server when a server/API exists.

Avoid:

```razor
@((MarkupString)UserProvidedHtml)
```

unless the content is trusted or sanitized and the reason is documented.

---

## Accessibility expectations

Blazor components must follow the accessibility rules in:

```text
.github/instructions/accessibility.instructions.md
```

Blazor-specific reminders:

- Use semantic elements.
- Use labels for form fields.
- Use buttons for actions.
- Use links for navigation.
- Ensure focus is visible.
- Ensure keyboard interaction works.
- Ensure dialogs manage focus.
- Ensure icon-only buttons have accessible names.
- Include accessible loading and error states.
- Avoid hover-only interactions.
- Do not remove focus outlines without replacement.

---

## Component naming

Use names that describe purpose, not implementation.

Good:

```text
CustomerCard
CustomerList
OrderSummary
AppButton
AppDialog
LoadingPanel
ErrorPanel
```

Avoid:

```text
RadzenCustomerButton
BlueBox
CommonComponent
Component1
```

Do not include third-party library names in project-owned wrapper names unless there is a specific reason.

---

## File placement

Follow `docs/project-structure.md`.

General guidance:

```text
Pages/
  Customers.razor
  Customers.razor.cs
  Customers.razor.css

Components/
  CustomerCard.razor
  CustomerCard.razor.cs
  CustomerCard.razor.css

Services/
  ICustomerService.cs
  CustomerService.cs
```

Do not create new folders or conventions unless they match the existing repository structure or are documented.

---

## Testing guidance

When changing Blazor behavior, consider tests.

Use component tests for:

- Rendering states.
- Parameters.
- Event callbacks.
- Form behavior.
- Validation behavior.
- Loading, empty, error, and success states.
- Conditional rendering.
- Wrapper component behavior.
- Important Radzen wrapper behavior.

Use service tests for:

- API client logic.
- State services.
- Business logic.
- Mapping and validation logic.

Do not skip tests for important behavior changes if the project already has a test pattern.

---

## Documentation guidance

Update documentation when changes affect:

- Component patterns.
- Wrapper component conventions.
- Radzen usage strategy.
- Service/API patterns.
- State management.
- Build or test commands.
- Accessibility behavior.
- Project structure.

Relevant docs:

```text
docs/solution-overview.md
docs/project-structure.md
docs/architecture.md
docs/build-and-test.md
```

---

## Common anti-patterns

Avoid:

```razor
@code {
    // Large component logic here
}
```

Avoid:

```razor
<div @onclick="SaveAsync">Save</div>
```

Avoid:

```razor
<RadzenButton Text="Save" Click="SaveAsync" />
```

when the same pattern is repeated across the app.

Avoid:

```csharp
private string CustomerName => CustomerService.GetCustomerNameAsync().Result;
```

Avoid:

```csharp
private async void Save()
{
    await CustomerService.SaveAsync(Model);
}
```

Avoid:

```razor
@((MarkupString)UserProvidedHtml)
```

unless explicitly trusted or sanitized.

Prefer:

```razor
<AppButton OnClick="SaveAsync">Save</AppButton>
```

```csharp
private async Task SaveAsync()
{
    await CustomerService.SaveAsync(Model);
}
```

---

## Copilot behavior

When generating or editing Blazor components:

1. Create both `.razor` and `.razor.cs` files for every component.
2. Add `.razor.css` when component-specific styling is needed.
3. Keep markup in `.razor`.
4. Keep logic in `.razor.cs`.
5. Prefer existing project-owned components.
6. Use typed client services instead of raw `HttpClient` in components.
7. Use local component state by default.
8. Use scoped services for shared state.
9. Use `EventCallback<T>` for child-to-parent communication.
10. Use `[Parameter]` and `[EditorRequired]` intentionally.
11. Include loading, empty, error, validation, and success states where relevant.
12. Use `@key` for rendered lists where identity matters.
13. Avoid unnecessary JavaScript interop.
14. Avoid leaking Radzen details across pages.
15. Recommend tests for meaningful behavior changes.
16. Summarize assumptions, changed files, and validation steps.

---

## Review checklist

Use this checklist for non-trivial Blazor changes:

```text
[ ] Component has `.razor` and `.razor.cs` files.
[ ] Component-specific CSS is in `.razor.css` where needed.
[ ] Markup is readable and not overloaded with logic.
[ ] Code-behind contains component logic.
[ ] Parameters are strongly typed.
[ ] Required parameters are marked or validated where appropriate.
[ ] EventCallback or EventCallback<T> is used for component callbacks.
[ ] Services are injected rather than manually instantiated.
[ ] Components call typed services instead of raw HttpClient.
[ ] Local state is used for local UI state.
[ ] Shared state uses scoped services where appropriate.
[ ] Loading, empty, error, and success states are handled.
[ ] Forms have labels and validation messages.
[ ] Repeated/styled Radzen usage is wrapped or intentionally left direct.
[ ] Lists use @key where identity matters.
[ ] Expensive work is not done directly in markup.
[ ] JavaScript interop is avoided or justified.
[ ] No secrets or server-only logic were added to the client.
[ ] Tests were added or updated for behavior changes where appropriate.
```
