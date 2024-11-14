namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class FactionsDS
{
    private FactionInfoGrid? factionInfoRef;

    [Parameter]
    [SupplyParameterFromQuery(Name = "faction")]
    public string Faction { get; set; } = string.Empty;

    [Parameter]
    [SupplyParameterFromQuery(Name = "info")]
    public string Info { get; set; } = string.Empty;

    private void UpdateSelectedFaction(FactionModel selectedFaction)
    {
        factionInfoRef?.UpdateSelectedFaction(selectedFaction);
        factionInfoRef?.SetFactionInfo(Info);
    }
}