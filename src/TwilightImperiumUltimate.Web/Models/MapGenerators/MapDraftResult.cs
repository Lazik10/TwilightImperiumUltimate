using TwilightImperiumUltimate.Web.Models.Galaxy;

namespace TwilightImperiumUltimate.Web.Models.MapGenerators;

public class MapDraftResult
{
    public IReadOnlyDictionary<int, SystemTileModel> MapTiles { get; set; } = new Dictionary<int, SystemTileModel>();
}
