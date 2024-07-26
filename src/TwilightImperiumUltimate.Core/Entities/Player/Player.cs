using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Player;

public class Player : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public PlayerColor Color { get; set; }
}