using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class Leader : IEntity
{
    public int Id { get; set; }

    public TiglPrestigeRank Name { get; set; }

    public TiglFactionName Faction { get; set; }

    public TiglLeague League { get; set; }

    public long? FirstUpdate { get; set; }

    public long? LastUpdate { get; set; }

    public long? PreviousUpdate { get; set; }

    public long? ShortestDuration { get; set; }

    public long? LongestDuration { get; set; }

    public int ChangeCount { get; set; }

    public int? PreviousOwnerId { get; set; }

    public int? CurrentOwnerId { get; set; }

    public TiglUser? CurrentOwner { get; set; }

    public int? ShortestHolderId { get; set; }

    public TiglUser? ShortestHolder { get; set; }

    public int? LongestHolderId { get; set; }

    public TiglUser? LongestHolder { get; set; }
}
