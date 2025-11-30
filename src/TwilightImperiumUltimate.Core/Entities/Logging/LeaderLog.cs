using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Logging;

public class LeaderLog : IEntity, ITimestamp
{
    public int Id { get; set; }

    public long Timestamp { get; set; }

    public TiglPrestigeRank Name { get; set; }

    public TiglFactionName Faction { get; set; }

    public TiglLeague League { get; set; }

    public int? PreviousOwnerId { get; set; }

    public long PreviousOwnerDiscordId { get; set; }

    public string PreviousOwner { get; set; } = string.Empty;

    public int? NewOwnerId { get; set; }

    public long NewOwnerDiscordId { get; set; }

    public string NewOwner { get; set; } = string.Empty;

    public long Duration { get; set; }

    public int MatchId { get; set; }

    public bool Published { get; set; }
}
