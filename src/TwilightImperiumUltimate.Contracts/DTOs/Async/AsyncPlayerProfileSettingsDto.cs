namespace TwilightImperiumUltimate.Contracts.DTOs.Async;

public record AsyncPlayerProfileSettingsDto
{
    public AsyncPlayerProfileSettingsDto(
        bool excludeFromAsyncStats,
        bool showWinRates,
        bool showTurnStats,
        bool showCombatStats,
        bool showVpStats,
        bool showFactionStats,
        bool showOpponents,
        bool showGames)
    {
        ExcludeFromAsyncStats = excludeFromAsyncStats;
        ShowWinRates = showWinRates;
        ShowTurnStats = showTurnStats;
        ShowCombatStats = showCombatStats;
        ShowVpStats = showVpStats;
        ShowFactionStats = showFactionStats;
        ShowOpponents = showOpponents;
        ShowGames = showGames;
    }

    public AsyncPlayerProfileSettingsDto()
    {
        ExcludeFromAsyncStats = false;
        ShowWinRates = true;
        ShowTurnStats = true;
        ShowCombatStats = true;
        ShowVpStats = true;
        ShowFactionStats = true;
        ShowOpponents = true;
        ShowGames = true;
    }

    public bool ExcludeFromAsyncStats { get; set; }

    public bool ShowWinRates { get; set; }

    public bool ShowTurnStats { get; set; }

    public bool ShowCombatStats { get; set; }

    public bool ShowVpStats { get; set; }

    public bool ShowFactionStats { get; set; }

    public bool ShowOpponents { get; set; }

    public bool ShowGames { get; set; }
}
