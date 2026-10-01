namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public abstract class FactionInfoComponentBase : ComponentBase
{
    [Inject]
    protected IFactionProvider FactionProvider { get; set; } = default!;

    protected FactionName FactionName => FactionProvider.CurrentFactionName;

    protected FactionDto Faction => FactionProvider.CurrentFaction!;
}
