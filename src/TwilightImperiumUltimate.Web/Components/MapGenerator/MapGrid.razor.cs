namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGrid
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTileModel> GeneratedPositionsWithSystemTiles { get; set; } = default!;

    [Parameter]
    public IEnumerable<int> MapPositions { get; set; } = default!;

    private SystemTileModel CurrentSystemTile { get; set; } = default!;
}
