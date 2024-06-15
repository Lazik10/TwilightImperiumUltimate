namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class FactionsData
{
    internal static List<Faction> Factions => GetFactions();

    private static List<Faction> GetFactions()
    {
        var factions = new List<Faction>()
        {
            // Base Game
            new() { FactionName = FactionName.TheArborec, HomeSystem = SystemTileName.Tile05, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheBaronyOfLetnev, HomeSystem = SystemTileName.Tile10, Commodities = 2, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheClanOfSaar, HomeSystem = SystemTileName.Tile11, Commodities = 3, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheEmbersOfMuaat, HomeSystem = SystemTileName.Tile04, Commodities = 4, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheEmiratesOfHacan, HomeSystem = SystemTileName.Tile16, Commodities = 6, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheFederationOfSol, HomeSystem = SystemTileName.Tile01, Commodities = 4, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheGhostsOfCreuss, HomeSystem = SystemTileName.Tile17, Commodities = 4, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheL1z1xMindnet, HomeSystem = SystemTileName.Tile06, Commodities = 2, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheMentakCoalition, HomeSystem = SystemTileName.Tile02, Commodities = 2, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheNaaluCollective, HomeSystem = SystemTileName.Tile09, Commodities = 3, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheNekroVirus, HomeSystem = SystemTileName.Tile08, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.SardakkNorr, HomeSystem = SystemTileName.Tile13, Commodities = 3, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheUniversitiesOfJolNar, HomeSystem = SystemTileName.Tile12, Commodities = 4, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheWinnu, HomeSystem = SystemTileName.Tile07, Commodities = 3, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheXxchaKingdom, HomeSystem = SystemTileName.Tile14, Commodities = 4, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheYinBrotherhood, HomeSystem = SystemTileName.Tile03, Commodities = 2, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.BaseGame },
            new() { FactionName = FactionName.TheYssarilTribes, HomeSystem = SystemTileName.Tile15, Commodities = 3, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.BaseGame },

            // Prophecy of Kings
            new() { FactionName = FactionName.TheArgentFlight, HomeSystem = SystemTileName.Tile58, Commodities = 3, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.ProphecyOfKing },
            new() { FactionName = FactionName.TheEmpyrean, HomeSystem = SystemTileName.Tile56, Commodities = 4, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.ProphecyOfKing },
            new() { FactionName = FactionName.TheMahactGeneSorcerers, HomeSystem = SystemTileName.Tile52, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.ProphecyOfKing },
            new() { FactionName = FactionName.TheNaazRokhaAlliance, HomeSystem = SystemTileName.Tile57, Commodities = 3, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.ProphecyOfKing },
            new() { FactionName = FactionName.TheNomad, HomeSystem = SystemTileName.Tile53, Commodities = 4, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.ProphecyOfKing },
            new() { FactionName = FactionName.TheTitansOfUl, HomeSystem = SystemTileName.Tile55, Commodities = 2, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.ProphecyOfKing },
            new() { FactionName = FactionName.TheVuilRaithCabal, HomeSystem = SystemTileName.Tile54, Commodities = 2, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.ProphecyOfKing },
            new() { FactionName = FactionName.TheCouncilKeleres, HomeSystem = SystemTileName.Tile92, Commodities = 2, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.CodexVigil },
        };

        var updatedFactions = factions.Select((faction, i) =>
        {
            faction.Id = i + 1;
            faction.Action = $"{faction.FactionName}_{nameof(Faction.Action)}";
            faction.PromissaryNote = $"{faction.FactionName}_{nameof(Faction.PromissaryNote)}";
            faction.History = $"{faction.FactionName}_{nameof(Faction.History)}";
            faction.Quote = $"{faction.FactionName}_{nameof(Faction.Quote)}";
            faction.SystemStats = $"{faction.FactionName}_{nameof(Faction.SystemStats)}";
            faction.SystemInfo = $"{faction.FactionName}_{nameof(Faction.SystemInfo)}";
            return faction;
        }).ToList();

        return updatedFactions;
    }
}
