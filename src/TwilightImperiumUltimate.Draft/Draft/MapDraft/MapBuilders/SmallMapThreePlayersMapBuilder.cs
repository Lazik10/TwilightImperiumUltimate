using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders;

internal class SmallMapThreePlayersMapBuilder : BaseMapBuilder
{
    private static readonly Random Random = new();

    public SmallMapThreePlayersMapBuilder(ISystemTileRepository systemTileRepository)
        : base(systemTileRepository)
    {
    }

    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, 25);

    public override async Task<Dictionary<int, SystemTile>> GenerateGalaxy()
    {
        var galaxy = new Dictionary<int, SystemTile>();

        var tiles = SystemTileRepository.GetAllSystemTiles();

        foreach (var position in MapPositions)
        {
            var tile = tiles.OrderBy(x => Random.Next()).First();
            galaxy.Add(position, tile);
        }

        return await Task.FromResult(galaxy);
    }
}
