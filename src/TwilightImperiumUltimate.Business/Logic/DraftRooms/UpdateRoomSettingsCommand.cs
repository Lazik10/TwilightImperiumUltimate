using FluentResults;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Updates the name and join-lock state of a draft room.
/// </summary>
/// <param name="Code">The public room code.</param>
/// <param name="Name">The new room name.</param>
/// <param name="IsLocked">Whether new participants are prevented from joining.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
public sealed record UpdateRoomSettingsCommand(
    string Code,
    string? Name,
    bool IsLocked,
    DraftRoomCaller Caller,
    DraftTypeId? DraftType = null)
    : IRequest<Result>;
