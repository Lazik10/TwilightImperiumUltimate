using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Changes the password used by new participants to join a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="Password">The new room password.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
public sealed record ChangeRoomPasswordCommand(
    string Code,
    string? Password,
    DraftRoomCaller Caller)
    : IRequest<Result>;
