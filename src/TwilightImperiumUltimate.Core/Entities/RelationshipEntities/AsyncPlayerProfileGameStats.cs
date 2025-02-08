using TwilightImperiumUltimate.Core.Entities.Async;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

public class AsyncPlayerProfileGameStats : IEntity
{
    public int Id { get; set; }

    public int AsyncPlayerProfileId { get; set; }

    public AsyncPlayerProfile AsyncPlayerProfile { get; set; } = null!;

    public int GameStatsId { get; set; }

    public GameStats GameStats { get; set; } = null!;
}
