using TwilightImperiumUltimate.Web.Helpers;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class VerticalSpace
{
    [Parameter]
    public int Height { get; set; } = 100;

    [Parameter]
    public int MinHeight { get; set; } = 0;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private string GetHeightString() => $"{FluidSizing.GetFluidSpacing(Height)};";

    private string GetMinHeightString() => $"{MinHeight}px;";
}
