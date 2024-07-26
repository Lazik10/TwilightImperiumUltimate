using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.RelationshipEntities;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Units;

public class Unit : IEntity
{
    public int Id { get; set; }

    public UnitName UnitName { get; set; }

    public UnitType UnitType { get; set; }

    public int Cost { get; set; }

    public int Combat { get; set; }

    public int Move { get; set; }

    public int Capacity { get; set; }

    public IReadOnlyCollection<FactionUnit> FactionUnits { get; set; } = new List<FactionUnit>();
}
