using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TiglPlayerRankHistoryEntryDto
{
    public string Date { get; init; } = string.Empty;

    public string League { get; init; } = string.Empty;

    public TiglRankName RankName { get; init; }

    public int Duration { get; init; }

    public string GameId { get; init; } = string.Empty;
}
