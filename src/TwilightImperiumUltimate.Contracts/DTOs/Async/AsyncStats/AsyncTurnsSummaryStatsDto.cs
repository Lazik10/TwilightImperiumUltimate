namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncTurnsSummaryStatsDto
{
    public AsyncTurnsSummaryStatsDto()
    {
    }

    public AsyncTurnsSummaryStatsDto(
        AsyncTurnsStatsDto all,
        AsyncTurnsStatsDto tigl,
        AsyncTurnsStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncTurnsStatsDto All { get; set; } = new();

    public AsyncTurnsStatsDto Tigl { get; set; } = new();

    public AsyncTurnsStatsDto Custom { get; set; } = new();
}
