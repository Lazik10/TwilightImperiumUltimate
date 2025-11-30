using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Rankings;

public record RankHistoryDto
{
    public int Id { get; init; }

    public TiglLeague League { get; init; }

    public TiglRankName Rank { get; init; }

    public long AchievedAt { get; init; }
}
