using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Core.Entities.Users;

namespace TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

public class SliceDraftRating
{
    public int Id { get; set; }

    public int SliceDraftId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public float Rating { get; set; }

    public SliceDraft SliceDraft { get; set; } = default!;

    public TwilightImperiumUser User { get; set; } = default!;
}
