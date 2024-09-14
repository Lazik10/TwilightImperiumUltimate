using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.GameTracker;

public record FactionStatsDto(
    FactionName FactionName,
    int Score,
    StrategyCardName StrategyCardName);
