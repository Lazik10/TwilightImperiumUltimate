using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Removes a free seat from a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="SeatId">The identifier of the seat to remove.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
public sealed record RemoveSeatCommand(
    string Code,
    Guid SeatId,
    DraftRoomCaller Caller)
    : IRequest<Result>;
