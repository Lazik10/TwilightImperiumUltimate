using FluentResults;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Updates the selected engine's settings in an authorized lobby.</summary>
/// <param name="Code">The public room code.</param>
/// <param name="Settings">The polymorphic engine settings.</param>
/// <param name="Caller">The account or guest identity making the request.</param>
public sealed record UpdateDraftSettingsCommand(string Code, DraftSettingsDto Settings, DraftRoomCaller Caller) : IRequest<Result>;
