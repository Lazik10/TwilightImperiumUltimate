namespace TwilightImperiumUltimate.Contracts.DTOs.GameTracker;

public record RoundStatisticsDto(
    Guid GameId,
    int Round,
    IReadOnlyCollection<FactionStatsDto> FactionStats);
