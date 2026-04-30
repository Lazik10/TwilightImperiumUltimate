namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TiglPlayerProfileLinkDto
{
    public long DiscordUserId { get; init; }

    public int TiglUserId { get; init; }

    public string Url { get; init; } = string.Empty;
}
