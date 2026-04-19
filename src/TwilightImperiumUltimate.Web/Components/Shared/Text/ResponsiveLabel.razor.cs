namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

public partial class ResponsiveLabel
{
    private string ComputedCssClass => $"responsive-label handel white shadow text-no-overflow {GetVisibilityClass()} {CssClass}";

    private string ComputedStyle => $"font-size: {GetFontSizeStyle()}; color: {GetColorClass()}; {GetTextAlignmentStyle()} {Style}";
}
