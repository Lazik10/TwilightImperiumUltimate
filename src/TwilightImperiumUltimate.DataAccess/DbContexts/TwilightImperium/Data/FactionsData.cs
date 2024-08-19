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
            new() { FactionName = FactionName.TheArgentFlight, HomeSystem = SystemTileName.Tile58, Commodities = 3, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.ProphecyOfKings },
            new() { FactionName = FactionName.TheEmpyrean, HomeSystem = SystemTileName.Tile56, Commodities = 4, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.ProphecyOfKings },
            new() { FactionName = FactionName.TheMahactGeneSorcerers, HomeSystem = SystemTileName.Tile52, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.ProphecyOfKings },
            new() { FactionName = FactionName.TheNaazRokhaAlliance, HomeSystem = SystemTileName.Tile57, Commodities = 3, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.ProphecyOfKings },
            new() { FactionName = FactionName.TheNomad, HomeSystem = SystemTileName.Tile53, Commodities = 4, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.ProphecyOfKings },
            new() { FactionName = FactionName.TheTitansOfUl, HomeSystem = SystemTileName.Tile55, Commodities = 2, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.ProphecyOfKings },
            new() { FactionName = FactionName.TheVuilRaithCabal, HomeSystem = SystemTileName.Tile54, Commodities = 2, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.ProphecyOfKings },
            new() { FactionName = FactionName.TheCouncilKeleres, HomeSystem = SystemTileName.Tile92, Commodities = 2, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.CodexVigil },

            // Discordant Stars
            new() { FactionName = FactionName.TheAugursOfIlyxum, HomeSystem = SystemTileName.Tile1001, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheBentorConglomerate, HomeSystem = SystemTileName.Tile1002, Commodities = 2, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheBerserkersOfKjalengard, HomeSystem = SystemTileName.Tile1003, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheCeldauriTradeConfederation, HomeSystem = SystemTileName.Tile1004, Commodities = 4, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheCheiranHordes, HomeSystem = SystemTileName.Tile1005, Commodities = 2, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheDihMohnFlotilla, HomeSystem = SystemTileName.Tile1006, Commodities = 2, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheEdynMandate, HomeSystem = SystemTileName.Tile1007, Commodities = 3, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheFlorzenProfiteers, HomeSystem = SystemTileName.Tile1008, Commodities = 4, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheFreeSystemsCompact, HomeSystem = SystemTileName.Tile1009, Commodities = 4, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheGheminaRaiders, HomeSystem = SystemTileName.Tile1010, Commodities = 2, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheGhotiWayfarers, HomeSystem = SystemTileName.Tile1011, Commodities = 4, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheGledgeUnion, HomeSystem = SystemTileName.Tile1012, Commodities = 2, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheGlimmerOfMortheus, HomeSystem = SystemTileName.Tile1013, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheKolleccSociety, HomeSystem = SystemTileName.Tile1014, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheKortaliTribunal, HomeSystem = SystemTileName.Tile1015, Commodities = 3, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheKyroSodality, HomeSystem = SystemTileName.Tile1016, Commodities = 2, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheLanefirRemnants, HomeSystem = SystemTileName.Tile1017, Commodities = 2, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheLiZhoDynasty, HomeSystem = SystemTileName.Tile1018, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheLTokkKhrask, HomeSystem = SystemTileName.Tile1019, Commodities = 2, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheMirvedaProtectorate, HomeSystem = SystemTileName.Tile1020, Commodities = 3, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheMonksOfKolume, HomeSystem = SystemTileName.Tile1021, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheMykoMentori, HomeSystem = SystemTileName.Tile1022, Commodities = 1, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheNivynStarKings, HomeSystem = SystemTileName.Tile1023, Commodities = 3, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheNokarSellships, HomeSystem = SystemTileName.Tile1024, Commodities = 4, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheOlradinLeague, HomeSystem = SystemTileName.Tile1025, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.RohDhnaMechatronics, HomeSystem = SystemTileName.Tile1026, Commodities = 4, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheSavagesOfCymiae, HomeSystem = SystemTileName.Tile1027, Commodities = 3, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheShipwrightsofAxis, HomeSystem = SystemTileName.Tile1028, Commodities = 5, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheTnelisSyndicate, HomeSystem = SystemTileName.Tile1029, Commodities = 2, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheVadenBankingClans, HomeSystem = SystemTileName.Tile1030, Commodities = 3, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheVaylerianScourge, HomeSystem = SystemTileName.Tile1031, Commodities = 2, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheVeldyrSovereignty, HomeSystem = SystemTileName.Tile1032, Commodities = 4, ComplexityRating = ComplexityRating.Low, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheZealotsOfRhodun, HomeSystem = SystemTileName.Tile1033, Commodities = 3, ComplexityRating = ComplexityRating.High, GameVersion = GameVersion.DiscordantStars },
            new() { FactionName = FactionName.TheZelianPurifier, HomeSystem = SystemTileName.Tile1034, Commodities = 2, ComplexityRating = ComplexityRating.Medium, GameVersion = GameVersion.DiscordantStars },
        };

        var updatedFactions = factions.Select((faction, i) =>
        {
            faction.Id = i + 1;
            faction.Action = $"{faction.FactionName}_{nameof(Faction.Action)}";
            faction.History = $"{faction.FactionName}_{nameof(Faction.History)}";
            faction.Quote = $"{faction.FactionName}_{nameof(Faction.Quote)}";
            faction.SystemStats = $"{faction.FactionName}_{nameof(Faction.SystemStats)}";
            faction.SystemInfo = $"{faction.FactionName}_{nameof(Faction.SystemInfo)}";
            return faction;
        }).ToList();

        return updatedFactions;
    }
}
