namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;

public record AsyncPlayerVpStatsDto
{
    public AsyncPlayerVpStatsDto()
    {
    }

    public AsyncPlayerVpStatsDto(
        IReadOnlyCollection<AsyncPlayerVpStatsByVpGameDto> vpStatsByVpGame,
        AsyncPlayerVpStatsByVpGameDto vpStatsTotal)
    {
        VpStatsByVpGame = vpStatsByVpGame;
        VpStatsTotal = vpStatsTotal;
    }

    public IReadOnlyCollection<AsyncPlayerVpStatsByVpGameDto> VpStatsByVpGame { get; set; } = new List<AsyncPlayerVpStatsByVpGameDto>();

    public AsyncPlayerVpStatsByVpGameDto VpStatsTotal { get; set; } = new AsyncPlayerVpStatsByVpGameDto();
}
