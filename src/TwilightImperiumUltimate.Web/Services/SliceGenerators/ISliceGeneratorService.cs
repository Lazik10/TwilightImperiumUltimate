namespace TwilightImperiumUltimate.Web.Services.SliceGenerators;

public interface ISliceGeneratorService
{
    IReadOnlyList<SystemTileModel> AllSystemTiles { get; }

    IReadOnlyList<SliceModel> Slices { get; }

    Task InitializeAllSystemTilesForSliceGenerator();

    IEnumerable<SystemTileModel> GetSystemTilesToShow(SystemTileTypeFilter systemTileType);

    Task GeneratePreviewSlices();

    Task GenerateSlices(bool previewSlices);

    Task SetImportedSlices(IReadOnlyCollection<SliceModel> slices);

    Task AddSlice();

    Task RemoveSlice();

    Task SetDraggedSystemTile(
        SystemTileModel systemTile,
        int draggedSystemTileSlicePosition,
        int draggedSystemTileSliceId);

    Task<SystemTileModel?> GetCurrentDraggingSystemTile();

    Task SwitchDraggingSystemTileWithDropSystemTile(
        SystemTileModel droppedSystemTile,
        int droppedSystemTileSliceId,
        int droppedSystemTileSlicePosition);
}
