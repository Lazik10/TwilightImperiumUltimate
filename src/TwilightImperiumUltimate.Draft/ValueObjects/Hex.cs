using System.Collections.ObjectModel;
using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.ValueObjects;

public class Hex(int x, int y, string name = " _ ")
{
    public int X { get; set; } = x;

    public int Y { get; set; } = y;

    public Collection<Hex> Neighbors { get; init; } = new Collection<Hex>();

    public string Name { get; set; } = name;

    public SystemTile? SystemTile { get; set; }

    public string PlayerName { get; set; } = string.Empty;

    public void AddCustomNeighbor(Hex neighbor)
    {
        if (neighbor is not null && !Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);

            // Ensure bidirectional relationship
            neighbor.Neighbors.Add(this);
        }
    }
}
