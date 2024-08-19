using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Factions;
using TwilightImperiumUltimate.Core.Entities.Technologies;

namespace TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

public sealed class FactionTechnology
{
    public FactionName FactionName { get; set; }

    public Faction Faction { get; set; } = default!;

    public TechnologyName TechnologyName { get; set; }

    public Technology Technology { get; set; } = default!;

    public bool StartingTechnology { get; set; }
}