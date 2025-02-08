namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncGamesSummaryStatsDto
{
    public AsyncGamesSummaryStatsDto()
    {
    }

    public AsyncGamesSummaryStatsDto(
        AsyncGamesStatsDto all,
        AsyncGamesStatsDto tigl,
        AsyncGamesStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncGamesStatsDto All { get; set; } = new();

    public AsyncGamesStatsDto Tigl { get; set; } = new();

    public AsyncGamesStatsDto Custom { get; set; } = new();
}
