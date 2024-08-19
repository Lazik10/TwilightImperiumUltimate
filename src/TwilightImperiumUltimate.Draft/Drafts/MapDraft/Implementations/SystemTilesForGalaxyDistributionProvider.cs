using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Constants;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;
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
        SatisfyLegendaries(galaxy, systemTilesForMapSetup, systemTilesForGalaxyDistribution, request);
        SatisfyRemainingRedTilesCount(galaxy, systemTilesForMapSetup, systemTilesForGalaxyDistribution, mapSettings);
        SatisfyRemainingBlueTilesCount(galaxy, systemTilesForMapSetup, systemTilesForGalaxyDistribution);

        return systemTilesForGalaxyDistribution;
    }

    private void SatisfyRemainingBlueTilesCount(Dictionary<(int X, int Y), Hex> galaxy, SystemTilesForMapSetup systemTilesForMapSetup, SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution)
    {
        // We need to calculate how many blue tiles are missing in the galaxy
        var galaxyEmptyPositionsCount = galaxy.Values
            .Count(hex => hex.Name != PositionName.Empty && !(hex.SystemTile is not null && hex.SystemTile.HasAnomaly) && hex.Name != PositionName.Home && hex.Name != PositionName.MecatolRex);

        _logger.LogInformation("Empty positions count: {EmptyPositions}", galaxyEmptyPositionsCount);

        // We need to decrease this number by already prepared blue and red tiles
        var requiredNumberOfBlueTiles = galaxyEmptyPositionsCount - (systemTilesForGalaxyDistribution.BlueTiles.Count + systemTilesForGalaxyDistribution.RedTiles.Count);

        _logger.LogInformation(
            "Prepared red tiles: {PreparedRedCount}, Prepared blue tiles: {PreparedBlue}",
            systemTilesForGalaxyDistribution.RedTilesCount,
            systemTilesForGalaxyDistribution.BlueTilesCount);

        _logger.LogInformation(
            "Required additional number of blue tiles for galaxy: {RequiredNumberOfBlueTiles}",
            requiredNumberOfBlueTiles);

        var alreadyPickedBlueTiles = systemTilesForGalaxyDistribution.BlueTiles
            .Select(x => x.SystemTileCode)
            .ToList();

        var galaxyBlueTiles = galaxy.Values
            .Where(x => x.SystemTile is not null && x.SystemTile.TileCategory == SystemTileCategory.Blue)
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToList();

        var remainingAvailableBlueTiles = systemTilesForMapSetup.BlueTiles
            .Where(x => !x.HasLegendaryPlanet && !alreadyPickedBlueTiles.Contains(x.SystemTileCode) && !galaxyBlueTiles.Contains(x.SystemTileCode))
            .OrderBy(x => _random.Next())
            .Take(requiredNumberOfBlueTiles)
            .ToList();

        _logger.LogInformation("Already picked blue tiles: {Count} with codes: {Codes}", alreadyPickedBlueTiles.Count, alreadyPickedBlueTiles);

        _logger.LogInformation("Blue tiles in database: {Count} with codes: {Codes}", systemTilesForMapSetup.BlueTiles.Count, systemTilesForMapSetup.BlueTiles.GetSystemTileCodes());

        _logger.LogInformation("Randomly taken {Count} blue tiles from database to fill prepared blue tiles", remainingAvailableBlueTiles.Count);

        systemTilesForGalaxyDistribution.BlueTiles.AddRange(remainingAvailableBlueTiles);

        _logger.LogInformation("Selected blue tiles: {Count} with codes: {Codes}", systemTilesForGalaxyDistribution.BlueTiles.Count, systemTilesForGalaxyDistribution.BlueTiles.GetSystemTileCodes());
    }

    private void SatisfyRemainingRedTilesCount(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution,
        IMapSettings mappSettings)
    {
        // Red tiles already present in the galaxy
        var galaxyRedTiles = galaxy.GetRedTileCodes();

        // Red tiles already picked for the galaxy distribution (e. g. Wormholes or empty spaces)
        var alreadySelectedRedTiles = systemTilesForGalaxyDistribution.RedTiles
            .Select(x => x.SystemTileCode)
            .ToList();

        // Red tiles that are not in the galaxy and not already picked
        var redTilesToChooseFrom = systemTilesForMapSetup.RedTiles
            .Where(x => !galaxyRedTiles.Contains(x.SystemTileCode)
                && !alreadySelectedRedTiles.Contains(x.SystemTileCode)
                && !x.HasAnomaly)
            .ToList();

        // We need to decrease a number of selected red tiles by the number of red tiles already in the galaxy
        // and take them randomly for the remaining galaxy distribution
        var requiredNumberOfRedTiles = mappSettings.NumberOfRedTiles - galaxyRedTiles.Count - alreadySelectedRedTiles.Count;

        _logger.LogInformation(
            "Already in gaaxy red tiles: {GalaxyRedTilesCount} / Prepared red tiles: {Count} / Required {RequiredCount} additional red tiles!",
            galaxyRedTiles.Count,
            alreadySelectedRedTiles.Count,
            requiredNumberOfRedTiles);

        var selectedRemainingRedTiles = redTilesToChooseFrom
            .OrderBy(x => _random.Next())
            .Take(requiredNumberOfRedTiles)
            .ToList();

        systemTilesForGalaxyDistribution.RedTiles.AddRange(selectedRemainingRedTiles);

        _logger.LogInformation("Selected red tiles: {Count} with codes: {Codes}", selectedRemainingRedTiles.Count, string.Join(",", selectedRemainingRedTiles.Select(x => x.SystemTileCode)));
    }

    private void SatisfyLegendaries(Dictionary<(int X, int Y), Hex> galaxy, SystemTilesForMapSetup systemTilesForMapSetup, SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution, GenerateMapRequest request)
    {
        var legendaryTilesInGalaxy = galaxy.GetLegendaryTileCodes();

        _logger.LogInformation("Galaxy has {LegendaryTilesInGalaxy} legendary tiles already: {SystemTileCodes}", legendaryTilesInGalaxy.Count, string.Join(",", legendaryTilesInGalaxy));

        _logger.LogInformation("Requested number of legendaries: {NumberOfLegendaries}", request.NumberOfLegendaries);

        var legendaryTiles = systemTilesForMapSetup.BlueTiles
            .Where(x => x.HasLegendaryPlanet && !legendaryTilesInGalaxy.Contains(x.SystemTileCode))
            .ToList();

        var numberOfLegendariesToAdd = Math.Min(legendaryTiles.Count, request.NumberOfLegendaries - legendaryTilesInGalaxy.Count);
        _logger.LogInformation("Number of legendaries to add: {NumberOfLegendariesToAdd}", numberOfLegendariesToAdd);

        // We need to decrease a number of selected legendaries by the number of legendaries already in the galaxy
        systemTilesForGalaxyDistribution.BlueTiles
            .AddRange(legendaryTiles.
                Take(numberOfLegendariesToAdd));

        _logger.LogInformation("Selected legendary tiles: {Count} with codes: {Codes}", legendaryTiles.Count, string.Join(",", legendaryTiles.Select(x => x.SystemTileCode)));
    }

    private void SatisfyWormholes(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution,
        GenerateMapRequest request)
    {
        if (request.WormholesDensity == WormholeDensity.Random)
            return;

        var presentAlphaWormholeTiles = galaxy.GetWormholeTileCodes(WormholeName.Alpha);

        _logger.LogInformation(
            "Galaxy has {PresentAlphaWormholeTiles} alpha wormhole tiles already: {SystemTileCodes}",
            presentAlphaWormholeTiles.Count,
            string.Join(",", presentAlphaWormholeTiles));

        var presentBetaWormholeTiles = galaxy.GetWormholeTileCodes(WormholeName.Beta);

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

        _logger.LogInformation("Combined wormhole tiles in galaxy already: {WormholeTilesCount}", presentAlphaWormholeTiles.Count + presentBetaWormholeTiles.Count);

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

            _logger.LogInformation(
                "Selected red wormhole tiles: {Count} with codes: {Codes}",
                systemTilesForGalaxyDistribution.RedTilesCount,
                systemTilesForGalaxyDistribution.RedTiles.GetSystemTilesWithWormholesCodes());

            _logger.LogInformation(
                "Selected blue wormhole tiles: {Count} with codes: {Codes}",
                systemTilesForGalaxyDistribution.BlueTilesCount,
                systemTilesForGalaxyDistribution.BlueTiles.GetSystemTilesWithWormholesCodes());

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
                systemTilesForGalaxyDistribution.RedTiles.GetSystemTilesWithWormholesCodes());

            _logger.LogInformation(
                "Selected blue wormhole tiles: {Count} with codes: {Codes}",
                systemTilesForGalaxyDistribution.BlueTilesCount,
                systemTilesForGalaxyDistribution.BlueTiles.GetSystemTilesWithWormholesCodes());

            return;
        }
    }
}
