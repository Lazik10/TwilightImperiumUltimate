namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TiglPlayerRankHistoryDto
{
    public long DiscordUserId { get; init; }

    public TiglPlayerCurrentRanksDto CurrentRanks { get; init; } = new();

    public IReadOnlyCollection<TiglPlayerRankHistoryEntryDto> Ranks { get; init; } = new List<TiglPlayerRankHistoryEntryDto>();
}
