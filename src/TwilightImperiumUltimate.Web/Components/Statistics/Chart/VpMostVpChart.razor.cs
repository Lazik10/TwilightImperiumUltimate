using TwilightImperiumUltimate.Web.Components.Charts;

namespace TwilightImperiumUltimate.Web.Components.Statistics.Chart;

public partial class VpMostVpChart
{
    [Parameter]
    public IReadOnlyCollection<RankingBarPoint> Data { get; set; } = [];

    [Parameter]
    public EventCallback<object?> TagClick { get; set; }

    [Parameter]
    public bool IsLoading { get; set; }
}
