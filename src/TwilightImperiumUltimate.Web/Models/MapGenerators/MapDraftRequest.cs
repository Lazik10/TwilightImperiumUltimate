using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Models.MapGenerators;

public class MapDraftRequest
{
    public IReadOnlyCollection<FactionName> Factions { get; set; } = new List<FactionName>();

    public MapTemplate MapTemplate { get; set; }

    public PlacementStyle PlacementStyle { get; set; }

    public SystemWeight SystemWeight { get; set; }

    public bool PreviewMap { get; set; }
}