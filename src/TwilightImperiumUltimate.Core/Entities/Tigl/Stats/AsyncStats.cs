using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

public class AsyncStats : IEntity
{
    public int Id { get; set; }

    public int TiglUserId { get; set; }

    public TiglUser? TiglUser { get; set; }

    public int AsyncRatingId { get; set; }

    public AsyncRating? Rating { get; set; }

    public TiglLeague League { get; set; }

    public ICollection<AsyncPlayerMatchStats> MatchStats { get; } = new List<AsyncPlayerMatchStats>();
}
