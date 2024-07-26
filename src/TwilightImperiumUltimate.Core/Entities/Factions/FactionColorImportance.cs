using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Factions;

public class FactionColorImportance : IEntity
{
    public int Id { get; set; }

    public FactionName FactionName { get; set; }

    public PlayerColor Color { get; set; }

    public int Importance { get; set; }
}
