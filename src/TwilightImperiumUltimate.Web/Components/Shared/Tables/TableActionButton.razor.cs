using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Compact action button sized to fit inside a ResponsiveTable cell. Mirrors
/// ResponsiveButton's Text/ChildContent/OnClick/TextColor/disabled contract, just at table-cell
/// scale -- see <see cref="Controls.ResponsiveButton"/> for the full-size page button used
/// everywhere else.
/// </summary>
public partial class TableActionButton
{
    /// <summary>
    /// Gets or sets the plain text content. Ignored when <see cref="ChildContent"/> is set.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets custom child content. Takes precedence over <see cref="Text"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the text color. Defaults to white.
    /// </summary>
    [Parameter]
    public TextColor TextColor { get; set; } = TextColor.White;

    /// <summary>
    /// Gets or sets a value indicating whether the button is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Gets or sets the native button type. Defaults to "button" (never submits a form).
    /// </summary>
    [Parameter]
    public ButtonType ButtonType { get; set; } = ButtonType.Button;

    /// <summary>
    /// Gets or sets the click callback.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets the click callback that also receives the mouse event args.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClickMouse { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional inline styles.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional unmatched attributes (e.g. "aria-label" for an icon-only button).
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task OnClickHandler(MouseEventArgs e)
    {
        if (IsDisabled)
            return;

        if (OnClickMouse.HasDelegate)
            await OnClickMouse.InvokeAsync(e);

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync();
    }

    private string GetButtonType() => ButtonType switch
    {
        ButtonType.Submit => "submit",
        ButtonType.Reset => "reset",
        _ => "button",
    };
}
