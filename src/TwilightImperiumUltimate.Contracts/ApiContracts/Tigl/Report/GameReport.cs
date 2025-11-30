using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

public class GameReport : IGameReport
{
    public string GameId { get; set; } = string.Empty;

    public int Score { get; set; }

    public int Round { get; set; }

    public int PlayerCount { get; set; }

    public IReadOnlyCollection<PlayerResult> PlayerResults { get; set; } = new List<PlayerResult>();

    public ResultSource Source { get; set; }

    public long StartTimestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    public long EndTimestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    public TiglLeague League { get; set; }

    public List<string> Events { get; set; } = new List<string>();
}
