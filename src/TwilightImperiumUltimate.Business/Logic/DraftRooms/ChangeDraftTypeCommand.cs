using FluentResults;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Chooses a room's draft type in the lobby.</summary>
/// <param name="Code">The public room code.</param>
/// <param name="Type">The requested engine.</param>
/// <param name="Caller">The verified account or guest identity.</param>
public sealed record ChangeDraftTypeCommand(string Code, DraftTypeId Type, DraftRoomCaller Caller) : IRequest<Result>;
