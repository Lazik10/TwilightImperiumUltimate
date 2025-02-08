namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncGamesStatsDto
{
    public AsyncGamesStatsDto()
    {
    }

    public AsyncGamesStatsDto(
        IReadOnlyCollection<AsyncGamesPlayerDto> mostGames,
        IReadOnlyCollection<AsyncGamesPlayerDto> mostActiveGames)
    {
        MostGames = mostGames;
        MostActiveGames = mostActiveGames;
    }

    public IReadOnlyCollection<AsyncGamesPlayerDto> MostGames { get; set; } = new List<AsyncGamesPlayerDto>();

    public IReadOnlyCollection<AsyncGamesPlayerDto> MostActiveGames { get; set; } = new List<AsyncGamesPlayerDto>();
}
