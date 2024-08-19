namespace TwilightImperiumUltimate.Web.Pages.Game;

public partial class FactionsDS
{
    private FactionInfoGrid? factionInfoRef;

    private void UpdateSelectedFaction(FactionModel selectedFaction)
    {
        factionInfoRef?.UpdateSelectedFaction(selectedFaction);
    }
}