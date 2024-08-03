using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.ValueObjects;

public class SlicePosition
{
    public int X { get; set; }

    public int Y { get; set; }

    public SystemTile? SystemTile { get; set; }
}
