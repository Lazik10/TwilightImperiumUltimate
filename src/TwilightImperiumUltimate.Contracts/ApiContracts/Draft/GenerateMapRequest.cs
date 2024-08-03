using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Draft;

public record GenerateMapRequest(
    bool PreviewMap,
    MapTemplate MapTemplate,
    HomeSystemDraftType HomeSystemDraftType,
    IReadOnlyCollection<FactionName> Factions,
    AnomalyDensity AnomalyDensity,
    PlacementStyle PlacementStyle,
    SystemWeight SystemWeight,
    WormholeDensity WormholesDensity,
    int NumberOfLegendaries);
