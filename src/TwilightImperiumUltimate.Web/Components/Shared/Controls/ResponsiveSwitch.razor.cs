using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Thin wrapper around RadzenSwitch with an off/on text label flanking either side (e.g.
/// "Edit ⟷ Preview"), the currently active side highlighted to match ResponsiveTab's colors.
/// </summary>
public partial class ResponsiveSwitch
{
    [Parameter]
    public bool IsToggled { get; set; }

    [Parameter]
    [EditorRequired]
    public string OnText { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string OffText { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<bool> IsToggledChanged { get; set; }

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

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
    public TextColor EnabledColor { get; set; } = TextColor.DarkGreen;

    /// <summary>
    /// Gets or sets the color used for the "off" state. Only applied when <see cref="Theme"/> is
    /// <see cref="SwitchColorTheme.Custom"/>.
    /// </summary>
    [Parameter]
    public TextColor DisabledColor { get; set; } = TextColor.Red;

    /// <summary>
    /// Gets the resolved "on" state color for the current <see cref="Theme"/>.
    /// </summary>
    private TextColor ResolvedEnabledColor => Theme switch
    {
        SwitchColorTheme.Primary => TextColor.LightBlue,
        SwitchColorTheme.Custom => EnabledColor,
        _ => TextColor.DarkGreen,
    };

    /// <summary>
    /// Gets the resolved "off" state color for the current <see cref="Theme"/>.
    /// </summary>
    private TextColor ResolvedDisabledColor => Theme switch
    {
        SwitchColorTheme.Primary => TextColor.LightBlue,
        SwitchColorTheme.Custom => DisabledColor,
        _ => TextColor.Red,
    };

    /// <summary>
    /// Gets the inline style for the outer wrapper: width plus the Radzen switch's track/circle
    /// custom properties (real Radzen theme variables), set to the resolved on/off colors so the
    /// circle always matches the current toggle state regardless of nesting depth.
    /// </summary>
    private string ComputedStyle
    {
        get
        {
            var widthStyle = Width is > 0 and < 100 ? $"width: {Width}%;" : string.Empty;
            var enabledCss = ResolvedEnabledColor.ConvertToString();
            var disabledCss = ResolvedDisabledColor.ConvertToString();
            return $"{widthStyle} " +
                $"--rz-switch-background-color: {disabledCss}; " +
                $"--rz-switch-checked-background-color: {enabledCss}; " +
                $"--rz-switch-circle-background-color: {disabledCss}; " +
                $"--rz-switch-checked-circle-background-color: {enabledCss};";
        }
    }

    private async Task HandleValueChangedAsync(bool value)
    {
        IsToggled = value;
        await IsToggledChanged.InvokeAsync(value);
    }
}
