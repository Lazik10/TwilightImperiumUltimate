using TwilightImperiumUltimate.Web.Components.Charts;

namespace TwilightImperiumUltimate.Web.Components.Statistics.Chart;

public partial class CombatWorstHitsDeviationChart
{
    [Parameter] 
    public IReadOnlyCollection<RankingBarPoint> Data { get; set; } = [];

    [Parameter] 
    public EventCallback<object?> TagClick { get; set; }
    
    [Parameter] 
    public bool IsLoading { get; set; }
    
    [Parameter] 
    public Func<double, string>? FormatValue { get; set; }

    private string FormatNegativeValue(double value) => $"-{FormatValue?.Invoke(Math.Abs(value)) ?? Math.Abs(value).ToString()}";

    private string FormatNegativeAxisValue(object value)
    {
        var numericValue = Convert.ToDouble(value);

        return numericValue == 0
        ? "0"
        : $"-{FormatValue?.Invoke(Math.Abs(numericValue)) ?? Math.Abs(numericValue).ToString()}";
    }
}
