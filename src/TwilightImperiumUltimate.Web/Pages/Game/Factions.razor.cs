namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class Factions
{
    private FactionInfoGrid? factionInfoRef;

    private void UpdateSelectedFaction(FactionModel selectedFaction)
    {
        factionInfoRef?.UpdateSelectedFaction(selectedFaction);
    }
}