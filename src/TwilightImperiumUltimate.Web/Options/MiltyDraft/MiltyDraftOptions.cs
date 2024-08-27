namespace TwilightImperiumUltimate.Web.Options.MiltyDraft;

public static class MiltyDraftOptions
{
    public static readonly MiltyDraftState State = MiltyDraftState.Initialized;

    public static readonly int NumberOfPlayers = 6;

    public static readonly int NumberOfSlices = 7;

    public static readonly int NumberOfFactions = 9;

    public static readonly int NumberOfLegendaryPlanets = 2;

    public static readonly int MaxNumberOfSlices = 9;

    public static readonly int MinNumberOfSlices = 3;

    public static readonly int MaxNumberOfPlayers = 8;

    public static readonly int MinNumberOfPlayers = 3;

    public static readonly bool EnablePlayerNames;

    public static readonly bool ImportSlices;

    public static readonly WormholeDensity WormholeDensity = WormholeDensity.Random;

    public static IReadOnlyList<GameVersion> GameVersions { get; set; } = [GameVersion.BaseGame, GameVersion.ProphecyOfKings];

    public static IReadOnlyCollection<MiltyDraftInitiativeModel> Initiatives => new List<MiltyDraftInitiativeModel>()
    {
        new() { Initiative = MiltyDraftInitiative.Speaker },
        new() { Initiative = MiltyDraftInitiative.Second },
        new() { Initiative = MiltyDraftInitiative.Third, },
        new() { Initiative = MiltyDraftInitiative.Fourth, },
        new() { Initiative = MiltyDraftInitiative.Fifth, },
        new() { Initiative = MiltyDraftInitiative.Sixth, },
        new() { Initiative = MiltyDraftInitiative.Seventh, },
        new() { Initiative = MiltyDraftInitiative.Eighth, },
    };

    public static IReadOnlyList<MiltyDraftPlayerModel> MiltyDraftPlayers { get; set; } = new List<MiltyDraftPlayerModel>
    {
        new MiltyDraftPlayerModel() { PlayerId = 0, PlayerName = "Player 1", PlayerDefaultName = "Player 1" },
        new MiltyDraftPlayerModel() { PlayerId = 1, PlayerName = "Player 2", PlayerDefaultName = "Player 2" },
        new MiltyDraftPlayerModel() { PlayerId = 2, PlayerName = "Player 3", PlayerDefaultName = "Player 3" },
        new MiltyDraftPlayerModel() { PlayerId = 3, PlayerName = "Player 4", PlayerDefaultName = "Player 4" },
        new MiltyDraftPlayerModel() { PlayerId = 4, PlayerName = "Player 5", PlayerDefaultName = "Player 5" },
        new MiltyDraftPlayerModel() { PlayerId = 5, PlayerName = "Player 6", PlayerDefaultName = "Player 6" },
    };
}
