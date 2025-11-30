using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.Ranks;

public class PrestigeRank : IEntity
{
    public int Id { get; set; }

    public TiglPrestigeRank Name { get; set; }

    public TiglFactionName FactionName { get; set; }

    public TiglLeague League { get; set; }

    public ICollection<TiglUserPrestigeRank>? PrestigeRanks { get; }
}
