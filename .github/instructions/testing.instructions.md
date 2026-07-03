---
applyTo: "**/*Tests*/**, **/*.Tests.cs, **/*Test.cs, **/*.razor, **/*.razor.cs"
---

# Testing instructions

These instructions apply to test files and to Blazor component files where behavior changes should trigger test consideration.

This repository uses **C# / Blazor WebAssembly**. Prefer **xUnit** for unit tests unless the repository already uses another framework. Prefer **bUnit** for Blazor component tests. Use **Playwright** for critical end-to-end user flows.

Testing should focus on behavior, maintainability, and regression prevention. Do not write brittle tests that only duplicate implementation details.

---

## Core principles

- Behavior changes should normally include tests.
- Prefer tests that describe observable behavior.
- Use clear Arrange / Act / Assert structure.
- Prefer readable tests over clever tests.
- Prefer deterministic tests.
- Avoid testing private implementation details.
- Avoid excessive mocking.
- Prefer simple fakes or stubs when they make tests easier to understand.
- Use mocking libraries when they improve clarity.
- Reusable wrapper components should be tested.
- Critical user flows should be covered by end-to-end tests where practical.
- Follow existing repository test patterns unless they conflict with these instructions.

---

## Preferred test stack

Use these tools unless the repository already has a different established stack:

| Test type | Preferred tool |
|---|---|
| Unit tests | xUnit |
| Assertions | FluentAssertions |
| Mocks | NSubstitute or Moq |
| Test data | Bogus or explicit builders |
| Blazor component tests | bUnit |
| End-to-end tests | Playwright |
| API integration tests | ASP.NET Core test host / WebApplicationFactory, if applicable |

Do not introduce new testing packages without explaining why.

If the repository already uses a different framework, follow the existing project pattern before introducing another one.

---

## Test project structure

Preferred structure:

```text
tests/
  ProjectName.UnitTests/
  ProjectName.ComponentTests/
  ProjectName.IntegrationTests/
  ProjectName.EndToEndTests/
```

Suggested responsibilities:

| Project | Responsibility |
|---|---|
| `*.UnitTests` | Pure logic, services, validators, mappers, state services |
| `*.ComponentTests` | Blazor components and project-owned UI wrappers |
| `*.IntegrationTests` | API, persistence, authentication, external boundaries |
| `*.EndToEndTests` | Critical user workflows in a real browser |

Do not create all test projects automatically. Add them when the repository needs them.

---

## Test naming convention

Always use this test naming style:

```text
MethodName_WhenCondition_ShouldExpectedResult
```

Examples:

```csharp
CalculateTotal_WhenCartIsEmpty_ShouldReturnZero
LoadCustomersAsync_WhenServiceFails_ShouldShowErrorMessage
SaveAsync_WhenModelIsValid_ShouldCallCustomerService
AppButton_WhenClicked_ShouldInvokeOnClickCallback
```

Rules:

- Test names should explain the behavior.
- Avoid vague names such as `Test1`, `Works`, or `ShouldPass`.
- Prefer behavior words such as `ShouldShow`, `ShouldReturn`, `ShouldCall`, `ShouldRender`, `ShouldNavigate`, `ShouldValidate`.
- For component tests, use the component behavior as the method name when no single method is being tested.

---

## Arrange / Act / Assert

Use a clear Arrange / Act / Assert structure.

Example:

```csharp
[Fact]
public void CalculateTotal_WhenCartIsEmpty_ShouldReturnZero()
{
    // Arrange
    var cart = new ShoppingCart();

    // Act
    var total = cart.CalculateTotal();

    // Assert
    total.Should().Be(0);
}
```

Rules:

- Keep arrange setup focused.
- Keep one main behavior per test.
- Avoid multiple unrelated assertions.
- Use comments when they improve readability.
- Do not over-comment obvious tests.
- Prefer helper methods or builders for repeated setup.

---

## xUnit conventions

Prefer xUnit for new test projects unless the repository already uses another framework.

Rules:

- Use `[Fact]` for tests without parameters.
- Use `[Theory]` with `[InlineData]`, `[MemberData]`, or `[ClassData]` for parameterized tests.
- Keep test classes focused on one subject.
- Avoid test ordering dependencies.
- Tests must be able to run independently.
- Avoid shared mutable state.
- Use constructor setup only for simple common setup.
- Use `IAsyncLifetime` for async setup and cleanup when needed.

Example:

