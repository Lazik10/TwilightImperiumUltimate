---
name: add-component-tests
description: Use this skill when adding or improving xUnit and bUnit tests for Blazor components, pages, reusable wrapper components, Radzen wrappers, forms, event callbacks, and UI states.
---

# Add component tests

Use this skill when creating or improving tests for Blazor components, pages, and project-owned wrapper components.

This repository uses:

- xUnit
- bUnit
- FluentAssertions
- NSubstitute or Moq
- Bogus where useful
- Playwright for critical end-to-end flows

---

## Repository context

Follow:

```text
.github/copilot-instructions.md
.github/instructions/testing.instructions.md
.github/instructions/blazor.instructions.md
.github/instructions/html.instructions.md
.github/instructions/accessibility.instructions.md
```

Use:

```text
docs/build-and-test.md
docs/project-structure.md
docs/architecture.md
```

---

## Test naming

Always use:

```text
MethodName_WhenCondition_ShouldExpectedResult
```

Examples:

```text
AppButton_WhenClicked_ShouldInvokeOnClickCallback
CustomerForm_WhenNameIsMissing_ShouldShowValidationMessage
CustomerList_WhenCustomersAreEmpty_ShouldShowEmptyState
OrdersPage_WhenServiceFails_ShouldShowErrorPanel
```

---

## Testing workflow

### 1. Inspect the component

Identify:

- Component purpose
- Parameters
- Event callbacks
- Injected services
- Child content
- Conditional rendering
- Loading, empty, error, validation, and success states
- Radzen usage
- Project-owned wrapper usage
- Accessibility-sensitive behavior
- Existing tests

---

### 2. Choose test type

Use:

```text
xUnit unit test      -> pure C# logic, services, state services
bUnit component test -> Blazor components, pages, wrappers
Playwright E2E      -> critical browser workflows
```

Do not use Playwright for simple component behavior that bUnit can test.

---

### 3. Identify behaviors to test

Prefer testing observable behavior.

Good test targets:

- Renders expected content.
- Required parameters are handled.
- Child content renders.
- Event callback fires.
- Disabled state prevents action.
- Loading state appears.
- Empty state appears.
- Error state appears.
- Validation message appears.
- Service is called when user submits.
- Retry callback works.
- Icon-only button has accessible label.
- Label/input relationship exists.
- Wrapper exposes expected project-owned API.

Avoid testing:

- Private methods.
- Radzen internals.
- Exact markup unless it is part of the component contract.
- CSS classes unless they are part of the public behavior.
- Framework behavior.
- Implementation details that can change without behavior changing.

---

## bUnit conventions

Use bUnit for component tests.

Typical shape:

```csharp
using Bunit;
using FluentAssertions;
using Xunit;

public sealed class AppButtonTests : TestContext
{
    [Fact]
    public void AppButton_WhenRendered_ShouldShowChildContent()
    {
        // Arrange & Act
        var component = RenderComponent<AppButton>(parameters => parameters
            .AddChildContent("Save"));

        // Assert
        component.Markup.Should().Contain("Save");
    }
}
```

Follow existing project setup if different.

---

## Arrange / Act / Assert

Use clear Arrange / Act / Assert.

```csharp
[Fact]
public void Component_WhenCondition_ShouldExpectedResult()
{
    // Arrange

    // Act

    // Assert
}
```

Do not hide important setup in overly magical helpers.

Use helpers/builders for repeated setup only when they make tests easier to read.

---

## Testing EventCallback

Example:

```csharp
[Fact]
public void AppButton_WhenClicked_ShouldInvokeOnClickCallback()
{
    // Arrange
    var wasClicked = false;

    var component = RenderComponent<AppButton>(parameters => parameters
        .Add(parameter => parameter.OnClick, EventCallback.Factory.Create(this, () => wasClicked = true))
        .AddChildContent("Save"));

    // Act
    component.Find("button").Click();

    // Assert
    wasClicked.Should().BeTrue();
}
```

Rules:

- Test the public callback behavior.
- Do not test private handler methods directly.
- Await async callbacks when needed.

---

## Testing parameters

Test parameter-driven behavior.

Example behaviors:

```text
AppButton_WhenDisabled_ShouldRenderDisabledButton
ErrorPanel_WhenMessageIsProvided_ShouldRenderMessage
CustomerCard_WhenCustomerIsInactive_ShouldShowInactiveStatus
```

Prefer strongly typed test data.

Use Bogus or builders only when data setup becomes noisy.

---

## Testing services in components

When a component depends on a service:

- Mock or fake the typed service.
- Register it in the bUnit test context.
- Test component behavior, not the service implementation.
- Use service unit tests for service logic.

Example with NSubstitute:

