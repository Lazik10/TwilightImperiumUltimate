using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class CombatStatistics
{
    private bool _isDataLoaded;
    private int _row;
    private AsyncCombatSummaryStatsDto _combatSummaryStats = new AsyncCombatSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncCombatStatsDto CombatStats => Filter switch
    {
        PlayerStatisticsType.All => _combatSummaryStats.All,
        PlayerStatisticsType.Tigl => _combatSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _combatSummaryStats.Custom,
        _ => _combatSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _combatSummaryStats = await AsyncStatsProvider.GetCombatStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isDataLoaded = false;
        StateHasChanged();
        _combatSummaryStats = await AsyncStatsProvider.GetCombatStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    private void RedirectToPlayerProfile(int id)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={id}");
    }
}