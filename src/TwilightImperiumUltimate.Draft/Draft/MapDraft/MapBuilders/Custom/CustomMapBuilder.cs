using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.DataAccess.Repositories;
using TwilightImperiumUltimate.Draft.Settings;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders.Custom;

internal class CustomMapBuilder : BaseMapBuilder
{
    private static readonly Random Random = new();

    public CustomMapBuilder(ISystemTileRepository systemTileRepository)
        : base(systemTileRepository)
    {
    }

    protected override IEnumerable<int> MapPositions { get; set; } = Enumerable.Range(0, MapTemplateSettings.MaxTilePositionsCustomMap);

    public override async Task<Dictionary<int, SystemTile>> GenerateGalaxy(bool previewMap)
    {
        var galaxy = new Dictionary<int, SystemTile>();

        var tiles = SystemTileRepository.GetAllSystemTiles();
        var emptyTilePlaceholder = SystemTileRepository.GetEmptySystemTilePlaceholderTile();

        foreach (var position in MapPositions)
        {
            if (previewMap)
            {
                galaxy.Add(position, emptyTilePlaceholder);
                continue;
            }

            var tile = tiles.OrderBy(x => Random.Next()).First();
            galaxy.Add(position, tile);
        }

        return await Task.FromResult(galaxy);
    }
}
