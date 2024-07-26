using System.Collections.ObjectModel;
using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;

internal class ThreePlayersTridentMapBuilder : MapBuilderBase
{
    public override async Task<IReadOnlyDictionary<int, SystemTile>> BuildMapLayout(
        IMapSettings mapSettings,
        GenerateMapRequest request,
        SystemTilesForMapSetup systemTilesForMapSetup)
    {
        // TODO: Implement logic for another placement styles
        /*if (request.PlacementStyle == PlacementStyle.Random)*/

        var mapLayout = CreateRandomMapLayoutForSpecificMap(mapSettings, request, systemTilesForMapSetup);

        return await Task.FromResult<IReadOnlyDictionary<int, SystemTile>>(new ReadOnlyDictionary<int, SystemTile>(mapLayout));
    }
}
