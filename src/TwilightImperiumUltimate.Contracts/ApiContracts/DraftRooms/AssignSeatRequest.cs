namespace TwilightImperiumUltimate.Contracts.ApiContracts.DraftRooms;

/// <summary>Identifies the active unseated participant a manager wants to assign.</summary>
/// <param name="ParticipantId">The room participant identifier.</param>
public sealed record AssignSeatRequest(Guid ParticipantId);
