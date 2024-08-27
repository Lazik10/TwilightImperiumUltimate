using System.Diagnostics.CodeAnalysis;
using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

internal class SliceBalancer(
    ILogger<SliceBalancer> logger)
    : ISliceBalancer
{
    private static readonly Random Random = new Random();
    private readonly ILogger<SliceBalancer> _logger = logger;

    public Task<(List<Slice> Slices, List<SystemTile> UnusesSystemTiles)> BalanceSlices(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        SystemTilesForMapSetup systemTilesForMapSetup,
        SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution,
        GenerateMapRequest request)
    {
        var slices = AddCorrespondingSystemTileToSlicePosition(galaxy, mapSettings);

        var result = RedistributeSystemTiles(galaxy, slices, systemTilesForGalaxyDistribution, request);

        return Task.FromResult(result);
    }

    private List<Slice> AddCorrespondingSystemTileToSlicePosition(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings)
    {
        var slices = new List<Slice>();
        var mapSettingsSlices = mapSettings.Slices;

        foreach (var slice in mapSettingsSlices)
        {
            var newSlice = new Slice();
            newSlice.Id = slice.Key;

            foreach (var slicePosition in slice.Value)
            {
                if (galaxy.TryGetValue(slicePosition, out var hex))
                {
                    newSlice.Positions.Add(new SlicePosition { X = slicePosition.X, Y = slicePosition.Y, SystemTile = hex.SystemTile });
                }
                else
                {
                    newSlice.Positions.Add(new SlicePosition { X = slicePosition.X, Y = slicePosition.Y, SystemTile = null });
                }
            }

            slices.Add(newSlice);
        }

        _logger.LogInformation("{GeneratedSlices}", slices.GetSlicesLog());

        return slices;
    }

    private (List<Slice> Slices, List<SystemTile> UnusedSystemTiles) RedistributeSystemTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        List<Slice> slices,
        SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution,
        GenerateMapRequest request)
    {
        var shuffledBlueSystemTiles = systemTilesForGalaxyDistribution.BlueTiles.OrderBy(x => Random.Next()).ToList();
        var shuffeledRedSystemTiles = systemTilesForGalaxyDistribution.RedTiles.OrderBy(x => Random.Next()).ToList();
        var remainingSystemTiles = shuffledBlueSystemTiles.Concat(shuffeledRedSystemTiles);

        _logger.LogInformation("Prepared blue tiles for redistribution {BlueTiles}", systemTilesForGalaxyDistribution.BlueTiles.GetSystemTileCodes());
        _logger.LogInformation("Prepared red tiles for redistribution {RedTiles}", systemTilesForGalaxyDistribution.RedTiles.GetSystemTileCodes());

        int numberOfEmptyPositionsInGalaxy = galaxy.GetNumberOfEmptyPositions();
        int remainingSystemTilesCount = remainingSystemTiles.Count();

        if (remainingSystemTilesCount < numberOfEmptyPositionsInGalaxy || (remainingSystemTilesCount > numberOfEmptyPositionsInGalaxy))
            _logger.LogInformation("Count missmatch! Remaining system tiles count {SystemTilesCount} : {GalaxyEmptyPositionsCount} empty positions in a galaxy required to be filled", remainingSystemTilesCount, numberOfEmptyPositionsInGalaxy);

        remainingSystemTiles = request.SystemWeight switch
        {
            SystemWeight.Random => remainingSystemTiles.OrderBy(x => Random.Next()),
            SystemWeight.Balanced => remainingSystemTiles.OrderByOptimalValue(),
            SystemWeight.Resources => remainingSystemTiles.OrderByResourceValue(),
            SystemWeight.Influence => remainingSystemTiles.OrderByInfluenceValue(),
            _ => remainingSystemTiles.OrderBy(x => Random.Next()),
        };

        var remainingSystemTilesForSlices = remainingSystemTiles.ToList();

        _logger.LogInformation(
            "Remaining system tiles count: {Count}, {RemainingSystemTiles}",
            remainingSystemTilesForSlices.Count,
            remainingSystemTilesForSlices.GetSystemTileCodesWithSystemWeight(request.SystemWeight));

        _logger.LogInformation("Slices log before wormhole assignment:\n{SlicesLog}", slices.GetSlicesLog());

        if (request.WormholesDensity == WormholeDensity.AtLeastTwoPairs)
            DistributeRequiredWormholePairs(remainingSystemTilesForSlices, slices, request);

        _logger.LogInformation(
            "Remaining system tiles count: {Count}, {RemainingSystemTiles}",
            remainingSystemTilesForSlices.Count,
            remainingSystemTilesForSlices.GetSystemTileCodesWithSystemWeight(request.SystemWeight));

        _logger.LogInformation("Slices log after wormhole assignment:\n{SlicesLog}", slices.GetSlicesLog());

        // Due to the wormhole distribution, we need to update the remaining system tiles list
        remainingSystemTilesForSlices = slices.UpdateRemainingAvailableSystemTiles(remainingSystemTilesForSlices);

        DistributeRedTiles(remainingSystemTilesForSlices, slices, request);

        remainingSystemTilesForSlices = slices.UpdateRemainingAvailableSystemTiles(remainingSystemTilesForSlices);

        DistributeBlueTiles(remainingSystemTilesForSlices, slices, request);

        remainingSystemTilesForSlices = slices.UpdateRemainingAvailableSystemTiles(remainingSystemTilesForSlices);

        _logger.LogInformation(
            "Remaining system tiles count: {Count}, {RemainingSystemTiles}",
            remainingSystemTilesForSlices.Count,
            remainingSystemTilesForSlices.GetSystemTileCodesWithSystemWeight(request.SystemWeight));

        _logger.LogInformation("Slices log after draft:\n{SlicesLog}", slices.GetSlicesLog());

        return (slices, remainingSystemTilesForSlices);
    }

    private void DistributeBlueTiles(List<SystemTile> remainingSystemTilesForSlices, List<Slice> slices, GenerateMapRequest request)
    {
        // Then distribute remaining blue tiles
        while (remainingSystemTilesForSlices.Count > 0)
        {
            _logger.LogInformation("{RemainingCount} remaining system tiles", remainingSystemTilesForSlices.Count);

            _logger.LogInformation(
                "Slices evaluation before sort: \n{SlicesEvaluation}",
                slices.GetSlicesEvaluation(request.SystemWeight));

            slices = slices.OrderBy(x => x.SliceEvaluation(request.SystemWeight))
                           .ThenBy(x => x.HasAnomaly())
                           .ToList();

            bool anySliceCanAcceptTile = false;

            for (int i = 0; i < slices.Count; i++)
            {
                var currentSlice = slices[i];

                if (currentSlice.DraftedSystemTiles.Count + currentSlice.Positions.Count(pos => pos.SystemTile is not null) < currentSlice.Positions.Count)
                {
                    anySliceCanAcceptTile = true;

                    if (currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalResourceValue()) >= currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalInfluenceValue()))
                    {
                        if (request.MapTemplate != MapTemplate.SixPlayersLargeMap && request.MapTemplate == MapTemplate.EightPlayersLargeMap)
                        {
                            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                                .OrderByDescending(x => x.HasLegendaryPlanet)
                                .ThenByDescending(x => x.GetOptimalInfluenceValue())
                                .ToList();
                        }
                        else
                        {
                            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                                .Where(x => x.Wormholes.Count == 0)
                                .OrderByDescending(x => x.HasLegendaryPlanet)
                                .ThenByDescending(x => x.GetOptimalInfluenceValue())
                                .ToList();
                        }
                    }
                    else
                    {
                        if (request.MapTemplate != MapTemplate.SixPlayersLargeMap && request.MapTemplate == MapTemplate.EightPlayersLargeMap)
                        {
                            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                                .OrderByDescending(x => x.HasLegendaryPlanet)
                                .ThenByDescending(x => x.GetOptimalResourceValue())
                                .ToList();
                        }
                        else
                        {
                            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                                .Where(x => x.Wormholes.Count == 0)
                                .OrderByDescending(x => x.HasLegendaryPlanet)
                                .ThenByDescending(x => x.GetOptimalResourceValue())
                                .ToList();
                        }
                    }

                    currentSlice.DraftedSystemTiles.Add(remainingSystemTilesForSlices[0]);

                    _logger.LogInformation(
                        "SystemTile {SystemTile} added to Slice {SliceId}",
                        remainingSystemTilesForSlices[0].SystemTileCode,
                        currentSlice.Id);

                    remainingSystemTilesForSlices.RemoveAt(0);
                    break;
                }
            }

            if (!anySliceCanAcceptTile)
            {
                _logger.LogInformation("No slices can accept more tiles. Exiting the loop.");
                break;
            }
        }
    }

    private void DistributeRedTiles(List<SystemTile> remainingSystemTilesForSlices, List<Slice> slices, GenerateMapRequest request)
    {
        // Distribute red tile first
        foreach (var systemTile in remainingSystemTilesForSlices.Where(x => x.TileCategory == SystemTileCategory.Red))
        {
            _logger.LogInformation(
                "Slices evaluation before sort: \n{SlicesEvaluation}",
                slices.GetSlicesEvaluation(request.SystemWeight));

            slices = slices.OrderBy(x => x.SliceEvaluation(request.SystemWeight))
                .ThenBy(x => x.HasAnomaly()).ToList();

            _logger.LogInformation(
                "Slices evaluation after sort: \n{SlicesEvaluation}",
                slices.GetSlicesEvaluation(request.SystemWeight));

            for (int i = 0; i < slices.Count; i++)
            {
                var isSystemTileRed = systemTile.TileCategory == SystemTileCategory.Red;

                // loop through slices and add the system tile to the first slice that still has an empty position
                // then break out and move to the next system tile, slices will be sorted by evaluation again so the weakest slice gets
                // the next best system tile
                if (slices[i].DraftedSystemTiles.Count < slices[i].Positions.Count(pos => pos.SystemTile is null)
                    && (!isSystemTileRed || slices[i].DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red) + slices[i].Positions.Count(pos => pos.SystemTile is not null && pos.SystemTile.TileCategory == SystemTileCategory.Red) < 2))
                {
                    slices[i].DraftedSystemTiles.Add(systemTile);

                    _logger.LogInformation(
                        "SystemTile {SystemTile} added to Slice {SliceId}",
                        systemTile.SystemTileCode,
                        slices[i].Id);
                    break;
                }
            }
        }
    }

    [SuppressMessage("Maintainability", "S1854", Justification = "Using the same distribution list in different scopes for simplicity.")]
    private void DistributeRequiredWormholePairs(List<SystemTile> remainingSystemTilesForSlices, List<Slice> slices, GenerateMapRequest request)
    {
        WormholeName selectWormholeTypeForOnePair = Random.Next(0, 2) == 0 ? WormholeName.Alpha : WormholeName.Beta;
        List<SystemTile> alphaWormholeTilesToDistribute = new List<SystemTile>();
        List<SystemTile> betaWormholeTilesToDistribute = new List<SystemTile>();

        if ((selectWormholeTypeForOnePair == WormholeName.Alpha && request.WormholesDensity == WormholeDensity.AtLeastOnePair)
            || request.WormholesDensity == WormholeDensity.AtLeastTwoPairs)
        {
            // Distribute wormhole first
            alphaWormholeTilesToDistribute = remainingSystemTilesForSlices
                .Where(x => x.Wormholes.Any(x => x.WormholeName == WormholeName.Alpha))
                .Take(2)
                .ToList();

            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                .Where(x => !alphaWormholeTilesToDistribute.Select(x => x.SystemTileCode).Contains(x.SystemTileCode))
                .ToList();
        }

        if ((selectWormholeTypeForOnePair == WormholeName.Beta && request.WormholesDensity == WormholeDensity.AtLeastOnePair)
            || request.WormholesDensity == WormholeDensity.AtLeastTwoPairs)
        {
            betaWormholeTilesToDistribute = remainingSystemTilesForSlices
                .Where(x => x.Wormholes.Any(x => x.WormholeName == WormholeName.Beta))
                .Take(2)
                .ToList();

            // Even though we don't use the remaining system tiles here, we need to update the list
            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                .Where(x => !betaWormholeTilesToDistribute.Select(x => x.SystemTileCode).Contains(x.SystemTileCode))
                .ToList();
        }

        if (request.MapTemplate != MapTemplate.EightPlayersLargeMap && request.MapTemplate != MapTemplate.SixPlayersLargeMap)
        {
            for (int i = 0; i < alphaWormholeTilesToDistribute.Count; i++)
            {
                int oppositeIndex = 0;
                int sliceIndex = 0;

                if (alphaWormholeTilesToDistribute.Count > 1)
                {
                    // Find the slice with the least red tiles (including drafted red tiles and red tiles in positions
                    var leastRedSlice = slices
                        .OrderBy(s => s.GetNumberOfRedTilesOrWormholeTiles())
                        .ThenBy(s => Random.Next())
                        .First();

                    _logger.LogInformation("Least red slice: {LeastRedSlice}", leastRedSlice.Id);

                    var numberOfRedTilesInleastRedSlce = leastRedSlice.GetNumberOfRedTilesOrWormholeTiles();

                    _logger.LogInformation("Number of red or wormhole tiles in least red slice: {Count}", numberOfRedTilesInleastRedSlce);

                    // Add the alpha wormhole tile to the leastRedSlice
                    if (alphaWormholeTilesToDistribute.Count > 0 && (alphaWormholeTilesToDistribute[0].TileCategory == SystemTileCategory.Green || numberOfRedTilesInleastRedSlce < 2))
                    {
                        leastRedSlice.DraftedSystemTiles.Add(alphaWormholeTilesToDistribute[0]);
                        alphaWormholeTilesToDistribute.RemoveAt(0);

                        _logger.LogInformation("Adding alpha wormhole tile to slice {SliceId}", leastRedSlice.Id);
                    }

                    sliceIndex = slices.IndexOf(slices.First(x => x.Id == leastRedSlice.Id));
                }
                else
                {
                    // If we have only one wormhole to distribute, we need to find the slice with the already placed alpha wormhole
                    // unfortunately the first wormhole can be placed during red tiles placement and outside of every slice, so wee need to
                    // choose the slice index randomly
                    sliceIndex = slices.Find(x => x.Positions
                        .Any(x => x.SystemTile is not null && x.SystemTile.Wormholes
                            .Any(x => x.WormholeName == WormholeName.Alpha)))?.Id ?? Random.Next(0, slices.Count);
                }

                // Calculate the opposite slice index
                if (sliceIndex < slices.Count / 2)
                {
                    oppositeIndex = (slices.Count / 2) + (sliceIndex % (slices.Count / 2));
                }
                else
                {
                    oppositeIndex = sliceIndex - (slices.Count / 2);
                }

                var oppositeSlice = slices[oppositeIndex];
                int numberOfRedTilesAlreadyInSlice;

                for (int j = 0; j < slices.Count; j++)
                {
                    numberOfRedTilesAlreadyInSlice = oppositeSlice.GetNumberOfRedTilesOrWormholeTiles();

                    if (alphaWormholeTilesToDistribute.Count > 0 && (alphaWormholeTilesToDistribute[0].TileCategory == SystemTileCategory.Green || numberOfRedTilesAlreadyInSlice < 2))
                    {
                        _logger.LogInformation("Adding alpha wormhole tile to slice {SliceId}, red tiles count: {Count}", oppositeSlice.Id, numberOfRedTilesAlreadyInSlice);
                        oppositeSlice.DraftedSystemTiles.Add(alphaWormholeTilesToDistribute[0]);
                        alphaWormholeTilesToDistribute.RemoveAt(0);
                        break;
                    }
                    else
                    {
                        _logger.LogInformation("Opposite slice {OppositeSliceId} already has 2 red tiles, moving 1 slice clockwise", oppositeSlice.Id);

                        oppositeIndex++;

                        if (oppositeIndex >= slices.Count)
                        {
                            oppositeIndex = 0;
                        }

                        oppositeSlice = slices[oppositeIndex];
                    }
                }
            }

            for (int i = 0; i < betaWormholeTilesToDistribute.Count; i++)
            {
                int oppositeIndex = 0;
                int sliceIndex = 0;

                if (betaWormholeTilesToDistribute.Count > 1)
                {
                    var leastRedSlice = slices
                        .OrderBy(s => s.GetNumberOfRedTilesOrWormholeTiles())
                        .ThenBy(s => Random.Next())
                        .First();

                    _logger.LogInformation("Least red slice: {LeastRedSlice}", leastRedSlice.Id);

                    var numberOfRedTilesInLeastRedSlice = leastRedSlice.GetNumberOfRedTilesOrWormholeTiles();

                    _logger.LogInformation("Number of red tiles or wormhole tiles in least red slice: {Count}", numberOfRedTilesInLeastRedSlice);

                    // Add the beta wormhole tile to the leastRedSlice
                    if (betaWormholeTilesToDistribute.Count > 0 && (betaWormholeTilesToDistribute[0].TileCategory == SystemTileCategory.Green || numberOfRedTilesInLeastRedSlice < 2))
                    {
                        leastRedSlice.DraftedSystemTiles.Add(betaWormholeTilesToDistribute[0]);
                        betaWormholeTilesToDistribute.RemoveAt(0);

                        _logger.LogInformation("Adding beta wormhole tile to slice {SliceId}", leastRedSlice.Id);
                    }

                    // Determine the index of the leastRedSlice
                    sliceIndex = slices.IndexOf(slices.First(x => x.Id == leastRedSlice.Id));
                }
                else
                {
                    // If we have only one wormhole to distribute, we need to find the slice with the already placed alpha wormhole
                    // unfortunately the first wormhole can be placed during red tiles placement and outside of every slice, so wee need to
                    // choose the slice index randomly
                    sliceIndex = slices.Find(x => x.Positions
                        .Any(x => x.SystemTile is not null && x.SystemTile.Wormholes
                            .Any(x => x.WormholeName == WormholeName.Beta)))?.Id ?? Random.Next(0, slices.Count);
                }

                if (sliceIndex < slices.Count / 2)
                {
                    oppositeIndex = (slices.Count / 2) + (sliceIndex % (slices.Count / 2));
                }
                else
                {
                    oppositeIndex = sliceIndex - (slices.Count / 2);
                }

                var oppositeSlice = slices[oppositeIndex];
                int numberOfRedTilesAlreadyInSlice;

                for (int j = 0; j < slices.Count; j++)
                {
                    numberOfRedTilesAlreadyInSlice = oppositeSlice.GetNumberOfRedTilesOrWormholeTiles();

                    if (betaWormholeTilesToDistribute.Count > 0 && (betaWormholeTilesToDistribute[0].TileCategory == SystemTileCategory.Green || numberOfRedTilesAlreadyInSlice < 2))
                    {
                        _logger.LogInformation("Adding alpha wormhole tile to slice {SliceId}, red tiles count: {Count}", oppositeSlice.Id, numberOfRedTilesAlreadyInSlice);
                        oppositeSlice.DraftedSystemTiles.Add(betaWormholeTilesToDistribute[0]);
                        betaWormholeTilesToDistribute.RemoveAt(0);
                        break;
                    }
                    else
                    {
                        _logger.LogInformation("Opposite slice {OppositeSliceId} already has 2 red tiles, moving 1 slice clockwise", oppositeSlice.Id);

                        oppositeIndex++;

                        if (oppositeIndex >= slices.Count)
                        {
                            oppositeIndex = 0;
                        }

                        oppositeSlice = slices[oppositeIndex];
                    }
                }
            }
        }
    }
}