```csharp
var customerService = Substitute.For<ICustomerService>();
customerService.GetCustomersAsync(Arg.Any<CancellationToken>())
    .Returns([new CustomerDto { Id = 1, Name = "Customer A" }]);

Services.AddSingleton(customerService);
```

Then render the component and assert visible behavior.

---

## Testing loading, empty, and error states

For data-driven components/pages, test:

```text
WhenLoading_ShouldShowLoadingPanel
WhenItemsAreEmpty_ShouldShowEmptyState
WhenServiceFails_ShouldShowErrorPanel
WhenRetryClicked_ShouldReloadData
```

Keep error messages user-facing.

Do not assert technical exception messages unless the UI intentionally shows them.

---

## Testing forms

For form components, test:

- Required field validation.
- Valid submit behavior.
- Invalid submit behavior.
- Error response behavior.
- Duplicate submit prevention.
- Validation messages.
- Label/input relationships where practical.

Example names:

```text
CustomerForm_WhenNameIsMissing_ShouldShowValidationMessage
CustomerForm_WhenValidModelIsSubmitted_ShouldCallSaveService
CustomerForm_WhenSaveFails_ShouldShowErrorMessage
```

---

## Testing project-owned wrappers

Reusable wrapper components should be tested.

Test wrappers such as:

```text
AppButton
AppTextInput
AppSelect
AppCheckbox
AppDialog
AppConfirmDialog
AppAlert
AppCard
AppDataGrid
LoadingPanel
EmptyState
ErrorPanel
```

Test the wrapper contract:

- Parameters
- Events
- Disabled/loading states
- Child content
- Accessible names
- Important ARIA attributes
- Label/input relationship
- Validation display
- Empty/error/loading states
- Radzen integration assumptions only when visible through wrapper behavior

Do not test Radzen internals.

---

## Testing Radzen wrappers

When a wrapper uses Radzen internally:

- Test project-owned API and behavior.
- Do not assert Radzen internal class names unless they are explicitly part of the wrapper contract.
- Prefer user-visible text, roles, labels, attributes, and callback behavior.
- Keep tests resilient to Radzen implementation changes.

Bad:

```text
Assert exact Radzen-generated DOM structure.
```

Good:

```text
Assert AppButton renders child content and invokes OnClick.
```

---

## Accessibility-sensitive tests

Where practical, test:

- Real buttons are rendered for actions.
- Links are rendered for navigation.
- Inputs have labels.
- Icon-only buttons have accessible names.
- Error messages render.
- Required validation messages render.
- Dialog title renders.
- Important ARIA state is synchronized.

Automated tests do not replace manual keyboard testing.

Recommend manual checks for complex UI:

```text
Keyboard-only navigation
Visible focus
Dialog focus management
Lighthouse
axe DevTools
Accessibility Insights
Screen reader smoke test
```

---

## Mocking rules

Preferred:

- Simple fake for simple dependency
- NSubstitute or Moq for interaction-based tests
- Bogus for readable generated data when helpful

Rules:

- Do not mock the system under test.
- Do not over-mock.
- Do not verify every internal call.
- Verify interactions only when the interaction is the behavior.
- Use existing repository mocking library before adding a new one.

---

## Async test rules

- Use `async Task`.
- Always await async operations.
- Avoid `.Result` and `.Wait()`.
- Avoid arbitrary `Task.Delay`.
- Use bUnit async helpers where the existing project pattern uses them.
- Make async tests deterministic.

---

## Validation

Use commands from `docs/build-and-test.md`.

Typical commands:

```bash
dotnet test
dotnet build
```

For a specific test project:

```bash
dotnet test tests/ProjectName.ComponentTests/ProjectName.ComponentTests.csproj
```

Do not invent command results.

---

## Final response format

Return:

```text
Summary:
- [tests added/updated]
- [behaviors covered]
- [files changed]

Validation:
- [commands run]
- [commands recommended if not run]

Notes:
- [coverage gaps]
- [assumptions]
```

---

## Checklist

```text
[ ] Test names use MethodName_WhenCondition_ShouldExpectedResult.
[ ] Tests use Arrange / Act / Assert.
[ ] Tests focus on observable behavior.
[ ] bUnit is used for Blazor components.
[ ] xUnit is used for test framework.
[ ] FluentAssertions is used where available.
[ ] NSubstitute/Moq is used only where useful.
[ ] Reusable wrappers are tested.
[ ] Radzen internals are not tested.
[ ] Loading/empty/error states are tested where relevant.
[ ] Form validation is tested where relevant.
[ ] Async behavior is awaited correctly.
[ ] Tests are deterministic and independent.
[ ] Validation command was run or recommended.
```
