namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncOpponentsStatsDto
{
    public AsyncOpponentsStatsDto()
    {
    }

    public AsyncOpponentsStatsDto(
        IReadOnlyCollection<AsyncOpponentsPlayerDto> playersWithMostOpponents)
    {
        PlayersWithMostOpponents = playersWithMostOpponents;
    }

    public IReadOnlyCollection<AsyncOpponentsPlayerDto> PlayersWithMostOpponents { get; set; } = new List<AsyncOpponentsPlayerDto>();
}
