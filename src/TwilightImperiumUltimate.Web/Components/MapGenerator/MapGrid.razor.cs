using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Models.Galaxy;

namespace TwilightImperiumUltimate.Web.Components.MapGenerator;

public partial class MapGrid
{
    [Parameter]
    public IReadOnlyDictionary<int, SystemTile> GeneratedPositionsWithSystemTiles { get; set; } = default!;

    [Parameter]
    public IEnumerable<int> MapPositions { get; set; } = default!;

    private SystemTile CurrentSystemTile { get; set; } = default!;
}
