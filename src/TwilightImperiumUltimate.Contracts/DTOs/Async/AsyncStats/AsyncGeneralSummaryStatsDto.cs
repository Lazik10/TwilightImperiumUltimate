namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncGeneralSummaryStatsDto
{
    public AsyncGeneralSummaryStatsDto()
    {
    }

    public AsyncGeneralSummaryStatsDto(
        AsyncGeneralStatsDto all,
        AsyncGeneralStatsDto tigl,
        AsyncGeneralStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncGeneralStatsDto All { get; set; } = new();

    public AsyncGeneralStatsDto Tigl { get; set; } = new();

    public AsyncGeneralStatsDto Custom { get; set; } = new();
}
