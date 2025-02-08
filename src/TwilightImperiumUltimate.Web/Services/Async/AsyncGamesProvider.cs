using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;

namespace TwilightImperiumUltimate.Web.Services.Async;

public class AsyncGamesProvider(
    ITwilightImperiumApiHttpClient httpClient)
    : IAsyncGamesProvider
{
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;

    private readonly Dictionary<(int, int), IReadOnlyCollection<AsyncGameDto>> _gamesCache = new();

    private IReadOnlyCollection<string> _gameNames = new List<string>();

    public Task<AsyncGameDto> GetAsyncGame(int gameId)
    {
        throw new NotImplementedException();
    }

    public async Task<AsyncGameDto> GetAsyncGameByDiscordId(string asyncGameDiscordId)
    {
        (var response, var statusCode) = await _httpClient.GetAsync<ApiResponse<AsyncGameDto>>(Paths.ApiPath_AsyncGameByDiscordId, $"?discordId={asyncGameDiscordId}");
        if (statusCode == HttpStatusCode.OK)
            return response!.Data!;

        return new AsyncGameDto(-1, string.Empty, string.Empty, 0, 0, false, false, 0, 0, 0, false);
    }

    public async Task<AsyncGameDatesDto> GetAsyncGameDates()
    {
        (var response, var statusCode) = await _httpClient.GetAsync<ApiResponse<AsyncGameDatesDto>>(Paths.ApiPath_AsyncGameDates);
        if (statusCode == HttpStatusCode.OK)
            return response!.Data!;

        return new AsyncGameDatesDto(new List<AsyncGameYearMonthDto>());
    }

    public async Task<IReadOnlyCollection<string>> GetAsyncGameNames()
    {
        if (_gameNames.Count > 0)
            return _gameNames;

        (var response, var statusCode) = await _httpClient.GetAsync<ApiResponse<AsyncGameNamesDto>>(Paths.ApiPath_AsyncGameNames);
        if (statusCode == HttpStatusCode.OK)
            _gameNames = response!.Data!.GameNames;

        return _gameNames;
    }

    public async Task<IReadOnlyCollection<AsyncGameDto>> GetAsyncGamesFromYearAndMonth(int year, int month)
    {
        var key = (year, month);

        if (_gamesCache.TryGetValue(key, out var cachedGames))
        {
            return cachedGames;
        }

        var games = await GetAsyncGamesFromDateWindow(year, month);
        if (games.Count > 0)
            _gamesCache[key] = games;

        return games;
    }

    private async Task<IReadOnlyCollection<AsyncGameDto>> GetAsyncGamesFromDateWindow(int year, int month)
    {
        var request = new AsyncGamesByYearAndMonthRequest(year, month);
        (var response, var statusCode) = await _httpClient.PostAsync<AsyncGamesByYearAndMonthRequest, ApiResponse<ItemListDto<AsyncGameDto>>>(Paths.ApiPath_AsyncGamesByDate, request);
        if (statusCode == HttpStatusCode.OK)
            return response!.Data!.Items;

        return new List<AsyncGameDto>();
    }
}
