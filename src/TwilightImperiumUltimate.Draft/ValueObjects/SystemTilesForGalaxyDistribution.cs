using System.Collections.ObjectModel;
using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.ValueObjects;

public class SystemTilesForGalaxyDistribution
{
    public List<SystemTile> BlueTiles { get; set; } = new List<SystemTile>();

    public List<SystemTile> RedTiles { get; set; } = new List<SystemTile>();

    public int TilesCount => BlueTiles.Count + RedTiles.Count;

    public int BlueTilesCount => BlueTiles.Count;

    public int RedTilesCount => RedTiles.Count;
}
