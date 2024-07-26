using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface IMapToStringConvertService
{
    public Task<string> ConvertMapLayoutToString(Dictionary<string, SystemTile> mapLayout, MapTemplate mapTemplate);
}
