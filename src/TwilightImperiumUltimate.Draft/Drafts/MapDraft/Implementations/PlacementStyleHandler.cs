using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

internal class PlacementStyleHandler(
    ISystemTilesForGalaxyDistributionProvider systemTilesForGalaxyDistributionProvider,
    ISystemTileSetter systemTileSetter,
    ISliceBalancer sliceBalancer)
    : IPlacementStyleHandler
{
    private readonly ISystemTilesForGalaxyDistributionProvider _systemTilesForGalaxyDistributionProvider = systemTilesForGalaxyDistributionProvider;
    private readonly ISystemTileSetter _systemTileSetter = systemTileSetter;
    private readonly ISliceBalancer _sliceBalancer = sliceBalancer;

    public async Task HandleRemainingPositions(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup, GenerateMapRequest request)
    {
        var remainingSystemTiles = _systemTilesForGalaxyDistributionProvider.GetRemainingSystemTilesForMapDistribution(galaxy, systemTilesForMapSetup, mapSettings, request);

        if (request.LegendaryPriorityInEquidistant)
            _systemTileSetter.SetLegendarySystemTiles(galaxy, remainingSystemTiles, mapSettings);

        var balancedSlices = await _sliceBalancer.BalanceSlices(galaxy, mapSettings, systemTilesForMapSetup, remainingSystemTiles, request);

        _systemTileSetter.SetRemainingSystemTiles(galaxy, mapSettings, request, balancedSlices);
    }
}
