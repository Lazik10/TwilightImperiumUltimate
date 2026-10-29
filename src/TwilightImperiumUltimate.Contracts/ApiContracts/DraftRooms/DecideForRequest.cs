using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;

/// <summary>Identifies the seat and explicit replacement decision. Identity is never accepted in this body.</summary>
public sealed record DecideForRequest(Guid SeatId, DraftActionDto Action);
