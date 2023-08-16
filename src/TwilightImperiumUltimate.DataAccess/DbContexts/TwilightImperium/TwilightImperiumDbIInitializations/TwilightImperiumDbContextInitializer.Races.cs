using TwilightImperiumUltimate.Core.Entities.Cards;
using TwilightImperiumUltimate.Core.Enums.Cards;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeRaces()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var races = new List<Race>()
        {
            // Base Game
            new Race()
            {
                RaceName = RaceName.TheArborec,
                HomeSystem = SystemTileName.Tile05,
                Commodities = 3,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheBaronyOfLetnev,
                HomeSystem = SystemTileName.Tile10,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheClanOfSaar,
                HomeSystem = SystemTileName.Tile11,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheEmbersOfMuaat,
                HomeSystem = SystemTileName.Tile04,
                Commodities = 4,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheEmiratesOfHacan,
                HomeSystem = SystemTileName.Tile16,
                Commodities = 6,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheFederationOfSol,
                HomeSystem = SystemTileName.Tile01,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheGhostsOfCreuss,
                HomeSystem = SystemTileName.Tile17,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheL1z1xMindnet,
                HomeSystem = SystemTileName.Tile06,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheMentacCoalition,
                HomeSystem = SystemTileName.Tile02,
                Commodities = 2,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheNaaluCollective,
                HomeSystem = SystemTileName.Tile09,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheNecroVirus,
                HomeSystem = SystemTileName.Tile08,
                Commodities = 3,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.SardakkNorr,
                HomeSystem = SystemTileName.Tile13,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheUniversitiesOfJolNar,
                HomeSystem = SystemTileName.Tile12,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheWinnu,
                HomeSystem = SystemTileName.Tile07,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheXxchaKingdom,
                HomeSystem = SystemTileName.Tile14,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheYinBrotherhood,
                HomeSystem = SystemTileName.Tile03,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },
            new Race()
            {
                RaceName = RaceName.TheYssarilTribes,
                HomeSystem = SystemTileName.Tile15,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.BaseGame,
            },

            // Prophecy of Kings
            new Race()
            {
                RaceName = RaceName.TheArgentFlight,
                HomeSystem = SystemTileName.Tile58,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Race()
            {
                RaceName = RaceName.TheEmpyrean,
                HomeSystem = SystemTileName.Tile56,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Race()
            {
                RaceName = RaceName.TheMahactGeneSorcerers,
                HomeSystem = SystemTileName.Tile52,
                Commodities = 3,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Race()
            {
                RaceName = RaceName.TheNaazRokhaAlliance,
                HomeSystem = SystemTileName.Tile57,
                Commodities = 3,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Race()
            {
                RaceName = RaceName.TheNomad,
                HomeSystem = SystemTileName.Tile53,
                Commodities = 4,
                ComplexityRating = ComplexityRating.Low,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Race()
            {
                RaceName = RaceName.TheTitansOfUl,
                HomeSystem = SystemTileName.Tile55,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Race()
            {
                RaceName = RaceName.TheVuilRaithCabal,
                HomeSystem = SystemTileName.Tile54,
                Commodities = 2,
                ComplexityRating = ComplexityRating.High,
                GameVersion = GameVersion.ProphecyOfKing,
            },
            new Race()
            {
                RaceName = RaceName.CouncilOfKeleres,
                HomeSystem = SystemTileName.TileKeleres1,
                Commodities = 2,
                ComplexityRating = ComplexityRating.Medium,
                GameVersion = GameVersion.CodexVigil,
            },
        };

        var updatedRaces = races.Select(race =>
        {
            race.Action = $"{race.RaceName}_{nameof(Race.Action)}";
            race.PromissaryNote = $"{race.RaceName}_{nameof(Race.PromissaryNote)}";
            race.History = $"{race.RaceName}_{nameof(Race.History)}";
            race.Quote = $"{race.RaceName}_{nameof(Race.Quote)}";
            race.SystemStats = $"{race.RaceName}_{nameof(Race.SystemStats)}";
            race.SystemInfo = $"{race.RaceName}_{nameof(Race.SystemInfo)}";
            return race;
        });

        dbContext.AddRange(updatedRaces);
        dbContext.SaveChanges();
    }
}
