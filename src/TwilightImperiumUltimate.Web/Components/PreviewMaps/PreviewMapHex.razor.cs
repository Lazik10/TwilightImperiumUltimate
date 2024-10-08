namespace TwilightImperiumUltimate.Web.Components.PreviewMaps;

public partial class PreviewMapHex
{
    private TileRotation _tileRotation;

    private int _degrees;

    [Parameter]
    public SystemTileModel SystemTile { get; set; } = new SystemTileModel();

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImagePath => PathProvider.GetLargeTileImagePath(SystemTile?.SystemTileName ?? SystemTileName.TileEmpty);

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
}