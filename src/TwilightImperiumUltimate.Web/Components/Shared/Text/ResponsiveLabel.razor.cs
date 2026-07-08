namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

public partial class ResponsiveLabel
{
    /// <summary>
    /// Gets or sets a value indicating whether the label is constrained to a single line and
    /// truncated with an ellipsis when it overflows. When false (default) the text wraps so it
    /// stays fully readable on any screen size.
    /// </summary>
    [Parameter]
    public bool Truncate { get; set; }

    private string ComputedCssClass =>
        $"responsive-label handel white shadow {(Truncate ? "text-no-overflow" : string.Empty)} {GetVisibilityClass} {CssClass}";

    private string ComputedStyle => $"font-size: {FontSizeStyle}; color: {GetColorClass}; {GetTextAlignmentStyle} {Style}";
}
