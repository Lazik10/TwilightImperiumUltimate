using Radzen;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Components.Charts;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class FactionStatistics
{
    private bool _areFiltersVisible;
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

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string FilterButtonLabel => (_areFiltersVisible ? "Hide faction filters" : "Show faction filters")
        + (AreFiltersApplied ? ", filters active" : string.Empty);

    private string FilterIconPath => PathProvider.GetIconPath(_areFiltersVisible || AreFiltersApplied ? IconType.FilterClicked : IconType.Filter);

    private bool AreFiltersApplied => _selectedFactionStatisticsFilter != FactionStatisticsFilter.Official
        || _selectedFactionVpStatisticsFilter != FactionStatisticsVpFilter.All
        || _selectedFactionStatisticsSubstatsFilter != FactionStatisticsSubstatsFilter.All;

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

    private List<KeyValuePair<FactionStatisticsFilter, string>> GetFactionFilterOptions() => Enum.GetValues<FactionStatisticsFilter>()
        .Select(value => new KeyValuePair<FactionStatisticsFilter, string>(value, value.GetDisplayName()))
        .ToList();

    private List<KeyValuePair<FactionStatisticsVpFilter, string>> GetFactionVpFilterOptions() => Enum.GetValues<FactionStatisticsVpFilter>()
        .Select(value => new KeyValuePair<FactionStatisticsVpFilter, string>(value, value.GetDisplayName()))
        .ToList();

    private List<KeyValuePair<FactionStatisticsSubstatsFilter, string>> GetFactionSubstatsFilterOptions() => Enum.GetValues<FactionStatisticsSubstatsFilter>()
        .Except(_excludedValues)
        .Select(value => new KeyValuePair<FactionStatisticsSubstatsFilter, string>(value, value.GetDisplayName()))
        .ToList();

    private string FormatFactionWinPercentageValue(double value) => ((float)value).ToStringWithPrecisionAndPercentage(2);

    private string FormatFactionAverageVpValue(double value) => ((float)value).ToStringWithPrecision(2);

    private string FormatFactionAverageVpPercentageValue(double value) => ((float)value).ToStringWithPrecisionAndPercentage(1);

    private string RemoveTrailingColon(string value) => value.TrimEnd(':');

    private void ToggleFilters() => _areFiltersVisible = !_areFiltersVisible;

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
            FactionStatisticsFilter.TwilightsFall => factionStats.Where(faction => faction.FactionName is >= AsyncFactionName.TheRubyMonarch and <= AsyncFactionName.ASickeningLurch).ToList(),
            FactionStatisticsFilter.DiscordantStars => factionStats.Where(faction => faction.FactionName is >= AsyncFactionName.TheAugursOfIlyxum and <= AsyncFactionName.TheZelianPurifier).ToList(),
            FactionStatisticsFilter.BlueReverie => factionStats.Where(faction => faction.FactionName is >= AsyncFactionName.AtokeraLegacy and <= AsyncFactionName.XinCourt).ToList(),
            FactionStatisticsFilter.WhispersFromTheVoid => factionStats.Where(faction => faction.FactionName is >= AsyncFactionName.TheArvaxiCovenant and <= AsyncFactionName.TheZephyrionPursuit).ToList(),
            FactionStatisticsFilter.Others => factionStats.Where(faction => faction.FactionName is >= AsyncFactionName.Franken and <= AsyncFactionName.Homebrew).ToList(),
            _ => factionStats.Where(faction => faction.FactionName is >= AsyncFactionName.TheArborec and <= AsyncFactionName.TheRalNelConsortium).ToList(),
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

    private List<RankingBarPoint> GetGamesData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).Games)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).Games,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private List<RankingBarPoint> GetWinsData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).Wins)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).Wins,
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private List<RankingBarPoint> GetWinPercentageData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).WinsPercentage)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).WinsPercentage,
            GetCorrectFactionStatsByVp(faction).WinsPercentage.GetWinrateColor().GetChartFillColor(),
            null))
        .ToList();

    private List<RankingBarPoint> GetEliminationsData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).Eliminations)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).Eliminations,
            TextColor.Red.GetChartFillColor(),
            null))
        .ToList();

    private List<RankingBarPoint> GetAverageVpData() => FactionsForDisplay
        .OrderByDescending(GetAverageVpValue)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetAverageVpValue(faction),
            TextColor.Green.GetChartFillColor(),
            null))
        .ToList();

    private List<RankingBarPoint> GetAverageVpPercentageData() => FactionsForDisplay
        .OrderByDescending(faction => GetCorrectFactionStatsByVp(faction).VpPercentage)
        .Select((faction, index) => new RankingBarPoint(
            index.ToString(),
            faction.FactionName.GetFactionUIText(FactionResourceType.Title),
            GetCorrectFactionStatsByVp(faction).VpPercentage,
            GetCorrectFactionStatsByVp(faction).VpPercentage.GetAverageVpPercentageColor().GetChartFillColor(),
            null))
        .ToList();
}
