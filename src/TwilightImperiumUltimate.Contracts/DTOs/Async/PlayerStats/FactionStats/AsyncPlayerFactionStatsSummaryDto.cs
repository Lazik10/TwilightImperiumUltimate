namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;

public record AsyncPlayerFactionStatsSummaryDto
{
    public AsyncPlayerFactionStatsSummaryDto()
    {
    }

    public AsyncPlayerFactionStatsSummaryDto(
        IReadOnlyCollection<AsyncPlayerFactionStatsDto> overall,
        IReadOnlyCollection<AsyncPlayerFactionStatsDto> tigl,
        IReadOnlyCollection<AsyncPlayerFactionStatsDto> custom)
    {
        All = overall;
        Tigl = tigl;
        Custom = custom;
    }

    public IReadOnlyCollection<AsyncPlayerFactionStatsDto> All { get; init; } = new List<AsyncPlayerFactionStatsDto>();

    public IReadOnlyCollection<AsyncPlayerFactionStatsDto> Tigl { get; init; } = new List<AsyncPlayerFactionStatsDto>();

    public IReadOnlyCollection<AsyncPlayerFactionStatsDto> Custom { get; init; } = new List<AsyncPlayerFactionStatsDto>();
}
