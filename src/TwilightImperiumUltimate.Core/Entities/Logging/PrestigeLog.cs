using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Logging;

public class PrestigeLog : IEntity, ITimestamp
{
    public int Id { get; set; }

    public long Timestamp { get; set; }

    public TiglPrestigeRank Name { get; set; }

    public TiglFactionName FactionName { get; set; }

    public TiglLeague League { get; set; }

    public int Rank { get; set; }

    public int? TiglUserId { get; set; }

    public long TiglUserDiscordId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public int MatchId { get; set; }

    public bool Published { get; set; }
}
