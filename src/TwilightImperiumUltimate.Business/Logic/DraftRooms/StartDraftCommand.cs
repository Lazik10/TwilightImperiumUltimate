using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Starts a fully occupied lobby using its configured draft engine.</summary>
/// <param name="Code">The public room code.</param>
/// <param name="Caller">The account or guest identity requesting Start.</param>
public sealed record StartDraftCommand(string Code, DraftRoomCaller Caller) : IRequest<Result>;
