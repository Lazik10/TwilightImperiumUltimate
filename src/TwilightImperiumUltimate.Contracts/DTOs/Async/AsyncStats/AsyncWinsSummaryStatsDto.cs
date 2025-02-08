namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncWinsSummaryStatsDto
{
    public AsyncWinsSummaryStatsDto()
    {
    }

    public AsyncWinsSummaryStatsDto(
        AsyncWinsStatsDto all,
        AsyncWinsStatsDto tigl,
        AsyncWinsStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncWinsStatsDto All { get; set; } = new();

    public AsyncWinsStatsDto Tigl { get; set; } = new();

    public AsyncWinsStatsDto Custom { get; set; } = new();
}
