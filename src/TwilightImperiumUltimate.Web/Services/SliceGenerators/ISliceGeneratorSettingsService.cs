namespace TwilightImperiumUltimate.Web.Services.SliceGenerators;

public interface ISliceGeneratorSettingsService
{
    int NumberOfSlices { get; }

    IReadOnlyCollection<GameVersion> GameVersions { get; }

    WormholeDensity WormholeDensity { get; }

    SystemTileOverlay SystemTileOverlay { get; }

    int NumberOfLegendaries { get; }

    Task IncreaseNumberOfSlices();

    Task DecreaseNumberOfSlices();

    Task UpdateGameVersion(GameVersion gameVersion);

    Task UpdateSystemTileOverlay(SystemTileOverlay systemTileOverlay);

    Task UpdateWormholeDensity(WormholeDensity wormholeDensity);

    Task DecreaseNumberOfLegendaries();

    Task IncreaseNumberOfLegendaries();
}
