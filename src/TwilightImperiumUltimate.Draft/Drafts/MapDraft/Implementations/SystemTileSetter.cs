using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Constants;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

public class SystemTileSetter(
    ILogger<SystemTileSetter> logger)
    : ISystemTileSetter
{
    private readonly ILogger<SystemTileSetter> _logger = logger;

    private static Random Random { get; set; } = new Random();

    public void SetRedSystemTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        IMapSettings mapSettings,
        GenerateMapRequest request)
    {
        var redTiles = systemTilesForMapSetup.RedTiles.ToList();
        _logger.LogInformation("Available red tiles: {RedTiles}", redTiles.GetSystemTileCodes());

        var blueTiles = systemTilesForMapSetup.BlueTiles.ToList();
        _logger.LogInformation("Available blue tiles: {BlueTiles}", blueTiles.GetSystemTileCodes());

        var shuffledRedTiles = redTiles
            .OrderBy(x => Random.Next())
            .ToList();

        var redTilesWithAnomaly = shuffledRedTiles
            .Where(x => x.HasAnomaly).ToList();
        _logger.LogInformation("Available red tiles with anomaly: {RedTiles}", redTilesWithAnomaly.GetSystemTileCodes());

        var redTilesWithoutAnomaly = shuffledRedTiles
            .Where(x => !x.HasAnomaly).ToList();
        _logger.LogInformation("Available red tiles without anomaly: {RedTiles}", redTilesWithoutAnomaly.GetSystemTileCodes());

        // Do not place red tile with legendary planet near home system (there is currently only one in the game)
        // Distribute it randomly after everything else is set up
        var redTilesWithAnomalyAndPlanets = redTilesWithAnomaly
            .Where(x => x.Planets.Count > 0 && !x.HasLegendaryPlanet).ToList();
        _logger.LogInformation("Available red tiles with anomaly and planets: {RedTiles}", redTilesWithAnomalyAndPlanets.GetSystemTileCodes());

        var redTilesWithAnomalyAndWithoutPlanets = redTilesWithAnomaly
            .Where(x => x.Planets.Count == 0).ToList();
        _logger.LogInformation("Available red tiles with anomaly and without planets: {RedTiles}", redTilesWithAnomalyAndWithoutPlanets.GetSystemTileCodes());

        var redTilesWithAnomalyAndLegendaryPlanet = redTilesWithAnomaly
            .Where(x => x.HasLegendaryPlanet).ToList();
        _logger.LogInformation("Available red tiles with anomaly and legendary planet: {RedTiles}", redTilesWithAnomalyAndLegendaryPlanet.GetSystemTileCodes());

        // Check all adjacent home positions, if there are only three and two of them are red, make sure that one of them has a planet
        // or replace it with blue tile with planet randomly
        foreach (var adjacentHomePositions in mapSettings.AdjacentHomePositions.Select(x => x.Value))
        {
            var redTilesCount = adjacentHomePositions.Count(x => galaxy.TryGetValue(x, out var hex) && hex.Name == PositionName.Red);
            var adjacentHomeTiles = adjacentHomePositions.Count;

            // We only need to fix the state where there are three adjavent home poisitons and two of them should have two red tiles
            if (redTilesCount == 2 && adjacentHomeTiles == 3)
            {
                // Check if there is a red tile with planets to avoid having two anomaly tiles near home system with only three neighbors
                // There can't be a situation where there are only two neighbor system tiles and both are anomalies, because of how the red tile positions are distributed
                bool redTileWithPlantesPlaced = false;

                foreach (var adjacentHomePosition in adjacentHomePositions)
                {
                    if (galaxy.TryGetValue(adjacentHomePosition, out var hex))
                    {
                        // Place anomaly with planet first if we didn't place it yet
                        if (hex.Name == PositionName.Red && redTilesWithAnomalyAndPlanets.Count > 0 && !redTileWithPlantesPlaced)
                        {
                            galaxy[adjacentHomePosition].SystemTile = redTilesWithAnomalyAndPlanets[0];

                            redTilesWithAnomalyAndPlanets.RemoveAt(0);
                            redTileWithPlantesPlaced = true;
                        }

                        // If there are no red tiles with planets, place random blue tile with planet to avoid having terrible starting positions
                        else if (hex.Name == PositionName.Red
                            && redTilesWithAnomalyAndPlanets.Count == 0
                            && redTilesWithAnomalyAndWithoutPlanets.Count > 0
                            && !redTileWithPlantesPlaced)
                        {
                            galaxy[adjacentHomePosition].SystemTile = blueTiles[0];
                            blueTiles.RemoveAt(0);
                            redTileWithPlantesPlaced = true;
                        }

                        // In other caes place red tile with anomaly and without planet or randomly place empty red tile
                        else
                        {
                            // Randomly place empty red tile
                            var placeEmptyRedTile = Random.Next(0, 3) == 2;
                            if (hex.Name == PositionName.Red && redTilesWithoutAnomaly.Count > 0 && placeEmptyRedTile)
                            {
                                galaxy[adjacentHomePosition].SystemTile = redTilesWithoutAnomaly[0];
                                redTilesWithoutAnomaly.RemoveAt(0);
                            }

                            // Or place red tile without anomaly and without planet
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

        _logger.LogInformation("Remaining red tiles with anomaly: {RedTilesCount}", remainingRedTiles.GetSystemTileCodes());

        var galaxyRedPositions = galaxy.Values.Where(x => x.Name == PositionName.Red && x.SystemTile is null).ToList();

        // Fill all red positions with red tiles or at least as many as possible
        foreach (var position in galaxyRedPositions)
        {
            if (remainingRedTiles.Count == 0)
            {
                _logger.LogWarning("Not enough red tiles to fill all red positions. Filling as many as possible.");
                break;
            }

            position.SystemTile = remainingRedTiles[0];
            remainingRedTiles.RemoveAt(0);
        }

        // Replace one random red tile with legendary one if it is not present already to satisfy 7 legendary planets request
        if (request.NumberOfLegendaries == 7 && redTilesWithAnomalyAndLegendaryPlanet.Count > 0)
        {
            var position = galaxy.Keys.Where(x => galaxy[x].Name == PositionName.Red && galaxy[x].SystemTile is not null)
                .OrderBy(x => Random.Next())
                .First();

            galaxy[position].SystemTile = redTilesWithAnomalyAndLegendaryPlanet.FirstOrDefault();
        }
        else
        {
            // Sometimes replace red tile with legendary one if we need legendaries already (but only if we have Uncharted Space)
            if (request.NumberOfLegendaries > 1
                && request.GameVersions.Contains(GameVersion.UnchartedSpace)
                && Random.Next(0, 100) < request.NumberOfLegendaries / 7.0f * 100)
            {
                var position = galaxy.Keys.Where(x => galaxy[x].Name == PositionName.Red && galaxy[x].SystemTile is not null)
                    .OrderBy(x => Random.Next())
                    .First();

                galaxy[position].SystemTile = redTilesWithAnomalyAndLegendaryPlanet.FirstOrDefault();
            }
        }

        _logger.LogInformation("Assigned red tile codes in galaxy: {RedTilesCount}", galaxy.GetRedTileCodes());
    }

    public void SetHomeSystemTiles(
        Dictionary<(int X, int Y), Hex> galaxy,
        SystemTilesForMapSetup systemTilesForMapSetup,
        IMapSettings mapSettings,
        HomeSystemDraftType homeSystemDraftType,
        IReadOnlyCollection<FactionName> factions,
        IReadOnlyCollection<string> playerNames)
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

        if (homeSystemDraftType == HomeSystemDraftType.SpecificFactions)
        {
            var specificFactionHomeTiles = systemTilesForMapSetup.HomeTiles
                .Where(x => factions.Contains(x.FactionName))
                .OrderBy(x => Random.Next())
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

        var newPlayerNames = playerNames.OrderBy(x => Random.Next()).ToList();

        if (galaxy.Values.Count(x => x.Name == PositionName.Home) == playerNames.Count)
        {
            foreach (var hex in galaxy.Values.Where(x => x.Name == PositionName.Home))
            {
                hex.PlayerName = newPlayerNames[0];
                newPlayerNames.Remove(newPlayerNames[0]);
            }
        }
    }

    public void SetMecatolSystemTile(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, SystemTile mecatolRexSystemTile)
    {
        galaxy[mapSettings.MecatolRexPosition].SystemTile = mecatolRexSystemTile;
    }

    public void SetLegendarySystemTiles(Dictionary<(int X, int Y), Hex> galaxy, SystemTilesForGalaxyDistribution remainingSystemTiles, IMapSettings mapSettings)
    {
        if (!remainingSystemTiles.BlueTiles.Exists(x => x.HasLegendaryPlanet))
            return;

        _logger.LogInformation("Number of legendary system tiles: {LegendarySystemTilesCount}", remainingSystemTiles.BlueTiles.Count(x => x.HasLegendaryPlanet));

        var legendarySystemTile = remainingSystemTiles.BlueTiles.Where(x => x.HasLegendaryPlanet).ToList();

        var availableEquidistantPositions = mapSettings.EquidistantPositions
            .Where(x => galaxy[x].SystemTile is null)
            .ToList();

        var shuffledEquidistantPositions = availableEquidistantPositions
            .OrderBy(x => Random.Next());

        foreach (var equidistantPosition in shuffledEquidistantPositions)
        {
            if (legendarySystemTile.Count == 0)
                break;

            galaxy[equidistantPosition].SystemTile = legendarySystemTile[0];
            var legendaryTileToRemove = remainingSystemTiles.BlueTiles
                .Single(x => x.SystemTileCode == legendarySystemTile[0].SystemTileCode);
            remainingSystemTiles.BlueTiles.Remove(legendaryTileToRemove);
            legendarySystemTile.RemoveAt(0);
        }

        _logger.LogInformation("Number of remaining legendary system tiles: {LegendarySystemTilesCount}", remainingSystemTiles.BlueTiles.Count(x => x.HasLegendaryPlanet));
    }

    public void SetRemainingSystemTiles(Dictionary<(int X, int Y), Hex> galaxy, IMapSettings mapSettings, GenerateMapRequest request, IReadOnlyCollection<Slice> balancedSlices)
    {
        _ = request.PlacementStyle switch
        {
            PlacementStyle.Random => SetRemainingSystemTilesByRandomPlacement(galaxy, balancedSlices),
            PlacementStyle.Initial => SetRemainingSystemTilesByInitialPlacement(galaxy, mapSettings, balancedSlices),
            PlacementStyle.Home => SetRemainingSystemTilesByHomePlacement(galaxy, mapSettings, balancedSlices, request.SystemWeight),
            PlacementStyle.Slice => SetRemainingSystemTilesBySlicePlacement(galaxy, mapSettings, balancedSlices, request.SystemWeight),
            _ => SetRemainingSystemTilesByRandomPlacement(galaxy, balancedSlices),
        };
    }

    public void SetHyperlines(Dictionary<(int X, int Y), Hex> galaxy, IHyperlineSettings hyperlineSettings, SystemTilesForMapSetup systemTilesForMapSetup)
    {
        if (hyperlineSettings.Hyperlines.Count == 0)
            return;

        foreach (var hyperline in hyperlineSettings.Hyperlines)
        {
            if (galaxy.TryGetValue((hyperline.X, hyperline.Y), out var hex) && hex.SystemTile is null)
            {
                hex.SystemTile = systemTilesForMapSetup.Hyperlines.FirstOrDefault(x => x.SystemTileCode == hyperline.SystemTileCode);
                if (hex.SystemTile is not null)
                    hex.SystemTile.SystemTileCode = hex.SystemTile.SystemTileCode + hyperline.Orientation;
            }
        }
    }

    public void SetFrameTiles(Dictionary<(int X, int Y), Hex> galaxy, SystemTile frameSystemPlaceholder)
    {
        foreach (var hex in galaxy.Values)
        {
            if (galaxy.TryGetValue((hex.X, hex.Y), out Hex? galaxyHex) && galaxyHex is not null)
            {
                galaxyHex.SystemTile = frameSystemPlaceholder;
            }
        }
    }

    private Task SetRemainingSystemTilesByRandomPlacement(Dictionary<(int X, int Y), Hex> galaxy, IReadOnlyCollection<Slice> balancedSlices)
    {
        foreach (var slice in balancedSlices)
        {
            slice.DraftedSystemTiles.Shuffle();
            foreach (var position in slice.Positions.Where(position => galaxy[position.Position].SystemTile is null))
            {
                galaxy[position.Position].SystemTile = slice.DraftedSystemTiles[0];
                slice.DraftedSystemTiles.RemoveAt(0);
            }
        }

        return Task.CompletedTask;
    }

    private Task SetRemainingSystemTilesByInitialPlacement(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        IReadOnlyCollection<Slice> balancedSlices)
    {
        // Set which adjacent home system has higher priority left or right
        var leftPositionFirst = Random.Next(0, 2) == 0;

        foreach (var slice in balancedSlices)
        {
            var slicePositionsInPriorityOrder = new List<(int X, int Y)>
            {
                mapSettings.Slices[slice.Id][1],
                mapSettings.Slices[slice.Id][leftPositionFirst ? 0 : 2],
                mapSettings.Slices[slice.Id][leftPositionFirst ? 2 : 0],
                mapSettings.Slices[slice.Id][4],
                mapSettings.Slices[slice.Id][3],
            };

            // Set the best system tile in front of the home system or adjacent to home system
            var systemTile = slice.DraftedSystemTiles.AsEnumerable().OrderByOptimalValue().First();
            var firstEmptyPosition = slicePositionsInPriorityOrder.First(position => galaxy[position].SystemTile is null);
            galaxy[firstEmptyPosition].SystemTile = systemTile;
            slice.Positions.First(x => x.Position == firstEmptyPosition).SystemTile = systemTile;
            slice.DraftedSystemTiles.Remove(systemTile);

            // Shuffle the remaining drafted system tiles and continue with the draft
            var shuffledDraftedSystemTiles = slice.DraftedSystemTiles.OrderBy(x => Random.Next()).ToList();
            foreach (var position in slice.Positions.Where(pos => galaxy[pos.Position].SystemTile is null))
            {
                galaxy[position.Position].SystemTile = shuffledDraftedSystemTiles[0];
                shuffledDraftedSystemTiles.RemoveAt(0);
            }
        }

        return Task.CompletedTask;
    }

    private Task SetRemainingSystemTilesByHomePlacement(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        IReadOnlyCollection<Slice> balancedSlices,
        SystemWeight systemWeight)
    {
        // Set which adjacent home system has higher priority left or right
        var priorityPositions = new List<int> { 0, 1, 2 };
        priorityPositions.Shuffle();

        foreach (var slice in balancedSlices)
        {
            var slicePositionsInPriorityOrder = new List<(int X, int Y)>
            {
                mapSettings.Slices[slice.Id][priorityPositions[0]],
                mapSettings.Slices[slice.Id][priorityPositions[1]],
                mapSettings.Slices[slice.Id][priorityPositions[2]],
                mapSettings.Slices[slice.Id][4],
                mapSettings.Slices[slice.Id][3],
            };

            // Shuffle the remaining drafted system tiles and continue with the draft
            var orderedDraftedSystemTileList = slice.DraftedSystemTiles.AsEnumerable().OrderByOptimalValue().ToList();

            _logger.LogInformation("Ordered system tiles by Optimal value: {SystemTiles}", string.Join(",", orderedDraftedSystemTileList.Select(x => $"{x.SystemTileCode} {x.GetValue(systemWeight)}")));

            foreach (var position in slicePositionsInPriorityOrder.Where(x => galaxy[(x.X, x.Y)].SystemTile is null))
            {
                galaxy[position].SystemTile = orderedDraftedSystemTileList[0];
                orderedDraftedSystemTileList.RemoveAt(0);
            }
        }

        return Task.CompletedTask;
    }

    private Task SetRemainingSystemTilesBySlicePlacement(
        Dictionary<(int X, int Y), Hex> galaxy,
        IMapSettings mapSettings,
        IReadOnlyCollection<Slice> balancedSlices,
        SystemWeight systemWeight)
    {
        // Set which adjacent home system has higher priority left or right
        var priorityPositions = new List<int> { 0, 2, };
        priorityPositions.Shuffle();

        foreach (var slice in balancedSlices)
        {
            var sliceEmptyPositions = slice.Positions.Where(x => galaxy[x.Position].SystemTile is null).ToList();

            _logger.LogInformation("Slice empty positions: {EmptyPositions}", string.Join(",", sliceEmptyPositions.Select(x => $"[{x.X},{x.Y}]")));

            _logger.LogInformation("Drafted system tiles: {Count} {DraftedSystemTiles}", slice.DraftedSystemTiles.Count,  string.Join(" ", slice.DraftedSystemTiles.Select(x => x.SystemTileCode)));

            var slicePositionsInPriorityOrder = new List<(int X, int Y)>();

            if (slice.Positions.Count == 5)
            {
                slicePositionsInPriorityOrder = new List<(int X, int Y)>
                {
                    mapSettings.Slices[slice.Id][1],
                    mapSettings.Slices[slice.Id][4],
                    mapSettings.Slices[slice.Id][priorityPositions[0]],
                    mapSettings.Slices[slice.Id][priorityPositions[1]],
                    mapSettings.Slices[slice.Id][3],
                };
            }
            else if (slice.Positions.Count == 4)
            {
                slicePositionsInPriorityOrder = new List<(int X, int Y)>
                {
                    mapSettings.Slices[slice.Id][1],
                    mapSettings.Slices[slice.Id][priorityPositions[0]],
                    mapSettings.Slices[slice.Id][priorityPositions[1]],
                    mapSettings.Slices[slice.Id][3],
                };
            }

            // Shuffle the remaining drafted system tiles and continue with the draft
            var orderedDraftedSystemTileList = slice.DraftedSystemTiles.AsEnumerable().OrderByOptimalValue().ToList();

            _logger.LogInformation("Ordered system tiles by Optimal value: {SystemTiles}", string.Join(",", orderedDraftedSystemTileList.Select(x => $"{x.SystemTileCode} {x.GetValue(systemWeight)}")));

            // If legendary was drafted for this slice, place it in the most conflict position
            if (slice.DraftedSystemTiles.Any(x => x.HasLegendaryPlanet))
            {
                for (var i = 0; i < slice.DraftedSystemTiles.Count(x => x.HasLegendaryPlanet); i++)
                {
                    var legendarySystemTile = slice.DraftedSystemTiles.FirstOrDefault(x => x.HasLegendaryPlanet);

                    var theMostConflictPosition = slice.Positions.Count - 1;
                    if (legendarySystemTile is not null && slice.Positions[theMostConflictPosition].SystemTile is null)
                    {
                        if (galaxy.TryGetValue((slice.Positions[theMostConflictPosition].X, slice.Positions[theMostConflictPosition].Y), out Hex? hex) && hex.SystemTile is null)
                        {
                            hex.SystemTile = legendarySystemTile;
                            var systemTileToRemove = orderedDraftedSystemTileList.Single(x => x.SystemTileCode == legendarySystemTile.SystemTileCode);
                            orderedDraftedSystemTileList.Remove(systemTileToRemove);
                        }
                    }
                    else if (legendarySystemTile is not null && slice.Positions[3].SystemTile is null
                        && galaxy.TryGetValue((slice.Positions[theMostConflictPosition - 1].X, slice.Positions[theMostConflictPosition - 1].Y), out Hex? hex)
                        && hex.SystemTile is null)
                    {
                        hex.SystemTile = legendarySystemTile;
                        var systemTileToRemove = orderedDraftedSystemTileList.Single(x => x.SystemTileCode == legendarySystemTile.SystemTileCode);
                        orderedDraftedSystemTileList.Remove(systemTileToRemove);
                    }
                }
            }

            foreach (var position in slicePositionsInPriorityOrder.Where(x => galaxy[x].SystemTile is null))
            {
                if (orderedDraftedSystemTileList.Count > 0)
                {
                    _logger.LogInformation("Handling position {Position}", position);
                    galaxy[position].SystemTile = orderedDraftedSystemTileList[0];
                    _logger.LogInformation("Tile assigned and removed: {TileCode}", orderedDraftedSystemTileList[0].SystemTileCode);
                    orderedDraftedSystemTileList.RemoveAt(0);
                }
            }
        }

        return Task.CompletedTask;
    }
}
