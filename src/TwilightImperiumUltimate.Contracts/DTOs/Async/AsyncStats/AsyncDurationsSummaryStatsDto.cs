namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public class AsyncDurationsSummaryStatsDto
{
    public AsyncDurationsSummaryStatsDto()
    {
    }

    public AsyncDurationsSummaryStatsDto(
        AsyncDurationsStatsDto all,
        AsyncDurationsStatsDto tigl,
        AsyncDurationsStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncDurationsStatsDto All { get; set; } = new();

    public AsyncDurationsStatsDto Tigl { get; set; } = new();

    public AsyncDurationsStatsDto Custom { get; set; } = new();
}
