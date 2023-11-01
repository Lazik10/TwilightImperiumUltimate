using TwilightImperiumUltimate.Core.Enums.Galaxy;
using TwilightImperiumUltimate.Draft.Draft.MapDraft.MapBuilders;

namespace TwilightImperiumUltimate.Draft.Draft.MapDraft;

public interface IMapBuilderProvider
{
    Task<IMapBuilder> GetMapBuilder(MapTemplate mapTemplate);
}
