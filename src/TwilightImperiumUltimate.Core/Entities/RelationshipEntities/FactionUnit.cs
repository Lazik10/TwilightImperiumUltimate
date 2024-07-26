using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Factions;
using TwilightImperiumUltimate.Core.Entities.Units;

namespace TwilightImperiumUltimate.Core.Entities.RelationshipEntities;

public class FactionUnit
{
    public FactionName FactionName { get; set; }

    public Faction Faction { get; set; } = default!;

    public UnitName UnitName { get; set; }

    public Unit Unit { get; set; } = default!;

    public int Count { get; set; }
}
