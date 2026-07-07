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
}
