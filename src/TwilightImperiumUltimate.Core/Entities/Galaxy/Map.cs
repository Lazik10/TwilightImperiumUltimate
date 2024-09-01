using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Entities.Users;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Galaxy;

public class Map : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string EventName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public MapTemplate MapTemplate { get; set; }

    public string TtsString { get; set; } = string.Empty;

    public string MapGeneratorLink { get; set; } = string.Empty;

    public string MapArchiveLink { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;

    public TwilightImperiumUser User { get; set; } = default!;

    public IReadOnlyCollection<MapRating> MapRatings { get; set; } = new List<MapRating>();
}
