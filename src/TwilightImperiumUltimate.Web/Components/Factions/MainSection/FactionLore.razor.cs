namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionLore : FactionInfoComponentBase
{
    private MarkupString? _markupString;

    public MarkupString? Lore => _markupString;

    protected override void OnParametersSet()
    {
        _markupString = new MarkupString(FactionName.GetFactionUIText(FactionResourceType.Lore));
    }
}
