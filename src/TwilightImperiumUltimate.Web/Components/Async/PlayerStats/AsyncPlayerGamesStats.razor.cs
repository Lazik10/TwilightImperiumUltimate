using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.GamesStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Web.Models.Async;
using TwilightImperiumUltimate.Web.Options.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.PlayerStats;

public partial class AsyncPlayerGamesStats
{
    private IJSObjectReference? _jsModule;

    [CascadingParameter(Name = "AsyncPlayerProfile")]
    public AsyncPlayerProfileSummaryStatsDto AsyncPlayerProfile { get; set; } = default!;

    [CascadingParameter(Name = "AsyncPlayerStatisticsType")]
    public PlayerStatisticsType StatType { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private IConfiguration Configuration { get; set; } = default!;

    private IEnumerable<GamesByYear> GroupedGames => GetGamesByStatType()
        .GroupBy(game => DateTimeOffset.FromUnixTimeSeconds(game.StartDate).Year)
        .OrderByDescending(yearGroup => yearGroup.Key)
        .Select(yearGroup => new GamesByYear
        {
            Year = new DateOnly(yearGroup.Key, 1, 1),
            Months = yearGroup
                .GroupBy(game => DateTimeOffset.FromUnixTimeSeconds(game.StartDate).Month)
                .OrderByDescending(monthGroup => monthGroup.Key)
                .Select(monthGroup => new GamesByMonth
                {
                    Month = new DateOnly(yearGroup.Key, monthGroup.Key, 1),
                    Games = monthGroup.ToList(),
                }),
        });

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Async/Games/AsyncGamesList.razor.js");
        }
    }

    private IReadOnlyCollection<AsyncPlayerGameDto> GetGamesByStatType()
    {
        return StatType switch
        {
            PlayerStatisticsType.All => AsyncPlayerProfile.Games.Games,
            PlayerStatisticsType.Tigl => AsyncPlayerProfile.Games.Games.Where(game => game.IsTigl).ToList(),
            PlayerStatisticsType.Custom => AsyncPlayerProfile.Games.Games.Where(game => !game.IsTigl).ToList(),
            _ => AsyncPlayerProfile.Games.Games,
        };
    }

    private string GetDurationTime(AsyncPlayerGameDto game)
    {
        var startDate = DateTimeOffset.FromUnixTimeSeconds(game.StartDate);
        var endDate = game.EndDate != 0 ? DateTimeOffset.FromUnixTimeSeconds(game.EndDate) : DateTimeOffset.Now;

        var duration = endDate - startDate;

        List<string> parts = new List<string>();

        if (duration.Days > 0)
            parts.Add($"{duration.Days:D2} d");
        if (duration.Hours > 0)
            parts.Add($"{duration.Hours:D2} h");
        if (duration.Minutes > 0)
            parts.Add($"{duration.Minutes:D2} m");
        if (duration.Seconds > 0)
            parts.Add($"{duration.Seconds:D2} s");

        return parts.Count > 0 ? string.Join(" ", parts) : "0s";
    }

    private string GetAsyncGameIdTextColor(AsyncPlayerGameDto game)
    {
        if (game.Ended && game.ValidEnd)
            return "color: red; justify-content: flex-start;";
        else if (game.Ended)
            return "color: orange; justify-content: flex-start;";
        else
            return "color: lawngreen; justify-content: flex-start;";
    }

    private bool ShowWins() => AsyncPlayerProfile.Settings.ShowWinRates;

    private async Task RedirectToGameDetails(string gameId)
    {
        if (_jsModule is null)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Async/Games/AsyncGamesList.razor.js");
        }

        var path = Configuration.GetSection(nameof(AsyncServerOptions))[nameof(AsyncServerOptions.BaseGameUrl)];
        _ = Task.Run(async () => await _jsModule.InvokeVoidAsync("openInNewTab", $"{path}{gameId}"));
    }
}