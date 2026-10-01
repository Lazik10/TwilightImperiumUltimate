using Radzen;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
using TwilightImperiumUltimate.Web.Models.Async;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Statistics.DataGrid;

public partial class FactionStatisticsTable
{
    [Parameter] public IReadOnlyCollection<AsyncFactionsStatsDto> FactionsForDisplay { get; set; } = [];
    [Parameter] public FactionStatisticsVpFilter SelectedVpFilter { get; set; }
    [Parameter] public FactionStatisticsSubstatsFilter SelectedSubstatsFilter { get; set; }
    [Parameter] public bool IsLoading { get; set; }

    [Inject] private IAsyncFactionMinMaxStatsProvider AsyncFactionMinMaxStatsProvider { get; set; } = default!;

    private int _row;

    private AsyncFactionStatsByGameVpDto GetCorrectFactionStatsByVp(AsyncFactionsStatsDto factionStats) => SelectedVpFilter switch
    {
        FactionStatisticsVpFilter.TenVp => factionStats.TenVp,
        FactionStatisticsVpFilter.TwelveVp => factionStats.TwelveVp,
        FactionStatisticsVpFilter.FourteenVp => factionStats.FourteenVp,
        _ => factionStats.All,
    };

    private AsyncFactionsMaxStatValues GetAllCorrectMaxFactionStatsByVp()
    {
        var stats = FactionsForDisplay.Select(GetCorrectFactionStatsByVp).ToList();
        return new AsyncFactionsMaxStatValues
        {
            Games = stats.Max(x => x.Games),
            Wins = stats.Max(x => x.Wins),
            WinPercentage = stats.Max(x => x.Games == 0 ? 0 : ((float)x.Wins / x.Games) * 100),
            Eliminations = stats.Max(x => x.Eliminations),
            AverageVp = stats.Max(x => x.Games == 0 ? 0 : (float)x.Vp / x.Games),
            AverageVpPrecentage = stats.Max(x => x.VpPercentage),
        };
    }

    private AsyncPlayerFactionMinMaxValues GetMinMaxStatsForFaction(AsyncFactionStatsByGameVpDto factionStats)
    {
        var values = AsyncFactionMinMaxStatsProvider.GetCorrectStatValues(GetAllCorrectMaxFactionStatsByVp(), factionStats, SelectedSubstatsFilter).GetAwaiter().GetResult();
        return new AsyncPlayerFactionMinMaxValues(values.Min, values.Value, values.Max);
    }

    private string GetCorrectLabelText() => SelectedSubstatsFilter switch
    {
        FactionStatisticsSubstatsFilter.Games => Strings.FactionStats_Games,
        FactionStatisticsSubstatsFilter.Wins => Strings.FactionStats_Wins,
        FactionStatisticsSubstatsFilter.WinPercentage => Strings.FactionStats_WinPercentage,
        FactionStatisticsSubstatsFilter.Eliminations => Strings.FactionStats_Eliminations,
        FactionStatisticsSubstatsFilter.MaxVp => Strings.FactionStats_MaxVp,
        FactionStatisticsSubstatsFilter.AverageVp => Strings.FactionStats_AverageVp,
        FactionStatisticsSubstatsFilter.MinVp => Strings.FactionStats_MinVp,
        FactionStatisticsSubstatsFilter.AverageVpPercentage => Strings.FactionStats_AverageVpPercentage,
        _ => Strings.FactionStats_All,
    };

    private static TextColor GetCorrectLabelColor() => TextColor.White;

    private string ToFormatedString(float value) => SelectedSubstatsFilter switch
    {
        FactionStatisticsSubstatsFilter.AverageVpPercentage or FactionStatisticsSubstatsFilter.WinPercentage => value.ToStringWithPrecisionAndPercentage(2),
        FactionStatisticsSubstatsFilter.AverageVp => value.ToStringWithPrecision(2),
        _ => value.ToStringWithPrecision(0),
    };

    private TextColor GetSubstatsCorrectColor(float value) => SelectedSubstatsFilter switch
    {
        FactionStatisticsSubstatsFilter.Games => TextColor.Yellow,
        FactionStatisticsSubstatsFilter.Wins => GetWinColor((int)value),
        FactionStatisticsSubstatsFilter.WinPercentage => value.GetWinrateColor(),
        FactionStatisticsSubstatsFilter.Eliminations => TextColor.Red,
        FactionStatisticsSubstatsFilter.AverageVpPercentage => value.GetAverageVpPercentageColor(),
        FactionStatisticsSubstatsFilter.AverageVp => value.GetAverageVpColor(),
        _ => TextColor.Yellow,
    };

    private TextColor GetCorrectProgressBarColor(float value) => SelectedSubstatsFilter switch
    {
        FactionStatisticsSubstatsFilter.Games or FactionStatisticsSubstatsFilter.Wins => TextColor.Green,
        FactionStatisticsSubstatsFilter.WinPercentage => value.GetWinrateColor(),
        FactionStatisticsSubstatsFilter.Eliminations or FactionStatisticsSubstatsFilter.MinVp => TextColor.Red,
        FactionStatisticsSubstatsFilter.AverageVp => value.GetAverageVpColor(),
        FactionStatisticsSubstatsFilter.MaxVp => TextColor.Green,
        FactionStatisticsSubstatsFilter.AverageVpPercentage => value.GetAverageVpPercentageColor(),
        _ => TextColor.Green,
    };

    private bool ShowSubstatsValue() => SelectedSubstatsFilter is not FactionStatisticsSubstatsFilter.WinPercentage and not FactionStatisticsSubstatsFilter.Wins;

    private static TextColor GetWinColor(int wins) => wins > 0 ? TextColor.Green : TextColor.Red;

    private IReadOnlyCollection<AsyncFactionsStatsDto> GetSortedFactionsForDisplayByStatistics() => SelectedSubstatsFilter switch
    {
        FactionStatisticsSubstatsFilter.Games => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Games).ToList(),
        FactionStatisticsSubstatsFilter.Wins => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Wins).ToList(),
        FactionStatisticsSubstatsFilter.WinPercentage => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).WinsPercentage).ToList(),
        FactionStatisticsSubstatsFilter.Eliminations => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Eliminations).ToList(),
        FactionStatisticsSubstatsFilter.AverageVp => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).Games == 0 ? 0 : (float)GetCorrectFactionStatsByVp(x).Vp / GetCorrectFactionStatsByVp(x).Games).ToList(),
        FactionStatisticsSubstatsFilter.AverageVpPercentage => FactionsForDisplay.OrderByDescending(x => GetCorrectFactionStatsByVp(x).VpPercentage).ToList(),
        _ => FactionsForDisplay,
    };
}