```csharp
public sealed class PriceCalculatorTests
{
    [Theory]
    [InlineData(100, 0.10, 90)]
    [InlineData(200, 0.25, 150)]
    public void ApplyDiscount_WhenDiscountIsValid_ShouldReturnDiscountedPrice(
        decimal price,
        decimal discount,
        decimal expected)
    {
        // Arrange
        var calculator = new PriceCalculator();

        // Act
        var result = calculator.ApplyDiscount(price, discount);

        // Assert
        result.Should().Be(expected);
    }
}
```

---

## Assertions

Prefer FluentAssertions for readable assertions when available.

Good:

```csharp
result.Should().NotBeNull();
result.Name.Should().Be("Customer A");
items.Should().ContainSingle();
items.Should().OnlyContain(item => item.IsActive);
```

Avoid unclear or low-signal assertions:

```csharp
Assert.True(result != null);
Assert.True(items.Count == 1);
```

Use xUnit assertions if FluentAssertions is not available and adding it is not justified.

---

## Mocking

Use mocking libraries when they improve clarity.

Preferred libraries:

- NSubstitute
- Moq

Use Bogus for realistic generated test data when it improves readability.

Rules:

- Prefer simple fakes or stubs for simple dependencies.
- Use NSubstitute or Moq for interaction-based tests.
- Do not mock the system under test.
- Do not over-mock simple value objects or DTOs.
- Do not verify every internal call.
- Verify interactions only when the interaction is the behavior.
- Keep mocks local to the test unless shared setup is genuinely useful.
- Avoid brittle tests that fail when implementation changes but behavior does not.

Example with NSubstitute:

```csharp
[Fact]
public async Task SaveAsync_WhenModelIsValid_ShouldCallCustomerService()
{
    // Arrange
    var customerService = Substitute.For<ICustomerService>();
    var model = new CustomerEditModel { Name = "Customer A" };
    var sut = new CustomerEditor(customerService);

    // Act
    await sut.SaveAsync(model);

    // Assert
    await customerService.Received(1).SaveAsync(model);
}
```

Example with Moq:

```csharp
[Fact]
public async Task SaveAsync_WhenModelIsValid_ShouldCallCustomerService()
{
    // Arrange
    var customerService = new Mock<ICustomerService>();
    var model = new CustomerEditModel { Name = "Customer A" };
    var sut = new CustomerEditor(customerService.Object);

    // Act
    await sut.SaveAsync(model);

    // Assert
    customerService.Verify(service => service.SaveAsync(model), Times.Once);
}
```

Use the mocking library already used by the repository before introducing a new one.

---

## Test data

Prefer explicit test data when only a few values are needed.

Good:

```csharp
var customer = new CustomerDto
{
    Id = 1,
    Name = "Customer A",
    IsActive = true
};
```

Use builders or Bogus when data setup becomes repetitive or noisy.

Example builder naming:

```csharp
var customer = CustomerDtoBuilder
    .Create()
    .WithName("Customer A")
    .WithActiveStatus()
    .Build();
```

Rules:

- Keep test data relevant to the behavior under test.
- Avoid random data unless the values do not matter.
- If using Bogus, seed randomness or keep assertions independent of random values.
- Avoid large object graphs when a small object is enough.
- Prefer readable test data over overly generic factories.

---

## Unit tests

Use unit tests for:

- Business logic.
- Validation logic.
- Mapping logic.
- Formatting logic.
- State services.
- Client services with mocked dependencies.
- Pure helper classes.
- Error handling behavior.

Unit tests should not depend on:

- Real databases.
- Real network calls.
- Real external services.
- Browser behavior.
- Test execution order.

Example:

```csharp
[Fact]
public void Validate_WhenEmailIsMissing_ShouldReturnValidationError()
{
    // Arrange
    var validator = new CustomerValidator();
    var model = new CustomerEditModel
    {
        Name = "Customer A",
        Email = string.Empty
    };

    // Act
    var result = validator.Validate(model);

    // Assert
    result.Errors.Should().Contain(error => error.PropertyName == nameof(CustomerEditModel.Email));
}
```

---

## Client service tests

Client services should be tested when they contain behavior beyond trivial pass-through code.

Test:

- Correct API route usage.
- Request model creation.
- Response handling.
- Error handling.
- Retry behavior, if implemented.
- Cancellation behavior, if implemented.
- Mapping between DTOs and UI models.

Do not test framework behavior.

Prefer fake `HttpMessageHandler` or a test server rather than real network calls.

Example focus:

```text
GetCustomersAsync_WhenApiReturnsCustomers_ShouldReturnCustomerList
GetCustomersAsync_WhenApiReturnsError_ShouldThrowOrReturnExpectedError
SaveCustomerAsync_WhenRequestIsValid_ShouldSendExpectedPayload
```

---

## Blazor component tests with bUnit

Use bUnit for Blazor component behavior.

Component tests should verify:

