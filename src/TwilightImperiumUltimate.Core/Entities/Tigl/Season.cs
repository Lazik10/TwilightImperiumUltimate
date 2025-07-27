using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class Season : IEntity
{
    public int Id { get; set; }

    public int SeasonNumber { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; } = DateOnly.MinValue;

    public DateOnly EndDate { get; set; } = DateOnly.MinValue;

    public bool IsActive { get; set; }
}
