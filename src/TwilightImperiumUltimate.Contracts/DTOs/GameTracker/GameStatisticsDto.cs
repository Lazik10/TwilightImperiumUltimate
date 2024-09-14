using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.GameTracker;

public record GameStatisticsDto(
    Guid GameId,
    int MaxPoints,
    int NumberOfPlayers,
    int GameRound,
    string Time,
    FactionName Winner);
