namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Contains the immutable public name and picks of a numbered seat.</summary>
public sealed record FactionPickResultSeatDto(int SeatIndex, string DisplayName, IReadOnlyList<FactionPickResultPickDto> Picks);
