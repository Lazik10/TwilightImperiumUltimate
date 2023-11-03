using TwilightImperiumUltimate.Web.Components.MapGenerator;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorService
{
    IReadOnlyDictionary<int, SystemTile> GeneratedPositionsWithSystemTiles { get; }

    Task<IReadOnlyDictionary<int, SystemTile>> GenerateMapAsync(bool previewMap);

    Task InitializeSystemTilesAsync();

    IEnumerable<SystemTile> GetSystemTilesToShow(SystemTileTypeFilter systemTileType);

    void SetDraggingSystemTile(MapHexTile mapHexTile);
}
