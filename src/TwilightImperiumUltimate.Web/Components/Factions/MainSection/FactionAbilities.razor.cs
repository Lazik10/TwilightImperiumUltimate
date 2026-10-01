namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionAbilities : FactionInfoComponentBase
{
    private MarkupString? _markupString;

    public MarkupString? Ability => _markupString;

    protected override void OnParametersSet()
    {
        _markupString = new MarkupString(FactionName.GetFactionUIText(FactionResourceType.Ability));
    }
}
