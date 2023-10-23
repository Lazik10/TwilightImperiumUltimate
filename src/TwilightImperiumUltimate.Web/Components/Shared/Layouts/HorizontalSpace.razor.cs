using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class HorizontalSpace
{
    [Parameter]
    public int Width { get; set; } = 100;

    private string GetWidthString() => $"{Width}px";
}
