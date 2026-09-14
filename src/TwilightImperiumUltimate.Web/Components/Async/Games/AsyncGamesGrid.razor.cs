using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Games;

public partial class AsyncGamesGrid
{
    private AsyncGameDatesDto _gameDates = new(new List<AsyncGameYearMonthDto>());

    private IReadOnlyCollection<string> _gameNames = new List<string>();

    private IReadOnlyCollection<string> _gameFunNames = new List<string>();

    private string _asyncGameDiscordId = string.Empty;

    private string _asyncGameFunName = string.Empty;

    private AsyncGameStatusFilter _gameStatus = AsyncGameStatusFilter.All;

    private AsyncGameType _asyncGameType = AsyncGameType.All;
    private DateTime _dateFrom;
    private DateTime _dateTo;
    private IReadOnlyCollection<AsyncGameType> _gameTypes = Enum.GetValues<AsyncGameType>();
    private IReadOnlyCollection<AsyncGameStatusFilter> _gameStatuses = Enum.GetValues<AsyncGameStatusFilter>();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IAsyncGamesProvider AsyncGamesProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetAsyncGameDates();
        SetDefaultDateRange();

        var namesTask = AsyncGamesProvider.GetAsyncGameNames();
        var funNamesTask = AsyncGamesProvider.GetAsyncGameFunNames();
        await Task.WhenAll(namesTask, funNamesTask);
        _gameNames = await namesTask;
        _gameFunNames = await funNamesTask;
    }

    private async Task GetAsyncGameDates()
    {
        _gameDates = await AsyncGamesProvider.GetAsyncGameDates();
    }

    private void SetDefaultDateRange()
    {
        var currentMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var hasCurrentMonth = _gameDates.GameDates.Any(x => x.Year == currentMonth.Year && x.Months.Contains(currentMonth.Month));
        var selectedMonth = hasCurrentMonth
            ? currentMonth
            : _gameDates.GameDates.OrderByDescending(x => x.Year).ThenByDescending(x => x.Months.Max()).Select(x => new DateTime(x.Year, x.Months.Max(), 1, 0, 0, 0, DateTimeKind.Utc)).FirstOrDefault(currentMonth);

        _dateFrom = selectedMonth;
        _dateTo = selectedMonth.AddMonths(1).AddDays(-1);
    }

    private void OnGameSelect(string gameName)
    {
        _asyncGameDiscordId = gameName;
        _asyncGameFunName = string.Empty;
        StateHasChanged();
    }

    private void OnDateFromChanged(DateTime value)
    {
        _dateFrom = value;
        if (_dateTo < _dateFrom)
            _dateTo = _dateFrom;
    }

    private void OnDateToChanged(DateTime value)
    {
        _dateTo = value;
        if (_dateFrom > _dateTo)
            _dateFrom = _dateTo;
    }

    private void OnGameFunNameSelect(string gameFunName)
    {
        _asyncGameDiscordId = string.Empty;
        _asyncGameFunName = gameFunName;
        StateHasChanged();
    }

    private void GameStatusChanged(AsyncGameStatusFilter status)
    {
        _asyncGameDiscordId = string.Empty;
        _asyncGameFunName = string.Empty;
        _gameStatus = status;
        StateHasChanged();
    }

    private void GameTypeChanged(AsyncGameType type)
    {
        _asyncGameDiscordId = string.Empty;
        _asyncGameFunName = string.Empty;
        _asyncGameType = type;
        StateHasChanged();
    }
}
