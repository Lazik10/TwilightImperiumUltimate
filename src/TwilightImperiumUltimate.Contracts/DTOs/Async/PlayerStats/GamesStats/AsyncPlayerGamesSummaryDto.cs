namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.GamesStats;

public record AsyncPlayerGamesSummaryDto
{
    public AsyncPlayerGamesSummaryDto()
    {
        Games = new List<AsyncPlayerGameDto>();
    }

    public AsyncPlayerGamesSummaryDto(IReadOnlyCollection<AsyncPlayerGameDto> games)
    {
        Games = games;
    }

    public IReadOnlyCollection<AsyncPlayerGameDto> Games { get; set; }
}
