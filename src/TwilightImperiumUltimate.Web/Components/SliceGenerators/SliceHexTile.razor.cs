using System.Globalization;
using Serilog;
using TwilightImperiumUltimate.Web.Pages.Tools;
using TwilightImperiumUltimate.Web.Services.SliceGenerators;

namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceHexTile : TwilightImperiumBaseComponent
{
    private TileRotation _tileRotation;

    private int _degrees;

    [Parameter]
    public SystemTileModel? SystemTile { get; set; } = null!;

    [Parameter]
    public int Position { get; set; }

    [Parameter]
    public int SliceId { get; set; }

    [CascadingParameter(Name = "SliceGeneratorPage")]
    public SliceGenerator SliceGeneratorPage { get; set; } = default!;

    [Parameter]
    public EventCallback SwappedTwoSystemTiles { get; set; }

    [Parameter]
    public EventCallback SwappedSystemTileFromMenu { get; set; }

    private string ImagePath => PathProvider.GetLargeTileImagePath(SystemTile?.SystemTileName ?? SystemTileName.TileEmpty);

    [Inject]
    private ISliceGeneratorService SliceGeneratorService { get; set; } = null!;

    [Inject]
    private ISliceGeneratorSettingsService SliceGeneratorSettingsService { get; set; } = null!;

    private SystemTileOverlay Overlay => SliceGeneratorSettingsService.MenuOverlay;

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

    protected override async Task OnParametersSetAsync()
    {
        if (SystemTile!.SystemTileCategory == SystemTileCategory.Hyperlane)
            await HandleInitialRotation();
    }

    private Task HandleInitialRotation()
    {
        _tileRotation = SystemTile!.SystemTileCode switch
        {
            string code when code.Contains("A1", StringComparison.Ordinal) || code.Contains("B1", StringComparison.Ordinal) => TileRotation.Rotation60,
            string code when code.Contains("A2", StringComparison.Ordinal) || code.Contains("B2", StringComparison.Ordinal) => TileRotation.Rotation120,
            string code when code.Contains("A3", StringComparison.Ordinal) || code.Contains("B3", StringComparison.Ordinal) => TileRotation.Rotation180,
            string code when code.Contains("A4", StringComparison.Ordinal) || code.Contains("B4", StringComparison.Ordinal) => TileRotation.Rotation240,
            string code when code.Contains("A5", StringComparison.Ordinal) || code.Contains("B5", StringComparison.Ordinal) => TileRotation.Rotation300,
            _ => TileRotation.Rotation0,
        };

        _degrees = (int)_tileRotation * 60;

        return Task.CompletedTask;
    }

    private async Task StartDragSystemTile()
    {
        if (SystemTile is not null)
            await SliceGeneratorService.SetDraggedSystemTile(SystemTile, Position, SliceId);
    }

    private void DragOverSystemTile()
    {
        Log.Information("Draging over system tile: {TileName}", SystemTile?.SystemTileName.ToString());
    }

    private void EndDragSystemTile()
    {
        Log.Information("Drag ended for system tile: {TileName}", SystemTile?.SystemTileName.ToString());
    }

    private async Task DropSystemTile()
    {
        if (SystemTile is not null)
        {
            var draggedSystemTile = await SliceGeneratorService.GetCurrentDraggingSystemTile();
            Log.Information("Dragged system tile was: {TileName}", draggedSystemTile?.SystemTileCode);
            Log.Information("Dropped on system tile: {TileName}", SystemTile.SystemTileCode);
            await SliceGeneratorService.SwitchDraggingSystemTileWithDropSystemTile(SystemTile, SliceId, Position);
            await SliceGeneratorPage.Reload();
            StateHasChanged();
        }
    }

    private void Rotate()
    {
        if (SystemTile!.SystemTileCategory != SystemTileCategory.Hyperlane)
            return;

        if (_tileRotation == TileRotation.Rotation300)
        {
            _tileRotation = TileRotation.Rotation0;
            _degrees = 0;
        }
        else
        {
            _tileRotation++;
            _degrees += 60;
        }

        SystemTile.SystemTileCode = SystemTile.SystemTileCode switch
        {
            string code when code.Contains('A', StringComparison.Ordinal) && code.Length == 3 => code.Replace("A", "A1", StringComparison.Ordinal),
            string code when code.Contains("A0", StringComparison.Ordinal) => code.Replace("A0", "A1", StringComparison.Ordinal),
            string code when code.Contains("A1", StringComparison.Ordinal) => code.Replace("A1", "A2", StringComparison.Ordinal),
            string code when code.Contains("A2", StringComparison.Ordinal) => code.Replace("A2", "A3", StringComparison.Ordinal),
            string code when code.Contains("A3", StringComparison.Ordinal) => code.Replace("A3", "A4", StringComparison.Ordinal),
            string code when code.Contains("A4", StringComparison.Ordinal) => code.Replace("A4", "A5", StringComparison.Ordinal),
            string code when code.Contains("A5", StringComparison.Ordinal) => code.Replace("A5", "A0", StringComparison.Ordinal),
            string code when code.Contains('B', StringComparison.Ordinal) && code.Length == 3 => code.Replace("B", "B1", StringComparison.Ordinal),
            string code when code.Contains("B0", StringComparison.Ordinal) => code.Replace("B0", "B1", StringComparison.Ordinal),
            string code when code.Contains("B1", StringComparison.Ordinal) => code.Replace("B1", "B2", StringComparison.Ordinal),
            string code when code.Contains("B2", StringComparison.Ordinal) => code.Replace("B2", "B3", StringComparison.Ordinal),
            string code when code.Contains("B3", StringComparison.Ordinal) => code.Replace("B3", "B4", StringComparison.Ordinal),
            string code when code.Contains("B4", StringComparison.Ordinal) => code.Replace("B4", "B5", StringComparison.Ordinal),
            string code when code.Contains("B5", StringComparison.Ordinal) => code.Replace("B5", "B0", StringComparison.Ordinal),
            _ => string.Empty,
        };
    }
}
