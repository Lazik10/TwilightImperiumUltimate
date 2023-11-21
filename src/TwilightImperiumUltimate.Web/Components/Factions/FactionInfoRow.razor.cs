using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionInfoRow
{
    [Parameter]
    public EventCallback<FactionInfoType> OnFactionInfoTypeChange { get; set; }

    private void OnInfoTypeClick(FactionInfoType factionInfoType) => OnFactionInfoTypeChange.InvokeAsync(factionInfoType);
}
