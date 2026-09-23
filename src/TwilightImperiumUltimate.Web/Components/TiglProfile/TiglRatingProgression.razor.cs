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

        var min = values.Min() - 5;
        var upper = values.Max() + 5;
        var step = Math.Max(1, Math.Ceiling((upper - min) / 5));
        return (min, upper, step);
    }
    private IReadOnlyList<ChartData> BuildSeries() => new List<ChartData> { new("Async", Profile.AsyncMatchHistory.Select((item,index) => new Point(index + 1,item.RatingNew)).ToList()), new("Glicko-2", Profile.GlickoMatchHistory.Select((item,index) => new Point(index + 1,item.RatingNew)).ToList()), new("TrueSkill", Profile.TrueSkillMatchHistory.Select((item,index) => new Point(index + 1,item.MuNew)).ToList()) }.Where(item => item.Points.Count > 0).ToList();
    private sealed record ChartData(string Title,List<Point> Points); private sealed record Point(int Game,double Rating);
}
