using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

public class MapToStringConvertService : IMapToStringConvertService
{
    public Task<string> ConvertMapLayoutToString(Dictionary<string, SystemTile> mapLayout, MapTemplate mapTemplate)
    {
        return Task.FromResult(string.Empty);
    }
}
