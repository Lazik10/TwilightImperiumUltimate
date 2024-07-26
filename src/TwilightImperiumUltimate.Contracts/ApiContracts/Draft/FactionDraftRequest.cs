using TwilightImperiumUltimate.Contracts.DTOs.Draft;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.Draft;

public record FactionDraftRequest(int NumberOfPlayers, int NumberOfFactionsPerPlayer, IReadOnlyCollection<DraftFactionDto> Factions);
