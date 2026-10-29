using FluentResults;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Links a verified guest participant to the authenticated account making the request.
/// </summary>
/// <param name="Code">The room code.</param>
/// <param name="Caller">The authenticated account and guest identity proofs.</param>
public sealed record LinkAccountCommand(
    string Code,
    DraftRoomCaller Caller)
    : IRequest<Result>;
