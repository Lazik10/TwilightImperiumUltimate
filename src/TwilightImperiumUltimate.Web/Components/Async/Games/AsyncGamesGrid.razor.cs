using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;
using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Games;

public partial class AsyncGamesGrid
{
    private AsyncGameDatesDto _gameDates = new(new List<AsyncGameYearMonthDto>());

    private IReadOnlyCollection<string> _gameNames = new List<string>();

    private string _asyncGameDiscordId = string.Empty;

    private AsyncGameStatusFilter _gameStatus = AsyncGameStatusFilter.All;

    private AsyncGameType _asyncGameType = AsyncGameType.All;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IAsyncGamesProvider AsyncGamesProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await GetAsyncGameDates();

        if (_gameNames.Count == 0)
        {
            _gameNames = await AsyncGamesProvider.GetAsyncGameNames();
        }
    }

    private async Task GetAsyncGameDates()
    {
        _gameDates = await AsyncGamesProvider.GetAsyncGameDates();
    }

    private void OnGameSelect(string gameName)
    {
        _asyncGameDiscordId = gameName;
        StateHasChanged();
    }

    private void GameStatusChanged(AsyncGameStatusFilter status)
    {
        _asyncGameDiscordId = string.Empty;
        _gameStatus = status;
        StateHasChanged();
    }

    private void GameTypeChanged(AsyncGameType type)
    {
        _asyncGameDiscordId = string.Empty;
        _asyncGameType = type;
        StateHasChanged();
    }
}