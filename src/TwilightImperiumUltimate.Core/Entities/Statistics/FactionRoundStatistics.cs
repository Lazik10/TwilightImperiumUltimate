using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Statistics;

public class FactionRoundStatistics : IEntity
{
    public int Id { get; set; }

    public Guid GameId { get; set; }

    public int Round { get; set; }

    public FactionName FactionName { get; set; }

    public int Score { get; set; }

    public StrategyCardName StrategyCardName { get; set; }
}
