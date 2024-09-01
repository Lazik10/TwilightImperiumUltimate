namespace TwilightImperiumUltimate.Web.Components.PreviewMaps;

public partial class PreviewMapHex
{
    [Parameter]
    public SystemTileModel SystemTile { get; set; } = new SystemTileModel();

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImagePath => PathProvider.GetLargeTileImagePath(SystemTile?.SystemTileName ?? SystemTileName.TileEmpty);
}