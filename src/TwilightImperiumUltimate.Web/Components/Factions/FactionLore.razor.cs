namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionLore : FactionInfoComponentBase
{
    private MarkupString? _markupString;

    public MarkupString? Lore => _markupString;

    protected override void OnParametersSet()
    {
        _markupString = new MarkupString(Faction.FactionName.GetFactionUIText(FactionResourceType.Lore));
    }
}
