using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class ImageBackgroundContainer
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string ImagePath { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;
}
