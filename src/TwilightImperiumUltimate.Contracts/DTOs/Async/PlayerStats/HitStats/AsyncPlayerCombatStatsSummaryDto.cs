namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.HitStats;

public record AsyncPlayerCombatStatsSummaryDto
{
    public AsyncPlayerCombatStatsSummaryDto()
    {
    }

    public AsyncPlayerCombatStatsSummaryDto(
        AsyncPlayerCombatStatsDto overall,
        AsyncPlayerCombatStatsDto tigl,
        AsyncPlayerCombatStatsDto custom)
    {
        Overall = overall;
        Tigl = tigl;
        Custom = custom;
    }

    public AsyncPlayerCombatStatsDto Overall { get; init; } = new AsyncPlayerCombatStatsDto();

    public AsyncPlayerCombatStatsDto Tigl { get; init; } = new AsyncPlayerCombatStatsDto();

    public AsyncPlayerCombatStatsDto Custom { get; init; } = new AsyncPlayerCombatStatsDto();
}

