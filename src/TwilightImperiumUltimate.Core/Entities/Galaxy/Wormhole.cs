using TwilightImperiumUltimate.Core.Enums.Galaxy;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Galaxy;

public class Wormhole : IEntity, IGameVersion
{
    public int Id { get; set; }

    public WormholeName WormholeName { get; set; }

    public SystemTileName SystemTileName { get; set; }

    public SystemTile SystemTile { get; set; } = default!;

    public GameVersion GameVersion { get; set; }
}
