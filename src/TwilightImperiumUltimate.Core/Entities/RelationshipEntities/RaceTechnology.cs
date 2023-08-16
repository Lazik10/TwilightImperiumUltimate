using TwilightImperiumUltimate.Core.Entities.Races;
using TwilightImperiumUltimate.Core.Entities.Technologies;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Enums.Technologies;

namespace TwilightImperiumUltimate.DataAccess.RelationshipEntities;

public sealed class RaceTechnology
{
    public RaceName RaceName { get; set; }

    public Race Race { get; set; } = new Race();

    public TechnologyName TechnologyName { get; set; }

    public Technology Technology { get; set; } = new Technology();

    public bool StartingTechnology { get; set; }
}