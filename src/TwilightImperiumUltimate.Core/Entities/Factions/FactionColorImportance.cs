using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Factions;

public class FactionColorImportance : IEntity
{
    public int Id { get; set; }

    public FactionName FactionName { get; set; }

    public PlayerColor Color { get; set; }

    public int Importance { get; set; }
}
