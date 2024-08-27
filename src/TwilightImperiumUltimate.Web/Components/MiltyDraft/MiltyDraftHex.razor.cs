namespace TwilightImperiumUltimate.Web.Components.MiltyDraft;

public partial class MiltyDraftHex
{
    [Parameter]
    public SystemTileModel SystemTile { get; set; } = new SystemTileModel();

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImagePath => PathProvider.GetLargeTileImagePath(SystemTile?.SystemTileName ?? SystemTileName.TileEmpty);
}