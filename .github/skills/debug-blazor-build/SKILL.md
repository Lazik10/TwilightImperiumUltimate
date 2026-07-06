---
name: debug-blazor-build
description: Use this skill when fixing C# / Blazor WebAssembly build errors, Razor compilation errors, test failures, dependency injection errors, namespace issues, component parameter errors, or Radzen-related build problems.
---

# Debug Blazor build

Use this skill when the solution fails to build, tests fail, Razor compilation fails, or a Blazor component has compile-time or runtime errors.

This skill focuses on **small, safe fixes**. Do not rewrite large areas of code unless the root cause requires it.

---

## Repository context

Follow `.github/copilot-instructions.md` and the relevant `.github/instructions/*.instructions.md` files (`blazor`, `csharp`, `html`, `css`, `accessibility`, `testing` — they auto-apply by file type). Use `docs/build-and-test.md` as the source of truth for build, test, and validation commands.

---

## Debugging workflow

### 1. Read the exact error

Identify:

- Error code
- Error message
- Project
- File
- Line number
- Symbol/type/member involved
- Whether the error is from C#, Razor, tests, dependency injection, or runtime behavior

Do not guess before reading the full error.

---

### 2. Classify the problem

Common categories:

```text
C# compiler error
Razor compiler error
Component parameter mismatch
Partial class mismatch
Namespace mismatch
Missing using
Missing package/reference
Dependency injection registration issue
Nullable reference warning/error
Async/await misuse
EventCallback type mismatch
Generic type inference problem
Radzen component usage issue
CSS isolation/build issue
bUnit test failure
Playwright/E2E failure
Runtime exception
```

---

### 3. Inspect related code

Before editing:

1. Open the file named by the error.
2. Open the matching `.razor` or `.razor.cs` file if the error is component-related.
3. Search for similar working examples.
4. Check namespaces.
5. Check project references.
6. Check service registrations.
7. Check component parameter definitions and usage.
8. Check generated or renamed files only if needed.

---

## Blazor-specific checks

When a Blazor component fails to build, verify:

```text
[ ] ComponentName.razor and ComponentName.razor.cs names match.
[ ] The code-behind class is `partial`.
[ ] The namespace matches the project convention.
[ ] The component is in a namespace imported by `_Imports.razor` or referenced fully.
[ ] Parameters used in markup exist on the child component.
[ ] Parameter types match.
[ ] EventCallback and EventCallback<T> types match.
[ ] Generic component type parameters are specified when needed.
[ ] Injected services are registered.
[ ] Razor expressions are valid C#.
[ ] Attributes use Razor syntax correctly.
[ ] `@bind-Value` targets a settable property.
[ ] `@key` expressions refer to valid values.
```

---

## `.razor` and `.razor.cs` mismatch checks

For this repository, every component should have:

```text
ComponentName.razor
ComponentName.razor.cs
```

Code-behind should look like:

```csharp
namespace ProjectName.Client.Components;

public partial class ComponentName
{
}
```

Check:

- Class name equals component file name.
- Class is `partial`.
- Namespace is correct.
- File is included in the correct project.
- There is no duplicate component/class with the same name.
- The `.razor` file does not contain a conflicting `@inherits` directive unless the project uses that pattern.

---

## Component parameter checks

If the error says a component does not have a property or parameter:

1. Open the child component.
2. Check `[Parameter]` names.
3. Check casing.
4. Check generic type arguments.
5. Check whether the parameter belongs to the wrapper component or the underlying Radzen component.
6. If using a project-owned wrapper, do not pass Radzen-only parameters unless the wrapper exposes them.

Example issue:

```razor
<AppButton ButtonStyle="ButtonStyle.Primary" />
```

If `AppButton` exposes `Variant` instead of `ButtonStyle`, use the project-owned API:

```razor
<AppButton Variant="AppButtonVariant.Primary" />
```

Do not leak Radzen parameter names into wrapper usage unless the wrapper intentionally exposes them.

---

## EventCallback checks

For callback errors, verify:

- `EventCallback` vs `EventCallback<T>`
- Handler method parameter type
- Async handler returns `Task`
- Callback is awaited when invoked
- Lambda passes expected parameter

Good:

```csharp
[Parameter]
public EventCallback<CustomerDto> CustomerSelected { get; set; }

private async Task SelectCustomerAsync(CustomerDto customer)
{
    await CustomerSelected.InvokeAsync(customer);
}
```

Good usage:

```razor
<CustomerList CustomerSelected="HandleCustomerSelectedAsync" />
```

