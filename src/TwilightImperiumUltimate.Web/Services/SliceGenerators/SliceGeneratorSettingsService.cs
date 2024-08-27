using TwilightImperiumUltimate.Web.Options.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Services.SliceGenerators;

public class SliceGeneratorSettingsService : ISliceGeneratorSettingsService
{
    private int _numberOfSlices = SliceGeneratorOptions.NumberOfSlices;

    public int NumberOfSlices => _numberOfSlices;

    public IReadOnlyCollection<GameVersion> GameVersions { get; set; } = SliceGeneratorOptions.GameVersions;

    public WormholeDensity WormholeDensity { get; set; } = SliceGeneratorOptions.WormholeDensity;

    public int NumberOfLegendaries { get; set; } = SliceGeneratorOptions.NumberOfLegendaryPlanets;

    public SystemTileOverlay SystemTileOverlay { get; set; } = SliceGeneratorOptions.SystemTileOverlay;

    public Task IncreaseNumberOfSlices()
    {
        if (_numberOfSlices < 9)
            _numberOfSlices++;

        return Task.CompletedTask;
    }

    public Task DecreaseNumberOfSlices()
    {
        if (_numberOfSlices > 0)
            _numberOfSlices--;

        return Task.CompletedTask;
    }

    public Task UpdateGameVersion(GameVersion gameVersion)
    {
        var gameVersions = GameVersions.ToList();

        if (!gameVersions.Remove(gameVersion))
            gameVersions.Add(gameVersion);

        GameVersions = gameVersions;

        int maxNumberOfLegendaryPlanets = 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.ProphecyOfKings) ? 2 : 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.UnchartedSpace) ? 5 : 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.AscendantSun) ? 12 : 0;

        if (NumberOfLegendaries > maxNumberOfLegendaryPlanets)
            NumberOfLegendaries = maxNumberOfLegendaryPlanets;

        return Task.CompletedTask;
    }

    public Task UpdateSystemTileOverlay(SystemTileOverlay systemTileOverlay)
    {
        SystemTileOverlay = systemTileOverlay;
        return Task.CompletedTask;
    }

    public Task UpdateWormholeDensity(WormholeDensity wormholeDensity)
    {
        WormholeDensity = wormholeDensity;
        return Task.CompletedTask;
    }

    public Task DecreaseNumberOfLegendaries()
    {
        if (NumberOfLegendaries > 0)
            NumberOfLegendaries--;

        return Task.CompletedTask;
    }

    public Task IncreaseNumberOfLegendaries()
    {
        int maxNumberOfLegendaryPlanets = 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.ProphecyOfKings) ? 2 : 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.UnchartedSpace) ? 5 : 0;
        maxNumberOfLegendaryPlanets += GameVersions.Contains(GameVersion.AscendantSun) ? 12 : 0;

        if (NumberOfLegendaries < maxNumberOfLegendaryPlanets)
            NumberOfLegendaries++;

        return Task.CompletedTask;
    }
}
