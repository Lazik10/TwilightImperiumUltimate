using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class PlayerResult : IEntity
{
    public int Id { get; set; }

    public int TiglUser { get; set; }

    public int Score { get; set; }

    public TiglFactionName Faction { get; set; }

    public int MatchReportId { get; set; }

    public MatchReport? MatchReport { get; set; }
}
