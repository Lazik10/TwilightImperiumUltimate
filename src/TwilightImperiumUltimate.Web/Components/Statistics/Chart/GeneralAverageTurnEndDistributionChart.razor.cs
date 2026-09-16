using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Components.Charts;

namespace TwilightImperiumUltimate.Web.Components.Statistics.Chart;

public partial class GeneralAverageTurnEndDistributionChart
{
    [Parameter]
    [EditorRequired]
    public IReadOnlyCollection<RankingBarPoint> Data { get; set; } = [];

    [Parameter]
    public bool IsLoading { get; set; }

    private static string FormatValue(double value) => value.ToString("N3");
}
