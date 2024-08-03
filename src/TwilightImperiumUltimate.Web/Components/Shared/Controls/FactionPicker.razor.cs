namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class FactionPicker
{
    [Parameter]
    public FactionName Value { get; set; }

    [Parameter]
    public EventCallback OnDecrease { get; set; }

    [Parameter]
    public EventCallback OnIncrease { get; set; }

    [Parameter]
    public MarkupString LeftArrowSymbol { get; set; } = (MarkupString)Strings.ButtonLeftArrow;

    [Parameter]
    public MarkupString RightArrowSymbol { get; set; } = (MarkupString)Strings.ButtonRightArrow;

    [Parameter]
    public int Width { get; set; } = 100;
}