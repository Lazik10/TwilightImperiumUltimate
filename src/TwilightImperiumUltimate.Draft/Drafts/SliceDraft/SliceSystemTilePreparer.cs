using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

public class SliceSystemTilePreparer : ISliceSystemTilePreparer
{
    private static readonly Random _random = new Random();

    public async Task<SystemTilesForSlices> PrepareSystemTilesForSlices(
        SystemTilesForMapSetup systemTilesForMapSetup,
        SystemTilesForSlices systemTilesForSliceRedistribution,
        SliceDraftRequest request)
    {
        ArgumentNullException.ThrowIfNull(systemTilesForMapSetup);
        ArgumentNullException.ThrowIfNull(request);

        if (request.WormholeDensity != WormholeDensity.Random)
        {
            await PrepareWormholes(request.WormholeDensity, systemTilesForSliceRedistribution, systemTilesForMapSetup);
        }

        if (request.NumberOfLegendaries > 0)
        {
            await PrepareLegendaries(request.NumberOfLegendaries, systemTilesForSliceRedistribution, systemTilesForMapSetup);
        }

        await PrepareRemainingRedTiles(request.NumberOfSlices, systemTilesForSliceRedistribution, systemTilesForMapSetup);
        await PrepareRemainingBlueTiles(request.NumberOfSlices, systemTilesForSliceRedistribution, systemTilesForMapSetup);

        return systemTilesForSliceRedistribution;
    }

    public Task<SystemTilesForSlices> PrepareRemainingBlueTiles(
        int numberOfSlices,
        SystemTilesForSlices systemTilesForSliceRedistribution,
        SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var alreadyDraftedBlueTilesCount = systemTilesForSliceRedistribution.BlueSystemTiles.Count
            + systemTilesForSliceRedistribution.LegendarySystemTiles.Count(x => x.TileCategory == SystemTileCategory.Blue)
            + systemTilesForSliceRedistribution.WormholeSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Blue);

        var legendaryBlueTilesCodes = systemTilesForSliceRedistribution.LegendarySystemTiles.Where(x => x.TileCategory == SystemTileCategory.Blue).Select(x => x.SystemTileCode).ToList();
        var womrholeBlueTilesCodes = systemTilesForSliceRedistribution.WormholeSystemTiles.Where(x => x.TileCategory == SystemTileCategory.Blue).Select(x => x.SystemTileCode).ToList();

        var blueTiles = systemTilesForMapSetup.BlueTiles
            .Where(x => !legendaryBlueTilesCodes.Contains(x.SystemTileCode) && !womrholeBlueTilesCodes.Contains(x.SystemTileCode))
            .OrderBy(x => _random.Next())
            .Take((numberOfSlices * 3) - alreadyDraftedBlueTilesCount)
            .ToList();

        systemTilesForSliceRedistribution.BlueSystemTiles = systemTilesForSliceRedistribution.BlueSystemTiles.Concat(blueTiles).ToList();

        return Task.FromResult(systemTilesForSliceRedistribution);
    }

    public Task<SystemTilesForSlices> PrepareRemainingRedTiles(
        int numberOfSlices,
        SystemTilesForSlices systemTilesForSliceRedistribution,
        SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var alreadyDraftedRedTiles = systemTilesForSliceRedistribution.RedSystemTiles.Count
            + systemTilesForSliceRedistribution.LegendarySystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red)
            + systemTilesForSliceRedistribution.WormholeSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red);

        var legendaryRedTilesCodes = systemTilesForSliceRedistribution.LegendarySystemTiles.Where(x => x.TileCategory == SystemTileCategory.Red).Select(x => x.SystemTileCode).ToList();
        var womrholeRedTilesCodes = systemTilesForSliceRedistribution.WormholeSystemTiles.Where(x => x.TileCategory == SystemTileCategory.Red).Select(x => x.SystemTileCode).ToList();

        var redTiles = systemTilesForMapSetup.RedTiles
            .Where(x => !legendaryRedTilesCodes.Contains(x.SystemTileCode) && !womrholeRedTilesCodes.Contains(x.SystemTileCode))
            .OrderBy(x => _random.Next())
            .Take((numberOfSlices * 2) - alreadyDraftedRedTiles)
            .ToList();

        systemTilesForSliceRedistribution.RedSystemTiles = systemTilesForSliceRedistribution.RedSystemTiles.Concat(redTiles).ToList();

        return Task.FromResult(systemTilesForSliceRedistribution);
    }

    public Task<SystemTilesForSlices> PrepareLegendaries(
        int numberOfLegendaries,
        SystemTilesForSlices systemTilesForSliceRedistribution,
        SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var legendarySystemTiles = systemTilesForMapSetup.BlueTiles
            .Concat(systemTilesForMapSetup.RedTiles)
            .Where(x => x.HasLegendaryPlanet)
            .AsEnumerable()
            .OrderBy(x => _random.Next())
            .Take(numberOfLegendaries)
            .ToList();

        systemTilesForSliceRedistribution.LegendarySystemTiles = legendarySystemTiles;

        return Task.FromResult(systemTilesForSliceRedistribution);
    }

    public Task<SystemTilesForSlices> PrepareWormholes(WormholeDensity wormholeDensity, SystemTilesForSlices systemTilesForSliceRedistribution, SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var allSystemTiles = systemTilesForMapSetup.BlueTiles.Concat(systemTilesForMapSetup.RedTiles).ToList();
        var selectedWormholes = new List<SystemTile>();

        var wormholeTiles = allSystemTiles
            .Where(x => x.HasWormholes)
            .OrderBy(x => _random.Next())
            .ToList();

        var selectedWormholeType = _random.Next(0, 2) == 0 ? WormholeName.Alpha : WormholeName.Beta;

        if (wormholeDensity == WormholeDensity.AtLeastOnePair)
        {
            selectedWormholes = wormholeTiles
                .Where(x => x.Wormholes.Any(x => x.WormholeName == selectedWormholeType))
                .Take(2)
                .ToList();
        }
        else if (wormholeDensity == WormholeDensity.AtLeastTwoPairs)
        {
            var alphaWormholes = wormholeTiles
                .Where(x => x.Wormholes.Any(x => x.WormholeName == WormholeName.Alpha))
                .Take(2)
                .ToList();

            var betaWormholes = wormholeTiles
                .Where(x => x.Wormholes.Any(x => x.WormholeName == WormholeName.Beta))
                .Take(2)
                .ToList();

            selectedWormholes = alphaWormholes.Concat(betaWormholes).ToList();
        }

        systemTilesForSliceRedistribution.WormholeSystemTiles = selectedWormholes.ToList();

        return Task.FromResult(systemTilesForSliceRedistribution);
    }
}
