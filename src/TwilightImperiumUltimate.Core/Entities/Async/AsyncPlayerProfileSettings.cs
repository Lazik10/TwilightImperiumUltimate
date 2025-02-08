using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Async;

public class AsyncPlayerProfileSettings : IEntity
{
    public int Id { get; set; }

    public bool ExcludeFromAsyncStats { get; set; }

    public bool ShowWinRate { get; set; } = true;

    public bool ShowTurnStats { get; set; } = true;

    public bool ShowCombatStats { get; set; } = true;

    public bool ShowVpStats { get; set; } = true;

    public bool ShowFactionStats { get; set; } = true;

    public bool ShowOpponents { get; set; } = true;

    public bool ShowGames { get; set; } = true;

    public int AsyncPlayerProfileId { get; set; }

    public AsyncPlayerProfile? AsyncPlayerProfile { get; set; }
}
