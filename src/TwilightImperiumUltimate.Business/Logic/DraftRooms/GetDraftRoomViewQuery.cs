using FluentResults;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Requests the personalized view of a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="Caller">The account or guest identity requesting the view.</param>
public sealed record GetDraftRoomViewQuery(string Code, DraftRoomCaller Caller)
    : IRequest<Result<DraftRoomViewDto>>;
