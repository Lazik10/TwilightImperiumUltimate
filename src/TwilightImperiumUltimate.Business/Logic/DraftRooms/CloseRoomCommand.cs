using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Permanently closes and deletes a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
public sealed record CloseRoomCommand(string Code, DraftRoomCaller Caller)
    : IRequest<Result>;
