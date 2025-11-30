using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Rankings;

public record PrestigeRankHistoryDto
{
    public TiglLeague League { get; init; }

    public TiglPrestigeRank PrestigeRank { get; init; }

    public TiglFactionName Faction { get; init; }

    public int Level { get; init; }

    public long AchievedAt { get; init; }
}
