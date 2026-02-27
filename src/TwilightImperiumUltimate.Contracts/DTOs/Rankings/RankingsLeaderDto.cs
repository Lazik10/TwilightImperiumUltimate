using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Rankings;

public record RankingsLeaderDto
{
    public int TiglUserId { get; init; }

    public TiglPrestigeRank Name { get; init; }

    public TiglFactionName Faction { get; init; }

    public string UserName { get; init; } = string.Empty;

    public long? LastUpdate { get; init; }

    public int ChangeCount { get; init; }

    public TiglLeague League { get; init; }

    public long? ShortestDuration { get; init; }

    public long? LongestDuration { get; init; }

    public string ShortestHolderName { get; init; } = string.Empty;

    public string LongestHolderName { get; init; } = string.Empty;
}