- Rendered content.
- Parameters.
- Event callbacks.
- Conditional rendering.
- Loading states.
- Empty states.
- Error states.
- Validation messages.
- Form behavior.
- Accessible names and important attributes.
- Project-owned wrapper behavior.
- Interaction with injected services.

Prefer testing behavior over implementation details.

Example:

```csharp
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
}
```

Follow the existing bUnit setup style if one already exists.

---

## Testing reusable wrapper components

Reusable wrapper components should be tested because they centralize UI behavior.

Test wrapper components such as:

- `AppButton`
- `AppTextInput`
- `AppSelect`
- `AppCheckbox`
- `AppDialog`
- `AppDataGrid`
- `LoadingPanel`
- `EmptyState`
- `ErrorPanel`

Test:

- Child content rendering.
- Parameters.
- Event callbacks.
- Disabled/loading states.
- CSS class behavior where meaningful.
- Accessible names.
- Important ARIA attributes.
- Label/input relationships.
- Validation display.
- Radzen integration assumptions, if the wrapper depends on Radzen.

Do not test Radzen internals. Test the project-owned wrapper contract.

Good test focus:

```text
AppButton_WhenDisabled_ShouldNotInvokeOnClickCallback
AppTextInput_WhenRendered_ShouldAssociateLabelWithInput
AppDialog_WhenOpen_ShouldRenderTitleAndContent
AppDataGrid_WhenItemsAreEmpty_ShouldShowEmptyState
```

---

## Testing pages

Page tests should focus on page behavior, not every detail of child components.

Test:

- Initial loading behavior.
- Data loaded through services.
- Error state when a service fails.
- Empty state when no data exists.
- Main user interactions.
- Navigation behavior, if relevant.
- Important authorization or permission-dependent rendering.

Prefer mocking typed services rather than making real API calls.

Example test names:

```text
CustomersPage_WhenCustomersLoad_ShouldRenderCustomerList
CustomersPage_WhenServiceFails_ShouldShowErrorPanel
CustomersPage_WhenNoCustomersExist_ShouldShowEmptyState
```

---

## Forms and validation tests

Forms should be tested when they contain important behavior.

Test:

- Required fields.
- Validation messages.
- Successful submit.
- Failed submit.
- Duplicate submit prevention.
- Error response handling.
- Field-level behavior.
- Correct service calls.
- Accessible labels and validation association where practical.

Example test names:

```text
CustomerForm_WhenNameIsMissing_ShouldShowValidationMessage
CustomerForm_WhenValidModelIsSubmitted_ShouldCallSaveService
CustomerForm_WhenSaveFails_ShouldShowErrorMessage
```

---

## Async test rules

Rules:

- Use `async Task` tests for async behavior.
- Always await async operations.
- Do not use `.Result` or `.Wait()`.
- Avoid arbitrary `Task.Delay`.
- Prefer deterministic synchronization.
- Test loading state separately from completed state when important.
- Ensure exceptions from async operations are asserted clearly.

Good:

```csharp
[Fact]
public async Task LoadAsync_WhenServiceFails_ShouldSetErrorMessage()
{
    // Arrange
    var service = Substitute.For<ICustomerService>();
    service.GetCustomersAsync().Returns<Task<IReadOnlyList<CustomerDto>>>(_ => throw new InvalidOperationException());

    var sut = new CustomerViewModel(service);

    // Act
    await sut.LoadAsync();

    // Assert
    sut.ErrorMessage.Should().Be("Customers could not be loaded.");
}
```

---

## Accessibility testing reminders

For UI-related tests, consider accessibility-sensitive behavior.

Check where practical:

- Buttons are real buttons.
- Links are real links.
- Inputs have labels.
- Icon-only buttons have accessible names.
- Error messages are rendered.
- Loading states are rendered.
- Focus-related behavior is covered for dialogs and complex widgets.
- Important ARIA attributes are present and state is synchronized.

Automated component tests do not replace manual keyboard and screen reader checks.

For non-trivial UI changes, recommend:

- Keyboard-only testing.
- Lighthouse.
- axe DevTools.
- Accessibility Insights.
- Browser accessibility tree inspection.
- Screen reader smoke testing when practical.

---

## Playwright end-to-end tests

Use Playwright for critical browser workflows.

Good candidates:

- Login and logout.
- Main navigation.
- Critical form submission.
- Checkout or payment flow.
- Admin workflow.
- Data creation/edit/delete workflow.
- Permission-sensitive workflow.
- Regression-prone user journeys.

Do not use E2E tests for every small component. Prefer unit and component tests for smaller behavior.

E2E test rules:

