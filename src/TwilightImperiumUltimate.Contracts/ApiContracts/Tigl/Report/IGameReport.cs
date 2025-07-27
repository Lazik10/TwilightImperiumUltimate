using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

public interface IGameReport
{
    string GameId { get; set; }

    int Score { get; set; }

    IReadOnlyCollection<PlayerResult> PlayerResults { get; set; }

    ResultSource Source { get; set; }

    long Timestamp { get; set; }
}
