using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorService
{
    IReadOnlyDictionary<int, SystemTile> GeneratedPositionsWithSystemTiles { get; }

    Task<Dictionary<int, SystemTile>> GenerateMapAsync(bool previewMap);

    Task InitializeSystemTilesAsync();

    IEnumerable<SystemTile> GetSystemTilesToShow(SystemTileTypeFilter systemTileType);

    void SetDraggingSystemTile(SystemTile systemTile);

    void SetDraggingSystemTilePosition(int draggedSystemTileStartMapPosition);

    void ResetDraggingSystemTile(SystemTile systemTile);

    void SwapSystemTiles(SystemTile systemTile, int mapPosition);

    SystemTile GetCurrentDraggingSystemTile();
}
