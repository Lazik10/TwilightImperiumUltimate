using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class FlexRowContainer
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public JustifyContent JustifyContent { get; set; } = JustifyContent.SpaceEvenly;

    [Parameter]
    public AlignItems AlignItems { get; set; } = AlignItems.Center;

    private string Justify => JustifyContent.GetJustifyString();

    private string Align => AlignItems.GetAlignString();
}
