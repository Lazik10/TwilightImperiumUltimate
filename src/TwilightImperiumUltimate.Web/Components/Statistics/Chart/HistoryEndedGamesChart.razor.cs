using TwilightImperiumUltimate.Web.Components.Async.Statistics;

namespace TwilightImperiumUltimate.Web.Components.Statistics.Chart;

public partial class HistoryEndedGamesChart
{
    [Parameter] 
    public HistoryBarSeries Series { get; set; } = new("Year", [], []);
    
    [Parameter] 
    public bool IsLoading { get; set; }
}
