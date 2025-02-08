using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class TurnsStatistics
{
    private bool _isDataLoaded;
    private int _row;
    private AsyncTurnsSummaryStatsDto _turnsSummaryStats = new AsyncTurnsSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncTurnsStatsDto TurnsStats => Filter switch
    {
        PlayerStatisticsType.All => _turnsSummaryStats.All,
        PlayerStatisticsType.Tigl => _turnsSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _turnsSummaryStats.Custom,
        _ => _turnsSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _turnsSummaryStats = await AsyncStatsProvider.GetTurnsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isDataLoaded = false;
        StateHasChanged();
        _turnsSummaryStats = await AsyncStatsProvider.GetTurnsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    private string GetAverageTurn(long turnTime)
    {
        TimeSpan averageTurnTime = TimeSpan.FromMilliseconds(turnTime);

        List<string> parts = new List<string>();

        if (averageTurnTime.Days > 0)
            parts.Add($"{averageTurnTime.Days:D2} d");
        if (averageTurnTime.Hours > 0)
            parts.Add($"{averageTurnTime.Hours:D2} h");
        if (averageTurnTime.Minutes > 0)
            parts.Add($"{averageTurnTime.Minutes:D2} m");
        if (averageTurnTime.Seconds > 0)
            parts.Add($"{averageTurnTime.Seconds:D2} s");

        return parts.Count > 0 ? string.Join(" ", parts) : "0s";
    }

    private TextColor GetAverageTurnColor(long turnTime)
    {
        TimeSpan averageTurnTime = TimeSpan.FromMilliseconds(turnTime);

        if (averageTurnTime.Hours > 6)
            return TextColor.Red;
        if (averageTurnTime.Hours > 4)
            return TextColor.Orange;
        if (averageTurnTime.Hours > 2)
            return TextColor.Yellow;

        return TextColor.Green;
    }

    private void RedirectToPlayerProfile(int id)
    {
        NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={id}");
    }
}