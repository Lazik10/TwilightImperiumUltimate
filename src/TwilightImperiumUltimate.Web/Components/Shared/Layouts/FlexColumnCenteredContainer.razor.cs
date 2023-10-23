using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class FlexColumnCenteredContainer
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;
}