- Test user-visible behavior.
- Use stable locators.
- Prefer accessible role/name locators when possible.
- Avoid brittle CSS selectors.
- Avoid arbitrary sleeps.
- Keep tests independent.
- Reset or isolate test data.
- Capture traces/screenshots on failure where CI supports it.

Example test names:

```text
CreateCustomer_WhenRequiredFieldsAreCompleted_ShouldCreateCustomer
Login_WhenCredentialsAreValid_ShouldNavigateToDashboard
OrdersPage_WhenSearchTermIsEntered_ShouldShowFilteredResults
```

---

## Integration tests

Use integration tests when testing boundaries that unit tests should not fake.

Use integration tests for:

- API endpoints.
- Persistence.
- Authentication and authorization.
- Serialization.
- Dependency injection configuration.
- Middleware behavior.
- Server-side validation.

Rules:

- Keep integration tests isolated.
- Use test databases or containers when needed.
- Do not depend on production services.
- Do not require shared mutable external state.
- Keep integration tests slower but meaningful.
- Use unit tests for most business logic.

---

## What not to test

Avoid tests that only verify:

- Private methods directly.
- Framework internals.
- Third-party library internals.
- Exact HTML markup when behavior is enough.
- CSS class names unless they are part of the component contract.
- Implementation details that can change without behavior changing.
- Trivial property getters and setters.
- Generated code, unless customized behavior is added.

Do not write snapshot-style markup tests unless the project intentionally uses them and accepts the maintenance cost.

---

## Regression tests

Bug fixes should usually include a regression test.

The test should fail before the fix and pass after the fix.

Name examples:

```text
SaveAsync_WhenServerReturnsValidationError_ShouldShowValidationMessage
OrderList_WhenOrderIsDeleted_ShouldRemoveOrderFromRenderedList
```

---

## Test maintainability

Rules:

- Keep tests short and focused.
- Use helper methods for repeated setup.
- Use builders for complex test data.
- Avoid hidden shared mutable setup.
- Prefer clear explicit setup over magical test infrastructure.
- Avoid testing multiple unrelated behaviors in one test.
- Keep tests deterministic.
- Do not make tests depend on execution order.
- Do not make tests depend on local machine configuration unless documented.

---

## CI validation

Recommended validation commands:

```bash
dotnet restore
dotnet build
dotnet test
```

Optional formatting check:

```bash
dotnet format --verify-no-changes
```

If Playwright tests are included in CI, document the browser installation and test commands in:

```text
docs/build-and-test.md
```

Do not invent CI commands if `docs/build-and-test.md` already documents them.

---

## Documentation updates

Update testing documentation when:

- A new test project is added.
- A test framework is introduced.
- Test commands change.
- Playwright is added.
- bUnit is added.
- CI validation changes.
- Test data setup requirements change.
- Integration tests require local infrastructure.

Relevant files:

```text
docs/build-and-test.md
docs/architecture.md
docs/project-structure.md
```

---

## Copilot behavior

When generating or editing code:

1. Check whether the change affects behavior.
2. If behavior changes, add or update tests where practical.
3. Prefer xUnit for new unit tests.
4. Prefer bUnit for Blazor component tests.
5. Prefer Playwright for critical end-to-end user flows.
6. Use the test name format `MethodName_WhenCondition_ShouldExpectedResult`.
7. Use Arrange / Act / Assert.
8. Prefer FluentAssertions when available.
9. Use NSubstitute, Moq, Bogus, or other industry-standard testing libraries when they improve clarity.
10. Use the mocking library already present in the repository before adding a new one.
11. Do not over-mock.
12. Test observable behavior, not private implementation details.
13. Add regression tests for bug fixes.
14. Test reusable wrapper components.
15. Do not test Radzen internals; test project-owned wrapper behavior.
16. Mention recommended test commands in summaries.

---

## Review checklist

Use this checklist for non-trivial changes:

```text
[ ] Behavior changes include tests where practical.
[ ] Tests use xUnit or the existing repository framework.
[ ] Test names follow MethodName_WhenCondition_ShouldExpectedResult.
[ ] Tests use clear Arrange / Act / Assert structure.
[ ] Assertions are readable.
[ ] Mocks/fakes are used only where helpful.
[ ] Tests focus on observable behavior.
[ ] Private implementation details are not tested directly.
[ ] Async tests are awaited correctly.
[ ] Component behavior is tested with bUnit where appropriate.
[ ] Reusable wrapper components are tested.
[ ] Radzen internals are not tested directly.
[ ] Critical user flows are considered for Playwright.
[ ] Bug fixes include regression tests where practical.
[ ] Tests are deterministic and independent.
[ ] Test documentation or build instructions were updated if needed.
```
