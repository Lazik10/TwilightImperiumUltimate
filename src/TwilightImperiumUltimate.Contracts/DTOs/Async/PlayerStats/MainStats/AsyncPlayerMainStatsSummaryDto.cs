namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.MainStats;

public record AsyncPlayerMainStatsSummaryDto
{
    public AsyncPlayerMainStatsSummaryDto()
    {
        All = new();
        Tigl = new();
        Custom = new();
    }

    public AsyncPlayerMainStatsSummaryDto(
        AsyncPlayerMainStatsDto all,
        AsyncPlayerMainStatsDto tigl,
        AsyncPlayerMainStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncPlayerMainStatsDto All { get; init; }

    public AsyncPlayerMainStatsDto Tigl { get; init; }

    public AsyncPlayerMainStatsDto Custom { get; init; }
}
