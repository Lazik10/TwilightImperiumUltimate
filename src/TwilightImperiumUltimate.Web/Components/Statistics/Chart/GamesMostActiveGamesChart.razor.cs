using TwilightImperiumUltimate.Web.Components.Charts;

namespace TwilightImperiumUltimate.Web.Components.Statistics.Chart;

public partial class GamesMostActiveGamesChart
{
    [Parameter]
    public IReadOnlyCollection<RankingBarPoint> Data { get; set; } = [];

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string SeriesTitle { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<object?> TagClick { get; set; }

    [Parameter]
    public bool IsLoading { get; set; }
}
