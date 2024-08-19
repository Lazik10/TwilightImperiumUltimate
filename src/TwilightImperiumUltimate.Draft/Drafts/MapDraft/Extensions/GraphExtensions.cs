using QuickGraph;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.MapDraft.Extensions;

public static class GraphExtensions
{
    public static Random Rng { get; set; } = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        if (list is null)
            return;

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T GetOtherVertex<T>(this Edge<T> edge, T vertex)
    {
        if (edge is null || vertex is null)
            return default!;

        if (EqualityComparer<T>.Default.Equals(edge.Source, vertex))
        {
            return edge.Target;
        }
        return edge.Source;
    }

    public static string GetMapLayoutLog(this Dictionary<(int X, int Y), Hex> universe, int n, int m)
    {
        var logString = "\nGenerated Map Layout: \n";

        for (int x = 0; x < n; x++)
        {
            for (int y = 0; y < m; y++)
            {
                if (universe.ContainsKey((x, y)))
                {
                    if (universe[(x, y)].Name == " _ ")
                    {
                        logString += " _ ";
                    }
                    else
                    {
                        logString += "{" + $"{universe[(x, y)].Name}" + "}";
                    }
                }
                else
                {
                    logString += "   ";
                }
            }

            logString += "\n";
        }

        logString += "\n";

        return logString;
    }
}
