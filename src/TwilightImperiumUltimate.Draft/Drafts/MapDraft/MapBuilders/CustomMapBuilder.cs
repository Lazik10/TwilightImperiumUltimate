using System.Collections.ObjectModel;
using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;

internal class CustomMapBuilder : MapBuilderBase
{
    public override Task<IReadOnlyDictionary<int, SystemTile>> BuildMapLayout(
        IMapSettings mapSettings,
        GenerateMapRequest request,
        SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var mapLayout = Enumerable.Range(0, mapSettings.MaxMapPositions)
            .ToDictionary(x => x, x => systemTilesForMapSetup.BuildTiles.ElementAt(Random.Next(systemTilesForMapSetup.BuildTiles.Count)));

        return Task.FromResult<IReadOnlyDictionary<int, SystemTile>>(new ReadOnlyDictionary<int, SystemTile>(mapLayout));
    }
}
