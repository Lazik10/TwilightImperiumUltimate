using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglRatingProgression
{
    [Parameter, EditorRequired] public TiglLeagueProfileDto Profile { get; set; } = default!;

    private IReadOnlyList<ChartData> Series => BuildSeries();

    private static (double Min, double Max, double Step) GetBounds(ChartData chart)
    {
        var values = chart.Points.Select(point => point.Rating).ToList();
        if (values.Count == 0)
            return (0, 10, 5);

        var min = values.Min() - 0.1;
        var upper = values.Max() + 0.1;
        var step = Math.Max(0.1, (upper - min) / 5);
        return (min, upper, step);
    }

    private IReadOnlyList<ChartData> BuildSeries() => new List<ChartData>
    {
        BuildChartData("Async", Profile.AsyncMatchHistory.Select(item => item.RatingNew).ToList()),
        BuildChartData("Glicko-2", Profile.GlickoMatchHistory.Select(item => item.RatingNew).ToList()),
        BuildChartData("TrueSkill", Profile.TrueSkillMatchHistory.Select(item => item.MuNew).ToList()),
    }
    .Where(item => item.Points.Count > 0)
    .ToList();

    private static ChartData BuildChartData(string title, List<double> ratings)
    {
        if (ratings.Count == 0)
            return new ChartData(title, [], 1, static value => value?.ToString() ?? string.Empty);

        if (ratings.Count == 1)
        {
            var singleRating = ratings[0];
            return new ChartData(
                title,
                [new Point(1, singleRating), new Point(2, singleRating)],
                2,
                static value => (value?.ToString() == "1" || value?.ToString() == "2") ? "1" : value?.ToString() ?? string.Empty);
        }

        var points = ratings.Select((rating, index) => new Point(index + 1, rating)).ToList();
        return new ChartData(title, points, points.Count, static value => value?.ToString() ?? string.Empty);
    }

    private sealed record ChartData(string Title, List<Point> Points, int MaxGame, Func<object, string> CategoryFormatter);

    private sealed record Point(int Game, double Rating);
}
