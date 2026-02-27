using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TiglPlayerProfileDto
{
    public int TiglUserId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public long DiscordId { get; set; }

    public string DiscordTag { get; set; } = string.Empty;

    public List<TiglLeagueProfileDto> LeagueProfiles { get; set; } = new();

    public List<TiglUserAchievementDto> Achievements { get; set; } = new();

    public int TotalTiglUsers { get; set; }

    public Dictionary<string, int> AchievementPlayerCounts { get; set; } = new();

    public List<RankHistoryDto> RankHistory { get; set; } = new();

    public List<PrestigeRankHistoryDto> PrestigeRankHistory { get; set; } = new();

    public List<TiglProfileGameDto> GameHistory { get; set; } = new();

    public List<TiglTopOpponentDto> TopOpponents { get; set; } = new();

    public List<TiglProfileFactionStatsDto> FactionStats { get; set; } = new();
}
