using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Draft;

public record SliceDraftRequest(
    bool PreviewSlices,
    int NumberOfSlices,
    IReadOnlyCollection<GameVersion> GameVersions,
    WormholeDensity WormholeDensity,
    int NumberOfLegendaries);
