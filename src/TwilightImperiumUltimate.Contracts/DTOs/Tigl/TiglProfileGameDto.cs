using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TiglProfileGameDto
{
    public int MatchReportId { get; set; }

    public string GameId { get; set; } = string.Empty;

    public TiglLeague League { get; set; }

    public MatchState State { get; set; }

    public ResultSource Source { get; set; }

    public long StartTimestamp { get; set; }

    public long EndTimestamp { get; set; }

    public int MaxScore { get; set; }

    public bool IsWinner { get; set; }

    public TiglFactionName Faction { get; set; }

    public int Score { get; set; }

    public int Season { get; set; }
}
