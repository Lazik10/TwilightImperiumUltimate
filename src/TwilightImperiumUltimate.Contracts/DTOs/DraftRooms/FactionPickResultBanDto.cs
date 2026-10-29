using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Describes a revealed ban and its public seat or manager attribution.</summary>
public sealed record FactionPickResultBanDto(int? SeatIndex, FactionName Faction, string? DecidedByDisplayName);
