using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

public class MiltyDraftSliceBalancer(
    ILogger<MiltyDraftSliceBalancer> logger)
    : IMiltyDraftSliceBalancer
{
    private readonly ILogger<MiltyDraftSliceBalancer> _logger = logger;

    public Task<List<Slice>> BalanceSlices(SystemTilesForSlices systemTilesForSlices, List<Slice> slices)
    {
        slices = RedistributeWormholeTiles(slices, systemTilesForSlices.WormholeSystemTiles.ToList());
        slices = RedistributeLegendaryTiles(slices, systemTilesForSlices.LegendarySystemTiles.ToList());
        slices = RedistributeRedTiles(slices, systemTilesForSlices.RedSystemTiles.ToList());
        slices = RedistributeBlueTiles(slices, systemTilesForSlices.BlueSystemTiles.ToList());

        return Task.FromResult(slices);
    }

    private List<Slice> RedistributeRedTiles(List<Slice> slices, List<SystemTile> redSystemTiles)
    {
        _logger.LogInformation("Redistributing red system tiles count: {Count}", redSystemTiles.Count);

        while (redSystemTiles.Count > 0)
        {
            _logger.LogInformation("{RemainingCount} remaining red system tiles", redSystemTiles.Count);

            _logger.LogInformation(
                "Slices evaluation before sort: \n{SlicesEvaluation}",
                slices.GetSlicesEvaluation(SystemWeight.Balanced));

            slices = slices
                .OrderBy(x => x.GetNumberOfRedTiles())
                .ThenBy(x => x.GetNumberOfWormholeTiles())
                .ThenBy(x => x.SliceEvaluation(SystemWeight.Balanced))
                .ThenBy(x => x.HasAnomaly())
                .ToList();

            bool anySliceCanAcceptTile = false;

            for (int i = 0; i < slices.Count; i++)
            {
                var currentSlice = slices[i];

                if (currentSlice.DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red) < 2)
                {
                    anySliceCanAcceptTile = true;

                    if (currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalResourceValue()) > currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalInfluenceValue()))
                    {
                        redSystemTiles = redSystemTiles
                            .OrderByDescending(x => x.HasWormholes)
                            .ThenByDescending(x => x.GetOptimalInfluenceValue())
                            .ToList();
                    }
                    else
                    {
                        redSystemTiles = redSystemTiles
                            .OrderByDescending(x => x.HasWormholes)
                            .ThenByDescending(x => x.GetOptimalResourceValue())
                            .ToList();
                    }

                    currentSlice.DraftedSystemTiles.Add(redSystemTiles[0]);

                    _logger.LogInformation(
                        "SystemTile {SystemTile} added to Slice {SliceId}",
                        redSystemTiles[0].SystemTileCode,
                        currentSlice.Id);

                    redSystemTiles.RemoveAt(0);
                    break;
                }
            }

            if (!anySliceCanAcceptTile)
            {
                _logger.LogInformation("No slices can accept more tiles. Exiting the loop.");
                break;
            }
        }

        return slices;
    }

    private List<Slice> RedistributeBlueTiles(List<Slice> slices, List<SystemTile> blueSystemTiles)
    {
        while (blueSystemTiles.Count > 0)
        {
            _logger.LogInformation("{RemainingCount} remaining system tiles", blueSystemTiles.Count);

            _logger.LogInformation(
                "Slices evaluation before sort: \n{SlicesEvaluation}",
                slices.GetSlicesEvaluation(SystemWeight.Balanced));

            slices = slices
                .OrderBy(x => x.SliceEvaluation(SystemWeight.Balanced))
                .ThenBy(x => x.GetNumberOfWormholeTiles())
                .ThenBy(x => x.HasAnomaly())
                .ToList();

            bool anySliceCanAcceptTile = false;

            for (int i = 0; i < slices.Count; i++)
            {
                var currentSlice = slices[i];

                if (currentSlice.DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Blue) < 3)
                {
                    anySliceCanAcceptTile = true;

                    if (currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalResourceValue()) >= currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalInfluenceValue()))
                    {
                        blueSystemTiles = blueSystemTiles
                            .OrderByDescending(x => x.GetOptimalInfluenceValue())
                            .ThenByDescending(x => x.HasWormholes)
                            .ToList();
                    }
                    else
                    {
                        blueSystemTiles = blueSystemTiles
                            .OrderByDescending(x => x.GetOptimalResourceValue())
                            .OrderByDescending(x => x.HasWormholes)
                            .ToList();
                    }

                    currentSlice.DraftedSystemTiles.Add(blueSystemTiles[0]);

                    _logger.LogInformation(
                        "SystemTile {SystemTile} added to Slice {SliceId}",
                        blueSystemTiles[0].SystemTileCode,
                        currentSlice.Id);

                    blueSystemTiles.RemoveAt(0);
                    break;
                }
            }

            if (!anySliceCanAcceptTile)
            {
                _logger.LogInformation("No slices can accept more tiles. Exiting the loop.");
                break;
            }
        }

        return slices;
    }

    private List<Slice> RedistributeLegendaryTiles(List<Slice> slices, List<SystemTile> legendarySystemTiles)
    {
        _logger.LogInformation("Redistributing legendary system tiles count: {Count}", legendarySystemTiles.Count);

        while (legendarySystemTiles.Count > 0)
        {
            _logger.LogInformation("{RemainingCount} remaining legendary system tiles", legendarySystemTiles.Count);

            _logger.LogInformation(
                "Slices evaluation before sort: \n{SlicesEvaluation}",
                slices.GetSlicesEvaluation(SystemWeight.Balanced));

            slices = slices.OrderBy(x => x.SliceEvaluation(SystemWeight.Balanced))
                           .ToList();

            var currentSystemTile = legendarySystemTiles[0];

            bool anySliceCanAcceptTile = false;

            for (int i = 0; i < slices.Count; i++)
            {
                var currentSlice = slices[i];

                if ((currentSystemTile.TileCategory == SystemTileCategory.Blue && currentSlice.DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Blue) < 3)
                    || (currentSystemTile.TileCategory == SystemTileCategory.Red && currentSlice.DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red) < 2))
                {
                    anySliceCanAcceptTile = true;

                    if (currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalResourceValue()) > currentSlice.DraftedSystemTiles.Sum(x => x.GetOptimalInfluenceValue()))
                    {
                        legendarySystemTiles = legendarySystemTiles
                            .OrderByDescending(x => x.GetOptimalInfluenceValue())
                            .ToList();
                    }
                    else
                    {
                        legendarySystemTiles = legendarySystemTiles
                            .OrderByDescending(x => x.GetOptimalResourceValue())
                            .ToList();
                    }

                    currentSlice.DraftedSystemTiles.Add(legendarySystemTiles[0]);

                    _logger.LogInformation(
                        "SystemTile {SystemTile} added to Slice {SliceId}",
                        legendarySystemTiles[0].SystemTileCode,
                        currentSlice.Id);

                    legendarySystemTiles.RemoveAt(0);
                    break;
                }
            }

            if (!anySliceCanAcceptTile)
            {
                _logger.LogInformation("No slices can accept more tiles. Exiting the loop.");
                break;
            }
        }

        return slices;
    }

    private List<Slice> RedistributeWormholeTiles(List<Slice> slices, List<SystemTile> wormholeSystemTiles)
    {
        _logger.LogInformation("Redistributing wormhole system tiles count: {Count}", wormholeSystemTiles.Count);

        while (wormholeSystemTiles.Count > 0)
        {
            _logger.LogInformation("{RemainingCount} remaining wormhole system tiles", wormholeSystemTiles.Count);

            _logger.LogInformation(
                "Slices evaluation before sort: \n{SlicesEvaluation}",
                slices.GetSlicesEvaluation(SystemWeight.Balanced));

            slices = slices
                .OrderBy(x => x.GetNumberOfRedTilesOrWormholeTiles())
                .ThenBy(x => x.SliceEvaluation(SystemWeight.Balanced))
                .ToList();

            bool anySliceCanAcceptTile = false;

            for (int i = 0; i < slices.Count; i++)
            {
                var currentSlice = slices[i];
                var currentWormholeTileCategory = wormholeSystemTiles[0].TileCategory;

                if ((currentWormholeTileCategory == SystemTileCategory.Blue && currentSlice.DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Blue) < 3)
                    || (currentWormholeTileCategory == SystemTileCategory.Red && currentSlice.DraftedSystemTiles.Count(x => x.TileCategory == SystemTileCategory.Red) < 2))
                {
                    anySliceCanAcceptTile = true;

                    currentSlice.DraftedSystemTiles.Add(wormholeSystemTiles[0]);

                    _logger.LogInformation(
                        "SystemTile {SystemTile} added to Slice {SliceId}",
                        wormholeSystemTiles[0].SystemTileCode,
                        currentSlice.Id);

                    wormholeSystemTiles.RemoveAt(0);
                    break;
                }
            }

            if (!anySliceCanAcceptTile)
            {
                _logger.LogInformation("No slices can accept more tiles. Exiting the loop.");
                break;
            }
        }

        return slices;
    }
}
