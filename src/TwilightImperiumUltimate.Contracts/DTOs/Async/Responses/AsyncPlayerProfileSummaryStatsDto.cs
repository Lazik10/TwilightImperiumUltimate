using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.GamesStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.HitStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.MainStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.Opponents;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.PlayerInfo;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.TurnStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;

namespace TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

public record AsyncPlayerProfileSummaryStatsDto(
    AsyncPlayerInfoDto PlayerInfo,
    AsyncPlayerProfileSettingsDto Settings,
    AsyncPlayerMainStatsSummaryDto GameStats,
    AsyncPlayerTurnStatsSummaryDto TurnStats,
    AsyncPlayerCombatStatsSummaryDto CombatStats,
    AsyncPlayerFactionStatsSummaryDto FactionStats,
    AsyncPlayerVpStatsSummaryDto VpStats,
    AsyncPlayerOpponentsSummaryDto Opponents,
    AsyncPlayerGamesSummaryDto Games);
