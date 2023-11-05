using Microsoft.AspNetCore.Components;
using Serilog;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Services.MapGenerators;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapHexTile
{
    [Parameter]
    public SystemTile SystemTile { get; set; } = null!;

    [Parameter]
    public string ImagePath { get; set; } = string.Empty;

    [Parameter]
    public string MapTileId { get; set; } = string.Empty;

    [Parameter]
    public SystemTileOverlay SystemTileOverlay { get; set; } = SystemTileOverlay.None;

    [Parameter]
    public int MapPosition { get; set; }

    [Parameter]
    public EventCallback SwappedTwoSystemTiles { get; set; }

    [Parameter]
    public EventCallback SwappedSystemTileFromMenu { get; set; }

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = null!;

    private void StartDragSystemTile(SystemTile systemTile)
    {
        MapGeneratorService.SetDraggingSystemTile(systemTile);
        MapGeneratorService.SetDraggingSystemTilePosition(MapPosition);
        Log.Information("Drag started for system tile: {TileName}", systemTile.Name.ToString());
    }

    private void DropSystemTile(SystemTile systemTile)
    {
        if (systemTile != null)
        {
            var draggedSystemTile = MapGeneratorService.GetCurrentDraggingSystemTile();
            Log.Information("Dragged system tile was: {TileName}", draggedSystemTile.Name.ToString());
            Log.Information("Dropped on system tile: {TileName}", systemTile.Name.ToString());
            MapGeneratorService.SwapSystemTiles(systemTile, MapPosition);
            SwappedTwoSystemTiles.InvokeAsync();
            SwappedSystemTileFromMenu.InvokeAsync();
            StateHasChanged();
            MapGeneratorService.ResetDraggingSystemTile(systemTile);
        }
    }

    private void DragOverSystemTile(SystemTile systemTile)
    {
        Log.Information("Draging over system tile: {TileName}", systemTile.Name.ToString());
    }

    private void EndDragSystemTile(SystemTile systemTile)
    {
        Log.Information("Drag ended for system tile: {TileName}", systemTile.Name.ToString());
        SwappedSystemTileFromMenu.InvokeAsync();
    }
}
