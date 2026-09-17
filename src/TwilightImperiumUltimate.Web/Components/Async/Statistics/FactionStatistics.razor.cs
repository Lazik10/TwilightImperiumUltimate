using Radzen;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class FactionStatistics
{
    private bool _isDataLoaded;
    private FactionStatisticsFilter _selectedFactionStatisticsFilter = FactionStatisticsFilter.Official;
    private FactionStatisticsVpFilter _selectedFactionVpStatisticsFilter = FactionStatisticsVpFilter.All;
    private FactionStatisticsSubstatsFilter _selectedFactionStatisticsSubstatsFilter = FactionStatisticsSubstatsFilter.All;
    private AsyncFactionsSummaryStatsDto _factionsSummaryStats = new AsyncFactionsSummaryStatsDto();
    private List<FactionStatisticsSubstatsFilter> _excludedValues = new List<FactionStatisticsSubstatsFilter>() { FactionStatisticsSubstatsFilter.MinVp, FactionStatisticsSubstatsFilter.MaxVp };

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    public IReadOnlyCollection<AsyncFactionsStatsDto> FactionsStats => Filter switch
    {
        PlayerStatisticsType.All => _factionsSummaryStats.All,
        PlayerStatisticsType.Tigl => _factionsSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _factionsSummaryStats.Custom,
        _ => _factionsSummaryStats.All,
    };

    public IReadOnlyCollection<AsyncFactionsStatsDto> FactionsForDisplay { get; set; } = new List<AsyncFactionsStatsDto>();

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _factionsSummaryStats = await AsyncStatsProvider.GetFactionsStatistics();
        _isDataLoaded = true;
    }

    protected override void OnParametersSet()
    {
        if (FactionsStats is not null)
            FactionsForDisplay = GetFilteredFactionStats(FactionsStats);
    }

    private static string FormatFactionWinPercentageValue(double value) => ((float)value).ToStringWithPrecisionAndPercentage(2);

    private static string FormatFactionAverageVpValue(double value) => ((float)value).ToStringWithPrecision(2);

    private static string FormatFactionAverageVpPercentageValue(double value) => ((float)value).ToStringWithPrecisionAndPercentage(1);

    private void OnFactionStatisticsFilterChanged(FactionStatisticsFilter filter)
    {
        _selectedFactionStatisticsFilter = filter;
        if (FactionsStats is not null)
            FactionsForDisplay = GetFilteredFactionStats(FactionsStats);
    }

    private void OnFactionStatisticsVpFilterChanged(FactionStatisticsVpFilter filter)
    {
        _selectedFactionVpStatisticsFilter = filter;
        StateHasChanged();
    }

    private void OnFactionStatisticsSubstatsFilterChanged(FactionStatisticsSubstatsFilter filter)
    {
        _selectedFactionStatisticsSubstatsFilter = filter;
        StateHasChanged();
    }

    private List<AsyncFactionsStatsDto> GetFilteredFactionStats(IReadOnlyCollection<AsyncFactionsStatsDto> factionStats)
    {
        return _selectedFactionStatisticsFilter switch
        {
            FactionStatisticsFilter.DiscordantStars => factionStats.Skip(32).Take(40).ToList(),
            FactionStatisticsFilter.Others => factionStats.Skip(72).Where(x => x.FactionName != AsyncFactionName.Unknown && x.FactionName != AsyncFactionName.TwilightsFall).ToList(),
            _ => factionStats.Take(32).ToList(),
        };
    }

    private AsyncFactionStatsByGameVpDto GetCorrectFactionStatsByVp(AsyncFactionsStatsDto factionStats) => _selectedFactionVpStatisticsFilter switch
    {
        FactionStatisticsVpFilter.TenVp => factionStats.TenVp,
        FactionStatisticsVpFilter.TwelveVp => factionStats.TwelveVp,
        FactionStatisticsVpFilter.FourteenVp => factionStats.FourteenVp,
        _ => factionStats.All,
    };

    private float GetAverageVpValue(AsyncFactionsStatsDto factionStats)
    {
        var stats = GetCorrectFactionStatsByVp(factionStats);
        return stats.Games == 0 ? 0 : (float)stats.Vp / stats.Games;
    }

    private IReadOnlyCollection<RankingBarPoint> GetGamesData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).Games)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).Games,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetWinsData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).Wins)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).Wins,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetWinPercentageData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).WinsPercentage)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).WinsPercentage,
            GetCorrectFactionStatsByVp(faction).WinsPercentage.GetWinrateColor().GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetEliminationsData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).Eliminations)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).Eliminations,
            TextColor.Red.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetAverageVpData() => FactionsForDisplay
        .OrderByDescending(GetAverageVpValue)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetAverageVpValue(faction),
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private IReadOnlyCollection<RankingBarPoint> GetAverageVpPercentageData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).VpPercentage)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).VpPercentage,
            GetCorrectFactionStatsByVp(faction).VpPercentage.GetAverageVpPercentageColor().GetChartFillColor(),
            null))
        .ToList();
}
