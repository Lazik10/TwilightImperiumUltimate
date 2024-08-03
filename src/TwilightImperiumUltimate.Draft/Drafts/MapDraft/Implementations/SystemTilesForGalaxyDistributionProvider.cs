using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Constants;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

internal class SystemTilesForGalaxyDistributionProvider(
    ILogger<SystemTilesForGalaxyDistributionProvider> logger)
    : ISystemTilesForGalaxyDistributionProvider
{
    private readonly Random _random = new Random();
    private readonly ILogger<SystemTilesForGalaxyDistributionProvider> _logger = logger;

    public SystemTilesForGalaxyDistribution GetRemainingSystemTilesForMapDistribution(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        IMapSettings mapSettings,
        GenerateMapRequest request)
    {
        var systemTilesForGalaxyDistribution = new SystemTilesForGalaxyDistribution();

        SatisfyWormholes(galaxy, systemTilesForMapSetup, systemTilesForGalaxyDistribution, request);
        SatisfyLegendaries(systemTilesForMapSetup, systemTilesForGalaxyDistribution, request);
        SatisfyRemainingRedTilesCount(galaxy, systemTilesForMapSetup, systemTilesForGalaxyDistribution, mapSettings);
        SatisfyRemainingBlueTilesCount(galaxy, systemTilesForMapSetup, systemTilesForGalaxyDistribution);

        return systemTilesForGalaxyDistribution;
    }

    private void SatisfyRemainingBlueTilesCount(Dictionary<(int X, int Y), Hex> galaxy, SystemTilesForMapSetup systemTilesForMapSetup, SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution)
    {
        var requiredRemainingBlueTilesCount = galaxy.Values
            .Count(hex => hex.Name != PositionName.Empty && hex.Name != PositionName.Red && hex.Name != PositionName.Home && hex.Name != PositionName.MecatolRex);

        requiredRemainingBlueTilesCount -= systemTilesForGalaxyDistribution.BlueTiles.Count;
        requiredRemainingBlueTilesCount -= systemTilesForGalaxyDistribution.RedTiles.Count;

        var alreadyPickedBlueTiles = systemTilesForGalaxyDistribution.BlueTiles.Select(x => x.SystemTileCode).ToList();

        var remainingBlueTiles = systemTilesForMapSetup.BlueTiles
            .Where(x => !x.HasLegendaryPlanet && !alreadyPickedBlueTiles.Contains(x.SystemTileCode))
            .OrderBy(x => _random.Next())
            .Take(requiredRemainingBlueTilesCount)
            .ToList();

        systemTilesForGalaxyDistribution.BlueTiles.AddRange(remainingBlueTiles);
    }

    private void SatisfyRemainingRedTilesCount(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution,
        IMapSettings mappSettings)
    {
        var galaxyRedTiles = galaxy.Values
            .Where(hex => hex.SystemTile is not null && hex.SystemTile.TileCategory == SystemTileCategory.Red)
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToList();

        var alreadySelectedRedTiles = systemTilesForGalaxyDistribution.RedTiles
            .Select(x => x.SystemTileCode)
            .ToList();

        var redTilesToChooseFrom = systemTilesForMapSetup.RedTiles
            .Where(x => !galaxyRedTiles.Contains(x.SystemTileCode)
                && !alreadySelectedRedTiles.Contains(x.SystemTileCode)
                && !x.HasAnomaly)
            .ToList();

        var selectedRemainingRedTiles = redTilesToChooseFrom
            .OrderBy(x => _random.Next())
            .Take((mappSettings.NumberOfPlayers * 2) - galaxyRedTiles.Count)
            .ToList();

        systemTilesForGalaxyDistribution.RedTiles.AddRange(selectedRemainingRedTiles);
    }

    private void SatisfyLegendaries(SystemTilesForMapSetup systemTilesForMapSetup, SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution, GenerateMapRequest request)
    {
        var legendaryTiles = systemTilesForMapSetup.BlueTiles
            .Where(x => x.HasLegendaryPlanet)
            .ToList();

        systemTilesForGalaxyDistribution.BlueTiles
            .AddRange(legendaryTiles.Take(Math.Min(legendaryTiles.Count, request.NumberOfLegendaries)));

        _logger.LogInformation("Selected legendary tiles: {Count} with codes: {Codes}", legendaryTiles.Count, string.Join(",", legendaryTiles.Select(x => x.SystemTileCode)));
    }

    private void SatisfyWormholes(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution,
        GenerateMapRequest request)
    {
        if (request.WormholesDensity == WormholeDensity.None || request.WormholesDensity == WormholeDensity.Random)
            return;

        var presentAlphaWormholeTiles = galaxy.Values.Where(hex =>
            hex.SystemTile is not null
            && hex.SystemTile.Wormholes.Any(x => x.WormholeName == WormholeName.Alpha))
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToList();

        _logger.LogInformation(
            "Galaxy has {PresentAlphaWormholeTiles} alpha wormhole tiles already: {SystemTileCodes}",
            presentAlphaWormholeTiles.Count,
            string.Join(",", presentAlphaWormholeTiles));

        var presentBetaWormholeTiles = galaxy.Values.Where(hex =>
            hex.SystemTile is not null
            && hex.SystemTile.Wormholes.Any(x => x.WormholeName == WormholeName.Alpha))
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToList();

        _logger.LogInformation(
            "Galaxy has {PresentBetaWormholeTiles} beta wormhole tiles already: {SystemTileCodes}",
            presentBetaWormholeTiles.Count,
            string.Join(",", presentBetaWormholeTiles));

        var allWormholeTiles = systemTilesForMapSetup.RedTiles
            .Concat(systemTilesForMapSetup.BlueTiles)
            .Where(x => x.HasWormholes && !x.HasAnomaly)
            .ToList();

        _logger.LogInformation(
            "Remaining system tiles with wormholes: {WormholeTiles} with SystemTileCodes: {SystemTileCodes}",
            allWormholeTiles.Count,
            string.Join(",", allWormholeTiles.Select(x => x.SystemTileCode).ToList()));

        var wormholeTilesToChooseFrom = allWormholeTiles
            .Where(x => !presentAlphaWormholeTiles.Contains(x.SystemTileCode) && !presentBetaWormholeTiles.Contains(x.SystemTileCode))
            .OrderBy(systemTile => _random.Next())
            .ToList();

        _logger.LogInformation("Choosing from system tiles count: {WormholeTilesCount}", presentAlphaWormholeTiles.Count + presentBetaWormholeTiles.Count);

        if (request.WormholesDensity == WormholeDensity.AtLeastOnePair)
        {
            var selectedWormholePair = _random.Next(0, 2) == 0 ? WormholeName.Alpha : WormholeName.Beta;
            var presentCount = selectedWormholePair == WormholeName.Alpha ? presentAlphaWormholeTiles.Count : presentBetaWormholeTiles.Count;

            var selectedWormholeTiles = wormholeTilesToChooseFrom
                .Where(x => x!.Wormholes.Any(x => x.WormholeName == selectedWormholePair))
                .Take(2 - presentCount)
                .ToList();

            foreach (var selectedWormholeTile in selectedWormholeTiles)
            {
                if (selectedWormholeTile!.TileCategory == SystemTileCategory.Red)
                    systemTilesForGalaxyDistribution.RedTiles.Add(selectedWormholeTile);
                else
                    systemTilesForGalaxyDistribution.BlueTiles.Add(selectedWormholeTile);
            }

            return;
        }

        if (request.WormholesDensity == WormholeDensity.AtLeastTwoPairs)
        {
            var selectedAlphaWormholes = wormholeTilesToChooseFrom
                .Where(x => x!.Wormholes.Any(x => x.WormholeName == WormholeName.Alpha))
                .Take(2 - presentAlphaWormholeTiles.Count)
                .ToList();

            var selectedBetaWormholes = wormholeTilesToChooseFrom
                .Where(x => x!.Wormholes.Any(x => x.WormholeName == WormholeName.Beta))
                .Take(2 - presentBetaWormholeTiles.Count)
                .ToList();

            foreach (var selectedWormholeTile in selectedAlphaWormholes)
            {
                if (selectedWormholeTile!.TileCategory == SystemTileCategory.Red)
                    systemTilesForGalaxyDistribution.RedTiles.Add(selectedWormholeTile);
                else
                    systemTilesForGalaxyDistribution.BlueTiles.Add(selectedWormholeTile);
            }

            foreach (var selectedWormholeTile in selectedBetaWormholes)
            {
                if (selectedWormholeTile!.TileCategory == SystemTileCategory.Red)
                    systemTilesForGalaxyDistribution.RedTiles.Add(selectedWormholeTile);
                else
                    systemTilesForGalaxyDistribution.BlueTiles.Add(selectedWormholeTile);
            }

            _logger.LogInformation(
                "Selected red wormhole tiles: {Count} with codes: {Codes}",
                systemTilesForGalaxyDistribution.RedTilesCount,
                string.Join(",", systemTilesForGalaxyDistribution.RedTiles
                .Where(x => x.HasWormholes)
                .Select(x => x.SystemTileCode)
                .ToList()));

            _logger.LogInformation(
                "Selected blue wormhole tiles: {Count} with codes: {Codes}",
                systemTilesForGalaxyDistribution.BlueTilesCount,
                string.Join(",", systemTilesForGalaxyDistribution.BlueTiles
                .Where(x => x.HasWormholes)
                .Select(x => x.SystemTileCode)
                .ToList()));

            return;
        }
    }
}
