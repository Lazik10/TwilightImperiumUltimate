namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncOpponentsSummaryStatsDto
{
    public AsyncOpponentsSummaryStatsDto()
    {
    }

    public AsyncOpponentsSummaryStatsDto(
        AsyncOpponentsStatsDto all,
        AsyncOpponentsStatsDto tigl,
        AsyncOpponentsStatsDto custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncOpponentsStatsDto All { get; set; } = new();

    public AsyncOpponentsStatsDto Tigl { get; set; } = new();

    public AsyncOpponentsStatsDto Custom { get; set; } = new();
}
