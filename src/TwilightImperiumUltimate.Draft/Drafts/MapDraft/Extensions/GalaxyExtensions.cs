using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Constants;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;

public static class GalaxyExtensions
{
    public static IReadOnlyCollection<string> GetWormholeTileCodes(this Dictionary<(int X, int Y), Hex> galaxy, WormholeName wormholeName)
    {
        return galaxy.Values
            .Where(hex => hex.SystemTile is not null
                && hex.SystemTile.Wormholes.Any(x => x.WormholeName == wormholeName))
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToList();
    }

    public static IReadOnlyCollection<string> GetLegendaryTileCodes(this Dictionary<(int X, int Y), Hex> galaxy)
    {
        return galaxy.Values
            .Where(hex => hex.SystemTile is not null && hex.SystemTile.HasLegendaryPlanet)
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToList();
    }

    public static IReadOnlyCollection<string> GetRedTileCodes(this Dictionary<(int X, int Y), Hex> galaxy)
    {
        return galaxy.Values
            .Where(hex => hex.SystemTile is not null && hex.SystemTile.TileCategory == SystemTileCategory.Red)
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToList();
    }

    public static int GetNumberOfEmptyPositions(this Dictionary<(int X, int Y), Hex> galaxy)
    {
        return galaxy.Values.Count(x => x.SystemTile is null && x.Name != PositionName.Empty);
    }
}
