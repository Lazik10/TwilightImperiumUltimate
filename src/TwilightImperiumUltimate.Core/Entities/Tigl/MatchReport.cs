using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class MatchReport : IEntity
{
    public int Id { get; set; }

    public string GameId { get; set; } = string.Empty;

    public ResultSource Source { get; set; }

    public long Timestamp { get; set; }

    public MatchState State { get; set; } = MatchState.New;

    public IReadOnlyCollection<PlayerResult> PlayerResults { get; set; } = new List<PlayerResult>();

    public ICollection<AsyncPlayerMatchStats>? PlayerMatchStats { get; }
}
