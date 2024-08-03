using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

internal class SliceBalancer : ISliceBalancer
{
    public Task BalanceSlices(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, SystemTilesForMapSetup systemTilesForMapSetup, GenerateMapRequest request)
    {
        return Task.CompletedTask;
    }

    private static List<Slice> AddCorrespondingSystemTileToSlicePosition(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings)
    {
        var slices = new List<Slice>();
        var mapSettingsSlices = mapSettings.Slices;

        foreach (var slice in mapSettingsSlices)
        {
            var newSlice = new Slice();
            newSlice.Id = slice.Key;

            foreach (var slicePosition in slice.Value)
            {
                if (galaxy.TryGetValue(slicePosition, out var hex))
                {
                    newSlice.Positions.Add(new SlicePosition { X = slicePosition.X, Y = slicePosition.Y, SystemTile = hex.SystemTile });
                }
                else
                {
                    newSlice.Positions.Add(new SlicePosition { X = slicePosition.X, Y = slicePosition.Y, SystemTile = null });
                }
            }

            slices.Add(newSlice);
        }

        return slices;
    }
}
