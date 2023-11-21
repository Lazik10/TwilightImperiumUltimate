using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Factions;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public abstract class FactionInfoComponentBase : ComponentBase
{
    [Parameter]
    public FactionModel Faction { get; set; } = default!;
}
