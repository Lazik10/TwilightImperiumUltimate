using TwilightImperiumUltimate.Core.Entities.Factions;
using TwilightImperiumUltimate.Core.Entities.Technologies;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Enums.Technologies;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

public sealed class FactionTechnology
{
    public FactionName FactionName { get; set; }

    public Faction Faction { get; set; } = null!;

    public TechnologyName TechnologyName { get; set; }

    public Technology Technology { get; set; } = null!;

    public bool StartingTechnology { get; set; }
}