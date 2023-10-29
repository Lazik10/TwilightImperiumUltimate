using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Models.MapGenerators;

public class MapDraftRequest
{
    public int NumberOfPlayers { get; set; }

    public IReadOnlyCollection<FactionName> Factions { get; set; } = new List<FactionName>();

    public MapTemplate MapTemplate { get; set; }

    public PlacementStyle PlacementStyle { get; set; }

    public SystemWeight SystemWeight { get; set; }
}