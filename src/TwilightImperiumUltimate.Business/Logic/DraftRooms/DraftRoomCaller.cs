namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Describes the account or guest identity making a draft-room request.
/// </summary>
/// <param name="UserId">The account identifier, or <see langword="null"/> for a guest.</param>
/// <param name="UserName">The account username, or <see langword="null"/> for a guest.</param>
/// <param name="GuestParticipantId">The guest participant identifier, when already known.</param>
/// <param name="GuestSecret">The guest secret, when already known.</param>
public sealed record DraftRoomCaller(
    string? UserId,
    string? UserName,
    Guid? GuestParticipantId,
    string? GuestSecret);
