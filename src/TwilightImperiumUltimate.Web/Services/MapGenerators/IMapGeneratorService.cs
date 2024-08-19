namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorService
{
    IEnumerable<SystemTileModel> AllSystemTiles { get; set; }

    IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; }

    Task<Dictionary<int, SystemTileModel>> GenerateMapAsync(bool previewMap, CancellationToken ct);

    Task InitializeSystemTilesAsync(CancellationToken ct);

    IEnumerable<SystemTileModel> GetSystemTilesToShow(SystemTileTypeFilter systemTileType);

    void SetDraggingSystemTile(SystemTileModel systemTile);

    void SetDraggingSystemTilePosition(int draggedSystemTileStartMapPosition);

    void ResetDraggingSystemTile(SystemTileModel systemTile);

    void SwapSystemTiles(SystemTileModel systemTile, int mapPosition);

    SystemTileModel GetCurrentDraggingSystemTile();

    string GetMapString();

    Task InitializeMapFromLink(Dictionary<int, SystemTileModel> map);
}
