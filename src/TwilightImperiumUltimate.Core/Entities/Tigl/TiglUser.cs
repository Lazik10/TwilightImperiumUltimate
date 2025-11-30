using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Tigl.Ratings;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class TiglUser : IEntity
{
    public int Id { get; set; }

    public long DiscordId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public string DiscordTag { get; set; } = string.Empty;

    public string TtsUserName { get; set; } = string.Empty;

    public string TtpgUserName { get; set; } = string.Empty;

    public string BgaUserName { get; set; } = string.Empty;

    public bool TiglUserNameChanged { get; set; }

    public DateOnly LastUserNameChange { get; set; } = DateOnly.MinValue;

    public ICollection<AsyncStats>? AsyncStats { get; set; }

    public ICollection<GlickoStats>? GlickoStats { get; set; }

    public ICollection<TrueSkillStats>? TrueSkillStats { get; set; }

    public ICollection<RatingDecay>? RatingDecays { get; }

    public ICollection<Leader>? Leaders { get; }

    public TiglRankName ProphecyOfKingsRank { get; set; }

    public TiglRankName ThundersEdgeRank { get; set; }

    public TiglRankName ShatteredRank { get; set; }

    public ICollection<TiglRank>? TiglRanks { get; set; }

    public ICollection<TiglUserPrestigeRank>? PrestigeRanks { get; }

    public ICollection<TiglUserAchievement>? Achievements { get; }
}
