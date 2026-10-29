using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Removes the last locked pick while a draft is running, without undoing revealed bans.</summary>
public sealed record UndoLastPickCommand(string Code, DraftRoomCaller Caller) : IRequest<Result>;
