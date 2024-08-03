using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

internal interface IPlacementStyleHandler
{
    public Task HandleRemainingPositions(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        SystemTilesForMapSetup systemTilesForMapSetup,
        GenerateMapRequest request);
}