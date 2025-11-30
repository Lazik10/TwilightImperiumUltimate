using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

public class GlickoStats
{
    public int Id { get; set; }

    public int TiglUserId { get; set; }

    public TiglUser? TiglUser { get; set; }

    public int GlickoRatingId { get; set; }

    public GlickoRating? Rating { get; set; }

    public TiglLeague League { get; set; }

    public ICollection<GlickoPlayerMatchStats> MatchStats { get; } = new List<GlickoPlayerMatchStats>();
}
