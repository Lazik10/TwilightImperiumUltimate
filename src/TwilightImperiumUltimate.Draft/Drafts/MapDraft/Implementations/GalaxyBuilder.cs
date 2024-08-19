using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Constants;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

public class GalaxyBuilder : IGalaxyBuilder
{
    public Task<Dictionary<(int X, int Y), Hex>> GenerateGalaxy(IMapSettings mapSettings)
    {
        var galaxy = new Dictionary<(int, int), Hex>();

        for (int x = 0; x < mapSettings.DimensionX; x += 2)
        {
            for (int y = 0; y < mapSettings.DimensionY; y += 2)
            {
                galaxy[(x, y)] = new Hex(x, y);
            }
        }

        for (int x = 1; x < mapSettings.DimensionX; x += 2)
        {
            for (int y = 1; y < mapSettings.DimensionY; y += 2)
            {
                galaxy[(x, y)] = new Hex(x, y);
            }
        }

        // Add standard neighbors based on hex grid logic
        foreach (var kv in galaxy)
        {
            var (x, y) = kv.Key;
            var hex = kv.Value;

            var directions = new (int, int)[]
            {
                (0, 2), (0, -2), (-1, -1), (-1, 1), (1, 1), (1, -1), (2, 0), (-2, 0),
            };

            foreach (var (dx, dy) in directions)
            {
                var neighborCoordinates = (x + dx, y + dy);
                if (galaxy.TryGetValue(neighborCoordinates, out Hex? neighborHex))
                {
                    hex.Neighbors.Add(neighborHex);
                }
            }
        }

        foreach (var pos in mapSettings.HomePositions.Where(pos => galaxy.ContainsKey(pos)))
        {
            galaxy[pos].Name = PositionName.Home;
        }

        foreach (var pos in mapSettings.EmptyPositions.Where(pos => galaxy.ContainsKey(pos)))
        {
            galaxy[pos].Name = PositionName.Empty;
        }

        if (galaxy.TryGetValue(mapSettings.MecatolRexPosition, out Hex? mecatolHex))
        {
            mecatolHex.Name = PositionName.MecatolRex;
        }

        return Task.FromResult(galaxy);
    }

    public void AddCustomNeighbor(Dictionary<(int X, int Y), Hex> galaxy, IReadOnlyCollection<(int X1, int Y1, int X2, int Y2)> customNeigbors)
    {
        if (customNeigbors is null || customNeigbors.Count == 0 || galaxy is null)
            return;

        foreach (var (x1, y1, x2, y2) in customNeigbors)
        {
            if (galaxy.TryGetValue((x1, y1), out Hex? hex1) && galaxy.TryGetValue((x2, y2), out Hex? hex2))
            {
                hex1.AddCustomNeighbor(hex2);
            }
        }
    }
}
