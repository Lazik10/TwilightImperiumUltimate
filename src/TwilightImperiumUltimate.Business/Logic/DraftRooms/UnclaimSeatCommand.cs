using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Unclaims an occupied draft-room seat.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="SeatId">The identifier of the seat to unclaim.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
public sealed record UnclaimSeatCommand(
    string Code,
    Guid SeatId,
    DraftRoomCaller Caller)
    : IRequest<Result>;
