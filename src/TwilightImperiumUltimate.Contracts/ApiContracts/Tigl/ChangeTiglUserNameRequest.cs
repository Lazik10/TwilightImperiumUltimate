namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl;

public class ChangeTiglUserNameRequest
{
    public long DiscordId { get; set; }

    public string NewTiglUserName { get; set; } = string.Empty;
}
