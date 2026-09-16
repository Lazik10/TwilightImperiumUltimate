using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Models.Async;
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
}
