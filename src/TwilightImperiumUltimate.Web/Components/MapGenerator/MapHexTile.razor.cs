using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapHexTile
{
    [Parameter]
    public string ImagePath { get; set; } = string.Empty;

    [Parameter]
    public string MapTileId { get; set; } = string.Empty;

    [Parameter]
    public bool ShowTileId { get; set; } = false;
}
