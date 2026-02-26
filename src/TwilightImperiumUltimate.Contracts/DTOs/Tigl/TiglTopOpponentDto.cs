using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TiglTopOpponentDto
{
    public int TiglUserId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public TiglLeague League { get; set; }

    public int GamesPlayed { get; set; }
}
