namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorService
{
    event Action? TileSelectionChanged;

    IEnumerable<SystemTileModel> AllSystemTiles { get; set; }

    IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; }

    bool HasSelectedSystemTile { get; }

    int SelectedSystemTileMapPosition { get; }

    Task<Dictionary<int, SystemTileModel>> GenerateMapAsync(bool previewMap, CancellationToken ct);

    Task<Dictionary<int, SystemTileModel>> GenerateMapLayoutPreview(MapTemplate mapTemplate);

    Task InitializeSystemTilesAsync(CancellationToken ct);

    IEnumerable<SystemTileModel> GetSystemTilesToShow(SystemTileTypeFilter systemTileType);

    void SetDraggingSystemTile(SystemTileModel systemTile);

    void SetDraggingSystemTilePosition(int draggedSystemTileStartMapPosition);

    void ResetDraggingSystemTile(SystemTileModel systemTile);

    void SelectSystemTile(SystemTileModel systemTile, int mapPosition);

    bool IsSystemTileSelected(SystemTileModel systemTile, int mapPosition);

    bool WouldPlacementCreateDuplicate(int mapPosition);

    void SwapSystemTiles(SystemTileModel systemTile, int mapPosition);

    SystemTileModel GetCurrentDraggingSystemTile();

    string GetMapString();

    Task InitializeMapFromLink(Dictionary<int, SystemTileModel> map);
}
