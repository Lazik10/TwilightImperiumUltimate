using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Constants;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

public class SystemTileSetter(
    ILogger<SystemTileSetter> logger)
    : ISystemTileSetter
{
    private static Random Random { get; set; } = new Random();
    private readonly ILogger<SystemTileSetter> _logger = logger;

    public void SetRedSystemTiles(Dictionary<(int X, int Y), Hex> galaxy, IReadOnlyCollection<SystemTile> redTiles, IMapSettings mapSettings, GenerateMapRequest request)
    {
        var shuffledRedTiles = redTiles
            .OrderBy(x => Random.Next())
            .ToList();

        var redTilesWithAnomaly = shuffledRedTiles
            .Where(x => x.HasAnomaly).ToList();

        var redTilesWithoutAnomaly = shuffledRedTiles
            .Where(x => !x.HasAnomaly).ToList();

        if (request.WormholesDensity == WormholeDensity.None)
        {
            redTilesWithAnomaly = redTilesWithAnomaly
                .Where(x => x.Wormholes.Count != 0).ToList();
        }

        var redTilesWithAnomalyAndPlanets = redTilesWithAnomaly
            .Where(x => x.Planets.Count > 0).ToList();

        var redTilesWithAnomalyAndWithoutPlanets = redTilesWithAnomaly
            .Where(x => x.Planets.Count == 0).ToList();

        foreach (var adjacentHomePositions in mapSettings.AdjacentHomePositions.Select(x => x.Value))
        {
            var redTilesCount = adjacentHomePositions.Count(x => galaxy.TryGetValue(x, out var hex) && hex.Name == PositionName.Red);

            if (redTilesCount >= 2)
            {
                bool redTileWithPlantesPlaced = false;

                foreach (var adjacentHomePosition in adjacentHomePositions)
                {
                    if (galaxy.TryGetValue(adjacentHomePosition, out var hex))
                    {
                        if (hex.Name == PositionName.Red && redTilesWithAnomalyAndPlanets.Count > 0 && !redTileWithPlantesPlaced)
                        {
                            galaxy[adjacentHomePosition].SystemTile = redTilesWithAnomalyAndPlanets[0];
                            redTilesWithAnomalyAndPlanets.RemoveAt(0);
                            redTileWithPlantesPlaced = true;
                        }
                        else if (hex.Name == PositionName.Red
                            && redTilesWithAnomalyAndPlanets.Count == 0
                            && redTilesWithAnomalyAndWithoutPlanets.Count > 0
                            && !redTileWithPlantesPlaced)
                        {
                            galaxy[adjacentHomePosition].SystemTile = redTilesWithoutAnomaly[0];
                            redTilesWithoutAnomaly.RemoveAt(0);
                            redTileWithPlantesPlaced = true;
                        }
                        else
                        {
                            var placeEmptyRedTile = Random.Next(0, 3) == 2;
                            if (hex.Name == PositionName.Red && redTilesWithoutAnomaly.Count > 0 && placeEmptyRedTile)
                            {
                                galaxy[adjacentHomePosition].SystemTile = redTilesWithoutAnomaly[0];
                                redTilesWithoutAnomaly.RemoveAt(0);
                            }
                            else if (hex.Name == PositionName.Red && redTilesWithAnomalyAndWithoutPlanets.Count > 0)
                            {
                                galaxy[adjacentHomePosition].SystemTile = redTilesWithAnomalyAndWithoutPlanets[0];
                                redTilesWithAnomalyAndWithoutPlanets.RemoveAt(0);
                            }
                        }
                    }
                }
            }
        }

        var remainingRedTiles = redTilesWithAnomalyAndPlanets
                .Concat(redTilesWithAnomalyAndWithoutPlanets)
                .ToList();

        galaxy.Keys
            .Where(x => galaxy[x].Name == PositionName.Red && galaxy[x].SystemTile is null)
            .Select((position, index) => new { position, RedTile = remainingRedTiles[index] })
            .ToList()
            .ForEach(x =>
            {
                galaxy[x.position].SystemTile = x.RedTile;
            });
    }

    public void SetHomeSystemTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        IMapSettings mapSettings,
        HomeSystemDraftType homeSystemDraftType,
        IReadOnlyCollection<FactionName> factions)
    {
        if (homeSystemDraftType == HomeSystemDraftType.Placeholders)
        {
            var homeSystemPlaceholder = systemTilesForMapSetup.EmptyHomeSystemPlaceholder;
            galaxy.Keys.Where(x => galaxy[x].Name == PositionName.Home)
                .Select((position, index) => new { position, HomeSystemTile = homeSystemPlaceholder })
                .ToList()
                .ForEach(x =>
                {
                    galaxy[x.position].SystemTile = x.HomeSystemTile;
                });
        }

        if (homeSystemDraftType == HomeSystemDraftType.RandomFactions)
        {
            var homeTiles = systemTilesForMapSetup.HomeTiles.ToList();
            galaxy.Keys.Where(x => galaxy[x].Name == PositionName.Home)
                .Select((position, index) => new { position, HomeSystemTile = homeTiles[index] })
                .ToList()
                .ForEach(x =>
                {
                    galaxy[x.position].SystemTile = x.HomeSystemTile;
                    homeTiles.Remove(x.HomeSystemTile);
                });
        }

        if (homeSystemDraftType == HomeSystemDraftType.SpecificFactions)
        {
            var specificFactionHomeTiles = systemTilesForMapSetup.HomeTiles
                .Where(x => factions.Contains(x.FactionName))
                .Take(mapSettings.NumberOfPlayers)
                .ToList();

            galaxy.Keys.Where(x => galaxy[x].Name == PositionName.Home)
                .Select((position, index) => new { position, HomeSystemTile = specificFactionHomeTiles[index] })
                .ToList()
                .ForEach(x =>
                {
                    galaxy[x.position].SystemTile = x.HomeSystemTile;
                    specificFactionHomeTiles.Remove(x.HomeSystemTile);
                });
        }
    }

    public void SetMecatolSystemTile(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, SystemTile mecatolRexSystemTile)
    {
        galaxy[mapSettings.MecatolRexPosition].SystemTile = mecatolRexSystemTile;
    }

    public void SetRemainingSystemTilesRandomly(Dictionary<(int X, int Y), Hex> galaxy, SystemTilesForGalaxyDistribution systemTilesForGalaxyDistribution)
    {
        var remainingSystemTiles = systemTilesForGalaxyDistribution.BlueTiles
            .Concat(systemTilesForGalaxyDistribution.RedTiles)
            .OrderBy(x => Random.Next())
            .ToList();

        var eligibleHexes = galaxy.Values
            .Where(x => x.Name != PositionName.Empty && x.Name != PositionName.Red && x.Name != PositionName.Home && x.Name != PositionName.MecatolRex)
            .ToList();

        if (remainingSystemTiles.Count < eligibleHexes.Count)
        {
            _logger.LogWarning("Not enough system tiles to fill all hexes. Filling as many as possible.");
        }

        using var tileEnumerator = remainingSystemTiles.GetEnumerator();

        foreach (var hex in eligibleHexes)
        {
            if (!tileEnumerator.MoveNext())
            {
                _logger.LogDebug("No more system tiles to place.");
            }

            hex.SystemTile = tileEnumerator.Current;
        }

        var sysetemTileCodes = galaxy.Values
            .Where(x => x.Name != PositionName.Empty && x.SystemTile is not null)
            .Select(x => x.SystemTile!.SystemTileCode)
            .ToList();

        var hashSet = sysetemTileCodes.ToHashSet();

        if (hashSet.Count != sysetemTileCodes.Count)
        {
            _logger.LogError("Duplicate system tile codes detected!");
        }
    }
}
