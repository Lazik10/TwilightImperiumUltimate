namespace TwilightImperiumUltimate.Web.Services.SliceGenerators;

public interface ISliceGeneratorSettingsService
{
    int NumberOfSlices { get; }

    IReadOnlyCollection<GameVersion> GameVersions { get; }

    WormholeDensity WormholeDensity { get; }

    SystemTileOverlay SliceOverlay { get; }

    SystemTileOverlay MenuOverlay { get; }

    int NumberOfLegendaries { get; }

    Task IncreaseNumberOfSlices();

    Task DecreaseNumberOfSlices();

    Task SetNumberOfSlices(int numberOfSlices);

    Task UpdateGameVersion(GameVersion gameVersion);

    Task UpdateSliceOverlay(SystemTileOverlay systemTileOverlay);

    Task UpdateMenuOverlay(SystemTileOverlay systemTileOverlay);

    Task UpdateWormholeDensity(WormholeDensity wormholeDensity);

    Task DecreaseNumberOfLegendaries();

    Task IncreaseNumberOfLegendaries();
}
