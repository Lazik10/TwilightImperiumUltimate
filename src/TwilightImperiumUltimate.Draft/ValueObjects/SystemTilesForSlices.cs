using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.ValueObjects;

public class SystemTilesForSlices
{
    public IReadOnlyCollection<SystemTile> BlueSystemTiles { get; set; } = new List<SystemTile>();

    public IReadOnlyCollection<SystemTile> RedSystemTiles { get; set; } = new List<SystemTile>();

    public IReadOnlyCollection<SystemTile> LegendarySystemTiles { get; set; } = new List<SystemTile>();

    public IReadOnlyCollection<SystemTile> WormholeSystemTiles { get; set; } = new List<SystemTile>();
}
