using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

public class GameReport : IGameReport
{
    public string GameId { get; set; } = string.Empty;

    public int Score { get; set; }

    public IReadOnlyCollection<PlayerResult> PlayerResults { get; set; } = new List<PlayerResult>();

    public ResultSource Source { get; set; }

    public long Timestamp { get; set; }
}
