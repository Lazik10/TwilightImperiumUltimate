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

    public static readonly IReadOnlyCollection<GameTrackerPlayerModel> TestGameTrackerPlayerModels = new List<GameTrackerPlayerModel>
    {
        new GameTrackerPlayerModel
        {
            Id = 0,
            DefaultName = "Player 1",
            Name = "Lazik",
            PlayerColor = PlayerColor.Red,
            FactionName = FactionName.TheBaronyOfLetnev,
            Initiative = InitiativeOrder.First,
            Score = 0,
        },
        new GameTrackerPlayerModel
        {
            Id = 1,
            DefaultName = "Player 2",
            Name = "Ondra",
            PlayerColor = PlayerColor.Blue,
            FactionName = FactionName.TheNekroVirus,
            Initiative = InitiativeOrder.First,
            Score = 0,
        },
        new GameTrackerPlayerModel
        {
            Id = 2,
            DefaultName = "Player 3",
            Name = "Kuba",
            PlayerColor = PlayerColor.Green,
            FactionName = FactionName.TheNaaluCollective,
            Initiative = InitiativeOrder.First,
            Score = 0,
        },
        new GameTrackerPlayerModel
        {
            Id = 3,
            DefaultName = "Player 4",
            Name = "Chamiel",
            PlayerColor = PlayerColor.Yellow,
            FactionName = FactionName.TheEmiratesOfHacan,
            Initiative = InitiativeOrder.First,
            Score = 0,
        },
        new GameTrackerPlayerModel
        {
            Id = 4,
            DefaultName = "Player 5",
            Name = "Beebee",
            PlayerColor = PlayerColor.Purple,
            FactionName = FactionName.TheEmpyrean,
            Initiative = InitiativeOrder.First,
            Score = 0,
        },
        new GameTrackerPlayerModel
        {
            Id = 5,
            DefaultName = "Player 6",
            Name = "Xmarkx",
            PlayerColor = PlayerColor.Orange,
            FactionName = FactionName.TheGhostsOfCreuss,
            Initiative = InitiativeOrder.First,
            Score = 0,
        },
    };

    public static readonly IReadOnlyCollection<GameTrackerPlayerModel> DefaultGameTrackerPlayerModels = new List<GameTrackerPlayerModel>
    {
        new GameTrackerPlayerModel { Id = 0, DefaultName = "Player 1", FactionName = FactionName.None, Initiative = InitiativeOrder.First, Score = 0 },
        new GameTrackerPlayerModel { Id = 1, DefaultName = "Player 2", FactionName = FactionName.None, Initiative = InitiativeOrder.First, Score = 0 },
        new GameTrackerPlayerModel { Id = 2, DefaultName = "Player 3", FactionName = FactionName.None, Initiative = InitiativeOrder.First, Score = 0 },
        new GameTrackerPlayerModel { Id = 3, DefaultName = "Player 4", FactionName = FactionName.None, Initiative = InitiativeOrder.First, Score = 0 },
        new GameTrackerPlayerModel { Id = 4, DefaultName = "Player 5", FactionName = FactionName.None, Initiative = InitiativeOrder.First, Score = 0 },
        new GameTrackerPlayerModel { Id = 5, DefaultName = "Player 6", FactionName = FactionName.None, Initiative = InitiativeOrder.First, Score = 0 },
    };
}
