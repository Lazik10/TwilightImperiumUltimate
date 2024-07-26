using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;

public interface IPreviewMapBuilder
{
    Task<IReadOnlyDictionary<int, SystemTile>> BuildPreviewMapLayout(IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup);
}