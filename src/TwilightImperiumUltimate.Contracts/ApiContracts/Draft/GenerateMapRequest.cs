using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Draft;

public record GenerateMapRequest(
    bool PreviewMap,
    MapTemplate MapTemplate,
    HomeSystemDraftType HomeSystemDraftType,
    IReadOnlyCollection<FactionName> Factions,
    IReadOnlyCollection<GameVersion> GameVersions,
    PlacementStyle PlacementStyle,
    SystemWeight SystemWeight,
    WormholeDensity WormholesDensity,
    int NumberOfLegendaries,
    bool LegendaryPriorityInEquidistant,
    IReadOnlyCollection<string> PlayerNames);
