namespace TwilightImperiumUltimate.Web.Services.SliceGenerators;

public interface ISliceGeneratorService
{
    IReadOnlyList<SystemTileModel> AllSystemTiles { get; }

    IReadOnlyList<SliceModel> Slices { get; }

    Task InitializeAllSystemTilesForSliceGenerator();

    IEnumerable<SystemTileModel> GetSystemTilesToShow(SystemTileTypeFilter systemTileType);

    Task GeneratePreviewSlices();

    Task GenerateSlices(bool previewSlices);

    Task AddSlice();

    Task RemoveSlice();
}
