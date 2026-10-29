using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Marks the caller as having left a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="Caller">The account or guest identity leaving the room.</param>
public sealed record LeaveRoomCommand(string Code, DraftRoomCaller Caller)
    : IRequest<Result>;
