namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncVpSummaryStatsDto
{
    public AsyncVpSummaryStatsDto()
    {
    }

    public AsyncVpSummaryStatsDto(
        AsyncVpStatsDto all,
        AsyncVpStatsDto tigl,
        AsyncVpStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncVpStatsDto All { get; set; } = new();

    public AsyncVpStatsDto Tigl { get; set; } = new();

    public AsyncVpStatsDto Custom { get; set; } = new();
}
