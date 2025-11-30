using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class MatchReport : IEntity
{
    public int Id { get; set; }

    public string GameId { get; set; } = string.Empty;

    public ResultSource Source { get; set; }

    public long StartTimestamp { get; set; }

    public long EndTimestamp { get; set; }

    public int Score { get; set; }

    public int Round { get; set; }

    public int PlayerCount { get; set; }

    public int Season { get; set; }

    public TiglLeague League { get; set; }

    public long Events { get; set; }

    public MatchState State { get; set; } = MatchState.New;

    public IReadOnlyCollection<PlayerResult> PlayerResults { get; set; } = new List<PlayerResult>();

    public ICollection<AsyncPlayerMatchStats>? PlayerMatchAsyncStats { get; }

    public ICollection<GlickoPlayerMatchStats>? PlayerMatchGlickoStats { get; }

    public ICollection<TrueSkillPlayerMatchStats>? PlayerMatchTrueSkillStats { get; }
}
