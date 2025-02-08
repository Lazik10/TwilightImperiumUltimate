namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncHistoryStatsDto
{
    public AsyncHistoryStatsDto()
    {
    }

    public AsyncHistoryStatsDto(
        IReadOnlyCollection<AsyncGamesHistoryDto> gamesHistory,
        IReadOnlyCollection<AsyncPlayersHistoryDto> playersHistory)
    {
        GamesHistory = gamesHistory;
        PlayersHistory = playersHistory;
    }

    public IReadOnlyCollection<AsyncGamesHistoryDto> GamesHistory { get; set; } = new List<AsyncGamesHistoryDto>();

    public IReadOnlyCollection<AsyncPlayersHistoryDto> PlayersHistory { get; set; } = new List<AsyncPlayersHistoryDto>();
}
