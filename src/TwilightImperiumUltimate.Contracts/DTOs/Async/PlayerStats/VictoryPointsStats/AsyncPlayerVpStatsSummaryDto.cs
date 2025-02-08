namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;

public record AsyncPlayerVpStatsSummaryDto
{
    public AsyncPlayerVpStatsSummaryDto()
    {
        All = new();
        Tigl = new();
        Custom = new();
    }

    public AsyncPlayerVpStatsSummaryDto(
        AsyncPlayerVpStatsDto all,
        AsyncPlayerVpStatsDto tigl,
        AsyncPlayerVpStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncPlayerVpStatsDto All { get; set; }

    public AsyncPlayerVpStatsDto Tigl { get; set; }

    public AsyncPlayerVpStatsDto Custom { get; set; }
}
