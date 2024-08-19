using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface IGalaxyBuilder
{
    Task<Dictionary<(int X, int Y), Hex>> GenerateGalaxy(IMapSettings mapSettings);

    void AddCustomNeighbor(Dictionary<(int X, int Y), Hex> galaxy, IReadOnlyCollection<(int X1, int Y1, int X2, int Y2)> customNeigbors);
}