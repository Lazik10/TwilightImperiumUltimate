namespace TwilightImperiumUltimate.Web.Enums;

/// <summary>
/// Color theme for <c>ResponsiveSwitch</c>/<c>TableActionSwitch</c>: controls the color of the
/// toggle circle (always colored for the current on/off state) and the checked/unchecked track.
/// </summary>
public enum SwitchColorTheme
{
    /// <summary>Off state is red, on state is green.</summary>
    Default,

    /// <summary>Both the off and on states use the site's light-blue accent color.</summary>
    Primary,

    /// <summary>Uses the switch's EnabledColor/DisabledColor parameters.</summary>
    Custom,
}
