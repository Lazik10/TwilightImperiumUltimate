using TwilightImperiumUltimate.Web.Components.Charts;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public sealed record HistoryBarSeries(string Title, IReadOnlyCollection<RankingBarPoint> Points, IReadOnlyCollection<string> Fills);
