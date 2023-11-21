namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeFactions()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var races = new List<Faction>()
        {
            // Base Game
            new Faction()
            {
                FactionName = FactionName.TheArborec,
                HomeSystem = SystemTileName.Tile05,
                Commodities = 3,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheBaronyOfLetnev,
                HomeSystem = SystemTileName.Tile10,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheClanOfSaar,
                HomeSystem = SystemTileName.Tile11,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheEmbersOfMuaat,
                HomeSystem = SystemTileName.Tile04,
                Commodities = 4,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheEmiratesOfHacan,
                HomeSystem = SystemTileName.Tile16,
                Commodities = 6,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheFederationOfSol,
                HomeSystem = SystemTileName.Tile01,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheGhostsOfCreuss,
                HomeSystem = SystemTileName.Tile17,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheL1z1xMindnet,
                HomeSystem = SystemTileName.Tile06,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheMentakCoalition,
                HomeSystem = SystemTileName.Tile02,
                Commodities = 2,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheNaaluCollective,
                HomeSystem = SystemTileName.Tile09,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheNekroVirus,
                HomeSystem = SystemTileName.Tile08,
                Commodities = 3,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.SardakkNorr,
                HomeSystem = SystemTileName.Tile13,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheUniversitiesOfJolNar,
                HomeSystem = SystemTileName.Tile12,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheWinnu,
                HomeSystem = SystemTileName.Tile07,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheXxchaKingdom,
                HomeSystem = SystemTileName.Tile14,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheYinBrotherhood,
                HomeSystem = SystemTileName.Tile03,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Faction()
            {
                FactionName = FactionName.TheYssarilTribes,
                HomeSystem = SystemTileName.Tile15,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },

            // Prophecy of Kings
            new Faction()
            {
                FactionName = FactionName.TheArgentFlight,
                HomeSystem = SystemTileName.Tile58,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Faction()
            {
                FactionName = FactionName.TheEmpyrean,
                HomeSystem = SystemTileName.Tile56,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Faction()
            {
                FactionName = FactionName.TheMahactGeneSorcerers,
                HomeSystem = SystemTileName.Tile52,
                Commodities = 3,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Faction()
            {
                FactionName = FactionName.TheNaazRokhaAlliance,
                HomeSystem = SystemTileName.Tile57,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Faction()
            {
                FactionName = FactionName.TheNomad,
                HomeSystem = SystemTileName.Tile53,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Faction()
            {
                FactionName = FactionName.TheTitansOfUl,
                HomeSystem = SystemTileName.Tile55,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Faction()
            {
                FactionName = FactionName.TheVuilRaithCabal,
                HomeSystem = SystemTileName.Tile54,
                Commodities = 2,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Faction()
            {
                FactionName = FactionName.TheCouncilKeleres,
                HomeSystem = SystemTileName.Tile92,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.CodexVigil,
            },
        };

        var updatedRaces = races.Select(race =>
        {
            race.Action = $"{race.FactionName}_{nameof(Faction.Action)}";
            race.PromissaryNote = $"{race.FactionName}_{nameof(Faction.PromissaryNote)}";
            race.History = $"{race.FactionName}_{nameof(Faction.History)}";
            race.Quote = $"{race.FactionName}_{nameof(Faction.Quote)}";
            race.SystemStats = $"{race.FactionName}_{nameof(Faction.SystemStats)}";
            race.SystemInfo = $"{race.FactionName}_{nameof(Faction.SystemInfo)}";
            return race;
        });

        dbContext.AddRange(updatedRaces);
        dbContext.SaveChanges();
    }
}
