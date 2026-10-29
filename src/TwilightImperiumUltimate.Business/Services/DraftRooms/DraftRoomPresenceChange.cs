namespace TwilightImperiumUltimate.Business.Services.DraftRooms;

/// <summary>
/// Describes a participant presence transition in a draft room.
/// </summary>
/// <param name="RoomId">The room whose participant presence changed.</param>
/// <param name="ParticipantId">The participant whose presence changed.</param>
public sealed record DraftRoomPresenceChange(Guid RoomId, Guid ParticipantId);
