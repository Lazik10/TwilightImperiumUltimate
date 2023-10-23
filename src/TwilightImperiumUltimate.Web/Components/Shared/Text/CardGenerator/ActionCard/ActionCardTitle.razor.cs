using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Shared.Text.CardGenerator.ActionCard;

public partial class ActionCardTitle
{
    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int FontSize { get; set; } = 42;

    [Parameter]
    public int MarginTop { get; set; } = 0;
}
