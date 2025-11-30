using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Logging;

public class RankUpLog : IEntity, ITimestamp
{
    public int Id { get; set; }

    public long Timestamp { get; set; }

    public TiglLeague League { get; set; }

    public TiglRankName OldRank { get; set; }

    public TiglRankName NewRank { get; set; }

    public int? TiglUserId { get; set; }

    public long TiglUserDiscordId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public long Duration { get; set; }

    public int MatchId { get; set; }

    public bool Published { get; set; }
}