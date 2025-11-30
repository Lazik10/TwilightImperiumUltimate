using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Rankings;

public class RankingsAchievementDto
{
    public int TiglUserId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public AchievementName AchievementName { get; set; }

    public AchievementCategory Category { get; set; }

    public TiglFactionName Faction { get; set; }

    public long AchievedAt { get; set; }

    public int MatchId { get; set; }

    public string MatchName { get; set; } = string.Empty;
}