using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Draft;

public record ColorDraftRequest(IReadOnlyCollection<FactionName> Factions, IReadOnlyCollection<PlayerColor> Colors);
