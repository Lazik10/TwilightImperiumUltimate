using System.Collections.ObjectModel;
using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.ValueObjects;

public class Slice
{
    public int Id { get; set; }

    public Collection<SystemTile> DraftedSystemTiles { get; } = new Collection<SystemTile>();

    public Collection<SlicePosition> Positions { get; } = new Collection<SlicePosition>();
}
