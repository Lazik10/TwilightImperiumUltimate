using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface IPreviewMapBuilder
{
    Task<Dictionary<(int X, int Y), Hex>> CreatePreviewMapLayout(IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup);
}