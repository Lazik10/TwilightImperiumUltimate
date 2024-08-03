using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface ISystemTileSetter
{
    void SetMecatolSystemTile(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        SystemTile mecatolRexSystemTile);

    void SetHomeSystemTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        IMapSettings mapSettings,
        HomeSystemDraftType homeSystemDraftType,
        IReadOnlyCollection<FactionName> factions);

    void SetRedSystemTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        IReadOnlyCollection<SystemTile> redTiles,
        IMapSettings mapSettings,
        GenerateMapRequest request);

    void SetRemainingSystemTilesRandomly(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution);
}
