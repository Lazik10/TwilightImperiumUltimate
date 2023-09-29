using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Buttons;

public partial class ArrowButton
{
    [Parameter]
    public EventCallback OnArrowClick { get; set; }

    [Parameter]
    public MarkupString ArrowSymbol { get; set; }
}
