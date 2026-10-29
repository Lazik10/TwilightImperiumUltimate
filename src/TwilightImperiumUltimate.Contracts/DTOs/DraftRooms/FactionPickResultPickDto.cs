using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

/// <summary>Describes a public locked pick without runtime identity identifiers.</summary>
public sealed record FactionPickResultPickDto(FactionName Faction, int Round, string? DecidedByDisplayName);
