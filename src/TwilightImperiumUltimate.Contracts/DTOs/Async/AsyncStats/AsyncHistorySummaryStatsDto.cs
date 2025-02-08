namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncHistorySummaryStatsDto
{
    public AsyncHistorySummaryStatsDto()
    {
    }

    public AsyncHistorySummaryStatsDto(
        AsyncHistoryStatsDto all,
        AsyncHistoryStatsDto tigl,
        AsyncHistoryStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncHistoryStatsDto All { get; set; } = new();

    public AsyncHistoryStatsDto Tigl { get; set; } = new();

    public AsyncHistoryStatsDto Custom { get; set; } = new();
}
