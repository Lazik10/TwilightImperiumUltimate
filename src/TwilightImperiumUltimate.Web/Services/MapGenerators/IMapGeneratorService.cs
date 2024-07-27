namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorService
{
    IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; }

    Task<Dictionary<int, SystemTileModel>> GenerateMapAsync(bool previewMap, CancellationToken ct);

    Task InitializeSystemTilesAsync(CancellationToken ct);

    IEnumerable<SystemTileModel> GetSystemTilesToShow(SystemTileTypeFilter systemTileType);

    void SetDraggingSystemTile(SystemTileModel systemTile);

    void SetDraggingSystemTilePosition(int draggedSystemTileStartMapPosition);

    void ResetDraggingSystemTile(SystemTileModel systemTile);

    void SwapSystemTiles(SystemTileModel systemTile, int mapPosition);

    SystemTileModel GetCurrentDraggingSystemTile();
}
