using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

internal class PlacementStyleHandler(
    ISystemTilesForGalaxyDistributionProvider systemTilesForGalaxyDistributionProvider,
    ISystemTileSetter systemTileSetter)
    : IPlacementStyleHandler
{
    private readonly ISystemTilesForGalaxyDistributionProvider _systemTilesForGalaxyDistributionProvider = systemTilesForGalaxyDistributionProvider;
    private readonly ISystemTileSetter _systemTileSetter = systemTileSetter;

    public Task HandleRemainingPositions(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup, GenerateMapRequest request)
    {
        var remainingSystemTiles = _systemTilesForGalaxyDistributionProvider.GetRemainingSystemTilesForMapDistribution(galaxy, systemTilesForMapSetup, mapSettings, request);

        if (request.PlacementStyle == PlacementStyle.Random)
        {
            HandleRandomPlacement(galaxy, remainingSystemTiles);
        }

        return Task.CompletedTask;
    }

    private void HandleRandomPlacement(Dictionary<(int X, int Y), Hex> galaxy, SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution)
    {
        _systemTileSetter.SetRemainingSystemTilesRandomly(galaxy, systemTilesForGalaxyDistribution);
    }
}
