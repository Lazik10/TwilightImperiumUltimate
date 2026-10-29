using FluentResults;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Changes a participant's role in a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="ParticipantId">The identifier of the participant whose role changes.</param>
/// <param name="Role">The new role for the participant.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
public sealed record SetParticipantRoleCommand(
    string Code,
    Guid ParticipantId,
    DraftRoomRole Role,
    DraftRoomCaller Caller)
    : IRequest<Result>;
