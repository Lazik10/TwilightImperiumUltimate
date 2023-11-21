using TwilightImperiumUltimate.Core.Entities.Factions;
using TwilightImperiumUltimate.Core.Entities.Units;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Enums.Units;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

public class FactionUnit
{
    public UnitName UnitName { get; set; }

    public Unit Unit { get; set; } = null!;

    public FactionName FactionName { get; set; }

    public Faction Faction { get; set; } = null!;

    public int Count { get; set; }
}
