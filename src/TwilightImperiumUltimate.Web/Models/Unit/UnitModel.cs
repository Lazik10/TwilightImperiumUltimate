namespace TwilightImperiumUltimate.Web.Models.Unit;

public class UnitModel
{
    public int Id { get; set; }

    public UnitName UnitName { get; set; }

    public UnitType UnitType { get; set; }

    public int Cost { get; set; }

    public int Combat { get; set; }

    public int Move { get; set; }

    public int Capacity { get; set; }

    public int Count { get; set; }
}
