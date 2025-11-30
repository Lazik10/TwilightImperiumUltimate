namespace TwilightImperiumUltimate.Contracts.DTOs.Rankings;

public record RankingsUserDto
{
    public int TiglUserId { get; init; }

    public string TiglUserName { get; init; } = string.Empty;

    public IReadOnlyCollection<RankingsLeagueInfoDto> Leagues { get; init; } = new List<RankingsLeagueInfoDto>();
}
