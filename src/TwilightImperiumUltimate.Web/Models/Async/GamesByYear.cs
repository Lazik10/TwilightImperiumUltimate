namespace TwilightImperiumUltimate.Web.Models.Async;

public class GamesByYear
{
    public DateOnly Year { get; set; }

    public IEnumerable<GamesByMonth> Months { get; set; } = new List<GamesByMonth>();
}
