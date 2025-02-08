using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Async;

public class PlayerStats : IEntity
{
    public int Id { get; set; }

    public long DiscordUserID { get; set; }

    public string DiscordUserName { get; set; } = string.Empty;

    public AsyncFactionName FactionName { get; set; }

    public int Score { get; set; }

    public string Color { get; set; } = string.Empty;

    public int TotalNumberOfTurns { get; set; }

    public long TotalTurnTime { get; set; }

    public float ExpectedHits { get; set; }

    public float ActualHits { get; set; }

    public bool Eliminated { get; set; }

    public bool Winner { get; set; }

    public int GameStatsId { get; set; }

    public GameStats GameStatistics { get; set; } = null!;
}
