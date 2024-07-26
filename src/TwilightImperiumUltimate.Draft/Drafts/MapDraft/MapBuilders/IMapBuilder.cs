using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;

public interface IMapBuilder
{
    Task<IReadOnlyDictionary<int, SystemTile>> BuildMapLayout(IMapSettings mapSettings, GenerateMapRequest request, SystemTilesForMapSetup systemTilesForMapSetup);
}
