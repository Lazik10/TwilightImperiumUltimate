namespace TwilightImperiumUltimate.Web.Services.SliceGenerators;

public interface ISliceGeneratorService
{
    IReadOnlyList<SystemTileModel> AllSystemTiles { get; }

    Task InitializeEmptySlices(int numberOfSlices);

    IReadOnlyList<SliceModel> Slices { get; }

    Task InitializeAllSystemTilesForSliceGenerator();

    IEnumerable<SystemTileModel> GetSystemTilesToShow(SystemTileTypeFilter systemTileType);

    Task GeneratePreviewSlices();

    Task GenerateSlices(bool previewSlices);

    event Action? TileSelectionChanged;

    bool HasSelectedSystemTile { get; }

    int SelectedSystemTileSliceId { get; }

    int SelectedSystemTileSlicePosition { get; }

    Task SetImportedSlices(IReadOnlyCollection<SliceModel> slices);

    Task AddSlice();

    Task RemoveSlice();

    Task SetDraggedSystemTile(
        SystemTileModel systemTile,
        int draggedSystemTileSlicePosition,
        int draggedSystemTileSliceId);

    Task<SystemTileModel?> GetCurrentDraggingSystemTile();

    Task SelectSystemTile(SystemTileModel systemTile, int slicePosition, int sliceId);

    Task ClearSystemTileSelection();

    bool IsSystemTileSelected(SystemTileModel systemTile, int slicePosition, int sliceId);

    bool WouldPlacementCreateDuplicate(int sliceId, int slicePosition);

    Task SwitchDraggingSystemTileWithDropSystemTile(
        SystemTileModel droppedSystemTile,
        int droppedSystemTileSliceId,
        int droppedSystemTileSlicePosition);
}
