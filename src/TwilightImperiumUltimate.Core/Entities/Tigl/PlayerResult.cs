using System.Diagnostics;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

[DebuggerDisplay("PlayerResult: Id={Id}, TiglUserId={TiglUserId}, Score={Score}, IsWinner={IsWinner}, Faction={Faction}, MatchReportId={MatchReportId}")]
public class PlayerResult : IEntity
{
    public int Id { get; set; }

    public int TiglUserId { get; set; }

    public int Score { get; set; }

    public bool IsWinner { get; set; }

    public TiglFactionName Faction { get; set; }

    public int MatchReportId { get; set; }

    public MatchReport? MatchReport { get; set; }
}
