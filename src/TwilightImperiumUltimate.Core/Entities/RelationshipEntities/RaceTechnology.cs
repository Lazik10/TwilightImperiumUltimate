using TwilightImperiumUltimate.Core.Entities.Factions;
using TwilightImperiumUltimate.Core.Entities.Technologies;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Enums.Technologies;

namespace TwilightImperiumUltimate.DataAccess.RelationshipEntities;

public sealed class RaceTechnology
{
    public FactionName RaceName { get; set; }

    public Faction Race { get; set; } = new Faction();

    public TechnologyName TechnologyName { get; set; }

    public Technology Technology { get; set; } = new Technology();

    public bool StartingTechnology { get; set; }
}