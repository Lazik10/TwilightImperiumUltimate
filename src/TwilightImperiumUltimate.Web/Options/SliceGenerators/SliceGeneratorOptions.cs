namespace TwilightImperiumUltimate.Web.Options.SliceGenerators;

public static class SliceGeneratorOptions
{
    public static readonly int NumberOfSlices = 6;

    public static readonly int NumberOfLegendaryPlanets = 2;

    public static readonly WormholeDensity WormholeDensity = WormholeDensity.Random;

    public static readonly IReadOnlyCollection<GameVersion> GameVersions = new List<GameVersion>()
    {
        GameVersion.BaseGame,
        GameVersion.ProphecyOfKings,
    };

    public static readonly SystemTileOverlay SystemTileOverlay = SystemTileOverlay.None;
}
