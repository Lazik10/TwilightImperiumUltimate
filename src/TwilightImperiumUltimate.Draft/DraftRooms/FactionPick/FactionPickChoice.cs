using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Draft.DraftRooms.FactionPick;

/// <summary>A server-only immutable choice, with no transport or database dependencies.</summary>
public sealed record FactionPickChoice(
    Guid? SeatId,
    FactionPickSelectionKind Kind,
    FactionName Faction,
    FactionPickSelectionState State,
    int Round = 0,
    Guid? DecidedByParticipantId = null);
