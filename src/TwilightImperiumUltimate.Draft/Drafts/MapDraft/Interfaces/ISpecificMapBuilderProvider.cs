using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface ISpecificMapBuilderProvider
{
    public Task<IMapBuilder> GetMapBuilderForSpecificTemplate(MapTemplate mapTemplate);
}
