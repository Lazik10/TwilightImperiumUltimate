﻿using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Draft.Settings;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders.ThreePlayers;

internal class SnowflakeMapThreePlayersMapBuilder : BaseMapBuilder
{
    private static readonly Random Random = new();

    public SnowflakeMapThreePlayersMapBuilder(ISystemTileRepository systemTileRepository)
        : base(systemTileRepository)
    {
    }

    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateSettings.MaxTilePositionsSnowflakeMap);

    public override async Task<Dictionary<int, SystemTile>> GenerateGalaxy(bool previewMap)
    {
        var galaxy = new Dictionary<int, SystemTile>();

        var tiles = SystemTileRepository.GetSystemTilesForBuildingGalaxy().OrderBy(x => Random.Next()).ToList();
        var homeTiles = SystemTileRepository.GetHomeSystemTiles().OrderBy(x => Random.Next()).ToList();
        var homeTilePlaceholder = SystemTileRepository.GetHomeSystemTilePlaceholderTile();
        var emptyTilePlaceholder = SystemTileRepository.GetEmptySystemTilePlaceholderTile();
        var mecatolRex = SystemTileRepository.GetMecatolRex();

        foreach (var position in MapPositions)
        {
            if (IsMecatolRexSpot(position))
            {
                galaxy.Add(position, mecatolRex);
                continue;
            }

            if (IsEmptySpot(position))
            {
                galaxy.Add(position, emptyTilePlaceholder);
                continue;
            }

            if (previewMap)
            {
                if (IsHomeSpot(position))
                {
                    galaxy.Add(position, homeTilePlaceholder);
                    continue;
                }

                var tile = emptyTilePlaceholder;
                galaxy.Add(position, tile);
            }
            else
            {
                if (IsHomeSpot(position))
                {
                    var homeTile = homeTiles[0];
                    galaxy.Add(position, homeTile);
                    homeTiles.Remove(homeTile);
                    continue;
                }

                var tile = tiles[0];
                galaxy.Add(position, tile);
                tiles.Remove(tile);
            }
        }

        return await Task.FromResult(galaxy);
    }

    private bool IsHomeSpot(int mapPosition)
    {
        var homeSpots = new[] { 10, 29, 33 };
        return homeSpots.Contains(mapPosition);
    }

    private bool IsMecatolRexSpot(int mapPosition)
    {
        return mapPosition == 24;
    }

    private bool IsEmptySpot(int mapPosition)
    {
        var emptySpots = new[] { 0, 1, 3, 5, 6, 7, 13, 17, 23, 25, 28, 34, 35, 41 };
        return emptySpots.Contains(mapPosition);
    }
}
