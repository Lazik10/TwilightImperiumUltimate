using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Helpers.Resources;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionSystemStats
{
    [Parameter]
    public FactionName FactionName { get; set; } = default!;

    private string[] FactionUIText()
    {
        string text = FactionName.GetFactionUIText(FactionResourceType.SystemStats);
        string[] lines = text.Split(';');
        return lines;
    }
}
