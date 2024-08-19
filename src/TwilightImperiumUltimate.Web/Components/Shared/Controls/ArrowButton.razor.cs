namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ArrowButton
{
    [Parameter]
    public EventCallback OnArrowClick { get; set; }

    [Parameter]
    public MarkupString ArrowSymbol { get; set; }

    [Parameter]
    public int FontSize { get; set; } = 46;
}
