namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TiglUserLiteDto
{
    public int Id { get; set; }

    public long DiscordUserId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public string DiscordUserName { get; set; } = string.Empty;
}
