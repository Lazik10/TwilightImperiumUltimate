using Serilog;
using System.Globalization;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapHexTile : TwilightImperiumBaseComponenet
{
    private TileRotation _tileRotation;

    private int _degrees;

    [Parameter]
    public SystemTileModel? SystemTile { get; set; } = null!;

    [Parameter]
    public int MapPosition { get; set; }

    [Parameter]
    public EventCallback SwappedTwoSystemTiles { get; set; }

    [Parameter]
    public EventCallback SwappedSystemTileFromMenu { get; set; }

    private string ImagePath => PathProvider.GetLargeTileImagePath(SystemTile?.SystemTileName ?? SystemTileName.TileEmpty);

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = null!;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = null!;

    private SystemTileOverlay Overlay => MapGeneratorSettingsService.SystemTileOverlay;

    private string SystemTileOverlayText => MapGeneratorSettingsService.SystemTileOverlay switch
    {
        SystemTileOverlay.Id => SystemTile?.SystemTileCode ?? string.Empty,
        SystemTileOverlay.Resources => SystemTile?.Resources.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
        SystemTileOverlay.Influence => SystemTile?.Influence.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
        _ => string.Empty,
    };

    private string SystemTileOverlayColor => MapGeneratorSettingsService.SystemTileOverlay switch
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
            string code when code.Contains("A1") || code.Contains("B1") => TileRotation.Rotation60,
            string code when code.Contains("A2") || code.Contains("B2") => TileRotation.Rotation120,
            string code when code.Contains("A3") || code.Contains("B3") => TileRotation.Rotation180,
            string code when code.Contains("A4") || code.Contains("B4") => TileRotation.Rotation240,
            string code when code.Contains("A5") || code.Contains("B5") => TileRotation.Rotation300,
            _ => TileRotation.Rotation0,
        };

        _degrees = (int)_tileRotation * 60;

        return Task.CompletedTask;
    }

    private void StartDragSystemTile(SystemTileModel systemTile)
    {
        MapGeneratorService.SetDraggingSystemTile(systemTile);
        MapGeneratorService.SetDraggingSystemTilePosition(MapPosition);
        Log.Information("Drag started for system tile: {TileName}", systemTile.SystemTileName.ToString());
    }

    private void DropSystemTile(SystemTileModel systemTile)
    {
        if (systemTile != null)
        {
            var draggedSystemTile = MapGeneratorService.GetCurrentDraggingSystemTile();
            Log.Information("Dragged system tile was: {TileName}", draggedSystemTile.SystemTileName.ToString());
            Log.Information("Dropped on system tile: {TileName}", systemTile.SystemTileName.ToString());
            MapGeneratorService.SwapSystemTiles(systemTile, MapPosition);
            SwappedTwoSystemTiles.InvokeAsync();
            SwappedSystemTileFromMenu.InvokeAsync();
            StateHasChanged();
            MapGeneratorService.ResetDraggingSystemTile(systemTile);
        }
    }

    private void DragOverSystemTile(SystemTileModel systemTile)
    {
        Log.Information("Draging over system tile: {TileName}", systemTile.SystemTileName.ToString());
    }

    private void EndDragSystemTile(SystemTileModel systemTile)
    {
        Log.Information("Drag ended for system tile: {TileName}", systemTile.SystemTileName.ToString());
        SwappedSystemTileFromMenu.InvokeAsync();
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
            string code when code.Contains('A') && code.Length == 3 => code.Replace("A", "A1"),
            string code when code.Contains("A0") => code.Replace("A0", "A1"),
            string code when code.Contains("A1") => code.Replace("A1", "A2"),
            string code when code.Contains("A2") => code.Replace("A2", "A3"),
            string code when code.Contains("A3") => code.Replace("A3", "A4"),
            string code when code.Contains("A4") => code.Replace("A4", "A5"),
            string code when code.Contains("A5") => code.Replace("A5", "A0"),
            string code when code.Contains('B') && code.Length == 3 => code.Replace("B", "B1"),
            string code when code.Contains("B0") => code.Replace("B0", "B1"),
            string code when code.Contains("B1") => code.Replace("B1", "B2"),
            string code when code.Contains("B2") => code.Replace("B2", "B3"),
            string code when code.Contains("B3") => code.Replace("B3", "B4"),
            string code when code.Contains("B4") => code.Replace("B4", "B5"),
            string code when code.Contains("B5") => code.Replace("B5", "B0"),
            _ => string.Empty,
        };
    }
}
