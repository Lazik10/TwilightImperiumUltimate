---
description: Create or update a typed C# service for Blazor WebAssembly components.
---

# Create service

Create or update a typed service based on the user's request.

This prompt is for client services, API services, state services, and reusable business/UI logic services.

---

## Workflow

1. Identify the service purpose.
2. Search for existing similar services.
3. Follow the repository folder and namespace conventions.
4. Create an interface and implementation when the project pattern uses interfaces.
5. Use dependency injection.
6. Use typed service methods rather than exposing raw `HttpClient`.
7. Add error handling where appropriate.
8. Add tests when behavior exists.
9. Update DI registration if needed.

---

## Preferred shape

```csharp
public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDto>> GetCustomersAsync(CancellationToken cancellationToken = default);
}
```

```csharp
public sealed class CustomerService : ICustomerService
{
    private readonly HttpClient httpClient;

    public CustomerService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetCustomersAsync(
        CancellationToken cancellationToken = default)
    {
        var customers = await httpClient.GetFromJsonAsync<List<CustomerDto>>(
            "api/customers",
            cancellationToken);

        return customers ?? [];
    }
}
```

Follow existing naming and field conventions.

---

## Rules

- Do not put service logic directly in components.
- Do not scatter raw API routes across many components.
- Keep service methods focused.
- Use strongly typed models.
- Use cancellation tokens where appropriate.
- Avoid swallowing exceptions unless converting them to a documented result type.
- Do not store secrets in the Blazor WebAssembly client.
- Add or update tests for non-trivial behavior.

---

## Output summary

Return:

```text
Summary:
- [service/interface created or updated]
- [DI registration updated]
- [tests added/updated]

Validation:
- [commands run or recommended]

Notes:
- [assumptions]
```
