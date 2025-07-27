namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

public interface IPlayerResult
{
    int Score { get; set; }

    string Faction { get; set; }

    long DiscordId { get; set; }
}
