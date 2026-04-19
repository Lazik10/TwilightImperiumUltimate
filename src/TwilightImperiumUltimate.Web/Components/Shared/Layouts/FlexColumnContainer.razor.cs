using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class FlexColumnContainer : LayoutContainerBase
{
    [Parameter]
    public int Height { get; set; } = 100;

    [Parameter]
    public JustifyContent JustifyContent { get; set; } = JustifyContent.FlexStart;

    [Parameter]
    public AlignItems AlignItems { get; set; } = AlignItems.FlexStart;

    private string Justify => JustifyContent.GetJustifyString();

    private string Align => AlignItems.GetAlignString();

    private string GetContainerStyle() => $"justify-content: {Justify}; align-items: {Align}; min-height: {Height}%; {Style}";
}
