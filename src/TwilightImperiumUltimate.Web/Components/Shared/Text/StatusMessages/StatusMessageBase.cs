namespace TwilightImperiumUltimate.Web.Components.Shared.Text.StatusMessages;

/// <summary>
/// Base class for the generic, reusable status message components (<see cref="SuccessMessage"/>,
/// <see cref="InfoMessage"/>, <see cref="WarningMessage"/>, <see cref="ErrorMessage"/>) so every
/// message type shares the same "Text" + "CssClass" parameter surface instead of each component
/// redeclaring it.
/// </summary>
public abstract class StatusMessageBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the message text to display.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional CSS classes applied to the rendered message, so callers can match
    /// their page's existing state-message styling (e.g. spacing/alignment) instead of every page
    /// having to redefine its own look for this generic message.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;
}
