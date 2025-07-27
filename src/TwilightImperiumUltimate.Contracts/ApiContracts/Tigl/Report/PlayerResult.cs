namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

public class PlayerResult : IPlayerResult
{
    public int Score { get; set; }

    public string Faction { get; set; } = string.Empty;

    public long DiscordId { get; set; }

    public string DiscordTag { get; set; } = string.Empty;
}
