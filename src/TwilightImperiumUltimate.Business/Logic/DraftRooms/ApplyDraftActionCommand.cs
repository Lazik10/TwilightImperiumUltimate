using FluentResults;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Applies a typed engine action using the authenticated participant's own seat.</summary>
public sealed record ApplyDraftActionCommand(string Code, DraftActionDto Action, DraftRoomCaller Caller) : IRequest<Result>;
