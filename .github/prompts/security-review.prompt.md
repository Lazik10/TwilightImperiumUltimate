---
description: Review C# / Blazor WebAssembly changes for security issues.
---

# Security review

Review the requested code, diff, feature, component, service, or page for security risks.

---

## Check

Review:

- Secrets in client code
- Connection strings or private keys
- Client/server trust boundary
- Authorization checks
- Authentication assumptions
- Server-side validation
- Raw HTML rendering
- MarkupString usage
- User-provided content
- API error handling
- Sensitive data logging
- Browser storage usage
- Dependency risk
- Unsafe JavaScript interop
- Open redirects or unsafe navigation
- CORS assumptions, if applicable

---

## Blazor WebAssembly rules

- Treat all client code as visible to users.
- Do not store secrets in the client.
- Do not rely only on client-side authorization.
- Validate protected operations on the server when a server/API exists.
- Do not render user-provided HTML unless sanitized and explicitly approved.
- Keep error messages user-friendly and avoid leaking internals.

---

## Output format

Return:

```text
Blocking security issues:
- [issue or "None"]

Security recommendations:
- [recommendation or "None"]

Tests / validation:
- [security tests or manual checks]

Overall:
- [Approve / Needs changes]
```
