namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class UnitsData
{
    internal static List<Unit> Units => new List<Unit>
    {
        new() { Id = 1, UnitName = UnitName.SpaceDock, UnitType = UnitType.Structure, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 2, UnitName = UnitName.Pds, UnitType = UnitType.Structure, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 3, UnitName = UnitName.Infantry, UnitType = UnitType.GroundUnit, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 4, UnitName = UnitName.Mech, UnitType = UnitType.GroundUnit, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 5, UnitName = UnitName.Fighter, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 6, UnitName = UnitName.Destroyer, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 7, UnitName = UnitName.Carrier, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 8, UnitName = UnitName.Cruiser, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 9, UnitName = UnitName.Dreadnought, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 10, UnitName = UnitName.Flagship, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
        new() { Id = 11, UnitName = UnitName.WarSun, UnitType = UnitType.Ship, Cost = -1, Combat = -1, Move = -1, Capacity = -1 },
    };
}
