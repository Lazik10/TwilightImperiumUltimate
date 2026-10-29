using FluentResults;
using MediatR;
using TwilightImperiumUltimate.Business.Services.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>Deletes a result only for a live-room manager or, after cleanup, its owning account.</summary>
public sealed record DeleteResultCommand(Guid Id, DraftRoomCaller Caller) : IRequest<Result>;
