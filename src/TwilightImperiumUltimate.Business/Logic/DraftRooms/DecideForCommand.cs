using FluentResults;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Publishes any affected private choice before applying a manager's replacement decision.</summary>
public sealed record DecideForCommand(string Code, Guid SeatId, DraftActionDto Action, DraftRoomCaller Caller) : IRequest<Result>;
