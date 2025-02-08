using TwilightImperiumUltimate.Contracts.DTOs.Async;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Games;

namespace TwilightImperiumUltimate.Web.Services.Async;

public interface IAsyncGamesProvider
{
    Task<AsyncGameDatesDto> GetAsyncGameDates();

    Task<IReadOnlyCollection<string>> GetAsyncGameNames();

    Task<AsyncGameDto> GetAsyncGame(int gameId);

    Task<AsyncGameDto> GetAsyncGameByDiscordId(string asyncGameDiscordId);

    Task<IReadOnlyCollection<AsyncGameDto>> GetAsyncGamesFromYearAndMonth(int year, int month);
}
