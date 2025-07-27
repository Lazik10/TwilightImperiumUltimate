using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

public class TrueSkillStats : IEntity
{
    public int Id { get; set; }

    public int TiglUserId { get; set; }

    public TiglUser? TiglUser { get; set; }

    public int TrueSkillRatingId { get; set; }

    public TrueSkillRating? TrueSkillRating { get; set; }

    public TiglLeague League { get; set; }

    public TiglAsyncRank Rank { get; set; }

    public ICollection<TrueSkillPlayerMatchStats> MatchStats { get; } = new List<TrueSkillPlayerMatchStats>();
}
