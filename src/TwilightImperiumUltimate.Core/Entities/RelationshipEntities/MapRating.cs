using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Core.Entities.Users;

namespace TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

public class MapRating
{
    public int Id { get; set; }

    public int MapId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public float Rating { get; set; }

    public Map Map { get; set; } = default!;

    public TwilightImperiumUser User { get; set; } = default!;
}
