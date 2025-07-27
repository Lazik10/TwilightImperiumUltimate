using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class TiglParameter : IEntity
{
    public int Id { get; set; }

    public TiglParameterName Name { get; set; }

    public bool Enabled { get; set; }
}
