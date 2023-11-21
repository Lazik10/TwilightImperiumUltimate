using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Helpers.Resources;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionAbilities : FactionInfoComponentBase
{
    private MarkupString? _markupString;

    public MarkupString? Ability => _markupString;

    protected override void OnParametersSet()
    {
        _markupString = new MarkupString(Faction.FactionName.GetFactionUIText(FactionResourceType.Ability));
    }
}
