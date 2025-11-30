using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

public interface IGameReport
{
    string GameId { get; set; }

    int Score { get; set; }

    int Round { get; set; }

    int PlayerCount { get; set; }

    IReadOnlyCollection<PlayerResult> PlayerResults { get; set; }

    ResultSource Source { get; set; }

    long StartTimestamp { get; set; }

    long EndTimestamp { get; set; }

    TiglLeague League { get; set; }

    List<string> Events { get; set; }
}
