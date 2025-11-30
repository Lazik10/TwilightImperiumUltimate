using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Rankings;

public record RankingsLeagueInfoDto
{
    public TiglLeague League { get; init; }

    public TiglPrestigeRank? HighestPrestigeRank { get; init; }

    public int HighestPrestigeRankLevel { get; init; }

    public int FactionPrestigeRankCount { get; init; }

    public TiglRankName CurrentRank { get; init; }

    public long LastAchievedAt { get; init; }

    public int GamesPlayed { get; init; }
}
