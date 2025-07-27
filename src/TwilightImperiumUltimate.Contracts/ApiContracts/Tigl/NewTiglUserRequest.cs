namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

public class NewTiglUserRequest
{
    public long DiscordId { get; set; }

    public string DiscordTag { get; set; } = string.Empty;

    public string TiglUserName { get; set; } = string.Empty;
}
