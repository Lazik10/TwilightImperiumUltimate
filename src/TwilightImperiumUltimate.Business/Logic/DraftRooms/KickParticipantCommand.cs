using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Kicks a participant from a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="ParticipantId">The identifier of the participant to kick.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
public sealed record KickParticipantCommand(
    string Code,
    Guid ParticipantId,
    DraftRoomCaller Caller)
    : IRequest<Result>;
