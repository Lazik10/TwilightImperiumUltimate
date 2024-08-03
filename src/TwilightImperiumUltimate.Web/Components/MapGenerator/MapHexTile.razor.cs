using Serilog;
using System.Globalization;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapHexTile : TwilightImperiumBaseComponenet
{
    [Parameter]
    public SystemTileModel? SystemTile { get; set; } = null!;

    [Parameter]
    public int MapPosition { get; set; }

    [Parameter]
    public EventCallback SwappedTwoSystemTiles { get; set; }

    [Parameter]
    public EventCallback SwappedSystemTileFromMenu { get; set; }

    private string ImagePath => PathProvider.GetLargeTileImagePath(SystemTile?.SystemTileName ?? SystemTileName.TileEmpty);

    private SystemTileOverlay Overlay => MapGeneratorSettingsService.SystemTileOverlay;

    private string SystemTileOverlayColor => MapGeneratorSettingsService.SystemTileOverlay switch
    {

        SystemTileOverlay.Id => "white",
        SystemTileOverlay.Resources => "yellow",
        SystemTileOverlay.Influence => "blue",
        SystemTileOverlay.MapPosition => "white",
        _ => string.Empty,
    };

    private string SystemTileOverlayText => MapGeneratorSettingsService.SystemTileOverlay switch
    {
        SystemTileOverlay.Id => SystemTile?.SystemTileCode ?? string.Empty,
        SystemTileOverlay.Resources => SystemTile?.Resources.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
        SystemTileOverlay.Influence => SystemTile?.Influence.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
        _ => string.Empty,
    };

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = null!;

    [Inject]
    private IMapGeneratorSettingsService MapGeneratorSettingsService { get; set; } = null!;

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
}
