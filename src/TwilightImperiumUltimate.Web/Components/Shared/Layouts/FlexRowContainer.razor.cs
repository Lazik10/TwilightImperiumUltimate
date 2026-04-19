using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class FlexRowContainer : LayoutContainerBase
{
    [Parameter]
    public JustifyContent JustifyContent { get; set; } = JustifyContent.SpaceEvenly;

    [Parameter]
    public AlignItems AlignItems { get; set; } = AlignItems.Center;

    private string Justify => JustifyContent.GetJustifyString();

    private string Align => AlignItems.GetAlignString();

    private string GetContainerStyle() => $"justify-content: {Justify}; align-items: {Align}; {Style}";
}
