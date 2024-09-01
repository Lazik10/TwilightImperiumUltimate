namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class FlexColumnCenteredContainer : TwilightImperiumBaseComponenet
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;
}
