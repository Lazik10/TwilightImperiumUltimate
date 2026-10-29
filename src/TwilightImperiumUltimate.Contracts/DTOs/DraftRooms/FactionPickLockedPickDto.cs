using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Contains a publicly locked seat pick, without identities or pending data.</summary>
public sealed record FactionPickLockedPickDto(Guid SeatId, FactionName Faction, int Round, bool DecidedByHost);
