# Copilot Instructions

## Project Guidelines
- Follow .editorconfig rules during refactoring (including StyleCop documentation rules like SA1623/SA1628), and keep Markdown files organized under a Documentation folder grouped by category with templates for new skillset docs.
- During refactoring, check for existing similar components and extract shared/general behavior into shared base classes or templates. If an existing component already handles similar behavior, refactor toward shared/general components instead of duplicating logic. **Create new reusable components when patterns repeat across multiple pages.**
- **Prefer using `ResponsiveContainer` or `ResponsiveGridContainer` over legacy flex containers and `GridLayout` on a page-by-page migration basis** (do not delete old components yet). Migrate legacy components to newer shared components page-by-page. Keep backward compatibility until each page is refactored. When asked to add alignment parameters, apply them to `ResponsiveContainer` rather than `ResponsiveText`. **Keep `JustifyContent` and `AlignItems` parameters on `ResponsiveContainer`; when `CenteredItems` is true, ignore those values and force centered enum alignment.**
- During refactoring, check for unused namespaces and remove them. Move common usings to `_Imports.razor` when appropriate.
- Move hardcoded text into appropriate `Resources` `.resx` files. Use `Paths.resx` for API paths/page routing, `ValidationMessages` for validation, and keep `Rules/RuleNotes` for pasted rules notes content.
- Prefer shared templates and shared base classes for repeated parameters/behavior (for example `Width`, `CssClass`, `Style`, click handling) so they are not redefined repeatedly.
- Always use code-behind files (`.razor.cs`, `.razor.js`, `.razor.css`) when needed; never put implementation logic directly into `.razor` files.
- **When creating grid-based layouts, use shell classes (e.g., `.cards-grid-shell`) for responsive width control (80% desktop, 92% tablet, 98% mobile).**
- If uncertainty remains about behavior or layout impact during refactoring, ask clarifying questions first.
- **Avoid using CSS wrapper divs for mobile-only spacer visibility. Instead, implement a reusable `ShowOnMobile` boolean on `ResponsiveVerticalSpacer` and utilize that property in places with the same behavior.**

## Testing Guidelines
- Do not run tests unrelated to the current task.