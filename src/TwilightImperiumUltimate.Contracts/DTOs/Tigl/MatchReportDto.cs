using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class MatchReportDto
{
    public int Id { get; set; }

    public string GameId { get; set; } = string.Empty;

    public ResultSource Source { get; set; }

    public long StartTimestamp { get; set; }

    public long EndTimestamp { get; set; }

    public MatchState State { get; set; }

    public TiglLeague League { get; set; }

    public int Season { get; set; }

    public IReadOnlyCollection<string> GalacticEvents { get; set; } = new List<string>();

    public IReadOnlyCollection<PlayerResultDto> PlayerResults { get; set; } = new List<PlayerResultDto>();

    public IReadOnlyCollection<AsyncPlayerMatchStatsDto> PlayerMatchAsyncStats { get; set; } = new List<AsyncPlayerMatchStatsDto>();

    public IReadOnlyCollection<GlickoPlayerMatchStatsDto> PlayerMatchGlickoStats { get; set; } = new List<GlickoPlayerMatchStatsDto>();

    public IReadOnlyCollection<TrueSkillPlayerMatchStatsDto> PlayerMatchTrueSkillStats { get; set; } = new List<TrueSkillPlayerMatchStatsDto>();
}