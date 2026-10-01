using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Components.Charts;

namespace TwilightImperiumUltimate.Web.Components.Statistics.Chart;

public partial class GeneralPlayerCountDistributionChart
{
    [Parameter]
    [EditorRequired]
    public IReadOnlyCollection<RankingBarPoint> Data { get; set; } = [];

    [Parameter]
    public bool IsLoading { get; set; }
}
