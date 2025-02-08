namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncEliminationsSummaryStatsDto
{
    public AsyncEliminationsSummaryStatsDto()
    {
    }

    public AsyncEliminationsSummaryStatsDto(
        AsyncEliminationsStatsDto all,
        AsyncEliminationsStatsDto tigl,
        AsyncEliminationsStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncEliminationsStatsDto All { get; set; } = new();

    public AsyncEliminationsStatsDto Tigl { get; set; } = new();

    public AsyncEliminationsStatsDto Custom { get; set; } = new();
}
