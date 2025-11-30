using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class Achievement : IEntity
{
    public int Id { get; set; }

    public AchievementName Name { get; set; }

    public TiglFactionName Faction { get; set; }

    public AchievementCategory Category { get; set; }

    public ICollection<TiglUserAchievement>? Achievements { get; }
}
