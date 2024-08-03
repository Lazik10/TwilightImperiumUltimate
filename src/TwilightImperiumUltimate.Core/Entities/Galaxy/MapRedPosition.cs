using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Galaxy;

public class MapRedPosition : IEntity
{
    public int Id { get; set; }

    public MapTemplate MapTemplate { get; set; }

    public int Count { get; set; }

    public string Positions { get; set; } = string.Empty;
}
