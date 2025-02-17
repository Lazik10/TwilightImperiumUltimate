using Microsoft.JSInterop;
using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;
using TwilightImperiumUltimate.Web.Options.Async;
using TwilightImperiumUltimate.Web.Pages.Community;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Games;

public partial class AsyncGamesList
{
    private IReadOnlyCollection<AsyncGameDto> _games = new List<AsyncGameDto>();
    private IJSObjectReference? _jsModule;
    private bool _isLoaded;
    private bool _datesSet;
    private int _year;
    private int _month;

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
        if (GameDates.GameDates.Count > 0 && !_datesSet)
        {
            _year = GameDates.GameDates.First().Year;
            _month = GameDates.GameDates.First().Months.First();
            _datesSet = true;
        }

        await UpdateGameList();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Async/Games/AsyncGamesList.razor.js");
        }
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
        if (duration.Minutes > 0)
            parts.Add($"{duration.Minutes:D2} m");
        if (duration.Seconds > 0)
            parts.Add($"{duration.Seconds:D2} s");

        return parts.Count > 0 ? string.Join(" ", parts) : "0s";
    }

    private async Task HandleYearMonthSelected((int Year, int Month) date)
    {
        _year = date.Year;
        _month = date.Month;

        AsyncGameDiscordId = string.Empty;
        AsyncGameFunName = string.Empty;

        await UpdateGameList();

        StateHasChanged();
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

    private async Task UpdateGameList()
    {
        _isLoaded = false;
        StateHasChanged();

        if (_year != 0 && _month != 0 && string.IsNullOrEmpty(AsyncGameDiscordId) && string.IsNullOrEmpty(AsyncGameFunName))
        {
            var games = await AsyncGamesProvider.GetAsyncGamesFromYearAndMonth(_year, _month);

            _games = FilterGames(games);
        }
        else if (!string.IsNullOrEmpty(AsyncGameDiscordId))
        {
            var game = await AsyncGamesProvider.GetAsyncGameByDiscordId(AsyncGameDiscordId);
            var startDate = DateTimeOffset.FromUnixTimeSeconds(game.StartDate);
            _year = startDate.Year;
            _month = startDate.Month;
            _games = new List<AsyncGameDto> { game };
        }
        else if (!string.IsNullOrEmpty(AsyncGameFunName))
        {
            var game = await AsyncGamesProvider.GetAsyncGameByFunName(AsyncGameFunName);
            var startDate = DateTimeOffset.FromUnixTimeSeconds(game.StartDate);
            _year = startDate.Year;
            _month = startDate.Month;
            _games = new List<AsyncGameDto> { game };
        }

        _isLoaded = true;
        StateHasChanged();
    }

    private List<AsyncGameDto> FilterGames(IReadOnlyCollection<AsyncGameDto> games)
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