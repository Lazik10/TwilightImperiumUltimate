using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

internal interface ISliceBalancer
{
    Task<(List<Slice> Slices, List<SystemTile> UnusesSystemTiles)> BalanceSlices(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        SystemTilesForMapSetup systemTilesForMapSetup,
        SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution,
        GenerateMapRequest request);
}
