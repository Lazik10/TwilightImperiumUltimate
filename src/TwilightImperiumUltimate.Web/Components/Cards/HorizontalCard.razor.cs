using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class HorizontalCard
{
    [Parameter]
    public string ImagePath { get; set; } = string.Empty;
}
