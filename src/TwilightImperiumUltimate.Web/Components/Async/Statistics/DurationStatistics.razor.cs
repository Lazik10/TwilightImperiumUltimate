using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;
using TwilightImperiumUltimate.Web.Options.Async;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class DurationStatistics
{
    private IJSObjectReference? _jsModule;
    private bool _isDataLoaded;
    private AsyncDurationsSummaryStatsDto _durationSummaryStats = new AsyncDurationsSummaryStatsDto();

    [CascadingParameter(Name = "Filter")]
    public PlayerStatisticsType Filter { get; set; }

    [CascadingParameter(Name = "Limit")]
    public int QueryLimit { get; set; }

    public AsyncDurationsStatsDto DurationsStats => Filter switch
    {
        PlayerStatisticsType.All => _durationSummaryStats.All,
        PlayerStatisticsType.Tigl => _durationSummaryStats.Tigl,
        PlayerStatisticsType.Custom => _durationSummaryStats.Custom,
        _ => _durationSummaryStats.All,
    };

    [Inject]
    private IAsyncStatsProvider AsyncStatsProvider { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private IConfiguration Configuration { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _isDataLoaded = false;
        _durationSummaryStats = await AsyncStatsProvider.GetDurationsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnParametersSetAsync()
    {
        _isDataLoaded = false;
        StateHasChanged();
        _durationSummaryStats = await AsyncStatsProvider.GetDurationsStatistics(QueryLimit);
        _isDataLoaded = true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Async/Games/AsyncGamesList.razor.js");
        }
    }

    private async Task RedirectToGameDetails(string gameId)
    {
        if (_jsModule is null)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Async/Statistics/DurationStatistics.razor.js");
        }

        var path = Configuration.GetSection(nameof(AsyncServerOptions))[nameof(AsyncServerOptions.BaseGameUrl)];
        _ = Task.Run(async () => await _jsModule.InvokeVoidAsync("openInNewTab", $"{path}{gameId}"));
    }
}