```csharp
private Task HandleCustomerSelectedAsync(CustomerDto customer)
{
    selectedCustomer = customer;
    return Task.CompletedTask;
}
```

---

## Dependency injection checks

If the error is a runtime exception about a missing service:

1. Identify the missing service type.
2. Search for the service implementation.
3. Check `Program.cs` or the project’s DI registration location.
4. Register the service with the lifetime used by similar services.
5. Prefer scoped services for Blazor WebAssembly client services unless the project uses another pattern.

Example:

```csharp
builder.Services.AddScoped<ICustomerService, CustomerService>();
```

Do not register services in random files. Follow the existing DI pattern.

---

## Namespace and using checks

For missing type errors:

- Check if the type exists.
- Check namespace.
- Check `_Imports.razor`.
- Check `using` statements.
- Check project references.
- Check whether the type belongs to Client, Shared, Server, or Tests.
- Do not make the Shared project depend on Client or Server.

Prefer adding the correct `using` or namespace import rather than moving files unnecessarily.

---

## Nullable reference checks

For nullable warnings or errors:

- Prefer fixing nullability correctly.
- Avoid using `!` unless the value is guaranteed by framework lifecycle or dependency injection.
- Use nullable types when values can be absent.
- Handle null states explicitly.
- Use empty collections instead of null collections.

Good:

```csharp
private IReadOnlyList<CustomerDto> customers = [];
private string? errorMessage;
```

For injected Blazor services, this is acceptable:

```csharp
[Inject]
private ICustomerService CustomerService { get; set; } = default!;
```

---

## Async checks

Avoid:

```csharp
.Result
.Wait()
async void
```

Prefer:

```csharp
private async Task LoadAsync()
{
    await CustomerService.GetCustomersAsync();
}
```

For UI operations:

- Show loading state.
- Handle errors.
- Prevent duplicate submissions when needed.
- Use cancellation tokens where the project pattern supports them.

---

## Radzen checks

If the error involves Radzen:

1. Verify the component name and namespace.
2. Verify the parameter names and types.
3. Check whether the project-owned wrapper should be used instead.
4. Check whether the usage is direct one-off Radzen or should be wrapped.
5. Do not fix by scattering Radzen-specific details across pages.
6. Do not test Radzen internals.

If a wrapper exists, prefer fixing usage to match the wrapper API.

---

## Test failure checks

For failing tests:

1. Read the failing test name.
2. Identify the behavior being tested.
3. Determine whether the test or implementation is wrong.
4. Prefer preserving intended behavior.
5. Update tests only when behavior intentionally changed.
6. Do not delete tests to make the build pass.
7. Do not weaken meaningful assertions without justification.

For bUnit failures:

- Check rendered markup.
- Check parameters.
- Check injected services.
- Check event callbacks.
- Check async rendering.
- Check wrapper component behavior.

---

## Fixing strategy

Apply the smallest safe fix.

Prefer:

```text
Fix namespace
Add missing using
Correct parameter name/type
Correct EventCallback type
Add missing DI registration
Fix partial class name
Fix null handling
Fix async usage
Update test expectation only when behavior intentionally changed
```

Avoid:

```text
Large unrelated refactors
Deleting tests
Removing validation
Removing accessibility behavior
Adding new packages without explanation
Changing public APIs without explaining impact
Moving files unnecessarily
Suppressing warnings without reason
```

---

## Validation

Use commands from `docs/build-and-test.md`.

Typical commands:

```bash
dotnet build
dotnet test
dotnet format --verify-no-changes
```

For focused debugging, recommend the smallest relevant command first, such as:

```bash
dotnet build src/ProjectName.Client/ProjectName.Client.csproj
dotnet test tests/ProjectName.ComponentTests/ProjectName.ComponentTests.csproj
```

Do not invent command results. If commands were not run, say they should be run.

---

## Final response format

Return:

```text
Root cause:
- [what caused the error]

Fix:
- [files changed]
- [what changed]

Validation:
- [commands run]
- [commands recommended if not run]

Notes:
- [assumptions]
- [follow-up work]
```

---

## Checklist

```text
[ ] Exact error was read.
[ ] Root cause was identified.
[ ] Similar existing code was inspected.
[ ] Smallest safe fix was applied.
[ ] No unrelated changes were introduced.
[ ] Component `.razor` / `.razor.cs` patterns are preserved.
[ ] Parameters and EventCallback types are correct.
[ ] DI registration is correct if needed.
[ ] Nullability is handled intentionally.
[ ] Tests were preserved or updated only for intentional behavior changes.
[ ] Validation command was run or recommended.
```
