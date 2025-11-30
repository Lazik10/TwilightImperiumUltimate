using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

public class TiglUserAchievement
{
    public int TiglUserId { get; set; }

    public TiglUser TiglUser { get; set; } = default!;

    public int AchievementId { get; set; }

    public AchievementName AchievementName { get; set; }

    public Achievement Achievement { get; set; } = default!;

    public TiglFactionName Faction { get; set; }

    public int Rank { get; set; }

    public long AchievedAt { get; set; }

    public int MatchId { get; set; }

    public string MatchName { get; set; } = string.Empty;
}
