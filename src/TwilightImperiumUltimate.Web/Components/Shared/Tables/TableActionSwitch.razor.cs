namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Compact ResponsiveSwitch for use inside a ResponsiveTable/ResponsiveGrid cell. Same OnText/
/// OffText/IsToggled contract as <see cref="Controls.ResponsiveSwitch"/>, just shrunk down to fit
/// a narrow column -- see TableActionSwitch.razor.css.
/// </summary>
public partial class TableActionSwitch
{
    /// <summary>
    /// Gets or sets a value indicating whether the switch is currently on.
    /// </summary>
    [Parameter]
    public bool IsToggled { get; set; }

    /// <summary>
    /// Gets or sets the label shown on the "on" side.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string OnText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the label shown on the "off" side.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string OffText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the callback raised when the toggle state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsToggledChanged { get; set; }

    /// <summary>
    /// Gets or sets the underlying input's "name" attribute.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the width as a percentage (only applied when between 0 and 100).
    /// </summary>
    [Parameter]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Gets or sets the color theme: Default (off=red, on=green), Primary (both off and on use
    /// the site's light-blue accent), or Custom (uses <see cref="EnabledColor"/>/
    /// <see cref="DisabledColor"/>). Applies to the toggle circle, the checked/unchecked track,
    /// and the currently active off/on label.
    /// </summary>
    [Parameter]
    public SwitchColorTheme Theme { get; set; } = SwitchColorTheme.Default;

    /// <summary>
    /// Gets or sets the color used for the "on" state. Only applied when <see cref="Theme"/> is
    /// <see cref="SwitchColorTheme.Custom"/>.
    /// </summary>
    [Parameter]
    public TextColor EnabledColor { get; set; } = TextColor.Green;

    /// <summary>
    /// Gets or sets the color used for the "off" state. Only applied when <see cref="Theme"/> is
    /// <see cref="SwitchColorTheme.Custom"/>.
    /// </summary>
    [Parameter]
    public TextColor DisabledColor { get; set; } = TextColor.Red;
}
