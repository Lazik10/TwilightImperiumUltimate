namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class FactionColorImportancesData
{
    internal static List<FactionColorImportance> FactionColorImportances => GetFactionColorImportances();

    private static List<FactionColorImportance> GetFactionColorImportances()
    {
        var factionColorMappings = GetFactionColorMappings();
        var factionColorImportances = new List<FactionColorImportance>();

        foreach (var (faction, colorMapping) in factionColorMappings)
        {
            foreach (var (color, importance) in colorMapping)
            {
                var entity = new FactionColorImportance
                {
                    FactionName = faction,
                    Color = color,
                    Importance = importance,
                };
                factionColorImportances.Add(entity);
            }
        }

        return factionColorImportances;
    }

    private static Dictionary<FactionName, Dictionary<PlayerColor, int>> GetFactionColorMappings()
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

            { FactionName.TheAugursOfIlyxum, CreateColorMapping(7, 8, 3, 0, 0, 9, 0, 0) },
            { FactionName.TheCeldauriTradeConfederation, CreateColorMapping(0, 4, 9, 0, 7, 0, 0, 2) },
            { FactionName.TheDihMohnFlotilla, CreateColorMapping(6, 0, 0, 0, 2, 10, 8, 1) },
            { FactionName.TheFlorzenProfiteers, CreateColorMapping(0, 5, 0, 7, 9, 3, 2, 0) },
            { FactionName.TheFreeSystemsCompact, CreateColorMapping(0, 0, 0, 5, 2, 8, 7, 3) },
            { FactionName.TheGheminaRaiders, CreateColorMapping(0, 0, 0, 4, 10, 5, 0, 8) },
            { FactionName.TheGlimmerOfMortheus, CreateColorMapping(5, 0, 0, 0, 7, 2, 9, 1) },
            { FactionName.TheKolleccSociety, CreateColorMapping(5, 9, 0, 2, 3, 0, 0, 7) },
            { FactionName.TheKortaliTribunal, CreateColorMapping(0, 2, 0, 3, 0, 8, 0, 10) },
            { FactionName.TheLiZhoDynasty, CreateColorMapping(2, 7, 5, 0, 0, 0, 0, 9) },
            { FactionName.TheLTokkKhrask, CreateColorMapping(0, 7, 2, 9, 5, 0, 0, 0) },
            { FactionName.TheMirvedaProtectorate, CreateColorMapping(0, 0, 0, 0, 8, 7, 10, 2) },
            { FactionName.TheMykoMentori, CreateColorMapping(0, 7, 0, 9, 3, 0, 8, 0) },
            { FactionName.TheNivynStarKings, CreateColorMapping(0, 0, 8, 4, 2, 0, 0, 9) },
            { FactionName.TheOlradinLeague, CreateColorMapping(0, 10, 8, 0, 6, 0, 0, 2) },
            { FactionName.RohDhnaMechatronics, CreateColorMapping(0, 0, 0, 0, 9, 6, 3, 8) },
            { FactionName.TheSavagesOfCymiae, CreateColorMapping(8, 9, 0, 3, 5, 0, 0, 0) },
            { FactionName.TheShipwrightsofAxis, CreateColorMapping(8, 0, 6, 2, 4, 0, 0, 10) },
            { FactionName.TheTnelisSyndicate, CreateColorMapping(5, 7, 0, 9, 2, 0, 0, 0) },
            { FactionName.TheVadenBankingClans, CreateColorMapping(0, 0, 7, 9, 5, 0, 0, 2) },
            { FactionName.TheVaylerianScourge, CreateColorMapping(3, 0, 0, 8, 9, 0, 0, 1) },
            { FactionName.TheVeldyrSovereignty, CreateColorMapping(0, 2, 0, 6, 9, 0, 0, 8) },
            { FactionName.TheZealotsOfRhodun, CreateColorMapping(0, 6, 0, 2, 8, 0, 0, 9) },
            { FactionName.TheZelianPurifier, CreateColorMapping(10, 6, 2, 0, 0, 0, 0, 8) },

            { FactionName.TheBentorConglomerate, CreateColorMapping(0, 0, 0, 5, 7, 8, 3, 0) },
            { FactionName.TheCheiranHordes, CreateColorMapping(6, 9, 2, 0, 7, 0, 0, 0) },
            { FactionName.TheEdynMandate, CreateColorMapping(1, 5, 7, 0, 0, 2, 0, 9) },
            { FactionName.TheGhotiWayfarers, CreateColorMapping(0, 6, 0, 9, 10, 2, 1, 0) },
            { FactionName.TheGledgeUnion, CreateColorMapping(0, 9, 5, 8, 2, 0, 0, 0) },
            { FactionName.TheBerserkersOfKjalengard, CreateColorMapping(7, 1, 0, 0, 9, 8, 2, 0) },
            { FactionName.TheMonksOfKolume, CreateColorMapping(0, 8, 0, 0, 5, 2, 1, 9) },
            { FactionName.TheKyroSodality, CreateColorMapping(1, 2, 7, 9, 3, 0, 0, 0) },
            { FactionName.TheLanefirRemnants, CreateColorMapping(0, 8, 1, 9, 4, 0, 0, 2) },
            { FactionName.TheNokarSellships, CreateColorMapping(0, 9, 2, 5, 8, 0, 0, 0) },
        };
    }

    private static Dictionary<PlayerColor, int> CreateColorMapping(params int[] importances)
    {
        var colors = (PlayerColor[])Enum.GetValues(typeof(PlayerColor));
        return colors.Zip(importances, (color, importance) => new { color, importance })
                        .ToDictionary(x => x.color, x => x.importance);
    }
}
