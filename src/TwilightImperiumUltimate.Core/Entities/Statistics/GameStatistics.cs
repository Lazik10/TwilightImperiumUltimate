using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Statistics;

public class GameStatistics : IEntity
{
    public int Id { get; set; }

    public Guid GameId { get; set; }

    public int MaxPoints { get; set; }

    public int NumberOfPlayers { get; set; }

    public int Round { get; set; }

    public string Time { get; set; } = string.Empty;

    public FactionName Winner { get; set; }
}
