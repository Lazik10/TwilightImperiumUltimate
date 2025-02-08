namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncCombatSummaryStatsDto
{
    public AsyncCombatSummaryStatsDto()
    {
    }

    public AsyncCombatSummaryStatsDto(
        AsyncCombatStatsDto all,
        AsyncCombatStatsDto tigl,
        AsyncCombatStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncCombatStatsDto All { get; set; } = new();

    public AsyncCombatStatsDto Tigl { get; set; } = new();

    public AsyncCombatStatsDto Custom { get; set; } = new();
}
