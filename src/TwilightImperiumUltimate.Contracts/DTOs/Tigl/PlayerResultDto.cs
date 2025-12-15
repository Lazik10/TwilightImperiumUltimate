using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class PlayerResultDto
{
    public int TiglUserId { get; set; }

    public string DiscordUserName { get; set; } = string.Empty;

    public string TiglUserName { get; set; } = string.Empty;

    public int Score { get; set; }

    public TiglFactionName Faction { get; set; }

    public bool IsWinner { get; set; }
}