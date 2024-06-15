using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class VerticalCard
{
    [Parameter]
    public string ImagePath { get; set; } = string.Empty;

    [Parameter]
    public string TypeOfCard { get; set; } = string.Empty;
}