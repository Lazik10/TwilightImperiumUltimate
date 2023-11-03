using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft;

public class MapDraftResult
{
    public IReadOnlyDictionary<int, SystemTile> MapTiles { get; set; } = new Dictionary<int, SystemTile>();
}
