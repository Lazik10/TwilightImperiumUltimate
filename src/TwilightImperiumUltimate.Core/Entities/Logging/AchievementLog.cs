using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Logging;

public class AchievementLog : IEntity
{
    public int Id { get; set; }

    public long Timestamp { get; set; }

    public AchievementName AchievementName { get; set; }

    public TiglFactionName Faction { get; set; }

    public int TiglUserId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public long TiglUserDiscordId { get; set; }

    public int MatchId { get; set; }

    public bool Published { get; set; }
}
