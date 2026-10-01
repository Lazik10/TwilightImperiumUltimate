using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Faction;

public record GetFactionsByGameVersionsRequest(
    IReadOnlyCollection<GameVersion> GameVersions);
