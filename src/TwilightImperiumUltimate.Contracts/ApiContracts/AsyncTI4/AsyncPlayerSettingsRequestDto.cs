namespace TwilightImperiumUltimate.Contracts.ApiContracts.AsyncTI4;

public record AsyncPlayerSettingsRequestDto(
    long PlayerDiscordId,
    bool ExcludeFromAsyncStats,
    bool ShowWinRates,
    bool ShowTurnStats,
    bool ShowCombatStats,
    bool ShowVpStats,
    bool ShowFactionStats,
    bool ShowOpponents,
    bool ShowGames);
