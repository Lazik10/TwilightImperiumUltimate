using System.Collections.ObjectModel;
using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapBuilders;

internal class PreviewMapBuilder : IPreviewMapBuilder
{
    public Task<IReadOnlyDictionary<int, SystemTile>> BuildPreviewMapLayout(IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup)
    {
        var mapLayout = Enumerable.Range(0, mapSettings.MaxMapPositions).ToDictionary(x => x, x => systemTilesForMapSetup.EmptySystemPlaceholder);

        mapSettings.HomePositions.ToList().ForEach(x => mapLayout[x] = systemTilesForMapSetup.EmptyHomeSystemPlaceholder);

        return Task.FromResult<IReadOnlyDictionary<int, SystemTile>>(new ReadOnlyDictionary<int, SystemTile>(mapLayout));
    }
}
