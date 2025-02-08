namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.TurnStats;

public record AsyncPlayerTurnStatsSummaryDto
{
    public AsyncPlayerTurnStatsSummaryDto()
    {
        All = new();
        Tigl = new();
        Custom = new();
    }

    public AsyncPlayerTurnStatsSummaryDto(
        AsyncPlayerTurnStatsDto all,
        AsyncPlayerTurnStatsDto tigl,
        AsyncPlayerTurnStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncPlayerTurnStatsDto All { get; init; }

    public AsyncPlayerTurnStatsDto Tigl { get; init; }

    public AsyncPlayerTurnStatsDto Custom { get; init; }
}
