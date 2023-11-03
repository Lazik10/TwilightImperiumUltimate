using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders;

public interface IMapBuilder
{
    Task<Dictionary<int, SystemTile>> GenerateGalaxy(bool previewMap);
}
