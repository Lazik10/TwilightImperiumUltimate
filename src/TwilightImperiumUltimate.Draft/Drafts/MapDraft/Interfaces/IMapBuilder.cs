using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface IMapBuilder
{
    Task<Dictionary<(int X, int Y), Hex>> CreateMapLayout(
        IMapSettings mapSettings,
        GenerateMapRequest request,
        SystemTilesForMapSetup systemTilesForMapSetup);
}
