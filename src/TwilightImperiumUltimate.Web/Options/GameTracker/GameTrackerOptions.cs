using TwilightImperiumUltimate.Web.Models.GameTracker;

namespace TwilightImperiumUltimate.Web.Options.GameTracker;

public static class GameTrackerOptions
{
    public static readonly int NumberOfPlayers = 6;

    public static readonly int NumberOfPoints = 10;

    public static readonly int MinimumNumberOfPoints = 1;

    public static readonly int MaximumNumberOfPoints = 24;

    public static readonly bool EnablePlayerNames;

    public static readonly IReadOnlyCollection<GameVersion> GameVersions = new List<GameVersion>()
    {
        GameVersion.BaseGame,
        GameVersion.ProphecyOfKings,
    };

    public static readonly IReadOnlyCollection<GameTrackerPlayerModel> DefaultGameTrackerPlayerModels = new List<GameTrackerPlayerModel>
    {
        new GameTrackerPlayerModel { Id = 0, DefaultName = "Player 1", FactionName = FactionName.None, Initiative = InitiativeOrder.First, Score = 0 },
        new GameTrackerPlayerModel { Id = 1, DefaultName = "Player 2", FactionName = FactionName.None, Initiative = InitiativeOrder.Second, Score = 0 },
        new GameTrackerPlayerModel { Id = 2, DefaultName = "Player 3", FactionName = FactionName.None, Initiative = InitiativeOrder.Third, Score = 0 },
        new GameTrackerPlayerModel { Id = 3, DefaultName = "Player 4", FactionName = FactionName.None, Initiative = InitiativeOrder.Fourth, Score = 0 },
        new GameTrackerPlayerModel { Id = 4, DefaultName = "Player 5", FactionName = FactionName.None, Initiative = InitiativeOrder.Fifth, Score = 0 },
        new GameTrackerPlayerModel { Id = 5, DefaultName = "Player 6", FactionName = FactionName.None, Initiative = InitiativeOrder.Sixth, Score = 0 },
    };
}
