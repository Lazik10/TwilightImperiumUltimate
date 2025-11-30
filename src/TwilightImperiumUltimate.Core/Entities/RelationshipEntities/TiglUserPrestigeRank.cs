using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ranks;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

public class TiglUserPrestigeRank : IEntity
{
    public int Id { get; set; }

    public int TiglUserId { get; set; }

    public TiglUser TiglUser { get; set; } = default!;

    public int PrestigeRankId { get; set; }

    public PrestigeRank PrestigeRank { get; set; } = default!;

    public int Rank { get; set; }

    public long AchievedAt { get; set; }
}
