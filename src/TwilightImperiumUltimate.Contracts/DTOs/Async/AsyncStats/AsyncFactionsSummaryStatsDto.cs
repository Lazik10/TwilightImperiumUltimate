namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncFactionsSummaryStatsDto
{
    public AsyncFactionsSummaryStatsDto()
    {
    }

    public AsyncFactionsSummaryStatsDto(
        IReadOnlyCollection<AsyncFactionsStatsDto> all,
        IReadOnlyCollection<AsyncFactionsStatsDto> tigl,
        IReadOnlyCollection<AsyncFactionsStatsDto> custom)
    {
        All = all;
        Tigl = tigl;
        Custom = custom;
    }

    public IReadOnlyCollection<AsyncFactionsStatsDto> All { get; set; } = new List<AsyncFactionsStatsDto>();

    public IReadOnlyCollection<AsyncFactionsStatsDto> Tigl { get; set; } = new List<AsyncFactionsStatsDto>();

    public IReadOnlyCollection<AsyncFactionsStatsDto> Custom { get; set; } = new List<AsyncFactionsStatsDto>();
}
