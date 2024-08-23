using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.SliceDraft;

public class MiltyDraftSystemTileSetter(
    ILogger<MiltyDraftSystemTileSetter> logger)
    : IMiltyDraftSystemTileSetter
{
    private static readonly Random _random = new Random();
    private readonly ILogger<MiltyDraftSystemTileSetter> _logger = logger;

    public Task<List<Slice>> SetSystemTilesForSlices(SystemTilesForSlices preparedSystemTiles, List<Slice> slices)
    {
        for (int i = 0; i < slices.Count; i++)
        {
            slices[i] = ReorderSystemTilesToComplyWithRedTilesPlacement(slices[i]);
        }

        return Task.FromResult(slices);
    }

    private Slice ReorderSystemTilesToComplyWithRedTilesPlacement(Slice slice)
    {
        if (slice.DraftedSystemTiles.Count != 6)
        {
            _logger.LogInformation("Slice must contain 6 system tiles!");
        }

        var slicePositions = MiltyDraftSliceSettings.SlicePositions;

        var sliceSystemRedTiles = slice.DraftedSystemTiles
            .Where(x => x.TileCategory == SystemTileCategory.Red)
            .OrderBy(x => _random.Next())
            .ToList();

        _logger.LogInformation("Red tiles count : {RedTilesCount} with Codes {Codes}", sliceSystemRedTiles.Count, sliceSystemRedTiles.GetSystemTileCodes());

        var sliceSystemBlueTiles = slice.DraftedSystemTiles
            .Where(x => x.TileCategory == SystemTileCategory.Blue)
            .OrderBy(x => _random.Next())
            .ToList();

        _logger.LogInformation("Blue tiles count : {BlueTilesCount} with Codes {Codes}", sliceSystemBlueTiles.Count, sliceSystemBlueTiles.GetSystemTileCodes());

        var redTilesWithAnomalyCount = sliceSystemRedTiles
            .Count(x => x.TileCategory == SystemTileCategory.Red && x.HasAnomaly);

        _logger.LogInformation("Red tiles with anomaly count : {RedTilesCount} (From red tiles for slice)", redTilesWithAnomalyCount);

        if (redTilesWithAnomalyCount == 2)
        {
            var firstRedAnomalyTilePosition = MiltyDraftSliceSettings.AvailableRedPositions.Keys.ElementAt(_random.Next(MiltyDraftSliceSettings.AvailableRedPositions.Count));
            var availableSecondRedAnomalyTilePositions = MiltyDraftSliceSettings.AvailableRedPositions[firstRedAnomalyTilePosition];
            var secondRedAnomalyTilePosition = availableSecondRedAnomalyTilePositions.OrderBy(x => _random.Next()).First();

            _logger.LogInformation("First red anomaly tile position : {FirstRedAnomalyTilePosition}", firstRedAnomalyTilePosition);
            _logger.LogInformation("Second red anomaly tile position : {SecondRedAnomalyTilePosition}", secondRedAnomalyTilePosition);

            // If there are two red anomalies and both contains zero planets and one of them is in position 1 or 3
            // (adjacent to home system) don't place the second one adjacent to the home system tile as well it would lead to a bad slice
            if (!sliceSystemRedTiles.Exists(x => x.HasPlanets))
            {
                _logger.LogInformation("Both red tiles have no planets, setting correct adjacency for second red tile");
                if (firstRedAnomalyTilePosition == 1)
                {
                    secondRedAnomalyTilePosition = 5;
                }
                else if (firstRedAnomalyTilePosition == 3)
                {
                    secondRedAnomalyTilePosition = _random.Next(0, 2) == 0 ? 4 : 5;
                }
            }

            slice.DraftedSystemTiles[firstRedAnomalyTilePosition] = sliceSystemRedTiles[0];
            sliceSystemRedTiles.RemoveAt(0);
            _logger.LogInformation("Placing first red tile in position {Position}", firstRedAnomalyTilePosition);
            slice.DraftedSystemTiles[secondRedAnomalyTilePosition] = sliceSystemRedTiles[0];
            sliceSystemRedTiles.RemoveAt(0);
            _logger.LogInformation("Placing second red tile in position {Position}", secondRedAnomalyTilePosition);

            slicePositions = slicePositions.Where(x => x != firstRedAnomalyTilePosition && x != secondRedAnomalyTilePosition).ToList();

            _logger.LogInformation("Remaining slice positions : {RemainingPositions}", string.Join(",", slicePositions.Select(x => x)));
        }
        else if (redTilesWithAnomalyCount == 1 || redTilesWithAnomalyCount == 0)
        {
            _logger.LogInformation("Red tiles with anomaly count : {RedTilesCount} (From red tiles for slice)", redTilesWithAnomalyCount);

            var adjacementHomePositions = MiltyDraftSliceSettings.SlicePositions
                .Take(3)
                .OrderBy(x => _random.Next())
                .ToList();

            _logger.LogInformation("Adjacent home positions : {AdjacentHomePositions}", string.Join(",", adjacementHomePositions.Select(x => x)));

            var blueTilesAdjacentToHomePositionCount = _random.Next(0, 5) == 0 ? 3 : 2;

            for (int i = 0; i < adjacementHomePositions.Count; i++)
            {
                if (blueTilesAdjacentToHomePositionCount == 3 || i < 2)
                {
                    _logger.LogInformation("Placing blue tile {TileCode} in position {Position}", sliceSystemBlueTiles[0].SystemTileCode, adjacementHomePositions[i]);

                    slice.DraftedSystemTiles[adjacementHomePositions[i]] = sliceSystemBlueTiles[0];
                    sliceSystemBlueTiles.RemoveAt(0);

                    slicePositions = slicePositions.Where(x => x != adjacementHomePositions[i]).ToList();

                    _logger.LogInformation("Remaining slice positions : {RemainingPositions}", string.Join(",", slicePositions.Select(x => x)));
                }
                else
                {
                    _logger.LogInformation("Placing red tile {TileCode} in position {Position}", sliceSystemRedTiles[0].SystemTileCode, adjacementHomePositions[i]);

                    slice.DraftedSystemTiles[adjacementHomePositions[i]] = sliceSystemRedTiles[0];
                    sliceSystemRedTiles.RemoveAt(0);

                    slicePositions = slicePositions.Where(x => x != adjacementHomePositions[i]).ToList();

                    _logger.LogInformation("Remaining slice positions : {RemainingPositions}", string.Join(",", slicePositions.Select(x => x)));
                }
            }
        }

        var remainingFreeSlicePositions = slicePositions;

        _logger.LogInformation("Remaining free slice positions : {RemainingPositions}", string.Join(",", remainingFreeSlicePositions.Select(x => x)));

        var remainingSystemTiles = sliceSystemBlueTiles.Concat(sliceSystemRedTiles).ToList();

        _logger.LogInformation("Remaining system tiles count : {RemainingCount} with codes: {Codes}", remainingSystemTiles.Count, remainingSystemTiles.GetSystemTileCodes());

        if ((remainingFreeSlicePositions.Contains(4)
            || remainingFreeSlicePositions.Contains(5))
            && remainingSystemTiles.Exists(x => x.TileCategory == SystemTileCategory.Blue))
        {
            remainingSystemTiles = remainingSystemTiles
                .OrderByDescending(x => x.TileCategory == SystemTileCategory.Blue)
                .ToList();

            if (remainingFreeSlicePositions.Contains(5) && slice.DraftedSystemTiles.Count == 6)
            {
                _logger.LogInformation("Placing blue tile in position 5, {RemainingPositions}", string.Join(",", remainingFreeSlicePositions.Select(x => x)));
                slice.DraftedSystemTiles[5] = remainingSystemTiles[0];
                remainingSystemTiles.RemoveAt(0);
                remainingFreeSlicePositions = remainingFreeSlicePositions.Where(x => x != 5).ToList();
            }
            else if (remainingFreeSlicePositions.Contains(4) && slice.DraftedSystemTiles.Count == 6)
            {
                _logger.LogInformation("Placing blue tile in position 5, {RemainingPositions}", string.Join(",", remainingFreeSlicePositions.Select(x => x)));
                slice.DraftedSystemTiles[4] = remainingSystemTiles[0];
                remainingSystemTiles.RemoveAt(0);
                remainingFreeSlicePositions = remainingFreeSlicePositions.Where(x => x != 4).ToList();
            }
        }

        foreach (var position in remainingFreeSlicePositions)
        {
            if (remainingSystemTiles.Count == 0)
            {
                break;
            }

            slice.DraftedSystemTiles[position] = remainingSystemTiles[0];
            remainingSystemTiles.RemoveAt(0);
        }

        _logger.LogInformation("Slice {SliceId} has been set with system tiles: {Code}", slice.Id, slice.DraftedSystemTiles.GetSystemTileCodes());

        return slice;
    }
}
