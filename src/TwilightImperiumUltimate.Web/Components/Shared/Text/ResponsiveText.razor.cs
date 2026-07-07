namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

public partial class ResponsiveText
{
    private string ComputedCssClass => $"responsive-text handel white shadow {GetVisibilityClass} {CssClass}";

    private string ComputedStyle => $"font-family: Handel, fallback, sans-serif !important; font-size: {FontSizeStyle}; color: {GetColorClass}; {GetTextAlignmentStyle} {Style}";
}
