using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class TiglRank : IEntity
{
    public int Id { get; set; }

    public TiglRankName Name { get; set; }

    public long AchievedAt { get; set; }

    public TiglLeague League { get; set; }

    public int TiglUserId { get; set; }

    public TiglUser? TiglUser { get; set; }
}
