using Azure.Core;
using TwilightImperiumUltimate.Core.Entities.Galaxy;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;

public static class SystemTileExtensions
{
    public static IOrderedEnumerable<SystemTile> OrderByOptimalValue(this IEnumerable<SystemTile> systemTiles)
    {
        return systemTiles.OrderByDescending(tile => tile.Planets.Sum(planet =>
        {
            var value = 0.0f;
            value += planet.Resources switch
            {
                var resources when resources > planet.Influence => resources,
                var resources when resources == planet.Influence => resources / 2.0f,
                var resources when resources < planet.Influence => planet.Influence,
                _ => 0.0f,
            };
            value += planet.IsLegendary ? 2.0f : 0.0f;
            value += planet.TechnologySkip != TechnologyType.None ? 0.3f : 0.0f;
            value += tile.Planets.Count * 0.2f;
            return value;
        }));
    }

    public static IOrderedEnumerable<SystemTile> OrderByResourceValue(this IEnumerable<SystemTile> systemTiles)
    {
        return systemTiles.OrderByDescending(tile => tile.Planets.Sum(planet =>
            planet.Resources switch
            {
                var resources when resources > planet.Influence => resources,
                var resources when resources == planet.Influence => resources / 2.0f,
                _ => 0.0f,
            }));
    }

    public static IOrderedEnumerable<SystemTile> OrderByInfluenceValue(this IEnumerable<SystemTile> systemTiles)
    {
        return systemTiles.OrderByDescending(tile => tile.Planets.Sum(planet =>
            planet.Influence switch
            {
                var influence when influence > planet.Resources => influence,
                var influence when influence == planet.Resources => influence / 2.0f,
                _ => 0.0f,
            }));
    }

    public static float GetValue(this SystemTile systemTile, SystemWeight systemWeight)
    {
        return systemWeight switch
        {
            SystemWeight.Balanced => systemTile.Planets.Sum(planet =>
            planet.Resources switch
            {
                var resources when resources > planet.Influence => resources,
                var resources when resources == planet.Influence => resources / 2.0f,
                var resources when resources < planet.Influence => planet.Influence,
                _ => 0.0f,
            }),
            SystemWeight.Resources => systemTile.Planets.Sum(planet =>
            planet.Resources switch
            {
                var resources when resources > planet.Influence => resources,
                var resources when resources == planet.Influence => resources / 2.0f,
                _ => 0.0f,
            }),
            SystemWeight.Influence => systemTile.Planets.Sum(planet =>
            planet.Influence switch
            {
                var influence when influence > planet.Resources => influence,
                var influence when influence == planet.Resources => influence / 2.0f,
                _ => 0.0f,
            }),
            _ => 0.0f,
        };
    }

    public static float GetOptimalResourceValue(this SystemTile systemTile)
    {
        return systemTile.Planets.Sum(planet =>
            planet.Resources switch
            {
                var resources when resources > planet.Influence => resources,
                var resources when resources == planet.Influence => resources / 2.0f,
                _ => 0.0f,
            });
    }

    public static float GetOptimalInfluenceValue(this SystemTile systemTile)
    {
        return systemTile.Planets.Sum(planet =>
            planet.Influence switch
            {
                var influence when influence > planet.Resources => influence,
                var influence when influence == planet.Resources => influence / 2.0f,
                _ => 0.0f,
            });
    }

    public static string GetSystemTileCodes(this IEnumerable<SystemTile> systemTiles)
    {
        return string.Join(",", systemTiles.Select(x => x.SystemTileCode).ToList());
    }

    public static string GetSystemTileCodesWithSystemWeight(this IEnumerable<SystemTile> systemTiles, SystemWeight systemWeight)
    {
        return string.Join(",", systemTiles.Select(x => string.Join("- ", $"Id {x.SystemTileCode}", $"Weight: {x.GetValue(systemWeight)}")));
    }
}