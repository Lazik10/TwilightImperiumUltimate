using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class Page
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}