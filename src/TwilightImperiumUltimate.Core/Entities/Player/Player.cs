using TwilightImperiumUltimate.Core.Entities.Galaxy;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Player;

public class Player : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public PlayerColor Color { get; set; }
}