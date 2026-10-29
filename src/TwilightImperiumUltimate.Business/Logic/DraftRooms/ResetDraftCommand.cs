using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Resets a running draft to its lobby while preserving settings and room membership.</summary>
/// <param name="Code">The public room code.</param>
/// <param name="Caller">The account or guest identity requesting Reset.</param>
public sealed record ResetDraftCommand(string Code, DraftRoomCaller Caller) : IRequest<Result>;
