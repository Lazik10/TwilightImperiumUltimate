using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Logging;

public class GamePublishLog : IEntity
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public long CreatedAt { get; set; }

    public bool RankUpPublish { get; set; }

    public bool PrestigePublish { get; set; }

    public bool LeaderPublish { get; set; }

    public bool AchievementPublish { get; set; }

    public bool AchievementsEvaluated { get; set; }

    public bool PublishingInProgress { get; set; }

    public bool Published { get; set; }
}
