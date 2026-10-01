using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

internal interface IPlacementStyleHandler
{
    Task HandleRemainingPositions(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        SystemTilesForMapSetup systemTilesForMapSetup,
        GenerateMapRequest request);

    Task HandleRemainingNonSlicePositions(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        SystemTilesForMapSetup systemTilesForMapSetup);
}
