using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;
using TwilightImperiumUltimate.Web.Options.Async;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Games;

public partial class AsyncGamesList
{
    private IReadOnlyCollection<AsyncGameDto> _games = new List<AsyncGameDto>();
    private IJSObjectReference? _jsModule;
    private bool _isLoaded;
    private int _requestVersion;
    private (DateTime From, DateTime To, string DiscordId, string FunName, AsyncGameStatusFilter Status, AsyncGameType Type)? _lastRequest;

    [Parameter]
    public AsyncGameStatusFilter StatusFilter { get; set; } = AsyncGameStatusFilter.All;

    [Parameter]
    public AsyncGameType AsyncGameType { get; set; } = AsyncGameType.All;

    [Parameter]
    public AsyncGameDatesDto GameDates { get; set; } = new(new List<AsyncGameYearMonthDto>());

    [Parameter]
    public string AsyncGameDiscordId { get; set; } = string.Empty;

    [Parameter]
    public string AsyncGameFunName { get; set; } = string.Empty;

    [Parameter]
    public DateTime DateFrom { get; set; }

    [Parameter]
    public DateTime DateTo { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IAsyncGamesProvider AsyncGamesProvider { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private IConfiguration Configuration { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        var request = (DateFrom, DateTo, AsyncGameDiscordId, AsyncGameFunName, StatusFilter, AsyncGameType);
        if (_lastRequest == request)
            return;

        _lastRequest = request;
        var requestVersion = ++_requestVersion;
        _isLoaded = false;

        if (string.IsNullOrWhiteSpace(AsyncGameDiscordId)
            && string.IsNullOrWhiteSpace(AsyncGameFunName)
            && (DateFrom == default || DateTo == default))
        {
            return;
        }

        await UpdateGameList(requestVersion);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Async/Games/AsyncGamesList.razor.js");
        }
    }

    private static IEnumerable<(int Year, int Month)> GetMonthsInRange(DateTime from, DateTime to)
    {
        var month = new DateTime(from.Year, from.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var lastMonth = new DateTime(to.Year, to.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        while (month <= lastMonth)
        {
            yield return (month.Year, month.Month);
            month = month.AddMonths(1);
        }
    }

    private static bool IsWithinDateRange(AsyncGameDto game, DateTime from, DateTime to)
    {
        var startDate = DateTimeOffset.FromUnixTimeSeconds(game.StartDate).Date;
        return startDate >= from && startDate <= to;
    }

    private string GetDurationTime(AsyncGameDto game)
    {
        if (game.EndDate == 0 && game.Finished)
            return "Unknown";

        var startDate = DateTimeOffset.FromUnixTimeSeconds(game.StartDate);
        var endDate = game.EndDate != 0 ? DateTimeOffset.FromUnixTimeSeconds(game.EndDate) : DateTimeOffset.Now;

        var duration = endDate - startDate;

        List<string> parts = new List<string>();

        if (duration.Days > 0)
            parts.Add($"{duration.Days:D2} d");
        if (duration.Hours > 0)
            parts.Add($"{duration.Hours:D2} h");

        return parts.Count > 0 ? string.Join(" ", parts) : "0s";
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

    private Task HandleGameRowClick(AsyncGameDto game) => RedirectToGameDetails(game.AsyncGameID);

    private async Task UpdateGameList(int requestVersion)
    {
        var from = DateFrom == default ? DateTime.UtcNow.Date : DateFrom.Date;
        var to = DateTo == default ? from : DateTo.Date;
        IReadOnlyCollection<AsyncGameDto> games;

        if (!string.IsNullOrWhiteSpace(AsyncGameDiscordId))
        {
            games = [await AsyncGamesProvider.GetAsyncGameByDiscordId(AsyncGameDiscordId)];
        }
        else if (!string.IsNullOrWhiteSpace(AsyncGameFunName))
        {
            games = [await AsyncGamesProvider.GetAsyncGameByFunName(AsyncGameFunName)];
        }
        else
        {
            var months = GetMonthsInRange(from, to).ToList();
            var monthResults = await Task.WhenAll(months.Select(month => AsyncGamesProvider.GetAsyncGamesFromYearAndMonth(month.Year, month.Month)));
            games = monthResults.SelectMany(x => x).ToList();
        }

        if (requestVersion != _requestVersion)
            return;

        var filteredGames = FilterGames(games);
        _games = string.IsNullOrWhiteSpace(AsyncGameDiscordId) && string.IsNullOrWhiteSpace(AsyncGameFunName)
            ? filteredGames.Where(game => IsWithinDateRange(game, from, to)).ToList()
            : filteredGames;

        _isLoaded = true;
    }

    private List<AsyncGameDto> FilterGames(IEnumerable<AsyncGameDto> games)
    {
        return games
            .Where(ApplyStatusFilter)
            .Where(ApplyGameTypeFilter)
            .ToList();
    }

    private bool ApplyStatusFilter(AsyncGameDto game)
    {
        return StatusFilter switch
        {
            AsyncGameStatusFilter.All => true,
            AsyncGameStatusFilter.Active => !game.Finished,
            AsyncGameStatusFilter.Forfeited => game.Finished && !game.ValidEnd,
            AsyncGameStatusFilter.Finished => game.Finished && game.ValidEnd,
            _ => false,
        };
    }

    private bool ApplyGameTypeFilter(AsyncGameDto game)
    {
        return AsyncGameType switch
        {
            AsyncGameType.All => true,
            AsyncGameType.Tigl => game.IsTigl,
            _ => !game.IsTigl,
        };
    }

    private string GetAsyncGameIdTextColor(AsyncGameDto game)
    {
        if (game.Finished && game.ValidEnd)
            return "color: red; justify-content: flex-start;";
        else if (game.Finished)
            return "color: orange; justify-content: flex-start;";
        else
            return "color: lawngreen; justify-content: flex-start;";
    }
}
