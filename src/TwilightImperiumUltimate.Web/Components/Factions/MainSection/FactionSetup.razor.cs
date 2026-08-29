using TwilightImperiumUltimate.Contracts.DTOs.Unit;

namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionSetup : FactionInfoComponentBase
{
    private bool _showTechnologyPickMessage;

    private string _technologyPickMessage = string.Empty;

    private List<UnitWithCountDto> _startingUnits = [];

    private List<TechnologyDto> _startingTechnologies = [];

    private int _startingUnitMaxWidth = 100;

    private int _technologyColumns = 1;

    protected override void OnParametersSet()
    {
        _startingUnits = GetStartingUnits();
        _startingTechnologies = GetStartingTechnologies();
        _startingUnitMaxWidth = GetStartingUnitMaxWidth(_startingUnits);
        _technologyColumns = GetTechnologyColumns(_startingTechnologies.Count);

        GetStartingTechnologyString();
    }

    private static int GetTechnologyColumns(int technologyCount)
    {
        return technologyCount switch
        {
            <= 0 => 1,
            1 => 1,
            _ => 2,
        };
    }

    private static int GetStartingUnitMaxWidth(IReadOnlyCollection<UnitWithCountDto> startingUnits)
    {
        var totalStartingUnitCount = startingUnits.Sum(x => x.Count);
        return totalStartingUnitCount > 0 ? Math.Min(100 / 12, 100 / totalStartingUnitCount) : 100;
    }

    private List<UnitWithCountDto> GetStartingUnits()
    {
        return Faction.StartingUnits.OrderBy(x => x.UnitName).ToList();
    }

    private List<TechnologyDto> GetStartingTechnologies()
    {
        return Faction.StartingTechnologies.OrderBy(x => x.Type).ToList();
    }

    private void GetStartingTechnologyString()
    {
        _showTechnologyPickMessage = false;

        var (show, message) = FactionName switch
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
