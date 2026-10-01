using System.Globalization;
using Serilog;
using TwilightImperiumUltimate.Web.Pages.Tools;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceHex : IDisposable
{
    [Parameter]
    public SystemTileModel SystemTile { get; set; } = new SystemTileModel();

    [Parameter]
    public int SliceId { get; set; }

    [Parameter]
    public int SlicePosition { get; set; }

    [CascadingParameter(Name = "SliceGeneratorPage")]
    public SliceGenerator SliceGeneratorPage { get; set; } = default!;

    [CascadingParameter(Name = "SliceGeneratorGrid")]
    public SliceGeneratorGrid SliceGeneratorGrid { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private ISliceGeneratorSettingsService SliceGeneratorSettingsService { get; set; } = default!;

    [Inject]
    private ISliceGeneratorService SliceGeneratorService { get; set; } = default!;

    private bool IsTileSelected =>
        SliceGeneratorService.IsSystemTileSelected(SystemTile, SlicePosition, SliceId);

    private SystemTileOverlay Overlay => SliceGeneratorSettingsService.SliceOverlay;

    private string ImagePath => PathProvider.GetLargeTileImagePath(SystemTile?.SystemTileName ?? SystemTileName.TileEmpty);

    private string SystemTileOverlayText => Overlay switch
    {
        SystemTileOverlay.Id => SystemTile?.SystemTileCode ?? string.Empty,
        SystemTileOverlay.Resources => SystemTile?.Resources.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
        SystemTileOverlay.Influence => SystemTile?.Influence.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
        _ => string.Empty,
    };

    private string SystemTileOverlayColor => Overlay switch
    {

        SystemTileOverlay.Id => "white",
        SystemTileOverlay.Resources => "yellow",
        SystemTileOverlay.Influence => "blue",
        SystemTileOverlay.MapPosition => "white",
        _ => string.Empty,
    };

    protected override void OnInitialized() =>
        SliceGeneratorService.TileSelectionChanged += HandleTileSelectionChanged;

    public void Dispose() =>
        SliceGeneratorService.TileSelectionChanged -= HandleTileSelectionChanged;

    private async Task StartDragSystemTile()
    {
        await SliceGeneratorService.SetDraggedSystemTile(SystemTile, SlicePosition, SliceId);
        Log.Information(
            "Starting to drag a system tile: {TileName}, slice ID: {SliceId}, slice position: {SlicePosition}",
            SystemTile.SystemTileCode,
            SliceId,
            SlicePosition);
    }

    private void DragOverSystemTile()
    {
        Log.Information("Draging over system tile: {TileName}", SystemTile?.SystemTileName.ToString());
    }

    private void EndDragSystemTile()
    {
        Log.Information("Drag ended for system tile: {TileName}", SystemTile?.SystemTileName.ToString());
        StateHasChanged();
    }

    private async Task DropSystemTile()
    {
        if (SystemTile is not null)
        {
            var draggedSystemTile = await SliceGeneratorService.GetCurrentDraggingSystemTile();
            Log.Information("Dragged system tile was: {TileName}", draggedSystemTile?.SystemTileCode);
            Log.Information("Dropped on system tile: {TileName}", SystemTile.SystemTileCode);
            await SliceGeneratorService.SwitchDraggingSystemTileWithDropSystemTile(SystemTile, SliceId, SlicePosition);
            SliceGeneratorGrid.UpdateSliceHexTileMenu();
            await SliceGeneratorPage.Reload();
            StateHasChanged();
        }
    }

    private Task RequestDropSystemTile() =>
        SliceGeneratorGrid.RequestSystemTileSwapAsync(DropSystemTile, SliceId, SlicePosition);

    private async Task HandleTileClick()
    {
        if (IsTileSelected)
        {
            await SliceGeneratorService.ClearSystemTileSelection();
            return;
        }

        if (SliceGeneratorService.HasSelectedSystemTile)
        {
            await RequestDropSystemTile();
            return;
        }

        await StartDragSystemTile();
        await SliceGeneratorService.SelectSystemTile(SystemTile, SlicePosition, SliceId);
    }

    private void HandleTileSelectionChanged() =>
        _ = InvokeAsync(StateHasChanged);
}
