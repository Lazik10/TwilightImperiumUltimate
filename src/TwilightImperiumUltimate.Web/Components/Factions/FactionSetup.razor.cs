namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionSetup : FactionInfoComponentBase
{
    private bool _showTechnologyPickMessage;

    private string _technologyPickMessage = string.Empty;

    private int NumberOfStartingTechnologies => Faction.StartingTechnologies.Count;

    protected override void OnParametersSet()
    {
         GetStartingTechnologyString();
    }

    private List<UnitModel> GetStartingUnits()
    {
        return Faction.StartingUnits.OrderBy(x => x.UnitName).ToList();
    }

    private List<TechnologyModel> GetStartingTechnologies()
    {
        return Faction.StartingTechnologies.OrderBy(x => x.Type).ToList();
    }

    private void GetStartingTechnologyString()
    {
        _showTechnologyPickMessage = false;

        var (show, message) = Faction.FactionName switch
        {
            FactionName.TheWinnu or FactionName.TheCheiranHordes or FactionName.TheGhotiWayfarers or FactionName.TheMonksOfKolume
            or FactionName.TheKyroSodality or FactionName.TheBerserkersOfKjalengard
            => (true, Strings.Faction_SetupChooseOneTechnology),

            FactionName.TheArgentFlight or FactionName.TheCeldauriTradeConfederation or FactionName.TheTnelisSyndicate
            or FactionName.TheVadenBankingClans or FactionName.TheBentorConglomerate or FactionName.TheGledgeUnion
            or FactionName.TheLanefirRemnants or FactionName.TheNokarSellships
            => (true, Strings.Faction_SetupChooseTwoTechnologies),

            FactionName.TheCouncilKeleres
            => (true, Strings.Faction_SetupChooseKeleresTechnologies),

            FactionName.TheEdynMandate
            => (true, Strings.Faction_SetupChooseEdynTechnologies),

            FactionName.TheDeepwroughtScholarate
            => (true, Strings.Faction_SetupChooseDeepwroughtTechnologies),

            FactionName.TheCrimsonRebellion
            => (true, Strings.Faction_SetupChooseCrimsonTechnologies),

            FactionName.TheRalNelConsortium
            => (true, Strings.Faction_SetupChooseRalNelTechnologies),

            FactionName.LastBastion
            => (true, Strings.Faction_SetupChooseBastionTechnologies),

            FactionName.TheFirmamentTheObsidian
            => (true, Strings.Faction_SetupChooseFirmamentTechnologies),

            _ => (false, string.Empty),
        };

        _showTechnologyPickMessage = show;
        _technologyPickMessage = message;
    }
}
