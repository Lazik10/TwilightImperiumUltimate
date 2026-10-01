using TwilightImperiumUltimate.Web.Components.Charts;

namespace TwilightImperiumUltimate.Web.Components.Statistics.Chart;

public partial class TurnsFastestPlayersChart
{
    [Parameter]
    public IReadOnlyCollection<RankingBarPoint> Data { get; set; } = [];

    [Parameter]
    public EventCallback<object?> TagClick { get; set; }

    [Parameter]
    public bool IsLoading { get; set; }

    [Parameter]
    public Func<double, string>? FormatValue { get; set; }

    private string FormatAxisValue(object value) => FormatValue?.Invoke(Convert.ToDouble(value)) ?? value.ToString() ?? string.Empty;
}
