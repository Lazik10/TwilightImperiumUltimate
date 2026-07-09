using TwilightImperiumUltimate.Web.Helpers;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class HorizontalSpace
{
    [Parameter]
    public int Width { get; set; } = 100;

    private string GetWidthString() => $"{FluidSizing.GetFluidSpacing(Width)};";
}
