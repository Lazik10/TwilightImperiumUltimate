using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

public class RatingDecay : IEntity
{
    public int Id { get; set; }

    public double Amount { get; set; }

    public int TiglUserId { get; set; }

    public TiglUser? TiglUser { get; set; }

    public long Timestamp { get; set; }

    public TiglLeague League { get; set; }

    public int Season { get; set; }

    public RankingSystem RatingSystem { get; set; }
}
