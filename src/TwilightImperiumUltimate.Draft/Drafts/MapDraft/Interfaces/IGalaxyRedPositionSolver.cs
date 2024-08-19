using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;

public interface IGalaxyRedPositionSolver
{
    Task<Dictionary<(int X, int Y), Hex>> SolveRedPositions(IMapSettings mapSettings, Dictionary<(int X, int Y), Hex> galaxy, GenerateMapRequest request);
}
