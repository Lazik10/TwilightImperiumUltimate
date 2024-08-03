namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class NumericPicker
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public int Value { get; set; }

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

    [Parameter]
    public int FontSize { get; set; } = 24;

    [Parameter]
    public bool HideTitle { get; set; } = false;
}
