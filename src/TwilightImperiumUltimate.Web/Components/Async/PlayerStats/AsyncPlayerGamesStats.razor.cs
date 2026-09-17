using Microsoft.AspNetCore.Components.Web;
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
    private readonly HashSet<int> _expandedYears = new();
    private readonly HashSet<(int Year, int Month)> _expandedMonths = new();
    private IJSObjectReference? _jsModule;
    private bool _expansionInitialized;
    private bool _allExpanded;
    private PlayerStatisticsType _lastStatType;

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

    protected override void OnParametersSet()
    {
        if (_expansionInitialized && _lastStatType == StatType)
            return;

        _lastStatType = StatType;
        _expansionInitialized = true;
        InitializeDefaultExpansion();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Async/Games/AsyncGamesList.razor.js");
        }
    }

    private void InitializeDefaultExpansion()
    {
        _expandedYears.Clear();
        _expandedMonths.Clear();
        _allExpanded = false;

        // "Current" year/month = the most recently played game's year/month, i.e. the first
        // (most recent, since GroupedGames orders descending) group -- falls back naturally to
        // the last game ever played when nothing was played in the actual current month.
        var mostRecentYear = GroupedGames.FirstOrDefault();
        if (mostRecentYear is null)
            return;

        _expandedYears.Add(mostRecentYear.Year.Year);

        var mostRecentMonth = mostRecentYear.Months.FirstOrDefault();
        if (mostRecentMonth is not null)
            _expandedMonths.Add((mostRecentYear.Year.Year, mostRecentMonth.Month.Month));

        // Active (still in-progress) games are never hidden behind a collapsed year/month, even
        // if they were played outside the most recent month.
        foreach (var year in GroupedGames)
        {
            foreach (var month in year.Months)
            {
                if (!month.Games.Any(game => !game.Ended))
                    continue;

                _expandedYears.Add(year.Year.Year);
                _expandedMonths.Add((year.Year.Year, month.Month.Month));
            }
        }
    }

    private void ToggleExpandAll()
    {
        if (_allExpanded)
        {
            InitializeDefaultExpansion();
            return;
        }

        foreach (var year in GroupedGames)
        {
            _expandedYears.Add(year.Year.Year);
            foreach (var month in year.Months)
                _expandedMonths.Add((year.Year.Year, month.Month.Month));
        }

        _allExpanded = true;
    }

    private bool IsYearExpanded(int year) => _expandedYears.Contains(year);

    private bool IsMonthExpanded(int year, int month) => _expandedMonths.Contains((year, month));

    private void ToggleYear(int year)
    {
        if (!_expandedYears.Remove(year))
            _expandedYears.Add(year);
    }

    private void ToggleMonth(int year, int month)
    {
        var key = (year, month);
        if (!_expandedMonths.Remove(key))
            _expandedMonths.Add(key);
    }

    private void OnYearKeyDown(KeyboardEventArgs e, int year)
    {
        if (e.Key is "Enter" or " ")
            ToggleYear(year);
    }

    private void OnMonthKeyDown(KeyboardEventArgs e, int year, int month)
    {
        if (e.Key is "Enter" or " ")
            ToggleMonth(year, month);
    }

    private void OnGameKeyDown(KeyboardEventArgs e, string gameId)
    {
        if (e.Key is "Enter" or " ")
            _ = RedirectToGameDetails(gameId);
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

        return parts.Count > 0 ? string.Join(" ", parts) : "0m";
    }

    private string GetAsyncGameIdTextColor(AsyncPlayerGameDto game)
    {
        if (game.Ended && game.ValidEnd)
            return "color: red;";
        else if (game.Ended)
            return "color: orange;";
        else
            return "color: lawngreen;";
    }

    private bool ShowWins(AsyncPlayerGameDto game)
    {
        var isFowGame = game.AsyncGameID == "fow";
        return !isFowGame && AsyncPlayerProfile.Settings.ShowWinRates;
    }

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
