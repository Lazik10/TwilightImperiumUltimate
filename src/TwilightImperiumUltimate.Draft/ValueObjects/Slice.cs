using System.Collections.ObjectModel;

namespace TwilightImperiumUltimate.Draft.ValueObjects;

public class Slice
{
    public int Id { get; set; }

    public Collection<SlicePosition> Positions { get; } = new Collection<SlicePosition>();
}
