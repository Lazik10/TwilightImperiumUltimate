using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Services.MapGenerators;
using TwilightImperiumUltimate.Web.Services.Path;

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
    public bool ShowTileId { get; set; } = false;

    [Parameter]
    public EventCallback<string> OnDropTile { get; set; }

    [Parameter]
    public EventCallback<string> OnDragTile { get; set; }

    [Inject]
    private IMapGeneratorService MapGeneratorService { get; set; } = null!;

    private void HandleDragStart()
    {
        OnDragTile.InvokeAsync(MapTileId);
        MapGeneratorService.SetDraggingSystemTile(this);
    }

    private void HandleDragEnd()
    {
        OnDragTile.InvokeAsync(MapTileId);
    }

    private async Task HandleDrop()
    {
        await OnDropTile.InvokeAsync(MapTileId);
    }

    private void HandleDragOver(DragEventArgs e)
    {
        // e.Handled = true; // This prevents the default action
    }
}
