using QuickGraph;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Constants;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.Interfaces;
using TwilightImperiumUltimate.Draft.Drafts.MapDraft.MapSettings;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Implementations;

public class GalaxyRedPositionSolver(
    ILogger<GalaxyRedPositionSolver> logger)
    : IGalaxyRedPositionSolver
{
    private static readonly Random _random = new();
    private readonly ILogger<GalaxyRedPositionSolver> _logger = logger;

    public Task<Dictionary<(int X, int Y), Hex>> SolveRedPositions(IMapSettings mapSettings, Dictionary<(int X, int Y), Hex> galaxy, GenerateMapRequest request)
    {
        var graph = GenerateGraphFromUniverse(galaxy);

        var galaxyWithRedPositions = CreateUniverseWithNRedHexes(galaxy, graph, mapSettings, request);

        return Task.FromResult(galaxyWithRedPositions);
    }

    private static UndirectedGraph<Hex, Edge<Hex>> GenerateGraphFromUniverse(Dictionary<(int X, int Y), Hex> universe)
    {
        ArgumentNullException.ThrowIfNull(universe);

        var graph = new UndirectedGraph<Hex, Edge<Hex>>();

        foreach (var hex in universe.Values)
        {
            graph.AddVertex(hex);
        }

        foreach (var hex in universe.Values)
        {
            foreach (var neighbor in hex.Neighbors)
            {
                graph.AddEdge(new Edge<Hex>(hex, neighbor));
            }
        }

        return graph;
    }

    private static List<Hex> FindMaxIndependentSet(UndirectedGraph<Hex, Edge<Hex>> graph)
    {
        var independentSet = new List<Hex>();
        var vertices = new List<Hex>(graph.Vertices);
        vertices.Shuffle();

        var visited = new HashSet<Hex>();

        foreach (var vertex in vertices)
        {
            if (!visited.Contains(vertex))
            {
                independentSet.Add(vertex);
                visited.Add(vertex);

                foreach (var edge in graph.AdjacentEdges(vertex))
                {
                    var neighbor = edge.GetOtherVertex(vertex);
                    visited.Add(neighbor);
                }
            }
        }

        return independentSet;
    }

    private Dictionary<(int X, int Y), Hex> CreateUniverseWithNRedHexes(Dictionary<(int X, int Y), Hex> galaxy, UndirectedGraph<Hex, Edge<Hex>> graph, IMapSettings mapSettings, GenerateMapRequest request)
    {
        _logger.LogInformation("Creating universe with Anomaly density set to: {AnomalyDensity}", request.AnomalyDensity);

        // Remove home positions, empty positions and Mecatol Rex from the graph
        foreach (var pos in mapSettings.HomePositions.Where(pos => galaxy.ContainsKey(pos)))
        {
            graph.RemoveVertex(galaxy[pos]);
        }

        foreach (var pos in mapSettings.EmptyPositions.Where(pos => galaxy.ContainsKey(pos)))
        {
            graph.RemoveVertex(galaxy[pos]);
        }

        if (galaxy.TryGetValue(mapSettings.MecatolRexPosition, out Hex? value))
        {
            graph.RemoveVertex(value);
        }

        // Find max independent set
        var maxIndependentSet = FindMaxIndependentSet(graph);
        _logger.LogInformation("Detected max {MaxIndependentSet} red positions and {MinRedPositions} required.", maxIndependentSet, mapSettings.NumberOfPlayers);

        // Get the number of anomaly red positions
        var n = Math.Max(mapSettings.NumberOfPlayers, maxIndependentSet.Count);

        var selectedNumberOfRedPositions = _random.Next(mapSettings.NumberOfPlayers, n + 1);

        _logger.LogInformation("Generatig {N} red positions...", selectedNumberOfRedPositions);

        // Filter out hexes that are homes or empty positions
        var filteredIndependentSet = maxIndependentSet
            .Where(hex => hex.Name != PositionName.Home
                && hex.Name != PositionName.Empty
                && hex.Name != PositionName.MecatolRex)
            .ToList();

        // Shuffle the filtered independent set and take the first N elements
        filteredIndependentSet.Shuffle();
        var redHexes = filteredIndependentSet.Take(selectedNumberOfRedPositions).ToList();

        // Mark the red hexes in the universe
        foreach (var hex in redHexes)
        {
            galaxy[(hex.X, hex.Y)].Name = PositionName.Red;
        }

        return galaxy;
    }
}
