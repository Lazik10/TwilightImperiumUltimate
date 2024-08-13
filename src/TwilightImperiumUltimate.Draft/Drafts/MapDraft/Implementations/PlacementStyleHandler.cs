using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Constants;
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
    private readonly Random _random = new Random();

    public async Task HandleRemainingPositions(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup, GenerateMapRequest request)
    {
        var remainingSystemTiles = _systemTilesForGalaxyDistributionProvider.GetRemainingSystemTilesForMapDistribution(galaxy, systemTilesForMapSetup, mapSettings, request);

        if (request.LegendaryPriorityInEquidistant)
            _systemTileSetter.SetLegendarySystemTiles(galaxy, remainingSystemTiles, mapSettings);

        var balancedSlices = await _sliceBalancer.BalanceSlices(galaxy, mapSettings, systemTilesForMapSetup, remainingSystemTiles, request);

        _systemTileSetter.SetRemainingSystemTiles(galaxy, mapSettings, request, balancedSlices);
    }

    public Task HandleRemainingNonSlicePositions(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup)
    {
        if (galaxy.Values.All(x => x.SystemTile is not null || (x.SystemTile is null && x.Name == PositionName.Empty)))
            return Task.CompletedTask;

        var galaxyUsedTiles = galaxy.Values
            .Where(x => x.SystemTile is not null)
            .OrderBy(x => _random.Next())
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToHashSet();

        var availableRemainingSystemTiles = systemTilesForMapSetup.BlueTiles
            .Where(x => !galaxyUsedTiles.Contains(x.SystemTileCode))
            .ToList();

        var countNeedToFill = galaxy.Values.Count(x => x.SystemTile is null && x.Name != PositionName.Empty);

        foreach (var hex in galaxy.Values.Where(x => x.SystemTile is null && x.Name != PositionName.Empty))
        {
            hex.SystemTile = availableRemainingSystemTiles[0];
            availableRemainingSystemTiles.RemoveAt(0);
        }

        return Task.CompletedTask;
    }
}
