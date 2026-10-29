using FluentResults;
using MediatR;
using TwilightImperiumUltimate.Contracts.DTOs.DraftRooms;

namespace TwilightImperiumUltimate.Business.Logic.DraftRooms;

/// <summary>
/// Gets the active draft-room memberships for an account.
/// </summary>
public sealed record GetMyRoomsQuery(string UserId) : IRequest<Result<IReadOnlyList<MyDraftRoomDto>>>;
