namespace TwilightImperiumUltimate.Web.Enums;

/// <summary>
/// Controls how <see cref="Components.Shared.Forms.ResponsiveForm"/> and
/// <see cref="Components.Shared.Forms.ResponsiveFormField{TValue}"/> surface validation errors.
/// </summary>
public enum ValidationDisplay
{
    /// <summary>
    /// No validation errors are rendered at all (neither the summary list nor per-field
    /// messages). Callers choosing this are responsible for surfacing errors some other way.
    /// </summary>
    None,

    /// <summary>
    /// Only the validation error list above the submit button is shown; per-field messages are
    /// suppressed.
    /// </summary>
    Summary,

    /// <summary>
    /// Only per-field validation messages (rendered next to each field) are shown; the summary
    /// list is suppressed.
    /// </summary>
    Field,
}
