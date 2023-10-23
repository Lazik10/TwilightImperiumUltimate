namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeFactionColorImportance()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var factionColors = GetFactionColorMappings();

        foreach (var (faction, colorMapping) in factionColors)
        {
            foreach (var (color, importance) in colorMapping)
            {
                var entity = new FactionColorImportance
                {
                    FactionName = faction,
                    Color = color,
                    Importance = importance,
                };
                context.FactionColorImportances.Add(entity);
            }
        }

        context.SaveChanges();
    }

    private Dictionary<FactionName, Dictionary<PlayerColor, int>> GetFactionColorMappings()
    {
        return new()
        {
            { FactionName.TheArborec, CreateColorMapping(1, 2, 6, 10, 0, 1, 0, 2) },
            { FactionName.TheArgentFlight, CreateColorMapping(2, 10, 2, 7, 2, 2, 0, 1) },
            { FactionName.TheBaronyOfLetnev, CreateColorMapping(8, 2, 0, 1, 4, 0, 0, 9) },
            { FactionName.TheClanOfSaar, CreateColorMapping(1, 9, 8, 1, 0, 1, 0, 2) },
            { FactionName.TheEmbersOfMuaat, CreateColorMapping(8, 9, 4, 0, 0, 0, 0, 4) },
            { FactionName.TheEmiratesOfHacan, CreateColorMapping(4, 8, 9, 0, 0, 0, 0, 1) },
            { FactionName.TheEmpyrean, CreateColorMapping(7, 4, 1, 0, 2, 9, 3, 5) },
            { FactionName.TheFederationOfSol, CreateColorMapping(2, 0, 8, 2, 9, 0, 0, 1) },
            { FactionName.TheGhostsOfCreuss, CreateColorMapping(0, 1, 0, 1, 10, 2, 1, 7) },
            { FactionName.TheL1z1xMindnet, CreateColorMapping(8, 1, 0, 1, 9, 0, 0, 8) },
            { FactionName.TheMahactGeneSorcerers, CreateColorMapping(1, 0, 10, 0, 0, 7, 2, 1) },
            { FactionName.TheMentakCoalition, CreateColorMapping(0, 9, 9, 0, 0, 4, 0, 8) },
            { FactionName.TheNaaluCollective, CreateColorMapping(0, 9, 9, 8, 0, 1, 0, 1) },
            { FactionName.TheNaazRokhaAlliance, CreateColorMapping(1, 0, 7, 10, 0, 1, 0, 2) },
            { FactionName.TheNekroVirus, CreateColorMapping(10, 1, 1, 0, 1, 1, 0, 4) },
            { FactionName.TheNomad, CreateColorMapping(1, 1, 2, 1, 8, 8, 7, 4) },
            { FactionName.SardakkNorr, CreateColorMapping(9, 0, 2, 4, 0, 0, 0, 8) },
            { FactionName.TheTitansOfUl, CreateColorMapping(0, 0, 1, 1, 1, 4, 10, 1) },
            { FactionName.TheUniversitiesOfJolNar, CreateColorMapping(0, 0, 1, 2, 8, 9, 4, 1) },
            { FactionName.TheVuilRaithCabal, CreateColorMapping(10, 6, 1, 0, 0, 0, 0, 6) },
            { FactionName.TheWinnu, CreateColorMapping(6, 8, 8, 2, 0, 9, 1, 1) },
            { FactionName.TheXxchaKingdom, CreateColorMapping(0, 1, 4, 10, 6, 1, 0, 2) },
            { FactionName.TheYinBrotherhood, CreateColorMapping(0, 1, 9, 1, 2, 0, 7, 1) },
            { FactionName.TheYssarilTribes, CreateColorMapping(1, 0, 0, 9, 0, 3, 1, 2) },
            { FactionName.TheCouncilKeleres, CreateColorMapping(0, 6, 6, 0, 5, 8, 7, 0) },
        };
    }

    private Dictionary<PlayerColor, int> CreateColorMapping(params int[] importances)
    {
        var colors = (PlayerColor[])Enum.GetValues(typeof(PlayerColor));
        return colors.Zip(importances, (color, importance) => new { color, importance })
                        .ToDictionary(x => x.color, x => x.importance);
    }
}
