---
description: Generate or update tests for C#, Blazor components, services, pages, and wrapper components.
---

# Generate tests

Generate tests for the selected or requested code.

Follow:

```text
.github/instructions/testing.instructions.md
.github/instructions/blazor.instructions.md
.github/instructions/accessibility.instructions.md
```

Use:

- xUnit
- bUnit for Blazor components
- FluentAssertions when available
- NSubstitute or Moq when mocking helps
- Bogus when generated test data improves readability
- Playwright only for critical end-to-end workflows

---

## Task

Create or update tests for the requested class, service, component, page, or feature.

---

## Before writing tests

1. Inspect the code under test.
2. Identify observable behavior.
3. Identify dependencies.
4. Check existing test style and test project structure.
5. Prefer existing testing libraries already used in the repository.
6. Decide whether unit, component, integration, or E2E tests are appropriate.
7. Avoid testing private implementation details.

---

## Test naming

Always use:

```text
MethodName_WhenCondition_ShouldExpectedResult
```

Examples:

```text
SaveAsync_WhenModelIsValid_ShouldCallCustomerService
CustomerForm_WhenNameIsMissing_ShouldShowValidationMessage
AppButton_WhenDisabled_ShouldNotInvokeOnClickCallback
```

---

## Test style

Use Arrange / Act / Assert.

```csharp
[Fact]
public void CalculateTotal_WhenCartIsEmpty_ShouldReturnZero()
{
    // Arrange
    var cart = new ShoppingCart();

    // Act
    var result = cart.CalculateTotal();

    // Assert
    result.Should().Be(0);
}
```

---

## What to test

Test:

- Public behavior
- Important branches
- Error handling
- Validation
- Async behavior
- Event callbacks
- Loading, empty, error, and success states
- Service interactions when interaction is the behavior
- Wrapper component contracts
- Accessible labels/names/attributes where important

Do not test:

- Private methods directly
- Framework internals
- Radzen internals
- Exact markup unless it is part of the component contract
- CSS classes unless they are part of public behavior
- Trivial getters/setters
- Implementation details that can change without changing behavior

---

## bUnit guidance

For Blazor components:

- Render the component with meaningful parameters.
- Inject fake or mocked services.
- Interact with rendered elements.
- Assert visible behavior.
- Test project-owned wrapper behavior.
- Do not test Radzen internals.

---

## Output summary

Return:

```text
Summary:
- [tests added/updated]
- [behaviors covered]

Validation:
- [test command run or recommended]

Notes:
- [coverage gaps]
- [assumptions]
```
