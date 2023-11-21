using TwilightImperiumUltimate.Core.Entities.Units;
using TwilightImperiumUltimate.Core.Enums.Units;

namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeUnits()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var units = new List<Unit>()
        {
            new Unit()
            {
                UnitName = UnitName.SpaceDock,
                UnitType = UnitType.Structure,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.Pds,
                UnitType = UnitType.Structure,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.Infantry,
                UnitType = UnitType.GroundUnit,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.Mech,
                UnitType = UnitType.GroundUnit,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.Fighter,
                UnitType = UnitType.Ship,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.Destroyer,
                UnitType = UnitType.Ship,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.Carrier,
                UnitType = UnitType.Ship,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.Cruiser,
                UnitType = UnitType.Ship,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.Dreadnought,
                UnitType = UnitType.Ship,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.Flagship,
                UnitType = UnitType.Ship,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
            new Unit()
            {
                UnitName = UnitName.WarSun,
                UnitType = UnitType.Ship,
                Cost = -1,
                Combat = -1,
                Move = -1,
                Capacity = -1,
            },
        };

        dbContext.Units.AddRange(units);
        dbContext.SaveChanges();
    }
}
