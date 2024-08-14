using System.Linq;
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

    public Task<List<Slice>> BalanceSlices(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        SystemTilesForMapSetup systemTilesForMapSetup,
        SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution,
        GenerateMapRequest request)
    {
        var slices = AddCorrespondingSystemTileToSlicePosition(galaxy, mapSettings);

        slices = RedistributeSystemTiles(slices, systemTilesForGalaxyDistribution, request);

        return Task.FromResult(slices);
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

    private List<Slice> RedistributeSystemTiles(List<Slice> slices, SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution, GenerateMapRequest request)
    {
        var shuffledBlueSystemTiles = systemTilesForGalaxyDistribution.BlueTiles.OrderBy(x => Random.Next()).ToList();
        var shuffeledRedSystemTiles = systemTilesForGalaxyDistribution.RedTiles.OrderBy(x => Random.Next()).ToList();
        var remainingSystemTiles = shuffledBlueSystemTiles.Concat(shuffeledRedSystemTiles);

        _logger.LogInformation("Prepared blue tiles for redistribution {BlueTiles}", systemTilesForGalaxyDistribution.BlueTiles.GetSystemTileCodes());
        _logger.LogInformation("Prepared red tiles for redistribution {RedTiles}", systemTilesForGalaxyDistribution.RedTiles.GetSystemTileCodes());
        int nullPositionsCount = slices.SelectMany(x => x.Positions).Count(x => x.SystemTile is null);


        int remainingSystemTilesCount = remainingSystemTiles.Count();

        if (remainingSystemTilesCount < nullPositionsCount || (remainingSystemTilesCount > nullPositionsCount))
            _logger.LogInformation("Count missmatch! System tiles {SystemTilesCount} : {NullPositionsCount} empty positions", remainingSystemTilesCount, nullPositionsCount);

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

        if (request.WormholesDensity == WormholeDensity.AtLeastTwoPairs)
        {
            // Distribute wormhole first
            var alphaWormholeTiles = remainingSystemTilesForSlices
                .Where(x => x.Wormholes.Any(x => x.WormholeName == WormholeName.Alpha))
                .Take(2)
                .ToList();

            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                .Where(x => !alphaWormholeTiles.Select(x => x.SystemTileCode).Contains(x.SystemTileCode))
                .ToList();

            var betaWormholeTiles = remainingSystemTilesForSlices
                .Where(x => x.Wormholes.Any(x => x.WormholeName == WormholeName.Beta))
                .Take(2)
                .ToList();

            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                .Where(x => !betaWormholeTiles.Select(x => x.SystemTileCode).Contains(x.SystemTileCode))
                .ToList();

            var alphaWormholeTilesToDistribute = alphaWormholeTiles.ToList();
            var betaWormholeTilesToDistribute = betaWormholeTiles.ToList();

            if (request.MapTemplate != MapTemplate.EightPlayersLargeMap && request.MapTemplate != MapTemplate.SixPlayersLargeMap)
            {
                for (int i = 0; i < alphaWormholeTiles.Count; i++)
                {
                    var leastRedSlice = slices
                        .OrderBy(s => s.Positions.Count(t => t.SystemTile is not null && t.SystemTile!.TileCategory == SystemTileCategory.Red)
                            + s.DraftedSystemTiles.Count(x => x.HasWormholes || x.TileCategory == SystemTileCategory.Red))
                        .First();

                    // Add the alpha wormhole tile to the leastRedSlice
                    if (alphaWormholeTilesToDistribute.Count > 0
                        && (leastRedSlice.DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red)
                        + leastRedSlice.Positions.Count(pos => pos.SystemTile is not null && pos.SystemTile.TileCategory == SystemTileCategory.Red) < 2))
                    {
                        leastRedSlice.DraftedSystemTiles.Add(alphaWormholeTilesToDistribute[0]);
                        alphaWormholeTilesToDistribute.RemoveAt(0);
                    }

                    // Determine the index of the leastRedSlice
                    var sliceIndex = slices.IndexOf(leastRedSlice);

                    // Calculate the opposite slice index
                    int oppositeIndex;

                    if (sliceIndex < slices.Count / 2)
                    {
                        oppositeIndex = (slices.Count / 2) + (sliceIndex % (slices.Count / 2));
                    }
                    else
                    {
                        oppositeIndex = sliceIndex - (slices.Count / 2);
                    }

                    var oppositeSlice = slices[oppositeIndex];

                    for (int j = 0; j < slices.Count; j++)
                    {
                        if (alphaWormholeTilesToDistribute.Count > 0
                            && slices[oppositeIndex].DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red)
                            + slices[oppositeIndex].Positions.Count(pos => pos.SystemTile is not null && pos.SystemTile.TileCategory == SystemTileCategory.Red) < 2)
                        {
                            oppositeSlice.DraftedSystemTiles.Add(alphaWormholeTilesToDistribute[0]);
                            alphaWormholeTilesToDistribute.RemoveAt(0);
                            break;
                        }
                        else
                        {
                            oppositeIndex++;
                            if (oppositeIndex >= slices.Count)
                            {
                                oppositeIndex = 0;
                            }
                        }
                    }
                }

                for (int i = 0; i < betaWormholeTiles.Count; i++)
                {
                    var leastRedSlice = slices
                        .OrderBy(s => s.Positions.Count(t => t.SystemTile is not null && t.SystemTile!.TileCategory == SystemTileCategory.Red)
                            + s.DraftedSystemTiles.Count(x => x.HasWormholes || x.TileCategory == SystemTileCategory.Red))
                        .First();

                    // Add the beta wormhole tile to the leastRedSlice
                    if (betaWormholeTilesToDistribute.Count > 0
                        && (leastRedSlice.DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red)
                        + leastRedSlice.Positions.Count(pos => pos.SystemTile is not null && pos.SystemTile.TileCategory == SystemTileCategory.Red) < 2))
                    {
                        leastRedSlice.DraftedSystemTiles.Add(betaWormholeTilesToDistribute[0]);
                        betaWormholeTilesToDistribute.RemoveAt(0);
                    }

                    // Determine the index of the leastRedSlice
                    var sliceIndex = slices.IndexOf(leastRedSlice);

                    // Calculate the opposite slice index
                    int oppositeIndex;

                    if (sliceIndex < slices.Count / 2)
                    {
                        oppositeIndex = (slices.Count / 2) + (sliceIndex % (slices.Count / 2));
                    }
                    else
                    {
                        oppositeIndex = sliceIndex - (slices.Count / 2);
                    }

                    var oppositeSlice = slices[oppositeIndex];

                    for (int j = 0; j < slices.Count; j++)
                    {
                        if (betaWormholeTilesToDistribute.Count > 0
                            && slices[oppositeIndex].DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red)
                            + slices[oppositeIndex].Positions.Count(pos => pos.SystemTile is not null && pos.SystemTile.TileCategory == SystemTileCategory.Red) < 2)
                        {
                            oppositeSlice.DraftedSystemTiles.Add(betaWormholeTilesToDistribute[0]);
                            betaWormholeTilesToDistribute.RemoveAt(0);
                            break;
                        }
                        else
                        {
                            oppositeIndex++;
                            if (oppositeIndex >= slices.Count)
                            {
                                oppositeIndex = 0;
                            }
                        }
                    }
                }
            }

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

                    if (currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalResourceValue()) > currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalInfluenceValue()))
                    {
                        if (request.MapTemplate != MapTemplate.SixPlayersLargeMap && request.MapTemplate == MapTemplate.EightPlayersLargeMap)
                        {
                            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                                .OrderByDescending(x => x.GetOptimalInfluenceValue())
                                .ToList();
                        }
                        else
                        {
                            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                                .Where(x => x.Wormholes.Count == 0)
                                .OrderByDescending(x => x.GetOptimalInfluenceValue())
                                .ToList();
                        }
                    }
                    else
                    {
                        if (request.MapTemplate != MapTemplate.SixPlayersLargeMap && request.MapTemplate == MapTemplate.EightPlayersLargeMap)
                        {
                            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                                .OrderByDescending(x => x.GetOptimalResourceValue())
                                .ToList();
                        }
                        else
                        {
                            remainingSystemTilesForSlices = remainingSystemTilesForSlices
                                .Where(x => x.Wormholes.Count == 0)
                                .OrderByDescending(x => x.GetOptimalResourceValue())
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

        _logger.LogInformation("Slices log after draft:\n{SlicesLog}", slices.GetSlicesLog());

        return slices;
    }
}
