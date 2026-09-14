namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

public partial class ResponsiveLabel
{
    /// <summary>
    /// Gets or sets a value indicating whether the label uses the default Handel typography and text shadow.
    /// </summary>
    [Parameter]
    public bool UseHandelStyling { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the label is constrained to a single line and
    /// truncated with an ellipsis when it overflows. When false (default) the text wraps so it
    /// stays fully readable on any screen size.
    /// </summary>
    [Parameter]
    public bool Truncate { get; set; }

    private string ComputedCssClass =>
        $"responsive-label {(UseHandelStyling ? "handel white shadow" : string.Empty)} {(Truncate ? "text-no-overflow" : string.Empty)} {GetVisibilityClass} {CssClass}";

    private string ComputedStyle => $"font-size: {FontSizeStyle}; color: {GetColorClass}; {GetTextAlignmentStyle} {Style}";
}
