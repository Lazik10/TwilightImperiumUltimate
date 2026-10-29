using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Claims a free draft-room seat for the caller.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="SeatId">The identifier of the seat to claim.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
public sealed record ClaimSeatCommand(
    string Code,
    Guid SeatId,
    DraftRoomCaller Caller)
    : IRequest<Result>;
