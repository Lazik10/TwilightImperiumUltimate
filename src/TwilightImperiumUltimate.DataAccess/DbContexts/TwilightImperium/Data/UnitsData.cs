namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class UnitsData
{
    internal static List<Unit> Units => new List<Unit>
    {
        new() { UnitName = UnitName.SpaceDock, UnitType = UnitType.Structure, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.Pds, UnitType = UnitType.Structure, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.Infantry, UnitType = UnitType.GroundUnit, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.Mech, UnitType = UnitType.GroundUnit, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.Fighter, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.Destroyer, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.Carrier, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.Cruiser, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.Dreadnought, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.Flagship, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { UnitName = UnitName.WarSun, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
    };
}
