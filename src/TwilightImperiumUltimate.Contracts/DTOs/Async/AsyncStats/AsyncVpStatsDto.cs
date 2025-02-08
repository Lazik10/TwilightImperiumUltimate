namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncVpStatsDto
{
    public AsyncVpStatsDto()
    {
    }

    public AsyncVpStatsDto(
        IReadOnlyCollection<AsyncVpPlayerDto> vpPercentagesPlayers,
        IReadOnlyCollection<AsyncVpPlayerDto> mostVpPlayers)
    {
        this.VpPercentagesPlayers = vpPercentagesPlayers;
        this.MostVpPlayers = mostVpPlayers;
    }

    public IReadOnlyCollection<AsyncVpPlayerDto> VpPercentagesPlayers { get; set; } = new List<AsyncVpPlayerDto>();

    public IReadOnlyCollection<AsyncVpPlayerDto> MostVpPlayers { get; set; } = new List<AsyncVpPlayerDto>();
}
