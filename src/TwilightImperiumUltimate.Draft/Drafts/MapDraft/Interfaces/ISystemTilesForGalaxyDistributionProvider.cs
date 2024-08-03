using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface ISystemTilesForGalaxyDistributionProvider
{
    public SystemTilesForGalaxyDistribution GetRemainingSystemTilesForMapDistribution(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        IMapSettings mapSettings,
        GenerateMapRequest request);
}
