namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncHistoryStatsDto
{
    public AsyncHistoryStatsDto()
    {
    }

    public AsyncHistoryStatsDto(
        IReadOnlyCollection<AsyncGamesHistoryDto> gamesHistory,
        IReadOnlyCollection<AsyncGamesHistoryDto> gamesEndedHistory,
        IReadOnlyCollection<AsyncPlayersHistoryDto> playersHistory)
    {
        GamesHistory = gamesHistory;
        GamesEndedHistory = gamesEndedHistory;
        PlayersHistory = playersHistory;
    }

    public IReadOnlyCollection<AsyncGamesHistoryDto> GamesHistory { get; set; } = new List<AsyncGamesHistoryDto>();

    public IReadOnlyCollection<AsyncGamesHistoryDto> GamesEndedHistory { get; set; } = new List<AsyncGamesHistoryDto>();

    public IReadOnlyCollection<AsyncPlayersHistoryDto> PlayersHistory { get; set; } = new List<AsyncPlayersHistoryDto>();
}
