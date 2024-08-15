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
        IReadOnlyCollection<FactionName> factions,
        IReadOnlyCollection<string> playerNames);

    void SetRedSystemTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        IMapSettings mapSettings,
        GenerateMapRequest request);

    void SetLegendarySystemTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForGalaxyDistribution remainingSystemTiles,
        IMapSettings mapSettings);

    void SetRemainingSystemTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        GenerateMapRequest request,
        IReadOnlyCollection<Slice> balancedSlices);

    void SetHyperlines(
        Dictionary<(int X, int Y), Hex> galaxy,
        IHyperlineSettings hyperlineSettings,
        SystemTilesForMapSetup systemTilesForMapSetup);

    void SetTransparentTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTile transparentSystemPlaceholder);
}
