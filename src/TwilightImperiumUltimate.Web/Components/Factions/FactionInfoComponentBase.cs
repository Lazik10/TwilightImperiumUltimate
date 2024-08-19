namespace TwilightImperiumUltimate.Web.Components.Factions;

public abstract class FactionInfoComponentBase : ComponentBase
{
    [Parameter]
    public FactionModel Faction { get; set; } = default!;
}
