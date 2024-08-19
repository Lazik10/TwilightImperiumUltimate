namespace TwilightImperiumUltimate.Web.Models.Galaxy;

public class WormholeModel
{
    public int Id { get; set; }

    public WormholeName WormholeName { get; set; }

    public SystemTileName SystemTileName { get; set; }

    public GameVersion GameVersion { get; set; }
}
