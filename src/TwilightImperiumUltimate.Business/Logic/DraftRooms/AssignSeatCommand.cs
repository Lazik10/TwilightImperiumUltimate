using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Assigns an active, eligible unseated participant to a vacancy.</summary>
/// <param name="Code">The room code.</param>
/// <param name="SeatId">The vacancy.</param>
/// <param name="ParticipantId">The new occupant.</param>
/// <param name="Caller">The manager's identity.</param>
public sealed record AssignSeatCommand(string Code, Guid SeatId, Guid ParticipantId, DraftRoomCaller Caller) : IRequest<Result>;
