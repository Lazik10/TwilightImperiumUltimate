using TwilightImperiumUltimate.Core.Entities.Galaxy;
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

    public async Task HandleRemainingPositions(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup, GenerateMapRequest request)
    {
        var remainingSystemTiles = _systemTilesForGalaxyDistributionProvider.GetRemainingSystemTilesForMapDistribution(galaxy, systemTilesForMapSetup, mapSettings, request);

        if (request.LegendaryPriorityInEquidistant)
            _systemTileSetter.SetLegendarySystemTiles(galaxy, remainingSystemTiles, mapSettings);

        var result = await _sliceBalancer.BalanceSlices(galaxy, mapSettings, systemTilesForMapSetup, remainingSystemTiles, request);

        _systemTileSetter.SetRemainingSystemTiles(galaxy, mapSettings, request, result.Slices);

        // This is a safe guard to assign all prepared system tiles that were not assigned to any slice
        HandleRemainingNonSlicePositionsWithUnusedSystemTiles(galaxy, result.UnusesSystemTiles);
    }

    public Task HandleRemainingNonSlicePositions(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup)
    {
        if (galaxy.Values.All(x => x.SystemTile is not null || (x.SystemTile is null && x.Name == PositionName.Empty)))
            return Task.CompletedTask;

        var galaxyUsedTiles = galaxy.Values
            .Where(x => x.SystemTile is not null)
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToHashSet();

        var availableRemainingSystemTiles = systemTilesForMapSetup.BlueTiles
            .Where(x => !galaxyUsedTiles.Contains(x.SystemTileCode))
            .ToList();

        var avalilableRemainingRedSystemTiles = systemTilesForMapSetup.RedTiles
            .Where(x => !galaxyUsedTiles.Contains(x.SystemTileCode))
            .ToList();

        // Add remaining red tiles so we have enough tiles to fill the galaxy
        if (mapSettings.MapTemplate == MapTemplate.SixPlayersLargeMap)
        {
            availableRemainingSystemTiles.AddRange(avalilableRemainingRedSystemTiles);
        }

        foreach (var hex in galaxy.Values.Where(x => x.SystemTile is null && x.Name != PositionName.Empty))
        {
            hex.SystemTile = availableRemainingSystemTiles[0];
            availableRemainingSystemTiles.RemoveAt(0);
        }

        return Task.CompletedTask;
    }

    private void HandleRemainingNonSlicePositionsWithUnusedSystemTiles(
        Dictionary<(int X, int Y),
        Hex> galaxy,
        List<SystemTile> unusedSystemTilesForGalaxyDistribution)
    {
        if (galaxy.Values.All(x => x.SystemTile is not null || (x.SystemTile is null && x.Name == PositionName.Empty)))
            return;

        foreach (var hex in galaxy.Values.Where(x => x.SystemTile is null && x.Name != PositionName.Empty))
        {
            if (unusedSystemTilesForGalaxyDistribution.Count == 0)
                break;

            hex.SystemTile = unusedSystemTilesForGalaxyDistribution[0];
            unusedSystemTilesForGalaxyDistribution.RemoveAt(0);
        }
    }
}
